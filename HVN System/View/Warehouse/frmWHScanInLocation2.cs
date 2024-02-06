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
using Outlook = Microsoft.Office.Interop.Outlook;
using DevExpress.XtraSplashScreen;
using HVN_System.View.Admin;

namespace HVN_System.View.Warehouse
{
    public partial class frmWHScanInLocation2 : Form
    {
        public frmWHScanInLocation2()
        {
            InitializeComponent();
        }
        private ObservableCollection<P_Label_Entity> List_Temp_Box;
        private ADO adoClass;
        private P_Label_Entity Current_Label;
        private List<string> List_Location;
        bool isPallet;
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
                else if (QR_Code == "SCAN WAITING ZONE")
                {
                    frmWHScaninWaitingZone frm = new frmWHScaninWaitingZone();
                    frm.Show();
                    this.Hide();
                }
                else if (QR_Code == "CHANGINGLC")
                {
                    frmWHScanInLocationToPacking frm = new frmWHScanInLocationToPacking();
                    frm.Show();
                    this.Hide();
                }
                else if (QR_Code == "REPACKING")
                {
                    frmWHScanInPackingZone2 frm = new frmWHScanInPackingZone2();
                    frm.Show();
                    this.Hide();
                }
                else if (QR_Code == "UNPACKING")
                {
                    frmWHScanUnpacking frm = new frmWHScanUnpacking();
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
                    if (txtBarcode.Text.Substring(2, 4) == "WHFG")
                    {
                        string location = txtBarcode.Text.Substring(4, txtBarcode.Text.Length - 4);
                        if (Check_Location(location))
                        {
                            if (lbLocation.Text == "")
                            {
                                lbLocation.Text = location;
                                List_Location.Add(location);
                            }
                            else
                            {
                                if (lbLocation.Text != txtBarcode.Text.Substring(4, txtBarcode.Text.Length - 4))
                                {
                                    lbError.Text = "BẠN CẦN XÁC THỰC CHO VỊ TRÍ HIỆN TẠI TRƯỚC KHI CHUYỂN VỊ TRÍ KHÁC/ PLEASE CONFIRM BEFORE CHANGE THE LOCATION";
                                }
                            }
                        }
                        else
                        {
                            lbError.Text = "VỊ TRÍ '" + location + "' KHÔNG TỒN TẠI/ LOCATION '" + location + "' IS NOT EXIST";
                        }

                    }
                    else if (txtBarcode.Text.Substring(2, 4) == "WHOP")
                    {
                        txtOperator.Text = txtBarcode.Text.Substring(6, txtBarcode.Text.Length - 6);
                    }
                    else if (txtBarcode.Text.Substring(2, 4) == "WHPL")
                    {
                        if (lbLocation.Text != "" && txtOperator.Text != "")
                        {
                            InserDataPallet(txtBarcode.Text);
                            isPallet = true;
                        }
                        else
                        {
                            lbError.Text = "QUÉT VỊ TRÍ VÀ TÊN BẠN TRƯỚC KHI SCAN HÀNG/ SCAN QR CODE OF LOCATION AND YOUR NAME BEFORE SCAN FG";
                        }
                    }
                    else
                    {
                        if (lbLocation.Text != "" && txtOperator.Text != "")
                        {
                            InsertData(txtBarcode.Text);
                        }
                        else
                        {
                            lbError.Text = "QUÉT VỊ TRÍ VÀ TÊN BẠN TRƯỚC KHI SCAN HÀNG/ SCAN QR CODE OF LOCATION AND YOUR NAME BEFORE SCAN FG";
                        }
                    }
                }
                txtBarcode.Text = "";
                txtBarcode.Focus();
                if (lbError.Text == "")
                {
                    lbQtyBox.BackColor = SystemColors.Control;
                    lbLocation.BackColor = SystemColors.Control;
                }
                else
                {
                    lbQtyBox.BackColor = Color.Red;
                    lbLocation.BackColor = Color.Red;
                }
            }
        }

        private bool Check_Location(string location)
        {
            adoClass = new ADO();
            DataTable dt = adoClass.Load_W_MasterList_Location("", "loc_name=N'" + location + "' and place=N'FG Zone'");
            try
            {
                if (dt.Rows.Count >= 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
        private void InserDataPallet(string barcode)
        {

            string pallet_code = barcode.Substring(2, barcode.Length - 2);
            string pallet_check;
            adoClass = new ADO();
            var check_exist = List_Temp_Box.FirstOrDefault(x => x.Pallet_no == pallet_code);
            if (check_exist == null)
            {
                pallet_check = "";
            }
            else
            {
                pallet_check = check_exist.Pallet_no;
            }
            if (string.IsNullOrEmpty(pallet_check))
            {
                DataTable dt = adoClass.Load_Label_FG_Data("label_code,product_code,product_customer_code,product_quantity,date_input_wh,lot_no,location_packed,wh_location", "pallet_no=N'" + pallet_code + "'");
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow item in dt.Rows)
                    {
                        P_Label_Entity box = new P_Label_Entity();
                        box.Stt = (List_Temp_Box.Count + 1).ToString();
                        box.Label_code = item["label_code"].ToString();
                        box.Pallet_no = pallet_code;
                        box.Product_code = item["product_code"].ToString();
                        box.Product_customer_code = item["product_customer_code"].ToString();
                        box.Product_quantity = int.Parse(item["product_quantity"].ToString());
                        box.Wh_location = lbLocation.Text;
                        box.Wh_op_locate = txtOperator.Text;
                        if (item["location_packed"].ToString() == "")
                        {
                            box.Note = "Packing zone to FG zone";
                        }
                        else
                        {
                            box.Note = "Relocate the pallet";
                        }
                        box.Lot_no = item["lot_no"].ToString();
                        box.Place = "FG Zone";
                        List_Temp_Box.Add(box);
                        List_Location.Add(item["wh_location"].ToString());
                    }
                    dgvInfo.DataSource = List_Temp_Box.ToList();
                    lbQtyBox.Text = List_Temp_Box.Count.ToString();
                }
                else
                {
                    lbError.Text = "CHỌN SAI, CHỌN ĐỔI VỊ TRÍ TRONG KHO THÀNH PHẨM/ SELECT RELOCATE IN FG ZONE";
                }
            }
            else
            {
                lbError.Text = barcode + ": PALLET ĐÃ ĐƯỢC THÊM VÀO DANH SÁCH CHỜ/ PALLET HAS BEEN ALREADY ADDED IN LIST";
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
            }
            else
            {
                adoClass = new ADO();
                DataTable dt = adoClass.Load_Label_FG_Data("label_code,product_code,product_customer_code,product_quantity,date_input_wh,wh_op_locate,lot_no,wh_location,place", "label_code=N'" + label_code + "' and date_input_wh not in ('')");
                if (dt.Rows.Count > 0)
                {
                    try
                    {
                        if (dt.Rows[0]["place"].ToString() == "Shipped")
                        {
                            lbError.Text = barcode + ": THÙNG HÀNG ĐÃ ĐƯỢC SHIP/ ERROR: THE BOX HAS BEEN SHIPPED ALREADY";
                        }
                        else
                        {
                            Current_Label.Stt = (List_Temp_Box.Count + 1).ToString();
                            Current_Label.Label_code = dt.Rows[0]["label_code"].ToString();
                            Current_Label.Product_code = dt.Rows[0]["product_code"].ToString();
                            Current_Label.Product_customer_code = dt.Rows[0]["product_customer_code"].ToString();
                            Current_Label.Product_quantity = int.Parse(dt.Rows[0]["product_quantity"].ToString());
                            Current_Label.Lot_no = dt.Rows[0]["lot_no"].ToString();
                            Current_Label.Wh_location = lbLocation.Text;
                            Current_Label.Wh_op_locate = txtOperator.Text;
                            Current_Label.Place = "FG Zone";
                            if (dt.Rows[0]["wh_op_locate"].ToString() == "")
                            {
                                Current_Label.Note = "Waiting zone to FG zone";
                            }
                            else
                            {
                                Current_Label.Note = "Relocate the box";
                            }
                            List_Temp_Box.Add(Current_Label);
                            List_Location.Add(dt.Rows[0]["wh_location"].ToString());
                            dgvInfo.DataSource = List_Temp_Box.ToList();
                            lbQtyBox.Text = List_Temp_Box.Count.ToString();
                        }
                    }
                    catch (Exception ex)
                    {
                        lbError.Text = ex.Message;
                    }
                }
                else
                {
                    lbError.Text = barcode + ": LỖI TEM CHƯA QUÉT TẠI CỬA KHO/ ERROR: LABEL HAS NOT BEEN SCANNED IN WAITING AREA";
                }
            }
        }

        private void frmWHScanReceptionArea_Load(object sender, EventArgs e)
        {
            List_Temp_Box = new ObservableCollection<P_Label_Entity>();
            List_Location = new List<string>();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            List_Temp_Box = new ObservableCollection<P_Label_Entity>();
            List_Location = new List<string>();
            dgvInfo.DataSource = List_Temp_Box.ToList();
            lbLocation.Text = "";
            lbQtyBox.Text = "0";
            lbError.Text = "";
            isPallet = false;
            txtBarcode.Text = "";
            txtBarcode.Focus();
        }

        private void frmWHScanInLocation_FormClosing(object sender, FormClosingEventArgs e)
        {
            frmMain frm = new frmMain();
            frm.Show();
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (List_Temp_Box.Count > 0)
            {
                adoClass = new ADO();
                adoClass.Update_time_to_WH_Location(List_Temp_Box, isPallet);
                if (lbError.Text == "")
                {
                    SplashScreenManager.ShowForm(this, typeof(frmWaitingForm), true, true, false);
                    SplashScreenManager.Default.SetWaitFormCaption("Printing label...\nĐang in tem...");
                    //---------
                    var actual_list_location = List_Location.GroupBy(x => x).Select(x => x);
                    foreach (var item in actual_list_location)
                    {
                        if (!string.IsNullOrEmpty(item.Key))
                        {
                            adoClass.Print_Location_Label(item.Key, txtOperator.Text);
                        }
                    }
                    //---------
                    SplashScreenManager.CloseForm();
                    if (lbError.Text == "")
                    {
                        btnClear.PerformClick();
                        frmNotification frm = new frmNotification("XÁC NHẬN THÀNH CÔNG \nCONFIRM SUCCESSFULLY", "notification", 5);
                        frm.ShowDialog();
                    }
                }
            }
            else
            {
                frmNotification frm = new frmNotification("XÁC NHẬN THÀNH CÔNG \nCONFIRM SUCCESSFULLY", "notification", 5);
                frm.ShowDialog();
            }
        }

        private void btnManualPrint_Click(object sender, EventArgs e)
        {
            SplashScreenManager.ShowForm(this, typeof(frmWaitingForm), true, true, false);
            SplashScreenManager.Default.SetWaitFormCaption("Printing label...\nĐang in tem...");
            //---------
            adoClass.Print_Location_Label(lbLocation.Text, txtOperator.Text) ;
            //---------
            SplashScreenManager.CloseForm();
            if (lbError.Text == "")
            {
                btnClear.PerformClick();
                frmNotification frm = new frmNotification("IN THÀNH CÔNG \nPRINT SUCCESSFULLY", "notification", 5);
                frm.ShowDialog();
            }
        }
    }
}
