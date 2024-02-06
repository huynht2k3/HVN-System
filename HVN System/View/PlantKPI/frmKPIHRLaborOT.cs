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
    public partial class frmKPIHRLaborOT : Form
    {
        public frmKPIHRLaborOT()
        {
            InitializeComponent();
        }
        public frmKPIHRLaborOT(string ot_lastday, string cum_Credit, string cum_OT)
        {
            InitializeComponent();
            Eff_daily = ot_lastday;
            Eff_m = cum_OT;
            Eff_m_1 = cum_Credit;
        }
        private string Eff_daily, Eff_m, Eff_m_1;
        private CmCn conn;
        private ADO adoClass;
        private void btnHome_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmDashboardPlantKPI_Load(object sender, EventArgs e)
        {
            Load_Source_Data();
            Load_Combobox();
            txtLastday.Text = Eff_daily;
            txtCumOT.Text = Eff_m_1;
            txtCumCredit.Text = Eff_m;
            cboMonth.Text = General_Infor.KPI_month_name;
            cboYear.Text = General_Infor.KPI_year;
            lbCurrentDateTime.Text = DateTime.Now.ToString("MM/dd/yyyy hh:mm");
            this.WindowState = FormWindowState.Maximized;
        }

        private void cboMonth_SelectionChangeCommitted(object sender, EventArgs e)
        {
            Load_Source_Data();
        }

        private void cboYear_SelectedValueChanged(object sender, EventArgs e)
        {
            Load_Source_Data();
        }

        private void btnOTBySection_Click(object sender, EventArgs e)
        {
            frmKPIHRLaborOTBySection frm = new frmKPIHRLaborOTBySection(Eff_daily, Eff_m_1, Eff_m);
            frm.ShowDialog();
        }

        private void Load_Combobox()
        {
            adoClass = new ADO();
            cboMonth.DataSource = adoClass.Load_Parameter("month_in_year");
            cboMonth.DisplayMember = "child_name";
            cboMonth.ValueMember = "child_id";
        }
        private void Load_Source_Data()
        {
            ckOT.Series.Clear();
            ckOTDI.Series.Clear();
            ckOTPercent.Series.Clear();
            string month;
            if (cboMonth.SelectedValue == null)
            {
                month = General_Infor.KPI_month;
            }
            else
            {
                month = cboMonth.SelectedValue.ToString();
            }
            string strQry = "select Date,[TargetOT],[CumulTargetOT],[OT],[CumulOT],[OT_normal],[CumulOT_normal],[OT_abnormal],[CumulOT_abnormal],OT_direct \n ";
            strQry += " ,OT_indirect,OT_amount_direct,OT_percent_indirect,OT_percent_direct \n ";
            strQry += " ,round(sum(OT_amount_indirect) over(order by Date)/1000000,2) as OT_amount_indirect_cumul \n ";
            strQry += " ,round(sum(OT_amount_direct) over(order by Date)/1000000,2) as OT_amount_direct_cumul \n ";
            strQry += " from ( \n ";
            strQry += " select * from KPI_HR_OT \n ";
            strQry += " where Month([Date]) = N'" + month + "' and year(Date)=N'" + cboYear.Text + "' \n ";
            strQry += " ) as abc \n ";

            conn = new CmCn();
            DataTable dt = conn.ExcuteDataTable(strQry);
            
            //---------------------------------------------------
            Series series1 = new Series("OT Normal", ViewType.StackedBar);
            ckOT.Series.Add(series1);
            series1.DataSource = dt;
            series1.ArgumentScaleType = ScaleType.DateTime;
            series1.ArgumentDataMember = "Date";
            series1.ValueDataMembers.AddRange(new string[] { "OT_normal" });
            series1.LabelsVisibility = default;
            series1.View.Color = Color.Green;
            Series series2 = new Series("OT Abnormal", ViewType.StackedBar);
            series2.DataSource = dt;
            series2.ArgumentScaleType = ScaleType.DateTime;
            series2.ArgumentDataMember = "Date";
            series2.ValueDataMembers.AddRange(new string[] { "OT_abnormal" });
            series2.LabelsVisibility = default;
            series2.View.Color = Color.Red;
            Series series3 = new Series("Daily OT CREDIT", ViewType.Line);
            series3.DataSource = dt;
            series3.ArgumentScaleType = ScaleType.DateTime;
            series3.ArgumentDataMember = "Date";
            series3.ValueDataMembers.AddRange(new string[] { "TargetOT" });
            series3.View.Color = Color.Purple;
            ckOT.Series.AddRange(new Series[] { series2, series3 });
            //---------------------------------------
            XYDiagram diagram = (XYDiagram)ckOT.Diagram;
            //diagram.AxisX.NumericScaleOptions.AutoGrid = false;
            //diagram.AxisX.NumericScaleOptions.GridSpacing = 1;
            //diagram.AxisX.NumericScaleOptions.GridAlignment = NumericGridAlignment.Ones;
            diagram.AxisX.Label.TextPattern = "{A:dd-MMM}";
            //diagram.AxisX.Label.ResolveOverlappingOptions.AllowHide = false;
            //diagram.AxisX.QualitativeScaleOptions.AutoGrid = false;
            //diagram.AxisX.Label.ResolveOverlappingOptions.AllowRotate = false;

            //-----------------------------------------------------------------------NEW CHART---------------------------------------------
            Series series4 = new Series("OT Indirect of Direct", ViewType.StackedBar);
            series4.DataSource = dt;
            series4.ArgumentScaleType = ScaleType.DateTime;
            series4.ArgumentDataMember = "Date";
            series4.ValueDataMembers.AddRange(new string[] { "OT_indirect" });
            series4.LabelsVisibility = default;
            series4.View.Color = Color.Orange;
            Series series5 = new Series("OT Direct", ViewType.StackedBar);
            series5.DataSource = dt;
            series5.ArgumentScaleType = ScaleType.DateTime;
            series5.ArgumentDataMember = "Date";
            series5.ValueDataMembers.AddRange(new string[] { "OT_direct" });
            series5.LabelsVisibility = default;
            series5.View.Color = Color.Blue;
            Series series6 = new Series("OT IoD amount cumul", ViewType.Line);
            series6.DataSource = dt;
            series6.ArgumentScaleType = ScaleType.DateTime;
            series6.ArgumentDataMember = "Date";
            series6.ValueDataMembers.AddRange(new string[] { "OT_amount_indirect_cumul" });
            series6.LabelsVisibility = default;
            //series6.Label.TextPattern = "{V:N0}";
            series6.View.Color = Color.DarkOrange;
            Series series7 = new Series("OT Direct amount cumul", ViewType.Line);
            series7.DataSource = dt;
            series7.ArgumentScaleType = ScaleType.DateTime;
            series7.ArgumentDataMember = "Date";
            series7.ValueDataMembers.AddRange(new string[] { "OT_amount_direct_cumul" });
            series7.LabelsVisibility = default;
            //series7.Label.TextPattern= "{V:N0}";
            series7.View.Color = Color.DarkBlue;
            ckOTDI.Series.AddRange(new Series[] { series4, series5,series6,series7 });
            //---------------------------------------
            XYDiagram diagram2 = (XYDiagram)ckOTDI.Diagram;
            diagram2.AxisX.Label.TextPattern = "{A:dd-MMM}";
            diagram2.AxisY.Title.Visibility= DevExpress.Utils.DefaultBoolean.True;
            diagram2.AxisY.Title.Text = "Hours";
            diagram2.AxisY.Title.TextColor = Color.Red;
            if (diagram2.SecondaryAxesY.Count > 0)
            {
                diagram2.SecondaryAxesY.Clear();
                SecondaryAxisY myAxisY2 = new SecondaryAxisY("my Y-Axis");
                ((XYDiagram)ckOTDI.Diagram).SecondaryAxesY.Add(myAxisY2);
                ((LineSeriesView)series6.View).AxisY = myAxisY2;
                ((LineSeriesView)series7.View).AxisY = myAxisY2;
                myAxisY2.Label.TextPattern = "{V:N0}";
                myAxisY2.Title.Visibility = DevExpress.Utils.DefaultBoolean.True;
                myAxisY2.Title.Text = "Million VND";
                myAxisY2.Title.TextColor = Color.Red;
            }
            else
            {
                SecondaryAxisY myAxisY2 = new SecondaryAxisY("my Y-Axis");
                ((XYDiagram)ckOTDI.Diagram).SecondaryAxesY.Add(myAxisY2);
                ((LineSeriesView)series6.View).AxisY = myAxisY2;
                ((LineSeriesView)series7.View).AxisY = myAxisY2;
                myAxisY2.Label.TextPattern = "{V:N0}";
                myAxisY2.Title.Visibility = DevExpress.Utils.DefaultBoolean.True;
                myAxisY2.Title.Text = "Million VND";
                myAxisY2.Title.TextColor = Color.Red;
            }
            //----------------------------------
            Series series8 = new Series("% hours IoD", ViewType.StackedBar);
            series8.DataSource = dt;
            series8.ArgumentScaleType = ScaleType.DateTime;
            series8.ArgumentDataMember = "Date";
            series8.ValueDataMembers.AddRange(new string[] { "OT_percent_indirect" });
            series8.LabelsVisibility = default;
            series8.Label.TextPattern= "{VP:p2}";
            series8.View.Color = Color.Orange;
            Series series9 = new Series("% hours Direct", ViewType.StackedBar);
            series9.DataSource = dt;
            series9.ArgumentScaleType = ScaleType.DateTime;
            series9.ArgumentDataMember = "Date";
            series9.ValueDataMembers.AddRange(new string[] { "OT_percent_direct" });
            series9.LabelsVisibility = default;
            series9.Label.TextPattern = "{VP:p2}";
            series9.View.Color = Color.Blue;
            ckOTPercent.Series.AddRange(new Series[] { series8, series9});
            XYDiagram diagram3 = (XYDiagram)ckOTPercent.Diagram;
            diagram3.AxisX.Label.TextPattern = "{A:dd-MMM}";
            diagram3.AxisY.Label.TextPattern= "{VP:p0}";
        }

    }
}
