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
using DevExpress.XtraCharts;
using DevExpress.Utils.TouchHelpers;

namespace HVN_System.View.PlantKPI
{
    public partial class frmKPIHRDisplaySafetyTRIRDetail : Form
    {
        public frmKPIHRDisplaySafetyTRIRDetail()
        {
            InitializeComponent();
        }
        public frmKPIHRDisplaySafetyTRIRDetail(string NoDayWithoutAcc)
        {
            InitializeComponent();
            txtNoDayWithoutAccident.Text = NoDayWithoutAcc;
        }
        private CmCn conn;

        private void btnHome_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        

       

        private void frmDashboardPlantKPI_Load(object sender, EventArgs e)
        {
            Load_DS_Infor();
            Load_TRIR();
            Load_TRIR_SUM();
            lbCurrentDateTime.Text = DateTime.Now.ToString("MM/dd/yyyy hh:mm");
            dtpDateTRIR.Value = DateTime.Today;
            this.WindowState= FormWindowState.Maximized;
        }
        private void cboYear_SelectionChangeCommitted(object sender, EventArgs e)
        {
        }
        private void Load_DS_Infor()
        {
            conn = new CmCn();
            string strQry = "select (select count(check_id) from KPI_IncidentMonitoring where isAction='No' and created_for>'2021-12-01' and inc_theme='Safety') + \n";
            strQry += "(select count(check_id) from KPI_ActionMonitoring where status = 'planned' and created_date > '2021-12-01' and theme = 'Safety') ";
            int Qty_Not_Done = string.IsNullOrEmpty(conn.ExcuteString(strQry)) ? 0 : int.Parse(conn.ExcuteString(strQry));
            string strQry2 = "select count(check_id) from KPI_IncidentMonitoring where inc_theme='Safety'";
            txtNumberDS.Text = conn.ExcuteString(strQry2);
            txtNumberDSNotDone.Text = Qty_Not_Done.ToString();
            txtNumberDSDone.Text = (int.Parse(txtNumberDS.Text) - Qty_Not_Done).ToString();
            
        }
        private void Load_TRIR()
        {
            string strQry3 = "select * from KPI_HR_TRIR \n";
            strQry3 += "where [Date]=N'" + dtpDateTRIR.Value.ToString("yyyy-MM-dd") + "' ";
            DataTable dt = conn.ExcuteDataTable(strQry3);
            if (dt.Rows.Count > 0)
            {
                lbDDeath.Text = string.IsNullOrEmpty(dt.Rows[0]["Death"].ToString()) ? "0" : dt.Rows[0]["Death"].ToString();
                lbDFirst.Text = string.IsNullOrEmpty(dt.Rows[0]["First_aids"].ToString()) ? "0" : dt.Rows[0]["First_aids"].ToString();
                lbDLost.Text = string.IsNullOrEmpty(dt.Rows[0]["Lost_time"].ToString()) ? "0" : dt.Rows[0]["Lost_time"].ToString();
                lbDNear.Text = string.IsNullOrEmpty(dt.Rows[0]["Near_misses"].ToString()) ? "0" : dt.Rows[0]["Near_misses"].ToString();
                lbDNoLost.Text = string.IsNullOrEmpty(dt.Rows[0]["No_lost_time"].ToString()) ? "0" : dt.Rows[0]["No_lost_time"].ToString();
                lbDRisky.Text = string.IsNullOrEmpty(dt.Rows[0]["Risky"].ToString()) ? "0" : dt.Rows[0]["Risky"].ToString();
            }
            else
            {
                lbDDeath.Text = "0";
                lbDFirst.Text = "0";
                lbDLost.Text = "0";
                lbDNear.Text = "0";
                lbDNoLost.Text = "0";
                lbDRisky.Text = "0";
            }
        }
        private void Load_TRIR_SUM()
        {
            string strQry3 = "select sum(Death) as Death, sum(First_aids) as First_aids,sum(Lost_time) as Lost_time,sum(Near_misses) as Near_misses,sum(No_lost_time) as No_lost_time,sum(Risky) as Risky  from KPI_HR_TRIR \n";
            strQry3 += "where year([Date])=N'" + dtpDateTRIR.Value.ToString("yyyy") + "' ";
            DataTable dt = conn.ExcuteDataTable(strQry3);
            if (dt.Rows.Count > 0)
            {
                lbMDeath.Text = string.IsNullOrEmpty(dt.Rows[0]["Death"].ToString()) ? "0" : dt.Rows[0]["Death"].ToString();
                lbMFirst.Text = string.IsNullOrEmpty(dt.Rows[0]["First_aids"].ToString()) ? "0" : dt.Rows[0]["First_aids"].ToString();
                lbMLost.Text = string.IsNullOrEmpty(dt.Rows[0]["Lost_time"].ToString()) ? "0" : dt.Rows[0]["Lost_time"].ToString();
                lbMNear.Text = string.IsNullOrEmpty(dt.Rows[0]["Near_misses"].ToString()) ? "0" : dt.Rows[0]["Near_misses"].ToString();
                lbMNoLost.Text = string.IsNullOrEmpty(dt.Rows[0]["No_lost_time"].ToString()) ? "0" : dt.Rows[0]["No_lost_time"].ToString();
                lbMRisky.Text = string.IsNullOrEmpty(dt.Rows[0]["Risky"].ToString()) ? "0" : dt.Rows[0]["Risky"].ToString();
            }
            else
            {
                lbMDeath.Text = "0";
                lbMFirst.Text = "0";
                lbMLost.Text = "0";
                lbMNear.Text = "0";
                lbMNoLost.Text = "0";
                lbMRisky.Text = "0";
            }
        }
        private void dtpDateTRIR_ValueChanged(object sender, EventArgs e)
        {
            txtDateTRIR.Text = dtpDateTRIR.Value.ToString("dd MMM,yyyy");
            txtYearTRIR.Text = dtpDateTRIR.Value.ToString("yyyy");
            Load_TRIR();
            Load_TRIR_SUM();
        }
    }
}
