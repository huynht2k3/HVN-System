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
using HVN_System.Util;

namespace HVN_System.View.Warehouse
{
    public partial class frmWHRubberReceiving : Form
    {
        public frmWHRubberReceiving(string _r_name,string _truckNo,string _inv_no, string _pic)
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
            txtR_Name.Text = _r_name;
            truck_no = _truckNo;
            inv_no = _inv_no;
            txtPIC.Text = _pic;
        }
        private ADO adoClass;
        private CmCn conn;
        string COM_Port, COM_Port_BigScale, truck_no, inv_no;
        //W_M_ReceiveDocDetail_Entity item;
        SerialPort comport;
        int expired_date = 0;
        bool isSmallScale = false;
        //bool isChangeScale=true;
        private void frmWHCCManagement_Load(object sender, EventArgs e)
        {
            dtpLotNo.Value = DateTime.Today;
            string[] line = File.ReadAllLines(@"C:\HVN_SYS_CONFIG\Config.txt", Encoding.UTF8);
            COM_Port = line[4].Substring(line[4].LastIndexOf(":") + 1);
            COM_Port_BigScale = line[5].Substring(line[5].LastIndexOf(":") + 1);
            comport = new SerialPort();
            Load_Rubber_info();
            dtpExpiredDate.MinDate = DateTime.Today;
            Receive_Data();
        }
        private void Load_Rubber_info()
        {
            string strQry = "select * from W_MasterList_Material where m_name=N'" + txtR_Name.Text + "'";
            conn = new CmCn();
            DataTable dt = conn.ExcuteDataTable(strQry);
            if (dt.Rows.Count > 0)
            {
                expired_date = int.Parse(dt.Rows[0]["expiry_day"].ToString());
                txtPalletWeight.Text = dt.Rows[0]["pallet_weight"].ToString();
                if (expired_date==0)
                {
                    MessageBox.Show("LỖI: MÃ CAO SU CHƯA CÓ THỜI GIAN HẾT HẠN");
                    this.Close();
                }
            }
            else
            {
                MessageBox.Show("LỖI: MÃ CAO SU KHÔNG TỒN TẠI. LIÊN LẠC IT");
                this.Close();
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
            //Load_Weight();
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
                            txtWeight.Text = test.Substring(1, test.Length - 6);
                            txtNetWeight.Text = (float.Parse(txtWeight.Text) - float.Parse(txtPalletWeight.Text)).ToString();
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
            dtpExpiredDate.Value = dtpLotNo.Value.AddDays(expired_date);
        }

        private void ckInputManual_CheckedChanged(object sender, EventArgs e)
        {
            if (ckInputManual.Checked)
            {
                lbStatus.Text = "Input manualy!!!";
                txtRawData.Text = "";
                txtWeight.Text = "1";
                txtNetWeight.ReadOnly = false;
            }
            else
            {
                txtNetWeight.ReadOnly = true;
            }
        }

        private void txtWeight_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void txtIssueLabelCode_KeyDown(object sender, KeyEventArgs e)
        {
            
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dtpSupplyDate_ValueChanged(object sender, EventArgs e)
        {

        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (expired_date != 0)
            {
                if (dtpLotNo.Value!=DateTime.Today)
                {
                    W_M_RubberLabel_Entity item = new W_M_RubberLabel_Entity();
                    item.Whrr_code = "WHRR" + Generate_Label_code();
                    item.R_name = txtR_Name.Text;
                    item.Weight = float.Parse(txtNetWeight.Text);
                    item.Lot_no = dtpLotNo.Value;
                    item.Created_date = DateTime.Now;
                    item.Expired_date = dtpExpiredDate.Value;
                    item.Truck_no = truck_no;
                    item.Rm_doc_id = inv_no;
                    item.Transaction = "Entry WH";
                    item.Is_check = "True";
                    item.Place = "WH Rubber";
                    item.Wh_op = txtPIC.Text;
                    string strQry = "insert into W_M_RubberLabel (whrr_code,r_name,weight,lot_no,created_time,created_user,wh_op,wh_receive_time,place,expired_date) \n";
                    strQry += "select N'" + item.Whrr_code + "',N'" + item.R_name + "',N'" + item.Weight + "',N'" + item.Lot_no.ToString("yyyy-MM-dd") +
                        "',getdate(),N'" + General_Infor.username + "',NULL,NULL,NULL,N'" + item.Expired_date.ToString("yyyy-MM-dd") + "'\n";
                    strQry += "Insert into TEMP_W_R_Receiving ([whrr_code],[r_name],[weight],[rm_doc_id] \n ";
                    strQry += " ,[wh_op],[wh_receive_time],[truck_no],[place],[transaction],[is_check],[wh_okng],is_active) values \n ";
                    strQry += " (N'" + item.Whrr_code + "',N'" + item.R_name + "',N'" + item.Weight + "' \n ";
                    strQry += " ,N'" + item.Rm_doc_id + "',N'" + item.Wh_op + "',getdate() \n ";
                    strQry += " ,N'" + item.Truck_no + "',N'" + item.Place + "',N'" + item.Transaction + "',N'" + item.Is_check + "',N'OK',N'0') \n ";
                    conn = new CmCn();
                    conn.ExcuteQry(strQry);
                    adoClass = new ADO();
                    adoClass.Print_W_M_RubberLabel(item, "incoming");
                    txtR_Name.Text = "";
                    txtPalletWeight.Text = "0";
                    expired_date = 0;
                    frmNotification frm = new frmNotification("IN TEM THÀNH CÔNG", "notification", 5);
                    frm.ShowDialog();
                }
                else
                {
                    MessageBox.Show("LỖI CHƯA CHỈNH THÔNG TIN LOT NO, NGÀY LOT NO ĐANG TRÙNG NGÀY HIỆN TẠI");
                }
            }
            else
            {
                MessageBox.Show("LỖI CHƯA CHỌN CAO SU HOẶC CAO SU KHÔNG CÓ THÔNG TIN HÊT HẠN");
            }
        }
       

        private int Generate_Label_code()
        {
            string Qry = "SELECT MAX(whrr_code) FROM W_M_RubberLabel ";
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
    }
}
