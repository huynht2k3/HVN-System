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
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraSplashScreen;
using HVN_System.View.Admin;
using HVN_System.View.PUR;

namespace HVN_System.View.Planning
{
    public partial class frmPURMasterListItemChangeManage : Form
    {
        public frmPURMasterListItemChangeManage()
        {
            InitializeComponent();
        }
        private ADO adoClass;
        private List<PUR_MasterListItem_Change_Entity> List_Data;
        private PUR_MasterListItem_Change_Entity Current_Request;
        private void btnEdit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (Current_Request.Request_id!="")
            {
                frmPURMasterListItemChangeDetail frm = new frmPURMasterListItemChangeDetail(Current_Request,"View");
                frm.ShowDialog();
                btnRefresh.PerformClick();
            }
        }
        private void btnRefresh_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Load_List_Request();
        }

        private void frmProductionPlanFG_Load(object sender, EventArgs e)
        {
            Load_List_Request();
            Current_Request = new PUR_MasterListItem_Change_Entity();
            adoClass = new ADO();
            //btnNew.Enabled = adoClass.Check_permission(this.Name, btnNew.Name, General_Infor.username);
        }
        private void Load_List_Request()
        {
            List_Data = new List<PUR_MasterListItem_Change_Entity>();
            adoClass = new ADO();
            DataTable dt = adoClass.Load_PUR_MasterListItem_Change("", "request_status not in ('Draft')");
            int i = 1;
            foreach (DataRow row in dt.Rows)
            {
                PUR_MasterListItem_Change_Entity item = new PUR_MasterListItem_Change_Entity();
                item.Stt = i;
                item.Request_id = row["request_id"].ToString();
                item.Item_name = row["item_name"].ToString();
                item.Requester = row["requester"].ToString();
                item.Requester_date = string.IsNullOrEmpty(row["requester_date"].ToString()) ? DateTime.Now : DateTime.Parse(row["requester_date"].ToString());
                item.Dept_mgr = row["dept_mgr"].ToString();
                item.Dept_mgr_date = string.IsNullOrEmpty(row["Dept_mgr_date"].ToString()) ? DateTime.Now : DateTime.Parse(row["Dept_mgr_date"].ToString());
                item.Pur = row["pur"].ToString();
                item.Pur_date = string.IsNullOrEmpty(row["Pur_date"].ToString()) ? DateTime.Now : DateTime.Parse(row["Pur_date"].ToString());
                item.Pur_mgr = row["pur_mgr"].ToString();
                item.Pur_mgr_date = string.IsNullOrEmpty(row["Pur_mgr_date"].ToString()) ? DateTime.Now : DateTime.Parse(row["Pur_mgr_date"].ToString());
                item.Fin_mgr = row["fin_mgr"].ToString();
                item.Fin_mgr_date = string.IsNullOrEmpty(row["Fin_mgr_date"].ToString()) ? DateTime.Now : DateTime.Parse(row["Fin_mgr_date"].ToString());
                item.Plant_mgr = row["plant_mgr"].ToString();
                item.Plant_mgr_date = string.IsNullOrEmpty(row["Plant_mgr_date"].ToString()) ? DateTime.Now : DateTime.Parse(row["Plant_mgr_date"].ToString());
                item.Current_pic = row["current_pic"].ToString();
                item.Request_status = row["request_status"].ToString();
                item.Note = row["note"].ToString();
                item.Dept = row["dept"].ToString();
                List_Data.Add(item);
                i++;
            }
            dgvResult.DataSource = List_Data.ToList();
        }
        private void btnNew_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmPURMasterListItemChangeDetail frm = new frmPURMasterListItemChangeDetail();
            frm.ShowDialog();
            btnRefresh.PerformClick();
        }

        private void gvResult_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            Current_Request = gvResult.GetRow(gvResult.FocusedRowHandle) as PUR_MasterListItem_Change_Entity;
        }


        private void gvResult_DoubleClick(object sender, EventArgs e)
        {
           
        }

        private void btnViewPR_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            Current_Request = gvResult.GetRow(gvResult.FocusedRowHandle) as PUR_MasterListItem_Change_Entity;
            frmPURMasterListItemChangeDetail frm = new frmPURMasterListItemChangeDetail(Current_Request, "View");
            frm.ShowDialog();
            btnRefresh.PerformClick();
        }

        private void btnEditPR_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            Current_Request = gvResult.GetRow(gvResult.FocusedRowHandle) as PUR_MasterListItem_Change_Entity;
            frmPURMasterListItemChangeDetail frm = new frmPURMasterListItemChangeDetail(Current_Request, "Edit");
            frm.ShowDialog();
            btnRefresh.PerformClick();
        }

        private void gvResult_CustomRowCellEdit(object sender, DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventArgs e)
        {
            if (e.Column.Caption == "Edit")
            {
                string val = gvResult.GetRowCellValue(e.RowHandle, "Request_status").ToString();
                if (val != "Pending requester"&& val !="Draft")
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
