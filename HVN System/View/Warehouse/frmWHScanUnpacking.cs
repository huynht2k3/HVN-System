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
    public partial class frmWHScanUnpacking : Form
    {
        public frmWHScanUnpacking()
        {
            InitializeComponent();
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private List<P_Label_Entity> List_Temp_Box;
        private ADO adoClass;
        private P_Label_Entity Current_Label;
        private List<string> List_pallet;
        private void txtBarcode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string QR_Code = txtBarcode.Text.Substring(2, txtBarcode.Text.Length - 2);
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
                else if (QR_Code == "CONFIRM")
                {
                    btnConfirm.PerformClick();
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
                        txtBarcode.Text = "";
                        lbError.Text = "";
                        txtBarcode.Focus();
                    }
                    else if (txtBarcode.Text.Substring(2, 4) == "WHPL")
                    {
                        if (txtOperator.Text != "")
                        {
                            InserDataPallet(txtBarcode.Text);
                            txtBarcode.Text = "";
                            txtBarcode.Focus();
                        }
                        else
                        {
                            lbError.Text = "QUÉT TÊN BẠN TRƯỚC KHI SCAN HÀNG/ SCAN QR CODE OF YOUR NAME BEFORE SCAN FG";
                            txtBarcode.Text = "";
                            txtBarcode.Focus();
                        }
                    }
                    else
                    {
                        if (txtOperator.Text != "")
                        {
                            InsertData(txtBarcode.Text);
                            txtBarcode.Text = "";
                            lbError.Text = "";
                            txtBarcode.Focus();
                        }
                        else
                        {
                            lbError.Text = "QUÉT TÊN BẠN TRƯỚC KHI SCAN HÀNG/ SCAN QR CODE OF YOUR NAME BEFORE SCAN FG";
                            txtBarcode.Text = "";
                            txtBarcode.Focus();
                        }
                    }
                }
            }
        }
        int Qty_FG = 0;
        private void InserDataPallet(string barcode)
        {

            string pallet_code = barcode.Substring(2, barcode.Length - 2);
            List_pallet.Add(pallet_code);
            adoClass = new ADO();
            DataTable dt = adoClass.Load_Label_FG_Data("label_code,product_code,product_customer_code,product_quantity,date_input_wh,lot_no,wh_location,location_packed", "pallet_no=N'" + pallet_code + "' and place not in ('','Shipped')");
            if (dt.Rows.Count > 0)
            {
                int Qty_FG = 0;
                foreach (DataRow item in dt.Rows)
                {
                    P_Label_Entity box = new P_Label_Entity();
                    box.Stt = (List_Temp_Box.Count + 1).ToString();
                    box.Label_code = item["label_code"].ToString();
                    //box.Pallet_no = pallet_code;
                    box.Lot_no = item["lot_no"].ToString();
                    box.Product_code = item["product_code"].ToString();
                    box.Product_customer_code = item["product_customer_code"].ToString();
                    box.Product_quantity = int.Parse(item["product_quantity"].ToString());
                    //if (string.IsNullOrEmpty(item["location_packed"].ToString()))
                    //{
                    //    box.Wh_location = dt.Rows[0]["wh_location"].ToString();
                    //}
                    //else
                    //{
                    //    box.Wh_location = dt.Rows[0]["location_packed"].ToString();
                    //}
                    box.Wh_op_locate = txtOperator.Text;
                    box.Place = "Waiting Zone";
                    box.Note = "Unpacking from the pallet";
                    List_Temp_Box.Add(box);
                    Qty_FG += box.Product_quantity;
                }
                dgvInfo.DataSource = List_Temp_Box.ToList();
                lbQtyBox.Text = List_Temp_Box.Count.ToString();
                lbQtyFG.Text = Qty_FG.ToString();
                txtBarcode.Text = "";
                lbError.Text = "";
                txtBarcode.Focus();
            }
            else
            {
                lbError.Text = barcode + ": LỖI HÀNG CHƯA ĐƯỢC ĐÓNG VÀO PALLET HOẶC ĐÃ SHIP/ THE BOX HAS NOT BEEN PACKED YET OR SHIPPED";
                txtBarcode.Text = "";
                txtBarcode.Focus();
            }
        }
        private void InsertData(string barcode)
        {
            string label_code = barcode.Substring(2, barcode.Length - 2);
            adoClass = new ADO();
            DataTable dt = adoClass.Load_Label_FG_Data("label_code,product_code,product_customer_code,product_quantity,scanned_date,lot_no,wh_location,location_packed,pallet_no", "label_code=N'" + label_code + "' and place not in ('','Shipped')");
            if (dt.Rows.Count > 0)
            {
                if (string.IsNullOrEmpty(dt.Rows[0]["pallet_no"].ToString()))
                {
                    lbError.Text = barcode + ": LỖI HÀNG CHƯA ĐƯỢC ĐÓNG VÀO PALLET/ THE BOX HAS NOT BEEN PACKED YET";
                }
                else
                {
                    try
                    {
                        Current_Label = new P_Label_Entity();
                        Current_Label.Stt = (List_Temp_Box.Count + 1).ToString();
                        Current_Label.Label_code = dt.Rows[0]["label_code"].ToString();
                        Current_Label.Product_code = dt.Rows[0]["product_code"].ToString();
                        Current_Label.Product_customer_code = dt.Rows[0]["product_customer_code"].ToString();
                        Current_Label.Product_quantity = int.Parse(dt.Rows[0]["product_quantity"].ToString());
                        Current_Label.Lot_no = dt.Rows[0]["lot_no"].ToString();
                        Current_Label.Pallet_no = dt.Rows[0]["pallet_no"].ToString();
                        if (string.IsNullOrEmpty(dt.Rows[0]["location_packed"].ToString()))
                        {
                            Current_Label.Wh_location = dt.Rows[0]["wh_location"].ToString();
                        }
                        else
                        {
                            Current_Label.Wh_location = dt.Rows[0]["location_packed"].ToString();
                        }
                        Current_Label.Wh_op_locate = txtOperator.Text;
                        Current_Label.Place = "Waiting Zone";
                        Current_Label.Note = "Unpacking from the pallet";
                        List_pallet.Add(dt.Rows[0]["pallet_no"].ToString());
                        List_Temp_Box.Add(Current_Label);
                        dgvInfo.DataSource = List_Temp_Box.ToList();
                        lbQtyBox.Text = List_Temp_Box.Count.ToString();
                        Qty_FG = Qty_FG + Current_Label.Product_quantity;
                        lbQtyFG.Text = Qty_FG.ToString();
                    }
                    catch (Exception ex)
                    {
                        lbError.Text = ex.Message;
                    }
                }
            }
            else
            {
                lbError.Text = barcode + ": LỖI HÀNG ĐÃ SHIP/ THE BOX HAS BEEN SHIPPED";
                txtBarcode.Text = "";
                txtBarcode.Focus();
            }
        }

        private void frmWHScanReceptionArea_Load(object sender, EventArgs e)
        {
            List_Temp_Box = new List<P_Label_Entity>();
            List_pallet = new List<string>();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            List_Temp_Box = new List<P_Label_Entity>();
            List_pallet = new List<string>();
            dgvInfo.DataSource = List_Temp_Box.ToList();
            lbQtyBox.Text = "0";
            lbQtyFG.Text = "0";
            Qty_FG = 0;
            lbError.Text = "";
            txtBarcode.Text = "";
            txtBarcode.Focus();
        }

        private void frmWHScanReceptionArea_FormClosing(object sender, FormClosingEventArgs e)
        {
            frmMain frm = new frmMain();
            frm.Show();
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            adoClass = new ADO();
            adoClass.Update_time_Unpacking(List_Temp_Box);
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
            }
            btnClear.PerformClick();
        }
    }
}
