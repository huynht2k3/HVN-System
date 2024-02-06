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
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraSplashScreen;
using HVN_System.View.Admin;
using HVN_System.View.PUR;
using Outlook = Microsoft.Office.Interop.Outlook;
using DevExpress.XtraBars;

namespace HVN_System.View.PUR
{
    public partial class frmPURMasterListItemChangeDetail : Form
    {
        public frmPURMasterListItemChangeDetail()
        {
            InitializeComponent();
            type = "New";
        }
        public frmPURMasterListItemChangeDetail(PUR_MasterListItem_Change_Entity current_rq, string _type)
        {
            InitializeComponent();
            Current_request = current_rq;
            type = _type;
        }
        private CmCn conn;
        private ADO adoClass;
        private PUR_MasterListItem_Change_Entity Current_request;
        private List<PUR_MasterListItem_ChangeDetail_Entity> List_changed;
        private string pur_email, finance_mgr_email, pur_mgr_email, plant_mgr_email;
        private string finance_mgr, plant_mgr, delegate_pic;
        string type;
        private void Load_permission()
        {
            adoClass = new ADO();
            btnApprove.Enabled = adoClass.Check_permission(this.Name, btnApprove.Name, General_Infor.username);
            btnReject.Enabled = adoClass.Check_permission(this.Name, btnApprove.Name, General_Infor.username);
            btnSubmit.Enabled = adoClass.Check_permission("frmPURMasterListItemChange", "btnNew", General_Infor.username);
            btnCancel.Enabled = adoClass.Check_permission("frmPURMasterListItemChange", "btnNew", General_Infor.username);
            btnSaveDraft.Enabled = adoClass.Check_permission("frmPURMasterListItemChange", "btnNew", General_Infor.username);
        }
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
        private void frmPURMasterListItemChangeDetail_Load(object sender, EventArgs e)
        {
            Load_permission();
            Load_delegate();
            Load_combobox();
            btnApprove.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            btnReject.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            btnCancel.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            switch (type)
            {
                case "New":
                    Load_PIC();
                    Load_RequestNo();
                    Current_request = new PUR_MasterListItem_Change_Entity();
                    txtRequesterDate.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
                    txtRequesterStatus.Text = "Pending";
                    btnCancel.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                    break;
                case "View":
                    if (Current_request.Request_status == "Draft")
                    {
                        Current_request.Request_status = "Pending requester";
                    }
                    Load_save_data();
                    cboItemName.Enabled = false;
                    btnSubmit.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                    btnSaveDraft.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                    gvResult.OptionsBehavior.Editable = false;
                    txtComment.ReadOnly = true;
                    if (Current_request.Request_status != "Pending requester"|| Current_request.Current_pic == delegate_pic)
                    {
                        if (Current_request.Current_pic == General_Infor.username)
                        {
                            btnApprove.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                            btnReject.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                        }
                    }
                    break;
                case "Edit":
                    Load_save_data();
                    Load_PIC();
                    btnCancel.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                    txtRequesterDate.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
                    txtRequesterStatus.Text = "Pending";
                    break;
                default:
                    break;
            }
        }
        private void Load_save_data()
        {
            txtRequestID.Text = Current_request.Request_id;
            cboItemName.Text = Current_request.Item_name;
            txtRequester.Text = Current_request.Requester;
            txtRequesterDate.Text = Current_request.Requester_date.ToString("dd/MM/yyyy HH:mm");
            txtDeptMgr.Text = Current_request.Dept_mgr;
            txtPur.Text = Current_request.Pur;
            txtPurMgr.Text = Current_request.Pur_mgr;
            txtFinMgr.Text = Current_request.Fin_mgr;
            txtPlantMgr.Text = Current_request.Plant_mgr;
            //Load Comment
            txtPurComment.Text = Current_request.Pur_comment;
            txtDeptMgrComment.Text = Current_request.Dept_comment;
            txtPurMgrComment.Text = Current_request.Pur_mgr_comment;
            txtFinMgrComment.Text = Current_request.Fin_mgr_comment;
            txtPurComment.Text = Current_request.Plant_mgr_comment;
            switch (Current_request.Request_status)
            {
                case "Pending requester":
                    txtRequesterStatus.Text = "pending";
                    break;
                case "Pending department mgr":
                    txtRequesterStatus.Text = "submitted";
                    txtDeptMgrStatus.Text = "pending";
                    txtDeptMgrDate.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
                    break;
                case "Pending purchaser":
                    txtRequesterStatus.Text = "submitted";
                    txtDeptMgrStatus.Text = "approved";
                    txtPurStatus.Text = "pending";
                    txtDeptMgrDate.Text = Current_request.Dept_mgr_date.ToString("dd/MM/yyyy HH:mm");
                    txtPurDate.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
                    break;
                case "Pending purchasing mgr":
                    txtDeptMgrStatus.Text = "approved";
                    txtPurStatus.Text = "approved";
                    txtPurMgrStatus.Text = "pending";
                    txtDeptMgrDate.Text = Current_request.Dept_mgr_date.ToString("dd/MM/yyyy HH:mm");
                    txtPurDate.Text = Current_request.Pur_date.ToString("dd/MM/yyyy HH:mm");
                    txtPurMgrDate.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
                    break;
                case "Pending finance mgr":
                    txtRequesterStatus.Text = "submitted";
                    txtDeptMgrStatus.Text = "approved";
                    txtPurStatus.Text = "approved";
                    txtPurMgrStatus.Text = "approved";
                    txtFinMgrStatus.Text = "pending";
                    txtDeptMgrDate.Text = Current_request.Dept_mgr_date.ToString("dd/MM/yyyy HH:mm");
                    txtPurDate.Text = Current_request.Pur_date.ToString("dd/MM/yyyy HH:mm");
                    txtPurMgrDate.Text = Current_request.Pur_mgr_date.ToString("dd/MM/yyyy HH:mm");
                    txtFinMgrDate.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
                    break;
                case "Pending plant mgr":
                    txtRequesterStatus.Text = "submitted";
                    txtDeptMgrStatus.Text = "approved";
                    txtPurStatus.Text = "approved";
                    txtFinMgrStatus.Text = "approved";
                    txtPurMgrStatus.Text = "approved";
                    txtPlantMgrStatus.Text = "pending";
                    txtDeptMgrDate.Text = Current_request.Dept_mgr_date.ToString("dd/MM/yyyy HH:mm");
                    txtPurDate.Text = Current_request.Pur_date.ToString("dd/MM/yyyy HH:mm");
                    txtPurMgrDate.Text = Current_request.Pur_mgr_date.ToString("dd/MM/yyyy HH:mm");
                    txtFinMgrDate.Text = Current_request.Fin_mgr_date.ToString("dd/MM/yyyy HH:mm");
                    txtPlantMgrDate.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
                    break;
                case "Fully approved":
                    txtDeptMgrStatus.Text = "approved";
                    txtPurStatus.Text = "approved";
                    txtFinMgrStatus.Text = "approved";
                    txtPurMgrStatus.Text = "approved";
                    txtDeptMgrDate.Text = Current_request.Dept_mgr_date.ToString("dd/MM/yyyy HH:mm");
                    txtPurDate.Text = Current_request.Pur_date.ToString("dd/MM/yyyy HH:mm");
                    txtPurMgrDate.Text = Current_request.Pur_mgr_date.ToString("dd/MM/yyyy HH:mm");
                    if (string.IsNullOrEmpty(txtFinMgr.Text))
                    {
                        layoutControlItem6.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                        layoutControlItem20.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                        layoutControlItem21.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                    }
                    else
                    {
                        txtFinMgrStatus.Text = "approved";
                        txtFinMgrDate.Text = Current_request.Fin_mgr_date.ToString("dd/MM/yyyy HH:mm");
                    }
                    if (string.IsNullOrEmpty(txtPlantMgr.Text))
                    {
                        layoutControlItem7.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                        layoutControlItem22.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                        layoutControlItem23.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                    }
                    else
                    {
                        txtPlantMgrStatus.Text = "approved";
                        txtPlantMgrDate.Text = Current_request.Plant_mgr_date.ToString("dd/MM/yyyy HH:mm");
                    }
                    break;
                default:
                    break;
            }
            string strQry = "select * from PUR_MasterListItem_ChangeDetail where request_id=N'" + txtRequestID.Text + "'";
            conn = new CmCn();
            DataTable dt = conn.ExcuteDataTable(strQry);
            if (dt.Rows.Count > 0)
            {
                List_changed = new List<PUR_MasterListItem_ChangeDetail_Entity>();
                foreach (DataRow row in dt.Rows)
                {
                    PUR_MasterListItem_ChangeDetail_Entity item = new PUR_MasterListItem_ChangeDetail_Entity();
                    item.Request_id = txtRequestID.Text;
                    item.Item_field = row["Item_field"].ToString();
                    item.Item_field_name = row["Item_field_name"].ToString();
                    item.Item_value = row["Item_value"].ToString();
                    item.Item_value_change = row["Item_value_change"].ToString();
                    List_changed.Add(item);
                }
                dgvResult.DataSource = List_changed.ToList();
            }
        }
        private void Check_PIC()
        {
            bool is_plus_PIC = false;
            txtFinMgr.Text = "";
            txtPlantMgr.Text = "";
            foreach (PUR_MasterListItem_ChangeDetail_Entity item in List_changed)
            {

                if (item.Item_value_change != "")
                {
                    switch (item.Item_field_name)
                    {
                        case "unit_currency":
                            is_plus_PIC = true;
                            break;
                        case "supplier_name":
                            is_plus_PIC = true;
                            break;
                        case "unit_price":
                            is_plus_PIC = true;
                            break;
                        case "max_price":
                            is_plus_PIC = true;
                            break;
                        case "min_price":
                            is_plus_PIC = true;
                            break;
                        default:
                            break;
                    }
                }
            }
            if (is_plus_PIC)
            {
                txtFinMgr.Text = finance_mgr;
                txtPlantMgr.Text = plant_mgr;
            }
        }
        private void Load_RequestNo()
        {
            string strQry = "select RIGHT(max(request_id),3) from PUR_MasterListItem_Change where cast(requester_date as date)=N'" + DateTime.Today.ToString("yyyy-MM-dd") + "'";
            conn = new CmCn();
            string number = conn.ExcuteString(strQry);
            if (number == "")
            {
                txtRequestID.Text = "CI-" + DateTime.Today.ToString("yyMMdd") + "001";
            }
            else
            {
                int stt = int.Parse(number) + 1;
                if (stt < 10)
                {
                    txtRequestID.Text = "CI-" + DateTime.Today.ToString("yyMMdd") + "00" + stt.ToString();
                }
                else if (stt < 100)
                {
                    txtRequestID.Text = "CI-" + DateTime.Today.ToString("yyMMdd") + "0" + stt.ToString();
                }
                else
                {
                    txtRequestID.Text = "CI-" + DateTime.Today.ToString("yyMMdd") + stt.ToString();
                }
            }
            string strQry2 = "insert into PUR_MasterListItem_Change (request_id,requester,requester_date,request_status,current_pic,dept) \n";
            strQry2 += "select N'" + txtRequestID.Text + "',N'" + General_Infor.username + "',getdate(),N'Draft',N'" + General_Infor.username + "',N'" + General_Infor.myaccount.Department + "'\n";
            conn.ExcuteQry(strQry2);
        }

        private void btnSubmit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (XtraMessageBox.Show("Do you want to submit this request?", "SUBMIT", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                if (cboItemName.Text != "")
                {
                    string check_cond = check_condition();
                    if (check_cond == "OK")
                    {
                        Check_PIC();
                        string strQry = "delete from PUR_MasterListItem_Change where request_id=N'" + txtRequestID.Text + "'\n";
                        strQry += " insert into PUR_MasterListItem_Change (request_id,item_name,requester,requester_date \n ";
                        strQry += "  ,dept_mgr,pur,pur_mgr,fin_mgr \n ";
                        strQry += "  ,plant_mgr,current_pic,request_status,note,dept) \n ";
                        strQry += "  select N'" + txtRequestID.Text + "',N'" + cboItemName.Text + "',N'" + General_Infor.username + "',getdate(), \n ";
                        strQry += "  N'" + txtDeptMgr.Text + "',N'" + txtPur.Text + "',N'" + txtPurMgr.Text + "',N'" + txtFinMgr.Text + "', \n ";
                        strQry += "  N'" + txtPlantMgr.Text + "',N'" + txtDeptMgr.Text + "',N'Pending department mgr',N'" + txtComment.Text + "',N'" + General_Infor.myaccount.Department + "' \n ";
                        //update detail
                        string qry2 = "";
                        string body_change = "";
                        foreach (PUR_MasterListItem_ChangeDetail_Entity item in List_changed)
                        {
                            if (qry2 == "")
                            {
                                qry2 += " select N'" + txtRequestID.Text + "',N'" + item.Item_field + "',N'" + item.Item_field_name + "',N'" + item.Item_value + "', N'" + item.Item_value_change + "' \n ";
                            }
                            else
                            {
                                qry2 += " union all select N'" + txtRequestID.Text + "',N'" + item.Item_field + "',N'" + item.Item_field_name + "',N'" + item.Item_value + "', N'" + item.Item_value_change + "' \n ";
                            }
                            if (item.Item_value_change.Trim() != "")
                            {
                                body_change += "-" + item.Item_field + ": " + item.Item_value + " -> " + item.Item_value_change + "\n";
                            }
                        }
                        if (body_change != "")
                        {
                            strQry += "delete from PUR_MasterListItem_ChangeDetail where request_id=N'" + txtRequestID.Text + "'\n";
                            strQry += "  insert into PUR_MasterListItem_ChangeDetail (request_id,item_field,item_field_name,item_value,item_value_change) \n ";
                            strQry += qry2;
                            conn = new CmCn();
                            try
                            {
                                SplashScreenManager.ShowForm(this, typeof(frmWaitingForm), true, true, false);
                                SplashScreenManager.Default.SetWaitFormCaption("Please wait...");
                                conn.ExcuteQry(strQry);
                                string To = PIC_emailadress(txtDeptMgr.Text);
                                string m_body = "";
                                string link_approve = @"https://drive.google.com/uc?export=view&id=1qycuNDOYPDQk69vCylhOjRpPk0fNlCgB";
                                string link_reject = @"https://drive.google.com/uc?export=view&id=19meOrB4l9aIlltXXZLJA_49XnY-dkw8q";
                                m_body += "<p>Dear ,</p> \n\n ";
                                m_body += "<p>Please check the " + txtRequestID.Text + " as attached with the note:</p> \n ";
                                m_body += "<p>" + txtComment.Text + "</p> \n ";
                                m_body += "<p>The item: " + cboItemName.Text + " has been change information as below:</p> \n ";
                                m_body += body_change;
                                m_body += "<p>Then click the button below to approve or reject:</p> \n ";
                                m_body += "<p><a href='mailto:hvn.system@hutchinson.vn?subject=Re:[HVN-System]:[PUR]:[Change item]:" + txtRequestID.Text + ":Yes&body=Note:'><img src='" + link_approve + "' width='108' height='35' alt='Approve' ></a></body></html></p> \n";
                                m_body += "<p><a href='mailto:hvn.system@hutchinson.vn?subject=Re:[HVN-System]:[PUR]:[Change item]:" + txtRequestID.Text + ":No&body=Reason:'><img src='" + link_reject + "' width='94' height='35'alt='Reject'></a></body></html></p> \n ";
                                m_body += "<p>Note: After send the email to approve or reject, you will not able to change your decision.</p> \n\n ";
                                m_body += "<p>Regards,</p> \n ";
                                SendEmail("[HVN-System]:[PUR]:[Change Item]:" + txtRequestID.Text, To, "", m_body, "");
                                SplashScreenManager.CloseForm();
                                this.Close();
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Please recheck your request. There is not any change in the request");
                        }
                    }
                    else
                    {
                        if (XtraMessageBox.Show(check_cond + ".Do you want to submit the vendor selection?", "Error", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                        {
                            frmPURAddNewItemDetail frm = new frmPURAddNewItemDetail(cboItemName.Text, "VS from Master Item", new_value);
                            frm.ShowDialog();
                            string strQry = "delete from PUR_MasterListItem_Change where request_id =N'" + txtRequestID.Text + "'\n";
                            strQry += "delete from PUR_MasterListItem_ChangeDetail where request_id =N'" + txtRequestID.Text + "'\n";
                            conn = new CmCn();
                            conn.ExcuteQry(strQry);
                            this.Close();
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Error: Please check item name");
                }
            }
        }
        float new_value = 0;
        private string check_condition()
        {
            string result = "OK";
            foreach (PUR_MasterListItem_ChangeDetail_Entity item in List_changed)
            {
                if (item.Item_field == "Price" && item.Item_value_change.Trim() != "")
                {
                    float current = float.Parse(item.Item_value);
                    new_value = float.Parse(item.Item_value_change);
                    if (new_value > current)
                    {
                        result = "New price is bigger than old price";
                    }
                }
            }
            return result;
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
            //mailItem.Attachments.Add(filepath);//logPath is a string holding path to the log.txt file
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
        private string PIC_emailadress(string pic)
        {
            string strQry = "select Email_address from Account where Username=N'" + pic + "'";
            conn = new CmCn();
            return conn.ExcuteString(strQry);
        }
        private void btnApprove_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (XtraMessageBox.Show("Do you want to APPROVE the request?", "APPROVE CHANGE INFO", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                string strQry = "";
                bool isClose = true;
                switch (Current_request.Request_status)
                {
                    case "Pending requester":
                        isClose = false;
                        break;
                    case "Pending department mgr":
                        strQry += " update PUR_MasterListItem_Change set dept_mgr=N'" + General_Infor.username + "', dept_mgr_date=getdate(),  request_status=N'Pending purchaser',current_pic=N'" + txtPur.Text + "' \n ";
                        strQry += ",dept_comment=N'" + txtCheckerComment.Text + "'\n";
                        strQry += " where request_id=N'" + Current_request.Request_id + "' \n ";
                        break;
                    case "Pending purchaser":
                        strQry += " update PUR_MasterListItem_Change set pur=N'" + General_Infor.username + "', pur_date=getdate(), request_status=N'Pending purchasing mgr',current_pic=N'" + txtPurMgr.Text + "' \n ";
                        strQry += ",pur_comment=N'" + txtCheckerComment.Text + "'\n";
                        strQry += " where request_id=N'" + Current_request.Request_id + "' \n ";
                        break;
                    case "Pending purchasing mgr":
                        if (txtFinMgr.Text == "")
                        {
                            strQry += " update PUR_MasterListItem_Change set pur_mgr=N'" + General_Infor.username + "', pur_mgr_date=getdate(), request_status=N'Fully approved',current_pic=N'' \n ";
                            strQry += ",pur_mgr_comment=N'" + txtCheckerComment.Text + "'\n";
                            strQry += " where request_id=N'" + Current_request.Request_id + "' \n ";
                            Final_approve();
                        }
                        else
                        {
                            strQry += " update PUR_MasterListItem_Change set pur_mgr=N'" + General_Infor.username + "', pur_mgr_date=getdate(), request_status=N'Pending finance mgr',current_pic=N'" + txtFinMgr.Text + "' \n ";
                            strQry += ",pur_mgr_comment=N'" + txtCheckerComment.Text + "'\n";
                            strQry += " where request_id=N'" + Current_request.Request_id + "' \n ";
                        }
                        break;
                    case "Pending finance mgr":
                        if (txtPlantMgr.Text == "")
                        {
                            strQry += " update PUR_MasterListItem_Change set fin_mgr=N'" + General_Infor.username + "', fin_mgr_date=getdate(), request_status=N'Fully approved',current_pic=N'' \n ";
                            strQry += ",fin_mgr_comment=N'" + txtCheckerComment.Text + "'\n";
                            strQry += " where request_id=N'" + Current_request.Request_id + "' \n ";
                            Final_approve();
                        }
                        else
                        {
                            strQry += " update PUR_MasterListItem_Change set fin_mgr=N'" + General_Infor.username + "', fin_mgr_date=getdate(),request_status=N'Pending plant mgr',current_pic=N'" + txtPlantMgr.Text + "' \n ";
                            strQry += ",fin_mgr_comment=N'" + txtCheckerComment.Text + "'\n";
                            strQry += " where request_id=N'" + Current_request.Request_id + "' \n ";
                        }
                        break;
                    case "Pending plant mgr":
                        strQry += " update PUR_MasterListItem_Change set plant_mgr=N'" + General_Infor.username + "', plant_mgr_date=getdate(), request_status=N'Fully approved',current_pic=N'' \n ";
                        strQry += ",plant_mgr_comment=N'" + txtCheckerComment.Text + "'\n";
                        strQry += " where request_id=N'" + Current_request.Request_id + "' \n ";
                        Final_approve();
                        break;
                    default:
                        isClose = false;
                        break;
                }
                if (isClose)
                {
                    strQry += "\ninsert into [ADM_Inbox_HVN_System] ([subject],date_send,sender,body,is_process,kind) \n ";
                    strQry += "   select N'Re:[HVN-System]:[PUR]:[Change Item]:" + txtRequestID.Text + ":Yes',getdate(),N'" + General_Infor.myaccount.Email_address + "', \n ";
                    strQry += "   N'" + txtCheckerComment.Text + "',N'No',N'System' \n ";
                    conn = new CmCn();
                    conn.ExcuteQry(strQry);
                    this.Close();
                }
            }
        }
        private void Final_approve()
        {
            string strQry = "";
            string qry2 = "";
            foreach (PUR_MasterListItem_ChangeDetail_Entity item in List_changed)
            {
                if (item.Item_value_change.Trim() != "")
                {
                    strQry += "update PUR_MasterListItem set " + item.Item_field_name + "= N'" + item.Item_value_change + "' where item_name =N'" + Current_request.Item_name + "' \n";
                    qry2 += item.Item_field + ":" + item.Item_value + "->" + item.Item_value_change + "; ";
                }
            }
            if (strQry != "")
            {
                strQry += "insert into PUR_MasterListItem_History (item_name,i_transaction,i_content,i_note,pic,input_time) \n";
                strQry += "select N'" + Current_request.Item_name + "',N'Modify information',N'" + qry2 + "',N'" + Current_request.Request_id + "',N'" + Current_request.Requester + "',N'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'";
                conn = new CmCn();
                conn.ExcuteQry(strQry);
            }
        }
        private void btnCancel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (XtraMessageBox.Show("Do you want to CANCEL this request?", "CANCEL", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                string strQry = "delete from PUR_MasterListItem_Change where request_id=N'" + txtRequestID.Text + "'\n";
                strQry += "delete from PUR_MasterListItem_ChangeDetail where request_id=N'" + txtRequestID.Text + "'\n";
                conn = new CmCn();
                conn.ExcuteQry(strQry);
                this.Close();
            }
        }

        private void btnReject_ItemClick(object sender, ItemClickEventArgs e)
        {
            string strQry = "update PUR_MasterListItem_Change set dept_mgr_date=null,pur_date=null,pur_mgr_date=null,fin_mgr_date=null \n";
            strQry += ",plant_mgr_date=null,request_status=N'Pending requester',current_pic=N'" + Current_request.Requester + "' \n ";
            switch (Current_request.Request_id)
            {
                case "Pending department mgr":
                    strQry += ",dept_comment=N'" + txtCheckerComment.Text + "' \n ";
                    break;
                case "Pending purchaser":
                    strQry += ",pur_comment=N'" + txtCheckerComment.Text + "' \n ";
                    break;
                case "Pending finance mgr":
                    strQry += ",fin_mgr_comment=N'" + txtCheckerComment.Text + "' \n ";
                    break;
                case "Pending purchasing mgr":
                    strQry += ",pur_mgr_comment=N'" + txtCheckerComment.Text + "' \n ";
                    break;
                case "Pending plant mgr":
                    strQry += ",plant_mgr_comment=N'" + txtCheckerComment.Text + "' \n ";
                    break;
                default:
                    break;
            }
            strQry += " where request_id=N'" + Current_request.Request_id + "' \n ";
            strQry += " insert into [ADM_Inbox_HVN_System] ([subject],date_send,sender,body,is_process,kind) \n ";
            strQry += " select N'Re:[HVN-System]:[PUR]:[Change Item]:" + txtRequestID.Text + ":No',getdate(),N'" + General_Infor.myaccount.Email_address + "', \n ";
            strQry += " N'" + txtCheckerComment.Text + "',N'No',N'System' \n ";
            conn = new CmCn();
            conn.ExcuteQry(strQry);
            this.Close();
        }

        private void btnSaveDraft_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Check_PIC();
            string strQry = "delete from PUR_MasterListItem_Change where request_id=N'" + txtRequestID.Text + "'\n";
            strQry += " insert into PUR_MasterListItem_Change (request_id,item_name,requester,requester_date \n ";
            strQry += "  ,dept_mgr,pur,pur_mgr,fin_mgr \n ";
            strQry += "  ,plant_mgr,current_pic,request_status,note,dept) \n ";
            strQry += "  select N'" + txtRequestID.Text + "',N'" + cboItemName.Text + "',N'" + General_Infor.username + "',getdate(), \n ";
            strQry += "  N'" + txtDeptMgr.Text + "',N'" + txtPur.Text + "',N'" + txtPurMgr.Text + "',N'" + txtFinMgr.Text + "', \n ";
            strQry += "  N'" + txtPlantMgr.Text + "',N'" + General_Infor.username + "',N'Draft',N'" + txtComment.Text + "',N'" + General_Infor.myaccount.Department + "' \n ";
            //update detail
            string qry2 = "";
            string body_change = "";
            foreach (PUR_MasterListItem_ChangeDetail_Entity item in List_changed)
            {
                if (qry2 == "")
                {
                    qry2 += " select N'" + txtRequestID.Text + "',N'" + item.Item_field + "',N'" + item.Item_field_name + "',N'" + item.Item_value + "', N'" + item.Item_value_change + "' \n ";
                }
                else
                {
                    qry2 += " union all select N'" + txtRequestID.Text + "',N'" + item.Item_field + "',N'" + item.Item_field_name + "',N'" + item.Item_value + "', N'" + item.Item_value_change + "' \n ";
                }
                if (item.Item_value_change.Trim() != "")
                {
                    body_change += item.Item_field + ": " + item.Item_value + " -> " + item.Item_value_change;
                }
            }
            if (qry2 != "")
            {
                strQry += "delete from PUR_MasterListItem_ChangeDetail where request_id=N'" + txtRequestID.Text + "'\n";
                strQry += "  insert into PUR_MasterListItem_ChangeDetail (request_id,item_field,item_field_name,item_value,item_value_change) \n ";
                strQry += qry2;
            }
            conn = new CmCn();
            try
            {
                SplashScreenManager.ShowForm(this, typeof(frmWaitingForm), true, true, false);
                SplashScreenManager.Default.SetWaitFormCaption("Please wait...");
                conn.ExcuteQry(strQry);
                SplashScreenManager.CloseForm();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Load_combobox()
        {
            string strQry = "";
            strQry = "select [item_name] as [Item name],erp_code as [ERP Code],supplier_name as [Supplier name] from PUR_MasterListItem ";
            conn = new CmCn();
            DataTable dt = conn.ExcuteDataTable(strQry);
            cboItemName.Properties.DataSource = dt;
            cboItemName.Properties.DisplayMember = "Item name";
            cboItemName.Properties.ValueMember = "Item name";
        }
        private void Load_PIC()
        {
            txtRequester.Text = General_Infor.myaccount.Username;
            txtDeptMgr.Text = General_Infor.myaccount.Direct_manager;
            string strQry2 = "select *  \n ";
            strQry2 += " from ADM_PersonInChargeOfProcess \n ";
            strQry2 += " where [procedure_name]=N'CI approval' \n ";
            conn = new CmCn();
            DataTable dt = conn.ExcuteDataTable(strQry2);
            foreach (DataRow item in dt.Rows)
            {
                switch (item["step_no"].ToString())
                {
                    case "1":
                        txtPur.Text = item["pic_user"].ToString();
                        pur_email = item["email_address"].ToString();
                        break;
                    case "2":
                        txtPurMgr.Text = item["pic_user"].ToString();
                        pur_mgr_email = item["email_address"].ToString();
                        break;
                    case "3":
                        finance_mgr = item["pic_user"].ToString();
                        finance_mgr_email = item["email_address"].ToString();
                        break;
                    case "4":
                        plant_mgr = item["pic_user"].ToString();
                        plant_mgr_email = item["email_address"].ToString();
                        break;
                    default:
                        break;
                }
                //Finance mgr
                layoutControlItem6.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                layoutControlItem20.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                layoutControlItem21.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                //Plant mgr
                layoutControlItem7.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                layoutControlItem22.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                layoutControlItem23.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            }
        }
        private void cboItemName_EditValueChanged(object sender, EventArgs e)
        {
            string strQry = "select * from PUR_MasterListItem where item_name=N'" + cboItemName.Text + "'";
            conn = new CmCn();
            DataTable dt = conn.ExcuteDataTable(strQry);
            if (dt.Rows.Count > 0)
            {
                List_changed = new List<PUR_MasterListItem_ChangeDetail_Entity>();
                PUR_MasterListItem_ChangeDetail_Entity item = new PUR_MasterListItem_ChangeDetail_Entity();
                item.Item_field = "ERP code";
                item.Item_field_name = "erp_code";
                item.Item_value = dt.Rows[0]["erp_code"].ToString();
                item.Item_value_change = "";
                List_changed.Add(item);
                item = new PUR_MasterListItem_ChangeDetail_Entity();
                item.Item_field = "HUTVN code";
                item.Item_field_name = "hut_code";
                item.Item_value = dt.Rows[0]["hut_code"].ToString();
                item.Item_value_change = "";
                List_changed.Add(item);
                item = new PUR_MasterListItem_ChangeDetail_Entity();
                item.Item_field = "Unit";
                item.Item_field_name = "item_unit";
                item.Item_value = dt.Rows[0]["item_unit"].ToString();
                item.Item_value_change = "";
                List_changed.Add(item);
                item = new PUR_MasterListItem_ChangeDetail_Entity();
                item.Item_field = "Price";
                item.Item_field_name = "unit_price";
                item.Item_value = dt.Rows[0]["unit_price"].ToString();
                item.Item_value_change = "";
                List_changed.Add(item);
                item = new PUR_MasterListItem_ChangeDetail_Entity();
                item.Item_field = "Standard transportation costs";
                item.Item_field_name = "delivery_cost";
                item.Item_value = dt.Rows[0]["delivery_cost"].ToString();
                item.Item_value_change = "";
                List_changed.Add(item);
                item = new PUR_MasterListItem_ChangeDetail_Entity();
                item.Item_field = "DDP cost";
                item.Item_field_name = "ddp_cost";
                item.Item_value = dt.Rows[0]["ddp_cost"].ToString();
                item.Item_value_change = "";
                List_changed.Add(item);
                item = new PUR_MasterListItem_ChangeDetail_Entity();
                item.Item_field = "Currency";
                item.Item_field_name = "unit_currency";
                item.Item_value = dt.Rows[0]["unit_currency"].ToString();
                item.Item_value_change = "";
                List_changed.Add(item);
                item = new PUR_MasterListItem_ChangeDetail_Entity();
                item.Item_field = "VAT";
                item.Item_field_name = "unit_vat";
                item.Item_value = dt.Rows[0]["unit_vat"].ToString();
                item.Item_value_change = "";
                List_changed.Add(item);
                item = new PUR_MasterListItem_ChangeDetail_Entity();
                item.Item_field = "Supplier name";
                item.Item_field_name = "supplier_name";
                item.Item_value = dt.Rows[0]["supplier_name"].ToString();
                item.Item_value_change = "";
                List_changed.Add(item);
                item = new PUR_MasterListItem_ChangeDetail_Entity();
                item.Item_field = "MOQ";
                item.Item_field_name = "moq";
                item.Item_value = dt.Rows[0]["moq"].ToString();
                item.Item_value_change = "";
                List_changed.Add(item);
                item = new PUR_MasterListItem_ChangeDetail_Entity();
                item.Item_field = "Unit of purchase";
                item.Item_field_name = "standard_packing";
                item.Item_value = dt.Rows[0]["standard_packing"].ToString();
                item.Item_value_change = "";
                List_changed.Add(item);
                item = new PUR_MasterListItem_ChangeDetail_Entity();
                item.Item_field = "Min price";
                item.Item_field_name = "min_price";
                item.Item_value = dt.Rows[0]["min_price"].ToString();
                item.Item_value_change = "";
                List_changed.Add(item);
                item = new PUR_MasterListItem_ChangeDetail_Entity();
                item.Item_field = "Max price";
                item.Item_field_name = "max_price";
                item.Item_value = dt.Rows[0]["max_price"].ToString();
                item.Item_value_change = "";
                List_changed.Add(item);
                dgvResult.DataSource = List_changed.ToList();
            }
        }
    }
}
