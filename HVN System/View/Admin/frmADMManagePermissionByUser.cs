using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using HVN_System.Entity;
using System.Collections.ObjectModel;
using HVN_System.Util;

namespace HVN_System.View.Admin
{
    public partial class frmADMManagePermissionByUser : Form
    {
        public frmADMManagePermissionByUser()
        {
            InitializeComponent();
        }
        private CmCn conn;
        private ADM_Account Current_account;
        private List<ADM_Account> List_Data;
        private List<ADM_Permission_Entity> List_User_Permission;
        private void btnLoad_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Load_Frm_name();
        }

        private void btnSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (MessageBox.Show("Do you want to save?", "Save", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                
            }
        }
        private void Load_Frm_name()
        {
            string strQry = "select * from Account where account_status=N'Active'";
            conn = new CmCn();
            DataTable dt = conn.ExcuteDataTable(strQry);
            List_Data = new List<ADM_Account>();
            foreach (DataRow row in dt.Rows)
            {
                ADM_Account item = new ADM_Account();
                item.Username = row["Username"].ToString();
                item.Department = row["Department"].ToString();
                List_Data.Add(item);
            }
            dgvUser.DataSource = List_Data.ToList();
        }
        private void Load_combobox()
        {
            string strQry = "select Username from Account where account_status=N'Active'";
            conn = new CmCn();
            DataTable dt = conn.ExcuteDataTable(strQry);
            cboFrom.Properties.DataSource = dt;
            cboFrom.Properties.ValueMember = "Username";
            cboFrom.Properties.DisplayMember = "Username";
            cboTo.Properties.DataSource = dt;
            cboTo.Properties.ValueMember = "Username";
            cboTo.Properties.DisplayMember = "Username";
        }
        private void btnSelectAllToolbox_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            foreach (ADM_Permission_Entity item in List_User_Permission)
            {
                item.Edit = true;
            }
        }

        private void frmManagePermission2_Load(object sender, EventArgs e)
        {
            Load_Frm_name();
            Load_combobox();
        }

        private void gvFrmName_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            //form_name = gvFrmName.GetRowCellValue(gvFrmName.FocusedRowHandle, "frm_name").ToString();
            Current_account = gvUser.GetRow(gvUser.FocusedRowHandle) as ADM_Account;
            string strQry = "select a.toolbox_group,a.frm_name,a.toolbox_name,a.toolbox_des, \n ";
            strQry += " case \n ";
            strQry += " when b.username = '"+ Current_account.Username+ "' then 'True' \n ";
            strQry += " else 'False' \n ";
            strQry += " end as Result \n ";
            strQry += "  from  \n ";
            strQry += " (select * from ADM_ToolboxOfForm) a \n ";
            strQry += " left join \n ";
            strQry += " (select * from ADM_ToolboxPermission \n ";
            strQry += " where username=N'" + Current_account.Username + "') b \n ";
            strQry += " on a.frm_name=b.frm_name  \n ";
            strQry += " and a.toolbox_name=b.toolbox_name \n ";

            conn = new CmCn();
            DataTable dt2 = conn.ExcuteDataTable(strQry);
            List_User_Permission = new List<ADM_Permission_Entity>();
            foreach (DataRow row in dt2.Rows)
            {
                ADM_Permission_Entity item = new ADM_Permission_Entity();
                item.Frm_name = row["frm_name"].ToString();
                item.Toolbox_name = row["toolbox_name"].ToString();
                item.Toolbox_des = row["toolbox_des"].ToString();
                item.Username = Current_account.Username;
                item.Department = row["toolbox_group"].ToString();
                item.Edit= row["Result"].ToString()=="True" ? true : false;
                item.Current_status = item.Edit;
                List_User_Permission.Add(item);
            }
            dgvFrmName.DataSource = List_User_Permission.ToList();
        }

        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            foreach (ADM_Permission_Entity item in List_User_Permission)
            {
                item.Edit = true;
            }
            dgvFrmName.DataSource = List_User_Permission.ToList();
        }

        private void btnUnselect_Click(object sender, EventArgs e)
        {
            foreach (ADM_Permission_Entity item in List_User_Permission)
            {
                item.Edit = false;
            }
            dgvFrmName.DataSource = List_User_Permission.ToList();
        }

        private void repositoryItemCheckEdit1_CheckedChanged(object sender, EventArgs e)
        {
            if (gvUser.PostEditor())
            {
                gvUser.UpdateCurrentRow();
            }
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to submit copy?", "submit copy", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (cboFrom.Text != "" && cboTo.Text != "")
                {
                    string strQry = "insert into ADM_ToolboxPermission(frm_name,toolbox_name,username,last_user_commit,last_time_commit) \n ";
                    strQry += " select frm_name,toolbox_name,N'" + cboTo.Text + "',N'" + General_Infor.username + "',getdate() \n ";
                    strQry += " from ADM_ToolboxPermission  \n ";
                    strQry += " where username=N'" + cboFrom.Text + "' \n ";
                    conn = new CmCn();
                    try
                    {
                        conn.ExcuteQry(strQry);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }

            }
            
        }

        private void dgvUsername_Click(object sender, EventArgs e)
        {

        }
    }
}
