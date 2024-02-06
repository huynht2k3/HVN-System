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
using Outlook = Microsoft.Office.Interop.Outlook;

namespace HVN_System.View.Production
{
    public partial class frmKPIManageIncident : Form
    {
        public frmKPIManageIncident()
        {
            InitializeComponent();
        }
        private ADO adoClass;
        private CmCn conn;
        private List<KPI_IncidentMonitoring> List_Incident;
        private List<KPI_ActionMonitoring_Entity> List_Action;
        private KPI_IncidentMonitoring Current_Incident;
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
            dgvIncident.DataSource = List_Incident.ToList();
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
                List_Action.Add(item);
            }
            dgvAction.DataSource = null;
        }
        private void frmKPIMyAction_Load(object sender, EventArgs e)
        {
            Load_My_Incident();
            Load_My_Action();
            Current_Incident = new KPI_IncidentMonitoring();
        }
        
        private void btnRefresh_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Load_My_Incident();
            Load_My_Action();
        }

        private void gvIncident_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            Current_Incident = gvIncident.GetRow(gvIncident.FocusedRowHandle) as KPI_IncidentMonitoring;
            SyncDataWithToolBox();
            var action_of_incident = List_Action.Where(x => x.Inc_name == Current_Incident.Inc_name);
            dgvAction.DataSource = action_of_incident.ToList();
        }
        private void SyncDataWithToolBox()
        {
            txtIncName.Text = Current_Incident.Inc_name;
            txtDescription.Text = Current_Incident.Inc_des;
            txtInc_location.Text = Current_Incident.Location;
            cboIncidentType.Text = Current_Incident.Inc_type;
            cboInc_level.Text = Current_Incident.Inc_level;
            cboInc_theme.Text = Current_Incident.Inc_theme;
            dtpInc_date.Value = Current_Incident.Created_for;
        }

        private void btnSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (MessageBox.Show("Do you want to change information?", "Save change", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                string strQry = "update KPI_IncidentMonitoring set inc_type='"+cboIncidentType.Text+ "',inc_theme='" + cboInc_theme.Text + "',inc_des='" + txtDescription.Text + "'";
                strQry += ",inc_level='" + cboInc_level.Text + "',location='" + txtInc_location.Text + "',update_time=getdate(),created_for=N'" + dtpInc_date.Value.ToString("yyyy-MM-dd hh:mm:ss") + "' \n";
                strQry += " where check_id=N'"+ Current_Incident.Check_id + "' \n";
                conn = new CmCn();
                try
                {
                    conn.ExcuteQry(strQry);
                    Load_My_Incident();
                    MessageBox.Show("Update sucessfully");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void btnDelete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (MessageBox.Show("Do you want to delete information?", "Delete item", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (Current_Incident.IsAction=="Yes")
                {
                    MessageBox.Show("You cannot delete incident that has been assigned action");
                }
                else
                {
                    string strQry = "delete from KPI_IncidentMonitoring \n";
                    strQry += " where check_id=N'" + Current_Incident.Check_id + "' \n";
                    conn = new CmCn();
                    try
                    {
                        conn.ExcuteQry(strQry);
                        Load_My_Incident();
                        MessageBox.Show("Delete sucessfully");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }

        private void btnExport_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Export_Excel(dgvIncident);
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

        private void btnAddNewAction_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            
        }
    }
}
