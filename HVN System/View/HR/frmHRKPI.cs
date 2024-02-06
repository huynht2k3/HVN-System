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
using DevExpress.XtraSplashScreen;
using HVN_System.View.Admin;

namespace HVN_System.View.HR
{
    public partial class frmHRKPI : Form
    {
        public frmHRKPI()
        {
            InitializeComponent();
        }
        private CmCn conn;
        private ADO adoClass;
        private void btnOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog OpenFile = new OpenFileDialog();
            OpenFile.Title = "Open File";
            OpenFile.Filter = "Excel (.xlsx)|*.xlsx";
            if (OpenFile.ShowDialog() != DialogResult.Cancel)
            {
                txtLink.Text = OpenFile.FileName;
            }
        }

        private void btnSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (cboMonth.Text!=""&&cboYear.Text!=""&&txtLink.Text!="")
            {
                switch (cboSheetName.Text)
                {
                    case "TRIR":
                        Update_TRIR();
                        break;
                    default:
                        MessageBox.Show("This item has not yet support");
                        break;
                }
            }
            else
            {
                MessageBox.Show("Lỗi thiếu thông tin");
            }
        }
        private void Update_TRIR()
        {
            string MonthAndYear = cboYear.Text + "-" + cboMonth.Text + "-";
            string Month = cboMonth.Text;
            adoClass = new ADO();
            conn = new CmCn();
            try
            {
                string strQry3 = " delete from KPI_HR_TRIR where Month(Date)=N'" + Month + "' and Year(Date)=N'" + cboYear.Text + "' \n";
                strQry3 += " insert into KPI_HR_TRIR([Date],[Death],[Lost_time],[No_lost_time],[First_aids],[Near_misses],[Risky]) \n";

                DataTable dt2 = adoClass.ReadExcelFile("TRIR", txtLink.Text);
                int j = -1;
                string qry5 = "";
                foreach (DataRow row in dt2.Rows)
                {
                    if (!string.IsNullOrEmpty(row["Ref"].ToString()))
                    {
                        j = int.Parse(row["Date"].ToString()) - 1;
                        if (string.IsNullOrEmpty(qry5))
                        {
                            qry5 += " select N'" + MonthAndYear + row["Date"].ToString() + "',N'" + row["Death"].ToString() + "',N'" + row["Lost time accidents"].ToString() + "',N'" + row["No lost time accident"].ToString() + "',N'" + row["First aids"].ToString() + "',N'" + row["Near misses"].ToString() + "',N'" + row["Risky Situation"].ToString() + "' \n";
                        }
                        else
                        {
                            qry5 += "union all select N'" + MonthAndYear + row["Date"].ToString() + "',N'" + row["Death"].ToString() + "',N'" + row["Lost time accidents"].ToString() + "',N'" + row["No lost time accident"].ToString() + "',N'" + row["First aids"].ToString() + "',N'" + row["Near misses"].ToString() + "',N'" + row["Risky Situation"].ToString() + "' \n";
                        }
                    }
                }
                strQry3 += qry5;
                conn.ExcuteQry(strQry3);
                MessageBox.Show("Update TRIR successfully");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi TRIR: " + ex.Message );
            }
        }
        private void Update_HR_File()
        {
            SplashScreenManager.ShowForm(this, typeof(frmWaitingForm), true, true, false);
            SplashScreenManager.Default.SetWaitFormCaption("Processing data...");
            int i = -1;
            string MonthAndYear = cboYear.Text + "-" + cboMonth.Text + "-";
            string Month = cboMonth.Text;
            adoClass = new ADO();
            conn = new CmCn();
            string error = "";
            try
            {
                DataTable dt = adoClass.ReadExcelFile("Labor resources", txtLink.Text);
                foreach (DataRow row in dt.Rows)
                {
                    if (!string.IsNullOrEmpty(row["PLT HC"].ToString()))
                    {
                        i = int.Parse(row["Day"].ToString()) - 1;
                    }
                }
                string strQry = "delete from KPI_HR_LaborResources where Month(Date)=N'" + Month + "' and Year(Date)=N'" + cboYear.Text + "' \n";
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
                    }
                }
                strQry += qry1;
                strQry += " Update KPI_PlantDashBoard set value=N'" + dt.Rows[i]["Labor + OT"].ToString() + "' where item_name='txtNoLaborAndOTLastDay' \n";
                strQry += " Update KPI_PlantDashBoard set value=N'" + dt.Rows[i]["Absenteeism"].ToString() + "' where item_name='txtAbsenteeism' \n";
                strQry += " Update KPI_PlantDashBoard set value=N'" + dt.Rows[i]["Real HC Direct + Ind of Direct"].ToString() + "' where item_name='txtNoLaborLastday' \n";
                strQry += " Update KPI_PlantDashBoard set value=N'" + dt.Rows[i]["SOP"].ToString() + "' where item_name='txtTargetSOP' \n";
                strQry += " Update KPI_PlantDashBoard set color=N'" + dt.Rows[i]["Color"].ToString() + "' where item_name='pnLabor' \n";
                conn.ExcuteQry(strQry);
            }
            catch (Exception ex)
            {
                error += "Lỗi Labor resources: " + ex.Message + "\n";
            }
            try
            {
                string strQry1 = " delete from KPI_HR_OT where Month(Date)=N'" + Month + "' and Year(Date)=N'" + cboYear.Text + "' \n";
                strQry1 += " insert into KPI_HR_OT(Date,TargetOT,CumulTargetOT,OT,CumulOT,OT_normal,CumulOT_normal,OT_abnormal,CumulOT_abnormal,OT_indirect,OT_direct) \n";

                DataTable dt2 = adoClass.ReadExcelFile("OT info", txtLink.Text);
                int j = -1;
                string qry2 = "";
                foreach (DataRow row in dt2.Rows)
                {
                    if (!string.IsNullOrEmpty(row["Total OT Day"].ToString()))
                    {
                        j = int.Parse(row["Date"].ToString()) - 1;
                        if (string.IsNullOrEmpty(qry2))
                        {
                            qry2 += " select N'" + MonthAndYear + row["Date"].ToString() + "',N'" + row["Target OT"].ToString() + "',N'" + row["Cumul OT CREDIT"].ToString() + "',N'" + row["Total OT Day"].ToString() + "',N'" + row["Cumul OT"].ToString() + "',N'" + row["OT NOR"].ToString() + "',N'" + row["Cumul OT NOR"].ToString() + "',N'" + row["OT ABN"].ToString() + "',N'" + row["Cumul OT ABN"].ToString() + "',N'" + row["OT Indirect of Direct"].ToString() + "',N'" + row["OT Direct"].ToString() + "' \n";
                        }
                        else
                        {
                            qry2 += "union all select N'" + MonthAndYear + row["Date"].ToString() + "',N'" + row["Target OT"].ToString() + "',N'" + row["Cumul OT CREDIT"].ToString() + "',N'" + row["Total OT Day"].ToString() + "',N'" + row["Cumul OT"].ToString() + "',N'" + row["OT NOR"].ToString() + "',N'" + row["Cumul OT NOR"].ToString() + "',N'" + row["OT ABN"].ToString() + "',N'" + row["Cumul OT ABN"].ToString() + "',N'" + row["OT Indirect of Direct"].ToString() + "',N'" + row["OT Direct"].ToString() + "' \n";
                        }
                    }
                }
                strQry1 += qry2;
                strQry1 += " Update KPI_PlantDashBoard set value=N'" + dt2.Rows[j]["Total OT Day"].ToString() + "' where item_name='txtNoOTLastday' \n";
                strQry1 += " Update KPI_PlantDashBoard set value=N'" + dt2.Rows[j]["Cumul OT"].ToString() + "' where item_name='txtCumulOT' \n";
                strQry1 += " Update KPI_PlantDashBoard set value=N'" + dt2.Rows[0]["CREDIT OT Month"].ToString() + "' where item_name='txtCulmulCreditOT' \n";
                strQry1 += " Update KPI_PlantDashBoard set color=N'" + dt2.Rows[j]["Status"].ToString() + "' where item_name='pnOT' \n";
                conn.ExcuteQry(strQry1);
            }
            catch (Exception ex)
            {
                error += "Lỗi OT info: " + ex.Message + "\n";
            }
            try
            {
                DataTable dt3 = adoClass.ReadExcelFile("Notification", txtLink.Text);
                string strQry2 = " delete from KPI_PlantDashBoard where item_name=N'txtImportantFact' \n";
                foreach (DataRow row in dt3.Rows)
                {
                    if (!string.IsNullOrEmpty(row["Notification"].ToString()))
                    {
                        strQry2 += " Insert into KPI_PlantDashBoard ([item_name],[value_string]) Select N'txtImportantFact',N'" + row["Notification"] + "' \n";
                    }
                }

                //-------------------------------------------------------------------------------------------------------------------------
                DataTable dt4 = adoClass.ReadExcelFile("Safety Alert", txtLink.Text);
                strQry2 += " Update KPI_PlantDashBoard set value=N'" + dt4.Rows[0]["number of safety alert of previous day"].ToString() + "' where item_name='txtNumberSALastday' \n";
                strQry2 += " Update KPI_PlantDashBoard set value=N'" + dt4.Rows[0]["total cumulated safety alert worldwide"].ToString() + "' where item_name='txtNumberSAGroup' \n";
                strQry2 += " Update KPI_PlantDashBoard set value=N'" + dt4.Rows[0]["total cumulated safety of Hung Yen"].ToString() + "' where item_name='txtNumberSAinHY' \n";
            }
            catch (Exception ex)
            {
                error += "Lỗi Notification + Safety: " + ex.Message + "\n";
            }
            try
            {
                //-----------------------------------------------------------------------------------------------------------------------------
                DataTable dt5 = adoClass.ReadExcelFile("OT info", txtLink.Text);
                string strQry3 = " delete from KPI_HR_OTBySection where Month(Date)=N'" + Month + "' and Year(Date)=N'" + cboYear.Text + "' \n";
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
                            qry5 += " union all select N'" + MonthAndYear + row["Date"].ToString() + "',N'Cleaning',N'" + row["Cleaning"].ToString() + "' \n";
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
                            qry5 += " union all select N'" + MonthAndYear + row["Date"].ToString() + "',N'Cleaning',N'" + row["Cleaning"].ToString() + "' \n";
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
                strQry3 += " Update KPI_PlantDashBoard set value=(SELECT DATEDIFF(day, max(created_for), GETDATE()) AS DateDiff from KPI_IncidentMonitoring where inc_type='Accident') where item_name='txtNoDayWithoutAccident' \n";
                conn = new CmCn();
                conn.ExcuteQry(strQry3);
            }
            catch (Exception ex)
            {
                error += "Lỗi OT by Location: " + ex.Message + "\n";
            }
            try
            {
                string strQry3 = " delete from KPI_HR_TRIR where Month(Date)=N'" + Month + "' and Year(Date)=N'" + cboYear.Text + "' \n";
                strQry3 += " insert into KPI_HR_TRIR([Date],[Death],[Lost_time],[No_lost_time],[First_aids],[Near_misses],[Risky]) \n";

                DataTable dt2 = adoClass.ReadExcelFile("TRIR", txtLink.Text);
                int j = -1;
                string qry5 = "";
                foreach (DataRow row in dt2.Rows)
                {
                    if (!string.IsNullOrEmpty(row["Ref"].ToString()))
                    {
                        j = int.Parse(row["Date"].ToString()) - 1;
                        if (string.IsNullOrEmpty(qry5))
                        {
                            qry5 += " select N'" + MonthAndYear + row["Date"].ToString() + "',N'" + row["Death"].ToString() + "',N'" + row["Lost time accidents"].ToString() + "',N'" + row["No lost time accident"].ToString() + "',N'" + row["First aids"].ToString() + "',N'" + row["Near misses"].ToString() + "',N'" + row["Risky Situation"].ToString() + "' \n";
                        }
                        else
                        {
                            qry5 += "union all select N'" + MonthAndYear + row["Date"].ToString() + "',N'" + row["Death"].ToString() + "',N'" + row["Lost time accidents"].ToString() + "',N'" + row["No lost time accident"].ToString() + "',N'" + row["First aids"].ToString() + "',N'" + row["Near misses"].ToString() + "',N'" + row["Risky Situation"].ToString() + "' \n";
                        }
                    }
                }
                strQry3 += qry5;
                conn.ExcuteQry(strQry3);
            }
            catch (Exception ex)
            {
                error += "Lỗi TRIR: " + ex.Message + "\n";
            }
            SplashScreenManager.CloseForm();
            if (error!="")
            {
                MessageBox.Show(error);
            }
            else
            {
                MessageBox.Show("Update successfully HR Report");
            }
        }

        private void frmHRSafetyAlert_Load(object sender, EventArgs e)
        {
            cboMonth.Text = General_Infor.KPI_month;
            cboYear.Text = General_Infor.KPI_year;
            cboSheetName.Text = "TRIR";
        }

        private void btnSaveAll_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (XtraMessageBox.Show("Do you want to update for all sheet?", "Delete Item", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (cboMonth.Text != "" && cboYear.Text != "" && txtLink.Text != "")
                {

                    Update_HR_File();

                }
                else
                {
                    MessageBox.Show("Lỗi thiếu thông tin");
                }
            }
        }
    }
}
