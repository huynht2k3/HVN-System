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

namespace HVN_System.View.Planning
{
    public partial class frmWHMaterial_IssueDocumentDetail : Form
    {
        public frmWHMaterial_IssueDocumentDetail()
        {
            InitializeComponent();
        }
        public frmWHMaterial_IssueDocumentDetail(W_M_IssueDoc_Entity item)
        {
            InitializeComponent();
            txtDocID.Text = item.M_doc_id;
            Documment = item;
            txtDocID.ReadOnly = true;
            isEdit = true;
            if (dtpSupplyDate.Value > DateTime.Today.AddYears(-10))
            {
                dtpSupplyDate.Value = item.M_doc_supply_date;
            }
        }
        private ADO adoClass;
        private CmCn conn;
        private W_M_IssueDoc_Entity Documment;
        private List<W_M_IssueDocDetail_Entity> List_Data;
        private bool isEdit = false;
        private void btnSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            adoClass = new ADO();
            if (Check_list_data()) //Kiem tra xem co PN nao khong tim thay trong master list
            {
                if (cboCustProdCode.Text == "")
                {
                    MessageBox.Show("Lỗi: Bạn chưa chọn Mã linh kiện");
                }
                else
                {
                    adoClass.Update_W_M_IssueDoc(List_Data, txtDocID.Text, dtpSupplyDate.Value);
                    MessageBox.Show("Lưu thành công");
                    this.Close();
                }
            }
        }
        private bool Check_list_data()
        {
            bool result = true;
            adoClass = new ADO();
            string List_error = "";
            foreach (W_M_IssueDocDetail_Entity item in List_Data)
            {
                if (item.M_demand > 0)
                {
                    DataTable dt = adoClass.Load_W_MasterList_Material("m_name", "m_name=N'" + item.M_name + "'");
                    if (dt.Rows.Count >= 1)
                    {

                    }
                    else
                    {
                        result = false;
                        List_error += item.M_name + "\n";
                    }
                }

            }
            if (List_error != "")
            {
                MessageBox.Show("There are some unknow material: \n" + List_error, "Error");
            }
            return result;
        }
        private void Load_Combobox()
        {
            txtQty.Value = 0;
            if (cboRequetType.Text == "THEO KE HOACH")
            {
                adoClass = new ADO();
                cboCustProdCode.Properties.DataSource = adoClass.Load_PL_PlanFG("customer_product_code as [MA THANH PHAM],line_no as [CHUYEN],[shift] as [CA]", "plan_date=N'" + DateTime.Today.ToString("yyyy-MM-dd") + "'"); ;
                cboCustProdCode.Properties.DisplayMember = "MA THANH PHAM";
                cboCustProdCode.Properties.ValueMember = "MA THANH PHAM";
                layoutControlItem9.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
            }
            else
            {
                adoClass = new ADO();
                cboCustProdCode.Properties.DataSource = adoClass.Load_MasterListProduct("product_customer_code as [MA THANH PHAM],product_line as [CHUYEN],N'' as [CA]", ""); ;
                cboCustProdCode.Properties.DisplayMember = "MA THANH PHAM";
                cboCustProdCode.Properties.ValueMember = "MA THANH PHAM";
                layoutControlItem9.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            }

        }
        private void Load_shift()
        {
            adoClass = new ADO();
            cboLine.DataSource = adoClass.Load_MasterListLine("line_name", "");
            cboLine.DisplayMember = "line_name";
            cboLine.ValueMember = "line_name";
            conn = new CmCn();
            string strQry = "select shift_name as [CA],plus_hour_start as [GIO BAT DAU], plus_hour_end as [GIO KET THUC] \n ";
            strQry += " from P_MasterListShift \n ";
            cboShift.Properties.DataSource = conn.ExcuteDataTable(strQry);
            cboShift.Properties.DisplayMember = "CA";
            cboShift.Properties.ValueMember = "CA";
        }
        private void Load_M_doc_ID()
        {
            string strQry = "select * from W_M_IssueDoc where m_doc_supply_date=N'" + dtpSupplyDate.Value.ToString("yyyy-MM-dd") + "'";
            conn = new CmCn();
            DataTable dt = conn.ExcuteDataTable(strQry);
            txtDocID.Text = dtpSupplyDate.Value.ToString("ddMMyyyy") + "-" + (dt.Rows.Count + 1).ToString();
        }
        private void frmProductionPlanFG_Load(object sender, EventArgs e)
        {
            if (General_Infor.username != "admin")
            {
                btnDelete.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            }
            Load_shift();
            Load_Combobox();
            if (isEdit)
            {
                cboLine.Enabled = false;
                cboCustProdCode.Enabled = false;
                List_Data = new List<W_M_IssueDocDetail_Entity>();
                string strQry = "select a.*,b.Actual_qty \n ";
                strQry += " from  \n ";
                strQry += " ( select * from W_M_IssueDocDetail where m_doc_id=N'" + txtDocID.Text + "') as a \n ";
                strQry += " left join  \n ";
                strQry += " ( select m_name,product_customer_code,p_shift,p_line, SUM(quantity) as Actual_qty \n ";
                strQry += " from W_M_IssueLabel where m_doc_id=N'" + txtDocID.Text + "' \n ";
                strQry += " group by m_name,product_customer_code,p_shift,p_line \n ";
                strQry += " )as b \n ";
                strQry += " on a.m_name=b.m_name  \n ";
                strQry += " and a.product_customer_code = b.product_customer_code \n ";
                strQry += " and a.p_line=b.p_line \n ";
                strQry += " and a.p_shift=b.p_shift \n ";
                conn = new CmCn();
                DataTable dt = new DataTable();
                try
                {
                    dt = conn.ExcuteDataTable(strQry);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                if (dt.Rows.Count > 0)
                {
                    txtDocID.Text = dt.Rows[0]["m_doc_id"].ToString();
                    cboCustProdCode.Text = dt.Rows[0]["product_customer_code"].ToString();
                    cboLine.Text = dt.Rows[0]["p_line"].ToString();
                    int i = 1;
                    foreach (DataRow row in dt.Rows)
                    {
                        W_M_IssueDocDetail_Entity item = new W_M_IssueDocDetail_Entity();
                        item.Stt = i;
                        item.M_doc_id = row["m_doc_id"].ToString();
                        item.M_name = row["m_name"].ToString();
                        item.Product_customer_code = row["product_customer_code"].ToString();
                        item.M_demand = int.Parse(row["m_demand"].ToString());
                        item.P_line = row["p_line"].ToString();
                        item.P_shift = row["p_shift"].ToString();
                        if (row["Actual_qty"].ToString() == "")
                        {
                            item.Actual_qty = 0;
                        }
                        else
                        {
                            item.Actual_qty = float.Parse(row["Actual_qty"].ToString());
                        }
                        if (item.Actual_qty == item.M_demand)
                        {
                            item.Status = "OK";
                        }
                        else
                        {
                            item.Status = "Not OK";
                        }
                        List_Data.Add(item);
                        i++;
                    }
                }
            }
            else
            {
                Load_M_doc_ID();
                List_Data = new List<W_M_IssueDocDetail_Entity>();
                //for (int i = 1; i < 10; i++)
                //{
                //    W_M_IssueDocDetail_Entity item = new W_M_IssueDocDetail_Entity();
                //    item.Stt = i;
                //    List_Data.Add(item);
                //}
            }
            dgvResult.DataSource = List_Data.ToList();
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

        private void cboCustProdCode_EditValueChanged(object sender, EventArgs e)
        {
            if (isEdit == false)
            {
                List_Data = new List<W_M_IssueDocDetail_Entity>();
                if (cboRequetType.Text == "THEO KE HOACH")
                {
                    string strQry = "select * from P_MasterListProduct_BOM  \n ";
                    strQry += " where product_customer_code=N'" + cboCustProdCode.Text + "' \n ";
                    conn = new CmCn();
                    DataTable dt = conn.ExcuteDataTable(strQry);
                    if (dt.Rows.Count > 0)
                    {
                        int i = 1;
                        foreach (DataRow row in dt.Rows)
                        {
                            W_M_IssueDocDetail_Entity item = new W_M_IssueDocDetail_Entity();
                            item.Stt = i;
                            item.M_doc_id = txtDocID.Text;
                            item.M_name = row["m_name"].ToString();
                            item.Product_customer_code = row["product_customer_code"].ToString();
                            item.Qty_bom = float.Parse(row["m_quantity"].ToString());
                            item.Actual_qty = 0;
                            List_Data.Add(item);
                            i++;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Lỗi Mã hàng không có linh kiện");
                    }
                }
                else
                {
                    string strQry = "select * from P_MasterListProduct_BOM  \n ";
                    strQry += " where product_customer_code=N'" + cboCustProdCode.Text + "' \n ";
                    conn = new CmCn();
                    DataTable dt = conn.ExcuteDataTable(strQry);
                    if (dt.Rows.Count > 0)
                    {
                        int i = 1;
                        foreach (DataRow row in dt.Rows)
                        {
                            W_M_IssueDocDetail_Entity item = new W_M_IssueDocDetail_Entity();
                            item.Stt = i;
                            item.M_doc_id = txtDocID.Text;
                            item.M_name = row["m_name"].ToString();
                            item.Product_customer_code = row["product_customer_code"].ToString();
                            item.M_demand = 0;
                            item.Actual_qty = 0;
                            List_Data.Add(item);
                            i++;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Lỗi Mã hàng không có linh kiện");
                    }
                }
                dgvResult.DataSource = List_Data.ToList();
            }
        }

        private void cboRequetType_SelectionChangeCommitted(object sender, EventArgs e)
        {
            Load_Combobox();
        }

        private void txtQty_ValueChanged(object sender, EventArgs e)
        {
            float quantity = string.IsNullOrEmpty(txtQty.Value.ToString()) ? 0 : float.Parse(txtQty.Value.ToString());
            foreach (W_M_IssueDocDetail_Entity item in List_Data)
            {
                item.M_demand = item.Qty_bom * quantity;
            }
            dgvResult.DataSource = null;
            dgvResult.DataSource = List_Data.ToList();
        }
        private DateTime start_time, end_time;
        private void cboShift_EditValueChanged(object sender, EventArgs e)
        {
            GridView view = cboShift.Properties.View;
            int rowHandle = view.FocusedRowHandle;
            object value = view.GetRowCellValue(rowHandle, "GIO BAT DAU");
            start_time = DateTime.Today.AddHours(int.Parse(value.ToString()));
            object value2 = view.GetRowCellValue(rowHandle, "GIO KET THUC");
            end_time = DateTime.Today.AddHours(int.Parse(value2.ToString()));
        }
    }
}
