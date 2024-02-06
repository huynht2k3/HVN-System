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
    public partial class frmADMManageFunction : Form
    {
        public frmADMManageFunction()
        {
            InitializeComponent();
        }
        private CmCn conn;
        private List<ADM_ToolboxOfForm_Entity> List_Data;
       
        private void btnLoad_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Load_combobox();
        }

        private void cboFormName_EditValueChanged(object sender, EventArgs e)
        {
            txtFormName.Text = cboFormName.Text;
            string strQry = "select * from ADM_ToolboxOfForm where frm_name=N'" + cboFormName.Text + "'";
            conn = new CmCn();
            DataTable dt = conn.ExcuteDataTable(strQry);
            List_Data = new List<ADM_ToolboxOfForm_Entity>();
            if (dt.Rows.Count>0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    ADM_ToolboxOfForm_Entity item = new ADM_ToolboxOfForm_Entity();
                    item.Toolbox_name = row["toolbox_name"].ToString();
                    item.Toolbox_des = row["toolbox_des"].ToString();
                    item.Toolbox_group = row["toolbox_group"].ToString();
                    item.Is_delegate = string.IsNullOrEmpty(row["Is_delegate"].ToString())?"False": row["Is_delegate"].ToString();
                    List_Data.Add(item);
                }
            }
            for (int i = 1; i <= 10; i++)
            {
                ADM_ToolboxOfForm_Entity item = new ADM_ToolboxOfForm_Entity();
                item.Toolbox_name = "";
                item.Toolbox_des = "";
                item.Toolbox_group = "";
                item.Is_delegate = "False";
                List_Data.Add(item);
            }
            dgvPermission.DataSource = List_Data.ToList();
        }
        private void Load_combobox()
        {
            string strQry = "select frm_name from ADM_ToolboxOfForm group by frm_name ";
            conn = new CmCn();
            DataTable dt = conn.ExcuteDataTable(strQry);
            cboFormName.Properties.DataSource = dt;
            cboFormName.Properties.DisplayMember = "frm_name";
            cboFormName.Properties.ValueMember = "frm_name";
        }
        private void cboFormType_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (cboFormType.Text=="New")
            {
                layoutControlItem2.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                layoutControlItem4.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                List_Data = new List<ADM_ToolboxOfForm_Entity>();
                for (int i = 1; i <= 10; i++)
                {
                    ADM_ToolboxOfForm_Entity item = new ADM_ToolboxOfForm_Entity();
                    item.Toolbox_name = "";
                    item.Toolbox_des = "";
                    List_Data.Add(item);
                }
                dgvPermission.DataSource = List_Data.ToList();
            }
            else
            {
                layoutControlItem2.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                layoutControlItem4.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
            }
        }

        private void frmManagePermission_Load(object sender, EventArgs e)
        {
            Load_combobox();
            layoutControlItem2.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            layoutControlItem4.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
            cboFormName.Text = "Exist";
        }

        private void btnSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (MessageBox.Show("Do you want to save?", "Save", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                string strQry = "";
                if (List_Data.Count > 0)
                {
                    string qry2 = "";
                    foreach (ADM_ToolboxOfForm_Entity item in List_Data)
                    {
                        if (item.Toolbox_name != "")
                        {
                            if (qry2 == "")
                            {
                                qry2 += "select N'" + txtFormName.Text + "',N'" + item.Toolbox_name + "',N'" + item.Toolbox_des + "',N'" + item.Toolbox_group + "',N'" + item.Is_delegate + "' \n";
                            }
                            else
                            {
                                qry2 += "union all select N'" + txtFormName.Text + "',N'" + item.Toolbox_name + "',N'" + item.Toolbox_des + "',N'" + item.Toolbox_group + "',N'" + item.Is_delegate + "' \n";
                            }
                        }
                    }
                    strQry += "delete from ADM_ToolboxOfForm where frm_name=N'" + txtFormName.Text + "'\n";
                    if (qry2!="")
                    {
                        strQry += "insert into ADM_ToolboxOfForm (frm_name,toolbox_name,toolbox_des,toolbox_group,is_delegate) \n";
                        strQry += qry2;
                    }
                    conn = new CmCn();
                    conn.ExcuteQry(strQry);
                    MessageBox.Show("Data has been saved");
                }
            }
        }

    }
}
