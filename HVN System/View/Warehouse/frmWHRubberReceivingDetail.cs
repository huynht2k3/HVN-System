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
    public partial class frmWHRubberReceivingDetail : Form
    {
        public frmWHRubberReceivingDetail()
        {
            InitializeComponent();
        }
        string truck_no,current_r_name="";
        DataTable dt_temp_box, dt_invoice;
        private ADO adoClass;
        private CmCn conn;
        private W_M_RubberLabel_Entity Current_Label;
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
                    Current_Label = new W_M_RubberLabel_Entity();
                    if (txtBarcode.Text.Substring(2, 4) == "WHOP")
                    {
                        txtOperator.Text = txtBarcode.Text.Substring(6, txtBarcode.Text.Length - 6);
                    }
                    else if (txtBarcode.Text.Substring(2, 4) == "WHRR")
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

            string strQry = "select a.m_name,a.quantity,a.number_carton,b.act_qty  \n ";
            strQry += "   ,b.act_boxqty,a.quantity-isnull(b.act_qty,0) as remain_qty  \n ";
            strQry += "   ,a.number_carton-ISNULL(b.act_boxqty,0) as remainbox,isnull(c.ng,0) as no_ng,  \n ";
            strQry += "   case  \n ";
            strQry += "       when a.quantity <= b.act_qty then 'ok'  \n ";
            strQry += "       else 'wait'  \n ";
            strQry += "       end as [Status]  \n ";
            strQry += "   from   \n ";
            strQry += "   (select m_name,quantity,number_carton   \n ";
            strQry += "   from W_M_ReceiveDocDetail where rm_doc_id=N'" + lbInvNo.Text + "') as a  \n ";
            strQry += "   left join  \n ";
            strQry += "   (select r_name,sum([weight]) as act_qty,count(r_name) as act_boxqty   \n ";
            strQry += "   from TEMP_W_R_Receiving where rm_doc_id=N'" + lbInvNo.Text + "' group by r_name) as b  \n ";
            strQry += "   on a.m_name=b.r_name  \n ";
            strQry += "   left join   \n ";
            strQry += "   (select r_name, count([weight]) as ng from TEMP_W_R_Receiving   \n ";
            strQry += "   where rm_doc_id=N'" + lbInvNo.Text + "' and wh_okng=N'NG' group by r_name) as c  \n ";
            strQry += "   on a.m_name=c.r_name  \n ";

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
            dt_temp_box = adoClass.Load_TEMP_W_R_Receiving("", "rm_doc_id=N'" + lbInvNo.Text + "'");
            dgvActual.DataSource = dt_temp_box;
            lbQtyBox.Text = dt_temp_box.Rows.Count.ToString();
        }
        private void RemoveData(string label_code)
        {
            string strQry = "delete from TEMP_W_R_Receiving where whrr_code=N'" + label_code+ "' and rm_doc_id=N'" + lbInvNo.Text+"' ";
            conn = new CmCn();
            conn.ExcuteQry(strQry);
        }
        private void InsertData(string label_code)
        {
            adoClass = new ADO();
            DataTable dt2=adoClass.Load_TEMP_W_R_Receiving("whrr_code", "whrr_code=N'" + label_code + "'");
            if (dt2.Rows.Count>0)
            {
                lbError.Text = label_code + ": TEM ĐÃ ĐƯỢC THÊM VÀO DANH SÁCH CHỜ/ LABEL HAS BEEN ALREADY ADDED IN LIST";
            }
            else
            {
                DataTable dt = adoClass.Load_W_M_RubberLabel("", "whrr_code=N'" + label_code + "'");
                try
                {
                    Current_Label.Whrr_code = label_code;
                    Current_Label.R_name = dt.Rows[0]["r_name"].ToString();
                    Current_Label.Weight = float.Parse(dt.Rows[0]["weight"].ToString());
                    //---------check PN in Invoice or not------------
                    var check_Invoice_PN = List_PN.Where(c => c == Current_Label.R_name);
                    if (check_Invoice_PN.Count() > 0)
                    {
                        
                    }
                    else
                    {
                        lbError.Text = label_code + ":MÃ CAO SU " + Current_Label.R_name + " KHÔNG CÓ TRONG INVOICE/ PN NOT IN THE LIST OF INVOICE";
                    }
                    Current_Label.Place = "WH Rubber";
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
                    adoClass.Insert_TEMP_W_R_Receiving(Current_Label);
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
                            strQry += "Update W_M_RubberLabel set wh_op=N'" + item["wh_op"].ToString() + "',wh_receive_time=N'" + item["wh_receive_time"].ToString() + "'," +
                            "wh_okng=N'" + item["wh_okng"].ToString() + "',truck_no=N'" + item["truck_no"].ToString() + "',place=N'" + item["place"].ToString() + "' " +
                            "where whrr_code=N'" + item["whrr_code"].ToString() + "' \n";
                        }
                    }
                    strQry += "insert into W_M_RubberTransaction([whrr_code],[r_name],[weight],[transaction],[input_time],[PIC],[truck_no],[invoice_no],[place]) \n";
                    strQry += "select [whrr_code],[r_name],[weight],[transaction],[wh_receive_time],[wh_op],[truck_no],[rm_doc_id],[place]\n";
                    strQry += "from TEMP_W_R_Receiving where rm_doc_id=N'" + lbInvNo.Text + "' and is_active=N'0'\n";
                    strQry += " update [TEMP_W_R_Receiving] set is_active=N'1' where rm_doc_id=N'" + lbInvNo.Text + "' and is_active=N'0'\n";
                    try
                    {
                        conn = new CmCn();
                        conn.ExcuteQry(strQry);
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
            string strQry = "update TEMP_W_R_Receiving set wh_okng=N'NG'  \n ";
            strQry += " where whrr_code= (select top 1 whrr_code  \n ";
            strQry += "                   from TEMP_W_R_Receiving  \n ";
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

        private void gvInfo_DoubleClick(object sender, EventArgs e)
        {
            btnPrintLabel.PerformClick();
        }

        private void gvInfo_Click(object sender, EventArgs e)
        {

        }

        private void btnPrintLabel_Click(object sender, EventArgs e)
        {
            if (lbInvNo.Text==""||txtOperator.Text=="")
            {
                lbError.Text = "LỖI CHƯA NHẬP THÔNG TIN INVOICE HOẶC TÊN NGƯỜI THỰC HIỆN";
            }
            else
            {
                frmWHRubberReceiving frm = new frmWHRubberReceiving(current_r_name, truck_no, lbInvNo.Text, txtOperator.Text);
                frm.ShowDialog();
                Load_Actual_Data();
                Load_Invoice();
            }
        }

        private void gvInfo_RowClick(object sender, RowClickEventArgs e)
        {
            current_r_name= gvInfo.GetRowCellValue(gvInfo.FocusedRowHandle, "m_name").ToString();
            
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
