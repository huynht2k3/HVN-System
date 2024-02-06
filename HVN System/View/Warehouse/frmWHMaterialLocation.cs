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
    public partial class frmWHMaterialLocation : Form
    {
        public frmWHMaterialLocation()
        {
            InitializeComponent();
        }
        private ObservableCollection<W_M_ReceiveLabel_Entity> List_Temp_Box;
        private ADO adoClass;
        private W_M_ReceiveLabel_Entity Current_Label;
        private List<string> List_Location;
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
                else
                {
                    string scan_id = txtBarcode.Text.Substring(0, 2);
                    Current_Label = new W_M_ReceiveLabel_Entity();
                    if (txtBarcode.Text.Substring(2, 4) == "WHML")
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
            DataTable dt = adoClass.Load_W_MasterList_Location("", "loc_name=N'" + location + "' and place =N'WH Material'");
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
        private void InsertData(string barcode)
        {
            string label_code = barcode.Substring(2, barcode.Length - 2);
            string label_check;
            var check_exist = List_Temp_Box.FirstOrDefault(x => x.Whmr_code == label_code);
            if (check_exist == null)
            {
                label_check = "";
            }
            else
            {
                label_check = check_exist.Whmr_code;
            }
            if (!string.IsNullOrEmpty(label_check))
            {
                lbError.Text = barcode + ": TEM ĐÃ ĐƯỢC THÊM VÀO DANH SÁCH CHỜ/ LABEL HAS BEEN ALREADY ADDED IN LIST";
            }
            else
            {
                adoClass = new ADO();
                DataTable dt = adoClass.Load_W_M_ReceiveLabel("", "whmr_code=N'" + label_code + "' and place =N'WH Material'");
                if (dt.Rows.Count > 0)
                {
                    try
                    {
                        Current_Label.Stt = List_Temp_Box.Count + 1;
                        Current_Label.Whmr_code = dt.Rows[0]["whmr_code"].ToString();
                        Current_Label.M_name = dt.Rows[0]["m_name"].ToString();
                        Current_Label.Quantity = int.Parse(dt.Rows[0]["quantity"].ToString());
                        Current_Label.Wh_location = lbLocation.Text;
                        Current_Label.Op_locate = txtOperator.Text;
                        Current_Label.Place= dt.Rows[0]["place"].ToString();
                        if (dt.Rows[0]["wh_location"].ToString() == "")
                        {
                            Current_Label.Transaction = "Put the box to location";
                        }
                        else
                        {
                            Current_Label.Transaction = "Relocate the box";
                        }
                        List_Temp_Box.Add(Current_Label);
                        List_Location.Add(dt.Rows[0]["wh_location"].ToString());
                        dgvInfo.DataSource = List_Temp_Box.ToList();
                        lbQtyBox.Text = List_Temp_Box.Count.ToString();
                    }
                    catch (Exception ex)
                    {
                        lbError.Text = ex.Message;
                    }
                }
                else
                {
                    lbError.Text = barcode + ": LỖI HÀNG KHÔNG TRONG KHO/ ERROR: THE BOX IS NOT IN WH";
                }
            }
        }

        private void frmWHScanReceptionArea_Load(object sender, EventArgs e)
        {
            List_Temp_Box = new ObservableCollection<W_M_ReceiveLabel_Entity>();
            List_Location = new List<string>();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            List_Temp_Box = new ObservableCollection<W_M_ReceiveLabel_Entity>();
            List_Location = new List<string>();
            dgvInfo.DataSource = List_Temp_Box.ToList();
            lbLocation.Text = "";
            lbQtyBox.Text = "0";
            lbError.Text = "";
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
                adoClass.Update_Material_Location(List_Temp_Box);
                if (lbError.Text == "")
                {
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
                frmNotification frm = new frmNotification("KHÔNG CÓ THÔNG TIN MỚI ĐỂ XÁC NHẬN/ THERE IS NOTHING NEW TO CHANGE", "notification", 5);
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
