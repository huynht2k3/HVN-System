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

namespace HVN_System.View.PlantKPI
{
    public partial class frmDashboardPlantKPI : Form
    {
        public frmDashboardPlantKPI()
        {
            InitializeComponent();
        }

        //--------------------
        //private ADO adoClass;
        private CmCn conn;
        private ADO adoClass;
        private List<KPI_PlantDashBoard> List_Data;
        private string txtNumberDSDone, txtNumberDSNotDone, txtNumberDS, txtLastDateAccident;
        //private List<KPI_IncidentMonitoring> List_Incident_this_Month;
        //private List<KPI_ActionMonitoring_Entity> List_Action_this_Month;
        //private List<KPI_ActionMonitoring_Entity> List_Action_Pending;
        private void frmDashboardPlantKPI_Load(object sender, EventArgs e)
        {
            adoClass = new ADO();
            btnEditSafety.Visible = adoClass.Check_permission(this.Name, btnEditSafety.Name, General_Infor.username);
            //load data vao datagrid
            lbCurrentDateTime.Text = DateTime.Now.ToString("MM/dd/yyyy hh:mm");
            Load_Source_Data();
            Load_TRIR();
            Load_chart();
            Load_permission();
        }

        private void Load_permission()
        {
            adoClass = new ADO();
            btnRefresh_HR.Visible = adoClass.Check_permission(this.Name, btnRefresh_HR.Name, General_Infor.username);
        }
        private void Load_Source_Data()
        {
            string strQry = "SELECT item_name, value, value_string,color,date  from dbo.KPI_PlantDashBoard";
            conn = new CmCn();
            DataTable dt = conn.ExcuteDataTable(strQry);
            List_Data = new List<KPI_PlantDashBoard>();
            foreach (DataRow row in dt.Rows)
            {
                KPI_PlantDashBoard item = new KPI_PlantDashBoard();
                item.Item_name = row["item_name"].ToString();
                item.Value = string.IsNullOrEmpty(row["value"].ToString()) ? 0 : float.Parse(row["value"].ToString());
                item.Value_string = row["value_string"].ToString();
                item.Color = row["color"].ToString();
                List_Data.Add(item);
            }
            txtNumberDSYearlyTarget.Text = Load_Value_for_Item(txtNumberDSYearlyTarget, "value");
            txtLastDateAccident = Load_Value_for_Item_name("txtLastDateAccident", "value_string");
            txtNoDayWithoutAccident.Text= (DateTime.Today-DateTime.Parse(txtLastDateAccident)).TotalDays.ToString()+ " days without accident";
            txtNumberDS = Load_Value_for_Item_name("txtNumberDS", "value");
            txtNumberDSNotDone = Load_Value_for_Item_name("txtNumberDSNotDone", "value");
            txtNumberDSDone = Load_Value_for_Item_name("txtNumberDSDone", "value");
            txtMTRLastday.Text = Load_Value_for_Item(txtMTRLastday, "value") + "%";
            txtMTRLastMonth.Text = Load_Value_for_Item(txtMTRLastMonth, "value") + "%";
            txtMTRThisMonth.Text = Load_Value_for_Item(txtMTRThisMonth, "value") + "%";
            txtEfficiencyLastday.Text = Load_Value_for_Item(txtEfficiencyLastday, "value") + "%";
            txtEfficiencyLastMonth.Text = Load_Value_for_Item(txtEfficiencyLastMonth, "value") + "%";
            txtEfficiencyThisMonth.Text = Load_Value_for_Item(txtEfficiencyThisMonth, "value") + "%";
            txtNoLaborLastday.Text = Load_Value_for_Item(txtNoLaborLastday, "value");
            txtNoLaborAndOTLastDay.Text = Load_Value_for_Item(txtNoLaborAndOTLastDay, "value");
            txtNoOTLastday.Text = Load_Value_for_Item(txtNoOTLastday, "value");
            txtCumulOT.Text = Load_Value_for_Item(txtCumulOT, "value");
            txtCulmulCreditOT.Text = Load_Value_for_Item(txtCulmulCreditOT, "value");
            txtCumulSale.Text = Load_Value_for_Item(txtCumulSale, "value") + "bd";
            txtForecast.Text = Load_Value_for_Item(txtForecast, "value");
            txtSaleAchievement.Text = Load_Value_for_Item(txtSaleAchievement, "value") + "%";
            txtExceptionAmountLastDay.Text = Load_Value_for_Item(txtExceptionAmountLastDay, "value")+" md";
            txtTargetSOP.Text = Load_Value_for_Item(txtTargetSOP, "value");
            txtGrossMargin.Text = "Gross Margin: " + Load_Value_for_Item(txtGrossMargin, "value") + "%";
            txtPPM.Text = "PPM: " + Load_Value_for_Item(txtPPM, "value");
            txtAbsenteeism.Text = Load_Value_for_Item(txtAbsenteeism, "value");
            txtSaleEstimate.Text = Load_Value_for_Item(txtSaleEstimate, "value");
            txtExceptionCumulAmount.Text= Load_Value_for_Item(txtExceptionCumulAmount, "value")+ " md";
            txtEnergyIntensityM.Text = Load_Value_for_Item(txtEnergyIntensityM, "value") + "%";
            txtEnergyIntensityM1.Text = Load_Value_for_Item(txtEnergyIntensityM1, "value") + "%";
            txtEnergyIntensityM2.Text = Load_Value_for_Item(txtEnergyIntensityM2, "value") + "%";
            txtNQCToday.Text = Load_Value_for_Item(txtNQCToday, "value") + " md";
            txtNQCLastMonth.Text= Load_Value_for_Item(txtNQCLastMonth, "value") + "%";
            txtNQCThisMonth.Text= Load_Value_for_Item(txtNQCThisMonth, "value") + "%";
            //Load Notification
            var result = List_Data.Where(x => x.Item_name == txtImportantFact.Name);
            string Notification = "";
            if (result.Count() > 0)
            {
                foreach (KPI_PlantDashBoard item in result)
                {
                    if (string.IsNullOrEmpty(Notification))
                    {
                        Notification += "-" + item.Value_string;
                    }
                    else
                    {
                        Notification += "\n-" + item.Value_string;
                    }
                }
            }
            txtImportantFact.Text = Notification;
            //Refresh_Color
            pnLabor.Refresh();
            pnMTR.Refresh();
            pnEff.Refresh();
            pnOT.Refresh();
            pnSale.Refresh();
            pnSA.Refresh();
            pnExceptional.Refresh();

        }
        private void Load_chart()
        {
            Load_chart_Info("Schaeffler", ckSchaeffler);
            Load_chart_Info("Franklin Precision Industry", ckFPI);
            Load_chart_Info("HKMC", ckHKMC);
            Load_chart_Info("Hutchinson", ckHutchinson);
            Load_chart_Info("ISUZU", ckISUZU);
            Load_chart_Info("Toyota", ckToyota);
            Load_chart_Info("Plastic Omnium", ckPOmnium);
            Load_chart_Info("Vinfast", ckVinfast);
            //color chart
            Load_color_chart("Schaeffler", ckSchaeffler);
            Load_color_chart("Franklin Precision Industry", ckFPI);
            Load_color_chart("HKMC", ckHKMC);
            Load_color_chart("Hutchinson", ckHutchinson);
            Load_color_chart("ISUZU", ckISUZU);
            Load_color_chart("Toyota", ckToyota);
            Load_color_chart("Plastic Omnium", ckPOmnium);
            Load_color_chart("Vinfast", ckVinfast);
        }
        private void Load_color_chart(string Chart_name,ChartControl chart)
        {
            conn = new CmCn();
            string qry = "select * from KPI_IncidentMonitoring where inc_customer =N'"+ Chart_name + "' and inc_status=N'Open' ";
            DataTable dt = conn.ExcuteDataTable(qry);
            if (dt.Rows.Count<1)
            {
                chart.BorderOptions.Color = Color.Chartreuse;
            }
            else
            {
                chart.BorderOptions.Color = Color.Red;
            }
        }
        private void Load_chart_Info(string Chart_name, ChartControl chart)
        {
            chart.Series.Clear();
            string strQry = "select a.Month_name,a.PPM_M,b.of_inc_no,c.sorting_time,d.inc_no,a.Month_no from  \n ";
            strQry += "    (select DATENAME(mm,[Date]) as Month_name,max(PPM) as [PPM_M],Month([Date]) as Month_no from KPI_QC_PPMByCustomer where inc_customer='" + Chart_name + "' and YEAR([Date])=N'"+General_Infor.KPI_year+"' group by Month([Date]),DATENAME(mm,[Date])) as a  \n ";
            strQry += "   left join  \n ";
            strQry += "   (select DATENAME(mm,created_for) as Month_name,count(inc_name) as of_inc_no from KPI_IncidentMonitoring where inc_customer =N'" + Chart_name + "'and YEAR([created_for])=N'" + General_Infor.KPI_year + "' and inc_level='Official claim'  \n ";
            strQry += "   group by DATENAME(mm,created_for),Month(created_for)) as b  \n ";
            strQry += "   on a.Month_name=b.Month_name  \n ";
            strQry += "   left join  \n ";
            strQry += "   (select DATENAME(mm,created_for) as Month_name,sum(sort_time) as sorting_time from KPI_IncidentMonitoring where inc_customer =N'" + Chart_name + "' and YEAR([created_for])=N'" + General_Infor.KPI_year + "' \n ";
            strQry += "   group by DATENAME(mm,created_for),Month(created_for)) as c  \n ";
            strQry += "   on a.Month_name=c.Month_name \n ";
            strQry += "   left join \n ";
            strQry += "   (select DATENAME(mm,created_for) as Month_name,count(inc_name) as inc_no from KPI_IncidentMonitoring where inc_customer =N'" + Chart_name + "' and inc_level not in ('Official claim','No claim') and YEAR([created_for])=N'" + General_Infor.KPI_year + "'\n ";
            //-- ngay 23/03/2023 Thao yeu cau bo cac item level = No Claim
            strQry += "   group by DATENAME(mm,created_for)) as d \n ";
            strQry += "   on a.Month_name = d.Month_name \n ";
            strQry += "   order by a.Month_no  \n ";
            strQry += "   \n ";

            conn = new CmCn();
            DataTable dt = conn.ExcuteDataTable(strQry);
            Series series = new Series("Non Official claim", ViewType.Bar);
            chart.Series.Add(series);
            series.DataSource = dt;
            series.ArgumentScaleType = ScaleType.Qualitative;
            series.ArgumentDataMember = "Month_no";
            series.ValueScaleType = ScaleType.Numerical;
            series.ValueDataMembers.AddRange(new string[] { "inc_no" });
            series.LabelsVisibility = default;
            SeriesViewBase viewBase = series.View;
            viewBase.Color = Color.Orange;
            Series series1 = new Series("Official claim", ViewType.Bar);
            chart.Series.Add(series1);
            series1.DataSource = dt;
            series1.ArgumentScaleType = ScaleType.Qualitative;
            series1.ArgumentDataMember = "Month_no";
            series1.ValueScaleType = ScaleType.Numerical;
            series1.ValueDataMembers.AddRange(new string[] { "of_inc_no" });
            series1.LabelsVisibility = default;
            SeriesViewBase viewBase1 = series1.View;
            viewBase1.Color = Color.Red;
            Series series2 = new Series("Sorting Time", ViewType.Line);
            chart.Series.Add(series2);
            series2.DataSource = dt;
            series2.ArgumentScaleType = ScaleType.Qualitative;
            series2.ArgumentDataMember = "Month_no";
            series2.ValueDataMembers.AddRange(new string[] { "sorting_time" });
            ((LineSeriesView)series2.View).MarkerVisibility = DevExpress.Utils.DefaultBoolean.True;
            series2.View.Color = Color.Purple;
            Series series3 = new Series("PPM", ViewType.Line);
            chart.Series.Add(series3);
            series3.DataSource = dt;
            series3.ArgumentScaleType = ScaleType.Qualitative;
            series3.ArgumentDataMember = "Month_no";
            series3.ValueDataMembers.AddRange(new string[] { "PPM_M" });
            ((LineSeriesView)series3.View).MarkerVisibility = DevExpress.Utils.DefaultBoolean.True;
            series3.View.Color = Color.Blue;
            chart.Legend.Visibility= DevExpress.Utils.DefaultBoolean.False;
            //chart.Legend.AlignmentHorizontal = LegendAlignmentHorizontal.Center;
            //chart.Legend.AlignmentVertical = LegendAlignmentVertical.BottomOutside;
            //chart.Legend.Direction = LegendDirection.LeftToRight;
            ChartTitle chartTitle1 = new ChartTitle();
            chartTitle1.Text = Chart_name;
            chartTitle1.Font = new Font("Arial", 12, FontStyle.Regular);
            chart.Titles.Add(chartTitle1);
            XYDiagram diagram3 = (XYDiagram)chart.Diagram;
            diagram3.AxisX.QualitativeScaleOptions.AutoGrid = false;
            diagram3.AxisX.Label.ResolveOverlappingOptions.AllowHide = false;
            //diagram3.AxisX.Label.Angle = 30;
            if (diagram3.SecondaryAxesY.Count > 0)
            {
                diagram3.SecondaryAxesY.Clear();
                SecondaryAxisY myAxisY = new SecondaryAxisY("my Y-Axis");
                ((XYDiagram)chart.Diagram).SecondaryAxesY.Add(myAxisY);
                ((LineSeriesView)series2.View).AxisY = myAxisY;
                ((LineSeriesView)series3.View).AxisY = myAxisY;
            }
            else
            {
                SecondaryAxisY myAxisY = new SecondaryAxisY("my Y-Axis");
                ((XYDiagram)chart.Diagram).SecondaryAxesY.Add(myAxisY);
                ((LineSeriesView)series2.View).AxisY = myAxisY;
                ((LineSeriesView)series3.View).AxisY = myAxisY;
            }
        }
        //private void Load_data_Incident()
        //{
        //    adoClass = new ADO();
        //    DataTable dt = adoClass.KPI_Load_KPI_Incident("", "month(created_for)=" + DateTime.Now.Month + " and YEAR(created_for)=" + DateTime.Now.Year + "\n order by created_for desc");
        //    List_Incident_this_Month = new List<KPI_IncidentMonitoring>();
        //    foreach (DataRow row in dt.Rows)
        //    {
        //        KPI_IncidentMonitoring item = new KPI_IncidentMonitoring();
        //        item.Inc_name = row["inc_name"].ToString();
        //        item.Inc_level = row["inc_level"].ToString();
        //        item.Inc_type = row["inc_type"].ToString();
        //        item.Inc_theme = row["inc_theme"].ToString();
        //        item.Inc_des = row["inc_des"].ToString();
        //        item.Author = row["author"].ToString();
        //        item.Location = row["location"].ToString();
        //        item.Created_time = string.IsNullOrEmpty(row["created_time"].ToString()) ? DateTime.Today : DateTime.Parse(row["created_time"].ToString());
        //        item.Created_for = string.IsNullOrEmpty(row["created_for"].ToString()) ? DateTime.Today : DateTime.Parse(row["created_for"].ToString());
        //        item.Update_time = string.IsNullOrEmpty(row["update_time"].ToString()) ? DateTime.Today : DateTime.Parse(row["update_time"].ToString());
        //        item.IsAction = row["isAction"].ToString();
        //        List_Incident_this_Month.Add(item);
        //    }
        //    dgvIncident.DataSource = List_Incident_this_Month.ToList();
        //    txtIncidentThisMonth.Text = List_Incident_this_Month.Count.ToString();
        //    //Get number safety incident;
        //    //var list_safety_incident = List_Incident_this_Month.Where(x => x.Inc_theme == "Safety");
        //    //txtNumberDSThisMonth.Text = list_safety_incident.Count().ToString();
        //    //var list_safety_last_day = List_Incident_this_Month.Where(x => x.Inc_theme == "Safety" && x.Created_for > DateTime.Today);
        //    //txtNumberDSLastDay.Text = list_safety_last_day.Count().ToString();
        //    var list_incident_without_action = List_Incident_this_Month.Where(x => x.IsAction == "No");
        //    txtIncidentWithoutAction.Text = list_incident_without_action.Count().ToString();
        //    //Get number action
        //    DataTable dt2 = adoClass.KPI_Load_KPI_Action("", "month(planned_for)=" + DateTime.Now.Month + " and YEAR(planned_for)=" + DateTime.Now.Year);
        //    List_Action_this_Month = new List<KPI_ActionMonitoring_Entity>();
        //    foreach (DataRow row in dt2.Rows)
        //    {
        //        KPI_ActionMonitoring_Entity item = new KPI_ActionMonitoring_Entity();
        //        item.Act_name = row["act_name"].ToString();
        //        item.Act_des = row["act_des"].ToString();
        //        item.Inc_name = row["inc_name"].ToString();
        //        item.Priority = row["priority"].ToString();
        //        item.Assigned_user = row["assigned_user"].ToString();
        //        item.Location = row["location"].ToString();
        //        item.Planned_for = string.IsNullOrEmpty(row["planned_for"].ToString()) ? DateTime.Today : DateTime.Parse(row["planned_for"].ToString());
        //        item.Created_date = string.IsNullOrEmpty(row["created_date"].ToString()) ? DateTime.Today : DateTime.Parse(row["created_date"].ToString());
        //        item.Last_time_commit = string.IsNullOrEmpty(row["last_time_commit"].ToString()) ? DateTime.Today : DateTime.Parse(row["last_time_commit"].ToString());
        //        item.Status = row["status"].ToString();
        //        List_Action_this_Month.Add(item);
        //    }
        //    txtActionThisMonth.Text = List_Action_this_Month.Count.ToString();
        //    var list_action_not_done = List_Action_this_Month.Where(x => x.Status != "Done" && x.Planned_for < DateTime.Today);
        //    txtActionNotDone.Text = list_action_not_done.Count().ToString();
        //    //Get list Action pending
        //    DataTable dt3 = adoClass.KPI_Load_KPI_Action("", "status not in ('Done') order by planned_for desc");
        //    List_Action_Pending = new List<KPI_ActionMonitoring_Entity>();
        //    foreach (DataRow row in dt3.Rows)
        //    {
        //        KPI_ActionMonitoring_Entity item = new KPI_ActionMonitoring_Entity();
        //        item.Act_name = row["act_name"].ToString();
        //        item.Act_des = row["act_des"].ToString();
        //        item.Inc_name = row["inc_name"].ToString();
        //        item.Priority = row["priority"].ToString();
        //        item.Assigned_user = row["assigned_user"].ToString();
        //        item.Location = row["location"].ToString();
        //        item.Planned_for = string.IsNullOrEmpty(row["planned_for"].ToString()) ? DateTime.Today : DateTime.Parse(row["planned_for"].ToString());
        //        item.Created_date = string.IsNullOrEmpty(row["created_date"].ToString()) ? DateTime.Today : DateTime.Parse(row["created_date"].ToString());
        //        item.Last_time_commit = string.IsNullOrEmpty(row["last_time_commit"].ToString()) ? DateTime.Today : DateTime.Parse(row["last_time_commit"].ToString());
        //        if (item.Planned_for > DateTime.Today.AddDays(1))
        //        {
        //            item.Status = row["status"].ToString();
        //        }
        //        else
        //        {
        //            item.Status = "Late";
        //        }
        //        List_Action_Pending.Add(item);
        //    }
        //    dgvAction.DataSource = List_Action_Pending.ToList();
        //}

        //private void Load_Incident_Last_Month()
        //{
        //    adoClass = new ADO();
        //    DataTable dt = adoClass.KPI_Load_KPI_Incident("", "month(created_for)=" + DateTime.Today.AddMonths(-1).Month + " and YEAR(created_for)=" + DateTime.Now.Year);
        //    txtIncidentLastMonth.Text = dt.Rows.Count.ToString();
        //    //Get number action
        //    DataTable dt2 = adoClass.KPI_Load_KPI_Action("", "month(planned_for)=" + DateTime.Today.AddMonths(-1).Month + " and YEAR(planned_for)=" + DateTime.Now.Year);
        //    txtActionLastMonth.Text = dt2.Rows.Count.ToString();
        //}
        private string Load_Value_for_Item(TextBox Item, string type)
        {
            var result = List_Data.FirstOrDefault(x => x.Item_name == Item.Name);
            switch (type)
            {
                case "value":
                    return result.Value.ToString();
                case "value_string":
                    return result.Value_string;
                default:
                    return null;
            }
        }
        private string Load_Value_for_Item_name(string ItemName, string type)
        {
            var result = List_Data.FirstOrDefault(x => x.Item_name == ItemName);
            switch (type)
            {
                case "value":
                    return result.Value.ToString();
                case "value_string":
                    return result.Value_string;
                default:
                    return null;
            }
        }
        private void pnDS_Paint(object sender, PaintEventArgs e)
        {
            Panel p = sender as Panel;
            var result = List_Data.FirstOrDefault(x => x.Item_name == p.Name);
            Color color;
            switch (result.Color)
            {
                case "Green":
                    color = Color.Green;
                    break;
                case "Yellow":
                    color = Color.Yellow;
                    break;
                case "Red":
                    color = Color.Red;
                    break;
                default:
                    color = Color.Green;
                    break;
            }
            if (p.Name == "pnDS")
            {
                if (txtNumberDSLastDay.Text == "0")
                {
                    color = Color.Red;
                }
                else
                {
                    color = Color.Green;
                }
            }
            ControlPaint.DrawBorder(e.Graphics, p.DisplayRectangle, color, ButtonBorderStyle.Solid);
        }
        private void Load_TRIR()
        {
            try
            {
                conn = new CmCn();
                string strQry1 = "select Death+Lost_time+No_lost_time+First_aids+Near_misses+Risky \n";
                strQry1 += "as DS_Lastday from KPI_HR_TRIR \n";
                strQry1 += "where [Date]=N'" + DateTime.Today.ToString("yyyy-MM-dd") + "' \n";
                txtNumberDSLastDay.Text = conn.ExcuteString(strQry1);
                string strQry2 = "select SUM(Death+Lost_time+No_lost_time+First_aids+Near_misses+Risky) \n";
                strQry2 += "as Total_DS_Lastday from KPI_HR_TRIR \n";
                strQry2 += "where year([Date])=N'" + DateTime.Today.ToString("yyyy") + "' and month([Date])=N'"+ DateTime.Today.ToString("MM") + "'\n";
                txtNumberDSThisMonth.Text = conn.ExcuteString(strQry2);
            }
            catch (Exception)
            {
                txtNumberDSLastDay.Text = "0";
                txtNumberDSThisMonth.Text = "0";
            }
        }
        private void tmRefreshValue_Tick(object sender, EventArgs e)
        {
            Load_TRIR();
            Load_Source_Data();
            lbCurrentDateTime.Text = DateTime.Now.ToString("MM/dd/yyyy hh:mm");
            pnDS.Refresh();
        }

        private void btnNewIncident_Click(object sender, EventArgs e)
        {
            frmKPIAddNewIncident frm = new frmKPIAddNewIncident();
            frm.ShowDialog();
        }

        private void btnNewAction_Click(object sender, EventArgs e)
        {
            frmKPIAddNewAction frm = new frmKPIAddNewAction();
            frm.ShowDialog();
        }

        private void btnMTRDetail_Click(object sender, EventArgs e)
        {
            frmKPIQualityMTR frm = new frmKPIQualityMTR(txtMTRLastday.Text, txtMTRThisMonth.Text, txtMTRLastMonth.Text, txtPPM.Text);
            frm.ShowDialog();
        }

        private void btnEffDetail_Click(object sender, EventArgs e)
        {
            frmKPIHRLaborEff frm = new frmKPIHRLaborEff(txtEfficiencyLastday.Text, txtEfficiencyThisMonth.Text, txtEfficiencyLastMonth.Text);
            frm.ShowDialog();
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

        private void btnQuality_Click(object sender, EventArgs e)
        {
            frmKPIQualityPPM frm = new frmKPIQualityPPM(txtMTRLastday.Text, txtMTRThisMonth.Text, txtMTRLastMonth.Text, txtPPM.Text);
            frm.ShowDialog();
        }

        private void btnLaborResourcDetail_Click(object sender, EventArgs e)
        {
            frmKPIHRLaborResouces frm = new frmKPIHRLaborResouces(txtNoLaborLastday.Text, txtNoLaborAndOTLastDay.Text, txtTargetSOP.Text);
            frm.ShowDialog();
        }

        private void btnOTDetail_Click(object sender, EventArgs e)
        {
            frmKPIHRLaborOT frm = new frmKPIHRLaborOT(txtNoOTLastday.Text, txtCumulOT.Text, txtCulmulCreditOT.Text);
            frm.ShowDialog();
        }

        private void btnSaleDetail_Click(object sender, EventArgs e)
        {
            frmKPISupplySale frm = new frmKPISupplySale(txtCumulSale.Text, txtForecast.Text, txtSaleAchievement.Text, txtExceptionAmountLastDay.Text);
            frm.ShowDialog();
        }

        private void btnExceptional_Click(object sender, EventArgs e)
        {
            frmKPISupplyExceptional frm = new frmKPISupplyExceptional(List_Data);
            frm.ShowDialog();
        }

        private void btnDisplaySA_Click(object sender, EventArgs e)
        {
            frmKPIHRDisplaySafetyAlert frm = new frmKPIHRDisplaySafetyAlert(List_Data, txtNoDayWithoutAccident.Text);
            frm.ShowDialog();
        }

        private void btnDisplaySafetyTRIR_Click(object sender, EventArgs e)
        {
            frmKPIHRDisplaySafetyTRIR frm = new frmKPIHRDisplaySafetyTRIR(txtNoDayWithoutAccident.Text, txtNumberDSDone, txtNumberDSNotDone, txtNumberDS);
            frm.ShowDialog();
        }
        string year = General_Infor.KPI_year;
        string month = General_Infor.KPI_month;
        private void btnRefresh_HR_Click(object sender, EventArgs e)
        {
            string source_HR = @"\\172.16.180.20\21.Teamwork\05.E-KPI\"+year+@"\0.Current_Month\02.HR_Report.xlsx";
            adoClass = new ADO();
            DataTable dt = adoClass.ReadExcelFile("Labor resources", source_HR);
            string MonthAndYear = year + "-" + month + "-";
            string Month = month;
            int i = -1;
            string strQry = "delete from KPI_HR_LaborResources where Month(Date)=N'" + Month + "' and Year(Date)=N'" + year + "' \n";
            strQry += "insert into KPI_HR_LaborResources(Date,SOP,LaborAndOT,RealHeadCount,PLT_HC,Absenteeism,Cumul_labor,Absent_abnormal,HC_Indirect,HC_Direct,[MOD],[MOI],Absent_normal,Absent_target,Absent_abnormal_group,Absent_off,Absent_al_cl) \n";
            string qry1 = "";
            foreach (DataRow row in dt.Rows)
            {
                if (!string.IsNullOrEmpty(row["PLT HC"].ToString()))
                {
                    if (string.IsNullOrEmpty(qry1))
                    {
                        qry1 += "select N'" + MonthAndYear + row["Day"].ToString() + "',N'" + row["SOP"].ToString() + "',N'" + row["Labor + OT"].ToString() + "',N'" + row["Real HC Direct + Ind of Direct"].ToString() + "',N'" + row["PLT HC"].ToString() + "',N'" + row["Absenteeism"].ToString() + "',N'" + row["Cumul Labor"].ToString() + "',N'" + row["(%) Abnormal (Internal)"].ToString() + "',N'" + row["Real HC Ind"].ToString() + "',N'" + row["Real HC Direct"].ToString() + "',N'" + row["MOD"].ToString() + "',N'" + row["MOI"].ToString() + "',N'" + row["(%) Abs Normal"].ToString() + "',N'" + row["Abs target"].ToString() + "',N'" + row["(%) Abnormal (Group report)"].ToString() + "',N'" + row["OFF"].ToString() + "',N'" + row["AL+CL"].ToString() + "' \n";
                    }
                    else
                    {
                        qry1 += " union all select N'" + MonthAndYear + row["Day"].ToString() + "',N'" + row["SOP"].ToString() + "',N'" + row["Labor + OT"].ToString() + "',N'" + row["Real HC Direct + Ind of Direct"].ToString() + "',N'" + row["PLT HC"].ToString() + "',N'" + row["Absenteeism"].ToString() + "',N'" + row["Cumul Labor"].ToString() + "',N'" + row["(%) Abnormal (Internal)"].ToString() + "',N'" + row["Real HC Ind"].ToString() + "',N'" + row["Real HC Direct"].ToString() + "',N'" + row["MOD"].ToString() + "',N'" + row["MOI"].ToString() + "',N'" + row["(%) Abs Normal"].ToString() + "',N'" + row["Abs target"].ToString() + "',N'" + row["(%) Abnormal (Group report)"].ToString() + "',N'" + row["OFF"].ToString() + "',N'" + row["AL+CL"].ToString() + "' \n";
                    }
                    i++;
                }
            }
            strQry += qry1;
            strQry += " Update KPI_PlantDashBoard set value=N'" + dt.Rows[i]["Labor + OT"].ToString() + "' where item_name='txtNoLaborAndOTLastDay' \n";
            strQry += " Update KPI_PlantDashBoard set value=N'" + dt.Rows[i]["Absenteeism"].ToString() + "' where item_name='txtAbsenteeism' \n";
            strQry += " Update KPI_PlantDashBoard set value=N'" + dt.Rows[i]["Real HC Direct + Ind of Direct"].ToString() + "' where item_name='txtNoLaborLastday' \n";
            strQry += " Update KPI_PlantDashBoard set value=N'" + dt.Rows[i]["SOP"].ToString() + "' where item_name='txtTargetSOP' \n";
            strQry += " Update KPI_PlantDashBoard set color=N'" + dt.Rows[i]["Color"].ToString() + "' where item_name='pnLabor' \n";
            //----------------------------------------------------------------------------------------------------------------------------

            string strQry3 = " delete from KPI_HR_OT where Month(Date)=N'" + Month + "' and Year(Date)=N'" + year + "' \n";
            strQry3 += " insert into KPI_HR_OT(Date,TargetOT,CumulTargetOT,OT,CumulOT,OT_normal,CumulOT_normal,OT_abnormal,CumulOT_abnormal,OT_indirect,OT_direct,OT_amount_indirect,OT_amount_direct,OT_percent_indirect,OT_percent_direct) \n";

            DataTable dt2 = adoClass.ReadExcelFile("OT info", source_HR);
            int j = -1;
            string qry2 = "";
            foreach (DataRow row in dt2.Rows)
            {
                if (!string.IsNullOrEmpty(row["Total OT Day"].ToString()))
                {
                    j = int.Parse(row["Date"].ToString()) - 1;
                    if (string.IsNullOrEmpty(qry2))
                    {
                        qry2 += " select N'" + MonthAndYear + row["Date"].ToString() + "',N'" + row["Target OT"].ToString() + "',N'" + row["Cumul OT CREDIT"].ToString() + "',N'" + row["Total OT Day"].ToString() + "',N'" + row["Cumul OT"].ToString() + "',N'" + row["OT NOR"].ToString() + "',N'" + row["Cumul OT NOR"].ToString() + "',N'" + row["OT ABN"].ToString() + "',N'" + row["Cumul OT ABN"].ToString() + "',N'" + row["OT Indirect of Direct"].ToString() + "',N'" + row["OT Direct"].ToString() + "',N'" + row["OT Amount Indirect"].ToString() + "',N'" + row["OT Amount Direct"].ToString() + "',N'" + row["% OT Indirect of Direct"].ToString() + "',N'" + row["% OT Direct"].ToString() + "' \n";
                    }
                    else
                    {
                        qry2 += "union all select N'" + MonthAndYear + row["Date"].ToString() + "',N'" + row["Target OT"].ToString() + "',N'" + row["Cumul OT CREDIT"].ToString() + "',N'" + row["Total OT Day"].ToString() + "',N'" + row["Cumul OT"].ToString() + "',N'" + row["OT NOR"].ToString() + "',N'" + row["Cumul OT NOR"].ToString() + "',N'" + row["OT ABN"].ToString() + "',N'" + row["Cumul OT ABN"].ToString() + "',N'" + row["OT Indirect of Direct"].ToString() + "',N'" + row["OT Direct"].ToString() + "',N'" + row["OT Amount Indirect"].ToString() + "',N'" + row["OT Amount Direct"].ToString() + "',N'" + row["% OT Indirect of Direct"].ToString() + "',N'" + row["% OT Direct"].ToString() + "' \n";
                    }
                }
            }
            strQry3 += qry2;
            strQry3 += " Update KPI_PlantDashBoard set value=N'" + dt2.Rows[j]["Total OT Day"].ToString() + "' where item_name='txtNoOTLastday' \n";
            strQry3 += " Update KPI_PlantDashBoard set value=N'" + dt2.Rows[j]["Cumul OT"].ToString() + "' where item_name='txtCumulOT' \n";
            strQry3 += " Update KPI_PlantDashBoard set value=N'" + dt2.Rows[0]["CREDIT OT Month"].ToString() + "' where item_name='txtCulmulCreditOT' \n";
            strQry3 += " Update KPI_PlantDashBoard set color=N'" + dt2.Rows[j]["Status"].ToString() + "' where item_name='pnOT' \n";
            //-----------------------------------------------------------------------------------------------------------------------------
            DataTable dt3 = adoClass.ReadExcelFile("Notification", source_HR);
            strQry += " delete from KPI_PlantDashBoard where item_name=N'txtImportantFact' \n";
            foreach (DataRow row in dt3.Rows)
            {
                if (!string.IsNullOrEmpty(row["Notification"].ToString()))
                {
                    strQry += " Insert into KPI_PlantDashBoard ([item_name],[value_string]) Select N'txtImportantFact',N'" + row["Notification"] + "' \n";
                }
            }

            //-----------------------------------------------------------------------------------------------------------------------------
            DataTable dt5 = adoClass.ReadExcelFile("OT info", source_HR);
            strQry3 += " delete from KPI_HR_OTBySection where Month(Date)=N'" + Month + "' and Year(Date)=N'" + year + "' \n";
            strQry3 += "insert into KPI_HR_OTBySection([Date],[Section],[OT_hours]) \n";
            int k = -1;
            string qry5 = "";
            foreach (DataRow row in dt5.Rows)
            {
                if (!string.IsNullOrEmpty(row["Total OT Day"].ToString()))
                {
                    k = int.Parse(row["Date"].ToString()) - 1;
                    if (string.IsNullOrEmpty(qry5))
                    {
                        qry5 += " select N'" + MonthAndYear + row["Date"].ToString() + "',N'Braiding',N'" + row["Braiding"].ToString() + "' \n";
                        qry5 += " union all select N'" + MonthAndYear + row["Date"].ToString() + "',N'ADM',N'" + row["ADM"].ToString() + "' \n";
                        qry5 += " union all select N'" + MonthAndYear + row["Date"].ToString() + "',N'BBY',N'" + row["BBY"].ToString() + "' \n";
                        qry5 += " union all select N'" + MonthAndYear + row["Date"].ToString() + "',N'Extrusion',N'" + row["Extrusion"].ToString() + "' \n";
                        qry5 += " union all select N'" + MonthAndYear + row["Date"].ToString() + "',N'Finishing',N'" + row["Finishing"].ToString() + "' \n";
                        qry5 += " union all select N'" + MonthAndYear + row["Date"].ToString() + "',N'GP12',N'" + row["GP12"].ToString() + "' \n";
                        qry5 += " union all select N'" + MonthAndYear + row["Date"].ToString() + "',N'Lab room',N'" + row["Lab room"].ToString() + "' \n";
                        qry5 += " union all select N'" + MonthAndYear + row["Date"].ToString() + "',N'Maintenance',N'" + row["Maintenance"].ToString() + "' \n";
                        qry5 += " union all select N'" + MonthAndYear + row["Date"].ToString() + "',N'Mixing',N'" + row["Mixing"].ToString() + "' \n";
                        qry5 += " union all select N'" + MonthAndYear + row["Date"].ToString() + "',N'PA room',N'" + row["PA room"].ToString() + "' \n";
                        qry5 += " union all select N'" + MonthAndYear + row["Date"].ToString() + "',N'Patrol',N'" + row["Patrol"].ToString() + "' \n";
                        qry5 += " union all select N'" + MonthAndYear + row["Date"].ToString() + "',N'Rework and coating',N'" + row["Rework and coating"].ToString() + "' \n";
                        qry5 += " union all select N'" + MonthAndYear + row["Date"].ToString() + "',N'Tooling',N'" + row["Tooling"].ToString() + "' \n";
                        qry5 += " union all select N'" + MonthAndYear + row["Date"].ToString() + "',N'Vulcanization',N'" + row["Vulcanization"].ToString() + "' \n";
                        qry5 += " union all select N'" + MonthAndYear + row["Date"].ToString() + "',N'Warehouse',N'" + row["Warehouse"].ToString() + "' \n";
                    }
                    else
                    {
                        qry5 += " union all select N'" + MonthAndYear + row["Date"].ToString() + "',N'Braiding',N'" + row["Braiding"].ToString() + "' \n";
                        qry5 += " union all select N'" + MonthAndYear + row["Date"].ToString() + "',N'ADM',N'" + row["ADM"].ToString() + "' \n";
                        qry5 += " union all select N'" + MonthAndYear + row["Date"].ToString() + "',N'BBY',N'" + row["BBY"].ToString() + "' \n";
                        qry5 += " union all select N'" + MonthAndYear + row["Date"].ToString() + "',N'Extrusion',N'" + row["Extrusion"].ToString() + "' \n";
                        qry5 += " union all select N'" + MonthAndYear + row["Date"].ToString() + "',N'Finishing',N'" + row["Finishing"].ToString() + "' \n";
                        qry5 += " union all select N'" + MonthAndYear + row["Date"].ToString() + "',N'GP12',N'" + row["GP12"].ToString() + "' \n";
                        qry5 += " union all select N'" + MonthAndYear + row["Date"].ToString() + "',N'Lab room',N'" + row["Lab room"].ToString() + "' \n";
                        qry5 += " union all select N'" + MonthAndYear + row["Date"].ToString() + "',N'Maintenance',N'" + row["Maintenance"].ToString() + "' \n";
                        qry5 += " union all select N'" + MonthAndYear + row["Date"].ToString() + "',N'Mixing',N'" + row["Mixing"].ToString() + "' \n";
                        qry5 += " union all select N'" + MonthAndYear + row["Date"].ToString() + "',N'PA room',N'" + row["PA room"].ToString() + "' \n";
                        qry5 += " union all select N'" + MonthAndYear + row["Date"].ToString() + "',N'Patrol',N'" + row["Patrol"].ToString() + "' \n";
                        qry5 += " union all select N'" + MonthAndYear + row["Date"].ToString() + "',N'Rework and coating',N'" + row["Rework and coating"].ToString() + "' \n";
                        qry5 += " union all select N'" + MonthAndYear + row["Date"].ToString() + "',N'Tooling',N'" + row["Tooling"].ToString() + "' \n";
                        qry5 += " union all select N'" + MonthAndYear + row["Date"].ToString() + "',N'Vulcanization',N'" + row["Vulcanization"].ToString() + "' \n";
                        qry5 += " union all select N'" + MonthAndYear + row["Date"].ToString() + "',N'Warehouse',N'" + row["Warehouse"].ToString() + "' \n";

                    }
                }
            }
            strQry3 += qry5;
            //---------------------------------------------------
            DataTable dt6 = adoClass.ReadExcelFile("Turnover", source_HR);

            string strQry2 = "";
            string qry6 = "";
            foreach (DataRow row in dt6.Rows)
            {
                if (!string.IsNullOrEmpty(row["Total HC"].ToString()))
                {
                    if (string.IsNullOrEmpty(qry6))
                    {
                        qry6 += " select N'" + MonthAndYear + row["Day"].ToString() + "',N'" + row["Total Turnover rate (%)"].ToString() + "' \n";
                    }
                    else
                    {
                        qry6 += " union all select N'" + MonthAndYear + row["Day"].ToString() + "',N'" + row["Total Turnover rate (%)"].ToString() + "' \n";
                    }
                }
            }
            if (qry6 != "")
            {
                strQry2 = "delete from KPI_HR_TurnoverRate where Month(Date)=N'" + Month + "' and Year(Date)=N'" + year + "' \n";
                strQry2 += " insert into KPI_HR_TurnoverRate (Date,turnover_rate) \n ";
                strQry2 += qry6;
            }
            conn = new CmCn();
            string error = "";
            try
            {
                error = "Update Label resource error!";
                conn.ExcuteQry(strQry);
                error = "Update OT error!";
                conn.ExcuteQry(strQry3);
                error = "Update Turn over error!";
                conn.ExcuteQry(strQry2);
                MessageBox.Show("HR data has been uploaded");
                Load_Source_Data();
            }
            catch (Exception ex)
            {
                MessageBox.Show(error + "\n" + ex.Message);
            }
        }

        private void btnEditSafety_Click(object sender, EventArgs e)
        {
            frmKPIHREditSafetyData frm = new frmKPIHREditSafetyData(txtLastDateAccident, txtNumberDS, txtNumberDSDone, txtNumberDSNotDone);
            frm.ShowDialog();
            Load_Source_Data();
        }

        private void btnEnergyIntensity_Click(object sender, EventArgs e)
        {
            frmKPIMaintEnergyIntensity frm = new frmKPIMaintEnergyIntensity(txtEnergyIntensityM.Text,txtEnergyIntensityM1.Text,txtEnergyIntensityM2.Text);
            frm.ShowDialog();
            
        }

        private void btnQualityNQC_Click(object sender, EventArgs e)
        {
            frmKPIQualityNQC frm = new frmKPIQualityNQC(txtMTRLastday.Text, txtMTRThisMonth.Text, txtMTRLastMonth.Text, txtPPM.Text);
            frm.ShowDialog();
        }

    }
}
