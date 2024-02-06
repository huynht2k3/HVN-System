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
using HVN_System.View.Planning;
using HVN_System.View.Warehouse;

namespace HVN_System.View.Warehouse
{
    public partial class frmPLA_M_ReceivingPlan : Form
    {
        public frmPLA_M_ReceivingPlan()
        {
            InitializeComponent();
        }
        private ADO adoClass;
        private List<W_M_CheckingPlan_Entity> List_Data;
        private W_M_CheckingPlan_Entity Current_Doc;
        private void btnEdit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (Current_Doc.Rm_plan_id!=null)
            {
                frmPLA_M_ReceivingPlanDeital frm = new frmPLA_M_ReceivingPlanDeital(Current_Doc);
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
            List_Data = new List<W_M_CheckingPlan_Entity>();
            adoClass = new ADO();
            DataTable dt = adoClass.Load_W_M_CheckingPlan("", "");
            foreach (DataRow row in dt.Rows)
            {
                W_M_CheckingPlan_Entity item = new W_M_CheckingPlan_Entity();
                item.Rm_plan_id = row["rm_plan_id"].ToString();
                item.Check_date = string.IsNullOrEmpty(row["check_date"].ToString())?DateTime.Today:DateTime.Parse(row["check_date"].ToString());
                List_Data.Add(item);
            }
            dgvResult.DataSource = List_Data.ToList();
        }
        private void btnNew_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmPLA_M_ReceivingPlanDeital frm = new frmPLA_M_ReceivingPlanDeital();
            frm.ShowDialog();
            Load_List_Doc();
        }

        private void gvResult_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            Current_Doc = gvResult.GetRow(gvResult.FocusedRowHandle) as W_M_CheckingPlan_Entity;
        }

        private void btnDelete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (!string.IsNullOrEmpty(Current_Doc.Rm_plan_id))
            {
                if (XtraMessageBox.Show("Do you want to delete plan no '"+ Current_Doc.Rm_plan_id + "' ?", "Delete documment", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    string strQry = "delete from W_M_CheckingPlan where rm_plan_id=N'" + Current_Doc.Rm_plan_id + "' \n";
                    strQry += "delete from W_M_CheckingPlanDetail where rm_plan_id = N'" + Current_Doc.Rm_plan_id + "'\n";
                    conn = new CmCn();
                    conn.ExcuteQry(strQry);
                    Load_List_Doc();
                }
            }
        }
        private CmCn conn;
        private void gvResult_DoubleClick(object sender, EventArgs e)
        {
            btnEdit.PerformClick();
        }

        private void dgvResult_Click(object sender, EventArgs e)
        {

        }
        
    }
}
