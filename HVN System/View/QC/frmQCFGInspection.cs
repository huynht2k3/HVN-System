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
using HVN_System.View.QC;

namespace HVN_System.View.Warehouse
{
    public partial class frmQCFGInspection : Form
    {
        public frmQCFGInspection()
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
                else
                {
                    if (txtBarcode.Text.Substring(2, 4) == "QCOP")
                    {
                        txtOperator.Text = txtBarcode.Text.Substring(6, txtBarcode.Text.Length - 6);
                    }
                    else
                    {
                        if (txtOperator.Text != "")
                        {
                            if (cboTypeResult.Text=="")
                            {
                                lbError.Text = "LỖI CHƯA CHỌN LOẠI KẾT QUẢ SAU KIỂM";
                            }
                            else
                            {
                                InsertData(QR_Code);
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
        private void InsertData(string label_code)
        {
            adoClass = new ADO();
            
            DataTable dt = adoClass.Load_Label_FG_Data("", "label_code=N'" + label_code + "'");
            if (dt.Rows.Count > 0)
            {
                try
                {
                    if (dt.Rows[0]["place"].ToString() == "Shipped")
                    {
                        lbError.Text = "THÙNG HÀNG ĐÃ ĐƯỢC SHIP/ ERROR: THE BOX HAS BEEN SHIPPED ALREADY";
                    }
                    else if (dt.Rows[0]["patrol_result"].ToString() != "")
                    {
                        lbError.Text = "THÙNG HÀNG ĐÃ ĐƯỢC KIỂM TRA";
                    }
                    else
                    {
                        DateTime applied_date = new DateTime(2023, 12,8);
                        if (DateTime.Parse(dt.Rows[0]["created_date"].ToString())> applied_date)
                        {
                            if (dt.Rows[0]["scanned_date"].ToString() == "")
                            {
                                lbError.Text = "THÙNG HÀNG " + label_code + " CHƯA ĐƯỢC SẢN XUẤT SCAN";
                            }
                        }
                        if (lbError.Text=="")
                        {
                            Current_Label = new P_Label_Entity();
                            Current_Label.Stt = (List_Temp_Box.Count + 1).ToString();
                            Current_Label.Label_code = dt.Rows[0]["label_code"].ToString();
                            Current_Label.Product_code = dt.Rows[0]["product_code"].ToString();
                            Current_Label.Product_customer_code = dt.Rows[0]["product_customer_code"].ToString();
                            Current_Label.Product_quantity = int.Parse(dt.Rows[0]["product_quantity"].ToString());
                            Current_Label.Lot_no = dt.Rows[0]["lot_no"].ToString();
                            Current_Label.Check_type = dt.Rows[0]["check_type"].ToString();
                            Current_Label.Op_input_wh = txtOperator.Text;
                            Current_Label.Place = "QC Area";
                            Current_Label.Note = "QC INSPECTION:GP12:" + cboTypeResult.SelectedValue.ToString();
                            Current_Label.Patrol_result = cboTypeResult.SelectedValue.ToString();
                            if (cboTypeResult.Text == "1 PHẦN THÙNG OK")
                            {
                                frmQCFGInspectionNGPart frm = new frmQCFGInspectionNGPart(Current_Label);
                                frm.ShowDialog();
                            }
                            else if (cboTypeResult.Text == "TOÀN BỘ THÙNG NG")
                            {
                                string strQry = "delete from QC_FG_NGPart where label_code=N'" + Current_Label.Label_code + "' and CAST(time_qc_check AS DATE)=N'" + DateTime.Today.ToString("yyyy-MM-dd") + "'\n";
                                strQry += "insert into QC_FG_NGPart ([label_code],[product_customer_code],[product_name],[product_quantity],[plan_date],[lot_no],[pic_qc],[time_qc_check],[ng_others])\n";
                                strQry += "select N'" + Current_Label.Label_code + "',N'" + Current_Label.Product_customer_code + "',N'" + Current_Label.Product_name + "',N'" + Current_Label.Product_quantity.ToString() +
                                    "',N'" + Current_Label.Plan_date.ToString("yyyy-MM-dd") + "',N'" + Current_Label.Lot_no + "',N'" + Current_Label.Op_input_wh + "',getdate(),N'" + Current_Label.Product_quantity + "'";
                                conn = new CmCn();
                                conn.ExcuteQry(strQry);
                            }
                            adoClass = new ADO();
                            adoClass.Update_time_Inspection(Current_Label);
                            var Check = List_Temp_Box.Where(x => x.Label_code == Current_Label.Label_code);
                            foreach (P_Label_Entity item in Check)
                            {
                                List_Temp_Box.Remove(item);
                                Qty_FG = Qty_FG - item.Product_quantity;
                            }
                            List_Temp_Box.Add(Current_Label);
                            dgvInfo.DataSource = List_Temp_Box.ToList();
                            lbQtyBox.Text = List_Temp_Box.Count.ToString();
                            Qty_FG = Qty_FG + Current_Label.Product_quantity;
                            lbQtyFG.Text = Qty_FG.ToString();
                            string strQry2 = "select plan_date, [shift],product_customer_code, \n ";
                            strQry2 += " sum(product_quantity) as qty, count(label_code) as number_box \n ";
                            strQry2 += " from P_Label \n ";
                            strQry2 += " where patrol_date>=N'" + DateTime.Today.ToString("yyyy-MM-dd") + "' \n ";
                            strQry2 += " group by plan_date, [shift],product_customer_code \n ";
                            strQry2 += " order by product_customer_code \n ";
                            conn = new CmCn();
                            dgvResult.DataSource = conn.ExcuteDataTable(strQry2);
                        }
                    }
                }
                catch (Exception ex)
                {
                    lbError.Text = ex.Message;
                }
            }
            else
            {
                lbError.Text = label_code + ": THÙNG HÀNG KHÔNG TỒN TẠI";
            }
        }

        private void frmWHScanReceptionArea_Load(object sender, EventArgs e)
        {
            List_Temp_Box = new ObservableCollection<P_Label_Entity>();
            Load_combobox();
            string strQry2 = "select plan_date, [shift],product_customer_code, \n ";
            strQry2 += " sum(product_quantity) as qty, count(label_code) as number_box \n ";
            strQry2 += " from P_Label \n ";
            strQry2 += " where patrol_date>=N'" + DateTime.Today.ToString("yyyy-MM-dd") + "' \n ";
            strQry2 += " group by plan_date, [shift],product_customer_code \n ";
            strQry2 += " order by product_customer_code \n ";

            conn = new CmCn();
            dgvResult.DataSource = conn.ExcuteDataTable(strQry2);
        }
        private void Load_combobox()
        {
            adoClass = new ADO();
            DataTable dt= adoClass.Load_Parameter_Detail("child_des,child_name", "parent_id='qc_type_result'");
            cboTypeResult.DataSource = dt;
            cboTypeResult.DisplayMember = "child_des";
            cboTypeResult.ValueMember = "child_name";
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
           
        }

        private void cboTypeResult_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtBarcode.Focus();
        }

        private void dgvInfo_Click(object sender, EventArgs e)
        {

        }
    }
}
