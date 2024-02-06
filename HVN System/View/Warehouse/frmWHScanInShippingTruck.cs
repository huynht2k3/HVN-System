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

namespace HVN_System.View.Warehouse
{
    public partial class frmWHScanInShippingTruck : Form
    {
        public frmWHScanInShippingTruck()
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
                else if (QR_Code == "SCAN WAITING ZONE")
                {
                    frmWHScaninWaitingZone frm = new frmWHScaninWaitingZone();
                    frm.Show();
                    this.Hide();
                }
                else if (QR_Code == "SCAN LOCATION")
                {
                    frmWHScanInLocation2 frm = new frmWHScanInLocation2();
                    frm.Show();
                    this.Hide();
                }
                else if (QR_Code == "CONFIRM")
                {
                    btnConfirm.PerformClick();
                }
                else
                {
                    string scan_id = txtBarcode.Text.Substring(0, 2);
                    Current_Label = new P_Label_Entity();
                    if (txtBarcode.Text.Substring(2, 4) == "WHOP")
                    {
                        txtOperator.Text = txtBarcode.Text.Substring(6, txtBarcode.Text.Length - 6);
                    }
                    else if (txtBarcode.Text.Substring(2, 4) == "HUTV")
                    {
                        Load_Invoice(QR_Code);
                    }
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
            lbInvoiceNo.Text = inv_no;
            adoClass = new ADO();
            DataTable dt = adoClass.Load_LOG_Invoice("", "invoice_no=N'"+lbInvoiceNo.Text+ "' and truck_no=N'" + lbITruckNo.Text + "'");
            if (dt.Rows.Count>0)
            {
                frmWHScanInShipping2  frm= new frmWHScanInShipping2(lbITruckNo.Text, lbInvoiceNo.Text,txtOperator.Text);
                frm.ShowDialog();
                string strQry = "select std.invoice_no,std.product_customer_code,std.qty,actual.actual_qty, \n ";
                strQry += " case \n ";
                strQry += "     when actual.actual_qty > std.qty then 'fail' \n ";
                strQry += "     when actual.actual_qty = std.qty then 'ok' \n ";
                strQry += "     else 'wait' \n ";
                strQry += " end as [Status] \n ";
                strQry += " from  \n ";
                strQry += " (select a.invoice_no,b.product_customer_code,b.quantity as qty from LOG_Invoice a,LOG_InvoiceDetail b \n ";
                strQry += " where a.invoice_no='" + lbInvoiceNo.Text + "' and a.invoice_no=b.invoice_no) as std \n ";
                strQry += " left join \n ";
                strQry += " (select product_customer_code,sum(product_quantity) as actual_qty from TEMP_W_ShippingInfor where invoice_no=N'" + lbInvoiceNo.Text + "' \n ";
                strQry += " group by product_customer_code) as actual \n ";
                strQry += " on std.product_customer_code = actual.product_customer_code \n ";
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
            string strQry = "select std.truck_no,std.invoice_no,std.qty, actual.actual_qty, \n ";
            strQry += " case \n ";
            strQry += "      when actual.actual_qty> std.qty then 'fail' \n ";
            strQry += "      when actual.actual_qty= std.qty then 'ok' \n ";
            strQry += "      else 'wait' \n ";
            strQry += " end as [Status] \n ";
            strQry += " from  \n ";
            strQry += " (select a.truck_no,a.invoice_no, SUM(b.quantity) as qty from LOG_Invoice a , LOG_InvoiceDetail b \n ";
            strQry += " where a.truck_no=N'"+ barcode + "' and a.invoice_no=b.invoice_no and a.ship_date=N'"+DateTime.Today.ToString("yyyy-MM-dd")+"' \n ";
            strQry += " group by a.truck_no,a.invoice_no) as std \n ";
            strQry += " left join \n ";
            strQry += " (select invoice_no,sum(product_quantity) as actual_qty from TEMP_W_ShippingInfor \n ";
            strQry += " WHERE truck_no=N'" + barcode + "' \n ";
            strQry += " group by truck_no, invoice_no) as actual \n ";
            strQry += " on std.invoice_no=actual.invoice_no \n ";
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
            DataTable check_detail = adoClass.Load_TEMP_W_ShippingInfor("is_check", "truck_no=N'" + lbITruckNo.Text + "' and is_check='False'");
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
                DataTable dt_confirm = adoClass.Load_TEMP_W_ShippingInfor("", "truck_no=N'" + lbITruckNo.Text+ "' and isActive=N'0'");
                adoClass.Confirm_Shipping(lbITruckNo.Text, dt_confirm);
                lbITruckNo.Text = "";
                lbInvoiceNo.Text = "";
                dgvActual.DataSource = null;
                dgvInfo.DataSource = null;
                frmNotification frm = new frmNotification("XÁC NHẬN THÀNH CÔNG \nCONFIRM SUCCESSFULLY", "notification", 5);
                frm.ShowDialog();
            }
            else
            {
                lbError.Text = "CÓ HÀNG LỖI HOẶC THỪA TRÊN XE/ SHOULD FINISH ALL PRODUCT BEFORE CONFIRM";
            }
        }

        private void gvActual_RowClick(object sender, RowClickEventArgs e)
        {
            lbInvoiceNo.Text = gvActual.GetRowCellValue(gvActual.FocusedRowHandle, "invoice_no").ToString();
            Load_Invoice(lbInvoiceNo.Text);
        }
    }
}
