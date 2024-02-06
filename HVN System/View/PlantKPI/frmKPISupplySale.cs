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
    public partial class frmKPISupplySale : Form
    {
        public frmKPISupplySale()
        {
            InitializeComponent();
        }
        private string MTR_Daily, MTR_m, MTR_m_1,PPM;
        public frmKPISupplySale(string _sale_culmul, string _foreCast, string _archivement, string OTD)
        {
            InitializeComponent();
            MTR_Daily = _sale_culmul;
            MTR_m = _foreCast;
            MTR_m_1 = _archivement;
            PPM = OTD;
        }
        private CmCn conn;
        private ADO adoClass;
        private void pnDS_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void tmRefreshData_Tick(object sender, EventArgs e)
        {
            Load_Sale_Chart();
            Load_Sale_Detail_Chart();
            lbCurrentDateTime.Text = DateTime.Now.ToString("MM/dd/yyyy hh:mm");
        }
        private void frmDashboardPlantKPI_Load(object sender, EventArgs e)
        {
            Load_Sale_Chart();
            Load_Sale_Detail_Chart();
            Load_Combobox();
            txtCumulSale.Text = MTR_Daily;
            txtForecast.Text = MTR_m;
            txtSaleAchievement.Text = MTR_m_1;
            txtOTD.Text = PPM;
            cboMonth.Text = General_Infor.KPI_month_name;
            cboYear.Text = General_Infor.KPI_year;
            lbCurrentDateTime.Text = DateTime.Now.ToString("MM/dd/yyyy hh:mm");
            this.WindowState= FormWindowState.Maximized;
        }
        private void Load_Combobox()
        {
            adoClass = new ADO();
            cboMonth.DataSource = adoClass.Load_Parameter("month_in_year");
            cboMonth.DisplayMember = "child_name";
            cboMonth.ValueMember = "child_id";
        }

        private void cboMonth_SelectionChangeCommitted(object sender, EventArgs e)
        {
            Load_Sale_Chart();
            Load_Sale_Detail_Chart();
        }

        private void cboYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            Load_Sale_Chart();
            Load_Sale_Detail_Chart();
        }

        private void Load_Sale_Chart()
        {
            ckSale.Series.Clear();
            string month;
            if (cboMonth.SelectedValue == null)
            {
                month = General_Infor.KPI_month;
            }
            else
            {
                month = cboMonth.SelectedValue.ToString();
            }
            string strQry = "select Date,SUM(sale) as [DAILY SALES],SUM(sale_cumul) as [SALE CUMUL],SUM(sale_target) as [SALE TARGET] from KPI_LOG_SALE \n";
            strQry += "where MONTH(Date)=N'" + month + "' and YEAR(Date)=N'" + cboYear.Text + "' group by [Date] ";
            conn = new CmCn();
            DataTable dt = conn.ExcuteDataTable(strQry);
            //----------------------
            Series series = new Series("DAILY SALES", ViewType.Bar);
            ckSale.Series.Add(series);
            series.DataSource = dt;
            series.ArgumentScaleType = ScaleType.DateTime;
            series.ArgumentDataMember = "Date";
            series.ValueScaleType = ScaleType.Numerical;
            series.ValueDataMembers.AddRange(new string[] { "DAILY SALES" });
            series.LabelsVisibility = default;
            SideBySideBarSeriesLabel label = ckSale.Series[0].Label as SideBySideBarSeriesLabel;
            if (label != null)
            {
                label.Position = BarSeriesLabelPosition.Top;
                label.TextPattern = "{V:N0}";
            }
            SeriesViewBase viewBase1 = series.View;
            viewBase1.Color = Color.Blue;
            //----------------------
            Series series3 = new Series("SALE CUMUL", ViewType.Bar);
            ckSale.Series.Add(series3);
            series3.DataSource = dt;
            series3.ArgumentScaleType = ScaleType.DateTime;
            series3.ArgumentDataMember = "Date";
            series3.ValueScaleType = ScaleType.Numerical;
            series3.ValueDataMembers.AddRange(new string[] { "SALE CUMUL" });
            series3.LabelsVisibility = default;
            SideBySideBarSeriesLabel label2 = ckSale.Series[1].Label as SideBySideBarSeriesLabel;
            if (label2 != null)
            {
                label2.Position = BarSeriesLabelPosition.Top;
                label2.TextPattern = "{V:N0}";
            }
            SeriesViewBase viewBase = series3.View;
            viewBase.Color = Color.Green;
            //------------------------------
            Series series2 = new Series("SALE TARGET", ViewType.Line);
            ckSale.Series.Add(series2);
            series2.DataSource = dt;
            series2.ArgumentScaleType = ScaleType.DateTime;
            series2.ArgumentDataMember = "Date";
            series2.ValueScaleType = ScaleType.Numerical;
            series2.ValueDataMembers.AddRange(new string[] { "SALE TARGET" });
            SeriesViewBase viewBase2 = series2.View;
            ((LineSeriesView)series2.View).MarkerVisibility = DevExpress.Utils.DefaultBoolean.True;
            viewBase2.Color = Color.Orange;
            //----------------------

            //((XYDiagram)ckMTR.Diagram).AxisY.Title.Text = "Percentage";
            //((XYDiagram)ckMTR.Diagram).AxisY.Label.TextPattern = "{VP:p0}"; //show by %
            //-----------------------------------------------------
            //XYDiagram diagram = (XYDiagram)ckSale.Diagram;
            //diagram.AxisY.Title.Visibility = DevExpress.Utils.DefaultBoolean.True;
            //diagram.AxisY.Title.Alignment = StringAlignment.Center;
            //diagram.AxisY.Title.Text = "Million VND";
            //diagram.AxisY.Title.TextColor = Color.Blue;
            ((XYDiagram)ckSale.Diagram).AxisX.Label.ResolveOverlappingOptions.AllowRotate = true;
            ((XYDiagram)ckSale.Diagram).AxisX.Label.ResolveOverlappingOptions.AllowStagger = false;
            ((XYDiagram)ckSale.Diagram).AxisX.Label.ResolveOverlappingOptions.AllowHide = false;
            ((XYDiagram)ckSale.Diagram).AxisX.QualitativeScaleOptions.AutoGrid = false;
            ((XYDiagram)ckSale.Diagram).AxisX.Label.TextPattern = "{A:dd-MMM}";
            //-------------------------------------------
            //diagram.AxisX.Label.ResolveOverlappingOptions.AllowHide = false;
            //diagram.AxisX.QualitativeScaleOptions.AutoGrid = false;
            //diagram.AxisX.Label.ResolveOverlappingOptions.AllowRotate = false;
            //DateTime min = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1); // first of month
            //diagram.AxisX.WholeRange.MinValue = min;
        }
        private void Load_Sale_Detail_Chart()
        {
            ckSaleDetail.Series.Clear();
            string month;
            if (cboMonth.SelectedValue == null)
            {
                month = General_Infor.KPI_month;
            }
            else
            {
                month = cboMonth.SelectedValue.ToString();
            }
            string strQry = "select project,sale as [SALE LAST DAY],sale_cumul as [SALE CUMUL],sale_target as [SALE TARGET] from KPI_LOG_SALE where [Date]=(SELECT MAX([Date]) as result from KPI_LOG_SALE)";
            conn = new CmCn();
            DataTable dt = conn.ExcuteDataTable(strQry);
            Series series = new Series("SALE LAST DAY", ViewType.Bar);
            ckSaleDetail.Series.Add(series);
            series.DataSource = dt;
            series.ArgumentScaleType = ScaleType.Auto;
            series.ArgumentDataMember = "project";
            series.ValueScaleType = ScaleType.Numerical;
            series.ValueDataMembers.AddRange(new string[] { "SALE LAST DAY" });
            series.LabelsVisibility = default;
            SideBySideBarSeriesLabel label = ckSaleDetail.Series[0].Label as SideBySideBarSeriesLabel;
            if (label != null)
            {
                label.Position = BarSeriesLabelPosition.Top;
                label.TextPattern = "{V:N0}";
            }
            SeriesViewBase viewBase = series.View;
            viewBase.Color = Color.Blue;
            //----------------------
            Series series2 = new Series("SALE CUMUL", ViewType.Bar);
            ckSaleDetail.Series.Add(series2);
            series2.DataSource = dt;
            series2.ArgumentScaleType = ScaleType.Auto;
            series2.ArgumentDataMember = "project";
            series2.ValueScaleType = ScaleType.Numerical;
            series2.ValueDataMembers.AddRange(new string[] { "SALE CUMUL" });
            series2.LabelsVisibility = default;
            SideBySideBarSeriesLabel label2 = ckSaleDetail.Series[1].Label as SideBySideBarSeriesLabel;
            if (label2 != null)
            {
                label2.Position = BarSeriesLabelPosition.Top;
                label2.TextPattern = "{V:N0}";
            }
            SeriesViewBase viewBase2 = series2.View;
            viewBase2.Color = Color.Green;
            //---------------------------
            Series series3 = new Series("SALE TARGET", ViewType.Bar);
            ckSaleDetail.Series.Add(series3);
            series3.DataSource = dt;
            series3.ArgumentScaleType = ScaleType.Auto;
            series3.ArgumentDataMember = "project";
            series3.ValueScaleType = ScaleType.Numerical;
            series3.ValueDataMembers.AddRange(new string[] { "SALE TARGET" });
            series3.LabelsVisibility = default;
            SideBySideBarSeriesLabel label3 = ckSaleDetail.Series[2].Label as SideBySideBarSeriesLabel;
            if (label3 != null)
            {
                label3.Position = BarSeriesLabelPosition.Top;
                label3.TextPattern = "{V:N0}";
            }
            SeriesViewBase viewBase3 = series3.View;
            viewBase3.Color = Color.Orange;
            //------------------------------
            //((XYDiagram)ckMTR.Diagram).AxisY.Title.Text = "Percentage";
            //((XYDiagram)ckMTR.Diagram).AxisY.Label.TextPattern = "{VP:p0}"; //show by %
            XYDiagram diagram = (XYDiagram)ckSaleDetail.Diagram;
            diagram.AxisY.WholeRange.MinValue = 50;
            diagram.AxisY.Title.Visibility = DevExpress.Utils.DefaultBoolean.True;
            diagram.AxisY.Title.Alignment = StringAlignment.Center;
            diagram.AxisY.Title.Text = "Million VND";
            diagram.AxisY.Title.TextColor = Color.Blue;
            //DateTime min = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1); // first of month
            //diagram.AxisX.WholeRange.MinValue = min;
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
