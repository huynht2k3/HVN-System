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
    public partial class frmWHRubberPrintStockLabel : Form
    {
        public frmWHRubberPrintStockLabel()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
        }
        private ADO adoClass;
        private CmCn conn;
        string COM_Port, COM_Port_BigScale;
        //W_M_ReceiveDocDetail_Entity item;
        SerialPort comport;
        int expired_date = 0;
        bool isSmallScale = false;
        //bool isChangeScale=true;
        private void frmWHCCManagement_Load(object sender, EventArgs e)
        {
            Load_Combobox();
            dtpExpiredDate.MinDate = DateTime.Today;
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

        private void cboRubberName_EditValueChanged(object sender, EventArgs e)
        {
            GridView view = cboRubberName.Properties.View;
            int rowHandle = view.FocusedRowHandle;
            string fieldName = "PALLET_WEIGHT";
            object value = view.GetRowCellValue(rowHandle, fieldName);
            txtPalletWeight.Text = value.ToString();
            //lay so ngay het han
            string fieldName2 = "EXPIRED DATE";
            string raw_value = view.GetRowCellValue(rowHandle, fieldName2).ToString();
            expired_date = string.IsNullOrEmpty(raw_value) ? 0 : int.Parse(raw_value);
        }

        private void dtpSupplyDate_ValueChanged(object sender, EventArgs e)
        {
            dtpExpiredDate.CustomFormat = "dd/MM/yyyy";
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (expired_date != 0)
            {
                if (txtPIC.Text.Trim()!="")
                {
                    W_M_RubberLabel_Entity item = new W_M_RubberLabel_Entity();
                    item.Whrr_code = "WHRR" + Generate_Label_code();
                    item.R_name = cboRubberName.Text;
                    item.Weight = float.Parse(txtNetWeight.Text);
                    item.Lot_no = dtpLotNo.Value;
                    item.Created_date = DateTime.Now;
                    item.Expired_date = dtpExpiredDate.Value;
                    item.Wh_op = txtPIC.Text;
                    string strQry = "insert into W_M_RubberLabel (whrr_code,r_name,weight,lot_no,created_time,created_user,wh_op,wh_receive_time,place,expired_date) \n";
                    strQry += "select N'" + item.Whrr_code + "',N'" + item.R_name + "',N'" + item.Weight + "',N'" + item.Lot_no.ToString("yyyy-MM-dd") +
                        "',getdate(),N'" + General_Infor.username + "',N'" + item.Wh_op + "',getdate(),N'WH Rubber',N'"+item.Expired_date.ToString("yyyy-MM-dd")+"'\n";
                    strQry += "insert into W_M_RubberTransaction(whrr_code,r_name,weight,lot_no,[transaction],input_time,place,PIC)\n";
                    strQry += "select N'" + item.Whrr_code + "',N'" + item.R_name + "',N'" + item.Weight + "',N'" + item.Lot_no.ToString("yyyy-MM-dd") +
                        "',N'Print stock label',getdate(),N'WH Rubber',N'" + item.Wh_op+"'\n";
                    conn = new CmCn();
                    conn.ExcuteQry(strQry);
                    adoClass = new ADO();
                    adoClass.Print_W_M_RubberLabel(item, "QCOK");
                    cboRubberName.Text = "";
                    txtPalletWeight.Text = "0";
                    expired_date = 0;
                    frmNotification frm = new frmNotification("IN TEM THÀNH CÔNG", "notification", 5);
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
        private void Load_Combobox()
        {
            adoClass = new ADO();
            DataTable dt = adoClass.Load_W_MasterList_Rubber("r_name as [RUBBER],pallet_weight as [PALLET_WEIGHT],expired_date as [EXPIRED DATE]", "");
            cboRubberName.Properties.DataSource = dt;
            cboRubberName.Properties.ValueMember = "RUBBER";
            cboRubberName.Properties.DisplayMember = "RUBBER";
        }

        private void txtBarcode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (txtBarcode.Text.Length > 6)
                {
                    string QR_code = "";
                    if (txtBarcode.Text.Substring(0, 4) == "WHRR")
                    {
                        QR_code = txtBarcode.Text;
                    }
                    else
                    {
                        QR_code = txtBarcode.Text.Substring(2, txtBarcode.Text.Length - 2);
                    }
                    string strQry = "select * from W_M_RubberLabel where whrr_code=N'" + QR_code + "'";
                    conn = new CmCn();
                    DataTable dt = conn.ExcuteDataTable(strQry);
                    W_M_RubberLabel_Entity item = new W_M_RubberLabel_Entity();
                    item.Whrr_code = dt.Rows[0]["whrr_code"].ToString();
                    item.R_name = dt.Rows[0]["r_name"].ToString();
                    item.Weight = float.Parse(dt.Rows[0]["weight"].ToString());
                    item.Created_date = string.IsNullOrEmpty(dt.Rows[0]["created_time"].ToString()) ? DateTime.Now : DateTime.Parse(dt.Rows[0]["created_time"].ToString());
                    item.Expired_date = string.IsNullOrEmpty(dt.Rows[0]["expired_date"].ToString()) ? DateTime.Now : DateTime.Parse(dt.Rows[0]["expired_date"].ToString());
                    item.Lot_no = DateTime.Parse(dt.Rows[0]["lot_no"].ToString());
                    adoClass = new ADO();
                    adoClass.Print_W_M_RubberLabel(item, "QCOK");
                    //if (dt.Rows[0]["time_qc_check"].ToString() != "")
                    //{
                    //    item.Time_qc_check = DateTime.Parse(dt.Rows[0]["time_qc_check"].ToString());
                    //    adoClass.Print_W_M_RubberLabel(item, "QCOK");
                    //}
                    //else
                    //{
                    //    adoClass.Print_W_M_RubberLabel(item, "WH");
                    //}
                }
                txtBarcode.Text = "";
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
