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
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraBars;
using HVN_System.View.Warehouse;

namespace HVN_System.View.Planning
{
    public partial class frmWHMaterial_ReceiveDocumentDetail : Form
    {
        public frmWHMaterial_ReceiveDocumentDetail()
        {
            InitializeComponent();
        }
        public frmWHMaterial_ReceiveDocumentDetail(W_M_ReceiveDoc_Entity item, bool is_Print)
        {
            InitializeComponent();
            txtDocID.Text = item.Rm_doc_id;
            txtAttachment.Text = item.Rm_doc_name;
            txtAttachment2.Text = item.Rm_doc_name2;
            txtAttachment3.Text = item.Rm_doc_name3;
            txtSupplier.Text = item.Supplier;
            txtTruckNo.Text = item.Truck_no;
            dtpReceiveDate.Value = item.Receive_date;
            Documment = item;
            attach_link = @item.Rm_doc_link;
            attach_link2 = @item.Rm_doc_link2;
            attach_link3 = @item.Rm_doc_link3;
            cboType.Text = item.Rm_kind;
            txtDocID.ReadOnly = true;
            cboType.Enabled = false;
            isEdit = true;
            if (is_Print)
            {
                btnSave.Visibility = BarItemVisibility.Never;
                btnDelete.Visibility = BarItemVisibility.Never;
                btnPrint.Visibility= BarItemVisibility.Always;
                if (item.Rm_kind=="Material")
                {
                    gridColumn5.Visible = true;
                }
                else
                {
                    gvResult.OptionsBehavior.Editable = false;
                }
                txtTruckNo.Enabled = false;
                dtpReceiveDate.Enabled = false;
                txtSupplier.Enabled = false;
            }
        }
        private ADO adoClass;
        private CmCn conn;
        private W_M_ReceiveDoc_Entity Documment;
        private W_M_ReceiveDocDetail_Entity Current_item;
        private List<W_M_ReceiveDocDetail_Entity> List_Data;
        private bool isEdit = false,isNewAttach=false, isNewAttach2=false, isNewAttach3=false;
        string attach_link,source, attach_link2, source2, attach_link3, source3;
        private void btnSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            adoClass = new ADO();
            bool isOk = false;
            if (Check_list_data()) //Kiem tra xem co PN nao khong tim thay trong master list
            {
                if (isEdit)
                {
                    isOk = true;
                }
                else
                {
                    if (txtDocID.Text == "")
                    {
                        MessageBox.Show("Please fill the Documment ID");
                    }
                    else
                    {
                        DataTable dt = adoClass.Load_W_M_ReceiveDoc("", "rm_doc_id=N'" + txtDocID.Text + "'");
                        if (dt.Rows.Count > 0)
                        {
                            MessageBox.Show("This Docummnet number is already exist. Please choose another number");
                        }
                        else
                        {
                            isOk = true;
                        }
                    }
                }
                if (isOk)
                {
                    if (isNewAttach)
                    {
                        if (System.IO.File.Exists(attach_link))
                        {
                            try
                            {
                                File.Delete(attach_link);
                            }
                            catch (Exception)
                            {
                                txtAttachment.Text += "_1";
                                attach_link = @"\\172.16.180.20\20.Public\05.IT\05.HVN_TOOL\ATTACHMENT\WAREHOUSE\" + txtAttachment.Text;
                            }
                        }
                        File.Copy(source, attach_link);
                    }
                    if (isNewAttach2)
                    {
                        if (System.IO.File.Exists(attach_link2))
                        {
                            try
                            {
                                File.Delete(attach_link2);
                            }
                            catch (Exception)
                            {
                                txtAttachment2.Text += "_1";
                                attach_link = @"\\172.16.180.20\20.Public\05.IT\05.HVN_TOOL\ATTACHMENT\WAREHOUSE\" + txtAttachment2.Text;
                            }
                        }
                        File.Copy(source2, attach_link2);
                    }
                    if (isNewAttach3)
                    {
                        if (System.IO.File.Exists(attach_link3))
                        {
                            try
                            {
                                File.Delete(attach_link3);
                            }
                            catch (Exception)
                            {
                                txtAttachment3.Text += "_1";
                                attach_link3 = @"\\172.16.180.20\20.Public\05.IT\05.HVN_TOOL\ATTACHMENT\WAREHOUSE\" + txtAttachment3.Text;
                            }
                        }
                        File.Copy(source3, attach_link3);
                    }
                    Documment.Rm_doc_id = txtDocID.Text;
                    Documment.Receive_date = dtpReceiveDate.Value;
                    Documment.Rm_doc_name = txtAttachment.Text;
                    Documment.Rm_doc_name2 = txtAttachment2.Text;
                    Documment.Rm_doc_name3 = txtAttachment3.Text;
                    Documment.Rm_doc_link = attach_link;
                    Documment.Rm_doc_link2 = attach_link2;
                    Documment.Rm_doc_link3 = attach_link3;
                    Documment.Rm_kind = cboType.Text;
                    Documment.Supplier = txtSupplier.Text;
                    Documment.Truck_no = txtTruckNo.Text;
                    adoClass.Update_W_M_ReceiveDoc(List_Data, Documment);
                    MessageBox.Show("Saving successfully");
                    this.Close();
                }
            }
        }
        private bool Check_list_data()
        {
            bool result = true;
            //adoClass = new ADO();
            //string List_error = "";
            //foreach (W_M_ReceiveDocDetail_Entity item in List_Data)
            //{
            //    if (item.M_name!=null&&item.M_name!="")
            //    {
            //        DataTable dt = adoClass.Load_W_MasterList_Material("m_name", "m_name=N'" + item.M_name + "'");
            //        if (dt.Rows.Count >= 1)
            //        {

            //        }
            //        else
            //        {
            //            result = false;
            //            List_error += item.M_name + "\n";
            //        }
            //    }
            //}
            //if (List_error != "")
            //{
            //    MessageBox.Show("There are some unknow material: \n" + List_error, "Error");
            //}
            return result;
        }
        private void frmProductionPlanFG_Load(object sender, EventArgs e)
        {
            
            if (isEdit)
            {
                List_Data = new List<W_M_ReceiveDocDetail_Entity>();
                adoClass = new ADO();
                DataTable dt = adoClass.Load_W_M_ReceiveDocDetail("", "rm_doc_id=N'"+Documment.Rm_doc_id+"'");
                int i = 1;
                foreach (DataRow row in dt.Rows)
                {
                    W_M_ReceiveDocDetail_Entity item = new W_M_ReceiveDocDetail_Entity();
                    item.Stt = i;
                    item.Rm_doc_id = row["rm_doc_id"].ToString();
                    item.M_name = row["m_name"].ToString();
                    item.Number_carton = string.IsNullOrEmpty(row["number_carton"].ToString())?0:float.Parse(row["number_carton"].ToString());
                    item.Quantity = string.IsNullOrEmpty(row["quantity"].ToString()) ? 0 : float.Parse(row["quantity"].ToString()); 
                    item.Lot_no = row["m_lot_no"].ToString();
                    item.IsSelect = false;
                    List_Data.Add(item);
                    i++;
                }
                for (int t = i+1; t <= i+5; t++)
                {
                    W_M_ReceiveDocDetail_Entity item = new W_M_ReceiveDocDetail_Entity();
                    item.Stt = t;
                    item.IsSelect = false;
                    List_Data.Add(item);
                }
            }
            else
            {
                cboType.Text = "Material";
                Documment = new W_M_ReceiveDoc_Entity();
                List_Data = new List<W_M_ReceiveDocDetail_Entity>();
                attach_link = "";
                for (int i = 1; i < 50; i++)
                {
                    W_M_ReceiveDocDetail_Entity item = new W_M_ReceiveDocDetail_Entity();
                    item.Stt = i;
                    item.IsSelect = false;
                    List_Data.Add(item);
                }
            }
            if (cboType.Text!="Material")
            {
                gridColumn2.Caption = "RUBBER PN";
                gridColumn3.Caption = "NUMBER PALLET";
            }
            dgvResult.DataSource = List_Data.ToList();
            Current_item = new W_M_ReceiveDocDetail_Entity();
            Load_combobox();
        }

        private void btnDelete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }
        private void Load_combobox()
        {
            cboMName.DataSource = null;
            string strQry = "";
            if (cboType.Text== "Material")
            {
                strQry = "select m_name as [Part Number] from W_MasterList_Material where m_kind=N'Material'";
            }
            else
            {
                strQry = "select m_name as [Part Number] from W_MasterList_Material where m_kind=N'Rubber' ";
            }
            conn = new CmCn();
            cboMName.DataSource = conn.ExcuteDataTable(strQry);
            cboMName.DisplayMember = "Part Number";
            cboMName.ValueMember= "Part Number";
        }
        private void gvResult_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            //GridView view = sender as GridView;
            //if (e.Column == view.Columns["Status"])
            //{
            //    string status = view.GetRowCellValue(e.RowHandle, view.Columns["Status"]).ToString();
            //    switch (status)
            //    {
            //        case "OK":
            //            e.Appearance.BackColor = Color.Chartreuse;
            //            break;
            //        case "Not OK":
            //            e.Appearance.BackColor = Color.Orange;
            //            break;
            //        default:
            //            break;
            //    }
            //}
        }

        private void btnOpenAttachment_Click(object sender, EventArgs e)
        {
            if (txtDocID.Text!="")
            {
                OpenFileDialog OpenFile = new OpenFileDialog();
                OpenFile.Title = "Mở tệp tin";
                OpenFile.Filter = "PDF (.pdf)|*.pdf";
                if (OpenFile.ShowDialog() != DialogResult.Cancel)
                {
                    source = OpenFile.FileName;
                    txtAttachment.Text =txtDocID.Text+"-"+ Path.GetFileName(source);
                    attach_link = @"\\172.16.180.20\20.Public\05.IT\05.HVN_TOOL\ATTACHMENT\WAREHOUSE\" + txtAttachment.Text;
                    isNewAttach = true;
                }
            }
            else
            {
                MessageBox.Show("Error: Missing Document ID");
            }
        }

        private void repositoryItemCheckEdit1_EditValueChanged(object sender, EventArgs e)
        {
            if (gvResult.PostEditor())
            {
                gvResult.UpdateCurrentRow();
            }
        }

        private void btnPrint_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (cboType.Text=="Material")
            {
                adoClass = new ADO();
                List<W_M_ReceiveLabel_Entity> List_Print = new List<W_M_ReceiveLabel_Entity>();
                int start_Id = Generate_Label_code();
                int stt = 1;
                foreach (W_M_ReceiveDocDetail_Entity row in List_Data)
                {
                    if (row.IsSelect)
                    {
                        for (int i = 1; i <= row.Number_carton; i++)
                        {
                            W_M_ReceiveLabel_Entity item = new W_M_ReceiveLabel_Entity();
                            item.Stt = stt;
                            item.Whmr_code = "WHMR" + start_Id;
                            item.M_name = row.M_name;
                            item.Quantity = float.Parse(Math.Round(row.Quantity / row.Number_carton, 0).ToString());
                            item.Rm_doc_id = txtDocID.Text;
                            item.Created_date = DateTime.Now;
                            item.Created_user = General_Infor.username;
                            item.IsSelected = true;
                            List_Print.Add(item);
                            start_Id++;
                            stt++;
                        }
                    }
                }
                frmWHMaterial_ReceiveDocumentPrint frm = new frmWHMaterial_ReceiveDocumentPrint(List_Print, "incomming");
                frm.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Vui lòng chọn chức năng nhận cao su để in tem");
            }
        }

        private void cboType_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (cboType.Text != "Material")
            {
                gridColumn2.Caption = "RUBBER PN";
                gridColumn3.Caption = "NUMBER PALLET";
            }
            else
            {
                gridColumn2.Caption = "MATERIAL PN";
                gridColumn3.Caption = "NUMBER CARTON";
            }
            Load_combobox();
        }

        private void gvResult_RowClick(object sender, RowClickEventArgs e)
        {
            Current_item = gvResult.GetRow(gvResult.FocusedRowHandle) as W_M_ReceiveDocDetail_Entity;
        }

        private float Check_quantity(string rm_name)
        {
            adoClass = new ADO();
            DataTable dt = adoClass.Load_W_MasterList_Material("", "m_name=N'" + rm_name + "'");
            if (dt.Rows[0]["m_qty"].ToString()!="")
            {
                return float.Parse(dt.Rows[0]["m_qty"].ToString());
            }
            else
            {
                return 0;
            }
        }
            
        private int Generate_Label_code()
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

        private void btnView2_Click(object sender, EventArgs e)
        {
            if (isEdit)
            {
                if (txtAttachment2.Text != "")
                {
                    System.Diagnostics.Process.Start(Documment.Rm_doc_link2);
                }
            }
        }

        private void btnView3_Click(object sender, EventArgs e)
        {
            if (isEdit)
            {
                if (txtAttachment3.Text != "")
                {
                    System.Diagnostics.Process.Start(Documment.Rm_doc_link3);
                }
            }
        }

        private void btnAtt2_Click(object sender, EventArgs e)
        {
            if (txtDocID.Text != "")
            {
                OpenFileDialog OpenFile = new OpenFileDialog();
                OpenFile.Title = "Mở tệp tin";
                OpenFile.Filter = "PDF (.pdf)|*.pdf";
                if (OpenFile.ShowDialog() != DialogResult.Cancel)
                {
                    source2 = OpenFile.FileName;
                    txtAttachment2.Text = txtDocID.Text + "-2-" + Path.GetFileName(source2);
                    attach_link2 = @"\\172.16.180.20\20.Public\05.IT\05.HVN_TOOL\ATTACHMENT\WAREHOUSE\" + txtAttachment2.Text;
                    isNewAttach2 = true;
                }
            }
            else
            {
                MessageBox.Show("Error: Missing Document ID");
            }
        }

        private void btnAtt3_Click(object sender, EventArgs e)
        {
            if (txtDocID.Text != "")
            {
                OpenFileDialog OpenFile = new OpenFileDialog();
                OpenFile.Title = "Mở tệp tin";
                OpenFile.Filter = "PDF (.pdf)|*.pdf";
                if (OpenFile.ShowDialog() != DialogResult.Cancel)
                {
                    source3 = OpenFile.FileName;
                    txtAttachment3.Text = txtDocID.Text + "-3-" + Path.GetFileName(source3);
                    attach_link3 = @"\\172.16.180.20\20.Public\05.IT\05.HVN_TOOL\ATTACHMENT\WAREHOUSE\" + txtAttachment3.Text;
                    isNewAttach3 = true;
                }
            }
            else
            {
                MessageBox.Show("Error: Missing Document ID");
            }
        }

        private void txtAttachment_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            btnViewAttachment.PerformClick();
        }

        private void btnViewAttachment_Click(object sender, EventArgs e)
        {
            if (isEdit)
            {
                if (txtAttachment.Text!="")
                {
                    System.Diagnostics.Process.Start(Documment.Rm_doc_link);
                }
            }
        }
    }
}
