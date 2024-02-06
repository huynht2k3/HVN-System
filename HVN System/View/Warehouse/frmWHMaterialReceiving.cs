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

namespace HVN_System.View.Warehouse
{
    public partial class frmWHMaterialReceiving : Form
    {
        public frmWHMaterialReceiving()
        {
            InitializeComponent();
        }
        private ADO adoClass;
        private CmCn conn;
        DataTable dt;
        private P_Label_Entity Current_Label;

        private void txtBarcode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                lbError.Text = "";
                if (txtBarcode.Text.Length <= 6)
                {
                    lbError.Text = "TEM LỖI /WRONG LABEL";
                    lbInvoiceNo.BackColor = Color.Red;
                    lbITruckNo.BackColor = Color.Red;
                    return;
                }
                string QR_Code = txtBarcode.Text.Substring(2, txtBarcode.Text.Length - 2);
                if (QR_Code == "CLEAR")
                {
                    btnClear.PerformClick();
                }
                else if (QR_Code == "CONFIRM")
                {
                    btnConfirm.PerformClick();
                }
                else
                {
                    Current_Label = new P_Label_Entity();
                    if (txtBarcode.Text.Substring(2, 4) == "WHOP")
                    {
                        txtOperator.Text = txtBarcode.Text.Substring(6, txtBarcode.Text.Length - 6);
                    }
                    //else if (txtBarcode.Text.Substring(2, 4) == "HUTV")
                    //{
                    //    Load_Invoice(QR_Code);
                    //}
                    else
                    {
                        if (txtOperator.Text != "")
                        {
                            if (lbITruckNo.Text=="")
                            {
                                Load_Truck_info(txtBarcode.Text);
                            }
                            else
                            {
                                lbError.Text = "VUI LÒNG XÁC NHẬN TRƯỚC KHI NHÂP CÔNG MỚI/ PLEASE SCAN CONFIRM BEFORE ENTER NEW TRUCK";
                            }
                        }
                        else
                        {
                            lbError.Text = "BẠN CẦN QUÉT TÊN NHÂN VIÊN/ PLEASE SCAN QR CODE OF OPERATOR";
                        }
                    }
                }
                txtBarcode.Text = "";
                txtBarcode.Focus();
                if (lbError.Text == "")
                {
                    lbInvoiceNo.BackColor = SystemColors.Control;
                    lbITruckNo.BackColor = SystemColors.Control;
                }
                else
                {
                    lbInvoiceNo.BackColor = Color.Red;
                    lbITruckNo.BackColor = Color.Red;
                }
            }
        }
        private void Load_Invoice(string inv_no)
        {
            lbError.Text = "";
            if (lbITruckNo.Text != "" && cboCondition1.Text != "" && cboCondition2.Text != "" && cboCondition3.Text != "")
            {
                
            }
            else
            {
                lbError.Text = "LỖI ĐIỀN THIẾU THÔNG TIN/ COULD NOT SELECT, MISSING INFORMATION";
                return;
            }
            lbInvoiceNo.Text = inv_no;
            adoClass = new ADO();
            DataTable dt = adoClass.Load_W_M_ReceiveDoc("", "rm_doc_id=N'" + lbInvoiceNo.Text+ "' and truck_no=N'" + lbITruckNo.Text + "'");
            if (dt.Rows.Count>0)
            {
                frmWHMaterialReceivingDetail frm = new frmWHMaterialReceivingDetail(lbITruckNo.Text, lbInvoiceNo.Text,txtOperator.Text,cboCondition1.Text, cboCondition2.Text, cboCondition3.Text);
                frm.ShowDialog();
                string strQry = "select a.m_name,a.quantity,a.number_carton,b.qty_notcheck,b.act_qty \n ";
                strQry += " ,b.act_boxqty,a.quantity-isnull(b.act_qty,0) as remain_qty \n ";
                strQry += " ,a.number_carton-ISNULL(b.act_boxqty,0) as remainbox,isnull(c.ng,0) as no_ng, \n ";
                strQry += " case \n ";
                strQry += "     when a.quantity = b.act_qty then 'ok' \n ";
                strQry += "     else 'wait' \n ";
                strQry += "     end as [Status] \n ";
                strQry += " from  \n ";
                strQry += " (select m_name,quantity,number_carton  \n ";
                strQry += " from W_M_ReceiveDocDetail where rm_doc_id=N'" + lbInvoiceNo.Text + "') as a \n ";
                strQry += " left join \n ";
                strQry += " (select m_name,sum(quantity) as qty_notcheck,sum(quantity) as act_qty,count(m_name) as act_boxqty  \n ";
                strQry += " from TEMP_W_M_Receiving where rm_doc_id=N'" + lbInvoiceNo.Text + "' group by m_name) as b \n ";
                strQry += " on a.m_name=b.m_name \n ";
                strQry += " left join  \n ";
                strQry += " (select m_name, sum(quantity) as ng from TEMP_W_M_Receiving  \n ";
                strQry += " where rm_doc_id=N'" + lbInvoiceNo.Text + "' and wh_okng=N'NG' group by m_name) as c \n ";
                strQry += " on a.m_name=c.m_name \n ";
                conn = new CmCn();
                DataTable dt2 = conn.ExcuteDataTable(strQry);
                dgvInfo.DataSource = dt2;
                Load_Truck_info(lbITruckNo.Text);
            }
            else
            {
                lbError.Text="INVOICE NÀY KHÔNG NẰM TRONG DANH SÁCH XE TẢI/ THIS INVOICE IS NOT BELONG THIS TRUCK";
            }
        }

        private void Load_Truck_info(string barcode)
        {
            string strQry = "select std.truck_no,std.rm_doc_id,std_qty, act.act_qty, \n ";
            strQry += " case \n ";
            strQry += "      when act.act_qty> std_qty then 'fail' \n ";
            strQry += "      when act.act_qty= std_qty then 'ok' \n ";
            strQry += "      else 'wait' \n ";
            strQry += " end as [Status] \n ";
            strQry += " from  \n ";
            strQry += " (select a.truck_no,a.rm_doc_id,sum(b.quantity) as std_qty from W_M_ReceiveDoc a,W_M_ReceiveDocDetail b  \n ";
            strQry += " where a.truck_no=N'"+barcode+"' and a.rm_doc_id=b.rm_doc_id and a.receive_date=N'"+DateTime.Today.ToString("yyyy-MM-dd")+"' \n ";
            strQry += " group by a.rm_doc_id,a.truck_no) as std \n ";
            strQry += " left join \n ";
            strQry += " (select rm_doc_id, sum(quantity) as act_qty \n ";
            strQry += " from TEMP_W_M_Receiving \n ";
            strQry += " where truck_no=N'" + barcode + "' \n ";
            strQry += " group by rm_doc_id) as act \n ";
            strQry += " on std.rm_doc_id=act.rm_doc_id \n ";
            conn = new CmCn();
            dt = new DataTable();
            dt = conn.ExcuteDataTable(strQry);
            if (dt.Rows.Count > 0)
            {
                dgvActual.DataSource = dt;
                lbITruckNo.Text = barcode;
            }
            else
            {
                lbError.Text = "KHÔNG TÌM THẤY THÔNG TIN XE CÔNG TRÊN HỆ THỐNG/ THE TRUCK NO IS NOT AVAILABEL ON SYSTEM";
                dgvActual.DataSource = null;
            }
        }
        private void frmWHScanReceptionArea_Load(object sender, EventArgs e)
        {
        }
        private void btnClear_Click(object sender, EventArgs e)
        {
            dgvActual.DataSource = null;
            dgvInfo.DataSource = null;
            lbITruckNo.Text = "";
            txtBarcode.Text = "";
            lbInvoiceNo.Text = "";
            lbError.Text = "";
            txtBarcode.Focus();
            lbInvoiceNo.BackColor = SystemColors.Control;
            lbITruckNo.BackColor = SystemColors.Control;

        }

        private void frmWHScanInLocation_FormClosing(object sender, FormClosingEventArgs e)
        {
            frmMain frm = new frmMain();
            frm.Show();
        }
        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {

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
        private bool Check_before_confirm()
        {
            bool check = true;
            if (dt.Rows.Count>0)
            {
                foreach (DataRow item in dt.Rows)
                {
                    if (item["Status"].ToString()== "fail"|| item["Status"].ToString() == "wait")
                    {
                        check = false;
                    }
                }
            }
            adoClass = new ADO();
            DataTable check_detail = adoClass.Load_TEMP_W_M_Receiving("is_check", "truck_no=N'" + lbITruckNo.Text + "' and is_check='False'");
            if (check_detail.Rows.Count>0)
            {
                check = false;
            }
            return check;
        }

       

        private void btnExportDetail_Click(object sender, EventArgs e)
        {
            adoClass = new ADO();
            adoClass.Export_Excel(dgvActual);
        }

        private void btnExportInfo_Click(object sender, EventArgs e)
        {
            adoClass = new ADO();
            adoClass.Export_Excel(dgvInfo);
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (Check_before_confirm())
            {
                adoClass = new ADO();
                DataTable dt_confirm = adoClass.Load_TEMP_W_M_Receiving("", "truck_no=N'" + lbITruckNo.Text+ "'");
                adoClass.Confirm_Receiving(lbITruckNo.Text, dt_confirm);
                lbITruckNo.Text = "";
                lbInvoiceNo.Text = "";
                dgvActual.DataSource = null;
                dgvInfo.DataSource = null;
                //Print_Documment();
                frmNotification frm = new frmNotification("XÁC NHẬN THÀNH CÔNG \nCONFIRM SUCCESSFULLY", "notification", 5);
                frm.ShowDialog();
            }
            else
            {
                lbError.Text = "CÓ HÀNG LỖI HOẶC THỪA NGUYÊN VẬT LIỆU/ SHOULD FINISH ALL PN BEFORE CONFIRM";
            }
        }

        private void gvActual_RowClick(object sender, RowClickEventArgs e)
        {
            lbInvoiceNo.Text = gvActual.GetRowCellValue(gvActual.FocusedRowHandle, "rm_doc_id").ToString();
            Load_Invoice(lbInvoiceNo.Text);
        }

        private void Print_Documment()
        {
            SplashScreenManager.ShowForm(this, typeof(frmWaitingForm), true, true, false);
            SplashScreenManager.Default.SetWaitFormCaption("Printing label...\nĐang in tem...");
            lbError.Text = "";
            adoClass = new ADO();
            if (lbITruckNo.Text != "" && cboCondition1.Text != "" && cboCondition2.Text != "" && cboCondition3.Text != "")
            {
                foreach (DataRow row in dt.Rows)
                {
                    //adoClass.Print_W_M_Receive_Document(row["rm_doc_id"].ToString(), row["truck_no"].ToString(), cboCondition1.Text, cboCondition2.Text, cboCondition3.Text, false);
                }
                frmNotification frm = new frmNotification("IN THÀNH CÔNG \nPRINT SUCCESFULLY", "notification", 5);
                frm.Show();
            }
            else
            {
                lbError.Text = "KHÔNG THỂ IN DO ĐIỀN THIẾU THÔNG TIN/ COULD NOT PRINT, MISSING INFORMATION";
            }
            SplashScreenManager.CloseForm();
        }
    }
}
