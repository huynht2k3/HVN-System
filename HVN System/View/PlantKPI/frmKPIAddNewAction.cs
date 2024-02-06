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
using DevExpress.XtraGrid.Views.Grid;

namespace HVN_System.View.PlantKPI
{
    public partial class frmKPIAddNewAction : Form
    {
        public frmKPIAddNewAction()
        {
            InitializeComponent();
        }
        public frmKPIAddNewAction(KPI_ActionMonitoring_Entity _kPI_Action)
        {
            InitializeComponent();
            isEdit = true;
            Load_Combobox();
            Load_Location();
            txtActionName.Text = _kPI_Action.Act_name;
            txtActionDes.Text = _kPI_Action.Act_des;
            cboIncident.Text = _kPI_Action.Inc_name;
            cboPriority.Text = _kPI_Action.Priority;
            dtpPlanedFor.Value = _kPI_Action.Planned_for;
            cboAssignedUser.Text = _kPI_Action.Assigned_user;
            cboLocation.Text = _kPI_Action.Location;
            check_id= _kPI_Action.Check_id;
        }
        private ADO adoClass;
        private KPI_ActionMonitoring_Entity kPI_Action;
        private string theme, assignee_email, assignee, check_id;
        private bool isEdit=false;
        private void frmKPIAddNewAction_Load(object sender, EventArgs e)
        {
            if (isEdit==false)
            {
                Load_Combobox();
                Load_Location();
            }
        }
        private void Load_Location()
        {
            adoClass = new ADO();
            cboLocation.DataSource = adoClass.Load_Parameter_Detail("", "parent_id='location'");
            cboLocation.DisplayMember = "child_name";
            cboLocation.ValueMember = "child_name";
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

        private void btnAdd_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (isEdit)
            {
                if (txtActionName.Text == "" || txtActionDes.Text == "")
                {
                    MessageBox.Show("Missing information.Please fill up all information before submit", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    kPI_Action = new KPI_ActionMonitoring_Entity();
                    kPI_Action.Act_name = txtActionName.Text;
                    kPI_Action.Act_des = txtActionDes.Text;
                    kPI_Action.Inc_name = cboIncident.Text;
                    kPI_Action.Priority = cboPriority.Text;
                    kPI_Action.Planned_for = dtpPlanedFor.Value;
                    kPI_Action.Assigned_user = cboAssignedUser.Text;
                    kPI_Action.Location = cboLocation.Text;
                    kPI_Action.Last_user_commit = General_Infor.username;
                    kPI_Action.Last_time_commit = DateTime.Now;
                    kPI_Action.Check_id = check_id;
                    adoClass = new ADO();
                    try
                    {
                        adoClass.KPI_Update_Action(kPI_Action);
                        //string To = assignee_email;
                        //string Subject = "[HVN System] KPI Action";
                        //string Body = "Dear " + assignee + ", \n\n I have updated the action that I granted to you on HVN System. Please check as below \n";
                        //Body += "Title: " + kPI_Action.Act_name;
                        //Body += "Description: " + kPI_Action.Act_des;
                        //Body += "Deadline: " + kPI_Action.Planned_for.ToString("dd/MMM/yyyy");
                        //Body += "\n\n Best regards.";
                        //adoClass.SendEmail(Subject, To, "", Body);
                        MessageBox.Show("Update successfully");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
            else
            {
                if (txtActionName.Text == "" || txtActionDes.Text == "")
                {
                    MessageBox.Show("Missing information.Please fill up all information before submit", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    kPI_Action = new KPI_ActionMonitoring_Entity();
                    kPI_Action.Act_name = txtActionName.Text;
                    kPI_Action.Act_des = txtActionDes.Text;
                    kPI_Action.Inc_name = cboIncident.Text;
                    kPI_Action.Priority = cboPriority.Text;
                    kPI_Action.Planned_for = dtpPlanedFor.Value;
                    kPI_Action.Assigned_user = cboAssignedUser.Text;
                    kPI_Action.Location = cboLocation.Text;
                    kPI_Action.Status = "planned";
                    kPI_Action.Theme = theme;
                    kPI_Action.Last_user_commit = General_Infor.username;
                    kPI_Action.Last_time_commit = DateTime.Now;
                    adoClass = new ADO();
                    try
                    {
                        adoClass.KPI_Insert_Action(kPI_Action);
                        string To = assignee_email;
                        string Subject = "[HVN System] KPI Action";
                        string Body = "Dear " + assignee + ", \n\n I have assigned new action to you on HVN System. Please check as below \n";
                        Body += "Title: " + kPI_Action.Act_name;
                        Body += "Description: " + kPI_Action.Act_des;
                        Body += "Deadline: " + kPI_Action.Planned_for.ToString("dd/MMM/yyyy");
                        Body += "\n\n Best regards.";
                        adoClass.SendEmail(Subject, To, "", Body);
                        MessageBox.Show("Create successfully");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
            this.Close();
        }

        private void txtLocation_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnAdd.PerformClick();
            }
        }

        private void txtActionName_Click(object sender, EventArgs e)
        {
            lbStatus.Text = "";
        }

        private void txtActionDes_Click(object sender, EventArgs e)
        {
            lbStatus.Text = "";
        }

        private void cboIncident_EditValueChanged(object sender, EventArgs e)
        {
            if (isEdit==false)
            {
                GridView view = cboIncident.Properties.View;
                int rowHandle = view.FocusedRowHandle;
                string fieldName = "TYPE";
                object value = view.GetRowCellValue(rowHandle, fieldName);
                theme = value.ToString();
            }
        }

        private void cboAssignedUser_EditValueChanged(object sender, EventArgs e)
        {
            if (isEdit == false)
            {
                GridView view = cboAssignedUser.Properties.View;
                int rowHandle = view.FocusedRowHandle;
                string fieldName = "EMAIL";
                object value = view.GetRowCellValue(rowHandle, fieldName);
                assignee_email = value.ToString();
                assignee = view.GetRowCellValue(rowHandle, "NAME").ToString();
            }
        }
    }
}
