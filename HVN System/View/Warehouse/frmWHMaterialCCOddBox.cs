using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HVN_System.View.Warehouse
{
    public partial class frmWHMaterialCCOddBox : Form
    {
        public frmWHMaterialCCOddBox()
        {
            InitializeComponent();
        }
        public frmWHMaterialCCOddBox(string PIC,DateTime Cc_date)
        {
            InitializeComponent();
            txtPIC.Text = PIC;
            dtpCCDate.Value = Cc_date;
        }
        string COM_Port, COM_Port_BigScale,raw_data="";
        float standard_weight;
        SerialPort comport;
        private CmCn conn;
        bool isSmallScale = true;
        private void ckAdd_CheckedChanged(object sender, EventArgs e)
        {
            if (ckAdd.Checked)
            {
                ckAdd.Text = "REMOVE/ XÓA";
            }
            else
            {
                ckAdd.Text = "ADD/ THÊM";
            }
        }

        private void txtBarcode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode==Keys.Enter)
            {
                string QR_code= txtBarcode.Text.Substring(2, txtBarcode.Text.Length - 2);
                if (QR_code.Length>=6)
                {
                    lbError.Text = "";
                    if (txtBarcode.Text.Substring(2, 4) == "WHOP")
                    {
                        txtPIC.Text= txtBarcode.Text.Substring(6, txtBarcode.Text.Length - 6);
                    }
                    else if(txtBarcode.Text.Substring(2, 4) == "WHMI")
                    {
                        if (txtPIC.Text == "")
                        {
                            lbError.Text = "LỖI: BẠN CHƯA QUÉT MÃ NHÂN VIÊN";
                        }
                        else
                        {
                            if (ckAdd.Checked)
                            {
                                Remove_Item(QR_code);
                            }
                            else
                            {
                                Add_Item(QR_code);
                            }
                        }
                        
                    }
                    else
                    {
                        lbError.Text = QR_code+":KHÔNG PHẢI TEM TEM XUẤT HÀNG";
                    }
                }
                txtBarcode.Text = "";
                txtBarcode.Focus();
                Load_data(dtpCCDate.Value);
            }
        }
        private void Remove_Item(string QRCode)
        {
            conn = new CmCn();
            string strQry = " delete from W_M_CCInventory where whmi_code=N'" + QRCode + "' and cc_date=N'" + dtpCCDate.Value.ToString("yyyy-MM-dd") + "'";
            conn.ExcuteQry(strQry);
        }
        private void Add_Item(string QRCode)
        {
            string strQry = "select a.whmi_code,a.m_name,a.quantity,round(b.raw_weight/b.raw_qty,2) as std_weight \n ";
            strQry += " from W_M_IssueLabel a,W_MasterList_Material b \n ";
            strQry += " where a.whmi_code=N'"+QRCode+"' \n ";
            strQry += " and a.m_name=b.m_name \n ";
            conn = new CmCn();
            DataTable dt = conn.ExcuteDataTable(strQry);
            if (dt.Rows.Count>0)
            {
                txtLabelCode.Text = dt.Rows[0]["whmi_code"].ToString();
                txtMName.Text = dt.Rows[0]["m_name"].ToString();
                standard_weight = float.Parse(dt.Rows[0]["std_weight"].ToString());
            }
            else
            {
                lbError.Text = QRCode + ":THÙNG KHÔNG TỒN TẠI HOẶC MÃ HÀNG BỊ SAI";
            }

        }
        private void Load_data(DateTime Cc_date)
        {
            string strQry = "select place,m_name,sum(quantity) as qty_pcs,count(m_name) as qty_box from W_M_CCInventory \n ";
            if (txtPIC.Text=="")
            {
                strQry += " where cc_date=N'" + dtpCCDate.Value.ToString("yyyy-MM-dd") + "' and m_kind=N'RETURN BOX' \n ";
            }
            else
            {
                strQry += " where cc_date=N'" + dtpCCDate.Value.ToString("yyyy-MM-dd") + "' and m_kind=N'RETURN BOX' and pic=N'"+txtPIC.Text.ToUpper()+"' \n ";
            }
            strQry += " group by place,m_name \n ";
            conn = new CmCn();
            dgvResult.DataSource = conn.ExcuteDataTable(strQry);
            string strQry2 = "select * from W_M_CCInventory \n ";
            if (txtPIC.Text == "")
            {
                strQry2 += " where cc_date=N'" + dtpCCDate.Value.ToString("yyyy-MM-dd") + "' and m_kind=N'RETURN BOX' \n ";
            }
            else
            {
                strQry2 += " where cc_date=N'" + dtpCCDate.Value.ToString("yyyy-MM-dd") + "' and m_kind=N'RETURN BOX' and pic=N'" + txtPIC.Text.ToUpper() + "' \n ";
            }
            DataTable dt = conn.ExcuteDataTable(strQry2);
            dgvDetail.DataSource= dt;
            txtTotalBox.Text = dt.Rows.Count.ToString();
        }

        private void frmWHMaterialCCFullBox_Load(object sender, EventArgs e)
        {
            string[] line = File.ReadAllLines(@"C:\HVN_SYS_CONFIG\Config.txt", Encoding.UTF8);
            COM_Port = line[4].Substring(line[4].LastIndexOf(":") + 1);
            COM_Port_BigScale = line[5].Substring(line[5].LastIndexOf(":") + 1);
            comport = new SerialPort();
            txtBarcode.Focus();
            cboPlace.Text = "WH Material";
            cboScaleType.Text = "CAN NHO";
            Load_data(dtpCCDate.Value);
            txtBarcode.Focus();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            lbError.Text = "";
            if (cboScaleType.Text == "CAN NHO")
            {
                isSmallScale = true;
            }
            else
            {
                isSmallScale = false;
            }
            comport = new SerialPort();
            Receive_Data();
            if (lbError.Text=="")
            {
                btnOK.Visible = true;
                btnStart.Enabled = false;
                cboScaleType.Enabled = false;
                txtBarcode.Focus();
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
            catch
            {
                //MessageBox.Show("Lỗi kết nối!Không thể kết nối tới cân.");
                lbError.Text = "Lỗi kết nối! Không thể kết nối tới cân.";
            }
        }
        private void Load_Weight()
        {
            //isChangeScale = true;
            while (true)
            {
                try
                {
                    //if (isSmallScale)
                    //{
                    //    Thread.Sleep(100);
                    //}
                    raw_data = comport.ReadLine();
                }
                catch (Exception)
                {

                }
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            string QRCode = txtLabelCode.Text;
            if (txtPIC.Text == "")
            {
                lbError.Text = "LỖI: BẠN CHƯA QUÉT MÃ NHÂN VIÊN";
                return;
            }
            else if (txtLabelCode.Text==""||txtQuantity.Text=="0")
            {
                lbError.Text = "LỖI: BẠN CHƯA QUÉT MÃ QR HOẶC CÂN CHƯA NHẬN";
                return;
            }
            string strQry = " delete from W_M_CCInventory where whmr_code=N'" + QRCode + "'  \n ";
            strQry += " insert into W_M_CCInventory (cc_date,whmr_code,m_name,quantity,place,pic,time_commit,m_kind) \n ";
            strQry += " select N'" + dtpCCDate.Value.ToString("yyyy-MM-dd") + "',N'" + QRCode + "',N'" + txtMName.Text + "',N'" + txtQuantity.Text + "' \n ";
            strQry += " ,N'" + cboPlace.Text + "',N'" + txtPIC.Text.ToUpper() + "',getdate(),N'RETURN BOX' \n ";
            conn.ExcuteQry(strQry);
            txtLabelCode.Text = "";
            txtMName.Text = "";
            txtBarcode.Focus();
            Load_data(dtpCCDate.Value);
        }

        private void txtLabelCode_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            txtBarcode.Focus();
        }

        private void Convert_small()
        {
            while (true)
            {
                Thread.Sleep(100);
                try
                {
                    if (raw_data.Length > 3)
                    {
                        txtWeight.Text = float.Parse(raw_data.Substring(0, raw_data.Length - 3)).ToString();
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
                    if (raw_data.Length > 4)
                    {
                        try
                        {
                            txtWeight.Text = raw_data.Substring(0, raw_data.Length - 3).Trim();
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
    }
}
