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
    public partial class frmKPIQualityPPM : Form
    {
        public frmKPIQualityPPM()
        {
            InitializeComponent();
        }
        private string MTR_Daily, MTR_m, MTR_m_1, PPM;
        private List<KPI_IncidentMonitoring> List_Incident_this_Month;
        public frmKPIQualityPPM(string _mtr_daily, string _mtr_m, string _mtr_m_1, string ppm)
        {
            InitializeComponent();
            MTR_Daily = _mtr_daily;
            MTR_m = _mtr_m;
            MTR_m_1 = _mtr_m_1;
            PPM = ppm;
        }
        private CmCn conn;
        private ADO adoClass;
        private void btnHome_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmDashboardPlantKPI_Load(object sender, EventArgs e)
        {
            Load_data_Incident();
            Load_PPM_Chart();
            Load_PPM_Incoming_Chart();
            Load_PPM_Customer_Chart();
            txtMTRLastday.Text = MTR_Daily;
            txtMTRLastMonth.Text = MTR_m_1;
            txtMTRThisMonth.Text = MTR_m;
            txtPPM.Text = PPM;
            lbCurrentDateTime.Text = DateTime.Now.ToString("MM/dd/yyyy hh:mm");
            this.WindowState = FormWindowState.Maximized;
        }
        private void Load_PPM_Chart()
        {
            ckPPM.Series.Clear();
            string strQry = "SELECT Date,[PPM],[PPM_Cumul],[Target] from dbo.[KPI_QC_PPM] where Month([Date]) = N'" + General_Infor.KPI_month + "' and year(Date)=" + General_Infor.KPI_year + "  order by [Date] ";
            conn = new CmCn();
            DataTable dt = conn.ExcuteDataTable(strQry);
            Series series = new Series("PPM", ViewType.Bar);
            ckPPM.Series.Add(series);
            series.DataSource = dt;
            series.ArgumentScaleType = ScaleType.DateTime;
            series.ArgumentDataMember = "Date";
            series.ValueScaleType = ScaleType.Numerical;
            series.ValueDataMembers.AddRange(new string[] { "PPM" });
            series.LabelsVisibility = default;
            SideBySideBarSeriesLabel label = ckPPM.Series[0].Label as SideBySideBarSeriesLabel;
            if (label != null)
            {
                label.Position = BarSeriesLabelPosition.Top;
            }
            SeriesViewBase viewBase = series.View;
            viewBase.Color = Color.Blue;
            //----------------------
            Series series2 = new Series("PPM Cumul", ViewType.Line);
            ckPPM.Series.Add(series2);
            series2.DataSource = dt;
            series2.ArgumentScaleType = ScaleType.DateTime;
            series2.ArgumentDataMember = "Date";
            series2.ValueScaleType = ScaleType.Numerical;
            series2.ValueDataMembers.AddRange(new string[] { "PPM_Cumul" });
            //series2.LabelsVisibility = default;
            SeriesViewBase viewBase2 = series2.View;
            ((LineSeriesView)series2.View).MarkerVisibility = DevExpress.Utils.DefaultBoolean.True;
            viewBase2.Color = Color.Green;
            //------------------------------
            Series series3 = new Series("Target", ViewType.Line);
            ckPPM.Series.Add(series3);
            series3.DataSource = dt;
            series3.ArgumentScaleType = ScaleType.DateTime;
            series3.ArgumentDataMember = "Date";
            series3.ValueScaleType = ScaleType.Numerical;
            series3.ValueDataMembers.AddRange(new string[] { "Target" });
            SeriesViewBase viewBase3 = series3.View;
            viewBase3.Color = Color.Red;
            //------------------------------
            XYDiagram diagram = (XYDiagram)ckPPM.Diagram;
            //diagram.AxisX.WholeRange.MinValue = 1;
            diagram.AxisX.NumericScaleOptions.AutoGrid = false;
            diagram.AxisX.NumericScaleOptions.GridSpacing = 1;
            diagram.AxisX.NumericScaleOptions.GridAlignment = NumericGridAlignment.Ones;
            diagram.AxisX.Label.TextPattern = "{A:dd-MMM}";
        }
        private void Load_PPM_Incoming_Chart()
        {
            ckPPMIncoming.Series.Clear();
            string strQry = "select Date, round(not_ok * 1000000 / actual, 0) as PPM_incoming, \n";
            strQry += "round(sum(not_ok) over(order by Date) * 1000000 / sum(actual) over(order by Date), 0) as PPM_incoming_cumul \n";
            strQry += "from(select Date, sum(not_ok) as not_ok, sum(actual) as actual from KPI_QC_PPMIncoming \n";
            strQry += "where Month([Date]) = N'" + General_Infor.KPI_month + "' and year(Date)=" + General_Infor.KPI_year + " group by Date) as abc \n";
            strQry += "order by abc.Date \n";
            conn = new CmCn();
            DataTable dt = conn.ExcuteDataTable(strQry);
            Series series = new Series("PPM Incoming", ViewType.Bar);
            ckPPMIncoming.Series.Add(series);
            series.DataSource = dt;
            series.ArgumentScaleType = ScaleType.DateTime;
            series.ArgumentDataMember = "Date";
            series.ValueScaleType = ScaleType.Numerical;
            series.ValueDataMembers.AddRange(new string[] { "PPM_incoming" });
            series.LabelsVisibility = default;
            SideBySideBarSeriesLabel label = ckPPM.Series[0].Label as SideBySideBarSeriesLabel;
            if (label != null)
            {
                label.Position = BarSeriesLabelPosition.Top;
            }
            SeriesViewBase viewBase = series.View;
            viewBase.Color = Color.Blue;
            //----------------------
            Series series2 = new Series("PPM Incoming Cumul", ViewType.Line);
            ckPPMIncoming.Series.Add(series2);
            series2.DataSource = dt;
            series2.ArgumentScaleType = ScaleType.DateTime;
            series2.ArgumentDataMember = "Date";
            series2.ValueScaleType = ScaleType.Numerical;
            series2.ValueDataMembers.AddRange(new string[] { "PPM_incoming_cumul" });
            SeriesViewBase viewBase2 = series2.View;
            //series2.LabelsVisibility = default;
            ((LineSeriesView)series2.View).MarkerVisibility = DevExpress.Utils.DefaultBoolean.True;
            viewBase2.Color = Color.Green;
            XYDiagram diagram = (XYDiagram)ckPPMIncoming.Diagram;
            diagram.AxisX.Label.TextPattern = "{A:dd-MMM}";
        }
        private void Load_PPM_Customer_Chart()
        {
            ckPPMCustomer.Series.Clear();
            string strQry = "select Date, claim_qty, \n";
            strQry += "round(sum(claim_qty) over(order by Date) * 1000000 / sum(shipping_qty) over(order by Date), 0) as PPM_customer_cumul \n";
            strQry += "from(select Date, sum(claim_qty) as claim_qty, sum(shipping_qty) as shipping_qty from KPI_QC_PPMCustomer \n";
            strQry += "where Month([Date]) = N'" +General_Infor.KPI_month + "' and year(Date)="+General_Infor.KPI_year+" group by Date) as abc \n";
            strQry += "order by abc.Date \n";
            conn = new CmCn();
            DataTable dt = conn.ExcuteDataTable(strQry);
            Series series = new Series("Claim Quanity", ViewType.Bar);
            ckPPMCustomer.Series.Add(series);
            series.DataSource = dt;
            series.ArgumentScaleType = ScaleType.DateTime;
            series.ArgumentDataMember = "Date";
            series.ValueScaleType = ScaleType.Numerical;
            series.ValueDataMembers.AddRange(new string[] { "claim_qty" });
            series.LabelsVisibility = default;
            SideBySideBarSeriesLabel label = ckPPM.Series[0].Label as SideBySideBarSeriesLabel;
            if (label != null)
            {
                label.Position = BarSeriesLabelPosition.Top;
            }
            SeriesViewBase viewBase = series.View;
            viewBase.Color = Color.Blue;
            //----------------------
            Series series2 = new Series("PPM Customer Cumul", ViewType.Line);
            ckPPMCustomer.Series.Add(series2);
            series2.DataSource = dt;
            series2.ArgumentScaleType = ScaleType.DateTime;
            series2.ArgumentDataMember = "Date";
            series2.ValueScaleType = ScaleType.Numerical;
            series2.ValueDataMembers.AddRange(new string[] { "PPM_customer_cumul" });
            series2.LabelsVisibility = default;
            SeriesViewBase viewBase2 = series2.View;
            ((LineSeriesView)series2.View).MarkerVisibility = DevExpress.Utils.DefaultBoolean.True;
            viewBase2.Color = Color.Green;
            XYDiagram diagram = (XYDiagram)ckPPMCustomer.Diagram;
            diagram.AxisX.Label.TextPattern = "{A:dd-MMM}";
        }
        private void Load_data_Incident()
        {
            adoClass = new ADO();
            DataTable dt = adoClass.KPI_Load_KPI_Incident("", "YEAR(created_for)=" + DateTime.Now.Year + " and [inc_theme] = N'Quality' \n order by created_for desc");
            List_Incident_this_Month = new List<KPI_IncidentMonitoring>();
            foreach (DataRow row in dt.Rows)
            {
                KPI_IncidentMonitoring item = new KPI_IncidentMonitoring();
                item.Inc_name = row["inc_name"].ToString();
                item.Inc_level = row["inc_level"].ToString();
                item.Inc_type = row["inc_type"].ToString();
                item.Inc_theme = row["inc_theme"].ToString();
                item.Inc_des = row["inc_des"].ToString();
                item.Author = row["author"].ToString();
                item.Location = row["location"].ToString();
                item.Created_time = string.IsNullOrEmpty(row["created_time"].ToString()) ? DateTime.Today : DateTime.Parse(row["created_time"].ToString());
                item.Created_for = string.IsNullOrEmpty(row["created_for"].ToString()) ? DateTime.Today : DateTime.Parse(row["created_for"].ToString());
                item.Update_time = string.IsNullOrEmpty(row["update_time"].ToString()) ? DateTime.Today : DateTime.Parse(row["update_time"].ToString());
                item.IsAction = row["isAction"].ToString();
                item.Cost= string.IsNullOrEmpty(row["cost"].ToString()) ? 0 : decimal.Parse(row["cost"].ToString());
                item.String_cost = item.Cost == 0 ? "" : item.Cost.ToString(("#,##0"));
                List_Incident_this_Month.Add(item);
            }
            dgvIncident.DataSource = List_Incident_this_Month.ToList();
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
