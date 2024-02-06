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
    public partial class frmQCCheckingRubberDetail : Form
    {
        public frmQCCheckingRubberDetail(W_M_ReceiveLabel_Entity _item)
        {
            InitializeComponent();
            item = _item;
            txtLabelCode.Text = item.Whmr_code;
            txtPN.Text = item.M_name;
            
            PIC = item.Pic_qc;
            total_qty = item.Quantity.ToString();
            rm_plan_id = _item.Rm_plan_id;
            created_date = _item.Created_date;
            //-----
        }
        public frmQCCheckingRubberDetail()
        {
            InitializeComponent();
        }
        //private ADO adoClass;
        private CmCn conn;
        W_M_ReceiveLabel_Entity item;
        string PIC="",rm_plan_id;
        string total_qty = "0";
        private DateTime created_date;
        //private bool isNotPrint = true;
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
