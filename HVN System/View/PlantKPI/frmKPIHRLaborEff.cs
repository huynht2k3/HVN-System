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
    public partial class frmKPIHRLaborEff : Form
    {
        public frmKPIHRLaborEff()
        {
            InitializeComponent();
        }
        public frmKPIHRLaborEff(string eff_daily, string eff_m, string eff_m_1)
        {
            InitializeComponent();
            Eff_daily = eff_daily;
            Eff_m = eff_m;
            Eff_m_1 = eff_m_1;
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
            Load_Chart_QtyProduce();
            Load_Combobox();
            cboMonth.Text = General_Infor.KPI_month_name;
            cboYear.Text = General_Infor.KPI_year;
            txtEfficiencyLastday.Text = Eff_daily;
            txtEfficiencyLastMonth.Text = Eff_m_1;
            txtEfficiencyLastMonth.Text = Eff_m_1;
            txtEfficiencyThisMonth.Text = Eff_m;
            lbCurrentDateTime.Text = DateTime.Now.ToString("MM/dd/yyyy hh:mm");
            this.WindowState = FormWindowState.Maximized;
        }
        private void Load_Combobox()
        {
            adoClass = new ADO();
            DataTable dt = adoClass.Load_Parameter("month_in_year");
            cboMonth.DataSource = dt;
            cboMonth.DisplayMember = "child_name";
            cboMonth.ValueMember = "child_id";
        }

        private void cboMonth_SelectionChangeCommitted(object sender, EventArgs e)
        {
            Load_Source_Data();
            Load_Chart_QtyProduce();
        }

        private void cboYear_SelectionChangeCommitted(object sender, EventArgs e)
        {

        }
        private void Load_Chart_QtyProduce()
        {
            ckFGProduce.Series.Clear();
            string month;
            if (cboMonth.SelectedValue == null)
            {
                month = General_Infor.KPI_month;
            }
            else
            {
                month = cboMonth.SelectedValue.ToString();
            }
            string strQry = "SELECT Date,Quantity as [Daily Quantity] from dbo.[KPI_PD_QtyFG] \n";
            strQry += "where MONTH(Date)=N'" + month + "' and YEAR(Date)=N'" + cboYear.Text + "' order by [Date] ";
            conn = new CmCn();
            DataTable dt = conn.ExcuteDataTable(strQry);
            Series series = new Series("Daily Quantity", ViewType.Bar);
            ckFGProduce.Series.Add(series);
            series.DataSource = dt;
            series.ArgumentScaleType = ScaleType.DateTime;
            series.ArgumentDataMember = "Date";
            series.ValueScaleType = ScaleType.Numerical;
            series.ValueDataMembers.AddRange(new string[] { "Daily Quantity" });
            series.LabelsVisibility = default;
            SideBySideBarSeriesLabel label = ckFGProduce.Series[0].Label as SideBySideBarSeriesLabel;
            if (label != null)
            {
                label.Position = BarSeriesLabelPosition.Top;
            }
            SeriesViewBase viewBase = series.View;
            viewBase.Color = Color.Blue;
            //----------------------
            XYDiagram diagram = (XYDiagram)ckFGProduce.Diagram;
            diagram.AxisX.Label.TextPattern = "{A:dd-MMM}";
            //Series series2 = new Series("Efficiency Cumul", ViewType.Line);
            //ckFGProduce.Series.Add(series2);
            //series2.DataSource = dt;
            //series2.ArgumentScaleType = ScaleType.DateTime;
            //series2.ArgumentDataMember = "Date";
            //series2.ValueScaleType = ScaleType.Numerical;
            //series2.ValueDataMembers.AddRange(new string[] { "Culmul_efficiency" });
            //SeriesViewBase viewBase2 = series2.View;
            //((LineSeriesView)series2.View).MarkerVisibility = DevExpress.Utils.DefaultBoolean.True;
            //viewBase2.Color = Color.Green;
        }

        private void cboMonth_SelectedValueChanged(object sender, EventArgs e)
        {
            lbDailyQtyContent.Text = "Daily Quantity Produce in " + cboMonth.Text + " " + cboYear.Text;
        }

        private void cboYear_SelectedValueChanged(object sender, EventArgs e)
        {
            Load_Source_Data();
            Load_Chart_QtyProduce();
            lbDailyQtyContent.Text = "Daily Quantity Produce in " + cboMonth.Text + " " + cboYear.Text;
        }
        private void btnEffYearly_Click(object sender, EventArgs e)
        {
            frmKPIHRLaborEffYearly frm = new frmKPIHRLaborEffYearly();
            frm.ShowDialog();
        }

        private void Load_Source_Data()
        {
            ckEFF.Series.Clear();
            string month;
            if (cboMonth.SelectedValue == null)
            {
                month = General_Infor.KPI_month;
            }
            else
            {
                month = cboMonth.SelectedValue.ToString();
            }
            string strQry = "SELECT Date,[Standard_hours],[Actual_hours],[Daily_efficiency],[Culmul_efficiency],0.9 as [Target] from dbo.[KPI_PD_Efficiency] \n";
            strQry += "where MONTH(Date)=N'" + month + "' and YEAR(Date)=N'" + cboYear.Text + "' order by [Date] ";
            conn = new CmCn();
            DataTable dt = conn.ExcuteDataTable(strQry);
            Series series8 = new Series("Efficiency", ViewType.StackedBar);
            series8.DataSource = dt;
            series8.ArgumentScaleType = ScaleType.DateTime;
            series8.ArgumentDataMember = "Date";
            series8.ValueDataMembers.AddRange(new string[] { "Daily_efficiency" });
            series8.LabelsVisibility = default;
            series8.Label.TextPattern = "{VP:p2}";
            series8.View.Color = Color.Blue;
            
            Series series9 = new Series("Efficiency Cumul", ViewType.Line);
            series9.DataSource = dt;
            series9.ArgumentScaleType = ScaleType.DateTime;
            series9.ArgumentDataMember = "Date";
            series9.ValueDataMembers.AddRange(new string[] { "Culmul_efficiency" });
            series9.LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;
            series9.Label.TextPattern = "{VP:p2}";
            SeriesViewBase viewBase = series9.View;
            ((LineSeriesView)series9.View).MarkerVisibility = DevExpress.Utils.DefaultBoolean.True;
            viewBase.Color = Color.Orange;
            ckEFF.Series.AddRange(new Series[] {series8,series9 });
            SideBySideBarSeriesLabel label = ckEFF.Series[0].Label as SideBySideBarSeriesLabel;
            if (label != null)
            {
                label.Position = BarSeriesLabelPosition.Top;
            }
            //---------------------------------
            //------------------------------
            Series series4 = new Series("Trend target", ViewType.Line);
            ckEFF.Series.Add(series4);
            series4.DataSource = dt;
            series4.ArgumentScaleType = ScaleType.DateTime;
            series4.ArgumentDataMember = "Date";
            series4.ValueScaleType = ScaleType.Numerical;
            series4.ValueDataMembers.AddRange(new string[] { "Target" });
            SeriesViewBase viewBase4 = series4.View;
            ((LineSeriesView)series4.View).LineStyle.DashStyle = DashStyle.Dash;
            viewBase4.Color = Color.Red;
            //------------------------------
            XYDiagram diagram = (XYDiagram)ckEFF.Diagram;
            diagram.AxisY.WholeRange.MinValue = 0.3;
            diagram.AxisY.Label.TextPattern= "{VP:p0}";
            diagram.AxisX.Label.TextPattern = "{A:dd-MMM}";
            //diagram.AxisX.WholeRange.MinValue = 1;
            //diagram.AxisX.NumericScaleOptions.AutoGrid = false;
            //diagram.AxisX.NumericScaleOptions.GridSpacing = 1;
            //diagram.AxisX.NumericScaleOptions.GridAlignment = NumericGridAlignment.Ones;
        }
        private void Export_Excel(DevExpress.XtraGrid.GridControl Grid)
        {
            SaveFileDialog SaveDialog = new SaveFileDialog();
            SaveDialog.Filter = "Excel (.xlsx)|*.xlsx";
            if (SaveDialog.ShowDialog() != DialogResult.Cancel)
            {
                string ExportFilePath = SaveDialog.FileName;
                //Using System.IO;
                string FileExtenstion = Path.GetExtension(ExportFilePath);
                switch (FileExtenstion)
                {
                    case ".xlsx":
                        Grid.ExportToXlsx(ExportFilePath);
                        break;
                    default:
                        break;
                }
                if (File.Exists(ExportFilePath))
                {
                    try
                    {
                        //Try to open the file and let windows decide how to open it.
                        System.Diagnostics.Process.Start(ExportFilePath);
                    }
                    catch
                    {
                        String msg = "The file could not be opened." + Environment.NewLine + Environment.NewLine + "Path: " + ExportFilePath;
                        XtraMessageBox.Show(msg, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    String msg = "The file could not be saved." + Environment.NewLine + Environment.NewLine + "Path: " + ExportFilePath;
                    XtraMessageBox.Show(msg, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
