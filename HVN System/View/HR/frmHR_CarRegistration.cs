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
using DevExpress.XtraEditors.Repository;

namespace HVN_System.View.HR
{
    public partial class frmHR_CarRegistration : Form
    {
        public frmHR_CarRegistration()
        {
            InitializeComponent();
        }
        private CmCn conn;
        private ADO adoClass;
        private HR_CarRegistration_Entity Current_request;
        private List<HR_CarRegistration_Entity> List_data;
        private void frmHRSafetyAlert_Load(object sender, EventArgs e)
        {
            Load_permission();
            Load_Data();
        }
        private void Load_permission()
        {
            adoClass = new ADO();
            btnNew.Enabled = adoClass.Check_permission(this.Name, btnNew.Name, General_Infor.username);
        }
        private void Load_Data()
        {
            List_data = new List<HR_CarRegistration_Entity>();
            string strQry = "select * from HR_CarRegistration where is_active=N'1' and dept=N'"+General_Infor.myaccount.Department+"'";
            conn = new CmCn();
            DataTable dt = conn.ExcuteDataTable(strQry);
            if (dt.Rows.Count>0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    HR_CarRegistration_Entity item = new HR_CarRegistration_Entity();
                    item.Request_id = row["request_id"].ToString();
                    item.Request_date = string.IsNullOrEmpty(row["request_date"].ToString()) ? DateTime.Today : DateTime.Parse(row["request_date"].ToString());
                    item.Requester = row["requester"].ToString();
                    item.Dept = row["dept"].ToString();
                    item.Car_type = row["car_type"].ToString();
                    item.From_date = string.IsNullOrEmpty(row["from_date"].ToString()) ? DateTime.Today : DateTime.Parse(row["from_date"].ToString());
                    item.To_date = string.IsNullOrEmpty(row["to_date"].ToString()) ? DateTime.Today : DateTime.Parse(row["to_date"].ToString());
                    item.Purpose = row["purpose"].ToString();
                    item.Estimated_cost = string.IsNullOrEmpty(row["estimated_cost"].ToString()) ? 0 : float.Parse(row["estimated_cost"].ToString());
                    item.Actual_cost = string.IsNullOrEmpty(row["actual_cost"].ToString()) ? 0 : float.Parse(row["actual_cost"].ToString());
                    item.Current_pic = row["current_pic"].ToString();
                    item.Request_status = row["request_status"].ToString();
                    item.Dept_mgr = row["dept_mgr"].ToString();
                    item.Dept_mgr_date = string.IsNullOrEmpty(row["dept_mgr_date"].ToString()) ? DateTime.Today : DateTime.Parse(row["dept_mgr_date"].ToString());
                    item.Hr_pic = row["hr_pic"].ToString();
                    item.Hr_pic_date = string.IsNullOrEmpty(row["hr_pic_date"].ToString()) ? DateTime.Today : DateTime.Parse(row["hr_pic_date"].ToString());
                    item.Plant_mgr = row["plant_mgr"].ToString();
                    item.Plant_mgr_date = string.IsNullOrEmpty(row["plant_mgr_date"].ToString()) ? DateTime.Today : DateTime.Parse(row["plant_mgr_date"].ToString());
                    item.Requester_sign = row["requester_sign"].ToString();
                    item.Dept_mgr_sign = row["dept_mgr_sign"].ToString();
                    item.Hr_pic_sign = row["hr_pic_sign"].ToString();
                    item.Plant_mgr_sign = row["plant_mgr_sign"].ToString();
                    item.From_loc = row["from_loc"].ToString();
                    item.To_loc = row["to_loc"].ToString();
                    List_data.Add(item);
                }
            }
            dgvResult.DataSource = List_data.ToList();
        }



        private void gvResult_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            //Current_Id = gvResult.GetRowCellValue(gvResult.FocusedRowHandle, "hr_reg_id").ToString();
            Current_request = gvResult.GetRow(gvResult.FocusedRowHandle) as HR_CarRegistration_Entity;
        }

        private void btnRefresh_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Load_Data();
        }

        private void gvResult_DoubleClick(object sender, EventArgs e)
        {
            Current_request = gvResult.GetRow(gvResult.FocusedRowHandle) as HR_CarRegistration_Entity;
        }

        private void btnExport_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            adoClass = new ADO();
            adoClass.Export_Excel(dgvResult);
        }

        private void btnAdd_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmHR_CarRegistrationDetail frm = new frmHR_CarRegistrationDetail();
            frm.ShowDialog();
            btnRefresh.PerformClick();
        }

        private void btnEdit_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            Current_request = gvResult.GetRow(gvResult.FocusedRowHandle) as HR_CarRegistration_Entity;
            frmHR_CarRegistrationDetail frm = new frmHR_CarRegistrationDetail(Current_request,"Edit");
            frm.ShowDialog();
            btnRefresh.PerformClick();
        }

        private void btnView_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            Current_request = gvResult.GetRow(gvResult.FocusedRowHandle) as HR_CarRegistration_Entity;
            frmHR_CarRegistrationDetail frm = new frmHR_CarRegistrationDetail(Current_request, "View");
            frm.ShowDialog();
            btnRefresh.PerformClick();
        }

        private void btnPrint_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            SplashScreenManager.ShowForm(this, typeof(frmWaitingForm), true, true, false);
            SplashScreenManager.Default.SetWaitFormCaption("Printing...");
            //---------
            Current_request = gvResult.GetRow(gvResult.FocusedRowHandle) as HR_CarRegistration_Entity;
            adoClass = new ADO();
            adoClass.Print_HR_CarRegistration(Current_request);
            //---------
            SplashScreenManager.CloseForm();
        }

        private void gvResult_CustomRowCellEdit(object sender, DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventArgs e)
        {
            if (e.Column.Name == "gcPrint")
            {
                string val = gvResult.GetRowCellValue(e.RowHandle, "Request_status").ToString();
                if (val != "Fully approve")
                {
                    RepositoryItemButtonEdit ritem = new RepositoryItemButtonEdit();
                    ritem.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
                    ritem.ReadOnly = true;
                    ritem.Buttons[0].Enabled = false;
                    e.RepositoryItem = ritem;
                }
            }
            if (e.Column.Name == "gcEdit")
            {
                string val = gvResult.GetRowCellValue(e.RowHandle, "Request_status").ToString();
                if (val != "Pending requester")
                {
                    RepositoryItemButtonEdit ritem = new RepositoryItemButtonEdit();
                    ritem.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
                    ritem.ReadOnly = true;
                    ritem.Buttons[0].Enabled = false;
                    e.RepositoryItem = ritem;
                }
            }    
        }
    }
}
