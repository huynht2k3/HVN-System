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
    public partial class frmWHMaterial_ReceiveDocumentConfirm : Form
    {
        public frmWHMaterial_ReceiveDocumentConfirm()
        {
            InitializeComponent();
        }
        public frmWHMaterial_ReceiveDocumentConfirm(W_M_ReceiveDoc_Entity item)
        {
            InitializeComponent();
            txtDocID.Text = item.Rm_doc_id;
            txtSupplier.Text = item.Supplier;
            txtTruckNo.Text = item.Truck_no;
            dtpReceiveDate.Value = item.Receive_date;
            Documment = item;
            cboType.Text = item.Rm_kind;
            txtDocID.ReadOnly = true;
            cboType.Enabled = false;
            txtTruckNo.Enabled = false;
            dtpReceiveDate.Enabled = false;
            txtSupplier.Enabled = false;
        }
        private ADO adoClass;
        private CmCn conn;
        private W_M_ReceiveDoc_Entity Documment;
        private W_M_ReceiveDocDetail_Entity Current_item;
        private List<W_M_ReceiveDocDetail_Entity> List_Data;
        private void btnSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            adoClass = new ADO();
            if (Check_list_data()) //Kiem tra xem co PN nao khong tim thay trong master list
            {
                Documment.Rm_doc_id = txtDocID.Text;
                Documment.Receive_date = dtpReceiveDate.Value;
                Documment.Rm_kind = cboType.Text;
                Documment.Supplier = txtSupplier.Text;
                Documment.Truck_no = txtTruckNo.Text;
                adoClass.Confirm_W_M_ReceiveDoc(List_Data);
                MessageBox.Show("Saving successfully");
            }
        }
        private bool Check_list_data()
        {
            bool result = true;
            string List_error = "";
            foreach (W_M_ReceiveDocDetail_Entity item in List_Data)
            {
                if (item.Quantity!=item.Qty_receive)
                {
                    result = false;
                    List_error += item.M_name + "\n";
                }
            }
            if (List_error != "")
            {
                MessageBox.Show("Các mã sau số nhận không khớp với invoice: \n" + List_error, "Error");
            }
            return result;
        }
        private void frmProductionPlanFG_Load(object sender, EventArgs e)
        {
            List_Data = new List<W_M_ReceiveDocDetail_Entity>();
            string strQry = "select *  \n ";
            strQry += " from W_M_ReceiveDoc a,W_M_ReceiveDocDetail b \n ";
            strQry += " where a.rm_doc_id=b.rm_doc_id \n ";
            strQry += " and receive_date=N'"+dtpReceiveDate.Value.ToString("yyyy-MM-dd")+"' and truck_no=N'"+txtTruckNo.Text+"' \n ";
            conn = new CmCn();
            DataTable dt = conn.ExcuteDataTable(strQry);
            int i = 1;
            foreach (DataRow row in dt.Rows)
            {
                W_M_ReceiveDocDetail_Entity item = new W_M_ReceiveDocDetail_Entity();
                item.Stt = i;
                item.Rm_doc_id = row["rm_doc_id"].ToString();
                item.M_name = row["m_name"].ToString();
                item.Number_carton = string.IsNullOrEmpty(row["number_carton"].ToString()) ? 0 : float.Parse(row["number_carton"].ToString());
                item.Quantity = string.IsNullOrEmpty(row["quantity"].ToString()) ? 0 : float.Parse(row["quantity"].ToString());
                item.Qty_receive = 0;
                item.Lot_no = row["m_lot_no"].ToString();
                item.IsSelect = false;
                List_Data.Add(item);
                i++;
            }
            if (cboType.Text!="Material")
            {
                gridColumn2.Caption = "RUBBER PN";
                gridColumn3.Caption = "NUMBER PALLET";
            }
            dgvResult.DataSource = List_Data.ToList();
            Current_item = new W_M_ReceiveDocDetail_Entity();
        }

        private void btnDelete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

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

 

        private void repositoryItemCheckEdit1_EditValueChanged(object sender, EventArgs e)
        {
            if (gvResult.PostEditor())
            {
                gvResult.UpdateCurrentRow();
            }
        }

        private void btnPrint_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (Check_list_data())
            {
                if (cboType.Text == "Material")
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
                                item.Created_date = DateTime.Now;
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
                    //frmWHMaterialPrintRubberLabel frm = new frmWHMaterialPrintRubberLabel(Current_item, dtpReceiveDate.Value, "CAN TO");
                    //frm.ShowDialog();
                }
            }
        }

        private void gvResult_RowClick(object sender, RowClickEventArgs e)
        {
            Current_item = gvResult.GetRow(gvResult.FocusedRowHandle) as W_M_ReceiveDocDetail_Entity;
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

    }
}
