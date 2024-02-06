using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using HVN_System.Entity;
using System.IO.Ports;
using System.Threading;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraSplashScreen;
using HVN_System.View.Admin;

namespace HVN_System.View.Warehouse
{
    public partial class frmWHMaterialReturnMaterial : Form
    {
        public frmWHMaterialReturnMaterial()
        {
            InitializeComponent();
        }
        public frmWHMaterialReturnMaterial(string PIC)
        {
            InitializeComponent();
            txtPIC.Text = PIC;
        }
        private ADO adoClass;
        private CmCn conn;
        string COM_Port, COM_Port_BigScale;
        float standard_weight;
        SerialPort comport;
        bool isSmallScale = true;
        //bool isChangeScale=true;
        private void frmWHCCManagement_Load(object sender, EventArgs e)
        {
            string[] line = File.ReadAllLines(@"C:\HVN_SYS_CONFIG\Config.txt", Encoding.UTF8);
            COM_Port = line[4].Substring(line[4].LastIndexOf(":") + 1);
            COM_Port_BigScale = line[5].Substring(line[5].LastIndexOf(":") + 1);
            comport = new SerialPort();
            txtBarcode.Focus();
        }

        private void Load_standard_weight()
        {
            adoClass = new ADO();
            try
            {
                DataTable dt = adoClass.Load_W_MasterList_Material("raw_weight/raw_qty as std_w", "m_name=N'" + txtMName.Text + "'");
                string result = dt.Rows[0][0].ToString();
                if (result != "")
                {
                    standard_weight = float.Parse(result);
                }
            }
            catch (Exception)
            {

                MessageBox.Show("Error: Cannot check standard weight \nLỗi không tìm được trọng lượng tiêu chuẩn mã:"+txtMName.Text, "Error");
            }
        }
        private void Receive_Data()
        {
            comport.Close();
            //comport.Parity = (Parity)Enum.Parse(typeof(Parity), Parity);
            comport = new SerialPort();
            if (isSmallScale)
            {
                comport.PortName = COM_Port;
            }
            else
            {
                comport.PortName = COM_Port_BigScale;
            }
            comport.BaudRate = 9600;
            comport.Parity = Parity.None;
            comport.DataBits = 8;
            comport.StopBits = StopBits.One;
            comport.ReceivedBytesThreshold = 1;
            try
            {
                comport.Open();//Mở kết nối
                lbStatus.ForeColor = Color.Empty;
                lbStatus.Text = "Cổng kết nối đã mở!";
            }
            catch
            {
                //MessageBox.Show("Lỗi kết nối!Không thể kết nối tới cân.");
                lbStatus.ForeColor = Color.Red;
                lbStatus.Text = "Lỗi kết nối! Không thể kết nối tới cân.";
            }
            Thread t = new Thread(Load_Weight);
            t.IsBackground = true; // khi tat chuong trinh thi Thread cung tat theo
            t.Start();
            if (isSmallScale)
            {
                Thread t2 = new Thread(Convert_small);
                t2.IsBackground = true;
                t2.Start();
            }
            else
            {
                Thread t2 = new Thread(Convert_big);
                t2.IsBackground = true;
                t2.Start();
            }
        }
        private void Load_Weight()
        {
            //isChangeScale = true;
            while (!ckInputManual.Checked)
            {
                try
                {
                    if (isSmallScale)
                    {
                        Thread.Sleep(100);
                    }
                    txtRawData.Text = comport.ReadLine();
                }
                catch (Exception)
                {

                }
            }
        }

        private void Convert_small()
        {
            while (!ckInputManual.Checked)
            {
                Thread.Sleep(100);
                try
                {
                    if (txtRawData.Text.Length > 3)
                    {
                        txtWeight.Text = float.Parse(txtRawData.Text.Substring(0, txtRawData.Text.Length - 3)).ToString();
                        txtQuantity.Text = Math.Round((float.Parse(txtWeight.Text) / standard_weight), 0).ToString();
                    }
                }
                catch (Exception)
                {

                }
            }
        }
        private void Convert_big()
        {
            while (!ckInputManual.Checked)
            {
                Thread.Sleep(100);
                try
                {
                    if (txtRawData.Text.Length > 4)
                    {
                        try
                        {
                            string test = txtRawData.Text;
                            txtWeight.Text = test.Substring(0, test.Length - 3).Trim();
                            txtQuantity.Text = Math.Round((float.Parse(txtWeight.Text) * 1000 / standard_weight), 0).ToString();
                        }
                        catch (Exception)
                        {

                        }
                        
                    }
                }
                catch (Exception)
                {
                    
                }

            }
        }
        private void frmWHMaterialIssueToPDDetail_FormClosing(object sender, FormClosingEventArgs e)
        {
            comport.Close();
        }

        private void dtpLotNo_ValueChanged(object sender, EventArgs e)
        {
            dtpLotNo.CustomFormat = "dd/MM/yyyy";
        }

        private void ckInputManual_CheckedChanged(object sender, EventArgs e)
        {
            if (ckInputManual.Checked)
            {
                lbStatus.Text = "Input manualy!!!";
                txtRawData.Text = "";
                txtQuantity.ReadOnly = false;
            }
            else
            {
                txtRawData.Text = "";
                txtQuantity.ReadOnly = true;
            }
        }

        private void txtWeight_TextChanged(object sender, EventArgs e)
        {
            if (ckInputManual.Checked)
            {
                if (txtWeight.Text.Length >= 1)
                {
                    if (isSmallScale)
                    {
                        txtQuantity.Text = Math.Round((float.Parse(txtWeight.Text) / standard_weight), 0).ToString();
                    }
                    else
                    {
                        txtQuantity.Text = Math.Round((float.Parse(txtWeight.Text) * 1000 / standard_weight), 0).ToString();
                    }
                }
            }
        }

        private void txtIssueLabelCode_KeyDown(object sender, KeyEventArgs e)
        {
            
        }

        private void txtBarcode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (txtBarcode.Text.Length>6)
                {
                    if (txtBarcode.Text.Substring(2,4)=="WHMR")
                    {
                        MessageBox.Show("ĐÂY LÀ TEM NHẬN LINH KIỆN KHÔNG PHẢI TEM CẤP HÀNG. VUI LÒNG SCAN TEM CẤP HÀNG");
                    }
                    else if (txtBarcode.Text.Substring(2, 4) == "WHMI")
                    {
                        Load_data_label(txtBarcode.Text.Substring(2, txtBarcode.Text.Length - 2));
                    }
                    else
                    {
                        MessageBox.Show("KHÔNG TÌM THẤY THÔNG TIN TEM \nMã tem: "+txtBarcode.Text.Substring(2,txtBarcode.Text.Length-2)+"");
                    }
                }
                else
                {
                    MessageBox.Show("Mã tem '"+txtBarcode.Text+"' không đúng.");
                }
                
            }
        }
        private void Load_data_label( string label_code)
        {
            string strQry2 = "select * from [W_M_IssueLabel] where [whmi_code]=N'" + label_code + "'";
            conn = new CmCn();
            DataTable dt2 = conn.ExcuteDataTable(strQry2);
            if (dt2.Rows.Count>0)
            {
                txtReceiveLabel.Text=dt2.Rows[0]["whmr_code"].ToString();
                string strQry = "select a.m_name,a.quantity \n ";
                strQry += " from W_M_ReceiveLabel a \n ";
                strQry += " where whmr_code=N'"+ txtReceiveLabel.Text + "' \n ";
                DataTable dt = conn.ExcuteDataTable(strQry);
                if (dt.Rows.Count < 1)
                {
                    MessageBox.Show("LỖI THÙNG CÓ MÃ " + txtReceiveLabel.Text + " KHÔNG TỒN TẠI");
                }
                else
                {
                    txtIssueLabel.Text = label_code;
                    txtQtyIssue.Text = dt2.Rows[0]["quantity"].ToString();
                    txtMName.Text = dt2.Rows[0]["m_name"].ToString();
                    txtProdCustCode.Text = dt2.Rows[0]["product_customer_code"].ToString();
                    txtPLine.Text = dt2.Rows[0]["p_line"].ToString();
                    txtPShift.Text = dt2.Rows[0]["p_shift"].ToString();
                    dtpLotNo.Value = DateTime.Parse(dt2.Rows[0]["lot_no"].ToString());
                    dtpSupplyDate.Value = DateTime.Parse(dt2.Rows[0]["supply_date"].ToString());
                    txtQtyInBox.Text = dt.Rows[0]["quantity"].ToString();
                    if (cboScaleType.Text== "NO SCALE/ KHONG DUNG CAN")
                    {
                        txtQuantity.Text= dt2.Rows[0]["quantity"].ToString();
                    }
                    else
                    {
                        Load_standard_weight();
                    }
                }
            }
            else
            {
                
            }
            txtBarcode.Text = "";
            txtBarcode.Focus();
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            txtBarcode.Focus();
        }

        private void txtBarcode_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void dtpSupplyDate_ValueChanged(object sender, EventArgs e)
        {
            dtpSupplyDate.CustomFormat = "dd/MM/yyyy";
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (cboScaleType.Text == "")
            {
                lbNote.Text = "LƯU Ý: BẠN CHƯA CHỌN LOẠI CÂN";
            }
            else 
            {
                if (cboScaleType.Text == "SMALL SCALE/ CAN NHO")
                {
                    isSmallScale = true;
                    comport = new SerialPort();
                    Receive_Data();
                    btnConfirm.Visible = true;
                    btnStart.Visible = false;
                    cboScaleType.Enabled = false;
                }
                else if (cboScaleType.Text == "BIG SCALE/ CAN TO")
                {
                    isSmallScale = false;
                    comport = new SerialPort();
                    Receive_Data();
                    btnConfirm.Visible = true;
                    btnStart.Visible = false;
                    cboScaleType.Enabled = false;
                }
                else
                {
                    txtWeight.Visible = false;
                    txtRawData.Visible = false;
                    btnConfirm.Visible = true;
                    btnStart.Visible = false;
                }
               
            }
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (txtPIC.Text.Trim()=="" || txtIssueLabel.Text != "")
            {
                if (float.Parse(txtQuantity.Text) > float.Parse(txtQtyIssue.Text))
                {
                    frmNotification frm = new frmNotification("Lỗi số lượng trả lại lớn hơn số lượng đã cấp \nError qty issue more than qty return", "Error", 5);
                    frm.ShowDialog();
                    return;
                }
                try
                {
                    if (txtIssueLabel.Text!="")
                    {
                        W_M_IssueLabel_Entity item1 = new W_M_IssueLabel_Entity();
                        item1.Whmr_code = txtReceiveLabel.Text;
                        item1.M_name = txtMName.Text;
                        item1.Lot_no = dtpLotNo.Value;
                        item1.Whmi_code = txtIssueLabel.Text;
                        item1.Quantity = float.Parse(txtQuantity.Text);
                        item1.Pic = txtPIC.Text;
                        item1.Transation = "Return material from Production";
                        float new_quantity = float.Parse(txtQuantity.Text) + float.Parse(txtQtyInBox.Text);
                        string strQry = "select * from W_M_ReceiveLabel where whmr_code=N'" + txtReceiveLabel.Text + "'";
                        conn = new CmCn();
                        DataTable dt = conn.ExcuteDataTable(strQry);
                        W_M_ReceiveLabel_Entity item2 = new W_M_ReceiveLabel_Entity();
                        item2.Whmr_code = dt.Rows[0]["whmr_code"].ToString();
                        item2.M_name = dt.Rows[0]["m_name"].ToString();
                        item2.Quantity = new_quantity;
                        item2.Created_date = string.IsNullOrEmpty(dt.Rows[0]["quantity"].ToString()) ? DateTime.Now : DateTime.Parse(dt.Rows[0]["created_time"].ToString());
                        if (dt.Rows[0]["lot_no"].ToString() == "")
                        {
                            MessageBox.Show("LỖI KHÔNG TÌM ĐƯỢC LOT NO CỦA TEM :" + item2.Whmr_code);
                        }
                        else
                        {
                            if (dt.Rows[0]["lot_no"].ToString() != "")
                            {
                                item2.Lot_no = DateTime.Parse(dt.Rows[0]["lot_no"].ToString());
                                if (dt.Rows[0]["time_qc_check"].ToString() != "")
                                {
                                    item2.Time_qc_check = DateTime.Parse(dt.Rows[0]["time_qc_check"].ToString());
                                }
                            }
                            adoClass = new ADO();
                            adoClass.Return_W_M_IssueLabel_2(item1, new_quantity);
                            SplashScreenManager.ShowForm(this, typeof(frmWaitingForm), true, true, false);
                            SplashScreenManager.Default.SetWaitFormCaption("Printing label...\nĐang in tem...");
                            adoClass.Print_W_M_ReceiveLabel(item2, "QCOK");
                            frmNotification frm = new frmNotification("TRẢ HÀNG THÀNH CÔNG", "notification", 5);
                            frm.ShowDialog();
                            SplashScreenManager.CloseForm();
                            txtMName.Text = "";
                            txtQtyInBox.Text = "0";
                            txtIssueLabel.Text = "";
                            txtReceiveLabel.Text = "";
                            txtProdCustCode.Text = "";
                            txtPLine.Text = "";
                            txtBarcode.Focus();
                        }
                    }
                    else
                    {
                        MessageBox.Show("LỖI CHƯA SCAN TEM CẤP LINH KIỆN");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("THIẾU THÔNG TIN TEM ĐẦU VÀO HOẶC TÊN NGƯỜI NHẬN ", "ERROR");
            }
        }

    }
}
