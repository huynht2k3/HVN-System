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

namespace HVN_System.View.Admin
{
    public partial class frmADMDelegation : Form
    {
        public frmADMDelegation()
        {
            InitializeComponent();
        }
        private CmCn conn;
        private List<ADM_Delegation_Entity> List_Data;
        private ADM_Delegation_Entity Current_request;
        private void btnEdit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (Current_request.Dl_id!="")
            {
                frmADMDelegationDetail frm = new frmADMDelegationDetail(Current_request,"Edit");
                frm.ShowDialog();
                btnRefresh.PerformClick();
            }
        }
        private void btnRefresh_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Load_List_Delegate();
        }

        private void frmProductionPlanFG_Load(object sender, EventArgs e)
        {
            Load_List_Delegate();
        }
        private void Load_List_Delegate()
        {
            List_Data = new List<ADM_Delegation_Entity>();
            conn = new CmCn();
            string strQry = "select * from ADM_Delegation where dl_requester=N'"+General_Infor.username+"'  ";
            DataTable dt = conn.ExcuteDataTable(strQry);
            foreach (DataRow row in dt.Rows)
            {
                ADM_Delegation_Entity item = new ADM_Delegation_Entity();
                item.Dl_id = row["dl_id"].ToString();
                item.Dl_time = DateTime.Parse(row["dl_time"].ToString());
                item.Dl_requester = row["dl_requester"].ToString();
                item.Dl_fromdate = DateTime.Parse(row["dl_fromdate"].ToString());
                item.Dl_todate = DateTime.Parse(row["dl_todate"].ToString());
                item.Delegated_pic = row["delegated_pic"].ToString();
                item.Dl_note = row["dl_note"].ToString();
                if (row["is_active"].ToString()=="0")
                {
                    item.Is_active = "Cancel";
                }
                else
                {
                    item.Is_active = "Active";
                }
                List_Data.Add(item);
            }
            dgvResult.DataSource = List_Data.ToList();
        }
        private void btnNew_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmADMDelegationDetail frm = new frmADMDelegationDetail();
            frm.ShowDialog();
            btnRefresh.PerformClick();
        }
        
        private void gvResult_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            Current_request = gvResult.GetRow(gvResult.FocusedRowHandle) as ADM_Delegation_Entity;
        }

        private void btnDelete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (XtraMessageBox.Show("Do you want to cancel this delegation?", "Cancel delegation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                conn = new CmCn();
                string strQry= "";
                strQry += "Update ADM_Delegation set is_active=N'0' where dl_id=N'" + Current_request.Dl_id + "'\n";
                strQry += "insert into ADM_Log_2 (act_ad_user,act_time,act_computer,act_user,action) \n";
                strQry += "select N'"+ System.Environment.UserDomainName + "',getdate(),N'"+ System.Environment.MachineName + "',N'"+General_Infor.username+"',N'Cancel delegation ID:"+
                    Current_request.Dl_id+ ", to:"+ Current_request.Delegated_pic + " from "+ Current_request.Dl_fromdate.ToString("dd/MM/yyyy") + " to "+ Current_request.Dl_todate.ToString("dd/MM/yyyy")+"'";
                conn.ExcuteQry(strQry);
                MessageBox.Show("The delegation has been canceled");
                Load_List_Delegate();
            }
        }

        private void gvResult_DoubleClick(object sender, EventArgs e)
        {
            Current_request = gvResult.GetRow(gvResult.FocusedRowHandle) as ADM_Delegation_Entity;
            btnEdit.PerformClick();
        }
    }
}
