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
    public partial class frmKPIMaintEnergyIntensityDetail : Form
    {
        public frmKPIMaintEnergyIntensityDetail()
        {
            InitializeComponent();
        }
        public frmKPIMaintEnergyIntensityDetail(string m, string m1, string m2,string txtActualD_, string txtCostD_, string txtActualE_, string txtCostE_)
        {
            InitializeComponent();
            txtEIm.Text = m;
            txtEIm1.Text = m1;
            txtEIm2.Text = m2;
            txtActualD.Text = txtActualD_;
            txtActualE.Text = txtActualE_;
            txtCostD.Text = txtCostD_;
            txtCostE.Text = txtCostE_;
        }
        private CmCn conn;
        private ADO adoClass;
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
            Load_Combobox();
            cboMonth.Text = General_Infor.KPI_month_name;
            cboYear.Text = General_Infor.KPI_year;
            Load_Source_Data();
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
        private void Load_Source_Data()
        {
            Draw_Chart("MixingKWH", "MixingCOST", ckMixing);
            Draw_Chart("BAEKWH", "BAEKWH", ckBAE);
            Draw_Chart("OVKWH", "OVCOST", ckOV);
            Draw_Chart("PFKWH", "PFCOST", ckPF);
            Draw_Chart("OfficeKWH", "OffceCOST", ckOffice);
            Draw_Chart("SPKWH", "SPCOST", ckSP);
        }
        private void Draw_Chart(string fieldKWH,string field_Cost, ChartControl chart)
        {
            chart.Series.Clear();
            string month;
            if (cboMonth.SelectedValue == null)
            {
                month = General_Infor.KPI_month;
            }
            else
            {
                month = cboMonth.SelectedValue.ToString();
            }
            string strQry = "select * from  [KPI_Maint_EnergyDetail] \n ";
            strQry += "where MONTH(Date)=N'" + month + "' and YEAR(Date)=N'" + cboYear.Text + "' order by [Date] ";
            conn = new CmCn();
            DataTable dt = conn.ExcuteDataTable(strQry);
            Series series1 = new Series("KWH", ViewType.Bar);
            chart.Series.Add(series1);
            series1.DataSource = dt;
            series1.ArgumentScaleType = ScaleType.DateTime;
            series1.ArgumentDataMember = "Date";
            series1.ValueScaleType = ScaleType.Numerical;
            series1.ValueDataMembers.AddRange(new string[] { fieldKWH });
            //series1.LabelsVisibility = default;
            SeriesViewBase viewBase1 = series1.View;
            viewBase1.Color = Color.Blue;
            Series series2 = new Series("COST", ViewType.Line);
            chart.Series.Add(series2);
            series2.DataSource = dt;
            series2.ArgumentScaleType = ScaleType.DateTime;
            series2.ArgumentDataMember = "Date";
            series2.ValueDataMembers.AddRange(new string[] { field_Cost });
            ((LineSeriesView)series2.View).MarkerVisibility = DevExpress.Utils.DefaultBoolean.True;
            //series2.LabelsVisibility = default;
            //series2.Label.TextPattern = "{V:N0}";
            series2.View.Color = Color.Orange;
            XYDiagram diagram3 = (XYDiagram)chart.Diagram;
            diagram3.AxisX.QualitativeScaleOptions.AutoGrid = false;
            diagram3.AxisX.Label.ResolveOverlappingOptions.AllowHide = false;
            diagram3.AxisX.Label.TextPattern = "{A:dd}";
            //diagram3.AxisY.Title.Visibility = DevExpress.Utils.DefaultBoolean.True;
            //diagram3.AxisY.Title.Alignment = StringAlignment.Center;
            //diagram3.AxisY.Title.Text = "KWh";
            //diagram3.AxisY.Title.TextColor = Color.Blue;
            //diagram3.AxisX.Label.Angle = 30;
            if (diagram3.SecondaryAxesY.Count > 0)
            {
                diagram3.SecondaryAxesY.Clear();
                SecondaryAxisY myAxisY = new SecondaryAxisY("my Y-Axis");
                ((XYDiagram)chart.Diagram).SecondaryAxesY.Add(myAxisY);
                ((LineSeriesView)series2.View).AxisY = myAxisY;
                //myAxisY.Title.Visibility = DevExpress.Utils.DefaultBoolean.True;
                //myAxisY.Title.TextColor = Color.Orange;
                //myAxisY.Title.Text = "VND";
            }
            else
            {
                SecondaryAxisY myAxisY = new SecondaryAxisY("my Y-Axis");
                ((XYDiagram)chart.Diagram).SecondaryAxesY.Add(myAxisY);
                ((LineSeriesView)series2.View).AxisY = myAxisY;
                //myAxisY.Title.Visibility = DevExpress.Utils.DefaultBoolean.True;
                //myAxisY.Title.TextColor = Color.Orange;
                //myAxisY.Title.Text = "VND";
            }
            //chart.Legend.Visibility = DevExpress.Utils.DefaultBoolean.False;
            chart.Legend.AlignmentHorizontal = LegendAlignmentHorizontal.Center;
            chart.Legend.AlignmentVertical = LegendAlignmentVertical.BottomOutside;
            chart.Legend.Direction = LegendDirection.LeftToRight;
        }
    }
}
