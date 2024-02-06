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
using Outlook = Microsoft.Office.Interop.Outlook;

namespace HVN_System.View.Production
{
    public partial class frmApproval : Form
    {
        public frmApproval()
        {
            InitializeComponent();
        }
        List<P_ChangingFGData_Entity> List_Submit;
        private CmCn conn;
        private DataTable dt_pending;
        private void btnApprove_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                conn = new CmCn();
                string query = "",name="",email_address="", message="", current_user="";
                foreach (P_ChangingFGData_Entity item in List_Submit.ToList())
                {
                    if (item.Selected==true)
                    {
                        message +="\n"+ item.Product_customer_code + ":" + item.Modified_content;
                        if (item.Request_user!= current_user)
                        {
                            name += item.Requester_name + ",";
                            if (!string.IsNullOrEmpty(item.Email_address))
                            {
                                email_address += item.Email_address + ";";
                            }
                            current_user = item.Request_user;
                        }
                        query += item.Modified_sql_query.Replace("@","'") + "\n";
                        query += " update P_MasterListProductSubmit set is_approval='Yes', approval_time=getdate(), approval_user = '" + General_Infor.username + "' where row_id=N'"+item.Row_id+"' \n";
                        List_Submit.Remove(item);
                    }
                }
                conn.ExcuteQry(query);
                SendEmail(email_address, name, message, "APPROVE");
                dgvPending.DataSource = List_Submit.ToList();
                MessageBox.Show("Approve successfully");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }
        private void SendEmail(string Email_address, string Name, string content,string type)
        {
            //Outlook.MailItem mailItem = (Outlook.MailItem)
            // this.Application.CreateItem(Outlook.OlItemType.olMailItem);
            Outlook.Application app = new Outlook.Application();
            Outlook.MailItem mailItem = app.CreateItem(Outlook.OlItemType.olMailItem);
            mailItem.Subject = "[HVN System] Changing masterlist data of finished goods";
            mailItem.To = Email_address;
            mailItem.Body = "Dear " + Name + " \n\nI "+ type + " for changing below item " + content + "\n \n Best regards, \n";
            //mailItem.Attachments.Add(logPath);//logPath is a string holding path to the log.txt file
            //mailItem.Importance = Outlook.OlImportance.olImportanceHigh;
            try
            {
                if (!string.IsNullOrEmpty(Email_address))
                {
                    mailItem.Send();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
}
        private void btnRefresh_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Load_Masterlist_FG_Submit();
        }
        private void Load_Masterlist_FG_Submit()
        {
            conn = new CmCn();
            string StrQry = " select a.row_id,a.request_time,a.request_user,a.product_code,a.product_customer_code,a.modified_content,a.modified_sql_query,a.recover_sql_query,a.recover_sql_query,b.Name,b.Email_address \n";
            StrQry += " from P_MasterListProductSubmit a, Account b \n";
            StrQry += " where a.request_user = b.Username and a.is_approval is null \n";
            StrQry += " order by a.request_user \n";
            dt_pending = conn.ExcuteDataTable(StrQry);
            List_Submit = new List<P_ChangingFGData_Entity>();
            foreach (DataRow row in dt_pending.Rows)
            {
                P_ChangingFGData_Entity item = new P_ChangingFGData_Entity();
                item.Request_time =DateTime.Parse(row["request_time"].ToString());
                item.Request_user = row["request_user"].ToString();
                item.Product_code = row["product_code"].ToString();
                item.Product_customer_code = row["product_customer_code"].ToString();
                item.Modified_content = row["modified_content"].ToString();
                item.Modified_sql_query = row["modified_sql_query"].ToString();
                item.Recover_sql_query = row["recover_sql_query"].ToString();
                item.Requester_name = row["Name"].ToString();
                item.Email_address = row["Email_address"].ToString();
                item.Row_id = row["row_id"].ToString();
                item.Selected = false;
                List_Submit.Add(item);
            }
            dgvPending.DataSource = List_Submit.ToList();
        }

        private void frmApproval_Load(object sender, EventArgs e)
        {
            Load_Masterlist_FG_Submit();
        }

        private void btnSelectAll_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            foreach (P_ChangingFGData_Entity item in List_Submit)
            {
                item.Selected = true;
            }
            dgvPending.DataSource = List_Submit.ToList();
        }

        private void btnReject_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                conn = new CmCn();
                string query = "", name = "", email_address = "", message = "", current_user = "";
                foreach (P_ChangingFGData_Entity item in List_Submit.ToList())
                {
                    if (item.Selected == true)
                    {
                        message = "\n" + item.Product_customer_code + ":" + item.Modified_content;
                        if (item.Request_user != current_user)
                        {
                            name += item.Requester_name + ",";
                            if (!string.IsNullOrEmpty(item.Email_address))
                            {
                                email_address += item.Email_address + ";";
                            }
                            current_user = item.Request_user;
                        }
                        query += " update P_MasterListProductSubmit set is_approval='No', approval_time=getdate(), approval_user = '" + General_Infor.username + "' where row_id=N'" + item.Row_id + "' \n";
                        List_Submit.Remove(item);
                    }
                }
                conn.ExcuteQry(query);
                SendEmail(email_address, name, message,"REJECT");
                dgvPending.DataSource = List_Submit.ToList();
                MessageBox.Show("Reject successfully");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnUnselectAll_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            foreach (P_ChangingFGData_Entity item in List_Submit)
            {
                item.Selected = false;
            }
            dgvPending.DataSource = List_Submit.ToList();
        }

        private void repositoryItemCheckEdit1_EditValueChanged(object sender, EventArgs e)
        {
            if (gvPending.PostEditor())
            {
                gvPending.UpdateCurrentRow();
            }
        }
    }
}
