using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using HVN_System.Entity;
using System.IO.Ports;
using System.Threading;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraSplashScreen;
using HVN_System.View.Admin;
using HVN_System.Util;

namespace HVN_System.View.QC
{
    public partial class frmQCCheckingMaterialDetail : Form
    {
        public frmQCCheckingMaterialDetail(W_M_ReceiveLabel_Entity _item)
        {
            InitializeComponent();
            item = _item;
            txtLabelCode.Text = item.Whmr_code;
            txtPN.Text = item.M_name;
            txtQTYNG.Text = "0";
            txtQTYOK.Text = item.Quantity.ToString();
            PIC = item.Pic_qc;
            total_qty = item.Quantity.ToString();
            rm_plan_id = _item.Rm_plan_id;
            created_date = _item.Created_date;
            //-----
        }
        public frmQCCheckingMaterialDetail()
        {
            InitializeComponent();
        }
        private ADO adoClass;
        private CmCn conn;
        W_M_ReceiveLabel_Entity item;
        string PIC="",rm_plan_id;
        string total_qty = "0";
        private DateTime created_date;
        private bool isNotPrint = true;
        private void frmWHMaterialIssueToPD_Load(object sender, EventArgs e)
        {
            txtBarcode.Focus();
            string strQry = "select top 1 lot_no from W_M_ReceiveLabel where lot_no not in ('') order by time_qc_check desc";
            conn = new CmCn();
            try
            {
                dtpLotNo.Value = DateTime.Parse(conn.ExcuteString(strQry));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (isNotPrint)
            {
                isNotPrint = false;
                btnConfirm.Enabled = false;
                float qty = float.Parse(txtQTYOK.Text);
                float qty_ng = float.Parse(txtQTYNG.Text);
                conn = new CmCn();
                string strQry = "Update W_M_ReceiveLabel set lot_no=N'" + dtpLotNo.Value.ToString("yyyy-MM-dd") + "', pic_qc=N'" + PIC + "', time_qc_check=getdate(),quantity=N'" + qty + "',[qc_okng]=N'OK' \n";
                strQry += " where whmr_code=N'" + txtLabelCode.Text + "'\n";
                string Whmr_code_ng = "";
                if (qty_ng > 0)
                {
                    Whmr_code_ng = "WHMR" + Generate_Label_code();
                    strQry += "Insert into W_M_ReceiveLabel ([whmr_code],[m_name],[quantity],[lot_no],[created_time]," +
                        "[created_user],[place],[qc_okng],[pic_qc],[time_qc_check]) \n";
                    strQry += "select N'" + Whmr_code_ng + "',N'" + txtPN.Text + "',N'" + qty_ng + "',N'" + dtpLotNo.Value.ToString("yyyy-MM-dd") + "',getdate(),N'" + General_Infor.username + "',N'QC Area',N'NG',N'" + PIC + "',getdate() \n";
                    strQry += "Insert into QC_M_NGPart ([whmr_code],[m_name],[quantity],[lot_no],[pic_qc],[time_qc_check]," +
                       "[ng_scratch],[ng_burr],[ng_rust],[ng_electric_fail],[ng_wrong_shape]," +
                       "[ng_wrong_dimension],[ng_others],[whmr_code_origin])\n";
                    strQry += "select N'" + Whmr_code_ng + "',N'" + txtPN.Text + "',N'" + qty_ng + "',N'" + dtpLotNo.Value.ToString("yyyy-MM-dd") + "',N'" + PIC + "',getdate()";
                    strQry += ",N'" + nmScr.Value.ToString() + "',N'" + nmBur.Value.ToString() + "',\n ";
                    strQry += "N'" + nmRus.Value.ToString() + "',N'" + nmEle.Value.ToString() + "',N'" + nmSha.Value.ToString() + "',N'" + nmDim.Value.ToString() + "',N'" + nmOth.Value.ToString() + "',N'" + txtLabelCode.Text + "'\n";
                }
                strQry += "Insert into W_M_HistoryOfTransaction (whmr_code,m_name,lot_no,[transaction],quantity,input_time,PIC,plan_no,place,ng_qty)\n";
                strQry += "select N'" + txtLabelCode.Text + "',N'" + txtPN.Text + "',N'" + dtpLotNo.Value.ToString("yyyy-MM-dd");
                strQry += "',N'QC checking material',N'" + qty + "',getdate(),N'" + PIC + "',N'" + rm_plan_id + "',N'QC Area'";
                if (qty_ng > 0)
                {
                    strQry += ",N'" + qty_ng + "'\n";
                }
                else
                {
                    strQry += ",N'0'\n";
                }
                conn.ExcuteQry(strQry);
                SplashScreenManager.ShowForm(this, typeof(frmWaitingForm), true, true, false);
                SplashScreenManager.Default.SetWaitFormCaption("Printing...");
                adoClass = new ADO();
                W_M_ReceiveLabel_Entity item = new W_M_ReceiveLabel_Entity();
                item.Whmr_code = txtLabelCode.Text;
                item.M_name = txtPN.Text;
                item.Quantity = qty;
                item.Quantity_ng = qty_ng;
                item.Lot_no = dtpLotNo.Value;
                item.Created_date = created_date;
                item.Time_qc_check = DateTime.Now;
                adoClass.Print_W_M_ReceiveLabel(item, "QCOK");
                if (qty_ng > 0)
                {
                    item.Whmr_code = Whmr_code_ng;
                    adoClass.Print_W_M_ReceiveLabel(item, "QCNG");
                }
                SplashScreenManager.CloseForm();
                txtBarcode.Text = "";
                txtBarcode.Focus();
                this.Close();
            }
        }
        private int Generate_Label_code()
        {
            string Qry = "SELECT MAX(whmr_code) FROM W_M_ReceiveLabel ";
            conn = new CmCn();
            DataTable dt = conn.ExcuteDataTable(Qry);
            if (dt.Rows[0][0].ToString() != "")
            {
                string max_value = dt.Rows[0][0].ToString().Substring(4, dt.Rows[0][0].ToString().Length - 4);
                return int.Parse(max_value) + 1;
            }
            else
            {
                return 10001;
            }
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void nmScr_ValueChanged(object sender, EventArgs e)
        {
            decimal total_ng = nmBur.Value + nmDim.Value + nmEle.Value + nmOth.Value + nmRus.Value + nmScr.Value+nmSha.Value;
            txtQTYNG.Text = total_ng.ToString();
            txtQTYOK.Text = (decimal.Parse(total_qty) - total_ng).ToString();
        }

        private void btnPrintNGLabel_Click(object sender, EventArgs e)
        {

        }

        private void dtpLotNo_ValueChanged(object sender, EventArgs e)
        {
            dtpLotNo.CustomFormat = "dd/MM/yyyy";
        }

        private void txtBarcode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode==Keys.Enter)
            {
                if (txtBarcode.Text.Length<4)
                {
                    MessageBox.Show("Lỗi barcode "+txtBarcode.Text+" không tồn tại");
                    return;
                }
                string QR_Code = txtBarcode.Text.Substring(2, txtBarcode.Text.Length - 2);
                if (QR_Code.Substring(0,4)=="CFOK")
                {
                    btnConfirm.PerformClick();
                }
                else if (QR_Code.Substring(0, 4) == "DATE")
                {
                    int day = int.Parse(QR_Code.Substring(4, QR_Code.Length - 4));
                    int month = dtpLotNo.Value.Month;
                    int year = dtpLotNo.Value.Year;
                    dtpLotNo.Value = new DateTime( year, month, day);
                }
                else if (QR_Code.Substring(0, 4) == "MONT")
                {
                    int day = dtpLotNo.Value.Day;
                    int month = int.Parse(QR_Code.Substring(5, QR_Code.Length - 5));
                    int year = dtpLotNo.Value.Year;
                    dtpLotNo.Value = new DateTime(year, month, day);
                }
                else if (QR_Code.Substring(0, 4) == "YEAR")
                {
                    int day = dtpLotNo.Value.Day;
                    int month = dtpLotNo.Value.Month;
                    int year = int.Parse(QR_Code.Substring(4, QR_Code.Length - 4));
                    dtpLotNo.Value = new DateTime(year, month, day);
                }
                txtBarcode.Text = "";
                txtBarcode.Focus();
            }
        }
    }
}
