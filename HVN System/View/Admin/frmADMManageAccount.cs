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
    public partial class frmADMManageAccount : Form
    {
        public frmADMManageAccount()
        {
            InitializeComponent();
        }
        private ADO adoClass;
        private CmCn conn;
        private List<ADM_Account> List_Account;
        private ADM_Account Current_Account;
        

        private void frmKPIMyAction_Load(object sender, EventArgs e)
        {
            List_Account = new List<ADM_Account>();
            Current_Account = new ADM_Account();
            Load_Account();
        }

        private void gvAction_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            Current_Account = gvResult.GetRow(gvResult.FocusedRowHandle) as ADM_Account;
        }
        private void Load_Account()
        {
            adoClass = new ADO();
            DataTable dt2 = adoClass.Load_data_Account("", "");
            List_Account = new List<ADM_Account>();
            foreach (DataRow row in dt2.Rows)
            {
                adoClass = new ADO();
                ADM_Account item = new ADM_Account();
                item.Username = row["Username"].ToString();
                item.Password = adoClass.Decrypt(row["Password"].ToString());
                item.Position = row["Position"].ToString();
                item.Department = row["Department"].ToString();
                item.Description = row["Description"].ToString();
                item.Name = row["Name"].ToString();
                item.Email_address = row["Email_address"].ToString();
                item.Direct_manager = row["Direct_manager"].ToString();
                item.Direct_checker = row["direct_checker"].ToString();
                item.Po_approver = row["Po_approver"].ToString();
                item.Signature = row["signature"].ToString();
                item.Account_status = row["account_status"].ToString();
                item.Expired_date = string.IsNullOrEmpty(row["Expired_date"].ToString())?DateTime.Today.AddDays(7):DateTime.Parse(row["Expired_date"].ToString());
                List_Account.Add(item);
            }
            dgvResult.DataSource = List_Account.ToList();
        }
        

       

        private void btnRefresh_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Load_Account();
        }

        private void btnDelete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (!string.IsNullOrEmpty(Current_Account.Username))
            {
                try
                {
                    if (MessageBox.Show("Do you want disable account "+ Current_Account.Username + "?","Disable Account", MessageBoxButtons.YesNo, MessageBoxIcon.Question)==DialogResult.Yes)
                    {
                        conn = new CmCn();
                        string strQry = "update Account set account_status=N'Disable' where Username='" + Current_Account.Username + "' " +
                            "\n Delete from ADM_ToolboxPermission where [username]=N'"+ Current_Account.Username + "'";
                        conn.ExcuteQry(strQry);
                        MessageBox.Show("Delete successfully");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
        }

        private void btnEdit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (Current_Account.Username != null)
            {
                frmADMAddNewAccount frm = new frmADMAddNewAccount(Current_Account);
                frm.ShowDialog();
            }
            else
            {
                MessageBox.Show("Please select the incident before edit");
            }
           
        }

        private void gvAction_DoubleClick(object sender, EventArgs e)
        {
            Current_Account = gvResult.GetRow(gvResult.FocusedRowHandle) as ADM_Account;
            frmADMAddNewAccount frm = new frmADMAddNewAccount(Current_Account);
            frm.ShowDialog();
        }

        private void btnAddNew_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmADMAddNewAccount frm = new frmADMAddNewAccount();
            frm.ShowDialog();
        }

        private void btnExport_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            adoClass = new ADO();
            adoClass.Export_Excel(dgvResult);
        }

        private void btnActive_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (!string.IsNullOrEmpty(Current_Account.Username))
            {
                try
                {
                    if (MessageBox.Show("Do you want activate account " + Current_Account.Username + "?", "Active Account", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        conn = new CmCn();
                        string strQry = "update Account set account_status=N'Active' where Username='" + Current_Account.Username + "' ";
                        conn.ExcuteQry(strQry);
                        MessageBox.Show("Delete successfully");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
        }
    }
}
