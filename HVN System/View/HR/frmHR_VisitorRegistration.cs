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
    public partial class frmHR_VisitorRegistration : Form
    {
        public frmHR_VisitorRegistration()
        {
            InitializeComponent();
        }
        public frmHR_VisitorRegistration(string _isViewAll)
        {
            InitializeComponent();
            isViewAll = true;
            btnApprove.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
            btnReject.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
        }
        private CmCn conn;
        private ADO adoClass;
        string Current_Id = "";
        private bool isViewAll = false;
        private void btnSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (isViewAll)
            {
                frmHR_Visitor_Info_Detail frm = new frmHR_Visitor_Info_Detail("", isViewAll);
                frm.ShowDialog();
                Load_Data();
            }
            else
            {
                if (DateTime.Now < DateTime.Today.AddHours(16))
                {
                    frmHR_Visitor_Info_Detail frm = new frmHR_Visitor_Info_Detail("",isViewAll);
                    frm.ShowDialog();
                    Load_Data();
                }
                else
                {
                    MessageBox.Show("Bạn không thể đăng ký sau 4h. Vui lòng liên hệ bộ phần Nhân sự \nYou cannot register after 4PM. Please contact HR for urgent case");
                }
            }
        }

        private void frmHRSafetyAlert_Load(object sender, EventArgs e)
        {
            Load_Data();
        }
        private void Load_Data()
        {
            if (isViewAll)
            {
                string strQry = "select * from HR_VR_VisitorInfor where is_active=N'1'";
                DataTable dt = new DataTable();
                conn = new CmCn();
                dt = conn.ExcuteDataTable(strQry);
                dgvResult.DataSource = dt;
            }
            else
            {
                string strQry = "select * from HR_VR_VisitorInfor where is_active=N'1' and user_commit=N'"+General_Infor.username+"'";
                DataTable dt = new DataTable();
                conn = new CmCn();
                dt = conn.ExcuteDataTable(strQry);
                dgvResult.DataSource = dt;
            }
            dtpPrintDate.EditValue = DateTime.Today;
        }
        
        private void btnDelete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (XtraMessageBox.Show("Do you want to delete registration ID "+ Current_Id +" ?", "Delete Item", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                string strQry = "update HR_VR_VisitorInfor set is_active = N'0', last_user_commit =N'"+General_Infor.username+ "',last_time_commit=getdate() where hr_reg_id =N'" + Current_Id+"'";
                conn = new CmCn();
                conn.ExcuteQry(strQry);
                Load_Data();
            }
        }

        private void gvResult_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            Current_Id = gvResult.GetRowCellValue(gvResult.FocusedRowHandle, "hr_reg_id").ToString();
        }

        private void btnRefresh_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Load_Data();
        }

        private void btnEdit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (Current_Id!="")
            {
                frmHR_Visitor_Info_Detail frm = new frmHR_Visitor_Info_Detail(Current_Id,isViewAll);
                frm.ShowDialog();
                Load_Data();
            }
        }

        private void gvResult_DoubleClick(object sender, EventArgs e)
        {
            Current_Id = gvResult.GetRowCellValue(gvResult.FocusedRowHandle, "hr_reg_id").ToString();
            btnEdit.PerformClick();
        }

        private void btnPrint_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            string strQry = "select *,cast(time_in as time) as [In_Time],cast(time_out as time) as [Out_Time] from HR_VR_VisitorInfor where is_active=N'1' and reg_date=N'" + dtpPrintDate.EditValue+"'";
            DataTable dt = new DataTable();
            conn = new CmCn();
            dt = conn.ExcuteDataTable(strQry);
            SplashScreenManager.ShowForm(this, typeof(frmWaitingForm), true, true, false);
            SplashScreenManager.Default.SetWaitFormCaption("Printing label...\nĐang in tem...");
            //---------
            adoClass = new ADO();
            adoClass.Print_HR_Visitor_Registration(dt);
            //---------
            SplashScreenManager.CloseForm();
            MessageBox.Show("In thành công");
        }

        private void btnApprovee_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            string strQry = "update HR_VR_VisitorInfor set [status]=N'Aprroved' where hr_reg_id=N'" + Current_Id + "'";
            conn = new CmCn();
            conn.ExcuteQry(strQry);
            Load_Data();
        }

        private void btnReject_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            string strQry = "update HR_VR_VisitorInfor set [status]=N'Rejected' where hr_reg_id=N'" + Current_Id + "'";
            conn = new CmCn();
            conn.ExcuteQry(strQry);
            Load_Data();
        }

        private void btnExport_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            adoClass = new ADO();
            adoClass.Export_Excel(dgvResult);
        }
    }
}
