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
    public partial class frmWHCCFGZone : Form
    {
        public frmWHCCFGZone()
        {
            InitializeComponent();
        }
        public frmWHCCFGZone(W_CycleCount_Entity _Cc, DataTable dt_partial, string _PIC)
        {
            InitializeComponent();
            txtCCName.Text = _Cc.Cc_name;
            txtCCType.Text = _Cc.Cc_type;
            dt_Parital = dt_partial;
            txtPIC.Text = _PIC;
        }
        private ObservableCollection<P_Label_Entity> List_Temp_Box;
        private ADO adoClass;
        //private CmCn conn;
        private W_CycleCountInventory_Entity Current_Label;
        private DataTable dt_Parital;
        private string place = "FG Zone";
        private string list_Parital = "";
        private void txtBarcode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode==Keys.Enter)
            {
                lbError.Text = "";
                string QR_Code = txtBarcode.Text.Substring(2, txtBarcode.Text.Length-2);
                if (QR_Code == "CLEAR")
                {
                    btnReCC.PerformClick();
                }
                else
                {
                    if (txtBarcode.Text.Length>=6)
                    {
                        if (txtBarcode.Text.Substring(2, 4) == "WHOP")
                        {
                            txtPIC.Text = txtBarcode.Text.Substring(6, txtBarcode.Text.Length - 6);
                        }
                        else if(txtBarcode.Text.Substring(2, 4) == "WHPL")
                        {
                            if (txtPIC.Text != "" && lbLocation.Text!="")
                            {
                                InserDataPallet(QR_Code);
                            }
                            else
                            {
                                lbError.Text = "THIẾU THÔNG TIN TÊN NHÂN VIÊN HOẶC VỊ TRÍ/ MISSING PIC NAME OR LOCATION";
                            }
                        }
                        else if (txtBarcode.Text.Substring(2, 4) == "WHFG")
                        {
                            string location = txtBarcode.Text.Substring(4, txtBarcode.Text.Length - 4);
                            if (Check_Location(location))
                            {
                                lbLocation.Text = location;
                            }
                            else
                            {
                                lbError.Text = "VỊ TRÍ '" + location + "' KHÔNG TỒN TẠI/ LOCATION '" + location + "' IS NOT EXIST";
                            }
                        }
                        else
                        {
                            if (txtPIC.Text != "" && lbLocation.Text != "")
                            {
                                InsertData(QR_Code);
                            }
                            else
                            {
                                lbError.Text = "THIẾU THÔNG TIN TÊN NHÂN VIÊN HOẶC VỊ TRÍ/ MISSING PIC NAME OR LOCATION";
                            }
                        }
                    }
                    else
                    {
                        lbError.Text = txtBarcode.Text +": KHÔNG KIỂM TRA ĐƯỢC TEM/ CANNOT RECOGNIZE THE QR CODE";
                    }
                }
                txtBarcode.Text = "";
                txtBarcode.Focus();
                if (lbError.Text=="")
                {
                    lbQtyBox.BackColor = SystemColors.Control;
                    lbLocation.BackColor= SystemColors.Control;
                }
                else
                {
                    lbQtyBox.BackColor = Color.Red;
                    lbLocation.BackColor = Color.Red;
                }
                Load_current_inventory();
            }
        }
        private bool Check_Location(string location)
        {
            adoClass = new ADO();
            DataTable dt = adoClass.Load_W_MasterList_Location("", "loc_name=N'" + location + "'");
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
        private void Load_current_inventory()
        {
            adoClass = new ADO();
            DataTable dt_info = adoClass.Load_W_CycleCountInventory("", "cc_name = N'" + txtCCName.Text + "' and place =N'"+place+"'");
            dgvInfo.DataSource = dt_info;
            lbQtyBox.Text = dt_info.Rows.Count.ToString();
        }
        private void InsertData(string label_code)
        {
            adoClass = new ADO();
            string condition = "";
            if (txtCCType.Text == "Partial cycle count")
            {
                condition += "\n and product_customer_code in (" + list_Parital + ")";
            }
            DataTable dt = adoClass.Load_Label_FG_Data("label_code,product_customer_code,product_quantity,pallet_no,plan_date", "label_code=N'" + label_code + "'"+condition);
            if (dt.Rows.Count>0)
            {
                Current_Label = new W_CycleCountInventory_Entity();
                Current_Label.Cc_name = txtCCName.Text;
                Current_Label.Label_code = label_code;
                Current_Label.Wh_location = lbLocation.Text;
                Current_Label.Pallet_no = dt.Rows[0]["pallet_no"].ToString();
                Current_Label.Place = place;
                Current_Label.PIC = txtPIC.Text;
                Current_Label.Last_time_commit = DateTime.Now;
                Current_Label.Product_customer_code = dt.Rows[0]["product_customer_code"].ToString();
                Current_Label.Product_quantity = dt.Rows[0]["product_quantity"].ToString();
                Current_Label.Plan_date = string.IsNullOrEmpty(dt.Rows[0]["plan_date"].ToString())?DateTime.Today.AddYears(50):DateTime.Parse(dt.Rows[0]["plan_date"].ToString());
                DataTable dt_check = adoClass.Load_W_CycleCountInventory("label_code", "cc_name=N'" + txtCCName.Text + "' and label_code=N'" + label_code + "'");
                try
                {
                    if (dt_check.Rows.Count == 0)
                    {
                        adoClass.Insert_W_CycleCountInventory(Current_Label);
                    }
                    else
                    {
                        lbError.Text += label_code + ":TEM ĐÃ ĐƯỢC KIỂM KÊ/ LABEL WAS SCANNED ALREADY \n";
                    }
                }
                catch (Exception ex)
                {
                    lbError.Text += label_code + ": " + ex.Message + "\n";
                }
            }
            else
            {
                if (txtCCType.Text == "Partial cycle count")
                {
                    lbError.Text += label_code + ": TEM KHÔNG NĂM TRONG DS KIỂM KÊ/ LABEL IS NOT IN THE LIST CYCLE COUNT \n";
                }
                else
                {
                    lbError.Text += label_code + ": KHÔNG TÌM THẤY THÔNG TIN TEM/ CANNOT CHECK INFORMATION OF LABEL \n";
                }
            }
        }

        private void frmWHScanReceptionArea_Load(object sender, EventArgs e)
        {
            lbLocation.Text = "";
            txtBarcode.Focus();
            Load_current_inventory();
            List_Temp_Box = new ObservableCollection<P_Label_Entity>();
            if (txtCCType.Text == "Partial cycle count")
            {
                try
                {
                    foreach (DataRow item in dt_Parital.Rows)
                    {
                        if (string.IsNullOrEmpty(list_Parital))
                        {
                            list_Parital += "'" + item["PART NUMBER"].ToString()+"'";
                        }
                        else
                        {
                            list_Parital += ",'" + item["PART NUMBER"].ToString() + "'";
                        }
                    }
                    cboPartial.Properties.DataSource = dt_Parital;
                    cboPartial.Properties.DisplayMember = "PART NUMBER";
                    cboPartial.Properties.ValueMember = "PART NUMBER";
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
            else
            {
                layoutControlItem16.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            }
        }

        private void btnReCC_Click(object sender, EventArgs e)
        {
            frmWHCCRemoveItem frm = new frmWHCCRemoveItem(txtCCName.Text, place);
            frm.ShowDialog();
            Load_current_inventory();
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
