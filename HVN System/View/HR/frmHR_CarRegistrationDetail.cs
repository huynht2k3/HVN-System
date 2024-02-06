using DevExpress.XtraSplashScreen;
using HVN_System.Entity;
using HVN_System.Util;
using HVN_System.View.Admin;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Outlook = Microsoft.Office.Interop.Outlook;

namespace HVN_System.View.HR
{
    public partial class frmHR_CarRegistrationDetail : Form
    {
        public frmHR_CarRegistrationDetail()
        {
            InitializeComponent();
            type_request = "New";
        }
        public frmHR_CarRegistrationDetail(HR_CarRegistration_Entity _request, string _type)
        {
            InitializeComponent();
            Current_request = _request;
            type_request = _type;
        }
        private CmCn conn;
        private ADO adoClass;
        private string type_request, delegate_pic = "";
        private HR_CarRegistration_Entity Current_request;
        private void frmHR_Visitor_Info_Detail_Load(object sender, EventArgs e)
        {
            Load_permission();
            Load_delegate();
            btnApprove.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            btnReject.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            btnF_Approve.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            btnF_Reject.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            btnSubmit.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            btnCancel.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            btnSave.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            layoutControlItem4.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            switch (type_request)
            {
                case "New":
                    btnSubmit.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                    btnCancel.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                    cboFromTime.Text = "00:00:00";
                    cboToTime.Text = "00:00:00";
                    Load_request_no();
                    Check_PIC();
                    break;
                case "View":
                    Block_input_data();
                    Load_save_request();
                    if (Current_request.Current_pic == General_Infor.username || Current_request.Current_pic == delegate_pic)
                    {
                        
                        if (Current_request.Request_status == "Pending requester")
                        {

                        }
                        else if (Current_request.Request_status == "Draft")
                        {
                           
                        }
                        else if (Current_request.Request_status == "Pending plant mgr")
                        {
                            btnF_Approve.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                            btnF_Reject.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;

                        }
                        else
                        {
                            btnApprove.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                            btnReject.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                        }
                    }
                    break;
                case "Edit":
                    btnSubmit.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                    btnCancel.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                    Check_PIC();
                    Load_save_request();
                    break;
                default:
                    break;
            }
        }
        private void Check_PIC()
        {
            txtRequester.Text = General_Infor.username;
            txtDept.Text = General_Infor.myaccount.Department;
            txtRequesterDate.Text = DateTime.Now.ToString("dd/MM/yyyy HH:ss");
            txtRequesterStatus.Text = "Pending";
            txtRequesterStatus.ForeColor = Color.Red;
            txtDeptMgr.Text = General_Infor.myaccount.Direct_manager;
            string strQry = "select pic_user from ADM_PersonInChargeOfProcess where procedure_name=N'HR car approval' and step_name=N'HR'";
            conn = new CmCn();
            txtHR.Text = conn.ExcuteString(strQry);
            string strQry2 = "select pic_user from ADM_PersonInChargeOfProcess where procedure_name=N'HR car approval' and step_name=N'Plant Manager'";
            txtPlantMgr.Text = conn.ExcuteString(strQry2);
        }
        private void Load_save_request()
        {
            txtRequester.Text = Current_request.Requester;
            txtDeptMgr.Text = Current_request.Dept_mgr;
            txtHR.Text = Current_request.Hr_pic;
            txtPlantMgr.Text = Current_request.Plant_mgr;
            switch (Current_request.Request_status)
            {
                case "Pending requester":
                    txtRequesterDate.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
                    txtRequesterStatus.Text = "Pending";
                    break;
                case "Pending department mgr":
                    txtDeptMgrDate.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
                    txtRequesterStatus.Text = "Submitted";
                    txtDeptMgrStatus.Text = "Pending";
                    txtRequesterDate.Text = Current_request.Request_date.ToString("dd/MM/yyyy HH:mm");
                    break;
                case "Pending HR":
                    txtRequesterDate.Text = Current_request.Request_date.ToString("dd/MM/yyyy HH:mm");
                    txtDeptMgrDate.Text = Current_request.Dept_mgr_date.ToString("dd/MM/yyyy HH:mm");
                    txtHRDate.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
                    txtRequesterStatus.Text = "Submitted";
                    txtDeptMgrStatus.Text = "Checked";
                    txtHRStatus.Text = "Pending";
                    break;
                case "Pending plant mgr":
                    txtRequesterDate.Text = Current_request.Request_date.ToString("dd/MM/yyyy HH:mm");
                    txtDeptMgrDate.Text = Current_request.Dept_mgr_date.ToString("dd/MM/yyyy HH:mm");
                    txtHRDate.Text = Current_request.Hr_pic_date.ToString("dd/MM/yyyy HH:mm");
                    txtPlantMgr.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
                    txtRequesterStatus.Text = "Submitted";
                    txtDeptMgrStatus.Text = "Checked";
                    txtHRStatus.Text = "Checked";
                    txtPlantMgr.Text = "Pending";
                    break;
                case "Fully approve":
                    txtRequesterDate.Text = Current_request.Request_date.ToString("dd/MM/yyyy HH:mm");
                    txtDeptMgrDate.Text = Current_request.Dept_mgr_date.ToString("dd/MM/yyyy HH:mm");
                    txtHRDate.Text = Current_request.Hr_pic_date.ToString("dd/MM/yyyy HH:mm");
                    txtPlantMgr.Text = Current_request.Plant_mgr_date.ToString("dd/MM/yyyy HH:mm");
                    txtRequesterStatus.Text = "Submitted";
                    txtDeptMgrStatus.Text = "Checked";
                    txtHRStatus.Text = "Checked";
                    txtPlantMgr.Text = "Approved";
                    layoutControlItem4.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                    btnSave.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                    break;
                default:
                    break;
            }
            txtRegID.Text = Current_request.Request_id;
            cboCarType.Text = Current_request.Car_type;
            txtDept.Text = Current_request.Dept;
            dtpRequestDate.Value = Current_request.Request_date;
            dtpFromDate.Value = Current_request.From_date;
            cboFromTime.Text = Current_request.From_date.ToString("HH:mm:ss");
            dtpToDate.Value = Current_request.To_date;
            cboToTime.Text = Current_request.To_date.ToString("HH:mm:ss");
            txtFromLoc.Text = Current_request.From_loc;
            txtToLoc.Text = Current_request.To_loc;
            txtPurpose.Text = Current_request.Purpose;
            nmEstimateCost.Value = decimal.Parse(Current_request.Estimated_cost.ToString());
            nmActualCost.Value = decimal.Parse(Current_request.Actual_cost.ToString());
        }
        private void Block_input_data()
        {
            cboFromTime.Enabled = false;
            cboToTime.Enabled = false;
            txtFromLoc.ReadOnly = true;
            txtToLoc.ReadOnly = true;
            txtPurpose.ReadOnly = true;
            nmEstimateCost.ReadOnly = true;
        }
        private void Load_request_no()
        {
            string strQry = "select max(RIGHT((request_id),2)) from HR_CarRegistration  \n ";
            strQry += " where cast(request_date as date)=N'" + DateTime.Today.ToString("yyyy-MM-dd") + "' \n ";
            conn = new CmCn();
            string number = conn.ExcuteString(strQry);
            if (number == "")
            {
                txtRegID.Text = "HRCA-" + DateTime.Today.ToString("yyMMdd") + "01";
            }
            else
            {
                int stt = int.Parse(number) + 1;
                if (stt < 10)
                {
                    txtRegID.Text = "HRCA-" + DateTime.Today.ToString("yyMMdd") + "0" + stt.ToString();
                }
                else
                {
                    txtRegID.Text = "HRCA-" + DateTime.Today.ToString("yyMMdd") + stt.ToString();
                }
            }
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
        private void Load_permission()
        {
            adoClass = new ADO();
            btnSubmit.Enabled = adoClass.Check_permission("frmHR_CarRegistration", "btnNew", General_Infor.username);
            btnCancel.Enabled = adoClass.Check_permission("frmHR_CarRegistration", "btnNew", General_Infor.username);
            btnApprove.Enabled = adoClass.Check_permission(this.Name, btnApprove.Name, General_Infor.username);
            btnReject.Enabled = adoClass.Check_permission(this.Name, btnApprove.Name, General_Infor.username);
            btnF_Approve.Enabled = adoClass.Check_permission(this.Name, btnF_Approve.Name, General_Infor.username);
            btnF_Reject.Enabled = adoClass.Check_permission(this.Name, btnF_Approve.Name, General_Infor.username);
            btnSave.Enabled = adoClass.Check_permission(this.Name, btnSave.Name, General_Infor.username);
        }
        private void SendEmail(string Subject, string To, string Cc, string Body)
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
        private bool Check_condition()
        {
            string error = "";
            if (cboFromTime.Text=="00:00:00"|| cboToTime.Text == "00:00:00")
            {
                error += "Not input time go out/Chưa nhập thời gian ra ngoài \n";
            }
            if (txtFromLoc.Text==""|| txtToLoc.Text == "")
            {
                error += "Not input place/ Chưa nhập thông tin địa điểm \n";
            }
            if (txtPurpose.Text == "")
            {
                error += "Not input Purpose/Chưa nhập lý do \n";
            }
            if (error == "")
            {
                return true;
            }
            else
            {
                MessageBox.Show("Please check error below:\n" + error);
                return false;
            }
        }
        private void btnSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (MessageBox.Show("Do you want to submit this request?", "Submit car request", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Load_request_no();
                string strQry = "delete from HR_CarRegistration where request_id=N'"+txtRegID.Text+"' \n ";
                strQry += "insert into HR_CarRegistration(request_id,request_date,requester,dept \n ";
                strQry += " ,car_type,from_date,to_date,purpose \n ";
                strQry += " ,from_loc,to_loc \n ";
                strQry += " ,estimated_cost,actual_cost,current_pic,request_status \n ";
                strQry += " ,dept_mgr,hr_pic,plant_mgr,is_active,requester_sign) \n ";
                strQry += " select N'" + txtRegID.Text + "',getdate(),N'" + txtRequester.Text + "',N'"+General_Infor.myaccount.Department+"', \n ";
                strQry += " N'" + cboCarType.Text + "',N'" + dtpFromDate.Value.ToString("yyyy-MM-dd") +" "+cboFromTime.Text+ "',N'"+ dtpToDate.Value.ToString("yyyy-MM-dd") + " " + cboToTime.Text + "',N'" + txtPurpose.Text + "', \n ";
                strQry += " N'" + txtFromLoc.Text + "',N'" + txtToLoc.Text + "', \n ";
                strQry += " N'"+nmEstimateCost.Value.ToString()+ "',N'" + nmActualCost.Value.ToString() + "',N'" + General_Infor.myaccount.Direct_manager + "',N'Pending department mgr', \n ";
                strQry += " N'" + txtDeptMgr.Text + "',N'" + txtHR.Text + "',N'" + txtPlantMgr.Text + "',N'1',N'"+General_Infor.myaccount.Signature+"' \n ";
                conn = new CmCn();
                if (Check_condition())
                {
                    try
                    {
                        SplashScreenManager.ShowForm(this, typeof(frmWaitingForm), true, true, false);
                        SplashScreenManager.Default.SetWaitFormCaption("Please wait...");
                        conn.ExcuteQry(strQry);
                        string m_body = "<p>Dear,</p> \n\n ";
                        m_body += "<p>Please check and approve for the " + txtRegID.Text + " on HVN System with content: </p> \n ";
                        m_body += "<p> Type of car: " + cboCarType.Text + "\n ";
                        m_body += "<p> Time: " + dtpFromDate.Value.ToString("yyyy-MM-dd") + " " + cboFromTime.Text + "->" + dtpToDate.Value.ToString("yyyy-MM-dd") + " " + cboToTime.Text + "\n ";
                        m_body += "<p> Location: " + txtFromLoc.Text + "->" + txtToLoc.Text + "\n ";
                        m_body += "<p> Purpose: " + txtPurpose.Text + "\n ";
                        m_body += "<p> Estimated cost : " + nmEstimateCost.Value.ToString() + " (VND)\n ";
                        m_body += "<p>Regards,</p> \n ";
                        string dept_mgr_email = Search_email_address(General_Infor.myaccount.Direct_manager);
                        SendEmail("[HVN-System]:[HR]:[Car registration]:" + txtRegID.Text, dept_mgr_email, "", m_body);
                        SplashScreenManager.CloseForm();
                        this.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }
        private string Search_email_address(string username)
        {
            string strQry2 = "select Email_address from Account where Username=N'" + username + "'";
            conn = new CmCn();
            return conn.ExcuteString(strQry2);
        }

        private void txtCarryItem_TextChanged(object sender, EventArgs e)
        {
           
        }

        private void dtpFromDate_ValueChanged(object sender, EventArgs e)
        {
            dtpRequestDate.CustomFormat = "dd/MM/yyyy";
        }

        private void btnApprove_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (MessageBox.Show("Do you want to APPROVE this request?", "APPROVAL", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                SplashScreenManager.ShowForm(this, typeof(frmWaitingForm), true, true, false);
                SplashScreenManager.Default.SetWaitFormCaption("Please wait...");
                string strQry = "";
                string to = "";
                switch (Current_request.Request_status)
                {
                    case "Pending requester":
                        break;
                    case "Pending department mgr":
                        strQry += "update HR_CarRegistration set dept_mgr=N'"+General_Infor.username+"',dept_mgr_date=getdate()," +
                            "dept_mgr_sign=N'"+General_Infor.myaccount.Signature+"',request_status=N'Pending HR',current_pic=N'" + txtHR.Text + "' \n";
                        strQry += "where request_id=N'" + txtRegID.Text + "'\n";
                        to = txtHR.Text;
                        break;
                    case "Pending HR":
                        strQry += "update HR_CarRegistration set hr_pic=N'" + General_Infor.username + "', hr_pic_date=getdate()," +
                            "hr_pic_sign=N'" + General_Infor.myaccount.Signature + "',request_status =N'Pending plant mgr',current_pic=N'" + txtPlantMgr.Text + "' \n";
                        strQry += "where request_id=N'" + txtRegID.Text + "'\n";
                        to = txtPlantMgr.Text;
                        break;
                    case "Pending plant mgr":
                        strQry += "update HR_CarRegistration set plant_mgr=N'" + General_Infor.username + "'," +
                           "plant_mgr_sign=N'" + General_Infor.myaccount.Signature + "',plant_mgr_date =getdate(),request_status=N'Fully approve' \n";
                        strQry += "where request_id=N'" + txtRegID.Text + "'\n";
                        break;
                    case "Fully approve":
                        layoutControlItem4.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                        break;
                    default:
                        break;
                }
                if (strQry!="")
                {
                    conn = new CmCn();
                    conn.ExcuteQry(strQry);
                }
                if (to!="")
                {
                    string m_body = "<p>Dear,</p> \n\n ";
                    m_body += "<p>Please check and approve for the " + txtRegID.Text + " on HVN System with content: </p> \n ";
                    m_body += "<p> Type of car: " + cboCarType.Text + "\n ";
                    m_body += "<p> Time: " + dtpFromDate.Value.ToString("yyyy-MM-dd") + " " + cboFromTime.Text + "->" + dtpToDate.Value.ToString("yyyy-MM-dd") + " " + cboToTime.Text + "\n ";
                    m_body += "<p> Location: " + txtFromLoc.Text + "->" + txtToLoc.Text + "\n ";
                    m_body += "<p> Purpose: " + txtPurpose.Text + "\n ";
                    m_body += "<p> Estimated cost : " + nmEstimateCost.Value.ToString() + " (VND)\n ";
                    m_body += "<p>Regards,</p> \n ";
                    string received_email = Search_email_address(to);
                    SendEmail("[HVN-System]:[HR]:[Car registration]:" + txtRegID.Text, received_email, "", m_body);
                }
                SplashScreenManager.CloseForm();
                this.Close();
            }
        }

        private void nmEstimateCost_ValueChanged(object sender, EventArgs e)
        {

        }

        private void nmActualCost_ValueChanged(object sender, EventArgs e)
        {

        }

        private void btnSave_ItemClick_1(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (MessageBox.Show("Do you want to modify actual cost?", "Modify", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                SplashScreenManager.ShowForm(this, typeof(frmWaitingForm), true, true, false);
                SplashScreenManager.Default.SetWaitFormCaption("Please wait...");
                string strQry = "update HR_CarRegistration set actual_cost=N'" + nmActualCost.Value.ToString() + "'";
                strQry += "where request_id=N'" + txtRegID.Text + "'\n";
                conn = new CmCn();
                conn.ExcuteQry(strQry);
                SplashScreenManager.CloseForm();
                this.Close();
            }
               
        }

        private void btnReject_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (MessageBox.Show("Do you want to REJECT this request?", "REJECT", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                SplashScreenManager.ShowForm(this, typeof(frmWaitingForm), true, true, false);
                SplashScreenManager.Default.SetWaitFormCaption("Please wait...");
                string strQry = "update HR_CarRegistration set current_pic=N'"+Current_request.Requester+"',request_status=N'Pending requester'";
                strQry += "where request_id=N'" + txtRegID.Text + "'\n";
                conn = new CmCn();
                conn.ExcuteQry(strQry);
                SplashScreenManager.CloseForm();
                this.Close();
            }
        }

        private void dtpFromDate_ValueChanged_1(object sender, EventArgs e)
        {
            dtpToDate.Value = dtpFromDate.Value;
        }

        private void btnCancel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (MessageBox.Show("Do you want to CANCEL this request?", "CANCEL", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                SplashScreenManager.ShowForm(this, typeof(frmWaitingForm), true, true, false);
                SplashScreenManager.Default.SetWaitFormCaption("Please wait...");
                string strQry = "update HR_CarRegistration set is_active=N'0'\n";
                strQry += "where request_id=N'" + txtRegID.Text + "'\n";
                conn = new CmCn();
                conn.ExcuteQry(strQry);
                SplashScreenManager.CloseForm();
                this.Close();
            }
        }

        private void cboPurpose_SelectionChangeCommitted(object sender, EventArgs e)
        {

        }

    }
}
