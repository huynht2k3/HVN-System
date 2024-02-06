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
using Outlook = Microsoft.Office.Interop.Outlook;
using System.Threading;
using DevExpress.XtraSplashScreen;
using HVN_System.View.Admin;
using DevExpress.XtraBars;

namespace HVN_System.View.Planning
{
    public partial class frmPURPRDetail : Form
    {
        public frmPURPRDetail(PUR_PR_Entity _PR, string _kind)
        {
            InitializeComponent();
            Current_PR = _PR;
            kind = _kind;
            if (kind== "Modify PR")
            {
                old_PrNo = Current_PR.Pr_no;
            }
            if (Current_PR.Old_pr_no!="")
            {
                old_PrNo = Current_PR.Old_pr_no;
            }
        }
        private CmCn conn;
        private ADO adoClass;
        private List<PUR_PRDetail_Entity> List_exist_item;
        private List<PUR_PR_CostSaving_Entity> List_saving_item;
        private List<PUR_PR_CompareCDE_Entity> List_CompareCDE;
        private string kind,old_PrNo="";
        private PUR_PR_Entity Current_PR;
        private decimal total_amount = 0, total_amount_without_vat = 0, total_vat = 0, total_qty=0;

        private void Load_Save_PR()
        {
            txtPRNo.Text = Current_PR.Pr_no;
            txtRequesterComment.Text = Current_PR.Pr_content;
            dtpEstimateDate.Value = Current_PR.Estimate_received_date;
            dtpExpectIssuePODate.Value= Current_PR.Expect_issue_po_date;
            dtpPRDate.Value = Current_PR.Pr_date;
            txtCurrency.Text = Current_PR.Pr_currency;
            txtRequester.Text = Current_PR.Requester;
            txtDept.Text = Current_PR.Dept;
            txtCheckerComment.Text = Current_PR.Checker_comment;
            txtApproveComment.Text = Current_PR.Approve_comment;
            cboSupplier.Text= Current_PR.Supplier_name;
            cboAdvancePayment.Text = Current_PR.Advance_payment;
            if (Current_PR.Pr_type == "CAPEX")
            {
                txtCapexNo.Text = Current_PR.Capex_no;
            }
            else
            {
                txtCapexNo.ReadOnly = true;
            }
            if (Current_PR.Pr_status == "Draft")
            {
                Current_PR.Pr_status = "Pending requester";
            }
            txtCust_name.Text = Current_PR.Customer_name;
            txtProject_name.Text = Current_PR.Project_name;
            if (Current_PR.Pr_status == "Draft")
            {
                Current_PR.Pr_status = "Pending requester";
            }
            switch (Current_PR.Pr_status)
            {
                case "Pending requester":
                    txtRequestDate.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
                    txtRequestStatus.Text = "Pending";
                    txtRequestStatus.ForeColor = Color.Red;
                    txtRequesterComment.ReadOnly = false;
                    break;
                case "Pending checker":
                    txtRequestDate.Text = Current_PR.Requester_date.ToString("dd/MM/yyyy HH:mm");
                    txtRequestStatus.Text = "Submitted";
                    txtRequestStatus.ForeColor = Color.Green;
                    txtChecker.Text = Current_PR.Current_pic;
                    txtCheckDate.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
                    txtCapexNo.ReadOnly = false;
                    txtCheckStatus.Text = "Pending";
                    txtCheckStatus.ForeColor = Color.Red;
                    txtCheckerComment.ReadOnly = false;
                    break;
                case "Pending approver":
                    txtRequestDate.Text = Current_PR.Requester_date.ToString("dd/MM/yyyy HH:mm");
                    txtRequestStatus.Text = "Submitted";
                    txtRequestStatus.ForeColor = Color.Green;
                    txtChecker.Text = Current_PR.Checker;
                    txtCheckDate.Text = Current_PR.Check_date.ToString("dd/MM/yyyy HH:mm");
                    txtCheckStatus.Text = "Checked";
                    txtCheckStatus.ForeColor = Color.Green;
                    txtApproval.Text = Current_PR.Approver;
                    txtApproveDate.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
                    txtApproveStatus.Text = "Pending";
                    txtApproveStatus.ForeColor = Color.Red;
                    txtCapexNo.ReadOnly = true;
                    txtApproveComment.ReadOnly = false;
                    break;
                default:
                    txtRequestDate.Text = Current_PR.Requester_date.ToString("dd/MM/yyyy HH:mm");
                    txtRequestStatus.Text = "Submitted";
                    txtRequestStatus.ForeColor = Color.Green;
                    txtChecker.Text = Current_PR.Checker;
                    txtCheckDate.Text = Current_PR.Check_date.ToString("dd/MM/yyyy HH:mm");
                    txtCheckStatus.Text = "Checked";
                    txtCheckStatus.ForeColor = Color.Green;
                    txtApproval.Text = Current_PR.Approver;
                    txtApproveDate.Text = Current_PR.Approve_date.ToString("dd/MM/yyyy HH:mm");
                    txtApproveStatus.Text = "Approved";
                    txtApproveStatus.ForeColor = Color.Green;
                    txtCapexNo.ReadOnly = true;
                    break;
            }
            cboRequestType.Text = Current_PR.Pr_type;
            string strQry = "Select * from PUR_PRDetail where pr_no=N'" + txtPRNo.Text + "' order by item_name";
            conn = new CmCn();
            DataTable dt = conn.ExcuteDataTable(strQry);
            List_exist_item = new List<PUR_PRDetail_Entity>();
            List_CompareCDE = new List<PUR_PR_CompareCDE_Entity>();
            if (dt.Rows.Count > 0)
            {
                int Stt = 1;
                foreach (DataRow row in dt.Rows)
                {
                    PUR_PRDetail_Entity item = new PUR_PRDetail_Entity();
                    item.Stt = Stt;
                    item.Pr_no = txtPRNo.Text;
                    item.Item_name = row["item_name"].ToString();
                    item.Hut_code = row["hut_code"].ToString();
                    item.Supplier_name = row["supplier_name"].ToString();
                    item.Quantity = decimal.Parse(row["quantity"].ToString());
                    item.Unit = row["unit"].ToString();
                    item.Unit_price = decimal.Parse(row["unit_price"].ToString());
                    item.Vat = decimal.Parse(row["vat"].ToString());
                    item.Vat_amount = item.Quantity * item.Vat * item.Unit_price;
                    item.Amount = decimal.Parse(row["amount"].ToString());
                    item.Moq = string.IsNullOrEmpty(row["moq"].ToString()) ? 0 : decimal.Parse(row["moq"].ToString());
                    item.Standard_packing = string.IsNullOrEmpty(row["standard_packing"].ToString()) ? 0 : decimal.Parse(row["standard_packing"].ToString());
                    List_exist_item.Add(item);
                    Stt++;
                }
            }
            for (int i = dt.Rows.Count + 1; i < 30 - dt.Rows.Count; i++)
            {
                PUR_PRDetail_Entity item = new PUR_PRDetail_Entity();
                item.Stt = i;
                item.Pr_no = txtPRNo.Text;
                List_exist_item.Add(item);
            }
            List_saving_item = new List<PUR_PR_CostSaving_Entity>();
            string strQry2 = "Select * from PUR_PR_CostSaving where pr_no=N'" + txtPRNo.Text + "' order by item_name";
            DataTable dt2 = conn.ExcuteDataTable(strQry2);
            if (dt2.Rows.Count > 0)
            {
                int Stt = 1;
                foreach (DataRow row in dt2.Rows)
                {
                    PUR_PR_CostSaving_Entity item = new PUR_PR_CostSaving_Entity();
                    item.Stt = Stt;
                    item.Pr_no = txtPRNo.Text;
                    item.Item_name = row["item_name"].ToString();
                    item.Before_price = decimal.Parse(row["before_price"].ToString());
                    item.After_price = decimal.Parse(row["after_price"].ToString());
                    item.Volume = decimal.Parse(row["volume"].ToString());
                    item.Is_purchased = row["is_purchased"].ToString();
                    List_saving_item.Add(item);
                    Stt++;
                }
            }
            for (int i = dt.Rows.Count + 1; i < 30 - dt.Rows.Count; i++)
            {
                PUR_PR_CostSaving_Entity item = new PUR_PR_CostSaving_Entity();
                item.Stt = i;
                item.Pr_no = txtPRNo.Text;
                List_saving_item.Add(item);
            }
            string strQry3 = "Select * from PUR_PR_CompareCDE where pr_no=N'" + txtPRNo.Text + "' order by item_name";
            DataTable dt3 = conn.ExcuteDataTable(strQry3);
            if (dt3.Rows.Count > 0)
            {
                int Stt = 1;
                foreach (DataRow row in dt3.Rows)
                {
                    PUR_PR_CompareCDE_Entity item = new PUR_PR_CompareCDE_Entity();
                    item.Stt = Stt;
                    item.Pr_no = txtPRNo.Text;
                    item.Item_name = row["item_name"].ToString();
                    item.Cde_budget = decimal.Parse(row["cde_budget"].ToString());
                    item.Actual_cost = decimal.Parse(row["actual_cost"].ToString());
                    item.Remain_budget = decimal.Parse(row["remain_budget"].ToString());
                    if (item.Cde_budget != 0)
                    {
                        item.Ultili_budget = item.Actual_cost / item.Cde_budget;
                    }
                    List_CompareCDE.Add(item);
                    Stt++;
                }
            }
            for (int i = dt3.Rows.Count + 1; i < 30 - dt3.Rows.Count; i++)
            {
                PUR_PR_CompareCDE_Entity item = new PUR_PR_CompareCDE_Entity();
                item.Stt = i;
                item.Pr_no = txtPRNo.Text;
                List_CompareCDE.Add(item);
            }
            dgvExistItem.DataSource = List_exist_item.ToList();
            dgvSaving.DataSource = List_saving_item.ToList();
            dgvCDE.DataSource = List_CompareCDE.ToList();
            Update_amount();
        }
        private void btnSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (XtraMessageBox.Show("Do you want to submit the PR?", "Submit PR", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (kind=="New")
                {
                    Load_PRNo();
                }
                Update_amount();
                string qry2 = "";
                foreach (PUR_PRDetail_Entity item in List_exist_item)
                {
                    if (item.Quantity > 0 && item.Item_name != "")
                    {
                        if (qry2 == "")
                        {
                            qry2 += " select N'" + txtPRNo.Text + "',N'" + item.Item_name + "',N'" + item.Erp_code + "',N'" + item.Hut_code + "', \n ";
                            qry2 += " N'" + item.Supplier_name + "',N'" + item.Quantity + "',N'" + item.Unit + "',N'" + item.Unit_price + "', \n ";
                            qry2 += " N'" + item.Vat + "',N'" + item.Amount.ToString() + "',N'" + item.Moq + "',N'" + item.Standard_packing + "',N'" + item.Unit_currency + "',N'" + item.Max_price + "',N'" + item.Min_price + "' \n ";
                        }
                        else
                        {
                            qry2 += " union all select N'" + txtPRNo.Text + "',N'" + item.Item_name + "',N'" + item.Erp_code + "',N'" + item.Hut_code + "', \n ";
                            qry2 += " N'" + item.Supplier_name + "',N'" + item.Quantity + "',N'" + item.Unit + "',N'" + item.Unit_price + "', \n ";
                            qry2 += " N'" + item.Vat + "',N'" + item.Amount.ToString() + "',N'" + item.Moq + "',N'" + item.Standard_packing + "',N'" + item.Unit_currency + "',N'" + item.Max_price + "',N'" + item.Min_price + "' \n ";
                        }
                    }
                }
                string qry3 = "";
                foreach (PUR_PR_CostSaving_Entity item in List_saving_item)
                {
                    if (!string.IsNullOrEmpty(item.Item_name))
                    {
                        if (qry3 == "")
                        {
                            qry3 += " select N'" + txtPRNo.Text + "',N'" + item.Item_name + "',N'" + item.Before_price + "',N'" + item.After_price + "', \n ";
                            qry3 += " N'" + item.Volume + "',N'" + item.Is_purchased + "' \n ";
                        }
                        else
                        {
                            qry3 += " union all select N'" + txtPRNo.Text + "',N'" + item.Item_name + "',N'" + item.Before_price + "',N'" + item.After_price + "', \n ";
                            qry3 += " N'" + item.Volume + "',N'" + item.Is_purchased + "' \n ";
                        }
                    }
                }
                string qry4 = "";
                foreach (PUR_PR_CompareCDE_Entity item in List_CompareCDE)
                {
                    if (!string.IsNullOrEmpty(item.Item_name))
                    {
                        if (qry4 == "")
                        {
                            qry4 += " select N'" + txtPRNo.Text + "',N'" + item.Item_name + "',N'" + item.Cde_budget + "',N'" + item.Actual_cost + "', \n ";
                            qry4 += " N'" + item.Remain_budget + "' \n ";
                        }
                        else
                        {
                            qry4 += " union all select N'" + txtPRNo.Text + "',N'" + item.Item_name + "',N'" + item.Cde_budget + "',N'" + item.Actual_cost + "', \n ";
                            qry4 += " N'" + item.Remain_budget + "' \n ";
                        }
                    }
                }
                string error = "";
                if (qry2 == "")
                {
                    error += "Not fill in the information of the item to buy\n";
                }
                if (dtpEstimateDate.Value.ToString("yyyyMMdd")==DateTime.Today.ToString("yyyyMMdd"))
                {
                    error += "Estimated receive date cannot be same date with today\n";
                }
                if (dtpExpectIssuePODate.Value.ToString("yyyyMMdd") == DateTime.Today.ToString("yyyyMMdd"))
                {
                    error += "Issue PO date cannot be same date with today\n";
                }
                if (cboRequestType.Text=="")
                {
                    error += "Not select the request type\n";
                }
                //--Check double
                var List_group_by = List_exist_item.Where(x => x.Item_name != null).GroupBy(x => x.Item_name).SelectMany(g => g.Skip(1)); ;
                if (List_group_by.Count() > 0)
                {
                    foreach (PUR_PRDetail_Entity item in List_group_by)
                    {
                        error += "Item "+item.Item_name+" has been double\n";
                    }
                }
                //-----
                if (error=="")
                {
                    string strQry = "";
                    strQry += "delete from PUR_PR where pr_no=N'" + txtPRNo.Text + "' \n";
                    strQry += "delete from PUR_PRDetail where pr_no=N'" + txtPRNo.Text + "' \n";
                    strQry += "delete from PUR_PR_CostSaving where pr_no=N'" + txtPRNo.Text + "' \n";
                    strQry += "delete from PUR_PR_CompareCDE where pr_no=N'" + txtPRNo.Text + "' \n";
                    strQry += "insert into PUR_PR([pr_no],[requester_date],[pr_date],[estimate_received_date],[pr_content] \n ";
                    strQry += " ,[pr_type],[requester],[dept],[amount] \n ";
                    strQry += " ,[vat],[pr_currency],[is_active],[create_user],pr_status,current_pic,customer_name,project_name,expect_issue_po_date,requester_sign,supplier_name,advance_payment,old_pr_no) \n ";
                    strQry += " select N'" + txtPRNo.Text + "',getdate(),N'" + dtpPRDate.Value.ToString("yyyy-MM-dd HH:mm:ss") + "',N'" + dtpEstimateDate.Value.ToString("yyyy-MM-dd") + "',N'" + txtRequesterComment.Text + "' \n ";
                    strQry += " ,N'" + cboRequestType.Text + "',N'" + txtRequester.Text + "',N'" + txtDept.Text + "',N'" + total_amount_without_vat + "' \n ";
                    strQry += " ,N'" + total_vat + "',N'" + txtCurrency.Text + "',N'1',N'" + General_Infor.username + "',N'Pending checker',N'"
                        + General_Infor.myaccount.Direct_checker + "',N'" + txtCust_name.Text + "',N'" + txtProject_name.Text + "',N'"
                        + dtpExpectIssuePODate.Value.ToString("yyyy-MM-dd") + "',N'" + General_Infor.myaccount.Signature + "',N'" + cboSupplier.Text + "',N'" + cboAdvancePayment.Text + "',N'" + old_PrNo + "' \n ";
                    if (kind == "Modify PR")
                    {
                        strQry += " update PUR_PR set pr_status=N'PR has been replaced by " + txtPRNo.Text + "' where pr_no=N'" + old_PrNo + "'\n";
                        strQry += " update PUR_PO set po_status=N'PO has been replaced by " + txtPRNo.Text + "' where po_no=N'" + Current_PR.Po_no + "'\n";
                    }
                    strQry += "insert into PUR_PRDetail ([pr_no],[item_name],[erp_code],[hut_code] \n ";
                    strQry += " ,[supplier_name],[quantity],[unit],[unit_price] \n ";
                    strQry += " ,[vat],[amount],moq,standard_packing,unit_currency,max_price,min_price) \n ";
                    strQry += qry2;
                    if (qry3 != "")
                    {
                        strQry += "insert into PUR_PR_CostSaving([pr_no],[item_name],[before_price],[after_price],[volume],[is_purchased]) \n";
                        strQry += qry3;
                    }
                    if (qry4 != "")
                    {
                        strQry += "insert into PUR_PR_CompareCDE(pr_no,item_name,cde_budget,actual_cost,remain_budget) \n";
                        strQry += qry4;
                    }
                    conn = new CmCn();
                    try
                    {
                        SplashScreenManager.ShowForm(this, typeof(frmWaitingForm), true, true, false);
                        SplashScreenManager.Default.SetWaitFormCaption("Please wait...");
                        conn.ExcuteQry(strQry);
                        string filepath = @"\\172.16.180.20\05.it\16.HVN\ATTACHMENT\PURCHASE\PR\" + txtPRNo.Text + ".pdf";
                        Export_PR(filepath, "begin");
                        Thread.Sleep(3000);
                        string PR_no = txtPRNo.Text;
                        string link_approve = "https://drive.google.com/uc?export=view&id=1qycuNDOYPDQk69vCylhOjRpPk0fNlCgB";
                        string link_reject = @"https://drive.google.com/uc?export=view&id=19meOrB4l9aIlltXXZLJA_49XnY-dkw8q";
                        string m_body = "<p>Dear " + checker_name + ",</p> \n\n ";
                        if (!string.IsNullOrEmpty(old_PrNo))
                        {
                            m_body += "<p>Please check the " + txtPRNo.Text + " (This PR is cancelling and replacing " + old_PrNo + ") as attached and the content:</p> \n ";
                        }
                        else
                        {
                            m_body += "<p>Please check the " + txtPRNo.Text + " as attached and the content:</p> \n ";
                        }
                        m_body += "<p>" + txtRequesterComment.Text + "</p> \n ";
                        m_body += "<p>Then click the button below to approve or reject:</p> \n ";
                        m_body += "<p><a href='mailto:hvn.system@hutchinson.vn?subject=Re:[HVN-System]:[PUR]:[PR]:" + PR_no + ":Yes&body=Note:'><img src='" + link_approve + "' width='108' height='35' alt='Approve' ></a></body></html></p> \n";
                        m_body += "<p><a href='mailto:hvn.system@hutchinson.vn?subject=Re:[HVN-System]:[PUR]:[PR]:" + PR_no + ":No&body=Reason:'><img src='" + link_reject + "' width='94' height='35'alt='Reject'></a></body></html></p> \n ";
                        m_body += "<p>History of PR:</p> \n";
                        m_body += "<p>PR made by " + txtRequester.Text + " at " + DateTime.Now.ToString("HH:mm dd/MM/yyyy") + "</p> \n";
                        m_body += "<p>Note: After send the email to approve or reject, you will not able to change your decision.</p> \n\n ";
                        m_body += "<p>Regards,</p> \n ";
                        SendEmail("[HVN-System]:[PUR]:[PR]:" + PR_no, email_direct_checker, "", m_body, filepath);
                        SplashScreenManager.CloseForm();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    kind = "Submit";
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Please check below error:\n" + error,"ERROR");
                }
                
            }
        }
        string email_direct_checker, checker_name;
        private bool Check_double()
        {
            bool isOK = true;
            var List_group_by = List_exist_item.Where(x => x.Item_name!=null).GroupBy(x=>x.Item_name).SelectMany(g => g.Skip(1)); ;
            if (List_group_by.Count()>0)
            {
                isOK = false;
            }
            return isOK;
        }
        private void Load_Direct_checker()
        {
            string strQry = "select * from Account where Username=N'" + General_Infor.myaccount.Direct_checker + "'";
            conn = new CmCn();
            DataTable dt = conn.ExcuteDataTable(strQry);
            if (dt.Rows.Count > 0)
            {
                email_direct_checker = dt.Rows[0]["Email_address"].ToString();
                checker_name = dt.Rows[0]["Name"].ToString();
            }
        }
        private void SendEmail(string Subject, string To, string Cc, string Body, string filepath)
        {
            Outlook.Application app = new Outlook.Application();
            Outlook.MailItem mailItem = app.CreateItem(Outlook.OlItemType.olMailItem);
            //mailItem.BodyFormat = Outlook.OlBodyFormat.olFormatPlain;
            mailItem.Subject = Subject;
            mailItem.To = To;
            if (!string.IsNullOrEmpty(Cc))
            {
                mailItem.CC = Cc;
            }
            mailItem.HTMLBody = Body;
            mailItem.Attachments.Add(filepath);//logPath is a string holding path to the log.txt file
            //mailItem.Importance = Outlook.OlImportance.olImportanceHigh;
            try
            {
                if (!string.IsNullOrEmpty(To))
                {
                    mailItem.Send();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void Load_PRNo()
        {
            string strQry = "select max(RIGHT((pr_no),2)) from PUR_PR where cast(pr_date as date)=N'" + DateTime.Today.ToString("yyyy-MM-dd") + "'";
            conn = new CmCn();
            string number = conn.ExcuteString(strQry);
            if (number == "")
            {
                txtPRNo.Text = "PR-" + DateTime.Today.ToString("yyMMdd") + "01";
            }
            else
            {
                int stt = int.Parse(number) + 1;
                if (stt<10)
                {
                    txtPRNo.Text = "PR-" + DateTime.Today.ToString("yyMMdd") + "0" + stt.ToString();
                }
                else
                {
                    txtPRNo.Text = "PR-" + DateTime.Today.ToString("yyMMdd") + stt.ToString();
                }
            }
        }
        private void Block_input_data()
        {
            dtpEstimateDate.Enabled = false;
            dtpExpectIssuePODate.Enabled = false;
            txtRequesterComment.ReadOnly = true;
            txtCust_name.ReadOnly = true;
            txtProject_name.ReadOnly = true;
            gvExistItem.OptionsBehavior.Editable = false;
            gvCDE.OptionsBehavior.Editable = false;
            gvSaving.OptionsBehavior.Editable = false;
            cboRequestType.Enabled = false;
            cboSupplier.Enabled = false;
            cboAdvancePayment.Enabled = false;
        }
        private void Load_permission()
        {
            adoClass = new ADO();
            btnApprove.Enabled = adoClass.Check_permission(this.Name, btnApprove.Name, General_Infor.username);
            btnReject.Enabled = adoClass.Check_permission(this.Name, btnApprove.Name, General_Infor.username);
        }
        string delegate_pic = "no one";
        private void Load_delegate()
        {
            string strQry = "select a.dl_requester from ADM_Delegation a,ADM_DelegationDetail b \n ";
            strQry += " where a.delegated_pic=N'" + General_Infor.username + "' \n ";
            strQry += " and a.dl_fromdate<getdate() and a.dl_todate>GETDATE() \n ";
            strQry += " and a.dl_id=b.dl_id \n ";
            strQry += " and b.toolbox_name=N'" + btnApprove.Name + "' \n ";
            strQry += " and b.frm_name=N'" + this.Name + "' \n ";
            strQry += " and a.is_active=N'1' \n ";
            conn = new CmCn();
            string result = conn.ExcuteString(strQry);
            if (result != "")
            {
                delegate_pic = result;
            }
            else
            {
                delegate_pic = "no one";
            }
        }
        private void frmProductionPlanFG_Load(object sender, EventArgs e)
        {
            Load_permission();
            Load_combobox();
            Load_delegate();
            Load_Direct_checker();
            btnSubmit.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            btnApprove.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            btnReject.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            btnSaveDraft.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            btnCancelPR.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            switch (kind)
            {
                case "New":
                    Load_PRNo();
                    Load_New_PR();
                    btnSaveDraft.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                    btnCancelPR.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                    btnSubmit.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                    txtRequesterComment.ReadOnly = false;
                    gvExistItem.OptionsBehavior.Editable = false;
                    gvSaving.OptionsBehavior.Editable = false;
                    gvCDE.OptionsBehavior.Editable = false;
                    break;
                case "View":
                    Load_Save_PR();
                    if (Current_PR.Current_pic == General_Infor.username|| Current_PR.Current_pic==delegate_pic)
                    {
                        if (Current_PR.Pr_status != "Pending requester")
                        {
                            btnApprove.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                            btnReject.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                        }
                    }
                    Block_input_data();
                    break;
                case "Edit":
                    Load_Save_PR();
                    txtRequesterComment.ReadOnly = false;
                    btnSaveDraft.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                    btnCancelPR.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                    btnSubmit.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                    break;
                case "Modify PR":
                    Load_Save_PR();
                    Load_PRNo();
                    txtRequesterComment.ReadOnly = false;
                    btnSaveDraft.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                    btnCancelPR.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                    btnSubmit.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                    break;
                default:
                    break;
            }
        }

        private void Load_New_PR()
        {
            txtRequester.Text = General_Infor.username;
            txtDept.Text = General_Infor.myaccount.Department;
            txtRequestDate.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
            txtRequestStatus.Text = "Pending";
            txtRequestStatus.ForeColor = Color.Red;
            txtChecker.Text = General_Infor.myaccount.Direct_checker;
            txtAmountWithVAT.Text = "0";
            txtVAT.Text = "0";
            txtTotalAmount.Text = "0";
            List_exist_item = new List<PUR_PRDetail_Entity>();
            for (int i = 1; i < 30; i++)
            {
                PUR_PRDetail_Entity item = new PUR_PRDetail_Entity();
                item.Stt = i;
                item.Pr_no = txtPRNo.Text;
                List_exist_item.Add(item);
            }
            List_saving_item = new List<PUR_PR_CostSaving_Entity>();
            for (int i = 1; i < 30; i++)
            {
                PUR_PR_CostSaving_Entity item = new PUR_PR_CostSaving_Entity();
                item.Stt = i;
                item.Pr_no = txtPRNo.Text;
                List_saving_item.Add(item);
            }
            List_CompareCDE = new List<PUR_PR_CompareCDE_Entity>();
            for (int i = 1; i < 30; i++)
            {
                PUR_PR_CompareCDE_Entity item = new PUR_PR_CompareCDE_Entity();
                item.Stt = i;
                item.Pr_no = txtPRNo.Text;
                List_CompareCDE.Add(item);
            }
            dgvExistItem.DataSource = List_exist_item.ToList();
            dgvSaving.DataSource = List_saving_item.ToList();
            dgvCDE.DataSource = List_CompareCDE.ToList();
        }
        private void Load_combobox()
        {
            string strQry = "";
            strQry = "select [item_name] as [Item Name],hut_code as [Hutchinson VN Code]," +
                "[unit_price] as [Price],unit_currency as [Currency] from PUR_MasterListItem";
            conn = new CmCn();
            DataTable dt = conn.ExcuteDataTable(strQry);
            cboItemName.DataSource = dt;
            cboItemName.DisplayMember = "Item Name";
            cboItemName.ValueMember = "Item Name";
            string strQry2 = "";
            if (kind == "New")
            {
                strQry2 = "select N'' as pr_type union all select pr_type from PUR_MasterList_TypePermission where position=N'" + General_Infor.myaccount.Position + "'";
            }
            else
            {
                switch (Current_PR.Pr_status)
                {
                    case "Draft":
                        strQry2 = "select pr_type from PUR_MasterList_TypePermission where position=N'" + General_Infor.myaccount.Position + "'";
                        break;
                    case "Pending requester":
                        strQry2 = "select pr_type from PUR_MasterList_TypePermission where position=N'" + General_Infor.myaccount.Position + "'";
                        break;
                    default:
                        strQry2 = "select pr_type from PUR_MasterList_TypePermission group by pr_type";
                        break;
                }
            }
            DataTable dt2 = conn.ExcuteDataTable(strQry2);
            if (dt2.Rows.Count > 0)
            {
                cboRequestType.DataSource = dt2;
                cboRequestType.DisplayMember = "pr_type";
                cboRequestType.ValueMember = "pr_type";
            }
            else
            {
                MessageBox.Show("You don't have the permission to create PR. Please contact IT!");
                this.Close();
            }
            string strQry3 = "Select supplier_name as [Supplier],sup_shortname as [Short name],sup_currency as [Currency] from PUR_MasterListSupplier where supplier_status=N'Active'";
            conn = new CmCn();
            DataTable dt3 = conn.ExcuteDataTable(strQry3);
            cboSupplier.Properties.DataSource = dt3;
            cboSupplier.Properties.ValueMember = "Supplier";
            cboSupplier.Properties.DisplayMember = "Supplier";
        }
        private void btnDelete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }
        PUR_PRDetail_Entity Current_exist_item;

        private void gvCDE_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            CurrentCDE_item = gvCDE.GetRow(gvCDE.FocusedRowHandle) as PUR_PR_CompareCDE_Entity;
            CurrentCDE_item.Remain_budget = CurrentCDE_item.Cde_budget - CurrentCDE_item.Actual_cost;
            CurrentCDE_item.Ultili_budget = CurrentCDE_item.Actual_cost / CurrentCDE_item.Cde_budget;
        }

        private void btnApprove_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (XtraMessageBox.Show("Do you want to APPROVE the PR?", "APPROVE PR", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                kind = "approve";
                SplashScreenManager.ShowForm(this, typeof(frmWaitingForm), true, true, false);
                SplashScreenManager.Default.SetWaitFormCaption("Please wait...");
                string strQry = "insert into [ADM_Inbox_HVN_System] ([subject],date_send,sender,body,is_process,kind) \n ";
                strQry += "   select N'Re:[HVN-System]:[PUR]:[PR]:" + txtPRNo.Text + ":Yes',getdate(),N'" + General_Infor.myaccount.Email_address + "', \n ";
                switch (Current_PR.Pr_status)
                {
                    case "Pending requester":
                        strQry += " N'" + txtRequesterComment.Text + "',N'No',N'System' \n ";
                        break;
                    case "Pending checker":
                        strQry += " N'" + txtCheckerComment.Text + "',N'No',N'System' \n ";
                        break;
                    case "Pending approver":
                        strQry += " N'" + txtApproveComment.Text + "',N'No',N'System' \n ";
                        break;
                    default:
                        strQry += " N'" + txtRequesterComment.Text + "',N'No',N'System' \n ";
                        break;
                }
                strQry += " update PUR_PR set capex_no =N'" + txtCapexNo.Text + "' where pr_no=N'" + txtPRNo.Text + "'\n";
                conn = new CmCn();
                conn.ExcuteQry(strQry);
                MessageBox.Show("The PR has been approved");
                SplashScreenManager.CloseForm();
                this.Close();
            }
        }

        PUR_PR_CompareCDE_Entity CurrentCDE_item;
        private void gvCDE_RowClick(object sender, RowClickEventArgs e)
        {
            CurrentCDE_item = gvCDE.GetRow(gvCDE.FocusedRowHandle) as PUR_PR_CompareCDE_Entity;
        }

        private void btnReject_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (XtraMessageBox.Show("Do you want to REJECT the PR?", "Reject PR", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                kind = "Reject";
                SplashScreenManager.ShowForm(this, typeof(frmWaitingForm), true, true, false);
                SplashScreenManager.Default.SetWaitFormCaption("Please wait...");
                string strQry = "insert into [ADM_Inbox_HVN_System] ([subject],date_send,sender,body,is_process,kind) \n ";
                strQry += "   select N'Re:[HVN-System]:[PUR]:[PR]:" + txtPRNo.Text + ":No',getdate(),N'" + General_Infor.myaccount.Email_address + "', \n ";
                switch (Current_PR.Pr_status)
                {
                    case "Pending requester":
                        strQry += " N'" + txtRequesterComment.Text + "',N'No',N'System' \n ";
                        break;
                    case "Pending checker":
                        strQry += " N'" + txtCheckerComment.Text + "',N'No',N'System' \n ";
                        break;
                    case "Pending approver":
                        strQry += " N'" + txtApproveComment.Text + "',N'No',N'System' \n ";
                        break;
                    default:
                        strQry += " N'" + txtRequesterComment.Text + "',N'No',N'System' \n ";
                        break;
                }
                conn = new CmCn();
                conn.ExcuteQry(strQry);
                MessageBox.Show("The PR has been rejected");
                SplashScreenManager.CloseForm();
                this.Close();
            }
        }
        private void Export_PR(string file_path, string kind)
        {
            PUR_PR_Entity item = new PUR_PR_Entity();
            item.Pr_no = txtPRNo.Text;
            item.Requester = txtRequester.Text;
            item.Pr_content = txtRequesterComment.Text;
            item.Pr_type = cboRequestType.Text;
            item.Pr_currency = txtCurrency.Text;
            item.Dept = txtDept.Text;
            item.Pr_date = DateTime.Now;
            item.Requester_sign = General_Infor.myaccount.Signature;
            item.Amount = total_amount_without_vat;
            item.Amount_vat = total_amount;
            item.Vat = total_vat;
            item.Advance_payment = cboAdvancePayment.Text;
            item.Old_pr_no = old_PrNo;
            item.Pr_status = "Pending checker";
            item.Estimate_received_date = dtpEstimateDate.Value;
            adoClass = new ADO();
            adoClass.Print_PUR_PR_Detail(item, file_path, kind);
        }
        private void dtpEstimateDate_ValueChanged(object sender, EventArgs e)
        {
            dtpEstimateDate.CustomFormat = "dd/MM/yyyy";
        }

        private void dtpExpectIssuePODate_ValueChanged(object sender, EventArgs e)
        {
            dtpExpectIssuePODate.CustomFormat = "dd/MM/yyyy";
        }

        private void cboRequestType_SelectionChangeCommitted(object sender, EventArgs e)
        {

        }

        private void gvExistItem_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {

            GridView view = sender as GridView;
            if (e.Column.FieldName == "Quantity" || e.Column.FieldName == "Item_name")
            {
                e.Appearance.BackColor = Color.LightGreen;
            }
        }

        private void gvExistItem_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            Current_exist_item = gvExistItem.GetRow(gvExistItem.FocusedRowHandle) as PUR_PRDetail_Entity;
            if (e.Column.FieldName == "Item_name")
            {
                if (string.IsNullOrEmpty(Current_exist_item.Item_name))
                {
                    Current_exist_item.Item_name = "";
                    Current_exist_item.Hut_code = "";
                    Current_exist_item.Erp_code = "";
                    Current_exist_item.Supplier_name = "";
                    Current_exist_item.Unit_currency = "";
                    Current_exist_item.Unit = "";
                    Current_exist_item.Vat = 0;
                    Current_exist_item.Unit_price = 0;
                    Current_exist_item.Moq = 0;
                    Current_exist_item.Quantity = 0;
                    Current_exist_item.Vat_amount = 0;
                    Current_exist_item.Amount = 0;
                    Current_exist_item.Standard_packing = 0;
                }
                else
                {
                    string strQry = "select * from PUR_MasterListItem where item_name=N'" + Current_exist_item.Item_name + "'";
                    conn = new CmCn();
                    DataTable dt = conn.ExcuteDataTable(strQry);
                    if (dt.Rows.Count > 0)
                    {
                        Current_exist_item.Hut_code = dt.Rows[0]["hut_code"].ToString();
                        Current_exist_item.Erp_code = dt.Rows[0]["erp_code"].ToString();
                        Current_exist_item.Supplier_name = dt.Rows[0]["supplier_name"].ToString();
                        Current_exist_item.Unit_currency = dt.Rows[0]["unit_currency"].ToString();
                        Current_exist_item.Unit = dt.Rows[0]["item_unit"].ToString();
                        Current_exist_item.Vat = string.IsNullOrEmpty(dt.Rows[0]["unit_vat"].ToString()) ? 0 : decimal.Parse(dt.Rows[0]["unit_vat"].ToString());
                        Current_exist_item.Unit_price = string.IsNullOrEmpty(dt.Rows[0]["unit_price"].ToString()) ? 0 : decimal.Parse(dt.Rows[0]["unit_price"].ToString());
                        Current_exist_item.Moq = string.IsNullOrEmpty(dt.Rows[0]["moq"].ToString()) ? 0 : decimal.Parse(dt.Rows[0]["moq"].ToString());
                        if (Current_exist_item.Moq > 0)
                        {
                            Current_exist_item.Quantity = Current_exist_item.Moq;
                            Current_exist_item.Vat_amount = Current_exist_item.Quantity * Current_exist_item.Vat * Current_exist_item.Unit_price;
                            Current_exist_item.Amount = Current_exist_item.Quantity * Current_exist_item.Unit_price + Current_exist_item.Vat_amount;
                            Update_amount();
                        }
                        Current_exist_item.Standard_packing = string.IsNullOrEmpty(dt.Rows[0]["standard_packing"].ToString()) ? 0 : decimal.Parse(dt.Rows[0]["standard_packing"].ToString());
                        Current_exist_item.Min_price = string.IsNullOrEmpty(dt.Rows[0]["min_price"].ToString()) ? Current_exist_item.Unit_price : decimal.Parse(dt.Rows[0]["min_price"].ToString());
                        Current_exist_item.Max_price = string.IsNullOrEmpty(dt.Rows[0]["max_price"].ToString()) ? Current_exist_item.Unit_price : decimal.Parse(dt.Rows[0]["max_price"].ToString());
                    }
                }
                var item = List_saving_item.FirstOrDefault(x => x.Stt == Current_exist_item.Stt);
                item.Pr_no = txtPRNo.Text;
                item.Item_name = Current_exist_item.Item_name;
                dgvSaving.DataSource = List_saving_item.ToList();
                var find = List_CompareCDE.FirstOrDefault(x => x.Stt == Current_exist_item.Stt);
                find.Item_name = Current_exist_item.Item_name;
                find.Pr_no = txtPRNo.Text;
                dgvCDE.DataSource = List_CompareCDE.ToList();
            }
            else if (e.Column.FieldName == "Quantity")
            {
                bool isOk = true;
                if (Current_exist_item.Quantity < Current_exist_item.Moq) //neu slg < MOQ
                {
                    MessageBox.Show("This item require quantity >= MOQ\nMặt hàng này yêu cầu số lượng >= MOQ ", "Error");
                    Current_exist_item.Quantity = Current_exist_item.Moq;
                    isOk = false;
                }
                else
                {
                    if (Current_exist_item.Standard_packing > 0) //Kiem tra xem co chia het cho std packing khong. Hang packing co std packing>0
                    {
                        decimal so_du = Current_exist_item.Quantity % Current_exist_item.Standard_packing;
                        if (so_du > 0)
                        {
                            MessageBox.Show("This item require match with standard packing\nMặt hàng này yêu cầu sô lượng chia hết cho số lượng tiêu chuẩn ", "Error");
                            Current_exist_item.Quantity = Current_exist_item.Moq;
                            isOk = false;
                        }
                        else
                        {
                            isOk = true;
                        }
                    }
                    else
                    {
                        isOk = true;
                    }
                }
                if (isOk)
                {
                    Update_amount();
                }
            }
            else if(e.Column.FieldName == "Unit_price")
            {
                if (Current_exist_item.Unit_price<Current_exist_item.Min_price || Current_exist_item.Unit_price > Current_exist_item.Max_price)
                {
                    MessageBox.Show("The price should be >="+ Current_exist_item.Min_price.ToString()+ " and <= "+ Current_exist_item.Max_price.ToString() + " ", "Error");
                    Current_exist_item.Unit_price = Current_exist_item.Min_price;
                }
                Update_amount();
            }
        }

        private void btnSaveDraft_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Update_amount();
            string qry2 = "";
            foreach (PUR_PRDetail_Entity item in List_exist_item)
            {
                if (item.Quantity > 0 && item.Item_name != "")
                {
                    if (qry2 == "")
                    {
                        qry2 += " select N'" + txtPRNo.Text + "',N'" + item.Item_name + "',N'" + item.Erp_code + "',N'" + item.Hut_code + "', \n ";
                        qry2 += " N'" + item.Supplier_name + "',N'" + item.Quantity + "',N'" + item.Unit + "',N'" + item.Unit_price + "', \n ";
                        qry2 += " N'" + item.Vat + "',N'" + item.Amount.ToString() + "',N'" + item.Moq + "',N'" + item.Standard_packing + "',N'" + item.Unit_currency + "' \n ";
                    }
                    else
                    {
                        qry2 += " union all select N'" + txtPRNo.Text + "',N'" + item.Item_name + "',N'" + item.Erp_code + "',N'" + item.Hut_code + "', \n ";
                        qry2 += " N'" + item.Supplier_name + "',N'" + item.Quantity + "',N'" + item.Unit + "',N'" + item.Unit_price + "', \n ";
                        qry2 += " N'" + item.Vat + "',N'" + item.Amount.ToString() + "',N'" + item.Moq + "',N'" + item.Standard_packing + "',N'" + item.Unit_currency + "' \n ";
                    }
                }
            }
            string qry3 = "";
            foreach (PUR_PR_CostSaving_Entity item in List_saving_item)
            {
                if (!string.IsNullOrEmpty(item.Item_name))
                {
                    if (qry3 == "")
                    {
                        qry3 += " select N'" + txtPRNo.Text + "',N'" + item.Item_name + "',N'" + item.Before_price + "',N'" + item.After_price + "', \n ";
                        qry3 += " N'" + item.Volume + "',N'" + item.Is_purchased + "' \n ";
                    }
                    else
                    {
                        qry3 += " union all select N'" + txtPRNo.Text + "',N'" + item.Item_name + "',N'" + item.Before_price + "',N'" + item.After_price + "', \n ";
                        qry3 += " N'" + item.Volume + "',N'" + item.Is_purchased + "' \n ";
                    }
                }
            }
            string qry4 = "";
            foreach (PUR_PR_CompareCDE_Entity item in List_CompareCDE)
            {
                if (!string.IsNullOrEmpty(item.Item_name))
                {
                    if (qry4 == "")
                    {
                        qry4 += " select N'" + txtPRNo.Text + "',N'" + item.Item_name + "',N'" + item.Cde_budget + "',N'" + item.Actual_cost + "', \n ";
                        qry4 += " N'" + item.Remain_budget + "' \n ";
                    }
                    else
                    {
                        qry4 += " union all select N'" + txtPRNo.Text + "',N'" + item.Item_name + "',N'" + item.Cde_budget + "',N'" + item.Actual_cost + "', \n ";
                        qry4 += " N'" + item.Remain_budget + "' \n ";
                    }
                }
            }
            if (qry2 != "")
            {
                string strQry = "";
                strQry += "delete from PUR_PR where pr_no=N'" + txtPRNo.Text + "' \n";
                strQry += "delete from PUR_PRDetail where pr_no=N'" + txtPRNo.Text + "' \n";
                strQry += "delete from PUR_PR_CostSaving where pr_no=N'" + txtPRNo.Text + "' \n";
                strQry += "delete from PUR_PR_CompareCDE where pr_no=N'" + txtPRNo.Text + "' \n";
                strQry += "insert into PUR_PR([pr_no],[requester_date],[pr_date],[estimate_received_date],[pr_content] \n ";
                strQry += " ,[pr_type],[requester],[dept],[amount] \n ";
                strQry += " ,[vat],[pr_currency],[is_active],[create_user],pr_status,current_pic,customer_name,project_name,expect_issue_po_date,requester_sign,supplier_name) \n ";
                strQry += " select N'" + txtPRNo.Text + "',getdate(),N'" + dtpPRDate.Value.ToString("yyyy-MM-dd HH:mm:ss") + "',N'" + dtpEstimateDate.Value.ToString("yyyy-MM-dd") + "',N'" + txtRequesterComment.Text + "' \n ";
                strQry += " ,N'" + cboRequestType.Text + "',N'" + txtRequester.Text + "',N'" + txtDept.Text + "',N'" + total_amount_without_vat + "' \n ";
                strQry += " ,N'" + total_vat + "',N'" + txtCurrency.Text + "',N'1',N'" + General_Infor.username + "',N'Draft',N'"
                    + General_Infor.username + "',N'" + txtCust_name.Text + "',N'" + txtProject_name.Text + "',N'"
                    + dtpExpectIssuePODate.Value.ToString("yyyy-MM-dd") + "',N'" + General_Infor.myaccount.Signature + "',N'" + cboSupplier.Text + "' \n ";
                strQry += "insert into PUR_PRDetail ([pr_no],[item_name],[erp_code],[hut_code] \n ";
                strQry += " ,[supplier_name],[quantity],[unit],[unit_price] \n ";
                strQry += " ,[vat],[amount],moq,standard_packing,unit_currency) \n ";
                strQry += qry2;
                if (qry3 != "")
                {
                    strQry += "insert into PUR_PR_CostSaving([pr_no],[item_name],[before_price],[after_price],[volume],[is_purchased]) \n";
                    strQry += qry3;
                }
                if (qry4 != "")
                {
                    strQry += "insert into PUR_PR_CompareCDE(pr_no,item_name,cde_budget,actual_cost,remain_budget) \n";
                    strQry += qry4;
                }
                conn = new CmCn();
                SplashScreenManager.ShowForm(this, typeof(frmWaitingForm), true, true, false);
                SplashScreenManager.Default.SetWaitFormCaption("Please wait...");
                try
                {
                    conn.ExcuteQry(strQry);
                    Thread.Sleep(1000);
                    kind = "SaveDraft";
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                SplashScreenManager.CloseForm();
            }
        }

        private void btnCancelPR_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (XtraMessageBox.Show("Do you want to CANCEL the PR?", "CANCEL PR", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                kind = "Cancel";
                string strQry = "";
                strQry += "delete from PUR_PR where pr_no=N'" + txtPRNo.Text + "' \n";
                strQry += "delete from PUR_PRDetail where pr_no=N'" + txtPRNo.Text + "' \n";
                strQry += "delete from PUR_PR_CostSaving where pr_no=N'" + txtPRNo.Text + "' \n";
                strQry += "delete from PUR_PR_CompareCDE where pr_no=N'" + txtPRNo.Text + "' \n";
                conn = new CmCn();
                conn.ExcuteQry(strQry);
                this.Close();
            }
        }

        private void cboSupplier_EditValueChanged(object sender, EventArgs e)
        {
            //Load infor of supplier
            string strQry = "select sup_currency from PUR_MasterListSupplier where supplier_name=N'" + cboSupplier.Text + "'";
            conn = new CmCn();
            txtCurrency.Text = conn.ExcuteString(strQry);
            //
            string strQry2 = "select [item_name] as [Item Name],hut_code as [Hutchinson VN Code]," +
                "[unit_price] as [Price],unit_currency as [Currency] from PUR_MasterListItem where supplier_name=N'" + cboSupplier.Text + "' and item_status=N'Active' and expired_date>getdate() ";
            DataTable dt2 = conn.ExcuteDataTable(strQry2);
            cboItemName.DataSource = dt2;
            cboItemName.DisplayMember = "Item Name";
            cboItemName.ValueMember = "Item Name";
            gvExistItem.OptionsBehavior.Editable = true;
            gvSaving.OptionsBehavior.Editable = true;
            gvCDE.OptionsBehavior.Editable = true;
            List_exist_item = new List<PUR_PRDetail_Entity>();
            for (int i = 1; i < 30; i++)
            {
                PUR_PRDetail_Entity item = new PUR_PRDetail_Entity();
                item.Stt = i;
                item.Pr_no = txtPRNo.Text;
                List_exist_item.Add(item);
            }
            List_saving_item = new List<PUR_PR_CostSaving_Entity>();
            for (int i = 1; i < 30; i++)
            {
                PUR_PR_CostSaving_Entity item = new PUR_PR_CostSaving_Entity();
                item.Stt = i;
                item.Pr_no = txtPRNo.Text;
                List_saving_item.Add(item);
            }
            List_CompareCDE = new List<PUR_PR_CompareCDE_Entity>();
            for (int i = 1; i < 30; i++)
            {
                PUR_PR_CompareCDE_Entity item = new PUR_PR_CompareCDE_Entity();
                item.Stt = i;
                item.Pr_no = txtPRNo.Text;
                List_CompareCDE.Add(item);
            }
            dgvExistItem.DataSource = List_exist_item.ToList();
            dgvSaving.DataSource = List_saving_item.ToList();
            dgvCDE.DataSource = List_CompareCDE.ToList();
        }

        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {

            Check_double();
        }

        private void cboAdvancePayment_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (cboAdvancePayment.Text!="")
            {
                cboAdvancePayment.ForeColor = Color.Red;
            }
        }

        private void frmPURPRDetail_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (kind=="New")
            {
                if (XtraMessageBox.Show("Do you want to save Draft before close?", "Saving", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    btnSaveDraft.PerformClick();
                }
                else
                {
                    
                }
            }
            else if (kind == "Edit")
            {
                if (XtraMessageBox.Show("Do you want to save Draft before close?", "Saving", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    btnSaveDraft.PerformClick();
                }
            }
        }

        private void Update_amount()
        {
            total_amount = 0; total_amount_without_vat = 0; total_vat = 0;total_qty = 0; ;
            foreach (PUR_PRDetail_Entity item in List_exist_item)
            {
                //decimal total_amount = 0;
                if (item.Item_name != "")
                {
                    item.Vat_amount = item.Quantity * item.Vat * item.Unit_price;
                    item.Amount = item.Quantity * item.Unit_price + item.Vat_amount;
                    total_amount += item.Amount;
                    total_vat += item.Vat_amount;
                    total_qty += item.Quantity;
                }
            }
            total_amount_without_vat = total_amount - total_vat;
            txtAmountWithVAT.Text = string.Format("{0:N2}", total_amount) + " " + txtCurrency.Text;
            txtTotalAmount.Text = string.Format("{0:N2}", total_amount_without_vat) + " " + txtCurrency.Text;
            txtVAT.Text = string.Format("{0:N2}", total_vat) + " " + txtCurrency.Text;
            lbQuantity.Text = total_qty.ToString();
        }
        private void cboItemName_EditValueChanged(object sender, EventArgs e)
        {
            if (gvExistItem.PostEditor())
            {
                gvExistItem.UpdateCurrentRow();
            }
        }

        private void gvExistItem_RowClick(object sender, RowClickEventArgs e)
        {
            //Current_item = gvExistItem.GetRow(gvExistItem.FocusedRowHandle) as PUR_PRDetail_Entity;
        }
    }
}
