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
    public partial class frmKPIQualityMTR : Form
    {
        public frmKPIQualityMTR()
        {
            InitializeComponent();
        }
        private string MTR_Daily, MTR_m, MTR_m_1,PPM;
        public frmKPIQualityMTR(string _mtr_daily, string _mtr_m, string _mtr_m_1, string ppm)
        {
            InitializeComponent();
            MTR_Daily = _mtr_daily;
            MTR_m = _mtr_m;
            MTR_m_1 = _mtr_m_1;
            PPM = ppm;
        }
        private CmCn conn;
        private ADO adoClass;
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
            Load_Source_Data();
            lbCurrentDateTime.Text = DateTime.Now.ToString("MM/dd/yyyy hh:mm");
        }
        private void frmDashboardPlantKPI_Load(object sender, EventArgs e)
        {
           
            Load_Source_Data();
            Load_Combobox();
            MTR_Yearly();
            txtMTRLastday.Text = MTR_Daily;
            txtMTRLastMonth.Text = MTR_m_1;
            txtMTRThisMonth.Text = MTR_m;
            txtPPM.Text = PPM;
            cboMonth.Text = General_Infor.KPI_month_name;
            cboYear.Text = General_Infor.KPI_year;
            lbCurrentDateTime.Text = DateTime.Now.ToString("MM/dd/yyyy hh:mm");
            this.WindowState= FormWindowState.Maximized;
        }

        private void cboMonth_SelectionChangeCommitted(object sender, EventArgs e)
        {
            Load_Source_Data();
        }

        private void cboYear_SelectionChangeCommitted(object sender, EventArgs e)
        {
            
        }

        private void cboYear_SelectedIndexChanged(object sender, EventArgs e)
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

        private void btnMTRYearly_Click(object sender, EventArgs e)
        {
            frmKPIQualityMTRYearly frm = new frmKPIQualityMTRYearly(txtMTRLastday.Text, txtMTRThisMonth.Text, txtMTRLastMonth.Text, txtPPM.Text);
            frm.ShowDialog();
        }

        private void Load_Source_Data()
        {
            ckMTR.Series.Clear();
            string month;
            if (cboMonth.SelectedValue == null)
            {
                month = General_Infor.KPI_month;
            }
            else
            {
                month = cboMonth.SelectedValue.ToString();
            }
            string strQry = "SELECT Date,[MTR_Daily],[MTR_Cumul],[MTR_Cumul_Except_Cutting_bit],[Target],[Scrap_weight],[Total_weight] from dbo.KPI_QC_MTR \n";
            strQry += "where MONTH(Date)=N'" + month + "' and YEAR(Date)=N'" + cboYear.Text + "' order by [Date] ";
            conn = new CmCn();
            DataTable dt = conn.ExcuteDataTable(strQry);
            Series series = new Series("MTR Daily", ViewType.Bar);
            ckMTR.Series.Add(series);
            series.DataSource = dt;
            series.ArgumentScaleType = ScaleType.DateTime;
            series.ArgumentDataMember = "Date";
            series.ValueScaleType = ScaleType.Numerical;
            series.ValueDataMembers.AddRange(new string[] { "MTR_Daily" });
            series.LabelsVisibility = default;
            SideBySideBarSeriesLabel label = ckMTR.Series[0].Label as SideBySideBarSeriesLabel;
            if (label != null)
            {
                label.Position = BarSeriesLabelPosition.Top;
                //label.TextPattern = "{VP}";
            }
            SeriesViewBase viewBase = series.View;
            viewBase.Color = Color.Blue;
            //----------------------
            Series series2 = new Series("MTR Cumul", ViewType.Line);
            ckMTR.Series.Add(series2);
            series2.DataSource = dt;
            series2.ArgumentScaleType = ScaleType.DateTime;
            series2.ArgumentDataMember = "Date";
            series2.ValueScaleType = ScaleType.Numerical;
            series2.ValueDataMembers.AddRange(new string[] { "MTR_Cumul" });
            SeriesViewBase viewBase2 = series2.View;
            ((LineSeriesView)series2.View).MarkerVisibility = DevExpress.Utils.DefaultBoolean.True;
            viewBase2.Color = Color.Green;
            //------------------------------
            Series series3 = new Series("MTR Cumul Except Cutting bit", ViewType.Line);
            ckMTR.Series.Add(series3);
            series3.DataSource = dt;
            series3.ArgumentScaleType = ScaleType.DateTime;
            series3.ArgumentDataMember = "Date";
            series3.ValueScaleType = ScaleType.Numerical;
            series3.ValueDataMembers.AddRange(new string[] { "MTR_Cumul_Except_Cutting_bit" });
            SeriesViewBase viewBase3 = series3.View;
            ((LineSeriesView)series3.View).MarkerVisibility = DevExpress.Utils.DefaultBoolean.True;
            ((LineSeriesView)series3.View).LineMarkerOptions.Kind = MarkerKind.Triangle;
            viewBase3.Color = Color.Brown;
            //------------------------------
            Series series4 = new Series("Trend target", ViewType.Line);
            ckMTR.Series.Add(series4);
            series4.DataSource = dt;
            series4.ArgumentScaleType = ScaleType.DateTime;
            series4.ArgumentDataMember = "Date";
            series4.ValueScaleType = ScaleType.Numerical;
            series4.ValueDataMembers.AddRange(new string[] { "Target" });
            SeriesViewBase viewBase4 = series4.View;
            ((LineSeriesView)series4.View).LineStyle.DashStyle = DashStyle.Dash;
            viewBase4.Color = Color.Red;
            //------------------------------
            //((XYDiagram)ckMTR.Diagram).AxisY.Title.Text = "Percentage";
            //((XYDiagram)ckMTR.Diagram).AxisY.Label.TextPattern = "{VP:p0}"; show by %
            XYDiagram diagram = (XYDiagram)ckMTR.Diagram;
            //diagram.AxisY.WholeRange.MinValue = 50;
            diagram.AxisY.Title.Visibility = DevExpress.Utils.DefaultBoolean.True;
            diagram.AxisY.Title.Alignment = StringAlignment.Center;
            diagram.AxisY.Title.Text = "Percentage (%)";
            diagram.AxisY.Title.TextColor = Color.Blue;
            diagram.AxisX.Label.TextPattern = "{A:dd-MMM}";
            //diagram.AxisX.WholeRange.MinValue = 1;
            //diagram.AxisY.Title.EnableAntialiasing = DevExpress.Utils.DefaultBoolean.True;
            //diagram.AxisX.NumericScaleOptions.AutoGrid = false;
            //diagram.AxisX.NumericScaleOptions.GridSpacing = 1;
            //diagram.AxisX.NumericScaleOptions.GridAlignment = NumericGridAlignment.Ones;
        }
        private void MTR_Yearly()
        {
            ckMTRYearly.Series.Clear();
            string strQry = "select FORMAT(Date,'MMM-yy') AS Date_name,a.MTR_Cumul,a.MTR_Cumul_Except_Cutting_bit,a.Target from KPI_QC_MTR a, \n";
            strQry += "(select MAX(Date) as Date_ from KPI_QC_MTR group by Month(Date)) as b \n";
            strQry += "where a.Date = b.Date_ and a.Date>N'" + DateTime.Today.AddYears(-1).ToString("yyyy-MM-dd") + "' and a.Date<N'" + DateTime.Today.ToString("yyyy-MM-dd") + "'";
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
