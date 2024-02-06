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
    public partial class frmKPIMaintEnergyIntensity : Form
    {
        public frmKPIMaintEnergyIntensity()
        {
            InitializeComponent();
        }
        public frmKPIMaintEnergyIntensity(string m, string m1, string m2)
        {
            InitializeComponent();
            txtEIm.Text = m;
            txtEIm1.Text = m1;
            txtEIm2.Text = m2;
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
            ckSumary.Series.Clear();
            ckElectricity.Series.Clear();
            ckDiesel.Series.Clear();
            ckWater.Series.Clear();
            string month;
            if (cboMonth.SelectedValue == null)
            {
                month = General_Infor.KPI_month;
            }
            else
            {
                month = cboMonth.SelectedValue.ToString(); 
            }
            //----------------------
            string strQry1 = "select * from KPI_Maint_Diesel \n ";
            strQry1 += "where MONTH(Date)=N'" + month + "' and YEAR(Date)=N'" + cboYear.Text + "' order by [Date] ";
            conn = new CmCn();
            DataTable dt1 = conn.ExcuteDataTable(strQry1);
            Series series2 = new Series("Actual (liter)", ViewType.Bar);
            ckDiesel.Series.Add(series2);
            series2.DataSource = dt1;
            series2.ArgumentScaleType = ScaleType.DateTime;
            series2.ArgumentDataMember = "Date";
            series2.ValueScaleType = ScaleType.Numerical;
            series2.ValueDataMembers.AddRange(new string[] { "Actual" });
            series2.LabelsVisibility = default;
            series2.View.Color = Color.Orange;
            SideBySideBarSeriesLabel label2 = ckDiesel.Series[0].Label as SideBySideBarSeriesLabel;
            if (label2 != null)
            {
                //label2.Position = BarSeriesLabelPosition.Top;
                label2.TextPattern = "{V:N0}";
            }
            Series series1 = new Series("Cost (VND)", ViewType.Line);
            ckDiesel.Series.Add(series1);
            series1.DataSource = dt1;
            series1.ArgumentScaleType = ScaleType.DateTime;
            series1.ArgumentDataMember = "Date";
            series1.ValueScaleType = ScaleType.Numerical;
            series1.ValueDataMembers.AddRange(new string[] { "Cost" });
            series1.LabelsVisibility = default;
            series1.Label.TextPattern= "{V:N0}";
            SeriesViewBase viewBase1 = series1.View;
            ((LineSeriesView)series1.View).MarkerVisibility = DevExpress.Utils.DefaultBoolean.True;
            viewBase1.Color = Color.Blue;
            XYDiagram diagram2 = (XYDiagram)ckDiesel.Diagram;
            diagram2.AxisX.Label.TextPattern = "{A:dd-MMM}";
            diagram2.AxisY.Title.Visibility = DevExpress.Utils.DefaultBoolean.True;
            diagram2.AxisY.Title.Alignment = StringAlignment.Center;
            diagram2.AxisY.Title.Text = "Liter";
            diagram2.AxisY.Title.TextColor = Color.Orange;
            if (diagram2.SecondaryAxesY.Count > 0)
            {
                diagram2.SecondaryAxesY.Clear();
                SecondaryAxisY myAxisY2 = new SecondaryAxisY("my Y-Axis");
                ((XYDiagram)ckDiesel.Diagram).SecondaryAxesY.Add(myAxisY2);
                ((LineSeriesView)series1.View).AxisY = myAxisY2;
                myAxisY2.Title.Visibility = DevExpress.Utils.DefaultBoolean.True;
                myAxisY2.Title.TextColor = Color.Blue;
                myAxisY2.Title.Text = "VND";
            }
            else
            {
                SecondaryAxisY myAxisY2 = new SecondaryAxisY("my Y-Axis");
                ((XYDiagram)ckDiesel.Diagram).SecondaryAxesY.Add(myAxisY2);
                ((LineSeriesView)series1.View).AxisY = myAxisY2;
            }
            //-----------------------------------------------------
            string strQry3 = "select * from KPI_Maint_Electricity \n ";
            strQry3 += "where MONTH(Date)=N'" + month + "' and YEAR(Date)=N'" + cboYear.Text + "' order by [Date] ";
            conn = new CmCn();
            DataTable dt3 = conn.ExcuteDataTable(strQry3);
            Series series4 = new Series("Actual (KWh)", ViewType.Bar);
            ckElectricity.Series.Add(series4);
            series4.DataSource = dt3;
            series4.ArgumentScaleType = ScaleType.DateTime;
            series4.ArgumentDataMember = "Date";
            series4.ValueScaleType = ScaleType.Numerical;
            series4.ValueDataMembers.AddRange(new string[] { "Actual" });
            series4.LabelsVisibility = default;
            series4.View.Color = Color.Blue;
            SideBySideBarSeriesLabel label4 = ckElectricity.Series[0].Label as SideBySideBarSeriesLabel;
            if (label4 != null)
            {
                //label4.Position = BarSeriesLabelPosition.Top;
                label4.TextPattern = "{V:N0}";
            }
            Series series3 = new Series("Cost (VND)", ViewType.Line);
            ckElectricity.Series.Add(series3);
            series3.DataSource = dt3;
            series3.ArgumentScaleType = ScaleType.DateTime;
            series3.ArgumentDataMember = "Date";
            series3.ValueScaleType = ScaleType.Numerical;
            series3.ValueDataMembers.AddRange(new string[] { "Cost" });
            series3.LabelsVisibility = default;
            series3.Label.TextPattern = "{V:N0}";
            SeriesViewBase viewBase3 = series3.View;
            ((LineSeriesView)series3.View).MarkerVisibility = DevExpress.Utils.DefaultBoolean.True;
            viewBase3.Color = Color.Orange;
            XYDiagram diagram4 = (XYDiagram)ckElectricity.Diagram;
            diagram4.AxisX.Label.TextPattern = "{A:dd-MMM}";
            diagram4.AxisY.Title.Visibility = DevExpress.Utils.DefaultBoolean.True;
            diagram4.AxisY.Title.Alignment = StringAlignment.Center;
            diagram4.AxisY.Title.Text = "KWh";
            diagram4.AxisY.Title.TextColor = Color.Blue;
            if (diagram4.SecondaryAxesY.Count > 0)
            {
                diagram4.SecondaryAxesY.Clear();
                SecondaryAxisY myAxisY4 = new SecondaryAxisY("my Y-Axis");
                ((XYDiagram)ckElectricity.Diagram).SecondaryAxesY.Add(myAxisY4);
                ((LineSeriesView)series3.View).AxisY = myAxisY4;
                myAxisY4.Title.Visibility = DevExpress.Utils.DefaultBoolean.True;
                myAxisY4.Title.TextColor = Color.Orange;
                myAxisY4.Title.Text = "VND";
            }
            else
            {
                SecondaryAxisY myAxisY4 = new SecondaryAxisY("my Y-Axis");
                ((XYDiagram)ckElectricity.Diagram).SecondaryAxesY.Add(myAxisY4);
                ((LineSeriesView)series3.View).AxisY = myAxisY4;
            }
            //------------------------------------------------
            string strQry11 = "select * from KPI_Maint_Water \n ";
            strQry11 += "where MONTH(Date)=N'" + month + "' and YEAR(Date)=N'" + cboYear.Text + "' order by [Date] ";
            conn = new CmCn();
            DataTable dt11 = conn.ExcuteDataTable(strQry11);
            Series series12 = new Series("Actual (M3)", ViewType.Bar);
            ckWater.Series.Add(series12);
            series12.DataSource = dt11;
            series12.ArgumentScaleType = ScaleType.DateTime;
            series12.ArgumentDataMember = "Date";
            series12.ValueScaleType = ScaleType.Numerical;
            series12.ValueDataMembers.AddRange(new string[] { "Actual" });
            series12.LabelsVisibility = default;
            series12.View.Color = Color.Green;
            SideBySideBarSeriesLabel label12 = ckWater.Series[0].Label as SideBySideBarSeriesLabel;
            if (label12 != null)
            {
                //label12.Position = BarSeriesLabelPosition.Top;
                label12.TextPattern = "{V:N0}";
            }
            Series series11 = new Series("Cost (VND)", ViewType.Line);
            ckWater.Series.Add(series11);
            series11.DataSource = dt11;
            series11.ArgumentScaleType = ScaleType.DateTime;
            series11.ArgumentDataMember = "Date";
            series11.ValueScaleType = ScaleType.Numerical;
            series11.ValueDataMembers.AddRange(new string[] { "Cost" });
            series11.LabelsVisibility = default;
            series11.Label.TextPattern = "{V:N0}";
            SeriesViewBase viewBase11 = series11.View;
            ((LineSeriesView)series11.View).MarkerVisibility = DevExpress.Utils.DefaultBoolean.True;
            viewBase11.Color = Color.Orange;
            XYDiagram diagram12 = (XYDiagram)ckWater.Diagram;
            diagram12.AxisX.Label.TextPattern = "{A:dd-MMM}";
            diagram12.AxisY.Title.Visibility = DevExpress.Utils.DefaultBoolean.True;
            diagram12.AxisY.Title.Alignment = StringAlignment.Center;
            diagram12.AxisY.Title.Text = "KWh";
            diagram12.AxisY.Title.TextColor = Color.Green;
            if (diagram12.SecondaryAxesY.Count > 0)
            {
                diagram12.SecondaryAxesY.Clear();
                SecondaryAxisY myAxisY12 = new SecondaryAxisY("my Y-Axis");
                ((XYDiagram)ckWater.Diagram).SecondaryAxesY.Add(myAxisY12);
                ((LineSeriesView)series11.View).AxisY = myAxisY12;
                myAxisY12.Title.Visibility = DevExpress.Utils.DefaultBoolean.True;
                myAxisY12.Title.TextColor = Color.Orange;
                myAxisY12.Title.Text = "VND";
            }
            else
            {
                SecondaryAxisY myAxisY12 = new SecondaryAxisY("my Y-Axis");
                ((XYDiagram)ckWater.Diagram).SecondaryAxesY.Add(myAxisY12);
                ((LineSeriesView)series11.View).AxisY = myAxisY12;
            }
            //--------------------------------------------------
            string strQry5 = "select Energy.Month_,Energy.Cost,Sale.sale_target/1000000 as sale_target,round(Energy.Cost/Sale.sale_target,4) as [Energy_Intensity] from \n ";
            strQry5 += " (select Month_, sum(Cost) as Cost,Ref from  \n ";
            strQry5 += " (select FORMAT(Date,'MMM-yy') as Month_,FORMAT(Date,'yyMM') as Ref,sum(Cost)as Cost from KPI_Maint_Diesel where Date > N'" + DateTime.Today.AddYears(-1).ToString("yyyy-MM-dd") + "' group by FORMAT(Date,'MMM-yy'),FORMAT(Date,'yyMM') \n ";
            strQry5 += " union \n ";
            strQry5 += " select FORMAT(Date,'MMM-yy') as Month_,FORMAT(Date,'yyMM') as Ref,sum(Cost)as Cost from KPI_Maint_Electricity where Date > N'" + DateTime.Today.AddYears(-1).ToString("yyyy-MM-dd") + "' group by FORMAT(Date,'MMM-yy'),FORMAT(Date,'yyMM') \n ";
            strQry5 += " union \n ";
            strQry5 += " select FORMAT(Date,'MMM-yy') as Month_,FORMAT(Date,'yyMM') as Ref,sum(Cost)as Cost from KPI_Maint_Water where Date > N'" + DateTime.Today.AddYears(-1).ToString("yyyy-MM-dd") + "' group by FORMAT(Date,'MMM-yy'),FORMAT(Date,'yyMM')) as a \n ";
            strQry5 += " group by Month_,Ref) as Energy \n ";
            strQry5 += " left join  \n ";
            strQry5 += " (select Month_, sum(saletarget)*1000000 as sale_target from  \n ";
            strQry5 += " (select FORMAT(Date,'MMM-yy') as Month_,MAX(sale_target) as saletarget from KPI_LOG_SALE where Date > N'" + DateTime.Today.AddYears(-1).ToString("yyyy-MM-dd") + "' group by FORMAT(Date,'MMM-yy'),project) as b \n ";
            strQry5 += " group by Month_) as Sale \n ";
            strQry5 += " on Sale.Month_=Energy.Month_ \n ";
            strQry5 += " order by Energy.Ref \n ";

            DataTable dt5 = conn.ExcuteDataTable(strQry5);
            Series series6 = new Series("Sale Target (M VND)", ViewType.Bar);
            ckSumary.Series.Add(series6);
            series6.DataSource = dt5;
            series6.ArgumentScaleType = ScaleType.Auto;
            series6.ArgumentDataMember = "Month_";
            series6.ValueScaleType = ScaleType.Numerical;
            series6.ValueDataMembers.AddRange(new string[] { "sale_target" });
            series6.LabelsVisibility = default;
            series6.View.Color = Color.Blue;
            SideBySideBarSeriesLabel label6 = ckSumary.Series[0].Label as SideBySideBarSeriesLabel;
            if (label6 != null)
            {
                //label2.Position = BarSeriesLabelPosition.Top;
                label6.TextPattern = "{V:N0}";
            }
            Series series5 = new Series("Energy Intensity", ViewType.Line);
            ckSumary.Series.Add(series5);
            series5.DataSource = dt5;
            series5.ArgumentScaleType = ScaleType.Auto;
            series5.ArgumentDataMember = "Month_";
            series5.ValueScaleType = ScaleType.Numerical;
            series5.ValueDataMembers.AddRange(new string[] { "Energy_Intensity" });
            series5.LabelsVisibility = default;
            series5.Label.TextPattern = "{VP:p0}";
            SeriesViewBase viewBase5 = series5.View;
            ((LineSeriesView)series5.View).MarkerVisibility = DevExpress.Utils.DefaultBoolean.True;
            viewBase5.Color = Color.Orange;
            
            XYDiagram diagram6 = (XYDiagram)ckSumary.Diagram;
            diagram6.AxisY.Title.Visibility = DevExpress.Utils.DefaultBoolean.True;
            diagram6.AxisY.Title.Alignment = StringAlignment.Center;
            diagram6.AxisY.Title.Text = "Million VND";
            diagram6.AxisY.Title.TextColor = Color.Blue;
            //diagram6.AxisX.Label.TextPattern = "{A:dd-MMM}";
            if (diagram6.SecondaryAxesY.Count > 0)
            {
                diagram6.SecondaryAxesY.Clear();
                SecondaryAxisY myAxisY6 = new SecondaryAxisY("my Y-Axis");
                ((XYDiagram)ckSumary.Diagram).SecondaryAxesY.Add(myAxisY6);
                ((LineSeriesView)series5.View).AxisY = myAxisY6;
                myAxisY6.Label.TextPattern = "{VP:p0}";
                myAxisY6.Title.Visibility = DevExpress.Utils.DefaultBoolean.True;
                myAxisY6.Title.TextColor = Color.Orange;
                myAxisY6.Title.Text = "Percentage %";
            }
            else
            {
                SecondaryAxisY myAxisY6 = new SecondaryAxisY("my Y-Axis");
                ((XYDiagram)ckSumary.Diagram).SecondaryAxesY.Add(myAxisY6);
                ((LineSeriesView)series5.View).AxisY = myAxisY6;
            }
            //-------------------------------
            string strQry8 = "select round(sum(Actual),0), round(sum(Cost)/1000000,1) from KPI_Maint_Diesel \n ";
            strQry8 += "where MONTH(Date)=N'" + month + "' and YEAR(Date)=N'" + cboYear.Text + "' ";
            DataTable dt8 = conn.ExcuteDataTable(strQry8);
            txtActualD.Text = dt8.Rows[0][0].ToString()+ " (L)";
            txtCostD.Text = dt8.Rows[0][1].ToString() + " (m VND)";
            string strQry9 = "select round(sum(Actual),0), round(sum(Cost)/1000000,1) from KPI_Maint_Electricity \n ";
            strQry9 += "where MONTH(Date)=N'" + month + "' and YEAR(Date)=N'" + cboYear.Text + "' ";
            DataTable dt9 = conn.ExcuteDataTable(strQry9);
            txtActualE.Text = dt9.Rows[0][0].ToString() + " (KWh)";
            txtCostE.Text = dt9.Rows[0][1].ToString() + " (m VND)";
            string strQry10 = "select round(sum(Actual),0), round(sum(Cost)/1000000,1) from KPI_Maint_Water \n ";
            strQry10 += "where MONTH(Date)=N'" + month + "' and YEAR(Date)=N'" + cboYear.Text + "' ";
            DataTable dt10 = conn.ExcuteDataTable(strQry10);
            txtActualW.Text = dt10.Rows[0][0].ToString() + " (M3)";
            txtCostW.Text = dt10.Rows[0][1].ToString() + " (m VND)";
        }

        private void btnEnergyDetail_Click(object sender, EventArgs e)
        {
            frmKPIMaintEnergyIntensityDetail frm = new frmKPIMaintEnergyIntensityDetail(txtEIm.Text, txtEIm1.Text, txtEIm2.Text, txtActualD.Text, txtCostD.Text, txtActualE.Text, txtCostE.Text);
            frm.ShowDialog();
        }
    }
}
