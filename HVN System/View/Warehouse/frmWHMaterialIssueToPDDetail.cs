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

namespace HVN_System.View.Warehouse
{
    public partial class frmWHMaterialIssueToPDDetail : Form
    {
        public frmWHMaterialIssueToPDDetail(W_M_IssueDocDetail_Entity select_Doc, DateTime supply_date, string _PIC)
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
            Doc_infor = select_Doc;
            DocID = Doc_infor.M_doc_id;
            txtMName.Text = Doc_infor.M_name;
            txtPLine.Text = Doc_infor.P_line;
            txtPShift.Text = Doc_infor.P_shift;
            PIC = _PIC;
            txtProdCustCode.Text = Doc_infor.Product_customer_code;
            demand = Doc_infor.M_demand;
            dtpSupplyDate.Value = supply_date;
        }
        private ADO adoClass;
        private CmCn conn;
        string DocID, PIC;
        float demand, current_qty;
        W_M_IssueLabel_Entity item;
        W_M_IssueDocDetail_Entity Doc_infor;
        //bool isChangeScale=true;
        private void frmWHCCManagement_Load(object sender, EventArgs e)
        {
            Load_Result();
            dtpSupplyDate.MinDate = DateTime.Today;
            txtBarcode.Focus();
        }

        private void Load_Result()
        {
            adoClass = new ADO();
            DataTable dt2 = new DataTable();
            if (DocID != "")
            {
                dt2 = adoClass.Load_W_M_IssueLabel("sum(quantity) as Total", "m_name=N'" + txtMName.Text + "' and product_customer_code=N'" + txtProdCustCode.Text + "' and p_line=N'" + txtPLine.Text + "' and p_shift=N'" + txtPShift.Text + "' and supply_date =N'" + dtpSupplyDate.Value.ToString("yyyy-MM-dd") + "' and m_doc_id=N'" + DocID + "'");
            }
            else
            {
                dt2 = adoClass.Load_W_M_IssueLabel("sum(quantity) as Total", "m_name=N'" + txtMName.Text + "' and product_customer_code=N'" + txtProdCustCode.Text + "' and p_line=N'" + txtPLine.Text + "' and p_shift=N'" + txtPShift.Text + "' and supply_date =N'" + dtpSupplyDate.Value.ToString("yyyy-MM-dd") + "' and m_doc_id=N''");
            }
            if (dt2.Rows[0][0].ToString() == "")
            {
                current_qty = 0;
            }
            else
            {
                current_qty = float.Parse(dt2.Rows[0][0].ToString());
            }
            txtResult.Text = current_qty + "/" + demand;
            txtRemaining.Text = (demand - current_qty).ToString();
        }

        private void frmWHMaterialIssueToPDDetail_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void dtpLotNo_ValueChanged(object sender, EventArgs e)
        {
            dtpLotNo.CustomFormat = "dd/MM/yyyy";
        }

        private void txtIssueLabelCode_KeyDown(object sender, KeyEventArgs e)
        {

        }
        private bool Check_Oldest_Lotno_Material(string whmrcode, string Mname)
        {
            string strQry = "declare @oldest_Y as varchar(20)  \n ";
            strQry += " declare @oldest_W as varchar(20)  \n ";
            strQry += " declare @oldest_lotno as date \n ";
            strQry += "  ---Get oldest year \n ";
            strQry += " select @oldest_Y=min(datepart(YY,created_time)) from    \n ";
            strQry += "   W_M_ReceiveLabel    \n ";
            strQry += "   where place = N'WH Material' and quantity>0   \n ";
            strQry += "   and qc_okng=N'OK'   \n ";
            strQry += "   and m_name = N'" + Mname + "' \n ";
            strQry += "   ---Get oldest week \n ";
            strQry += "   select @oldest_W=min(datepart(wk,created_time)) from    \n ";
            strQry += "   W_M_ReceiveLabel    \n ";
            strQry += "   where place = N'WH Material' and quantity>0   \n ";
            strQry += "   and qc_okng=N'OK'   \n ";
            strQry += "   and m_name = N'" + Mname + "'  \n ";
            strQry += "   and datepart(YY,created_time)=@oldest_Y  \n ";
            strQry += "   ---Get oldest lot_no \n ";
            strQry += "   select @oldest_lotno=min(lot_no) from   \n ";
            strQry += "   W_M_ReceiveLabel  \n ";
            strQry += "   where  1=1  \n ";
            strQry += "   and datepart(wk,created_time)=@oldest_W \n ";
            strQry += "   and datepart(YY,created_time)=@oldest_Y \n ";
            strQry += "   and place = N'WH Material' and quantity>0   \n ";
            strQry += "   and qc_okng=N'OK'   \n ";
            strQry += "   and m_name = N'" + Mname + "' \n ";
            strQry += "   ---Get result \n ";
            strQry += "   select CAST(@oldest_lotno AS varchar(20)) as oldest_lotno  \n ";
            strQry += "   ,@oldest_W as oldest_W,@oldest_Y as oldest_Y,  \n ";
            strQry += "   case   \n ";
            strQry += "   when lot_no=@oldest_lotno   \n ";
            strQry += "   and datepart(wk,created_time)=@oldest_W then 'OK'   \n ";
            strQry += "   else 'NOT OK'   \n ";
            strQry += "   end as Result   \n ";
            strQry += "   from W_M_ReceiveLabel   \n ";
            strQry += "   where whmr_code=N'" + whmrcode + "'  \n ";

            conn = new CmCn();
            DataTable dt = conn.ExcuteDataTable(strQry);
            if (dt.Rows[0]["Result"].ToString() == "OK")
            {
                return true;
            }
            else
            {
                lbNote.Text = "LỖI THÙNG CŨ NHẤT CÓ TUẦN LÀ:" + dt.Rows[0]["oldest_W"].ToString() + ",NĂM LÀ:" + dt.Rows[0]["oldest_Y"].ToString() + "  VÀ CÓ LOT_NO LÀ:" + dt.Rows[0]["oldest_lotno"].ToString();
                return false;
            }
        }
        private void txtBarcode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string label_code = txtBarcode.Text.Substring(2, txtBarcode.Text.Length - 2);
                if (Check_Oldest_Lotno_Material(label_code,txtMName.Text))
                {
                    Load_data_label(label_code);
                }
                txtBarcode.Text = "";
                txtBarcode.Focus();
            }
        }
        private void Load_data_label(string label_code)
        {
            string strQry = "select m_name,quantity,lot_no,qc_okng from W_M_ReceiveLabel where whmr_code = N'" + label_code + "' and place=N'WH Material'";
            conn = new CmCn();
            DataTable dt = conn.ExcuteDataTable(strQry);
            if (dt.Rows.Count < 1)
            {
                MessageBox.Show("LỖI THÙNG CÓ MÃ " + label_code + " KHÔNG CÓ TRONG KHO");
            }
            else
            {
                if (dt.Rows[0]["m_name"].ToString() == txtMName.Text)
                {
                    if (dt.Rows[0]["lot_no"].ToString() == "")
                    {
                        MessageBox.Show("LỖI THÙNG KHÔNG CÓ LOT NO, VUI LÒNG CHUYỂN QUA QC KIỂM TRA");
                    }
                    else
                    {
                        if (dt.Rows[0]["quantity"].ToString() != "0")
                        {
                            txtReceiveLabelCode.Text = label_code;
                            txtQtyInBox.Text = dt.Rows[0]["quantity"].ToString();
                            dtpLotNo.Value = DateTime.Parse(dt.Rows[0]["lot_no"].ToString());
                        }
                        else
                        {
                            txtReceiveLabelCode.Text = "";
                            txtQtyInBox.Text = "0";
                        }
                    }
                }
                else
                {
                    MessageBox.Show("LỖI THÙNG THÙNG CHỨA MÃ LINH KIỆN " + dt.Rows[0]["m_name"].ToString() + " KHÔNG KHỚP VỚI " + txtMName.Text);
                    txtReceiveLabelCode.Text = "";
                    txtQtyInBox.Text = "0";
                }
            }
            
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            txtBarcode.Focus();
        }

        private void dtpSupplyDate_ValueChanged(object sender, EventArgs e)
        {
            dtpSupplyDate.CustomFormat = "dd/MM/yyyy";
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (txtWeight.Text != "0" || txtQtyInBox.Text != "0")
            {
                try
                {
                    if (true)
                    {

                    }
                    item = new W_M_IssueLabel_Entity();
                    item.Whmi_code = "WHMI" + Generate_Label_code();
                    item.Whmr_code = txtReceiveLabelCode.Text;
                    item.M_name = txtMName.Text;
                    item.P_line = txtPLine.Text;
                    item.P_shift = txtPShift.Text;
                    item.M_doc_id = DocID;
                    item.Pic = PIC;
                    item.Quantity = float.Parse(txtQtyInBox.Text);
                    item.Lot_no = dtpLotNo.Value;
                    item.Weight = float.Parse(txtWeight.Text);
                    item.Product_customer_code = txtProdCustCode.Text;
                    item.Supply_date = dtpSupplyDate.Value;
                    item.Qty_in_box = float.Parse(txtQtyInBox.Text);
                    item.Transation = "Issue material to production (no scale)";
                    adoClass.Insert_W_M_IssueLabel_2(item, "Update special");
                    SplashScreenManager.ShowForm(this, typeof(frmWaitingForm), true, true, false);
                    SplashScreenManager.Default.SetWaitFormCaption("Printing label...\nĐang in tem...");
                    //-------------------------------------------
                    adoClass = new ADO();
                    adoClass.Print_W_M_IssueLabel(item);
                    SplashScreenManager.CloseForm();
                    Load_data_label(txtReceiveLabelCode.Text);
                    Load_Result();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("LỖI CHƯA QUÉT MÃ VẠCH TRÊN THÙNG \nPLEASE SCAN QR CODE", "ERROR");
            }
        }


        private int Generate_Label_code()
        {
            string Qry = "SELECT MAX(whmi_code) FROM W_M_IssueLabel ";
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
    }
}
