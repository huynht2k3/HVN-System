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
    public partial class frmADMManagePermissionByFunction : Form
    {
        public frmADMManagePermissionByFunction()
        {
            InitializeComponent();
        }
        private CmCn conn;
        private ADM_ToolboxOfForm_Entity Current_function;
        private List<ADM_ToolboxOfForm_Entity> List_Data;
        private List<ADM_Permission_Entity> List_User_Permission;
        private void btnLoad_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Load_Frm_name();
        }

        private void btnSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (MessageBox.Show("Do you want to save?", "Save", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                string qry2 = "";
                foreach (ADM_Permission_Entity item in List_User_Permission)
                {
                    if (item.Edit==true)
                    {
                        if (qry2 == "")
                        {
                            qry2 += "select N'" + Current_function.Frm_name + "',N'" + Current_function.Toolbox_name + "',N'" + item.Username + "',N'" + General_Infor.username + "',getdate()\n";
                        }
                        else
                        {
                            qry2 += "union all select N'" + Current_function.Frm_name + "',N'" + Current_function.Toolbox_name + "',N'" + item.Username + "',N'" + General_Infor.username + "',getdate()\n";
                        }
                    }
                }
                string strQry = "delete from ADM_ToolboxPermission where toolbox_name=N'" + Current_function.Toolbox_name + "' and frm_name=N'" + Current_function.Frm_name + "' \n";
                if (qry2!="")
                {
                    strQry += "insert into ADM_ToolboxPermission (frm_name,toolbox_name,username,last_user_commit,last_time_commit)\n";
                    strQry += qry2;
                   
                }
                conn = new CmCn();
                conn.ExcuteQry(strQry);
                MessageBox.Show("Data has been saved!");
            }
        }
        private void Load_Frm_name()
        {
            string strQry = "select * from ADM_ToolboxOfForm";
            conn = new CmCn();
            DataTable dt = conn.ExcuteDataTable(strQry);
            List_Data = new List<ADM_ToolboxOfForm_Entity>();
            foreach (DataRow row in dt.Rows)
            {
                ADM_ToolboxOfForm_Entity item = new ADM_ToolboxOfForm_Entity();
                item.Frm_name = row["frm_name"].ToString();
                item.Toolbox_name = row["toolbox_name"].ToString();
                item.Toolbox_des = row["toolbox_des"].ToString();
                item.Toolbox_group = row["toolbox_group"].ToString();
                List_Data.Add(item);
            }
            dgvFrmName.DataSource = List_Data.ToList();
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
            if (General_Infor.username!="admin")
            {
                gridColumn7.Visible = false;
            }
        }

        private void gvFrmName_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            //form_name = gvFrmName.GetRowCellValue(gvFrmName.FocusedRowHandle, "frm_name").ToString();
            Current_function = gvFrmName.GetRow(gvFrmName.FocusedRowHandle) as ADM_ToolboxOfForm_Entity;
            string strQry = " select a.Username,a.Department,a.Position,b.username as isEdit \n ";
            strQry += " ,b.frm_name,b.toolbox_name \n ";
            strQry += "   from  \n ";
            strQry += "   (select [Username],Department,Position from Account where account_status=N'Active') a \n ";
            strQry += "   left join \n ";
            strQry += "   (select * from ADM_ToolboxPermission  \n ";
            strQry += "   where frm_name=N'"+ Current_function.Frm_name+ "' \n ";
            strQry += "   and toolbox_name=N'" + Current_function.Toolbox_name + "')b \n ";
            strQry += "   on a.Username=b.username \n ";
            conn = new CmCn();
            DataTable dt2 = conn.ExcuteDataTable(strQry);
            List_User_Permission = new List<ADM_Permission_Entity>();
            foreach (DataRow row in dt2.Rows)
            {
                ADM_Permission_Entity item = new ADM_Permission_Entity();
                item.Frm_name = row["frm_name"].ToString();
                item.Toolbox_name = row["toolbox_name"].ToString();
                item.Username = row["Username"].ToString();
                item.Position = row["Position"].ToString();
                item.Department = row["Department"].ToString();
                item.Edit= string.IsNullOrEmpty(row["isEdit"].ToString()) ? false : true;
                List_User_Permission.Add(item);
            }
            dgvUsername.DataSource = List_User_Permission.ToList();
        }

        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            foreach (ADM_Permission_Entity item in List_User_Permission)
            {
                item.Edit = true;
            }
            dgvUsername.DataSource = List_User_Permission.ToList();
        }

        private void btnUnselect_Click(object sender, EventArgs e)
        {
            foreach (ADM_Permission_Entity item in List_User_Permission)
            {
                item.Edit = false;
            }
            dgvUsername.DataSource = List_User_Permission.ToList();
        }

        private void repositoryItemCheckEdit1_CheckedChanged(object sender, EventArgs e)
        {
            if (gvFrmName.PostEditor())
            {
                gvFrmName.UpdateCurrentRow();
            }
        }
    }
}
