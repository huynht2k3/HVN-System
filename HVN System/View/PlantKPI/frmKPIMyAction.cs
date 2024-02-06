using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HVN_System.Entity;
using HVN_System.Util;
using HVN_System.View.PlantKPI;
using Outlook = Microsoft.Office.Interop.Outlook;

namespace HVN_System.View.Production
{
    public partial class frmKPIMyAction : Form
    {
        public frmKPIMyAction()
        {
            InitializeComponent();
        }
        private ADO adoClass;
        private CmCn conn;
        private List<KPI_ActionMonitoring_Entity> List_Action;
        private KPI_ActionMonitoring_Entity Current_Action;
        private void Load_My_Action()
        {
            adoClass = new ADO();
            DataTable dt2 = adoClass.KPI_Load_KPI_Action("", "last_user_commit=N'" + General_Infor.username+"'");
            List_Action = new List<KPI_ActionMonitoring_Entity>();
            foreach (DataRow row in dt2.Rows)
            {
                KPI_ActionMonitoring_Entity item = new KPI_ActionMonitoring_Entity();
                item.Act_name = row["act_name"].ToString();
                item.Act_des = row["act_des"].ToString();
                item.Inc_name = row["inc_name"].ToString();
                item.Priority = row["priority"].ToString();
                item.Assigned_user = row["assigned_user"].ToString();
                item.Location = row["location"].ToString();
                item.Planned_for = string.IsNullOrEmpty(row["planned_for"].ToString()) ? DateTime.Today : DateTime.Parse(row["planned_for"].ToString());
                item.Created_date = string.IsNullOrEmpty(row["created_date"].ToString()) ? DateTime.Today : DateTime.Parse(row["created_date"].ToString());
                item.Last_time_commit = string.IsNullOrEmpty(row["last_time_commit"].ToString()) ? DateTime.Today : DateTime.Parse(row["last_time_commit"].ToString());
                item.Status = row["status"].ToString();
                item.Check_id= row["check_id"].ToString();
                List_Action.Add(item);
            }
            dgvAction.DataSource = List_Action.ToList();
        }

        private void frmKPIMyAction_Load(object sender, EventArgs e)
        {
            Load_My_Action();
            Current_Action = new KPI_ActionMonitoring_Entity();
        }

        private void gvAction_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            Current_Action = gvAction.GetRow(gvAction.FocusedRowHandle) as KPI_ActionMonitoring_Entity;
        }

        private void btnDone_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (!string.IsNullOrEmpty(Current_Action.Act_name))
            {
                try
                {
                    string strQry = "update KPI_ActionMonitoring set status='Done',last_time_commit=GETDATE() where act_name='" + Current_Action.Act_name + "'";
                    conn = new CmCn();
                    conn.ExcuteQry(strQry);
                    Load_My_Action();
                    MessageBox.Show("Update successfully");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                
            }
        }

        private void btnCancel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (!string.IsNullOrEmpty(Current_Action.Act_name))
            {
                try
                {
                    string strQry = "update KPI_ActionMonitoring set status='Cancelled',last_time_commit=GETDATE() where act_name='" + Current_Action.Act_name + "'";
                    conn = new CmCn();
                    conn.ExcuteQry(strQry);
                    Load_My_Action();
                    MessageBox.Show("Update successfully");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
        }

        private void btnRefresh_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Load_My_Action();
        }

        private void btnDelete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (!string.IsNullOrEmpty(Current_Action.Act_name))
            {
                try
                {
                    conn = new CmCn();
                    string strQryCheck = "select act_name from KPI_ActionMonitoring where act_name='" + Current_Action.Act_name + "'and inc_name not in ('')";
                    string Check = conn.ExcuteString(strQryCheck);
                    if (string.IsNullOrEmpty(Check))
                    {
                        string strQry = "Delete from KPI_ActionMonitoring where act_name='" + Current_Action.Act_name + "'";
                        conn.ExcuteQry(strQry);
                        Load_My_Action();
                        MessageBox.Show("Delete successfully");
                    }
                    else
                    {
                        MessageBox.Show("You cannot delete the action for the incident");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
        }

        private void btnNewAction_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmKPIAddNewAction frm = new frmKPIAddNewAction();
            frm.ShowDialog();
            Load_My_Action();
        }

        private void btnEdit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (Current_Action.Act_name != null)
            {
                frmKPIAddNewAction frm = new frmKPIAddNewAction(Current_Action);
                frm.ShowDialog();
                Load_My_Action();
            }
            else
            {
                MessageBox.Show("Please select the incident before edit");
            }
           
        }

        private void gvAction_DoubleClick(object sender, EventArgs e)
        {
            Current_Action = gvAction.GetRow(gvAction.FocusedRowHandle) as KPI_ActionMonitoring_Entity;
            frmKPIAddNewAction frm = new frmKPIAddNewAction(Current_Action);
            frm.ShowDialog();
            Load_My_Action();
        }
    }
}
