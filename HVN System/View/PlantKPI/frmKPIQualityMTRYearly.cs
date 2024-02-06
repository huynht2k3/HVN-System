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
    public partial class frmKPIQualityMTRYearly : Form
    {
        public frmKPIQualityMTRYearly()
        {
            InitializeComponent();
        }
        private string MTR_Daily, MTR_m, MTR_m_1,PPM;
        public frmKPIQualityMTRYearly(string _mtr_daily, string _mtr_m, string _mtr_m_1, string ppm)
        {
            InitializeComponent();
            MTR_Daily = _mtr_daily;
            MTR_m = _mtr_m;
            MTR_m_1 = _mtr_m_1;
            PPM = ppm;
        }
        private CmCn conn;
        private void button1_Click(object sender, EventArgs e)
        {
            string progFiles = @"C:\Program Files\Common Files\Microsoft Shared\ink";
            string onScreenKeyboardPath = System.IO.Path.Combine(progFiles, "TabTip.exe");
            System.Diagnostics.Process.Start(onScreenKeyboardPath);
        }

        private void pnDS_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void tmRefreshData_Tick(object sender, EventArgs e)
        {
            lbCurrentDateTime.Text = DateTime.Now.ToString("MM/dd/yyyy hh:mm");
        }
        private void frmDashboardPlantKPI_Load(object sender, EventArgs e)
        {
            cboYearly.Text = General_Infor.KPI_year;
            MTR_Yearly();
            txtMTRLastday.Text = MTR_Daily;
            txtMTRLastMonth.Text = MTR_m_1;
            txtMTRThisMonth.Text = MTR_m;
            txtPPM.Text = PPM;
            lbCurrentDateTime.Text = DateTime.Now.ToString("MM/dd/yyyy hh:mm");
            this.WindowState= FormWindowState.Maximized;
        }

        private void cboYear_SelectionChangeCommitted(object sender, EventArgs e)
        {
            
        }

        private void cboYearly_SelectedIndexChanged(object sender, EventArgs e)
        {
            MTR_Yearly();
        }

        private void MTR_Yearly()
        {
            ckMTRYearly.Series.Clear();
            string strQry = "select DATENAME(month, Date) AS Date_name,a.MTR_Cumul,a.MTR_Cumul_Except_Cutting_bit,a.Target from KPI_QC_MTR a, \n";
            strQry += "(select MAX(Date) as Date_ from KPI_QC_MTR group by Month(Date)) as b \n";
            strQry += "where a.Date = b.Date_ and year(a.Date)= N'"+cboYearly.Text+"' \n";
            strQry += "order by Date \n";
            conn = new CmCn();
            DataTable dt = conn.ExcuteDataTable(strQry);
            Series series2 = new Series("MTR Cumul", ViewType.Line);
            ckMTRYearly.Series.Add(series2);
            series2.DataSource = dt;
            series2.ArgumentScaleType = ScaleType.Qualitative;
            series2.ArgumentDataMember = "Date_name";
            series2.ValueScaleType = ScaleType.Numerical;
            series2.ValueDataMembers.AddRange(new string[] { "MTR_Cumul" });
            series2.LabelsVisibility = default;
            SeriesViewBase viewBase2 = series2.View;
            ((LineSeriesView)series2.View).MarkerVisibility = DevExpress.Utils.DefaultBoolean.True;
            viewBase2.Color = Color.Green;
            //------------------------------
            Series series3 = new Series("MTR Cumul Except Cutting bit", ViewType.Line);
            ckMTRYearly.Series.Add(series3);
            series3.DataSource = dt;
            series3.ArgumentScaleType = ScaleType.Qualitative;
            series3.ArgumentDataMember = "Date_name";
            series3.ValueScaleType = ScaleType.Numerical;
            series3.ValueDataMembers.AddRange(new string[] { "MTR_Cumul_Except_Cutting_bit" });
            series3.LabelsVisibility = default;
            SeriesViewBase viewBase3 = series3.View;
            ((LineSeriesView)series3.View).MarkerVisibility = DevExpress.Utils.DefaultBoolean.True;
            ((LineSeriesView)series3.View).LineMarkerOptions.Kind = MarkerKind.Triangle;
            viewBase3.Color = Color.Brown;
            //------------------------------
            Series series4 = new Series("Trend target", ViewType.Line);
            ckMTRYearly.Series.Add(series4);
            series4.DataSource = dt;
            series4.ArgumentScaleType = ScaleType.Qualitative;
            series4.ArgumentDataMember = "Date_name";
            series4.ValueScaleType = ScaleType.Numerical;
            series4.ValueDataMembers.AddRange(new string[] { "Target" });
            SeriesViewBase viewBase4 = series4.View;
            ((LineSeriesView)series4.View).LineStyle.DashStyle = DashStyle.Dash;
            viewBase4.Color = Color.Red;
            //------------------------------
            XYDiagram diagram = (XYDiagram)ckMTRYearly.Diagram;
            diagram.AxisY.WholeRange.MinValue = 70;
            diagram.AxisY.Title.Visibility = DevExpress.Utils.DefaultBoolean.True;
            diagram.AxisY.Title.Alignment = StringAlignment.Center;
            diagram.AxisY.Title.Text = "Percentage (%)";
            diagram.AxisY.Title.TextColor = Color.Blue;
            //diagram.AxisX.Label.TextPattern = "{A:MMM}";
        }
    }
}
