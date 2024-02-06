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
    public partial class frmKPIQualityNQC : Form
    {
        public frmKPIQualityNQC()
        {
            InitializeComponent();
        }
        private string MTR_Daily, MTR_m, MTR_m_1, PPM;
        public frmKPIQualityNQC(string _mtr_daily, string _mtr_m, string _mtr_m_1, string ppm)
        {
            InitializeComponent();
            MTR_Daily = _mtr_daily;
            MTR_m = _mtr_m;
            MTR_m_1 = _mtr_m_1;
            PPM = ppm;
        }
        private CmCn conn;
        private void pnDS_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmDashboardPlantKPI_Load(object sender, EventArgs e)
        {
            Load_NQC_Total();
            Load_NQC();
            txtMTRLastday.Text = MTR_Daily;
            txtMTRLastMonth.Text = MTR_m_1;
            txtMTRThisMonth.Text = MTR_m;
            txtPPM.Text = PPM;
            lbCurrentDateTime.Text = DateTime.Now.ToString("MM/dd/yyyy hh:mm");
            this.WindowState = FormWindowState.Maximized;
        }
        
       
        private void Load_NQC_Total()
        {
            string strQry = "SELECT 'TOTAL MTD' as [Title] \n ";
            strQry += " ,round(sum([Rubber_scrap]),2) as [Rubber_scrap] \n ";
            strQry += " ,round(sum([Sorting_internal]),2) as [Sorting_internal] \n ";
            strQry += " ,round(sum([Sorting_external]),2) as [Sorting_external] \n ";
            strQry += " ,round(sum([Exceptional_transportation]),2) as [Exceptional_transportation] \n ";
            strQry += " ,round(sum([Re_work_re_pack]),2) as [Re_work_re_pack] \n ";
            strQry += " ,round(sum([Business_trip]),2) as [Business_trip] \n ";
            strQry += " ,round(sum([Warranty_cost]),2) as [Warranty_cost] \n ";
            strQry += " ,round(avg([Acc_MTD]),2) as [Acc_MTD] \n ";
            strQry += " ,round(avg([Target]),2) as [Target] \n ";
            strQry += "   FROM [HVN_SYS].[dbo].[KPI_QC_NQC] \n ";
            strQry += " WHERE Month([Date]) = N'" + General_Infor.KPI_month + "' and year(Date)=" + General_Infor.KPI_year;
            conn = new CmCn();
            DataTable dt = conn.ExcuteDataTable(strQry);
            Series series1 = new Series("Rubber Scrap", ViewType.StackedBar);
            series1.DataSource = dt;
            series1.ArgumentScaleType = ScaleType.Auto;
            series1.ArgumentDataMember = "Title";
            series1.ValueDataMembers.AddRange(new string[] { "Rubber_scrap" });
            series1.LabelsVisibility = default;
            series1.View.Color = Color.Blue;
            //---------------------
            Series series2 = new Series("Sorting internal", ViewType.StackedBar);
            series2.DataSource = dt;
            series2.ArgumentScaleType = ScaleType.Auto;
            series2.ArgumentDataMember = "Title";
            series2.ValueDataMembers.AddRange(new string[] { "Sorting_internal" });
            series2.LabelsVisibility = default;
            series2.View.Color = Color.Orange;
            //---------------------
            Series series3 = new Series("Sorting External", ViewType.StackedBar);
            series3.DataSource = dt;
            series3.ArgumentScaleType = ScaleType.Auto;
            series3.ArgumentDataMember = "Title";
            series3.ValueDataMembers.AddRange(new string[] { "Sorting_external" });
            series3.LabelsVisibility = default;
            series3.View.Color = Color.LightGray;
            //---------------------
            Series series4 = new Series("Exceptional Transportation", ViewType.StackedBar);
            series4.DataSource = dt;
            series4.ArgumentScaleType = ScaleType.Auto;
            series4.ArgumentDataMember = "Title";
            series4.ValueDataMembers.AddRange(new string[] { "Exceptional_transportation" });
            series4.LabelsVisibility = default;
            series4.View.Color = Color.Yellow;
            //---------------------
            Series series5 = new Series("Re-work/Re-pack at Hut Korea", ViewType.StackedBar);
            series5.DataSource = dt;
            series5.ArgumentScaleType = ScaleType.Auto;
            series5.ArgumentDataMember = "Title";
            series5.ValueDataMembers.AddRange(new string[] { "Re_work_re_pack" });
            series5.LabelsVisibility = default;
            series5.View.Color = Color.LightSkyBlue;
            //---------------------
            Series series6 = new Series("Business trip due to Q issue", ViewType.StackedBar);
            series6.DataSource = dt;
            series6.ArgumentScaleType = ScaleType.Auto;
            series6.ArgumentDataMember = "Title";
            series6.ValueDataMembers.AddRange(new string[] { "Business_trip" });
            series6.LabelsVisibility = default;
            series6.View.Color = Color.Green;
            //---------------------
            Series series7 = new Series("Warranty Cost", ViewType.StackedBar);
            series7.DataSource = dt;
            series7.ArgumentScaleType = ScaleType.Auto;
            series7.ArgumentDataMember = "Title";
            series7.ValueDataMembers.AddRange(new string[] { "Warranty_cost" });
            series7.LabelsVisibility = default;
            series7.View.Color = Color.Purple;
            //---------------------
            Series series8 = new Series("Target", ViewType.Line);
            series8.DataSource = dt;
            series8.ArgumentScaleType = ScaleType.Auto;
            series8.ArgumentDataMember = "Title";
            series8.ValueScaleType = ScaleType.Numerical;
            series8.ValueDataMembers.AddRange(new string[] { "Target" });
            SeriesViewBase viewBase8 = series8.View;
            ((LineSeriesView)series8.View).MarkerVisibility = DevExpress.Utils.DefaultBoolean.True;
            viewBase8.Color = Color.Red;
            ckNQCTotal.Series.AddRange(new Series[] { series1, series2, series3, series4, series5, series6, series7, series8 });
            XYDiagram diagram = (XYDiagram)ckNQCTotal.Diagram;
            diagram.AxisY.Title.Visibility = DevExpress.Utils.DefaultBoolean.True;
            diagram.AxisY.Title.Alignment = StringAlignment.Center;
            diagram.AxisY.Title.Text = "Million VND";
            diagram.AxisY.Title.TextColor = Color.Blue;
        }
        private void Load_NQC()
        {
            string strQry = "SELECT [Date] \n ";
            strQry += " ,round([Rubber_scrap],2) as [Rubber_scrap] \n ";
            strQry += " ,round([Sorting_internal],2) as [Sorting_internal] \n ";
            strQry += " ,round([Sorting_external],2) as [Sorting_external] \n ";
            strQry += " ,round([Exceptional_transportation],2) as [Exceptional_transportation] \n ";
            strQry += " ,round([Re_work_re_pack],2) as [Re_work_re_pack] \n ";
            strQry += " ,round([Business_trip],2) as [Business_trip] \n ";
            strQry += " ,round([Warranty_cost],2) as [Warranty_cost] \n ";
            strQry += " ,round([Acc_MTD],4) as [Acc_MTD] \n ";
            strQry += " ,round([Target],2) as [Target] \n ";
            strQry += "   FROM [HVN_SYS].[dbo].[KPI_QC_NQC] \n ";
            strQry += "   WHERE month(Date)=N'"+General_Infor.KPI_month+"' and year(Date)="+General_Infor.KPI_year+" \n ";
            conn = new CmCn();
            DataTable dt = conn.ExcuteDataTable(strQry);
            Series series1 = new Series("Rubber Scrap", ViewType.StackedBar);
            series1.DataSource = dt;
            series1.ArgumentScaleType = ScaleType.DateTime;
            series1.ArgumentDataMember = "Date";
            series1.ValueDataMembers.AddRange(new string[] { "Rubber_scrap" });
            series1.LabelsVisibility = default;
            series1.View.Color = Color.Blue;
            //---------------------
            Series series2 = new Series("Sorting internal", ViewType.StackedBar);
            series2.DataSource = dt;
            series2.ArgumentScaleType = ScaleType.DateTime;
            series2.ArgumentDataMember = "Date";
            series2.ValueDataMembers.AddRange(new string[] { "Sorting_internal" });
            series2.LabelsVisibility = default;
            series2.View.Color = Color.Orange;
            //---------------------
            Series series3 = new Series("Sorting External", ViewType.StackedBar);
            series3.DataSource = dt;
            series3.ArgumentScaleType = ScaleType.DateTime;
            series3.ArgumentDataMember = "Date";
            series3.ValueDataMembers.AddRange(new string[] { "Sorting_external" });
            series3.LabelsVisibility = default;
            series3.View.Color = Color.LightGray;
            //---------------------
            Series series4 = new Series("Exceptional Transportation", ViewType.StackedBar);
            series4.DataSource = dt;
            series4.ArgumentScaleType = ScaleType.DateTime;
            series4.ArgumentDataMember = "Date";
            series4.ValueDataMembers.AddRange(new string[] { "Exceptional_transportation" });
            series4.LabelsVisibility = default;
            series4.View.Color = Color.Yellow;
            //---------------------
            Series series5 = new Series("Re-work/Re-pack at Hut Korea", ViewType.StackedBar);
            series5.DataSource = dt;
            series5.ArgumentScaleType = ScaleType.DateTime;
            series5.ArgumentDataMember = "Date";
            series5.ValueDataMembers.AddRange(new string[] { "Re_work_re_pack" });
            series5.LabelsVisibility = default;
            series5.View.Color = Color.LightSkyBlue;
            //---------------------
            Series series6 = new Series("Business trip due to Q issue", ViewType.StackedBar);
            series6.DataSource = dt;
            series6.ArgumentScaleType = ScaleType.DateTime;
            series6.ArgumentDataMember = "Date";
            series6.ValueDataMembers.AddRange(new string[] { "Business_trip" });
            series6.LabelsVisibility = default;
            series6.View.Color = Color.Green;
            //---------------------
            Series series7 = new Series("Warranty Cost", ViewType.StackedBar);
            series7.DataSource = dt;
            series7.ArgumentScaleType = ScaleType.DateTime;
            series7.ArgumentDataMember = "Date";
            series7.ValueDataMembers.AddRange(new string[] { "Warranty_cost" });
            series7.LabelsVisibility = default;
            series7.View.Color = Color.Purple;
            //---------------------
            Series series8 = new Series("Acc (MTD) %", ViewType.Line);
            series8.DataSource = dt;
            series8.ArgumentScaleType = ScaleType.DateTime;
            series8.ArgumentDataMember = "Date";
            series8.ValueScaleType = ScaleType.Numerical;
            series8.ValueDataMembers.AddRange(new string[] { "Acc_MTD" });
            SeriesViewBase viewBase8 = series8.View;
            //((LineSeriesView)series8.View).MarkerVisibility = DevExpress.Utils.DefaultBoolean.True;
            viewBase8.Color = Color.Red;
            //-------------------------------
            ckNQC.Series.AddRange(new Series[] { series1, series2, series3, series4, series5, series6, series7, series8 });
            XYDiagram diagram3 = (XYDiagram)ckNQC.Diagram;
            diagram3.AxisX.Label.TextPattern = "{A:dd-MMM}";
            diagram3.AxisY.Title.Visibility = DevExpress.Utils.DefaultBoolean.True;
            diagram3.AxisY.Title.Alignment = StringAlignment.Center;
            diagram3.AxisY.Title.Text = "Million VND";
            diagram3.AxisY.Title.TextColor = Color.Blue;
            SecondaryAxisY myAxisY = new SecondaryAxisY("my Y-Axis");
            ((XYDiagram)ckNQC.Diagram).SecondaryAxesY.Add(myAxisY);
            ((LineSeriesView)series8.View).AxisY = myAxisY;
            myAxisY.Label.TextPattern = "{VP:p0}";
        }
    }
}
