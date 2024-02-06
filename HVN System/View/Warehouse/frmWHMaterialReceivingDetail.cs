using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Data.SqlClient;
using System.IO;
using HVN_System.Entity;
using HVN_System.Util;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using Excel = Microsoft.Office.Interop.Excel;
using System.Drawing.Printing;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraSplashScreen;
using HVN_System.View.Admin;
using HVN_System.View.Planning;

namespace HVN_System.View.Warehouse
{
    public partial class frmWHMaterialReceivingDetail : Form
    {
        public frmWHMaterialReceivingDetail()
        {
            InitializeComponent();
        }
        public frmWHMaterialReceivingDetail(string truck_no_,string inv_no,string PIC, string cond1, string cond2, string cond3)
        {
            InitializeComponent();
            truck_no = truck_no_;
            lbInvNo.Text = inv_no;
            txtOperator.Text = PIC;
            txtCondition1.Text = cond1;
            txtCondition2.Text = cond2;
            txtCondition3.Text = cond3;
        }
        string truck_no;
        DataTable dt_temp_box, dt_invoice;
        private ADO adoClass;
        private CmCn conn;
        private W_M_ReceiveLabel_Entity Current_Label;
        private List<string> List_PN;
        private W_M_ReceiveDoc_Entity Current_item;
        private void txtBarcode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                lbError.Text = "";
                string QR_Code = txtBarcode.Text.Substring(2, txtBarcode.Text.Length - 2);
                if (QR_Code == "CLEAR")
                {
                    btnClear.PerformClick();
                }
                else if (QR_Code == "CONFIRM")
                {
                    btnConfirm.PerformClick();
                }
                else if (QR_Code == "WHNG")
                {
                    
                }
                else if (txtBarcode.Text.Length <= 6)
                {
                    lbError.Text = "TEM LỖI /WRONG LABEL";
                }
                else
                {
                    Current_Label = new W_M_ReceiveLabel_Entity();
                    if (txtBarcode.Text.Substring(2, 4) == "WHOP")
                    {
                        txtOperator.Text = txtBarcode.Text.Substring(6, txtBarcode.Text.Length - 6);
                    }
                    else if (txtBarcode.Text.Substring(2, 4) == "WHMR")
                    {
                        if (txtOperator.Text != "" && lbInvNo.Text != "")
                        {
                            if (ckUnship.Checked)
                            {
                                RemoveData(QR_Code);
                            }
                            else
                            {
                                InsertData(QR_Code);
                            }
                        }
                        else
                        {
                            lbError.Text = "BẠN CẦN QUÉT TÊN NHÂN VIÊN VÀ NHẬP BIỂN SỐ XE/ PLEASE SCAN QR CODE OF OPERATOR AND TYPE TRUCK PLATE";
                        }
                    }
                    else
                    {
                        if (lbInvNo.Text=="")
                        {
                            truck_no = txtBarcode.Text;
                            Load_Truck_Info(truck_no);
                        }
                        else
                        {
                            lbError.Text = "KHÔNG XÁC ĐINH ĐƯỢC THÔNG TIN TEM";
                        }
                    }
                }
                txtBarcode.Text = "";
                txtBarcode.Focus();
                
                if (lbError.Text == "")
                {
                    lbQtyBox.BackColor = SystemColors.Control;
                    lbInvNo.BackColor = SystemColors.Control;
                    Load_Actual_Data();
                    Load_Invoice();
                    gvActual.BestFitColumns();
                    gvInfo.BestFitColumns();
                }
                else
                {
                    lbQtyBox.BackColor = Color.Red;
                    lbInvNo.BackColor = Color.Red;
                }
            }
        }
        private void Load_Truck_Info(string truck_no_)
        {
            string strQry = "select *  \n ";
            strQry += " from W_M_ReceiveDoc \n ";
            strQry += " where receive_date=N'"+DateTime.Today.ToString("yyyy-MM-dd")+"' and truck_no=N'"+truck_no_+"' \n ";
            conn = new CmCn();
            DataTable dt = conn.ExcuteDataTable(strQry);
            if (dt.Rows.Count>0)
            {
                lbInvNo.Text = dt.Rows[0]["rm_doc_id"].ToString();

                Current_item = new W_M_ReceiveDoc_Entity();
                Current_item.Rm_doc_id= dt.Rows[0]["rm_doc_id"].ToString();
                Current_item.Supplier = dt.Rows[0]["supplier"].ToString();
                Current_item.Truck_no = dt.Rows[0]["truck_no"].ToString();
                Current_item.Receive_date = DateTime.Parse(dt.Rows[0]["receive_date"].ToString());
                Current_item.Rm_kind = dt.Rows[0]["rm_kind"].ToString();
                Load_invoice_infor();
            }
            else
            {
                lbError.Text = "LỖI KHÔNG TÌM THẤY THÔNG TIN XE " + truck_no_ + " HÔM NAY/ ERROR: CANNOT FIND THE TRUCK " + truck_no_ + " TODAY";
            }
        }
        private void Load_Invoice()
        {
            string strQry = "select a.m_name,a.quantity,a.number_carton,b.qty_notcheck,b.act_qty \n ";
            strQry += " ,b.act_boxqty,a.quantity-isnull(b.act_qty,0) as remain_qty \n ";
            strQry += " ,a.number_carton-ISNULL(b.act_boxqty,0) as remainbox,isnull(c.ng,0) as no_ng, \n ";
            strQry += " case \n ";
            strQry += "     when a.quantity = b.act_qty then 'ok' \n ";
            strQry += "     else 'wait' \n ";
            strQry += "     end as [Status] \n ";
            strQry += " from  \n ";
            strQry += " (select m_name,quantity,number_carton  \n ";
            strQry += " from W_M_ReceiveDocDetail where rm_doc_id=N'"+lbInvNo.Text+"') as a \n ";
            strQry += " left join \n ";
            strQry += " (select m_name,sum(quantity) as qty_notcheck,sum(quantity) as act_qty,count(m_name) as act_boxqty  \n ";
            strQry += " from TEMP_W_M_Receiving where rm_doc_id=N'" + lbInvNo.Text + "' group by m_name) as b \n ";
            strQry += " on a.m_name=b.m_name \n ";
            strQry += " left join  \n ";
            strQry += " (select m_name, count(quantity) as ng from TEMP_W_M_Receiving  \n ";
            strQry += " where rm_doc_id=N'" + lbInvNo.Text + "' and wh_okng=N'NG' group by m_name) as c \n ";
            strQry += " on a.m_name=c.m_name \n ";
            if (lbInvNo.Text!="")
            {
                conn = new CmCn();
                dt_invoice = conn.ExcuteDataTable(strQry);
                dgvInfo.DataSource = dt_invoice;
            }
        }
        private void Load_invoice_infor()
        {
            List_PN = new List<string>();
            adoClass = new ADO();
            DataTable dt = adoClass.Load_W_M_ReceiveDocDetail("m_name", "rm_doc_id=N'" + lbInvNo.Text + "'");
            if (dt.Rows.Count>0)
            {
                foreach (DataRow item in dt.Rows)
                {
                    string PN = item["m_name"].ToString();
                    List_PN.Add(PN);
                }
            }
            else
            {
                lbError.Text = "INVOICE KHÔNG TỒN TẠI TRÊN HỆ THỐNG/ THE INVOICE IS INCORRECT";
            }
        }
        private void Load_Actual_Data()
        {
            adoClass = new ADO();
            dt_temp_box = adoClass.Load_TEMP_W_M_Receiving("", "rm_doc_id=N'" + lbInvNo.Text + "' and truck_no=N'" + truck_no + "'");
            dgvActual.DataSource = dt_temp_box;
            lbQtyBox.Text = dt_temp_box.Rows.Count.ToString();
        }
        private void RemoveData(string label_code)
        {
            string strQry = "delete from TEMP_W_M_Receiving where whmr_code=N'" + label_code+ "' and rm_doc_id=N'" + lbInvNo.Text+"' ";
            conn = new CmCn();
            conn.ExcuteQry(strQry);
        }
        private void InsertData(string label_code)
        {
            adoClass = new ADO();
            DataTable dt2=adoClass.Load_TEMP_W_M_Receiving("whmr_code", "whmr_code=N'" + label_code + "'");
            if (dt2.Rows.Count>0)
            {
                lbError.Text = label_code + ": TEM ĐÃ ĐƯỢC THÊM VÀO DANH SÁCH CHỜ/ LABEL HAS BEEN ALREADY ADDED IN LIST";
            }
            else
            {
                DataTable dt = adoClass.Load_W_M_ReceiveLabel("", "whmr_code=N'" + label_code + "'");
                try
                {
                    Current_Label.Whmr_code = label_code;
                    Current_Label.M_name = dt.Rows[0]["M_name"].ToString();
                    Current_Label.Quantity = int.Parse(dt.Rows[0]["Quantity"].ToString());
                    //---------check PN in Invoice or not------------
                    var check_Invoice_PN = List_PN.Where(c => c == Current_Label.M_name);
                    if (check_Invoice_PN.Count() > 0)
                    {
                        //--- check over qty
                        if (!Check_Over_qty(Current_Label.M_name, Current_Label.Quantity))
                        {
                            lbError.Text = label_code + ": LỖI NVL " + Current_Label.M_name + " VƯỢT QUÁ SỐ LƯỢNG/ ERROR: OVER QTY";
                        }
                    }
                    else
                    {
                        lbError.Text = label_code + ":MÃ NVL " + Current_Label.M_name + " KHÔNG CÓ TRONG INVOICE/ PN NOT IN THE LIST OF INVOICE";
                    }
                    Current_Label.Place = "WH Material";
                    Current_Label.Wh_receive_time = DateTime.Now;
                    Current_Label.Wh_op = txtOperator.Text;
                    Current_Label.Rm_doc_id = lbInvNo.Text;
                    Current_Label.Truck_no = truck_no;
                    Current_Label.Transaction = "Entry WH";
                    if (lbError.Text == "")
                    {
                        Current_Label.Check = true;
                        Current_Label.Is_check = "True";
                    }
                    else
                    {
                        Current_Label.Check = false;
                        Current_Label.Is_check = "False";
                    }
                    adoClass.Insert_TEMP_W_M_Receiving(Current_Label);
                }
                catch (Exception ex)
                {
                    lbError.Text = ex.Message;
                }
            }
        }
        private void frmWHScanReceptionArea_Load(object sender, EventArgs e)
        {
            List_PN = new List<string>();
            dt_temp_box = new DataTable();
            dt_invoice = new DataTable();
            lcDetailLabel.Visibility= DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            lcDetailTable.Visibility= DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            txtCondition1.Text = "OK";
            txtCondition2.Text = "OK";
            txtCondition3.Text = "OK";
            //Load_invoice_infor();
            //Load_Actual_Data();
            //Load_Invoice();
        }
        private bool Check_Over_qty(string PN,float add_qty)
        {
            string strQry = "declare @t_qty float=(select quantity from W_M_ReceiveDocDetail where rm_doc_id=N'" + lbInvNo.Text + "' and m_name='" + PN + "'); \n ";
            strQry += " declare @a_qty float = \n ";
            strQry += " (select sum(quantity) as actual_qty  \n ";
            strQry += " from TEMP_W_M_Receiving  \n ";
            strQry += " where [rm_doc_id]='" + lbInvNo.Text + "' and m_name=N'" + PN + "' \n ";
            strQry += " group by rm_doc_id,truck_no); \n ";
            strQry += " select  \n ";
            strQry += " case  \n ";
            strQry += "     when isnull(@a_qty,0) + " + add_qty+" <= @t_qty  then 'ok' \n ";
            strQry += "     else 'fail' \n ";
            strQry += " end as result; \n ";
            conn = new CmCn();
            try
            {
                string result = conn.ExcuteString(strQry);
                if (result=="ok")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }
        private void btnClear_Click(object sender, EventArgs e)
        {
            lbInvNo.Text = "";
            txtBarcode.Text = "";
            lbQtyBox.Text = "0";
            txtBarcode.Focus();
            dt_invoice = new DataTable();
            dt_temp_box = new DataTable();
            dgvActual.DataSource = null;
            dgvInfo.DataSource = null;
        }

        private void gvInfo_RowStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs e)
        {
            GridView view = sender as GridView;
            if (e.RowHandle >= 0)
            {
                string status = view.GetRowCellValue(e.RowHandle, view.Columns["Status"]).ToString();
                if (status == "fail")
                {
                    e.Appearance.BackColor = Color.Red;
                }
                else if (status == "ok")
                {
                    e.Appearance.BackColor = Color.Chartreuse;
                }
                else if (status == "wait")
                {
                    e.Appearance.BackColor = Color.LightBlue;
                }
            }
        }

        private void gvActual_RowStyle(object sender, RowStyleEventArgs e)
        {
            GridView view = sender as GridView;
            if (e.RowHandle >= 0)
            {
                string status = view.GetRowCellValue(e.RowHandle, view.Columns["is_check"]).ToString();
                if (status == "False")
                {
                    e.Appearance.BackColor = Color.Red;
                }
                else if (status == "True")
                {
                    e.Appearance.BackColor = Color.Chartreuse;
                }
            }
        }
        private bool Check_before_confirm()
        {
            bool check = true;
            int count_err_iv = 0;
            foreach (DataRow item in dt_invoice.Rows)
            {
                if (item["Status"].ToString() == "wait")
                {
                    count_err_iv++;
                }
            }
            if (count_err_iv>0)
            {
                check = false;
            }
            if (dt_temp_box.Rows.Count>0)
            {
                int count_err = 0;
                foreach (DataRow item in dt_temp_box.Rows)
                {
                    if (item["is_check"].ToString() == "False")
                    {
                        count_err++;
                    }
                }
                if (count_err > 0)
                {
                    check = false;
                }
            }
            return check;
        }

        private void ckUnship_CheckedChanged(object sender, EventArgs e)
        {
            if (ckUnship.Checked)
            {
                ckUnship.Text = "BỎ KHỎI DANH SÁCH/ REMOVE CARTON FROM THE LIST";
                txtBarcode.Focus();
            }
            else
            {
                ckUnship.Text = "THÊM VÀO DANH SÁCH/ ADD CARTON TO THE LIST";
                txtBarcode.Focus();
            }
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (lbInvNo.Text!="")
            {
                if (Check_before_confirm())
                {
                    string strQry = "";
                    foreach (DataRow item in dt_temp_box.Rows)
                    {
                        if (item["is_active"].ToString()!="1")
                        {
                            strQry += "Update W_M_ReceiveLabel set wh_op=N'" + item["wh_op"].ToString() + "',wh_receive_time=N'" + item["wh_receive_time"].ToString() + "'," +
                            "wh_okng=N'" + item["wh_okng"].ToString() + "',truck_no=N'" + item["truck_no"].ToString() + "',place=N'" + item["place"].ToString() + "' " +
                            "where whmr_code=N'" + item["whmr_code"].ToString() + "' \n";
                        }
                    }
                    strQry += "insert into W_M_HistoryOfTransaction([whmr_code],[m_name],[quantity],[transaction],[input_time],[PIC],[truck_no],[invoice_no],[place]) \n";
                    strQry += "select [whmr_code],[m_name],[quantity],[transaction],[wh_receive_time],[wh_op],[truck_no],[rm_doc_id],[place]\n";
                    strQry += "from TEMP_W_M_Receiving where rm_doc_id=N'" + lbInvNo.Text + "' and is_active=N'0'\n";
                    strQry += " update [TEMP_W_M_Receiving] set is_active=N'1' where rm_doc_id=N'" + lbInvNo.Text + "' and is_active=N'0'\n";
                    try
                    {
                        SplashScreenManager.ShowForm(this, typeof(frmWaitingForm), true, true, false);
                        SplashScreenManager.Default.SetWaitFormCaption("Save document...\nĐang lưu kết quả..");
                        conn = new CmCn();
                        conn.ExcuteQry(strQry);
                        lbError.Text = "";
                        //adoClass = new ADO();
                        //adoClass.Print_W_M_Receive_Document(lbInvNo.Text, truck_no, txtCondition1.Text, txtCondition2.Text, txtCondition3.Text, true);
                        SplashScreenManager.CloseForm();
                        frmNotification frm = new frmNotification("XÁC NHẬN THÀNH CÔNG \nCONFIRM SUCCESSFULLY", "notification", 5);
                        frm.ShowDialog();
                    }
                    catch (Exception ex)
                    {
                        lbError.Text = "ERROR:" + ex.Message;
                    }
                }
                else
                {
                    lbError.Text = "CÓ HÀNG LỖI,THỪA HOẶC THIẾU HÀNG/ MISSING OR WRONG BOX IN THE LIST RECEIVING";
                }
            }
        } 

        private void btnPrintResult_Click(object sender, EventArgs e)
        {
            string strQry = "update TEMP_W_M_Receiving set wh_okng=N'NG'  \n ";
            strQry += " where whmr_code= (select top 1 whmr_code  \n ";
            strQry += "                   from TEMP_W_M_Receiving  \n ";
            strQry += "                   where rm_doc_id=N'"+lbInvNo.Text+"' \n ";
            strQry += "                   order by wh_receive_time desc) \n ";
            conn = new CmCn();
            conn.ExcuteQry(strQry);
            Load_Actual_Data();
            Load_Invoice();
        }

        private void txtCondition1_SelectedValueChanged(object sender, EventArgs e)
        {
            txtBarcode.Focus();
        }

        private void txtCondition2_SelectedValueChanged(object sender, EventArgs e)
        {
            txtBarcode.Focus();
        }

        private void txtCondition3_SelectedValueChanged(object sender, EventArgs e)
        {
            txtBarcode.Focus();
        }

        private void btnPrintLabel_Click(object sender, EventArgs e)
        {
            //SplashScreenManager.ShowForm(this, typeof(frmWaitingForm), true, true, false);
            //SplashScreenManager.Default.SetWaitFormCaption("Printing label...\nĐang in tem...");
            //try
            //{
            //    lbError.Text = "";
            //    if (lbInvNo.Text!=""&&txtOperator.Text!="")
            //    {
            //        if (not_print)
            //        {
            //            Print_Label();
            //            not_print = false;
            //        }
            //        else
            //        {
            //            lbError.Text = "BẠN ĐÃ ĐẶT LỆNH IN, KHỔNG THỂ IN THÊM/ YOU ALREADY PRESS PRINT, CANNOT PRINT MORE";
            //        }
            //    }
            //    else
            //    {
            //        lbError.Text = "LỖI THIẾU THÔNG TIN XE TẢI HOẶC CÔNG NHÂN/ MISSING TRUCK PLATE OR OPERATOR";
            //    }
            //}
            //catch (Exception)
            //{
            //    lbError.Text = "LỖI IN TEM/ ERROR CANNOT PRINT";
            //}
            //SplashScreenManager.CloseForm();
            //if (lbError.Text == "")
            //{
            //    frmNotification frm = new frmNotification("IN THÀNH CÔNG \nPRINT SUCCESSFULLY", "notification", 5);
            //    frm.ShowDialog();
            //}
            frmWHMaterial_ReceiveDocumentConfirm frm = new frmWHMaterial_ReceiveDocumentConfirm(Current_item);
            frm.Show();
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
        private void Print_Label()
        {
            adoClass = new ADO();
            List<W_M_ReceiveDocDetail_Entity> List_Data = new List<W_M_ReceiveDocDetail_Entity>();
            DataTable dt_list = adoClass.Load_W_M_ReceiveDocDetail("", "rm_doc_id=N'" + lbInvNo.Text + "'");
            foreach (DataRow row in dt_list.Rows)
            {
                W_M_ReceiveDocDetail_Entity item = new W_M_ReceiveDocDetail_Entity();
                item.Rm_doc_id = row["rm_doc_id"].ToString();
                item.M_name = row["m_name"].ToString();
                item.Number_carton = string.IsNullOrEmpty(row["number_carton"].ToString()) ? 0 : float.Parse(row["number_carton"].ToString());
                item.Quantity = string.IsNullOrEmpty(row["quantity"].ToString()) ? 0 : float.Parse(row["quantity"].ToString());
                item.Lot_no = row["m_lot_no"].ToString();
                List_Data.Add(item);
            }
            int start_Id = Generate_Label_code();
            int stt = 1;
            foreach (W_M_ReceiveDocDetail_Entity row in List_Data)
            {
                for (int i = 1; i <= row.Number_carton; i++)
                {
                    W_M_ReceiveLabel_Entity item = new W_M_ReceiveLabel_Entity();
                    item.Stt = stt;
                    item.Whmr_code = "WHMR" + start_Id;
                    item.M_name = row.M_name;
                    item.Quantity = float.Parse(Math.Round(row.Quantity / row.Number_carton, 0).ToString());
                    item.Rm_doc_id = lbInvNo.Text;
                    item.Created_date = DateTime.Now;
                    item.Created_user = txtOperator.Text;
                    item.IsSelected = true;
                    adoClass.Insert_W_M_ReceiveLabel(item);
                    adoClass.Print_W_M_ReceiveLabel(item, "WH");
                    start_Id++;
                    stt++;
                }
            }
        }

        private void btnShowDetail_Click(object sender, EventArgs e)
        {
            if (btnShowDetail.Text== "HIỆN CHI TIẾT")
            {
                lcDetailLabel.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                lcDetailTable.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                btnShowDetail.Text = "ẨN CHI TIẾT";
            }
            else
            {
                lcDetailLabel.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                lcDetailTable.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                btnShowDetail.Text = "HIỆN CHI TIẾT";
            }
        }

        private void frmWHMaterialReceivingDetail_FormClosing(object sender, FormClosingEventArgs e)
        {
            frmMain frm = new frmMain();
            frm.Show();
        }

        //private void Print_Documment()
        //{
        //    SplashScreenManager.ShowForm(this, typeof(frmWaitingForm), true, true, false);
        //    SplashScreenManager.Default.SetWaitFormCaption("Printing label...\nĐang in tem...");
        //    try
        //    {
        //        lbError.Text = "";
        //        adoClass = new ADO();
        //        adoClass.Print_W_M_Receive_Document(lbInvNo.Text, truck_no, txtCondition1.Text, txtCondition2.Text, txtCondition3.Text, true);
        //    }
        //    catch (Exception)
        //    {
        //        lbError.Text = "LỖI IN TEM/ ERROR CANNOT PRINT";
        //    }
        //    SplashScreenManager.CloseForm();
        //    if (lbError.Text == "")
        //    {
        //        frmNotification frm = new frmNotification("IN THÀNH CÔNG \nPRINT SUCCESSFULLY", "notification", 5);
        //        frm.ShowDialog();
        //    }
        //}
    }
}
