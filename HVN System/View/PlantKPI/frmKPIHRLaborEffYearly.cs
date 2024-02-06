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
    public partial class frmKPIHRLaborEffYearly : Form
    {
        public frmKPIHRLaborEffYearly()
        {
            InitializeComponent();
        }
        public frmKPIHRLaborEffYearly(string eff_daily, string eff_m, string eff_m_1)
        {
            InitializeComponent();
            Eff_daily = eff_daily;
            Eff_m = eff_m;
            Eff_m_1 = eff_m_1;
        }
        private string Eff_daily, Eff_m, Eff_m_1;
        private CmCn conn;
        private void btnHome_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmDashboardPlantKPI_Load(object sender, EventArgs e)
        {
            cboYear.Text = General_Infor.KPI_year;
            Load_Source_Data();
            Load_Eff_12M();
            txtEfficiencyLastday.Text = Eff_daily;
            txtEfficiencyLastMonth.Text = Eff_m_1;
            txtEfficiencyLastMonth.Text = Eff_m_1;
            txtEfficiencyThisMonth.Text = Eff_m;
            lbCurrentDateTime.Text = DateTime.Now.ToString("MM/dd/yyyy hh:mm");
            this.WindowState = FormWindowState.Maximized;
        }

        private void cboMonth_SelectionChangeCommitted(object sender, EventArgs e)
        {
            Load_Source_Data();
        }

        private void cboYear_SelectionChangeCommitted(object sender, EventArgs e)
        {

        }
        private void cboYear_SelectedValueChanged(object sender, EventArgs e)
        {
            Load_Source_Data();
        }
        private void Load_Source_Data()
        {
            ckEFF.Series.Clear();
            string strQry = "select DATENAME(month, Date) AS Month,a.Culmul_efficiency,a.Culmul_efficiency_new from KPI_PD_Efficiency a, \n";
            strQry += "(select MAX(Date) as Date_ from KPI_PD_Efficiency group by Month(Date)) as b \n";
            strQry += "where a.Date = b.Date_ and year(a.Date)= N'" + cboYear.Text + "' \n";
            strQry += "order by Date";
            conn = new CmCn();
            DataTable dt = conn.ExcuteDataTable(strQry);
            Series series = new Series("Efficiency", ViewType.Line);
            ckEFF.Series.Add(series);
            series.DataSource = dt;
            series.ArgumentScaleType = ScaleType.Qualitative;
            series.ArgumentDataMember = "Month";
            series.ValueScaleType = ScaleType.Numerical;
            series.ValueDataMembers.AddRange(new string[] { "Culmul_efficiency" });
            series.LabelsVisibility = default;
            SeriesViewBase viewBase = series.View;
            ((LineSeriesView)series.View).MarkerVisibility = DevExpress.Utils.DefaultBoolean.True;
            viewBase.Color = Color.Green;
            //----------------------
            XYDiagram diagram = (XYDiagram)ckEFF.Diagram;
            diagram.AxisY.WholeRange.MinValue = 0.5;
            //diagram.AxisY.Title.Visibility = DevExpress.Utils.DefaultBoolean.True;
            //diagram.AxisY.Title.Alignment = StringAlignment.Center;
            //diagram.AxisY.Title.Text = "Percentage (%)";
            diagram.AxisY.Label.TextPattern= "{VP:p0}";
            diagram.AxisX.Label.TextPattern = "{A:dd-MMM}";
            //diagram.AxisX.WholeRange.MinValue = 1;
            //diagram.AxisX.NumericScaleOptions.AutoGrid = false;
            //diagram.AxisX.NumericScaleOptions.GridSpacing = 1;
            //diagram.AxisX.NumericScaleOptions.GridAlignment = NumericGridAlignment.Ones;
        }
        private void Load_Eff_12M()
        {
            ckEffLast12M.Series.Clear();
            string strQry = "select FORMAT(Date,'MMM-yy') AS Month,a.Culmul_efficiency from KPI_PD_Efficiency a, \n";
            strQry += "(select MAX(Date) as Date_ from KPI_PD_Efficiency group by Month(Date)) as b \n";
            strQry += "where a.Date = b.Date_ and a.Date>N'"+DateTime.Today.AddYears(-1).ToString("yyyy-MM-dd")+"' and a.Date<N'" + DateTime.Today.ToString("yyyy-MM-dd") + "'";
            strQry += "order by Date";
            conn = new CmCn();
            DataTable dt = conn.ExcuteDataTable(strQry);
            Series series = new Series("Efficiency", ViewType.Line);
            ckEffLast12M.Series.Add(series);
            series.DataSource = dt;
            series.ArgumentScaleType = ScaleType.Qualitative;
            series.ArgumentDataMember = "Month";
            series.ValueScaleType = ScaleType.Numerical;
            series.ValueDataMembers.AddRange(new string[] { "Culmul_efficiency" });
            series.LabelsVisibility = default;
            series.Label.TextPattern = "{VP:p0}";
            SeriesViewBase viewBase = series.View;
            ((LineSeriesView)series.View).MarkerVisibility = DevExpress.Utils.DefaultBoolean.True;
            viewBase.Color = Color.Green;
            //----------------------
            XYDiagram diagram = (XYDiagram)ckEffLast12M.Diagram;
            diagram.AxisY.WholeRange.MinValue = 0.5;
            diagram.AxisY.Label.TextPattern = "{VP:p0}";
            diagram.AxisX.Label.TextPattern = "{A:dd-MMM}";
            //diagram.AxisX.WholeRange.MinValue = 1;
            //diagram.AxisX.NumericScaleOptions.AutoGrid = false;
            //diagram.AxisX.NumericScaleOptions.GridSpacing = 1;
            //diagram.AxisX.NumericScaleOptions.GridAlignment = NumericGridAlignment.Ones;
        }
    }
}
