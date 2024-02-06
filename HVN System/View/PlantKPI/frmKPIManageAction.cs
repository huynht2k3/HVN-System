using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
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
    public partial class frmKPIManageAction : Form
    {
        public frmKPIManageAction()
        {
            InitializeComponent();
        }
        private ADO adoClass;
        private CmCn conn;
        private List<KPI_IncidentMonitoring> List_Incident;
        private List<KPI_ActionMonitoring_Entity> List_Action;
        private KPI_ActionMonitoring_Entity Current_Action;
        private bool isEdit = false;
        private void Load_My_Incident()
        {
            adoClass = new ADO();
            DataTable dt = adoClass.KPI_Load_KPI_Incident("", "");
            List_Incident = new List<KPI_IncidentMonitoring>();
            foreach (DataRow row in dt.Rows)
            {
                KPI_IncidentMonitoring item = new KPI_IncidentMonitoring();
                item.Inc_name = row["inc_name"].ToString();
                item.Inc_level = row["inc_level"].ToString();
                item.Inc_type = row["inc_type"].ToString();
                item.Inc_theme = row["inc_theme"].ToString();
                item.Inc_des = row["inc_des"].ToString();
                item.Author = row["author"].ToString();
                item.Location = row["location"].ToString();
                item.Created_time = string.IsNullOrEmpty(row["created_time"].ToString()) ? DateTime.Today : DateTime.Parse(row["created_time"].ToString());
                item.Created_for = string.IsNullOrEmpty(row["created_for"].ToString()) ? DateTime.Today : DateTime.Parse(row["created_for"].ToString());
                item.Update_time = string.IsNullOrEmpty(row["update_time"].ToString()) ? DateTime.Today : DateTime.Parse(row["update_time"].ToString());
                item.IsAction = row["isAction"].ToString();
                item.Check_id= row["check_id"].ToString();
                List_Incident.Add(item);
            }
            dgvIncident.DataSource = null;
        }
        private void Load_My_Action()
        {
            adoClass = new ADO();
            DataTable dt2 = adoClass.KPI_Load_KPI_Action("", "");
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
            dgvAction.DataSource = List_Action;
        }
        private void frmKPIMyAction_Load(object sender, EventArgs e)
        {
            Load_My_Incident();
            Load_My_Action();
            Load_Combobox();
            Current_Action = new KPI_ActionMonitoring_Entity();
        }
        
        private void btnRefresh_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Load_My_Incident();
            Load_My_Action();
        }
        private void SyncDataWithToolBox()
        {
            txtActionName.Text = Current_Action.Act_name;
            txtActionDes.Text = Current_Action.Act_des;
            txtLocation.Text = Current_Action.Location;
            cboAssignedUser.Text = Current_Action.Assigned_user;
            cboIncident.Text = Current_Action.Inc_name;
            cboPriority.Text = Current_Action.Priority;
            if (Current_Action.Planned_for<DateTime.Today.AddYears(-10))
            {
                dtpPlanedFor.Value = DateTime.Now;
            }
            else
            {
                dtpPlanedFor.Value = Current_Action.Planned_for;
            }
        }

        private void btnSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (MessageBox.Show("Do you want to change information?", "Save change", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (isEdit)
                {
                    string strQry = "Update KPI_ActionMonitoring set act_des=N'"+txtActionDes.Text+"',inc_name=N'"+cboIncident.Text+"'";
                    strQry += ",priority=N'"+cboPriority.Text+ "',planned_for=N'"+dtpPlanedFor.Value.ToString("yyyy-MM-dd")+ "',assigned_user=N'"+cboAssignedUser.Text+"'";
                    strQry += ",[location]=N'"+txtLocation.Text+ "',last_user_commit=N'"+General_Infor.username+"',last_time_commit=getdate() \n";
                    strQry += "where check_id=N'"+Current_Action.Check_id+"'"; 
                    conn = new CmCn();
                    try
                    {
                        conn.ExcuteQry(strQry);
                        Load_My_Incident();
                        Current_Action = new KPI_ActionMonitoring_Entity();
                        SyncDataWithToolBox();
                        MessageBox.Show("Action has been updated.");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                else
                {
                    string strQry = "insert into KPI_ActionMonitoring(act_name,act_des,inc_name,priority,planned_for,assigned_user,[location],status,created_date,last_user_commit,last_time_commit) \n";
                    strQry += "select N'" + txtActionName.Text + "',N'" + txtActionDes.Text + "',N'" + cboIncident.Text + "',N'" + cboPriority.Text + "',N'" + dtpPlanedFor.Value.ToString("yyyy-MM-dd") + "',N'" + cboAssignedUser.Text + "',N'" + txtLocation.Text + "',N'planned',getdate(),N'" + General_Infor.username + "',getdate()";
                    conn = new CmCn();
                    try
                    {
                        conn.ExcuteQry(strQry);
                        Load_My_Incident();
                        MessageBox.Show("New action has been created.");
                        Current_Action = new KPI_ActionMonitoring_Entity();
                        SyncDataWithToolBox();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }
        private void gvAction_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            isEdit = true;
            txtActionName.Enabled = false;
            Current_Action = gvAction.GetRow(gvAction.FocusedRowHandle) as KPI_ActionMonitoring_Entity;
            SyncDataWithToolBox();
            var action_of_incident = List_Incident.Where(x => x.Inc_name == Current_Action.Inc_name);
            dgvIncident.DataSource = action_of_incident.ToList();
        }

        private void btnExport_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Export_Excel(dgvAction);
        }
        private void Export_Excel(DevExpress.XtraGrid.GridControl Grid)
        {
            SaveFileDialog SaveDialog = new SaveFileDialog();
            SaveDialog.Filter = "Excel (.xlsx)|*.xlsx";
            if (SaveDialog.ShowDialog() != DialogResult.Cancel)
            {
                string ExportFilePath = SaveDialog.FileName;
                //Using System.IO;
                string FileExtenstion = Path.GetExtension(ExportFilePath);
                switch (FileExtenstion)
                {
                    case ".xlsx":
                        Grid.ExportToXlsx(ExportFilePath);
                        break;
                    default:
                        break;
                }
                if (File.Exists(ExportFilePath))
                {
                    try
                    {
                        //Try to open the file and let windows decide how to open it.
                        System.Diagnostics.Process.Start(ExportFilePath);
                    }
                    catch
                    {
                        String msg = "The file could not be opened." + Environment.NewLine + Environment.NewLine + "Path: " + ExportFilePath;
                        MessageBox.Show(msg, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    String msg = "The file could not be saved." + Environment.NewLine + Environment.NewLine + "Path: " + ExportFilePath;
                    MessageBox.Show(msg, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void Load_Combobox()
        {
            adoClass = new ADO();
            cboAssignedUser.Properties.DataSource = adoClass.Load_data_Account(" Username as [USER NAME],Name as [NAME],Department as [DEPARTMENT],Email_address as [EMAIL] ", "");
            cboAssignedUser.Properties.DisplayMember = "USER NAME";
            cboAssignedUser.Properties.ValueMember = "USER NAME";
            cboIncident.Properties.DataSource = adoClass.KPI_Load_KPI_Incident("inc_theme as [TYPE],inc_name as [INCIDENT],inc_des as [DESCRIPTION],LOCATION,created_for as [WHEN] ", "");
            cboIncident.Properties.DisplayMember = "INCIDENT";
            cboIncident.Properties.ValueMember = "INCIDENT";
        }
        private void btnAddNewAction_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //isEdit = false;
            //txtActionName.Enabled = true;
            //Current_Action = new KPI_ActionMonitoring_Entity();
            //SyncDataWithToolBox();
            frmKPIAddNewAction frm = new frmKPIAddNewAction();
            frm.ShowDialog();
            Load_My_Action();
        }
    }
}
