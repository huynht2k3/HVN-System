using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HVN_System.Entity;
using HVN_System.Util;
using HVN_System.View.PlantKPI;
using Outlook = Microsoft.Office.Interop.Outlook;

namespace HVN_System.View.Production
{
    public partial class frmKPIQualitySupplierClaim : Form
    {
        public frmKPIQualitySupplierClaim()
        {
            InitializeComponent();
        }
        //private ADO adoClass;
        private CmCn conn;
        //private List<KPI_IncidentMonitoring> List_Incident;
        string ClaimID = "";
        private void Load_Supplier_Claim()
        {
            string strQry = "Select * from [KPI_QC_SupplierClaim]";
            conn = new CmCn();
            DataTable dt = conn.ExcuteDataTable(strQry);
            dgvIncident.DataSource = dt;
        }

        private void frmKPIMyAction_Load(object sender, EventArgs e)
        {
            Load_Supplier_Claim();
            gvIncident.Appearance.HeaderPanel.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
        }
        
        private void btnRefresh_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Load_Supplier_Claim();
        }

        private void gvIncident_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            ClaimID = gvIncident.GetRowCellValue(gvIncident.FocusedRowHandle, "claim_id").ToString();
        }

        private void btnSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (ClaimID != "")
            {
                frmKPIQualitySupplierClaimDetail frm = new frmKPIQualitySupplierClaimDetail(ClaimID);
                frm.ShowDialog();
                btnRefresh.PerformClick();
            }
            else
            {
                MessageBox.Show("Please select the incident before edit");
            }
        }

        private void btnDelete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (MessageBox.Show("Do you want to delete information?", "Delete item", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (ClaimID != "")
                {
                    string strQry = "Delete from [KPI_QC_SupplierClaim] where claim_id=N'" + ClaimID + "'";
                    conn = new CmCn();
                    conn.ExcuteQry(strQry);
                    MessageBox.Show("Deleted successfully");
                    btnRefresh.PerformClick();
                }
            }
        }

        private void btnNewIncident_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmKPIQualitySupplierClaimDetail frm = new frmKPIQualitySupplierClaimDetail();
            frm.ShowDialog();
            btnRefresh.PerformClick();
        }

        private void gvIncident_DoubleClick(object sender, EventArgs e)
        {
            ClaimID = gvIncident.GetRowCellValue(gvIncident.FocusedRowHandle, "claim_id").ToString();
            btnEdit.PerformClick();
        }
    }
}
