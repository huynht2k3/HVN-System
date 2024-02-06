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
    public partial class frmADMLogActivities : Form
    {
        public frmADMLogActivities()
        {
            InitializeComponent();
        }
        private CmCn conn;
        

        private void frmKPIMyAction_Load(object sender, EventArgs e)
        {
            Load_data();
        }

        private void gvAction_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            
        }
        private void Load_data()
        {
            string strQry = "Select * from ADM_LogActivities";
            conn = new CmCn();
            dgvResult.DataSource = conn.ExcuteDataTable(strQry);
        }

       

        private void btnRefresh_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Load_data();
        }


        private void gvAction_DoubleClick(object sender, EventArgs e)
        {
            
        }

      
    }
}
