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

namespace HVN_System.View.PlantKPI
{
    public partial class frmKPIAddNewIncident : Form
    {
        public frmKPIAddNewIncident()
        {
            InitializeComponent();
        }
        public frmKPIAddNewIncident(string QC)
        {
            InitializeComponent();
            cboIncidentType.Text = "Incident";
            cboInc_theme.Text = "Quality";
            txtSortingTime.Text = "0";
        }
        public frmKPIAddNewIncident(KPI_IncidentMonitoring _kPI_Incident)
        {
            InitializeComponent();
            isEdit = true;
            txtInc_name.Text = _kPI_Incident.Inc_name;
            cboIncidentType.Text = _kPI_Incident.Inc_type;
            cboInc_theme.Text = _kPI_Incident.Inc_theme;
            Load_Inc_level();
            cboInc_level.Text= _kPI_Incident.Inc_level;
            txtInc_des.Text = _kPI_Incident.Inc_des;
            txtAuthor.Text = _kPI_Incident.Author;
            txtLocation.Text = _kPI_Incident.Location;
            dtpInc_date.Value = _kPI_Incident.Created_for;
            dtpInc_time.Value = _kPI_Incident.Created_for;
            check_id = _kPI_Incident.Check_id;
            cbo8D.Text= _kPI_Incident.Is8D;
            txtSortingTime.Text = _kPI_Incident.Sorting_time.ToString();
            cboCustomer.Text= _kPI_Incident.Inc_customer;
            cboIncStatus.Text= _kPI_Incident.Inc_status;
            nmsort_ext_cost.Value = _kPI_Incident.Sort_ext_cost;
            nmsort_int_cost.Value = _kPI_Incident.Sort_int_cost;
            nmCost.Value = _kPI_Incident.Cost;
            nmtrans_cost.Value = _kPI_Incident.Trans_cost;
            nmclaim_cost.Value = _kPI_Incident.Customer_claim_cost;
            nmother_cost.Value = _kPI_Incident.Other_cost;
            dtpext_lot.Value = _kPI_Incident.Extrus_lot;
            dtpfinishing_lot.Value = _kPI_Incident.Finishing_lot;
            txtLink_image.Text= _kPI_Incident.Image_link;
        }
        private ADO adoClass;
        private KPI_IncidentMonitoring kPI_Incident;
        bool isEdit = false;
        string check_id;
        private void btnExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void btnAdd_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (isEdit)
            {
                adoClass = new ADO();
                kPI_Incident = new KPI_IncidentMonitoring();
                kPI_Incident.Inc_name = txtInc_name.Text;
                kPI_Incident.Inc_type = cboIncidentType.Text;
                kPI_Incident.Inc_theme = cboInc_theme.Text;
                kPI_Incident.Inc_des = txtInc_des.Text;
                kPI_Incident.Inc_level = cboInc_level.Text;
                kPI_Incident.Author = txtAuthor.Text;
                kPI_Incident.Location = txtLocation.Text;
                kPI_Incident.Created_for = dtpInc_date.Value.Date + dtpInc_time.Value.TimeOfDay;
                kPI_Incident.Last_user_commit = General_Infor.username;
                kPI_Incident.Check_id = check_id;
                kPI_Incident.Is8D = cbo8D.Text;
                kPI_Incident.Sorting_time = float.Parse(txtSortingTime.Text);
                kPI_Incident.Inc_customer = cboCustomer.Text;
                kPI_Incident.Inc_status = cboIncStatus.Text;
                kPI_Incident.Cost = nmCost.Value;
                kPI_Incident.Sort_ext_cost = nmsort_ext_cost.Value;
                kPI_Incident.Sort_int_cost = nmsort_int_cost.Value;
                kPI_Incident.Trans_cost = nmtrans_cost.Value;
                kPI_Incident.Customer_claim_cost = nmclaim_cost.Value;
                kPI_Incident.Other_cost = nmother_cost.Value;
                kPI_Incident.Extrus_lot = dtpext_lot.Value;
                kPI_Incident.Finishing_lot = dtpfinishing_lot.Value;
                kPI_Incident.Image_link = txtLink_image.Text;
                adoClass = new ADO();
                adoClass.KPI_Update_Incident(kPI_Incident);
                MessageBox.Show("Update successfully");
            }
            else
            {
                if (txtInc_name.Text == "" || txtInc_des.Text == "" || cboInc_theme.Text == "" || txtLocation.Text == "")
                {
                    MessageBox.Show("Missing information.Please fill up all information before submit", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    adoClass = new ADO();
                    kPI_Incident = new KPI_IncidentMonitoring();
                    kPI_Incident.Inc_name = txtInc_name.Text;
                    kPI_Incident.Inc_type = cboIncidentType.Text;
                    kPI_Incident.Inc_theme = cboInc_theme.Text;
                    kPI_Incident.Inc_des = txtInc_des.Text;
                    kPI_Incident.Inc_level = cboInc_level.Text;
                    kPI_Incident.Author = txtAuthor.Text;
                    kPI_Incident.Location = txtLocation.Text;
                    kPI_Incident.Created_for = dtpInc_date.Value.Date + dtpInc_time.Value.TimeOfDay;
                    kPI_Incident.Last_user_commit = General_Infor.username;
                    kPI_Incident.Check_id = check_id;
                    kPI_Incident.Is8D = cbo8D.Text;
                    kPI_Incident.Sorting_time = float.Parse(txtSortingTime.Text);
                    kPI_Incident.Inc_customer = cboCustomer.Text;
                    kPI_Incident.Inc_status = cboIncStatus.Text;
                    kPI_Incident.Cost = nmCost.Value;
                    kPI_Incident.Sort_ext_cost = nmsort_ext_cost.Value;
                    kPI_Incident.Sort_int_cost = nmsort_int_cost.Value;
                    kPI_Incident.Trans_cost = nmtrans_cost.Value;
                    kPI_Incident.Customer_claim_cost = nmclaim_cost.Value;
                    kPI_Incident.Other_cost = nmother_cost.Value;
                    kPI_Incident.Extrus_lot = dtpext_lot.Value;
                    kPI_Incident.Finishing_lot = dtpfinishing_lot.Value;
                    kPI_Incident.Image_link = txtLink_image.Text;
                    adoClass = new ADO();
                    try
                    {
                        adoClass.KPI_Insert_Incident(kPI_Incident);
                        //MessageBox.Show("Create successfully");
                        //txtInc_name.Text = "";
                        //txtInc_des.Text = "";
                        //txtAuthor.Text = "";
                        //txtInc_location.Text = "";
                        //txtInc_name.Focus();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
            this.Close();
        }
        private void Load_Inc_level()
        {
            adoClass = new ADO();
            DataTable dt;
            if (cboIncidentType.Text == "Incident")
            {
                dt = adoClass.Load_Parameter_Detail("", "parent_id='incident_level' and child_des=N'" + cboInc_theme.Text + "'");
            }
            else
            {
                dt = adoClass.Load_Parameter_Detail("", "parent_id='accident_level' and child_des=N'" + cboInc_theme.Text + "'");
            }
            if(cboInc_theme.Text=="Quality")
            {
                lci8D.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                lciAuthor.Visibility= DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                lciTime.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                lciSortingTime.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
            }
            else
            {
                lci8D.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                lciAuthor.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                lciTime.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                lciSortingTime.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            }
            cboInc_level.DataSource = dt;
            cboInc_level.DisplayMember = "child_name";
            cboInc_level.ValueMember = "child_name";
        }
        private void frmKPIAddNewIncident_Load(object sender, EventArgs e)
        {
            if (isEdit == false)
            {
                Load_Inc_level();
            }
            //layoutControl1.Controls.Remove(cbo8D);
           
        }

        private void cboIncidentType_SelectionChangeCommitted(object sender, EventArgs e)
        {
            Load_Inc_level();
        }

        private void cboInc_theme_SelectionChangeCommitted(object sender, EventArgs e)
        {
            Load_Inc_level();
        }

        private void txtSortingTime_TextChanged(object sender, EventArgs e)
        {
            try
            {
                float test = float.Parse(txtSortingTime.Text);
            }
            catch (Exception)
            {
                txtSortingTime.Text = "";
                MessageBox.Show("Cannot convert sorting time to number");
            }
        }

        private void nmsort_ext_cost_ValueChanged(object sender, EventArgs e)
        {
            nmCost.Value = nmsort_ext_cost.Value + nmsort_int_cost.Value + nmtrans_cost.Value + nmclaim_cost.Value + nmother_cost.Value;
        }

        private void btnOpenLink_Click(object sender, EventArgs e)
        {
            OpenFileDialog OpenFile = new OpenFileDialog();
            OpenFile.Title = "Mở tệp tin";
            OpenFile.Filter = "JPG (.jpg)|*.jpg";
            if (OpenFile.ShowDialog() != DialogResult.Cancel)
            {
                txtLink_image.Text = OpenFile.FileName;
            }
        }
    }
}
