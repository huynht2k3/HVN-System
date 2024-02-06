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
using Outlook = Microsoft.Office.Interop.Outlook;

namespace HVN_System.View.Warehouse
{
    public partial class frmWHMaterialStockWeekly : Form
    {
        public frmWHMaterialStockWeekly()
        {
            InitializeComponent();
        }
        private CmCn conn;
        private ADO adoClass;
        private void Load_Data()
        {
            string strQry = "select * from RPT_W_M_WeeklyStock where cast(report_date as date)=N'"+dtpReportDate.Value.ToString("yyyy-MM-dd")+"'";
            conn = new CmCn();
            DataTable dt = conn.ExcuteDataTable(strQry);
            dgvResult.DataSource = dt;
        }

        private void frmKPIMyAction_Load(object sender, EventArgs e)
        {
            Load_Data();
        }
        private void btnRefresh_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Load_Data();
        }

        private void gvIncident_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {

        }

        private void gvResult_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            
        }

        private void btnCheck_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }

        private void repositoryItemComboBox1_EditValueChanged(object sender, EventArgs e)
        {
            if (gvResult.PostEditor())
            {
                gvResult.UpdateCurrentRow();
            }
        }

        private void btnExport_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            adoClass = new ADO();
            adoClass.Export_Excel(dgvResult);
        }
    }
}
