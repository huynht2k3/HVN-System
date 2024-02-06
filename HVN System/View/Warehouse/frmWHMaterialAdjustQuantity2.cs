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
    public partial class frmWHMaterialAdjustQuantity2 : Form
    {
        public frmWHMaterialAdjustQuantity2()
        {
            InitializeComponent();
        }
        public frmWHMaterialAdjustQuantity2(string PIC)
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
                DataTable dt = adoClass.Load_W_MasterList_Material("raw_weight/raw_qty as std_w,scale_type", "m_name=N'" + txtMName.Text + "'");
                string result = dt.Rows[0][0].ToString();
                if (result != "")
                {
                    standard_weight = float.Parse(result);
                }
            }
            catch (Exception)
            {

                MessageBox.Show("Error: Cannot check standard weight \nLỗi không tìm được trọng lượng tiêu chuẩn", "Error");
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
                    //if (isSmallScale)
                    //{
                    //    Thread.Sleep(100);
                    //}
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
            while (true)
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
                txtWeight.Text = "1";
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
                string label_code = txtBarcode.Text.Substring(2, txtBarcode.Text.Length - 2);
                Load_data_label(label_code);
            }
        }
        private void Load_data_label(string label_code)
        {
            string strQry = "select m_name,quantity,lot_no from W_M_ReceiveLabel where whmr_code = N'" + label_code + "'";
            conn = new CmCn();
            DataTable dt = conn.ExcuteDataTable(strQry);
            if (dt.Rows.Count < 1)
            {
                MessageBox.Show("LỖI THÙNG CÓ MÃ " + label_code + " KHÔNG CÓ TRONG KHO");
            }
            else
            {
                txtReceiveLabelCode.Text = label_code;
                txtMName.Text= dt.Rows[0]["m_name"].ToString();
                txtQtyInBox.Text = dt.Rows[0]["quantity"].ToString();
                try
                {
                    dtpLotNo.Value = DateTime.Parse(dt.Rows[0]["lot_no"].ToString());
                }
                catch (Exception)
                {
                    dtpLotNo.Value = DateTime.Today;
                }
                
                Load_standard_weight();
                
            }
            txtBarcode.Text = "";
            txtBarcode.Focus();
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            txtBarcode.Focus();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (cboScaleType.Text== "")
            {
                lbNote.Text = "LƯU Ý: BẠN CHƯA CHỌN LOẠI CÂN";
            }
            else 
            {
                if (cboScaleType.Text == "SMALL SCALE/ CAN NHO")
                {
                    isSmallScale = true;
                }
                else
                {
                    isSmallScale = false;
                }
                comport = new SerialPort();
                Receive_Data();
                btnConfirm.Visible = true;
                btnStart.Visible = false;
                cboScaleType.Enabled = false;
            }
           
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (txtPIC.Text!="")
            {
                if (txtQuantity.Text != "0" && txtQtyInBox.Text != "0")
                {
                    string strQry2 = "update W_M_ReceiveLabel set quantity=N'" + txtQuantity.Text + "' where whmr_code=N'" + txtReceiveLabelCode.Text + "'\n";
                    strQry2 += "Insert into W_M_HistoryOfTransaction (whmr_code,m_name,quantity,lot_no,[transaction],input_time,PIC,m_note) \n ";
                    strQry2 += "select N'"+txtReceiveLabelCode.Text+ "',N'" + txtMName.Text + "',N'" + txtQuantity.Text + "'" +
                        ",N'" + dtpLotNo.Value.ToString("yyyy-MM-dd") + "',N'Adjust quantity of box by scale',getdate(),N'" + txtPIC.Text + "',N'"+txtQtyInBox.Text+"'\n";
                    conn = new CmCn();
                    conn.ExcuteQry(strQry2);
                    string strQry = "select * from W_M_ReceiveLabel where whmr_code=N'" + txtReceiveLabelCode.Text + "'";
                    DataTable dt = conn.ExcuteDataTable(strQry);
                    W_M_ReceiveLabel_Entity item = new W_M_ReceiveLabel_Entity();
                    item.Whmr_code = dt.Rows[0]["whmr_code"].ToString();
                    item.M_name = dt.Rows[0]["m_name"].ToString();
                    item.Quantity = float.Parse(dt.Rows[0]["quantity"].ToString());
                    item.Created_date = string.IsNullOrEmpty(dt.Rows[0]["quantity"].ToString()) ? DateTime.Now : DateTime.Parse(dt.Rows[0]["created_time"].ToString());
                    SplashScreenManager.ShowForm(this, typeof(frmWaitingForm), true, true, false);
                    SplashScreenManager.Default.SetWaitFormCaption("Printing...");
                    if (dt.Rows[0]["lot_no"].ToString()!="")
                    {
                        item.Lot_no = DateTime.Parse(dt.Rows[0]["lot_no"].ToString());
                        if (dt.Rows[0]["time_qc_check"].ToString() != "")
                        {
                            item.Time_qc_check = DateTime.Parse(dt.Rows[0]["time_qc_check"].ToString());
                        }
                        adoClass = new ADO();
                        adoClass.Print_W_M_ReceiveLabel(item, "QCOK");
                    }
                    else
                    {
                        adoClass = new ADO();
                        adoClass.Print_W_M_ReceiveLabel(item, "WH");
                    }
                    txtMName.Text = "";
                    txtQtyInBox.Text = "0";
                    txtReceiveLabelCode.Text = "";
                    SplashScreenManager.CloseForm();
                    frmNotification frm = new frmNotification("ĐÃ CẬP NHẬT DỮ LIỆU \nCOMPLETE UPDATE DATA", "notification", 5);
                    frm.ShowDialog();
                }
                else
                {
                    MessageBox.Show("CHƯA CÓ NGUYÊN VẬT LIỆU TRÊN CÂN \nPLEASE PUT MATERIAL ON SCALE", "ERROR");
                }
            }
            else
            {
                MessageBox.Show("LỖI CHƯA NHẬP TÊN NGƯỜI LÀM \nPLEASE IPUT PIC", "ERROR");
            }
        }

    }
}
