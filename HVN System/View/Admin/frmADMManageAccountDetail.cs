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
    public partial class frmADMAddNewAccount : Form
    {
        public frmADMAddNewAccount()
        {
            InitializeComponent();
        }
        public frmADMAddNewAccount(ADM_Account _Account)
        {
            InitializeComponent();
            isEdit = true;
            Current_Account = new ADM_Account();
            Current_Account = _Account;
        }
        private ADO adoClass;
        private CmCn conn;
        private ADM_Account Current_Account;
        private bool isEdit = false;
        private void frmKPIAddNewAction_Load(object sender, EventArgs e)
        {
            Load_combobox();
            if (isEdit == true)
            {
                txtAccount.Text = Current_Account.Username;
                txtPassword.Text = Current_Account.Password;
                txtName.Text = Current_Account.Name;
                txtPosition.Text = Current_Account.Position;
                txtDepartment.Text = Current_Account.Department;
                txtDirectManager.Text = Current_Account.Direct_manager;
                txtDescription.Text = Current_Account.Description;
                txtEmailAddress.Text = Current_Account.Email_address;
                txtDirectChecker.Text= Current_Account.Direct_checker;
                cboPOapprover.Text= Current_Account.Po_approver;
                txtAccount.Enabled = false;
            }
            else
            {
                Current_Account = new ADM_Account();
            }
        }
        private void Load_combobox()
        {
            adoClass = new ADO();
            DataTable dt = adoClass.Load_data_Account("Username as [User Name]", "");
            txtDirectChecker.Properties.DataSource = dt;
            txtDirectChecker.Properties.DisplayMember = "User Name";
            txtDirectChecker.Properties.ValueMember = "User Name";
            txtDirectManager.Properties.DataSource = dt;
            txtDirectManager.Properties.DisplayMember = "User Name";
            txtDirectManager.Properties.ValueMember = "User Name";
            cboPOapprover.Properties.DataSource = dt;
            cboPOapprover.Properties.DisplayMember = "User Name";
            cboPOapprover.Properties.ValueMember = "User Name";
        }
        private void btnSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            adoClass = new ADO();
            Current_Account.Username = txtAccount.Text;
            Current_Account.Password = txtPassword.Text;
            Current_Account.Name = txtName.Text;
            Current_Account.Position = txtPosition.Text;
            Current_Account.Department = txtDepartment.Text;
            Current_Account.Direct_manager = txtDirectManager.Text;
            Current_Account.Direct_checker = txtDirectChecker.Text;
            Current_Account.Description = txtDescription.Text;
            Current_Account.Email_address = txtEmailAddress.Text;
            Current_Account.Po_approver = cboPOapprover.Text;
            if (Current_Account.Username==""|| Current_Account.Password == "")
            {
                MessageBox.Show("Missing information");
            }
            else
            {

                adoClass = new ADO();
                string pw = adoClass.Encrypt(Current_Account.Password);
                string strQry = "";
                if (isEdit)
                {
                    strQry += "update Account set \n";
                    strQry += " [Password]=N'"+ pw + "',Position=N'"+ Current_Account.Position + "',account_status=N'" + Current_Account.Account_status + "', \n ";
                    strQry += " Department=N'" + Current_Account.Department + "',[Name]=N'" + Current_Account.Name + "', \n ";
                    strQry += " Email_address=N'" + Current_Account.Email_address + "',Direct_manager=N'" + Current_Account.Direct_manager + "', \n ";
                    strQry += " direct_checker=N'" + Current_Account.Direct_checker + "',po_approver=N'" + Current_Account.Po_approver + "',[signature]=N'" + Current_Account.Signature + "' \n ";
                    strQry += " where Username=N'"+ txtAccount.Text + "' \n";
                }
                else
                {
                    strQry += " insert into Account (Username,Password,Position,Department,Description,Name,Email_address,Direct_manager,direct_checker,po_approver,signature,expired_date,account_status) \n";
                    strQry += " values (N'" + Current_Account.Username + "',N'" + pw + "',N'" + Current_Account.Position + "',N'" + Current_Account.Department
                        + "',N'" + Current_Account.Description + "',N'" + Current_Account.Name + "',N'" + Current_Account.Email_address + "',N'"
                        + Current_Account.Direct_manager + "',N'" + Current_Account.Direct_checker + "',N'" + Current_Account.Po_approver 
                        + "',N'" + Current_Account.Signature + "',N'" + Current_Account.Expired_date.ToString("yyyy-MM-dd") + "',N'Active')";
                }
                try
                {
                    conn = new CmCn();
                    conn.ExcuteQry(strQry);
                    MessageBox.Show("Save successfully");
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                
            }
            
        }
    }
}
