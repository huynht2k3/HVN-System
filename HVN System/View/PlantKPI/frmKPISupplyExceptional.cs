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
    public partial class frmKPISupplyExceptional : Form
    {
        public frmKPISupplyExceptional()
        {
            InitializeComponent();
        }
        public frmKPISupplyExceptional(List<KPI_PlantDashBoard> list_data)
        {
            InitializeComponent();
            List_Data = list_data;
        }
        private ADO adoClass;
        private CmCn conn;
        private List<KPI_PlantDashBoard> List_Data;
        private void pnDS_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void tmRefreshData_Tick(object sender, EventArgs e)
        {
            //Load_Data_Abn();
            Load_Data_Pre();
            lbCurrentDateTime.Text = DateTime.Now.ToString("MM/dd/yyyy hh:mm");
        }
        private void frmDashboardPlantKPI_Load(object sender, EventArgs e)
        {
            Load_Combobox();
            cboMonth.Text = General_Infor.KPI_month_name;
            cboYear.Text = General_Infor.KPI_year;
            txtPreTransFee.Text = Load_Value_for_Item(txtPreTransFee, "value")+"m VND";
            //txtAbnTransFee.Text = Load_Value_for_Item(txtAbnTransFee, "value");
            txtTransFeeMN1.Text = Load_Value_for_Item(txtTransFeeMN1, "value") + "%";
            txtOTD.Text= Load_Value_for_Item(txtOTD, "value")+"%";
            //Load_Data_Abn();
            Load_Data_Pre();
            //Load_Data_Pre_Expect();
            //Load_Data_Abn_Expect();
            lbCurrentDateTime.Text = DateTime.Now.ToString("MM/dd/yyyy hh:mm");
            this.WindowState = FormWindowState.Maximized;
        }
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
        private void Load_Combobox()
        {
            adoClass = new ADO();
            cboMonth.DataSource = adoClass.Load_Parameter("month_in_year");
            cboMonth.DisplayMember = "child_name";
            cboMonth.ValueMember = "child_id";
        }

        private void cboMonth_SelectionChangeCommitted(object sender, EventArgs e)
        {
            //Load_Data_Abn();
            Load_Data_Pre();
        }

        private void cboYear_SelectedValueChanged(object sender, EventArgs e)
        {
            //Load_Data_Abn();
            Load_Data_Pre();
        }

        //private void Load_Data_Abn()
        //{
        //    ckAbnTrans.Series.Clear();
        //    string month;
        //    if (cboMonth.SelectedValue == null)
        //    {
        //        month = General_Infor.KPI_month;
        //    }
        //    else
        //    {
        //        month = cboMonth.SelectedValue.ToString();
        //    }
        //    string strQry = "select Date,ROUND(Abn_Mass_Prod/1000000,2) as Abn_Mass_Prod,ROUND([Abn_Claim]/1000000,2) as Abn_Claim,ROUND([Abn_ReseachDevelopment]/1000000,2)as Abn_ReseachDevelopment, \n";
        //    strQry += " round(sum(Abn_Claim+Abn_Mass_Prod+Abn_ReseachDevelopment) over(order by Date) / 1000000, 2) as Abn_trans_fee, round(TransFeeLastMonth/1000000,2) as TransFeeLastMonth \n";
        //    strQry += " from KPI_LOG_ExceptionFreight where MONTH(Date) = N'" + month + "' and YEAR(Date)=N'" + cboYear.Text + "' \n";
        //    strQry += " order by Date \n";
        //    conn = new CmCn();
        //    DataTable dt = conn.ExcuteDataTable(strQry);
        //    //----------------------
        //    Series series1 = new Series("Mass Production", ViewType.StackedBar);
        //    series1.DataSource = dt;
        //    series1.ArgumentScaleType = ScaleType.DateTime;
        //    series1.ArgumentDataMember = "Date";
        //    series1.ValueDataMembers.AddRange(new string[] { "Abn_Mass_Prod" });
        //    series1.LabelsVisibility = default;
        //    series1.View.Color = Color.Orange;
        //    //----------------------
        //    Series series2 = new Series("Claim", ViewType.StackedBar);
        //    series2.DataSource = dt;
        //    series2.ArgumentScaleType = ScaleType.DateTime;
        //    series2.ArgumentDataMember = "Date";
        //    series2.ValueDataMembers.AddRange(new string[] { "Abn_Claim" });
        //    series2.LabelsVisibility = default;
        //    series2.View.Color = Color.LightBlue;
        //    //------------------------------
        //    Series series3 = new Series("R&D", ViewType.StackedBar);
        //    series3.DataSource = dt;
        //    series3.ArgumentScaleType = ScaleType.DateTime;
        //    series3.ArgumentDataMember = "Date";
        //    series3.ValueDataMembers.AddRange(new string[] { "Abn_ReseachDevelopment" });
        //    series3.LabelsVisibility = default;
        //    series3.View.Color = Color.Blue;
        //    //----------------------
        //    Series series4 = new Series("Cumul Exceptional fees", ViewType.Line);
        //    series4.DataSource = dt;
        //    series4.ArgumentScaleType = ScaleType.DateTime;
        //    series4.ArgumentDataMember = "Date";
        //    series4.ValueDataMembers.AddRange(new string[] { "Abn_trans_fee" });
        //    series4.View.Color = Color.Red;
        //    //-------------------------------
        //    Series series5 = new Series("I/E fees m-1", ViewType.Line);
        //    series5.DataSource = dt;
        //    series5.ArgumentScaleType = ScaleType.DateTime;
        //    series5.ArgumentDataMember = "Date";
        //    series5.ValueDataMembers.AddRange(new string[] { "TransFeeLastMonth" });
        //    series5.View.Color = Color.Green;
        //    //-------------------------------
        //    ckAbnTrans.Series.AddRange(new Series[] { series1, series2, series3, series4, series5 });
        //    XYDiagram diagram = (XYDiagram)ckAbnTrans.Diagram;
        //    diagram.AxisY.Title.Visibility = DevExpress.Utils.DefaultBoolean.True;
        //    diagram.AxisY.Title.Text = "Million VND";
        //    diagram.AxisY.Title.TextColor = Color.Blue;
        //    diagram.AxisX.Label.TextPattern = "{A:dd-MMM}";
        //    if (diagram.SecondaryAxesY.Count > 0)
        //    {
        //        diagram.SecondaryAxesY.Clear();
        //        SecondaryAxisY myAxisY = new SecondaryAxisY("my Y-Axis");
        //        ((XYDiagram)ckAbnTrans.Diagram).SecondaryAxesY.Add(myAxisY);
        //        //((LineSeriesView)series4.View).AxisY = myAxisY;
        //        ((LineSeriesView)series5.View).AxisY = myAxisY;
        //        myAxisY.Title.Visibility = DevExpress.Utils.DefaultBoolean.True;
        //        myAxisY.Title.Text = "Million VND";
        //        myAxisY.Title.TextColor = Color.Red;
        //    }
        //    else
        //    {
        //        SecondaryAxisY myAxisY = new SecondaryAxisY("my Y-Axis");
        //        ((XYDiagram)ckAbnTrans.Diagram).SecondaryAxesY.Add(myAxisY);
        //        //((LineSeriesView)series4.View).AxisY = myAxisY;
        //        ((LineSeriesView)series5.View).AxisY = myAxisY;
        //        myAxisY.Title.Text = "Million VND";
        //        myAxisY.Title.TextColor = Color.Red;
        //    }
        //    //DateTime min = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1); // first of month
        //    //diagram.AxisX.WholeRange.MinValue = min;
        //}
        private void Load_Data_Pre()
        {
            ckPreTrans.Series.Clear();
            string month;
            if (cboMonth.SelectedValue == null)
            {
                month = General_Infor.KPI_month;
            }
            else
            {
                month = cboMonth.SelectedValue.ToString();
            }
            string strQry = "select Date,ROUND(Pre_Mass_Prod/1000000,2) as Pre_Mass_Prod,ROUND([Pre_Claim]/1000000,2) as Pre_Claim,ROUND([Pre_ReseachDevelopment]/1000000,2)as Pre_ReseachDevelopment, \n";
            strQry += " round(sum(Pre_Claim+Pre_Mass_Prod+Pre_ReseachDevelopment) over(order by Date) / 1000000, 2) as Pre_trans_fee, round(TransFeeLastMonth/1000000,2) as TransFeeLastMonth \n";
            strQry += " from KPI_LOG_ExceptionFreight where MONTH(Date) = N'" + month + "' and YEAR(Date)=N'" + cboYear.Text + "' \n";
            strQry += " order by Date \n";
            conn = new CmCn();
            DataTable dt = conn.ExcuteDataTable(strQry);
            string strQry2= "select round(avg(OTD),2) from KPI_LOG_OTD where MONTH(Date) = N'" + month + "' and YEAR(Date)=N'" + cboYear.Text + "' \n";
            txtOTDMonth.Text = conn.ExcuteString(strQry2)+"%";
            //----------------------
            Series series1 = new Series("Mass Production", ViewType.StackedBar);
            series1.DataSource = dt;
            series1.ArgumentScaleType = ScaleType.DateTime;
            series1.ArgumentDataMember = "Date";
            series1.ValueDataMembers.AddRange(new string[] { "Pre_Mass_Prod" });
            series1.LabelsVisibility = default;
            series1.View.Color = Color.Orange;
            //----------------------
            Series series2 = new Series("Claim", ViewType.StackedBar);
            series2.DataSource = dt;
            series2.ArgumentScaleType = ScaleType.DateTime;
            series2.ArgumentDataMember = "Date";
            series2.ValueDataMembers.AddRange(new string[] { "Pre_Claim" });
            series2.LabelsVisibility = default;
            series2.View.Color = Color.LightBlue;
            //------------------------------
            Series series3 = new Series("R&D", ViewType.StackedBar);
            series3.DataSource = dt;
            series3.ArgumentScaleType = ScaleType.DateTime;
            series3.ArgumentDataMember = "Date";
            series3.ValueDataMembers.AddRange(new string[] { "Pre_ReseachDevelopment" });
            series3.LabelsVisibility = default;
            series3.View.Color = Color.Blue;
            //----------------------
            //----------------------
            Series series4 = new Series("Cumul Exceptional fees", ViewType.Line);
            series4.DataSource = dt;
            series4.ArgumentScaleType = ScaleType.DateTime;
            series4.ArgumentDataMember = "Date";
            series4.ValueDataMembers.AddRange(new string[] { "Pre_trans_fee" });
            series4.View.Color = Color.Red;
            //-------------------------------
            Series series5 = new Series("I/E fees m-1", ViewType.Line);
            series5.DataSource = dt;
            series5.ArgumentScaleType = ScaleType.DateTime;
            series5.ArgumentDataMember = "Date";
            series5.ValueDataMembers.AddRange(new string[] { "TransFeeLastMonth" });
            series5.View.Color = Color.Green;
            //-----------------------------------
            ckPreTrans.Series.AddRange(new Series[] { series1, series2, series3, series4, series5 });
            XYDiagram diagram = (XYDiagram)ckPreTrans.Diagram;
            diagram.AxisY.Title.Visibility = DevExpress.Utils.DefaultBoolean.True;
            diagram.AxisY.Title.Text = "Million VND";
            diagram.AxisY.Title.TextColor = Color.Blue;
            diagram.AxisX.Label.TextPattern = "{A:dd-MMM}";
            if (diagram.SecondaryAxesY.Count > 0)
            {
                diagram.SecondaryAxesY.Clear();
                SecondaryAxisY myAxisY = new SecondaryAxisY("my Y-Axis");
                ((XYDiagram)ckPreTrans.Diagram).SecondaryAxesY.Add(myAxisY);
                //((LineSeriesView)series4.View).AxisY = myAxisY;
                ((LineSeriesView)series5.View).AxisY = myAxisY;
                myAxisY.Title.Visibility = DevExpress.Utils.DefaultBoolean.True;
                myAxisY.Title.Text = "Million VND";
                myAxisY.Title.TextColor = Color.Red;
            }
            else
            {
                SecondaryAxisY myAxisY = new SecondaryAxisY("my Y-Axis");
                ((XYDiagram)ckPreTrans.Diagram).SecondaryAxesY.Add(myAxisY);
                //((LineSeriesView)series4.View).AxisY = myAxisY;
                ((LineSeriesView)series5.View).AxisY = myAxisY;
                myAxisY.Title.TextColor = Color.Red;
                myAxisY.Title.Text = "Million VND";
                myAxisY.Title.TextColor = Color.Red;
            }
            //DateTime min = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1); // first of month
            //diagram.AxisX.WholeRange.MinValue = min;
        }
        //private void Load_Data_Pre_Expect()
        //{
        //    ckEstPre.Series.Clear();
        //    string strQry = "Select *,FORMAT(Date,'MMM-yy') AS Date_name,Pre_Claim+Pre_Mass_Prod+Pre_RND as Total From LOG_Estimate_Exceptional where month(Date)>=6 and month(Date)<=11";
        //    conn = new CmCn();
        //    DataTable dt = conn.ExcuteDataTable(strQry);
        //    //----------------------
        //    Series series1 = new Series("Pre Mass Prod", ViewType.StackedBar);
        //    series1.DataSource = dt;
        //    series1.ArgumentScaleType = ScaleType.Qualitative;
        //    series1.ArgumentDataMember = "Date_name";
        //    series1.ValueDataMembers.AddRange(new string[] { "Pre_Mass_Prod" });
        //    series1.View.Color = Color.Blue;
        //    //----------------------
        //    Series series2 = new Series("Pre Claim", ViewType.StackedBar);
        //    series2.DataSource = dt;
        //    series2.ArgumentScaleType = ScaleType.Qualitative;
        //    series2.ArgumentDataMember = "Date_name";
        //    series2.ValueDataMembers.AddRange(new string[] { "Pre_Claim" });
        //    series2.View.Color = Color.Red;
        //    //------------------------------
        //    Series series3 = new Series("Pre R&D", ViewType.StackedBar);
        //    series3.DataSource = dt;
        //    series3.ArgumentScaleType = ScaleType.Qualitative;
        //    series3.ArgumentDataMember = "Date_name";
        //    series3.ValueDataMembers.AddRange(new string[] { "Pre_RND" });
        //    series3.View.Color = Color.Green;
        //    //----------------------
        //    //----------------------
        //    Series series4 = new Series("Total Pre", ViewType.Line);
        //    series4.DataSource = dt;
        //    series4.ArgumentScaleType = ScaleType.Qualitative;
        //    series4.ArgumentDataMember = "Date_name";
        //    series4.ValueDataMembers.AddRange(new string[] { "Total" });
        //    series4.LabelsVisibility = default;
        //    series4.View.Color = Color.Purple;
        //    //-------------------------------
        //    ckEstPre.Series.AddRange(new Series[] { series1, series2, series3, series4 });
        //    XYDiagram diagram = (XYDiagram)ckPreTrans.Diagram;
        //    diagram.AxisY.Title.Visibility = DevExpress.Utils.DefaultBoolean.True;
        //    diagram.AxisY.Title.Text = "Million VND";
        //    diagram.AxisY.Title.TextColor = Color.Blue;
        //    //diagram.AxisX.Label.TextPattern = "{A:dd-MMM}";
        //    if (diagram.SecondaryAxesY.Count > 0)
        //    {
        //        diagram.SecondaryAxesY.Clear();
        //        SecondaryAxisY myAxisY = new SecondaryAxisY("my Y-Axis");
        //        ((XYDiagram)ckEstPre.Diagram).SecondaryAxesY.Add(myAxisY);
        //        ((LineSeriesView)series4.View).AxisY = myAxisY;
        //        myAxisY.Title.Visibility = DevExpress.Utils.DefaultBoolean.True;
        //        myAxisY.Title.Text = "Million VND";
        //        myAxisY.Title.TextColor = Color.Purple;
        //    }
        //    else
        //    {
        //        SecondaryAxisY myAxisY = new SecondaryAxisY("my Y-Axis");
        //        ((XYDiagram)ckEstPre.Diagram).SecondaryAxesY.Add(myAxisY);
        //        ((LineSeriesView)series4.View).AxisY = myAxisY;
        //        myAxisY.Title.TextColor = Color.Red;
        //        myAxisY.Title.Text = "Million VND";
        //        myAxisY.Title.TextColor = Color.Purple;
        //    }
        //    //DateTime min = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1); // first of month
        //    //diagram.AxisX.WholeRange.MinValue = min;
        //}
        //private void Load_Data_Abn_Expect()
        //{
        //    ckEstAbn.Series.Clear();
        //    string strQry = "Select *,FORMAT(Date,'MMM-yy') AS Date_name,Abn_Claim+Abn_Mass_Prod+Abn_RND as Total From LOG_Estimate_Exceptional where month(Date)>=6 and month(Date)<=11";
        //    conn = new CmCn();
        //    DataTable dt = conn.ExcuteDataTable(strQry);
        //    //----------------------
        //    Series series1 = new Series("Abn Mass Prod", ViewType.StackedBar);
        //    series1.DataSource = dt;
        //    series1.ArgumentScaleType = ScaleType.Qualitative;
        //    series1.ArgumentDataMember = "Date_name";
        //    series1.ValueDataMembers.AddRange(new string[] { "Abn_Mass_Prod" });
        //    series1.View.Color = Color.Blue;
        //    //----------------------
        //    Series series2 = new Series("Abn Claim", ViewType.StackedBar);
        //    series2.DataSource = dt;
        //    series2.ArgumentScaleType = ScaleType.Qualitative;
        //    series2.ArgumentDataMember = "Date_name";
        //    series2.ValueDataMembers.AddRange(new string[] { "Abn_Claim" });
        //    series2.View.Color = Color.Red;
        //    //------------------------------
        //    Series series3 = new Series("Abn R&D", ViewType.StackedBar);
        //    series3.DataSource = dt;
        //    series3.ArgumentScaleType = ScaleType.Qualitative;
        //    series3.ArgumentDataMember = "Date_name";
        //    series3.ValueDataMembers.AddRange(new string[] { "Abn_RND" });
        //    series3.View.Color = Color.Green;
        //    //----------------------
        //    //----------------------
        //    Series series4 = new Series("Total Abn", ViewType.Line);
        //    series4.DataSource = dt;
        //    series4.ArgumentScaleType = ScaleType.Qualitative;
        //    series4.ArgumentDataMember = "Date_name";
        //    series4.ValueDataMembers.AddRange(new string[] { "Total" });
        //    series4.LabelsVisibility = default;
        //    series4.View.Color = Color.Purple;
        //    //-------------------------------
        //    ckEstAbn.Series.AddRange(new Series[] { series1, series2, series3, series4 });
        //    XYDiagram diagram = (XYDiagram)ckAbnTrans.Diagram;
        //    diagram.AxisY.Title.Visibility = DevExpress.Utils.DefaultBoolean.True;
        //    diagram.AxisY.Title.Text = "Million VND";
        //    diagram.AxisY.Title.TextColor = Color.Blue;
        //    //diagram.AxisX.Label.TextPattern = "{A:dd-MMM}";
        //    if (diagram.SecondaryAxesY.Count > 0)
        //    {
        //        diagram.SecondaryAxesY.Clear();
        //        SecondaryAxisY myAxisY = new SecondaryAxisY("my Y-Axis");
        //        ((XYDiagram)ckEstAbn.Diagram).SecondaryAxesY.Add(myAxisY);
        //        ((LineSeriesView)series4.View).AxisY = myAxisY;
        //        myAxisY.Title.Visibility = DevExpress.Utils.DefaultBoolean.True;
        //        myAxisY.Title.Text = "Million VND";
        //        myAxisY.Title.TextColor = Color.Purple;
        //    }
        //    else
        //    {
        //        SecondaryAxisY myAxisY = new SecondaryAxisY("my Y-Axis");
        //        ((XYDiagram)ckEstAbn.Diagram).SecondaryAxesY.Add(myAxisY);
        //        ((LineSeriesView)series4.View).AxisY = myAxisY;
        //        myAxisY.Title.TextColor = Color.Red;
        //        myAxisY.Title.Text = "Million VND";
        //        myAxisY.Title.TextColor = Color.Purple;
        //    }
        //    //DateTime min = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1); // first of month
        //    //diagram.AxisX.WholeRange.MinValue = min;
        //}
    }
}
