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
    public partial class frmWHRubberIssueToPD : Form
    {
        public frmWHRubberIssueToPD()
        {
            InitializeComponent();
        }
        private List<W_M_RubberLabel_Entity> List_Temp_Pallet;
        private ADO adoClass;
        private CmCn conn;
        private W_M_RubberLabel_Entity Current_Label;
        float Total_weight = 0;
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
        
        private void InsertData(string barcode)
        {
            string label_code = barcode.Substring(2, barcode.Length - 2);
            
            adoClass = new ADO();
            DataTable dt = adoClass.Load_W_M_RubberLabel("*,cast(lot_no as varchar(20)) as current_lot", "whrr_code=N'" + label_code + "' and place=N'WH Rubber' \n");
            

            if (dt.Rows.Count>0)
            {
                Current_Label = new W_M_RubberLabel_Entity();
                Current_Label.Stt = List_Temp_Pallet.Count + 1;
                Current_Label.Whrr_code = dt.Rows[0]["whrr_code"].ToString();
                Current_Label.R_name = dt.Rows[0]["r_name"].ToString();
                Current_Label.Place = "Mixing Area";
                Current_Label.Weight = float.Parse(dt.Rows[0]["weight"].ToString());
                Current_Label.Wh_op = txtOperator.Text;
                List_Temp_Pallet.Add(Current_Label);
                string strQry_check = "select min(lot_no) as Oldest_lot from W_M_RubberLabel where place=N'WH Rubber'and r_name=N'" + Current_Label.R_name + "'";
                conn = new CmCn();
                DataTable dt2 = conn.ExcuteDataTable(strQry_check);
                if (dt2.Rows.Count>0)
                {
                    if (dt.Rows[0]["lot_no"].ToString() != "")
                    {
                        Current_Label.Lot_no = DateTime.Parse(dt.Rows[0]["lot_no"].ToString());
                        DateTime oldest_lot = DateTime.Parse(dt2.Rows[0]["Oldest_lot"].ToString());
                        if (Current_Label.Lot_no != oldest_lot)
                        {
                            lbError.Text = "LỖI THÙNG KHÔNG PHẢI LOT NO CŨ NHẤT " + oldest_lot.ToString("dd/MM/yyyy");
                        }
                    }
                    else
                    {
                        lbError.Text = "LỖI CẤP CAO SU KHÔNG CÓ LOT NO/ PALLET HAS NO LOT NO";
                    }
                }
                else
                {
                    lbError.Text = "LỖI TRONG KHO CAO SU KHÔNG CÓ MÃ CẦN TÌM";
                }
                //--------------------
                if (lbError.Text=="")
                {
                    string strQry = "update W_M_RubberLabel set time_issue_pd=getdate(),pic_issue_pd=N'" + txtOperator.Text + "',place=N'Mixing Area'  \n ";
                    strQry += " where whrr_code=N'" + label_code + "' \n ";
                    strQry += "insert into W_M_RubberTransaction(whrr_code,r_name,weight,lot_no,[transaction],input_time,place,PIC)\n";
                    strQry += "select N'" + Current_Label.Whrr_code + "',N'" + Current_Label.R_name + "',N'" + Current_Label.Weight + "',N'" + Current_Label.Lot_no.ToString("yyyy-MM-dd") +
                            "',N'Issue rubber to PD',getdate(),N'WH Material',N'" + Current_Label.Wh_op + "'\n";
                    try
                    {
                        conn = new CmCn();
                        conn.ExcuteQry(strQry);
                    }
                    catch (Exception ex)
                    {
                        lbError.Text = ex.Message;
                    }
                    dgvInfo.DataSource = List_Temp_Pallet.ToList();
                    lbQtyBox.Text = List_Temp_Pallet.Count.ToString();
                    Total_weight = Total_weight + Current_Label.Weight;
                    lbQtyFG.Text = Total_weight.ToString();
                }
            }
            else
            {
                lbError.Text = label_code + ": PALLET KHÔNG CÓ TRONG KHO/ PALLET NOT EXIST IN WH";
            }
        }

        private void frmWHScanReceptionArea_Load(object sender, EventArgs e)
        {
            List_Temp_Pallet = new List<W_M_RubberLabel_Entity>();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            List_Temp_Pallet = new List<W_M_RubberLabel_Entity>();
            dgvInfo.DataSource = List_Temp_Pallet.ToList();
            lbQtyBox.Text = "0";
            lbQtyFG.Text = "0";
            Total_weight = 0;
            lbError.Text = "";
        }
        private void frmWHScanReceptionArea_FormClosing(object sender, FormClosingEventArgs e)
        {
            frmMain frm = new frmMain();
            frm.Show();
        }

    }
}
