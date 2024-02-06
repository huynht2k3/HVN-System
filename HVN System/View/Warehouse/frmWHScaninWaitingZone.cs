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
    public partial class frmWHScaninWaitingZone : Form
    {
        public frmWHScaninWaitingZone()
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
        private CmCn conn;
        private P_Label_Entity Current_Label;
        private List<string> list_FGToRM;
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
                    else if (txtBarcode.Text.Substring(2, 4) == "WHMR")
                    {
                        if (txtOperator.Text != "")
                        {
                            Return_Material(QR_Code);
                        }
                        else
                        {
                            lbError.Text = "QUÉT TÊN BẠN TRƯỚC KHI SCAN HÀNG/ SCAN QR CODE OF YOUR NAME BEFORE SCAN FG";
                        }
                    }
                    else
                    {
                        if (txtOperator.Text != "")
                        {
                            InsertData(txtBarcode.Text);
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
        private void Return_Material(string QRCode)
        {
            adoClass = new ADO();
            DataTable dt = adoClass.Load_W_M_ReceiveLabel("", "place=N'QC Area' and whmr_code=N'"+QRCode+"'");
            if (dt.Rows.Count>0)
            {
                Current_Label = new P_Label_Entity();
                Current_Label.Stt = (List_Temp_Box.Count + 1).ToString();
                Current_Label.Label_code = dt.Rows[0]["whmr_code"].ToString();
                Current_Label.Product_customer_code = dt.Rows[0]["m_name"].ToString();
                Current_Label.Product_quantity = int.Parse(dt.Rows[0]["quantity"].ToString());
                List_Temp_Box.Add(Current_Label);
                string strQry = "update W_M_ReceiveLabel set place=N'WH Material' where whmr_code=N'"+QRCode+"'\n";
                strQry += "insert into W_M_HistoryOfTransaction([whmr_code],[m_name],[quantity],[transaction],[input_time],[PIC],[place]) \n";
                strQry += " select N'" + QRCode + "', N'" + Current_Label.Product_customer_code + "', N'" + Current_Label.Product_quantity + "', N'QC transfer to WH',";
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
                lbError.Text = "LỖI NGUYÊN VẬT LIỆU Ở NGOÀI KHO QC/ ERROR MATERIAL NOT COME FROM QC AREA";
            }
        }
        private void Load_list_FGToRM()
        {
            list_FGToRM = new List<string>();
            string strQry = "select product_customer_code from P_MasterListProduct where product_kind=N'RM'";
            conn = new CmCn();
            DataTable dt = conn.ExcuteDataTable(strQry);
            foreach (DataRow item in dt.Rows)
            {
                string PN = item["product_customer_code"].ToString();
                list_FGToRM.Add(PN);
            }
        }
        private void InsertData(string barcode)
        {
            string label_code = barcode.Substring(2, barcode.Length - 2);

            adoClass = new ADO();
            DataTable dt = adoClass.Load_Label_FG_Data("", "label_code=N'" + label_code + "' and date_input_wh is null \n");
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["patrol_result"].ToString() != "Checked OK full")
                {
                    lbError.Text = barcode + ": LỖI HÀNG CHƯA ĐƯỢC QC KIỂM/ ERROR: LABEL HAS NOT BEEN CHECKED BY QC";
                }
                else
                {
                    try
                    {
                        if (dt.Rows[0]["place"].ToString() == "Shipped")
                        {
                            lbError.Text = barcode + ": THÙNG HÀNG ĐÃ ĐƯỢC SHIP/ ERROR: THE BOX HAS BEEN SHIPPED ALREADY";
                        }
                        else if (dt.Rows[0]["isLock"].ToString() == "Block")
                        {
                            lbError.Text = barcode + ": THÙNG HÀNG ĐÃ BỊ KHÓA BỞI QC/ ERROR: THE BOX HAS BEEN BLOCKED BY QC";
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
                            Current_Label.Patrol_date = DateTime.Parse(dt.Rows[0]["patrol_date"].ToString());
                            Current_Label.Patrol_op = dt.Rows[0]["patrol_op"].ToString();
                            Current_Label.Plan_date = DateTime.Parse(dt.Rows[0]["plan_date"].ToString());
                            string strQry = "select label_code from W_HistoryOfTransaction where label_code=N'" + label_code + "' and [transaction] like N'%Waiting zone'";
                            conn = new CmCn();
                            DataTable dt_3 = conn.ExcuteDataTable(strQry);
                            if (dt_3.Rows.Count > 0)
                            {
                                Current_Label.Note = "[QC recheck] go to Waiting zone";
                            }
                            else
                            {
                                if (dt.Rows[0]["product_type"].ToString() == "Repack")
                                {
                                    Current_Label.Note = "[Repack] go to Waiting zone";
                                }
                                else
                                {
                                    Current_Label.Note = "[New product] go to Waiting zone";
                                }
                            }
                            var check_RM = list_FGToRM.FirstOrDefault(x => x == Current_Label.Product_customer_code);
                            if (check_RM == null)
                            {
                                Current_Label.Place = "Waiting Zone";
                            }
                            else
                            {
                                Current_Label.Place = "";
                                Insert_FGtoRM();
                            }
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
                lbError.Text = label_code + ": TEM ĐÃ VÀO KHO HOẶC KHÔNG TỒN TẠI/ LABEL ALREADY IN WH OR NOT EXIST";
            }
        }
        private int Generate_RM_code()
        {
            string Qry = "SELECT MAX(whmr_code) FROM W_M_ReceiveLabel ";
            conn = new CmCn();
            DataTable dt = conn.ExcuteDataTable(Qry);
            if (dt.Rows[0][0].ToString() != "")
            {
                string max_value = dt.Rows[0][0].ToString().Substring(4, dt.Rows[0][0].ToString().Length - 4);
                return int.Parse(max_value) + 1;
            }
            else
            {
                return 10001;
            }
        }
        private void Insert_FGtoRM()
        {
            string RM_code = "WHMR" + Generate_RM_code();
            string strQry = "insert into W_M_ReceiveLabel (whmr_code,m_name,quantity,lot_no,created_time, \n ";
            strQry += " created_user,wh_op,wh_receive_time,wh_okng,place,pic_qc,time_qc_check,qc_okng,rm_doc_id) \n ";
            strQry += " select N'"+ RM_code + "',N'"+Current_Label.Product_customer_code+ "',N'" + Current_Label.Product_quantity + "',N'" + Current_Label.Plan_date.ToString("yyyy-MM-dd") + "' \n ";
            strQry += " ,getdate(),N'" + General_Infor.username + "',N'" + Current_Label.Op_input_wh + "',getdate() \n ";
            strQry += " ,N'OK',N'WH Material',N'" + Current_Label.Patrol_op + "',N'" + Current_Label.Patrol_date.ToString("yyyy-MM-dd HH:mm:ss") + "' \n ";
            strQry += " ,N'OK',N'FG to RM' \n ";
            strQry += " insert into [W_M_HistoryOfTransaction] ([whmr_code],[m_name],[quantity],[transaction],[input_time],[PIC],[invoice_no],[truck_no]) \n ";
            strQry += " select N'" + RM_code + "',N'" + Current_Label.Product_customer_code + "',N'" + Current_Label.Product_quantity +
                "',N'Entry WH from PD',getdate(),N'" + Current_Label.Patrol_op + "',N'FG to RM',N'' \n ";
            conn = new CmCn();
            conn.ExcuteQry(strQry);
            W_M_ReceiveLabel_Entity item = new W_M_ReceiveLabel_Entity();
            item.Whmr_code = RM_code;
            item.M_name = Current_Label.Product_customer_code; 
            item.Quantity = Current_Label.Product_quantity;
            item.Created_date = DateTime.Now;
            item.Lot_no = Current_Label.Plan_date;
            item.Time_qc_check = Current_Label.Patrol_date;
            adoClass = new ADO();
            adoClass.Print_W_M_ReceiveLabel(item, "QCOK");
        }
        private void frmWHScanReceptionArea_Load(object sender, EventArgs e)
        {
            List_Temp_Box = new ObservableCollection<P_Label_Entity>();
            Load_list_FGToRM();
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
