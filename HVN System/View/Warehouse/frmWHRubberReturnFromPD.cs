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
    public partial class frmWHRubberReturnFromPD : Form
    {
        public frmWHRubberReturnFromPD()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
        }
        public frmWHRubberReturnFromPD(string isAdjust)
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
            if (isAdjust!="")
            {
                isAdjustLabel = true;
                lbFormName.Text = "CHỈNH SỬA CÂN NẶNG PALLET CAO SU";
            }
            
        }
        private ADO adoClass;
        private CmCn conn;
        string COM_Port, COM_Port_BigScale;
        //W_M_ReceiveDocDetail_Entity item;
        SerialPort comport;
        bool isSmallScale = false, isAdjustLabel=false;
        DateTime checking_date = DateTime.Today,created_date=DateTime.Today;

        //bool isChangeScale=true;
        private void frmWHCCManagement_Load(object sender, EventArgs e)
        {
            txtBarcode.Focus();
            //dtpExpiredDate.MinDate = DateTime.Today;
            string[] line = File.ReadAllLines(@"C:\HVN_SYS_CONFIG\Config.txt", Encoding.UTF8);
            COM_Port = line[4].Substring(line[4].LastIndexOf(":") + 1);
            COM_Port_BigScale = line[5].Substring(line[5].LastIndexOf(":") + 1);
            comport = new SerialPort();
            Receive_Data();
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

        private void dtpSupplyDate_ValueChanged(object sender, EventArgs e)
        {
            dtpExpiredDate.CustomFormat = "dd/MM/yyyy";
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (txtRubberName.Text != "")
            {
                if (txtPIC.Text.Trim()!="")
                {
                    W_M_RubberLabel_Entity item = new W_M_RubberLabel_Entity();
                    item.Whrr_code = txtWHRRCode.Text;
                    item.R_name = txtRubberName.Text;
                    item.Weight = float.Parse(txtNetWeight.Text);
                    item.Wh_op = txtPIC.Text;
                    item.Lot_no = dtpLotNo.Value;
                    item.Expired_date = dtpExpiredDate.Value;
                    item.Created_date = created_date;
                    string strQry = "";
                    if (isAdjustLabel)
                    {
                        strQry += "update W_M_RubberLabel set weight=N'"+item.Weight+"' where whrr_code=N'" + item.Whrr_code + "' \n";
                        strQry += "insert into W_M_RubberTransaction(whrr_code,r_name,weight,lot_no,[transaction],input_time,place,PIC)\n";
                        strQry += "select N'" + item.Whrr_code + "',N'" + item.R_name + "',N'" + item.Weight + "',N'" + item.Lot_no.ToString("yyyy-MM-dd") +
                            "',N'Adjust weight by function',getdate(),N'WH Rubber',N'" + item.Wh_op + "'\n";
                    }
                    else
                    {
                        strQry += "update W_M_RubberLabel set weight=N'" + item.Weight + "',time_issue_pd=getdate(),pic_return_pd=N'" + item.Wh_op + "' , place=N'WH Rubber' where whrr_code=N'" + item.Whrr_code + "' \n";
                        strQry += "insert into W_M_RubberTransaction(whrr_code,r_name,weight,lot_no,[transaction],input_time,place,PIC)\n";
                        strQry += "select N'" + item.Whrr_code + "',N'" + item.R_name + "',N'" + item.Weight + "',N'" + item.Lot_no.ToString("yyyy-MM-dd") +
                            "',N'Return Rubber from PD',getdate(),N'WH Rubber',N'" + item.Wh_op + "'\n";
                    }
                    conn = new CmCn();
                    conn.ExcuteQry(strQry);
                    adoClass = new ADO();
                    adoClass.Print_W_M_RubberLabel(item, "QCOK");
                    txtRubberName.Text = "";
                    txtPalletWeight.Text = "0";
                    frmNotification frm = new frmNotification("TRẢ CAO SU THÀNH CÔNG", "notification", 5);
                    frm.ShowDialog();
                }
                else
                {
                    MessageBox.Show("LỖI CHƯA NHẬP TÊN NGƯỜI THỰC HIỆN");
                }
            }
            else
            {
                MessageBox.Show("LỖI CHƯA CHỌN CAO SU HOẶC CAO SU KHÔNG CÓ THÔNG TIN HÊT HẠN");
            }
        }

        private void txtBarcode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                lbError.Text = "";
                if (txtBarcode.Text.Substring(2, 4) == "WHOP")
                {
                    txtPIC.Text = txtBarcode.Text.Substring(6, txtBarcode.Text.Length - 6);
                }
                else
                {
                    string QR_Code = txtBarcode.Text.Substring(2, txtBarcode.Text.Length - 2);
                    insert_info(QR_Code);
                }
                txtBarcode.Text = "";
                txtBarcode.Focus();
            }
        }
        private void insert_info(string qr_code)
        {
            string strQry = "select a.whrr_code,a.r_name,a.lot_no,a.expired_date,b.pallet_weight,a.created_time,a.place \n ";
            strQry += " from W_M_RubberLabel a,W_MasterList_Material b \n ";
            strQry += " where a.whrr_code=N'"+qr_code+"' \n ";
            strQry += " and a.r_name=b.m_name \n ";
            conn = new CmCn();
            DataTable dt = conn.ExcuteDataTable(strQry);
            if (dt.Rows.Count>0)
            {
                if (dt.Rows[0]["place"].ToString() == "WH Rubber"&&!isAdjustLabel)
                {
                    MessageBox.Show("Lỗi cao su đang ở trong kho, không thể nhận 2 lần");
                }
                else
                {
                    txtWHRRCode.Text = dt.Rows[0]["whrr_code"].ToString();
                    txtRubberName.Text = dt.Rows[0]["r_name"].ToString();
                    if (dt.Rows[0]["lot_no"].ToString() != "")
                    {
                        dtpLotNo.Value = DateTime.Parse(dt.Rows[0]["lot_no"].ToString());
                        dtpExpiredDate.Value = DateTime.Parse(dt.Rows[0]["expired_date"].ToString());
                    }

                    txtPalletWeight.Text = dt.Rows[0]["pallet_weight"].ToString();
                    created_date = DateTime.Parse(dt.Rows[0]["created_time"].ToString());
                }
            }
            else
            {
                MessageBox.Show("Lỗi không tim thấy thông tin tem. Liên hệ IT");
            }
        }
        
    }
}
