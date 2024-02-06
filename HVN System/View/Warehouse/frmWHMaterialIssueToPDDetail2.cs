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
    public partial class frmWHMaterialIssueToPDDetail2 : Form
    {
        public frmWHMaterialIssueToPDDetail2(W_M_IssueDocDetail_Entity select_Doc, DateTime supply_date, string Scale_type, string _PIC)
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
            Doc_infor = select_Doc;
            DocID = Doc_infor.M_doc_id;
            txtMName.Text = Doc_infor.M_name;
            txtPLine.Text = Doc_infor.P_line;
            txtPShift.Text = Doc_infor.P_shift;
            PIC = _PIC;
            txtProdCustCode.Text = Doc_infor.Product_customer_code;
            demand = Doc_infor.M_demand;
            dtpSupplyDate.Value = supply_date;
            if (Scale_type=="CAN NHO")
            {
                isSmallScale = true;
                lbNote.Text = "*LƯU Ý: ĐANG LẤY DỮ LIỆU TỪ CÂN NHỎ";
            }
            else
            {
                isSmallScale = false;
                lbNote.Text = "*LƯU Ý: ĐANG LẤY DỮ LIỆU TỪ CÂN TO";
            }
        }
        private ADO adoClass;
        private CmCn conn;
        string COM_Port, COM_Port_BigScale,DocID,PIC;
        float standard_weight, demand, current_qty;
        W_M_IssueLabel_Entity item;
        W_M_IssueDocDetail_Entity Doc_infor;
        SerialPort comport;
        bool isSmallScale = true;
        //bool isChangeScale=true;
        private void frmWHCCManagement_Load(object sender, EventArgs e)
        {
            Load_standard_weight();
            Load_Result();
            dtpSupplyDate.MinDate = DateTime.Today;
            string[] line = File.ReadAllLines(@"C:\HVN_SYS_CONFIG\Config.txt", Encoding.UTF8);
            COM_Port = line[4].Substring(line[4].LastIndexOf(":") + 1);
            COM_Port_BigScale = line[5].Substring(line[5].LastIndexOf(":") + 1);
            comport = new SerialPort();
            Receive_Data();
            txtBarcode.Focus();
        }

        private void Load_standard_weight()
        {
            adoClass = new ADO();
            try
            {
                DataTable dt = adoClass.Load_W_MasterList_Material("raw_weight/raw_qty as std_w,scale_type", "m_name=N'" + Doc_infor.M_name + "'");
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
        private void Load_Result()
        {
            adoClass = new ADO();
            DataTable dt2 = new DataTable();
            if (DocID!="")
            {
                dt2 = adoClass.Load_W_M_IssueLabel("sum(quantity) as Total", "m_name=N'" + txtMName.Text + "' and product_customer_code=N'" + txtProdCustCode.Text + "' and p_line=N'" + txtPLine.Text + "' and p_shift=N'" + txtPShift.Text + "' and supply_date =N'" + dtpSupplyDate.Value.ToString("yyyy-MM-dd") + "' and m_doc_id=N'"+DocID+"'");
            }
            else
            {
                dt2 = adoClass.Load_W_M_IssueLabel("sum(quantity) as Total", "m_name=N'" + txtMName.Text + "' and product_customer_code=N'" + txtProdCustCode.Text + "' and p_line=N'" + txtPLine.Text + "' and p_shift=N'" + txtPShift.Text + "' and supply_date =N'" + dtpSupplyDate.Value.ToString("yyyy-MM-dd") + "' and m_doc_id=N''");
            }
            if (dt2.Rows[0][0].ToString() == "")
            {
                current_qty = 0;
            }
            else
            {
                current_qty = float.Parse(dt2.Rows[0][0].ToString());
            }
            txtResult.Text = current_qty + "/" + demand;
            txtRemaining.Text = (demand - current_qty).ToString();
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
        private bool Check_Oldest_Lotno_Material(string whmrcode, string Mname)
        {
            string strQry = "declare @oldest_Y as varchar(20)  \n ";
            strQry += " declare @oldest_W as varchar(20)  \n ";
            strQry += " declare @oldest_lotno as date \n ";
            strQry += "  ---Get oldest year \n ";
            strQry += " select @oldest_Y=min(datepart(YY,created_time)) from    \n ";
            strQry += "   W_M_ReceiveLabel    \n ";
            strQry += "   where place = N'WH Material' and quantity>0   \n ";
            strQry += "   and qc_okng=N'OK'   \n ";
            strQry += "   and m_name = N'" + Mname + "' \n ";
            strQry += "   ---Get oldest week \n ";
            strQry += "   select @oldest_W=min(datepart(wk,created_time)) from    \n ";
            strQry += "   W_M_ReceiveLabel    \n ";
            strQry += "   where place = N'WH Material' and quantity>0   \n ";
            strQry += "   and qc_okng=N'OK'   \n ";
            strQry += "   and m_name = N'" + Mname + "'  \n ";
            strQry += "   and datepart(YY,created_time)=@oldest_Y  \n ";
            strQry += "   ---Get oldest lot_no \n ";
            strQry += "   select @oldest_lotno=min(lot_no) from   \n ";
            strQry += "   W_M_ReceiveLabel  \n ";
            strQry += "   where  1=1  \n ";
            strQry += "   and datepart(wk,created_time)=@oldest_W \n ";
            strQry += "   and datepart(YY,created_time)=@oldest_Y \n ";
            strQry += "   and place = N'WH Material' and quantity>0   \n ";
            strQry += "   and qc_okng=N'OK'   \n ";
            strQry += "   and m_name = N'" + Mname + "' \n ";
            strQry += "   ---Get result \n ";
            strQry += "   select CAST(@oldest_lotno AS varchar(20)) as oldest_lotno  \n ";
            strQry += "   ,@oldest_W as oldest_W,@oldest_Y as oldest_Y,  \n ";
            strQry += "   case   \n ";
            strQry += "   when lot_no=@oldest_lotno   \n ";
            strQry += "   and datepart(wk,created_time)=@oldest_W then 'OK'   \n ";
            strQry += "   else 'NOT OK'   \n ";
            strQry += "   end as Result   \n ";
            strQry += "   from W_M_ReceiveLabel   \n ";
            strQry += "   where whmr_code=N'" + whmrcode + "'  \n ";

            conn = new CmCn();
            DataTable dt = conn.ExcuteDataTable(strQry);
            if (dt.Rows[0]["Result"].ToString() == "OK")
            {
                return true;
            }
            else
            {
                lbNote.Text = "LỖI THÙNG CŨ NHẤT CÓ TUẦN LÀ:" + dt.Rows[0]["oldest_W"].ToString()+ ",NĂM LÀ:" + dt.Rows[0]["oldest_Y"].ToString() + "  VÀ CÓ LOT_NO LÀ:" + dt.Rows[0]["oldest_lotno"].ToString();
                return false;
            }

        }
        private void txtBarcode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string label_code = txtBarcode.Text.Substring(2, txtBarcode.Text.Length - 2);
                if (Check_Oldest_Lotno_Material(label_code,txtMName.Text))
                {
                    Load_data_label(label_code);
                }
                txtBarcode.Text = "";
                txtBarcode.Focus();
            }
        }
        private void Load_data_label( string label_code)
        {
            string strQry = "select m_name,quantity,lot_no from W_M_ReceiveLabel where whmr_code = N'" + label_code + "' and place=N'WH Material'";
            conn = new CmCn();
            DataTable dt = conn.ExcuteDataTable(strQry);
            if (dt.Rows.Count < 1)
            {
                MessageBox.Show("LỖI THÙNG CÓ MÃ " + label_code + " KHÔNG CÓ TRONG KHO");
            }
            else
            {
                if (dt.Rows[0]["m_name"].ToString() == txtMName.Text)
                {
                    if (dt.Rows[0]["lot_no"].ToString()=="")
                    {
                        MessageBox.Show("LỖI THÙNG KHÔNG CÓ LOT NO, VUI LÒNG CHUYỂN QUA QC KIỂM TRA");
                    }
                    else
                    {
                        if (dt.Rows[0]["quantity"].ToString() != "0")
                        {
                            txtReceiveLabelCode.Text = label_code;
                            txtQtyInBox.Text = dt.Rows[0]["quantity"].ToString();
                            dtpLotNo.Value = DateTime.Parse(dt.Rows[0]["lot_no"].ToString());
                        }
                        else
                        {
                            txtReceiveLabelCode.Text = "";
                            txtQtyInBox.Text = "0";
                        }
                    }
                }
                else
                {
                    MessageBox.Show("LỖI THÙNG THÙNG CHỨA MÃ LINH KIỆN " + dt.Rows[0]["m_name"].ToString() + " KHÔNG KHỚP VỚI " + txtMName.Text);
                    txtReceiveLabelCode.Text = "";
                    txtQtyInBox.Text = "0";
                }
            }
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            txtBarcode.Focus();
        }

        private void btnConfirmAb_Click(object sender, EventArgs e)
        {
            if (txtReceiveLabelCode.Text=="")
            {
                MessageBox.Show("LỖI CHƯA SCAN TEM LINH KIỆN");
            }
            else
            {
                if (txtQuantity.Text != "0")
                {
                    try
                    {
                        item = new W_M_IssueLabel_Entity();
                        item.Whmi_code = "WHMI" + Generate_Label_code();
                        item.Whmr_code = txtReceiveLabelCode.Text;
                        item.M_name = txtMName.Text;
                        item.P_line = txtPLine.Text;
                        item.P_shift = txtPShift.Text;
                        item.M_doc_id = DocID;
                        item.Pic = PIC;
                        item.Quantity = float.Parse(txtQuantity.Text);
                        item.Lot_no = dtpLotNo.Value;
                        item.Weight = float.Parse(txtWeight.Text);
                        item.Product_customer_code = txtProdCustCode.Text;
                        item.Supply_date = dtpSupplyDate.Value;
                        item.Qty_in_box = float.Parse(txtQtyInBox.Text);
                        item.Note = "Actual:" + txtQuantity.Text + " ,Theoretical:" + txtQtyInBox.Text;
                        if (float.Parse(txtQuantity.Text) > float.Parse(txtQtyInBox.Text))
                        {
                            item.Transation = "Issue abnormal(+) material to PD";
                        }
                        else
                        {
                            item.Transation = "Issue abnormal(-) material to PD";
                        }
                        if (current_qty + item.Quantity <= demand)
                        {
                            adoClass = new ADO();
                            adoClass.Insert_W_M_IssueLabel_2(item, "Update special");
                            SplashScreenManager.ShowForm(this, typeof(frmWaitingForm), true, true, false);
                            SplashScreenManager.Default.SetWaitFormCaption("Printing label...\nĐang in tem...");
                            adoClass.Print_W_M_IssueLabel(item);
                            SplashScreenManager.CloseForm();
                            txtReceiveLabelCode.Text = "";
                            txtQtyInBox.Text = "0";
                            Load_Result();
                            if (current_qty == demand)
                            {
                                txtReceiveLabelCode.Text = "";
                                txtQtyInBox.Text = "0";
                                frmNotification frm = new frmNotification("HOÀN THÀNH CẤP NGUYÊN VẬT LIỆU NÀY \nCOMPLETE ISSUE THIS MATERIAL", "notification", 5);
                                frm.ShowDialog();
                                this.Close();
                            }
                            else
                            {
                                Load_Result();
                                frmNotification frm = new frmNotification("XÁC NHẬN THÀNH CÔNG \nCONFIRM SUCCESSFULLY", "notification", 2);
                                frm.ShowDialog();
                            }
                        }
                        else
                        {
                            frmNotification frm = new frmNotification("LỖI THỪA NGUYÊN VẬT LIỆU \nTHE ISSUED QUANTITY IS MORE THAN DEMAND", "ERROR", 2);
                            frm.ShowDialog();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                else
                {
                    MessageBox.Show("CHƯA CÓ NGUYÊN VẬT LIỆU TRÊN CÂN \nPLEASE PUT MATERIAL ON SCALE", "ERROR");
                }
            }
        }

        private void dtpSupplyDate_ValueChanged(object sender, EventArgs e)
        {
            dtpSupplyDate.CustomFormat = "dd/MM/yyyy";
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (txtQuantity.Text != "0" || txtQtyInBox.Text != "0")
            {
                if (float.Parse(txtQuantity.Text) > float.Parse(txtQtyInBox.Text))
                {
                    frmNotification frm = new frmNotification("Có lỗi số lượng trên cân lớn hơn số lượng trong hộp. Vui lòng chọn xác nhận hết hàng trong thùng \nError quantity on scale more than quantity in box", "Error", 5);
                    frm.ShowDialog();
                    return;
                }
                try
                {
                    item = new W_M_IssueLabel_Entity();
                    item.Whmi_code = "WHMI" + Generate_Label_code();
                    item.Whmr_code = txtReceiveLabelCode.Text;
                    item.M_name = txtMName.Text;
                    item.P_line = txtPLine.Text;
                    item.P_shift = txtPShift.Text;
                    item.M_doc_id = DocID;
                    item.Pic = PIC;
                    item.Quantity = float.Parse(txtQuantity.Text);
                    item.Lot_no = dtpLotNo.Value;
                    item.Weight = float.Parse(txtWeight.Text);
                    item.Product_customer_code = txtProdCustCode.Text;
                    item.Supply_date = dtpSupplyDate.Value;
                    item.Qty_in_box = float.Parse(txtQtyInBox.Text);
                    item.Transation = "Issue material to production";
                    if (current_qty + item.Quantity <= demand)
                    {
                        adoClass = new ADO();
                        adoClass.Insert_W_M_IssueLabel_2(item,"Normal");
                        SplashScreenManager.ShowForm(this, typeof(frmWaitingForm), true, true, false);
                        SplashScreenManager.Default.SetWaitFormCaption("Printing label...\nĐang in tem...");
                        adoClass.Print_W_M_IssueLabel(item);
                        SplashScreenManager.CloseForm();
                        Load_data_label(txtReceiveLabelCode.Text);
                        Load_Result();
                        if (current_qty == demand)
                        {
                            string strQry = "select * from W_M_ReceiveLabel where whmr_code=N'"+ txtReceiveLabelCode.Text + "'";
                            conn = new CmCn();
                            DataTable dt = conn.ExcuteDataTable(strQry);
                            W_M_ReceiveLabel_Entity item2 = new W_M_ReceiveLabel_Entity();
                            item2.Whmr_code = dt.Rows[0]["whmr_code"].ToString();
                            item2.M_name= dt.Rows[0]["m_name"].ToString();
                            item2.Quantity = float.Parse(dt.Rows[0]["quantity"].ToString());
                            item2.Created_date = string.IsNullOrEmpty(dt.Rows[0]["quantity"].ToString()) ? DateTime.Now : DateTime.Parse(dt.Rows[0]["created_time"].ToString());
                            if (item.Quantity>0)
                            {
                                item2.Lot_no = DateTime.Parse(dt.Rows[0]["lot_no"].ToString());
                                if (dt.Rows[0]["time_qc_check"].ToString() != "")
                                {
                                    item2.Time_qc_check = DateTime.Parse(dt.Rows[0]["time_qc_check"].ToString());
                                }
                                adoClass = new ADO();
                                adoClass.Print_W_M_ReceiveLabel(item2, "QCOK");
                                frmNotification frm = new frmNotification("HOÀN THÀNH CẤP NGUYÊN VẬT LIỆU NÀY \nCOMPLETE ISSUE THIS MATERIAL", "notification", 5);
                                frm.ShowDialog();
                                this.Close();
                               
                            }
                           
                        }
                        else
                        {
                            frmNotification frm = new frmNotification("XÁC NHẬN THÀNH CÔNG \nCONFIRM SUCCESSFULLY", "notification", 2);
                            frm.ShowDialog();
                        }
                    }
                    else
                    {
                        frmNotification frm = new frmNotification("LỖI THỪA NGUYÊN VẬT LIỆU \nTHE ISSUED QUANTITY IS MORE THAN DEMAND", "ERROR", 2);
                        frm.ShowDialog();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("CHƯA CÓ NGUYÊN VẬT LIỆU TRÊN CÂN \nPLEASE PUT MATERIAL ON SCALE", "ERROR");
            }
        }


        private int Generate_Label_code()
        {
            string Qry = "SELECT MAX(whmi_code) FROM W_M_IssueLabel ";
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
