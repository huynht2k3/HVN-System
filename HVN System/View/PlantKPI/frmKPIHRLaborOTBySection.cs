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
    public partial class frmKPIHRLaborOTBySection : Form
    {
        public frmKPIHRLaborOTBySection()
        {
            InitializeComponent();
        }
        public frmKPIHRLaborOTBySection(string ot_lastday, string cum_Credit, string cum_OT)
        {
            InitializeComponent();
            Eff_daily = ot_lastday;
            Eff_m = cum_OT;
            Eff_m_1 = cum_Credit;
        }
        private string Eff_daily, Eff_m, Eff_m_1;
        private CmCn conn;
        private ADO adoClass;
        DataTable dt;
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
            string month;
            if (cboMonth.SelectedValue == null)
            {
                month = General_Infor.KPI_month;
            }
            else
            {
                month = cboMonth.SelectedValue.ToString();
            }
            string strQry = "SELECT [Section],SUM([OT_hours]) as OT_hours from KPI_HR_OTBySection \n";
            strQry += "where MONTH(Date)=N'" + month + "' and YEAR(Date)=N'" + cboYear.Text + "' group by [Section]  ";
            conn = new CmCn();
            dt = conn.ExcuteDataTable(strQry);
            //---------------------------------------------------
            Series series1 = new Series("Overtime hours", ViewType.StackedBar);
            ckOT.Series.Add(series1);
            series1.DataSource = dt;
            series1.ArgumentScaleType = ScaleType.Auto;
            series1.ArgumentDataMember = "Section";
            series1.ValueDataMembers.AddRange(new string[] { "OT_hours" });
            series1.LabelsVisibility = default;
            series1.View.Color = Color.Red;
            SideBySideBarSeriesLabel label = ckOT.Series[0].Label as SideBySideBarSeriesLabel;
            if (label != null)
            {
                label.Position = BarSeriesLabelPosition.Top;
            }
            //---------------------------------------
            XYDiagram diagram = (XYDiagram)ckOT.Diagram;
            diagram.AxisY.Title.Visibility = DevExpress.Utils.DefaultBoolean.True;
            diagram.AxisY.Title.Alignment = StringAlignment.Center;
            diagram.AxisY.Title.Text = "Hours (h)";
            //diagram.AxisX.Label.TextPattern = "{A:dd-MMM}";
        }

    }
}
