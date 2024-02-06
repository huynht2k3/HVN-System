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
    public partial class frmWHMaterial_IssueDocument : Form
    {
        public frmWHMaterial_IssueDocument()
        {
            InitializeComponent();
        }
        private ADO adoClass;
        private List<W_M_IssueDoc_Entity> List_Data;
        private W_M_IssueDoc_Entity Current_Doc;
        private void btnEdit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (Current_Doc.M_doc_id!="")
            {
                frmWHMaterial_IssueDocumentDetail frm = new frmWHMaterial_IssueDocumentDetail(Current_Doc);
                frm.ShowDialog();
                Load_List_Doc();
            }
        }
        private void btnRefresh_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Load_List_Doc();
        }

        private void frmProductionPlanFG_Load(object sender, EventArgs e)
        {
            Load_List_Doc();
        }
        private void Load_List_Doc()
        {
            List_Data = new List<W_M_IssueDoc_Entity>();
            adoClass = new ADO();
            DataTable dt = adoClass.Load_W_M_IssueDoc("", "m_doc_supply_date=N'"+DateTime.Today.ToString("yyyy-MM-dd")+"'");
            int i = 1;
            foreach (DataRow row in dt.Rows)
            {
                W_M_IssueDoc_Entity item = new W_M_IssueDoc_Entity();
                item.M_doc_id = row["m_doc_id"].ToString();  
                item.M_doc_supply_date= DateTime.Parse(row["m_doc_supply_date"].ToString());
                List_Data.Add(item);
                i++;
            }
            dgvResult.DataSource = List_Data.ToList();
        }
        private void btnNew_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmWHMaterial_IssueDocumentDetail frm = new frmWHMaterial_IssueDocumentDetail();
            frm.ShowDialog();
            Load_List_Doc();
        }

        private void gvResult_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            Current_Doc = gvResult.GetRow(gvResult.FocusedRowHandle) as W_M_IssueDoc_Entity;
        }

        private void btnDelete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (XtraMessageBox.Show("Bạn có chắc chắn muốn xóa yêu cầu không?", "Xóa yêu cầu", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                adoClass = new ADO();
                DataTable dt = adoClass.Load_W_M_IssueLabel("[m_name]", "[m_doc_id]=N'" + Current_Doc.M_doc_id + "'");
                if (dt.Rows.Count>0)
                {
                    MessageBox.Show("Kho đã/đang cấp hàng cho yêu cầu này. Không thể xóa");
                }
                else
                {
                    adoClass.Delete_W_M_IssueDocDetail(Current_Doc.M_doc_id);
                    Load_List_Doc();
                }
            }
        }

        private void gvResult_DoubleClick(object sender, EventArgs e)
        {
            btnEdit.PerformClick();
        }

        private void dgvResult_Click(object sender, EventArgs e)
        {

        }

        private void gvResult_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            //GridView view = sender as GridView;
            //if (e.Column == view.Columns["M_doc_status"])
            //{
            //    string status = view.GetRowCellValue(e.RowHandle, view.Columns["M_doc_status"]).ToString();
            //    switch (status)
            //    {
            //        case "Completed":
            //            e.Appearance.BackColor = Color.Chartreuse;
            //            break;
            //        case "Processing":
            //            e.Appearance.BackColor = Color.Orange;
            //            break;
            //        default:
            //            break;
            //    }
            //}
        }
    }
}
