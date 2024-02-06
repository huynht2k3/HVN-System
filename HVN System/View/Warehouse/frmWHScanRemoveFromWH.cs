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

namespace HVN_System.View.Warehouse
{
    public partial class frmWHScanRemoveFromWH : Form
    {
        public frmWHScanRemoveFromWH()
        {
            InitializeComponent();
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private ObservableCollection<P_Label_Entity> List_Temp_Box;
        private ADO adoClass;
        private P_Label_Entity Current_Label;
        
        private void txtBarcode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode==Keys.Enter)
            {
                string QR_Code = txtBarcode.Text.Substring(2, txtBarcode.Text.Length-2);
                lbError.Text = "";
                if (QR_Code == "CLEAR")
                {
                    btnClear.PerformClick();
                }
                else if (QR_Code == "RELOCATE")
                {
                    frmWHScanInLocation2 frm = new frmWHScanInLocation2();
                    frm.Show();
                    this.Hide();
                }
                else if (QR_Code == "REPACKING")
                {
                    frmWHScanInPackingZone2 frm = new frmWHScanInPackingZone2();
                    frm.Show();
                    this.Hide();
                }
                else
                {
                    if (txtBarcode.Text.Substring(2, 4) == "WHOP")
                    {
                        txtOperator.Text = txtBarcode.Text.Substring(6, txtBarcode.Text.Length - 6);
                    }
                    else if (txtBarcode.Text.Substring(2, 4) == "WHPL")
                    {
                        InserDataPallet(QR_Code);
                    }
                    else
                    {
                        if (txtOperator.Text != "")
                        {
                            if (cboReason.Text!="")
                            {
                                InsertData(QR_Code);
                            }
                            else
                            {
                                lbError.Text = "LỖI CHƯA CHỌN LÝ DO/ NOT YET SELECTED THE REASON";
                            }
                        }
                        else
                        {
                            lbError.Text = "QUÉT TÊN BẠN TRƯỚC KHI SCAN HÀNG/ SCAN QR CODE OF YOUR NAME BEFORE SCAN FG";
                        }
                    }
                }
                txtBarcode.Text = "";
                txtBarcode.Focus();
                if (lbError.Text=="")
                {
                    lbQtyBox.BackColor = SystemColors.Control;
                    lbQtyFG.BackColor= SystemColors.Control;
                }
                else
                {
                    lbQtyBox.BackColor = Color.Red;
                    lbQtyFG.BackColor = Color.Red;
                }
            }
        }
        int Qty_FG = 0;
        private void InserDataPallet(string pallet_code)
        {
            adoClass = new ADO();
            DataTable dt = adoClass.Load_Label_FG_Data("label_code", "pallet_no=N'" + pallet_code + "'");
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow item in dt.Rows)
                {
                    string label_code = item["label_code"].ToString();
                    InsertData(label_code);
                }
            }
            else
            {
                lbError.Text = "PALLET KHONG TON TAI/ THE PALLET IS NOT EXIST";
            }

        }
        private void InsertData(string label_code)
        {
            adoClass = new ADO();
            DataTable dt = adoClass.Load_Label_FG_Data("label_code,product_code,product_customer_code,product_quantity,lot_no,place", "label_code=N'" + label_code + "' and date_input_wh not in ('')");
            if (dt.Rows.Count>0)
            {
                try
                {
                    if (dt.Rows[0]["place"].ToString() == "Shipped")
                    {
                        lbError.Text = "THÙNG HÀNG ĐÃ ĐƯỢC SHIP/ ERROR: THE BOX HAS BEEN SHIPPED ALREADY";
                    }
                    else
                    {
                        Current_Label = new P_Label_Entity();
                        Current_Label.Stt = (List_Temp_Box.Count + 1).ToString();
                        Current_Label.Label_code = dt.Rows[0]["label_code"].ToString();
                        Current_Label.Product_code = dt.Rows[0]["product_code"].ToString();
                        Current_Label.Product_customer_code = dt.Rows[0]["product_customer_code"].ToString();
                        Current_Label.Product_quantity = int.Parse(dt.Rows[0]["product_quantity"].ToString());
                        Current_Label.Lot_no = dt.Rows[0]["lot_no"].ToString();
                        Current_Label.Op_input_wh = txtOperator.Text;
                        Current_Label.Place = "Waiting Zone";
                        Current_Label.Note = cboReason.SelectedValue.ToString();
                        adoClass = new ADO();
                        adoClass.Update_time_leave_Warehouse(Current_Label);
                        List_Temp_Box.Add(Current_Label);
                        dgvInfo.DataSource = List_Temp_Box.ToList();
                        lbQtyBox.Text = List_Temp_Box.Count.ToString();
                        Qty_FG = Qty_FG + Current_Label.Product_quantity;
                        lbQtyFG.Text = Qty_FG.ToString();
                    }
                }
                catch (Exception ex)
                {
                    lbError.Text = ex.Message;
                }
            }
            else
            {
                lbError.Text = label_code + ": THÙNG KHÔNG CÓ TRONG KHO/ BOX IS NOT IN WH";
            }
        }

        private void frmWHScanReceptionArea_Load(object sender, EventArgs e)
        {
            List_Temp_Box = new ObservableCollection<P_Label_Entity>();
            Load_combobox();
        }
        private void Load_combobox()
        {
            adoClass = new ADO();
            DataTable dt= adoClass.Load_Parameter_Detail("child_des,child_name", "parent_id='wh_reason_return'");
            cboReason.DataSource = dt;
            cboReason.DisplayMember = "child_des";
            cboReason.ValueMember = "child_name";
        }
        private void btnClear_Click(object sender, EventArgs e)
        {
            List_Temp_Box = new ObservableCollection<P_Label_Entity>();
            dgvInfo.DataSource = List_Temp_Box.ToList();
            //cboReason.Text = "TRẢ HÀNG VỀ SẢN XUẤT/ RETURN TO PRODUCTION";
            lbQtyBox.Text = "0";
            lbQtyFG.Text = "0";
            Qty_FG = 0;
        }

        private void frmWHScanReceptionArea_FormClosing(object sender, FormClosingEventArgs e)
        {
            frmMain frm = new frmMain();
            frm.Show();
        }
    }
}
