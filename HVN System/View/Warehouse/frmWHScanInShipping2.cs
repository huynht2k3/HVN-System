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
    public partial class frmWHScanInShipping2 : Form
    {
        public frmWHScanInShipping2()
        {
            InitializeComponent();
        }
        public frmWHScanInShipping2(string truck_no_,string inv_no,string PIC)
        {
            InitializeComponent();
            truck_no = truck_no_;
            lbInvNo.Text = inv_no;
            txtOperator.Text = PIC;
        }
        string truck_no;
        DataTable dt_temp_box;
        private ADO adoClass;
        private CmCn conn;
        private P_Label_Entity Current_Label;
        private List<string> List_PN;
        private string last_PN = "";
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
                else if (QR_Code == "UNSHIP")
                {
                    if (ckUnship.Checked)
                    {
                        ckUnship.Checked = false;
                    }
                    else
                    {
                        ckUnship.Checked = true;
                    }
                }
                else if (QR_Code == "CONFIRM")
                {
                    btnConfirm.PerformClick();
                }
                else if (txtBarcode.Text.Length <= 6)
                {
                    lbError.Text = "TEM LỖI /WRONG LABEL";
                }
                else
                {
                    string scan_id = txtBarcode.Text.Substring(0, 2);
                    Current_Label = new P_Label_Entity();
                    if (txtBarcode.Text.Substring(2, 4) == "WHOP")
                    {
                        txtOperator.Text = txtBarcode.Text.Substring(6, txtBarcode.Text.Length - 6);
                    }
                    else if (txtBarcode.Text.Substring(2, 4) == "WHPL")
                    {
                        if (txtOperator.Text != "" && lbInvNo.Text != "")
                        {
                            if (ckUnship.Checked)
                            {
                                RemoveDataPallet(QR_Code);
                            }
                            else
                            {
                                InserDataPallet(QR_Code);
                            }
                            Load_check_pallet(last_PN);
                        }
                        else
                        {
                            lbError.Text = "BẠN CẦN QUÉT TÊN NHÂN VIÊN VÀ SỐ INVOICE/ PLEASE SCAN QR CODE OF OPERATOR AND INVOICE NO";
                        }
                    }
                    else if (txtBarcode.Text.Substring(2, 4) == "HUTV")
                    {
                        lbInvNo.Text = txtBarcode.Text.Substring(2, txtBarcode.Text.Length - 2);
                        DataTable dt = adoClass.Load_LOG_Invoice("", "invoice_no=N'" + lbInvNo.Text + "' and truck_no=N'" + truck_no + "'");
                        if (dt.Rows.Count > 0)
                        {
                            if (Check_before_confirm())
                            {
                                string error_box = Check_box_error();
                                string error_pallet = Check_Pallet_error();
                                if (error_box == "" && error_pallet=="")
                                {
                                    Load_invoice_infor();
                                }
                                else
                                {
                                    lbError.Text = "LỖI FIFO CÁC MÃ HÀNG SAU: Box:" + error_box + "| Pallet:"+error_pallet;
                                }
                            }
                            else
                            {
                                lbError.Text = "CÓ HÀNG LỖI HOẶC THỪA TRÊN XE, KHÔNG THỂ CHUYỂN INVOICE MỚI/ SHOULD FINISH ALL PRODUCT BEFORE CHANGE INVOICE";
                            }
                        }
                        else
                        {
                            lbError.Text = "INVOICE NÀY KHÔNG NẰM TRONG DANH SÁCH XE TẢI/ THIS INVOICE IS NOT BELONG THIS TRUCK";
                        }
                    }
                    else
                    {
                        if (txtOperator.Text != "" && lbInvNo.Text != "")
                        {
                            if (ckUnship.Checked)
                            {
                                RemoveData(QR_Code);
                            }
                            else
                            {
                                InsertData(QR_Code, "Shipping the box");
                            }
                            Load_check_box(last_PN);
                        }
                        else
                        {
                            lbError.Text = "BẠN CẦN QUÉT TÊN NHÂN VIÊN VÀ SỐ INVOICE/ PLEASE SCAN QR CODE OF OPERATOR AND INVOICE NO";
                        }
                    }
                }
                txtBarcode.Text = "";
                txtBarcode.Focus();
                Load_Actual_Data();
                Load_Invoice();
                if (lbError.Text == "")
                {
                    lbQtyBox.BackColor = SystemColors.Control;
                    lbInvNo.BackColor = SystemColors.Control;
                    lbQtyPallet.BackColor = SystemColors.Control;
                }
                else
                {
                    lbQtyBox.BackColor = Color.Red;
                    lbInvNo.BackColor = Color.Red;
                    lbQtyPallet.BackColor = Color.Red;
                }
            }
        }
        private string Check_box_error()
        {
            string strQry2 = "select product_customer_code from [TEMP_W_ShippingInfor]  \n ";
            strQry2 += " where [transaction]=N'Shipping the box' \n ";
            strQry2 += " and invoice_no=N'" + lbInvNo.Text + "' \n ";
            strQry2 += " group by product_customer_code \n ";
            conn = new CmCn();
            DataTable dt = conn.ExcuteDataTable(strQry2);
            string list_Error = "";
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow item in dt.Rows)
                {
                    string strQry = "select case   \n ";
                    strQry += "       when count(kq.Stt)=max(kq.Stt) then ''    \n ";
                    strQry += "       else '" + item["product_customer_code"].ToString() + "'   \n ";
                    strQry += "       end as Double_check  \n ";
                    strQry += "   from  \n ";
                    strQry += "   (select ROW_NUMBER() OVER(ORDER BY (a.id_number)) as Stt,a.*,      \n ";
                    strQry += "          case       \n ";
                    strQry += "          when b.product_quantity>0 then 'OK'      \n ";
                    strQry += "          else 'WAIT'      \n ";
                    strQry += "          end as Result      \n ";
                    strQry += "          from    \n ";
                    strQry += "   (select label_code as id_number,product_customer_code,product_quantity,plan_date from P_Label      \n ";
                    strQry += "          where 1=1      \n ";
                    strQry += "          and pallet_no is null    \n ";
                    strQry += "          and place in ('Waiting Zone','FG Zone','Packing Zone')      \n ";
                    strQry += "          and product_customer_code=N'" + item["product_customer_code"].ToString() + "') a  \n ";
                    strQry += "   left join  \n ";
                    strQry += "   (select label_code,product_quantity from TEMP_W_ShippingInfor   \n ";
                    strQry += "   where invoice_no=N'" + lbInvNo.Text + "' and product_customer_code=N'" + item["product_customer_code"].ToString() + "') b  \n ";
                    strQry += "   on a.id_number=b.label_code) kq \n ";
                    strQry += "   where kq.Result=N'OK' \n ";
                    conn = new CmCn();
                    string result = conn.ExcuteString(strQry);
                    if (result != "")
                    {
                        list_Error += result + ",";
                    }
                }
            }
            return list_Error;
        }
        private string Check_Pallet_error()
        {
            string strQry2 = "select product_customer_code from [TEMP_W_ShippingInfor]  \n ";
            strQry2 += " where [transaction]=N'Shipping the pallet' \n ";
            strQry2 += " and invoice_no=N'" + lbInvNo.Text + "' \n ";
            strQry2 += " group by product_customer_code \n ";
            conn = new CmCn();
            DataTable dt = conn.ExcuteDataTable(strQry2);
            string list_error = "";
            foreach (DataRow item in dt.Rows)
            {
                string strQry = "select case   \n ";
                strQry += "       when count(kq.Stt)=max(kq.Stt) then ''   \n ";
                strQry += "       else '" + item["product_customer_code"].ToString() + "'   \n ";
                strQry += "       end as Double_check   \n ";
                strQry += " from \n ";
                strQry += " (select ROW_NUMBER() OVER(ORDER BY (a.plan_date)) as Stt,a.*,      \n ";
                strQry += "          case       \n ";
                strQry += "          when b.Qty>0 then 'OK'      \n ";
                strQry += "          else 'WAIT'      \n ";
                strQry += "          end as Result      \n ";
                strQry += "          from    \n ";
                strQry += "   (select pallet_no as id_number, min(plan_date) as plan_date,max(product_customer_code) as product_customer_code from P_Label      \n ";
                strQry += "          where 1=1      \n ";
                strQry += "          and pallet_no not in ('')    \n ";
                strQry += "          and place in ('Waiting Zone','FG Zone','Packing Zone')      \n ";
                strQry += "          and product_customer_code=N'" + item["product_customer_code"].ToString() + "'  \n ";
                strQry += "          group by pallet_no) a  \n ";
                strQry += "   left join  \n ";
                strQry += "   (select pallet_no,sum(product_quantity) as Qty from TEMP_W_ShippingInfor   \n ";
                strQry += "   where invoice_no=N'" + lbInvNo.Text + "' and product_customer_code=N'" + item["product_customer_code"].ToString() + "'  \n ";
                strQry += "   group by pallet_no) b  \n ";
                strQry += "   on a.id_number=b.pallet_no) kq \n ";
                strQry += "  where kq.Result=N'OK' \n ";
                conn = new CmCn();
                string result = conn.ExcuteString(strQry);
                if (result != "")
                {
                    list_error += result + ",";
                }
            }
            return list_error.Trim();
        }
        private void Load_check_box(string last_PN)
        {
            string strQry = "select ROW_NUMBER() OVER(ORDER BY (a.id_number)) as Stt,a.*,     \n ";
            strQry += "        case      \n ";
            strQry += "        when b.product_quantity>0 then 'OK'     \n ";
            strQry += "        else 'WAIT'     \n ";
            strQry += "        end as Result     \n ";
            strQry += "        from   \n ";
            strQry += " (select label_code as id_number,product_customer_code,product_quantity,plan_date from P_Label     \n ";
            strQry += "        where 1=1     \n ";
            strQry += "        and pallet_no is null   \n ";
            strQry += "        and place in ('Waiting Zone','FG Zone','Packing Zone')     \n ";
            strQry += "        and product_customer_code=N'" + last_PN + "') a \n ";
            strQry += " left join \n ";
            strQry += " (select label_code,product_quantity from TEMP_W_ShippingInfor  \n ";
            strQry += " where invoice_no=N'" + lbInvNo.Text + "' and product_customer_code=N'" + last_PN + "') b \n ";
            strQry += " on a.id_number=b.label_code \n ";
            conn = new CmCn();
            
            dgvCheck.DataSource = conn.ExcuteDataTable(strQry);
        }
        private void Load_check_pallet(string last_PN)
        {
            string strQry = "select ROW_NUMBER() OVER(ORDER BY (a.id_number)) as Stt,a.*,     \n ";
            strQry += "        case      \n ";
            strQry += "        when b.Qty>0 then 'OK'     \n ";
            strQry += "        else 'WAIT'     \n ";
            strQry += "        end as Result     \n ";
            strQry += "        from   \n ";
            strQry += " (select pallet_no as id_number, min(plan_date) as plan_date,max(product_customer_code) as product_customer_code from P_Label  \n ";
            strQry += "        where 1=1     \n ";
            strQry += "        and pallet_no not in ('')   \n ";
            strQry += "        and place in ('Waiting Zone','FG Zone','Packing Zone')     \n ";
            strQry += "        and product_customer_code=N'"+ last_PN + "' \n ";
            strQry += "        group by pallet_no) a \n ";
            strQry += " left join \n ";
            strQry += " (select pallet_no,sum(product_quantity) as Qty from TEMP_W_ShippingInfor  \n ";
            strQry += " where invoice_no=N'"+lbInvNo.Text+"' and product_customer_code=N'"+ last_PN + "' \n ";
            strQry += " group by pallet_no) b \n ";
            strQry += " on a.id_number=b.pallet_no \n ";
            conn = new CmCn();
            dgvCheck.DataSource = conn.ExcuteDataTable(strQry);
        }


        private void Load_Invoice()
        {
            string strQry = "select std.invoice_no,std.product_customer_code,std.qty,actual.actual_qty, \n ";
            strQry += " case \n ";
            strQry += "     when actual.actual_qty > std.qty then 'fail' \n ";
            strQry += "     when actual.actual_qty = std.qty then 'ok' \n ";
            strQry += "     else 'wait' \n ";
            strQry += " end as [Status] \n ";
            strQry += " from  \n ";
            strQry += " (select a.invoice_no,b.product_customer_code,b.quantity as qty from LOG_Invoice a,LOG_InvoiceDetail b \n ";
            strQry += " where a.invoice_no='" + lbInvNo.Text + "' and a.invoice_no=b.invoice_no) as std \n ";
            strQry += " left join \n ";
            strQry += " (select product_customer_code,sum(product_quantity) as actual_qty from TEMP_W_ShippingInfor where invoice_no=N'" + lbInvNo.Text + "'\n ";
            strQry += " group by product_customer_code) as actual \n ";
            strQry += " on std.product_customer_code = actual.product_customer_code \n ";
            conn = new CmCn();
            DataTable dt2 = conn.ExcuteDataTable(strQry);
            dgvInfo.DataSource = dt2;
        }
        private void Load_invoice_infor()
        {
            List_PN = new List<string>();
            adoClass = new ADO();
            DataTable dt = adoClass.Load_Invoice_Detail("product_customer_code", "invoice_no=N'" + lbInvNo.Text + "'");
            if (dt.Rows.Count>0)
            {
                foreach (DataRow item in dt.Rows)
                {
                    string PN = item["product_customer_code"].ToString();
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
            dt_temp_box = adoClass.Load_TEMP_W_ShippingInfor("", "invoice_no=N'" + lbInvNo.Text + "' and isActive=N'0' and truck_no=N'" + truck_no + "'");
            dgvActual.DataSource = dt_temp_box;
            //lbQtyBox.Text = dt_temp_box.Rows.Count.ToString();
            var result = from rows in dt_temp_box.AsEnumerable()
                         where rows.Field<string>("pallet_no") == ""
                         select rows;
            var result2 = from rows in dt_temp_box.AsEnumerable()
                         where rows.Field<string>("pallet_no") != ""
                         group rows by new { pallet_no = rows["pallet_no"] } into grp
                         select grp;
            lbQtyBox.Text = result.Count().ToString();
            lbQtyPallet.Text = result2.Count().ToString();
        }
        private void RemoveDataPallet(string pallet_code)
        {
            string strQry = "delete from TEMP_W_ShippingInfor where pallet_no=N'" + pallet_code + "' and invoice_no=N'" + lbInvNo.Text + "' and isActive=N'0'";
            conn = new CmCn();
            conn.ExcuteQry(strQry);
        }
        private void RemoveData(string label_code)
        {
            string strQry = "delete from TEMP_W_ShippingInfor where label_code=N'"+label_code+ "' and invoice_no=N'"+lbInvNo.Text+"' and isActive=N'0'";
            conn = new CmCn();
            conn.ExcuteQry(strQry);
        }
        private void InsertData(string label_code,string kind)
        {
            adoClass = new ADO();
            DataTable dt2=adoClass.Load_TEMP_W_ShippingInfor("label_code","label_code=N'"+ label_code + "'");
            if (dt2.Rows.Count>0)
            {
                lbError.Text = label_code + ": TEM ĐÃ ĐƯỢC THÊM VÀO DANH SÁCH CHỜ/ LABEL HAS BEEN ALREADY ADDED IN LIST";
            }
            else
            {
                DataTable dt = adoClass.Load_Label_FG_Data("label_code,product_code,product_customer_code,product_quantity,pallet_no,lot_no,wh_location,location_packed,isLock,place,date_input_wh", "label_code=N'" + label_code + "'");
                if (dt.Rows[0]["date_input_wh"].ToString() == "")
                {
                    lbError.Text = label_code + ": LỖI TEM CHƯA QUÉT TẠI CỬA KHO/ ERROR: LABEL HAS NOT BEEN SCANNED IN WAITING AREA";
                }
                else if (dt.Rows[0]["place"].ToString() == "Shipped")
                {
                    lbError.Text = label_code + ": THÙNG HÀNG ĐÃ ĐƯỢC SHIP/ ERROR: THE BOX HAS BEEN SHIPPED ALREADY";
                }
                else
                {
                    try
                    {
                        Current_Label.Label_code = label_code;
                        Current_Label.Product_code = dt.Rows[0]["product_code"].ToString();
                        Current_Label.Product_customer_code = dt.Rows[0]["product_customer_code"].ToString();
                        last_PN = Current_Label.Product_customer_code;
                        Current_Label.IsLock = dt.Rows[0]["isLock"].ToString();
                        Current_Label.Product_quantity = int.Parse(dt.Rows[0]["product_quantity"].ToString());
                        Current_Label.Lot_no = dt.Rows[0]["lot_no"].ToString();
                        if (Current_Label.IsLock == "Block")
                        {
                            lbError.Text = label_code + ": LỖI TEM BỊ KHÓA TRÊN HỆ THỐNG/ ERROR: LABEL HAS BEEN LOCKED";
                        }
                        else
                        {
                            //---------check PN in Invoice or not------------
                            var check_Invoice_PN = List_PN.Where(c => c == Current_Label.Product_customer_code);
                            if (check_Invoice_PN.Count() > 0)
                            {
                                //--- check over qty
                                if (!Check_Over_qty(Current_Label.Product_customer_code, Current_Label.Product_quantity))
                                {
                                    lbError.Text = label_code + ": LỖI HÀNG " + Current_Label.Product_customer_code + " VƯỢT QUÁ SỐ LƯỢNG/ ERROR: OVER QTY";
                                }
                            }
                            else
                            {
                                lbError.Text = label_code + ":MÃ HÀNG " + Current_Label.Product_customer_code + " KHÔNG CÓ TRONG INVOICE/ PN NOT IN THE LIST OF INVOICE";
                            }
                        }
                        if (kind== "Shipping the pallet")
                        {
                            Current_Label.Pallet_no = dt.Rows[0]["pallet_no"].ToString();
                        }
                        
                        
                        if (dt.Rows[0]["location_packed"].ToString() == "")
                        {
                            Current_Label.Wh_location = dt.Rows[0]["wh_location"].ToString();
                        }
                        else
                        {
                            Current_Label.Wh_location = dt.Rows[0]["location_packed"].ToString();
                        }
                        Current_Label.Ship_date = DateTime.Now;
                        Current_Label.Ship_op = txtOperator.Text;
                        Current_Label.Invoice_no = lbInvNo.Text;
                        Current_Label.Truck_no = truck_no;
                        Current_Label.Place = "Shipped";
                        Current_Label.Note = kind;
                        Current_Label.Error = lbError.Text;
                        if (lbError.Text == "")
                        {
                            Current_Label.Check = true;
                        }
                        else
                        {
                            Current_Label.Check = false;
                        }
                        if (lbError.Text=="")
                        {
                            adoClass.Insert_TEMP_W_ShippingInfor(Current_Label);
                        }
                    }
                    catch (Exception ex)
                    {
                        lbError.Text = ex.Message;
                    }
                }
            }
        }
        private void InserDataPallet(string pallet_code)
        {
            adoClass = new ADO();
            DataTable dt = adoClass.Load_Label_FG_Data("label_code,product_customer_code", "pallet_no=N'" + pallet_code + "'");
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow item in dt.Rows)
                {
                    string label_code = item["label_code"].ToString();
                    InsertData(label_code, "Shipping the pallet");
                }
            }
            else
            {
                lbError.Text = "PALLET KHONG TON TAI/ THE PALLET IS NOT EXIST";
            }
        }
        private void frmWHScanReceptionArea_Load(object sender, EventArgs e)
        {
            List_PN = new List<string>();
            dt_temp_box = new DataTable();
            Load_invoice_infor();
            Load_Actual_Data();
            Load_Invoice();
        }
        private bool Check_Over_qty(string PN,int add_qty)
        {
            string strQry = "declare @t_qty int=(select quantity from LOG_InvoiceDetail where invoice_no='"+ lbInvNo.Text + "' and product_customer_code=N'"+ PN + "'); \n ";
            strQry += " declare @a_qty int = \n ";
            strQry += " (select sum(product_quantity) as actual_qty  \n ";
            strQry += " from TEMP_W_ShippingInfor  \n ";
            strQry += " where invoice_no='" + lbInvNo.Text + "' and product_customer_code=N'" + PN + "' \n ";
            strQry += " group by invoice_no,truck_no); \n ";
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
            //GridView view = sender as GridView;
            //if (e.RowHandle >= 0)
            //{
            //    string status = view.GetRowCellValue(e.RowHandle, view.Columns["is_check"]).ToString();
            //    if (status == "False")
            //    {
            //        e.Appearance.BackColor = Color.Red;
            //    }
            //    else if (status == "True")
            //    {
            //        e.Appearance.BackColor = Color.Chartreuse;
            //    }
            //}
        }
        private bool Check_before_confirm()
        {
            bool check = true;
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
                ckUnship.Text = "BỎ HÀNG KHỎI XE CÔNG/ REMOVE PRODUCT FROM CONTAINER";
                txtBarcode.Focus();
            }
            else
            {
                ckUnship.Text = "THÊM HÀNG LÊN XE CÔNG/ LOAD PRODUCT TO CONTAINER";
                txtBarcode.Focus();
            }
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
                string error_box = Check_box_error();
                string error_pallet = Check_Pallet_error();
                if (error_box == "" && error_pallet == "")
                {
                    this.Close();
                }
                else
                {
                    lbError.Text = "LỖI FIFO CÁC MÃ HÀNG SAU: Box:" + error_box + "| Pallet:" + error_pallet;
                }
            }
            else
            {
                lbError.Text = "CÓ HÀNG LỖI HOẶC THỪA TRÊN XE/ SHOULD FINISH ALL PRODUCT BEFORE CONFIRM";
            }
        }

        private void gvActual_DoubleClick(object sender, EventArgs e)
        {
            string error = gvActual.GetRowCellValue(gvActual.FocusedRowHandle, "error").ToString();
            if (error!="")
            {
                MessageBox.Show("LỖI THÙNG LÀ: " + error);
            }
        }

        private void gvActual_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            GridView view = sender as GridView;
            if (e.Column == view.Columns["is_check"])
            {
                string status = view.GetRowCellValue(e.RowHandle, view.Columns["is_check"]).ToString();
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
    }
}
