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
    public partial class frmKPIHRLaborResouces : Form
    {
        public frmKPIHRLaborResouces()
        {
            InitializeComponent();
        }
        public frmKPIHRLaborResouces(string labor, string laborOT, string sop)
        {
            InitializeComponent();
            Eff_daily = labor;
            Eff_m = sop;
            Eff_m_1 = laborOT;
        }
        private string Eff_daily, Eff_m, Eff_m_1;
        private CmCn conn;
        private ADO adoClass;
        DataTable dt;
        private void btnHome_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void Load_Combobox()
        {
            adoClass = new ADO();
            cboMonth.DataSource = adoClass.Load_Parameter("month_in_year");
            cboMonth.DisplayMember = "child_name";
            cboMonth.ValueMember = "child_id";
        }
        private void frmDashboardPlantKPI_Load(object sender, EventArgs e)
        {
            Load_Source_Data();
            Load_Combobox();
            txtLabor.Text = Eff_daily;
            txtLaborAndOT.Text = Eff_m_1;
            txtSOP.Text = Eff_m;
            cboMonth.Text = General_Infor.KPI_month_name;
            cboYear.Text = General_Infor.KPI_year;
            lbCurrentDateTime.Text = DateTime.Now.ToString("MM/dd/yyyy hh:mm");
            this.WindowState = FormWindowState.Maximized;
        }

        private void panel28_Paint(object sender, PaintEventArgs e)
        {

        }

        private void cboMonth_SelectionChangeCommitted(object sender, EventArgs e)
        {
            Load_Source_Data();
        }

        private void cboYear_SelectedValueChanged(object sender, EventArgs e)
        {
            Load_Source_Data();
        }

        private void btnAbsentInfo_Click(object sender, EventArgs e)
        {
            frmKPIHRAbsenteeism frm = new frmKPIHRAbsenteeism(txtLabor.Text,txtLaborAndOT.Text,txtSOP.Text);
            frm.ShowDialog();
        }

        private void Load_Source_Data()
        {
            ckAbsenteeism.Series.Clear();
            ckHeadcount.Series.Clear();
            //ckLBSpending.Series.Clear();
            ckTurnoverRate.Series.Clear();
            string month;
            if (cboMonth.SelectedValue == null)
            {
                month = General_Infor.KPI_month;
            }
            else
            {
                month = cboMonth.SelectedValue.ToString();
            }
            string strQry = "SELECT Date,[SOP],[Cumul_labor],[LaborAndOT],[HC_Indirect],[HC_Direct],[PLT_HC],[Absenteeism],[Absent_abnormal]/100 as [Absent_abnormal], \n";
            strQry += "[Absent_normal]/100 as [Absent_normal],[MOI]/100 as MOI,[MOD]/100 as [MOD],Absent_target/100 as Absent_target,[Absent_abnormal_group]/100 as [Absent_abnormal_group],Absent_off,Absent_al_cl from dbo.[KPI_HR_LaborResources]\n";
            strQry += "where MONTH(Date)=N'" + month + "' and YEAR(Date)=N'" + cboYear.Text + "' order by [Date] ";
            conn = new CmCn();
            dt = conn.ExcuteDataTable(strQry);
            //Series series = new Series("Labor + Overtime", ViewType.Bar);
            //ckLBSpending.Series.Add(series);
            //series.DataSource = dt;
            //series.ArgumentScaleType = ScaleType.DateTime;
            //series.ArgumentDataMember = "Date";
            //series.ValueScaleType = ScaleType.Numerical;
            //series.ValueDataMembers.AddRange(new string[] { "LaborAndOT" });
            //series.LabelsVisibility = default;
            //SideBySideBarSeriesLabel label = ckLBSpending.Series[0].Label as SideBySideBarSeriesLabel;
            //if (label != null)
            //{
            //    label.Position = BarSeriesLabelPosition.Top;
            //}
            //SeriesViewBase viewBase = series.View;
            //viewBase.Color = Color.Green;
            ////----------------------
            //Series series1 = new Series("Cumul labor spending", ViewType.Line);
            //ckLBSpending.Series.Add(series1);
            //series1.DataSource = dt;
            //series1.ArgumentScaleType = ScaleType.DateTime;
            //series1.ArgumentDataMember = "Date";
            //series1.ValueScaleType = ScaleType.Numerical;
            //series1.ValueDataMembers.AddRange(new string[] { "Cumul_labor" });
            //SeriesViewBase viewBase1 = series1.View;
            //((LineSeriesView)series1.View).MarkerVisibility = DevExpress.Utils.DefaultBoolean.True;
            //viewBase1.Color = Color.Blue;
            ////----------------------
            //Series series2 = new Series("SOP", ViewType.Line);
            //ckLBSpending.Series.Add(series2);
            //series2.DataSource = dt;
            //series2.ArgumentScaleType = ScaleType.DateTime;
            //series2.ArgumentDataMember = "Date";
            //series2.ValueScaleType = ScaleType.Numerical;
            //series2.ValueDataMembers.AddRange(new string[] { "SOP" });
            //SeriesViewBase viewBase2 = series2.View;
            //((LineSeriesView)series2.View).MarkerVisibility = DevExpress.Utils.DefaultBoolean.True;
            //viewBase2.Color = Color.Red;
            ////-------------------------------
            //XYDiagram diagram = (XYDiagram)ckLBSpending.Diagram;
            //diagram.AxisX.Label.TextPattern = "{A:dd-MMM}";
            //------------------------------------------------------------------------------
            //--------------------------NEW CHART---------------------------------
            Series series3 = new Series("Real HC Direct", ViewType.StackedBar);
            ckHeadcount.Series.Add(series3);
            series3.DataSource = dt;
            series3.ArgumentScaleType = ScaleType.DateTime;
            series3.ArgumentDataMember = "Date";
            series3.ValueScaleType = ScaleType.Numerical;
            series3.ValueDataMembers.AddRange(new string[] { "HC_Direct" });
            series3.LabelsVisibility = default;
            series3.View.Color = Color.RosyBrown;
            //-----------------------
            Series series9 = new Series("Real HC Indirect", ViewType.StackedBar);
            ckHeadcount.Series.Add(series9);
            series9.DataSource = dt;
            series9.ArgumentScaleType = ScaleType.DateTime;
            series9.ArgumentDataMember = "Date";
            series9.ValueDataMembers.AddRange(new string[] { "HC_Indirect" });
            series9.LabelsVisibility = default;
            series9.View.Color = Color.Blue;
            //-----------------------
            Series series4 = new Series("PLT_HC", ViewType.Line);
            ckHeadcount.Series.Add(series4);
            series4.DataSource = dt;
            series4.ArgumentScaleType = ScaleType.DateTime;
            series4.ArgumentDataMember = "Date";
            series4.ValueScaleType = ScaleType.Numerical;
            series4.ValueDataMembers.AddRange(new string[] { "PLT_HC" });
            SeriesViewBase viewBase4 = series4.View;
            ((LineSeriesView)series4.View).MarkerVisibility = DevExpress.Utils.DefaultBoolean.True;
            viewBase4.Color = Color.Red;
            ////-------------------------------
            Series series11 = new Series("MOI", ViewType.Line);
            series11.DataSource = dt;
            series11.ArgumentScaleType = ScaleType.DateTime;
            series11.ArgumentDataMember = "Date";
            series11.ValueDataMembers.AddRange(new string[] { "MOI" });
            series11.View.Color = Color.Purple;
            //-------------------------------
            //Series series12 = new Series("MOD", ViewType.Line);
            //series12.DataSource = dt;
            //series12.ArgumentScaleType = ScaleType.DateTime;
            //series12.ArgumentDataMember = "Date";
            //series12.ValueDataMembers.AddRange(new string[] { "MOD" });
            //series12.View.Color = Color.Brown;
            //-------------------------------
            ckHeadcount.Series.AddRange(new Series[] { series11 });
            XYDiagram diagram2 = (XYDiagram)ckHeadcount.Diagram;
            diagram2.AxisX.Label.TextPattern = "{A:dd-MMM}";
            //--------------------SECOND AXIS----------
            if (diagram2.SecondaryAxesY.Count > 0)
            {
                diagram2.SecondaryAxesY.Clear();
                SecondaryAxisY myAxisY2 = new SecondaryAxisY("my Y-Axis");
                ((XYDiagram)ckHeadcount.Diagram).SecondaryAxesY.Add(myAxisY2);
                ((LineSeriesView)series11.View).AxisY = myAxisY2;
                //((LineSeriesView)series12.View).AxisY = myAxisY2;
                myAxisY2.Label.TextPattern = "{VP:p0}";
                myAxisY2.WholeRange.MaxValue = 1;
                //myAxisY2.Title.Visibility = DevExpress.Utils.DefaultBoolean.True;
                //myAxisY2.Title.TextColor = Color.Red;
            }
            else
            {
                SecondaryAxisY myAxisY2 = new SecondaryAxisY("my Y-Axis");
                ((XYDiagram)ckHeadcount.Diagram).SecondaryAxesY.Add(myAxisY2);
                ((LineSeriesView)series11.View).AxisY = myAxisY2;
                //((LineSeriesView)series12.View).AxisY = myAxisY2;
                //myAxisY2.WholeRange.MaxValue = 1;
                myAxisY2.Label.TextPattern = "{VP:p0}";
                myAxisY2.Title.TextColor = Color.Red;
            }
            //-------------------------------------------------------------
            //--------------------------NEW CHART---------------------------------
            Series series5 = new Series("Absent. OFF", ViewType.StackedBar);
            series5.DataSource = dt;
            series5.ArgumentScaleType = ScaleType.DateTime;
            series5.ArgumentDataMember = "Date";
            series5.ValueDataMembers.AddRange(new string[] { "Absent_off" });
            series5.LabelsVisibility = default;
            series5.View.Color = Color.Orange;
            //-------------------------------
            Series series6 = new Series("Absent. AL+CL", ViewType.StackedBar);
            series6.DataSource = dt;
            series6.ArgumentScaleType = ScaleType.DateTime;
            series6.ArgumentDataMember = "Date";
            series6.ValueDataMembers.AddRange(new string[] { "Absent_al_cl" });
            series6.LabelsVisibility = default;
            series6.View.Color = Color.Green;
            //-------------------------------
            Series series8 = new Series("Absenteeism Actual", ViewType.Line);
            series8.DataSource = dt;
            series8.ArgumentScaleType = ScaleType.DateTime;
            series8.ArgumentDataMember = "Date";
            series8.ValueDataMembers.AddRange(new string[] { "Absent_abnormal" });
            ((LineSeriesView)series8.View).MarkerVisibility = DevExpress.Utils.DefaultBoolean.True;
            series8.View.Color = Color.Purple;
            //-------------------------------
            //Series series10 = new Series("Absent. Abnormal Group", ViewType.Line);
            //series10.DataSource = dt;
            //series10.ArgumentScaleType = ScaleType.DateTime;
            //series10.ArgumentDataMember = "Date";
            //series10.ValueDataMembers.AddRange(new string[] { "Absent_abnormal_group" });
            //((LineSeriesView)series10.View).MarkerVisibility = DevExpress.Utils.DefaultBoolean.True;
            //((LineSeriesView)series10.View).LineMarkerOptions.Kind = MarkerKind.Triangle;
            //series10.View.Color = Color.Blue;
            //-------------------------------
            Series series12 = new Series("Absenteeism Target", ViewType.Line);
            series12.DataSource = dt;
            series12.ArgumentScaleType = ScaleType.DateTime;
            series12.ArgumentDataMember = "Date";
            series12.ValueDataMembers.AddRange(new string[] { "Absent_target" });
            series12.View.Color = Color.Red;
            //-------------------------------
            ckAbsenteeism.Series.AddRange(new Series[] { series5, series6, series8, series12 });
            XYDiagram diagram3 = (XYDiagram)ckAbsenteeism.Diagram;
            diagram3.AxisX.Label.TextPattern = "{A:dd-MMM}";
            //--------------------SECOND AXIS----------
            if (diagram3.SecondaryAxesY.Count > 0)
            {
                diagram3.SecondaryAxesY.Clear();
                SecondaryAxisY myAxisY = new SecondaryAxisY("my Y-Axis");
                ((XYDiagram)ckAbsenteeism.Diagram).SecondaryAxesY.Add(myAxisY);
                ((LineSeriesView)series8.View).AxisY = myAxisY;
                //((LineSeriesView)series10.View).AxisY = myAxisY;
                ((LineSeriesView)series12.View).AxisY = myAxisY;
                myAxisY.Label.TextPattern = "{VP:p0}";
                //myAxisY.Title.Visibility = DevExpress.Utils.DefaultBoolean.True;
                //myAxisY.Title.TextColor = Color.Red;
            }
            else
            {
                SecondaryAxisY myAxisY = new SecondaryAxisY("my Y-Axis");
                ((XYDiagram)ckAbsenteeism.Diagram).SecondaryAxesY.Add(myAxisY);
                ((LineSeriesView)series8.View).AxisY = myAxisY;
                //((LineSeriesView)series10.View).AxisY = myAxisY;
                ((LineSeriesView)series12.View).AxisY = myAxisY;
                //myAxisY.Label.TextPattern = "{VP:p0}";
                //myAxisY.Title.TextColor = Color.Red;
            }
            string strQry2 = "Select * from KPI_HR_TurnoverRate \n";
            strQry2 += "where MONTH(Date)=N'" + month + "' and YEAR(Date)=N'" + cboYear.Text + "' order by [Date] ";
            conn = new CmCn();
            DataTable dt2 = conn.ExcuteDataTable(strQry2);
            Series series40 = new Series("Turnover rate (%)", ViewType.StackedBar);
            series40.DataSource = dt2;
            series40.ArgumentScaleType = ScaleType.DateTime;
            series40.ArgumentDataMember = "Date";
            series40.ValueDataMembers.AddRange(new string[] { "turnover_rate" });
            series40.LabelsVisibility = default;
            series40.Label.TextPattern = "{VP:p2}";
            series40.View.Color = Color.Red;
            ckTurnoverRate.Series.AddRange(new Series[] { series40 });
            XYDiagram diagram5 = (XYDiagram)ckTurnoverRate.Diagram;
            diagram5.AxisX.Label.TextPattern = "{A:dd-MMM}";
        }
      
    }
}
