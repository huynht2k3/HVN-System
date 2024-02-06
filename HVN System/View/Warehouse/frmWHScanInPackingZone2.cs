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
using DevExpress.XtraSplashScreen;
using HVN_System.View.Admin;
using DevExpress.XtraGrid.Views.Grid;

namespace HVN_System.View.Warehouse
{
    public partial class frmWHScanInPackingZone2 : Form
    {
        public frmWHScanInPackingZone2()
        {
            InitializeComponent();
        }
        private List<P_Label_Entity> List_Temp_Box;
        private ADO adoClass;
        private CmCn conn;
        private P_Label_Entity Current_Label;
        private P_Label_Entity last_Label;
        private List<string> List_pallet;
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
                else if (QR_Code == "REPACKING")
                {
                    if (ckRepacked.Checked)
                    {
                        ckRepacked.Checked = false;
                    }
                    else
                    {
                        ckRepacked.Checked = true;
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
                    //else if (txtBarcode.Text.Substring(2, 4) == "WHPL")
                    //{
                    //    if (ckRepacked.Checked)
                    //    {
                    //        InserDataPallet(txtBarcode.Text);
                    //    }
                    //    else
                    //    {
                    //        lbError.Text = "CHỌN SAI, VUI LÒNG CHỌN REPACKING/ SELECT INCORRECT, PLEASE SCAN QR UNPACKING";
                    //    }
                    //}
                    else
                    {
                        if (txtOperator.Text.Trim() != "" && lbPalletNo.Text != "")
                        {
                            InsertData(txtBarcode.Text);
                        }
                        else
                        {
                            if (ckRepacked.Checked)
                            {
                                lbError.Text = "BẠN CẦN QUÉT TÊN NHÂN VIÊN VÀ PALLET KHI ĐÓNG LẠI PALLET/ PLEASE SCAN QR CODE OF OPERATOR AND PALLET BEFORE REPACKING FG";
                            }
                            else
                            {
                                lbError.Text = "BẠN CẦN QUÉT TÊN NHÂN VIÊN KHI ĐÓNG PALLET MỚI/ PLEASE SCAN QR CODE OF OPERATOR PACKING FG";
                            }
                        }
                    }
                }
                txtBarcode.Text = "";
                txtBarcode.Focus();
                if (lbError.Text == "")
                {
                    lbQtyBox.BackColor = SystemColors.Control;
                    lbPalletNo.BackColor = SystemColors.Control;
                }
                else
                {
                    lbQtyBox.BackColor = Color.Red;
                    lbPalletNo.BackColor = Color.Red;
                }
            }
        }
        private void InsertData(string barcode)
        {
            string label_code = barcode.Substring(2, barcode.Length - 2);
            string label_check;
            var check_exist = List_Temp_Box.FirstOrDefault(x => x.Label_code == label_code);
            if (check_exist == null)
            {
                label_check = "";
            }
            else
            {
                label_check = check_exist.Label_code;
            }
            if (!string.IsNullOrEmpty(label_check))
            {
                lbError.Text = barcode + ": TEM ĐÃ ĐƯỢC THÊM VÀO DANH SÁCH CHỜ/ LABEL HAS BEEN ALREADY ADDED IN LIST";
                txtBarcode.Text = "";
                txtBarcode.Focus();
            }
            else
            {
                adoClass = new ADO();
                DataTable dt = adoClass.Load_Label_FG_Data("", "label_code=N'" + label_code + "'");
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["date_input_wh"].ToString()=="")
                    {
                        lbError.Text = barcode + ": LỖI TEM CHƯA QUÉT TẠI CỬA KHO/ ERROR: LABEL HAS NOT BEEN SCANNED IN WAITING AREA";
                    }
                    else if (dt.Rows[0]["place"].ToString() == "Shipped")
                    {
                        lbError.Text = barcode + ": THÙNG HÀNG ĐÃ ĐƯỢC SHIP/ ERROR: THE BOX HAS BEEN SHIPPED ALREADY";
                    }
                    else if (dt.Rows[0]["pallet_no"].ToString() != "")
                    {
                        lbError.Text = barcode + ": THÙNG ĐÃ ĐƯỢC ĐÓNG VÀO PALLET "+ dt.Rows[0]["pallet_no"].ToString() + "/ ERROR: THE BOX HAS BEEN PACKED ALREADY";
                    }
                    else if (dt.Rows[0]["isLock"].ToString() == "Block")
                    {
                        lbError.Text = barcode + ": THÙNG HÀNG ĐÃ BỊ KHÓA, KHÔNG THỂ ĐÓNG PALLET/ ERROR: THE BOX HAS BEEN LOCKED";
                    }
                    else
                    {
                        try
                        {
                            Current_Label.Stt = (List_Temp_Box.Count + 1).ToString();
                            Current_Label.Label_code = dt.Rows[0]["label_code"].ToString();
                            Current_Label.Product_code = dt.Rows[0]["product_code"].ToString();
                            Current_Label.Product_customer_code = dt.Rows[0]["product_customer_code"].ToString();
                            Current_Label.Date_input_packing_zone = DateTime.Now;
                            Current_Label.Product_quantity = int.Parse(dt.Rows[0]["product_quantity"].ToString());
                            Current_Label.Pallet_no = lbPalletNo.Text;
                            Current_Label.Lot_no = dt.Rows[0]["lot_no"].ToString();
                            Current_Label.Op_input_packing_zone = txtOperator.Text;
                            Current_Label.Place = "Packing Zone";
                            Current_Label.Wh_location = dt.Rows[0]["wh_location"].ToString();
                            if (dt.Rows[0]["pallet_no"].ToString() == "")
                            {
                                Current_Label.Note = "Packing the pallet";
                            }
                            else
                            {
                                Current_Label.Note = "Repacking the pallet";
                                List_pallet.Add(dt.Rows[0]["pallet_no"].ToString());
                            }
                            if (last_Label.Product_customer_code!=Current_Label.Product_customer_code&& last_Label.Product_customer_code!=null)
                            {
                                if (!Check_Result(last_Label.Product_customer_code))
                                {
                                    lbError.Text = "LỖI CHƯA CHO THÙNG CŨ NHẤT CỦA " + last_Label.Product_customer_code + " VÀO PALLET. KHÔNG THỂ CHUYỂN SANG MÃ MỚI";
                                }
                            }
                            if (lbError.Text=="")
                            {
                                List_pallet.Add(lbPalletNo.Text);
                                List_Temp_Box.Add(Current_Label);
                                string strQry2 = "insert into TEMP_W_FG_PackingPallet (label_code,product_customer_code,product_quantity)\n";
                                strQry2 += "select N'" + Current_Label.Label_code + "',N'" + Current_Label.Product_customer_code + "',N'" + Current_Label.Product_quantity + "' \n";
                                //string strQry = "select ROW_NUMBER() OVER(ORDER BY (SELECT 100)) as Stt,a.*,    \n ";
                                //strQry += "      case     \n ";
                                //strQry += "      when b.product_quantity>0 then 'OK'    \n ";
                                //strQry += "      else 'WAIT'    \n ";
                                //strQry += "      end as Result    \n ";
                                //strQry += "      from    \n ";
                                //strQry += "          (select label_code,product_customer_code,product_quantity,plan_date from P_Label    \n ";
                                //strQry += "      where 1=1    \n ";
                                //strQry += "      and pallet_no is null    \n ";
                                //strQry += "      and place in ('Waiting Zone','FG Zone','Packing Zone')    \n ";
                                //strQry += "      and product_customer_code=N'" + Current_Label.Product_customer_code + "') a    \n ";
                                //strQry += "      left join (select * from TEMP_W_FG_PackingPallet where product_customer_code=N'" + Current_Label.Product_customer_code + "') b    \n ";
                                //strQry += "      on a.label_code=b.label_code  \n ";

                                string strQry = "select ROW_NUMBER() OVER(ORDER BY plan_date,label_code) as Stt, \n ";
                                strQry += "    kq.* \n ";
                                strQry += "    from \n ";
                                strQry += "    (select a.*,   \n ";
                                strQry += "    case    \n ";
                                strQry += "    when b.product_quantity>0 then 'OK'   \n ";
                                strQry += "    else 'WAIT'   \n ";
                                strQry += "    end as Result   \n ";
                                strQry += "    from   \n ";
                                strQry += "        (select label_code,product_customer_code,product_quantity,plan_date from P_Label   \n ";
                                strQry += "    where 1=1   \n ";
                                strQry += "    and pallet_no is null   \n ";
                                strQry += "    and isLock = 'Unblock'   \n ";
                                strQry += "    and place in ('Waiting Zone','FG Zone','Packing Zone')   \n ";
                                strQry += "    and product_customer_code=N'" + Current_Label.Product_customer_code + "') a   \n ";
                                strQry += "    left join (select * from TEMP_W_FG_PackingPallet where product_customer_code=N'" + Current_Label.Product_customer_code + "') b   \n ";
                                strQry += "    on a.label_code=b.label_code  \n ";
                                strQry += "    ) kq \n ";
                                conn = new CmCn();
                                conn.ExcuteQry(strQry2);
                                dgvChecking.DataSource = conn.ExcuteDataTable(strQry);
                                dgvInfo.DataSource = List_Temp_Box.ToList();
                                lbQtyBox.Text = List_Temp_Box.Count.ToString();
                                last_Label = Current_Label;
                            }
                        }
                        catch (Exception ex)
                        {
                            lbError.Text = ex.Message;
                        }
                    }
                }
                else
                {
                    lbError.Text = barcode + ": TEM SAI HOẶC BỊ LỖI/ WRONG LABEL OR BROKEN";
                    txtBarcode.Text = "";
                    txtBarcode.Focus();
                }
            }
        }
        private bool Check_Result (string PN) //kiem tra xe co lien tiep tu dau hay khong
        {
            string strQry = "select   \n ";
            strQry += "     case  \n ";
            strQry += "     when count(abc.Stt)=max(abc.Stt) then 'OK'  \n ";
            strQry += "     else 'NOK'  \n ";
            strQry += "     end as Double_check  \n ";
            strQry += "   from   \n ";
            strQry += " ( \n ";
            strQry += "    select ROW_NUMBER() OVER(ORDER BY plan_date,Result) as Stt, \n ";
            strQry += "    kq.* \n ";
            strQry += "    from \n ";
            strQry += "    (select a.*,   \n ";
            strQry += "    case    \n ";
            strQry += "    when b.product_quantity>0 then 'OK'   \n ";
            strQry += "    else 'WAIT'   \n ";
            strQry += "    end as Result   \n ";
            strQry += "    from   \n ";
            strQry += "        (select label_code,product_customer_code,product_quantity,plan_date from P_Label   \n ";
            strQry += "    where 1=1   \n ";
            strQry += "    and pallet_no is null   \n ";
            strQry += "    and isLock = 'Unblock'   \n ";
            strQry += "    and place in ('Waiting Zone','FG Zone','Packing Zone')   \n ";
            strQry += "    and product_customer_code=N'"+PN+"') a   \n ";
            strQry += "    left join (select * from TEMP_W_FG_PackingPallet where product_customer_code=N'" + PN + "') b   \n ";
            strQry += "    on a.label_code=b.label_code  \n ";
            strQry += "    ) kq \n ";
            strQry += " ) abc  \n ";
            strQry += "    where abc.Result='OK'  \n ";
            strQry += "   \n ";

            conn = new CmCn();
            string Result = conn.ExcuteString(strQry);
            if (Result=="OK")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
       
        private void frmWHScanReceptionArea_Load(object sender, EventArgs e)
        {
            List_Temp_Box = new List<P_Label_Entity>();
            List_pallet = new List<string>();
            last_Label = new P_Label_Entity();
            Create_Pallet_Code();
        }
        private void Create_Pallet_Code()
        {
            string Qry = "delete from TEMP_W_FG_PackingPallet \n";
            Qry += "SELECT MAX([pallet_no]) FROM P_Label ";
            conn = new CmCn();
            DataTable dt = conn.ExcuteDataTable(Qry);
            int so_chay;
            if (dt.Rows[0][0].ToString() != "")
            {
                string max_value = dt.Rows[0][0].ToString().Substring(4, dt.Rows[0][0].ToString().Length - 4);
                so_chay = int.Parse(max_value) + 1;
            }
            else
            {
                so_chay = 10001;
            }
            lbPalletNo.Text = "WHPL" + so_chay.ToString();
        }
        private void btnClear_Click(object sender, EventArgs e)
        {
            List_Temp_Box = new List<P_Label_Entity>();
            List_pallet = new List<string>();
            last_Label = new P_Label_Entity();
            dgvInfo.DataSource = List_Temp_Box.ToList();
            if (ckRepacked.Checked)
            {
                lbPalletNo.Text = "";
            }
            else
            {
                Create_Pallet_Code();
            }
            dgvChecking.DataSource = null;
            txtBarcode.Text = "";
            lbQtyBox.Text = "0";
            lbError.Text = "";
            txtBarcode.Focus();
            /*
            var packing_infor = List_Temp_Box.GroupBy(x => x.Product_customer_code).Select(x=>x);
            string hienthi="";
            foreach (var item in packing_infor)
            {
                hienthi += item.Key.ToString()+":\t";
                int qty=0;
                foreach (P_Label_Entity row in item)
                {
                    qty += row.Product_quantity;
                }
                hienthi += qty.ToString() + "(pcs)\n";
            }
            MessageBox.Show(hienthi);
            */
            //Print_Packing_list(List_Temp_Box);
        }

        private void frmWHScanInLocation_FormClosing(object sender, FormClosingEventArgs e)
        {
            frmMain frm = new frmMain();
            frm.Show();
        }
        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            Bitmap bmp;
            MessagingToolkit.QRCode.Codec.QRCodeEncoder encoder = new MessagingToolkit.QRCode.Codec.QRCodeEncoder();
            encoder.QRCodeScale = 2;
            bmp = encoder.Encode(lbPalletNo.Text);
            //e.Graphics.DrawImage(bmp, 0, 0);
            int y = 0;//Bắt đầu ngang
            int x = 0;//Bắt đầu dọc

            e.Graphics.DrawImage(bmp, x + 650, y + 8);
            //HEADER
            e.Graphics.DrawString("PACKING DETAIL", new Font("Arial", 30, FontStyle.Bold), Brushes.Black, new Point(x + 200, y + 8));
            e.Graphics.DrawLine(new Pen(Color.Black, 2), x, y + 65, x + 1000, y + 65);
            //PACKING CONTENT
            e.Graphics.DrawString("Pallet No:" + lbPalletNo.Text, new Font("Arial", 30, FontStyle.Bold), Brushes.Black, new Point(x, y + 80));
        }
        private void Repacking_status()
        {
            if (ckRepacked.Checked)
            {
                ckRepacked.Text = "ĐÓNG LẠI PALLET/ REPACKING THE PALLET";
                btnClear.PerformClick();
                //ckRelocate.ForeColor = Color.Black;
            }
            else
            {
                ckRepacked.Text = "ĐÓNG PALLET MỚI/ PACKING NEW PALLET";
                btnClear.PerformClick();
                //ckRelocate.ForeColor = Color.Blue;
            }
        }
        private void ckRepacked_CheckedChanged(object sender, EventArgs e)
        {
            Repacking_status();
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (List_Temp_Box.Count > 0)
            {
                if (Check_Result(Current_Label.Product_customer_code))
                {
                    txtBarcode.Text = "";
                    SplashScreenManager.ShowForm(this, typeof(frmWaitingForm), true, true, false);
                    SplashScreenManager.Default.SetWaitFormCaption("Processing data...");
                    adoClass = new ADO();
                    adoClass.Update_time_to_Packing_zone(List_Temp_Box);
                    //--danh sach pallet da duoc group by
                    var actual_list_pallet = List_pallet.GroupBy(x => x).Select(x => x);
                    foreach (var item in actual_list_pallet)
                    {
                        adoClass.Print_Pallet_Label(item.Key, txtOperator.Text);
                    }
                    if (lbError.Text == "")
                    {
                        frmNotification frm = new frmNotification("XÁC NHẬN THÀNH CÔNG \nCONFIRM SUCCESSFULLY", "notification", 5);
                        frm.ShowDialog();
                        btnClear.PerformClick();
                    }
                    
                    SplashScreenManager.CloseForm();
                }
                else
                {
                    lbError.Text = "LỖI CHƯA LẤY CÁC THÙNG CŨ NHẤT ĐÓNG HÀNG. KIỂM TRA BẢNG BÊN.";
                }
            }
        }

        private void btnManualPrint_Click(object sender, EventArgs e)
        {
            SplashScreenManager.ShowForm(this, typeof(frmWaitingForm), true, true, false);
            SplashScreenManager.Default.SetWaitFormCaption("Printing label...\nĐang in tem...");
            //---------
            adoClass.Print_Pallet_Label(lbPalletNo.Text, txtOperator.Text);
            //---------
            SplashScreenManager.CloseForm();
            if (lbError.Text == "")
            {
                btnClear.PerformClick();
                frmNotification frm = new frmNotification("IN THÀNH CÔNG \nPRINT SUCCESSFULLY", "notification", 5);
                frm.ShowDialog();
            }
        }

        private void dgvInfo_Click(object sender, EventArgs e)
        {

        }

        private void gvCheching_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            GridView view = sender as GridView;
            if (e.Column == view.Columns["Result"])
            {
                string status = view.GetRowCellValue(e.RowHandle, view.Columns["Result"]).ToString();
                switch (status)
                {
                    case "OK":
                        e.Appearance.BackColor = Color.Chartreuse;
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
