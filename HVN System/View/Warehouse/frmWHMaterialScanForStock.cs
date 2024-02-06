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
    public partial class frmWHMaterialScanForStock : Form
    {
        public frmWHMaterialScanForStock()
        {
            InitializeComponent();
        }
        private ObservableCollection<P_Label_Entity> List_Temp_Box;
        private ADO adoClass;
        private CmCn conn;
        private P_Label_Entity Current_Label;
        
        private void txtBarcode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode==Keys.Enter)
            {
                lbError.Text = "";
                string QR_Code = txtBarcode.Text.Substring(2, txtBarcode.Text.Length-2);
                if (QR_Code == "CLEAR")
                {
                    btnClear.PerformClick();
                }
                else
                {
                    if (txtBarcode.Text.Substring(2, 4) == "WHOP")
                    {
                        txtOperator.Text = txtBarcode.Text.Substring(6, txtBarcode.Text.Length - 6);
                    }
                    else if (txtBarcode.Text.Substring(2, 4) == "WHMR")
                    {
                        if (txtOperator.Text != "")
                        {
                            Update_Material(QR_Code);
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
        private void Update_Material(string QRCode)
        {
            adoClass = new ADO();
            DataTable dt = adoClass.Load_W_M_ReceiveLabel("", "place is null and whmr_code=N'"+QRCode+"'");
            if (dt.Rows.Count>0)
            {
                Current_Label = new P_Label_Entity();
                Current_Label.Stt = (List_Temp_Box.Count + 1).ToString();
                Current_Label.Label_code = dt.Rows[0]["whmr_code"].ToString();
                Current_Label.Product_customer_code = dt.Rows[0]["m_name"].ToString();
                Current_Label.Product_quantity = int.Parse(dt.Rows[0]["quantity"].ToString());
                List_Temp_Box.Add(Current_Label);
                //string strQry = "update W_M_ReceiveLabel set place=N'WH Material',wh_op=N'"+txtOperator.Text+ "',[wh_receive_time]=getdate(),[wh_okng]=N'OK',[pic_issue_qc]='System'" +
                //    ",[time_issue_qc]=getdate(),[rm_plan_id]=N'',[qc_okng]=N'OK',[pic_qc]=N'System',[time_qc_check]=getdate() where whmr_code=N'" + QRCode+"'\n";
                string strQry = "update W_M_ReceiveLabel set place=N'WH Material' where whmr_code=N'" + QRCode + "'\n";
                strQry += "insert into W_M_HistoryOfTransaction([whmr_code],[m_name],[quantity],[transaction],[input_time],[PIC],[place]) \n";
                strQry += " select N'" + QRCode + "', N'" + Current_Label.Product_customer_code + "', N'" + Current_Label.Product_quantity + "', N'Scan inventory',";
                strQry += "getdate(), N'" + txtOperator.Text + "', N'WH Material'";
                conn = new CmCn();
                conn.ExcuteQry(strQry);
                dgvInfo.DataSource = List_Temp_Box.ToList();
                lbQtyBox.Text = List_Temp_Box.Count.ToString();
                Qty_FG = Qty_FG + Current_Label.Product_quantity;
                lbQtyFG.Text = Qty_FG.ToString();
            }
            else
            {
                lbError.Text = "LỖI MÃ TEM KHÔNG TỒN TẠI HOẶC ĐÃ VÀO KHO/ THE LABEL IS NOT EXIST";
            }
        }
        private void InsertData(string barcode)
        {
            string label_code = barcode.Substring(2, barcode.Length - 2);
            adoClass = new ADO();
            DataTable dt = adoClass.Load_Label_FG_Data("label_code,product_code,product_customer_code,product_quantity,scanned_date,lot_no,place", "label_code=N'" + label_code + "' and date_input_wh is null");
            if (dt.Rows.Count>0)
            {
                if (1==0)//string.IsNullOrEmpty(dt.Rows[0]["scanned_date"].ToString()))
                {
                    //lbError.Text = barcode + ": LỖI TEM CHƯA QUÉT TẠI SX/ ERROR: LABEL HAS NOT BEEN SCANNED BY PD";
                }
                else
                {
                    try
                    {
                        if (dt.Rows[0]["place"].ToString() == "Shipped")
                        {
                            lbError.Text = barcode + ": THÙNG HÀNG ĐÃ ĐƯỢC SHIP/ ERROR: THE BOX HAS BEEN SHIPPED ALREADY";
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
                            Current_Label.Note = "Go to Waiting zone";
                            adoClass = new ADO();
                            adoClass.Update_time_to_Warehouse(Current_Label);
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
            }
            else
            {
                lbError.Text = barcode + ": LỖI TEM ĐÃ SCAN HOẶC TEM LỖI/ LABEL HAS BEEN ALREADY SCANNED";
            }
        }

        private void frmWHScanReceptionArea_Load(object sender, EventArgs e)
        {
            List_Temp_Box = new ObservableCollection<P_Label_Entity>();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            List_Temp_Box = new ObservableCollection<P_Label_Entity>();
            dgvInfo.DataSource = List_Temp_Box.ToList();
            lbQtyBox.Text = "0";
            lbQtyFG.Text = "0";
            Qty_FG = 0;
            lbError.Text = "";
        }

        private void frmWHScanReceptionArea_FormClosing(object sender, FormClosingEventArgs e)
        {
            frmMain frm = new frmMain();
            frm.Show();
        }

    }
}
