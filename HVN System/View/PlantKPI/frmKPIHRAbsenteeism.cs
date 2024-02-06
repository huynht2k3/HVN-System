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
    public partial class frmKPIHRAbsenteeism : Form
    {
        public frmKPIHRAbsenteeism()
        {
            InitializeComponent();
        }
        public frmKPIHRAbsenteeism(string labor, string laborOT, string sop)
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
            Load_Combobox();
            txtLabor.Text = Eff_daily;
            txtLaborAndOT.Text = Eff_m_1;
            txtSOP.Text = Eff_m;
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
            ckAbsenteeism.Series.Clear();
            string month;
            if (cboMonth.SelectedValue == null)
            {
                month = General_Infor.KPI_month;
            }
            else
            {
                month = cboMonth.SelectedValue.ToString();
            }
            string strQry = "select * from  \n ";
            strQry += " ( \n ";
            strQry += "      select Date,Absent_type,Employee_no \n ";
            strQry += "      from KPI_HR_Absenteeism \n ";
            strQry += "      where month([Date])=N'"+ month + "' and year([Date])=N'"+cboYear.Text+"' \n ";

            strQry += " ) scr \n ";
            strQry += " pivot  \n ";
            strQry += " ( \n ";
            strQry += "       sum(Employee_no) \n ";
            strQry += "       for Absent_type in ([Sick],[Unexpected],[Covid impact]) \n ";
            strQry += " ) pv \n ";
            conn = new CmCn();
            dt = conn.ExcuteDataTable(strQry);
            //-------------------------------------------------------------
            //--------------------------NEW CHART---------------------------------
            try
            {
                //Series series5 = new Series("Personal issues", ViewType.StackedBar);
                //series5.DataSource = dt;
                //series5.ArgumentScaleType = ScaleType.DateTime;
                //series5.ArgumentDataMember = "Date";
                //series5.ValueDataMembers.AddRange(new string[] { "Personal issues" });
                //series5.LabelsVisibility = default;
                //series5.View.Color = Color.Green;
                //-------------------------------
                Series series6 = new Series("Sick", ViewType.StackedBar);
                series6.DataSource = dt;
                series6.ArgumentScaleType = ScaleType.DateTime;
                series6.ArgumentDataMember = "Date";
                series6.ValueDataMembers.AddRange(new string[] { "Sick" });
                series6.LabelsVisibility = default;
                series6.View.Color = Color.Orange;
                //-------------------------------
                Series series8 = new Series("Covid impact", ViewType.StackedBar);
                series8.DataSource = dt;
                series8.ArgumentScaleType = ScaleType.DateTime;
                series8.ArgumentDataMember = "Date";
                series8.ValueDataMembers.AddRange(new string[] { "Covid impact" });
                series8.LabelsVisibility = default;
                series8.View.Color = Color.Red;
                //-------------------------------
                Series series10 = new Series("Unexpected", ViewType.StackedBar);
                series10.DataSource = dt;
                series10.ArgumentScaleType = ScaleType.DateTime;
                series10.ArgumentDataMember = "Date";
                series10.ValueDataMembers.AddRange(new string[] { "Unexpected" });
                series10.LabelsVisibility = default;
                series10.View.Color = Color.Purple;
                //-------------------------------
                ckAbsenteeism.Series.AddRange(new Series[] { series6, series8, series10 });
                XYDiagram diagram3 = (XYDiagram)ckAbsenteeism.Diagram;
                diagram3.AxisX.Label.TextPattern = "{A:dd-MMM}";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }
    }
}
