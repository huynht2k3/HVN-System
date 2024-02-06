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
using System.Globalization;
using DevExpress.XtraBars;

namespace HVN_System.View.PUR
{
    public partial class frmPURPODetail : Form
    {
        public frmPURPODetail(PUR_PR_Entity _Pr)
        {
            InitializeComponent();
            PR = _Pr;
            PO_Type = "PO from PR";
        }
        public frmPURPODetail(PUR_PO_Entity _Po, string _PO_Type)
        {
            InitializeComponent();
            PO = _Po;
            PO_Type = _PO_Type;
            PR = new PUR_PR_Entity();
            Old_PO_no = PO.Po_no;
        }
        public frmPURPODetail()
        {
            InitializeComponent();
            PO_Type = "New PO";
            PR = new PUR_PR_Entity();
        }
        private CmCn conn;
        private string PO_Type, Supplier_shortname = "",Old_PO_no="",delegate_pic="no one";
        private PUR_PR_Entity PR;
        private PUR_PO_Entity PO;
        private List<PUR_PODetail_Entity> List_item;
        private void Load_permission()
        {
            adoClass = new ADO();
            btnApprove.Enabled = adoClass.Check_permission(this.Name, btnApprove.Name, General_Infor.username);
            btnReject.Enabled = adoClass.Check_permission(this.Name, btnApprove.Name, General_Infor.username);
            btnSubmit.Enabled= adoClass.Check_permission("frmPURPO", "btnNew", General_Infor.username);
            btnCancel.Enabled = adoClass.Check_permission("frmPURPO", "btnNew", General_Infor.username);
            btnSaveDraft.Enabled = adoClass.Check_permission("frmPURPO", "btnNew", General_Infor.username);
            btnF_Approve.Enabled = adoClass.Check_permission(this.Name, btnF_Approve.Name, General_Infor.username);
            btnF_Reject.Enabled = adoClass.Check_permission(this.Name, btnF_Approve.Name, General_Infor.username);
        }
        private void Load_delegate()
        {
            string strQry = "select a.dl_requester from ADM_Delegation a,ADM_DelegationDetail b \n ";
            strQry += " where a.delegated_pic=N'"+ General_Infor.username + "' \n ";
            strQry += " and a.dl_fromdate<getdate() and a.dl_todate>GETDATE() \n ";
            strQry += " and a.dl_id=b.dl_id \n ";
            strQry += " and b.toolbox_name=N'"+ btnApprove.Name + "' \n ";
            strQry += " and b.frm_name=N'"+ this.Name + "' \n ";
            strQry += " and a.is_active=N'1' \n ";
            conn = new CmCn();
            string result = conn.ExcuteString(strQry);
            if (result!="")
            {
                delegate_pic = result;
            }
            else
            {
                delegate_pic = "no one";
            }
        }
        private void frmPURPODetail_Load(object sender, EventArgs e)
        {
            if (General_Infor.username=="admin")
            {
                dtpPODate.Enabled = true;
            }
            Load_permission();
            Load_combobox();
            Load_delegate();
            btnApprove.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            btnReject.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            btnF_Approve.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            btnF_Reject.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            btnSubmit.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            btnCancel.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            btnSaveDraft.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            List_item = new List<PUR_PODetail_Entity>();
            switch (PO_Type)
            {
                case "PO from PR":
                    Load_PO_From_PR();
                    Load_PONo();
                    Load_PIC();
                    gridColumn9.OptionsColumn.ReadOnly = true;
                    btnSubmit.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                    btnCancel.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                    break;
                case "New PO":
                    Load_PONo();
                    Load_New_PO();
                    Load_PIC();
                    btnSubmit.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                    btnSaveDraft.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                    break;
                case "View PO":
                    Load_View_PO();
                    if (PO.Current_pic == General_Infor.username|| PO.Current_pic ==delegate_pic)
                    {
                        if(PO.Po_status == "Pending approve PO")
                        {
                            btnF_Approve.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                            btnF_Reject.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                        }
                        else if (PO.Po_status == "Pending check PO")
                        {
                            btnApprove.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                            btnReject.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                        }
                    }
                    break;
                case "Edit PO":
                    Load_View_PO();
                    Load_PIC();
                    gvResult.OptionsBehavior.Editable = true;
                    dtpPickupDate.Enabled = true;
                    btnSubmit.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                    cboSupplier.Enabled = true;
                    txtPIC.Text = General_Infor.username;
                    txtPICStatus.Text = "Pending";
                    txtDept.Text = General_Infor.myaccount.Department;
                    btnSaveDraft.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                    break;
                case "Modify PO":
                    Load_View_PO();
                    Load_PONo();
                    Load_PIC();
                    dtpPODate.Value = PO.Po_date;
                    gvResult.OptionsBehavior.Editable = true;
                    dtpPickupDate.Enabled = true;
                    btnSubmit.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                    cboSupplier.Enabled = true;
                    btnSaveDraft.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                    txtDeliveryMode.ReadOnly = false;
                    txtPurpose.ReadOnly = false;
                    break;
                default:
                    break;
            }
        }
        private ADM_Account checker_po;
        private void Load_PIC()
        {
            string strQry = "select a.pic_user,b.* \n ";
            strQry += " from ADM_PersonInChargeOfProcess a,Account b \n ";
            strQry += " where a.[procedure_name]=N'PO approval' and a.step_name=N'Checking PO' \n ";
            strQry += " and a.pic_user=b.Username \n ";
            conn = new CmCn();
            DataTable dt = conn.ExcuteDataTable(strQry);
            checker_po = new ADM_Account();
            checker_po.Username = dt.Rows[0]["username"].ToString();
            checker_po.Email_address = dt.Rows[0]["email_address"].ToString();
            checker_po.Signature = dt.Rows[0]["signature"].ToString();
            txtChecker.Text = checker_po.Username;
            string strQry2 = "";
            strQry2 = "select pic_user from ADM_PersonInChargeOfProcess where procedure_name=N'Approve PO' and step_name=N'" + txtDept.Text + "'";
            txtApprover.Text = string.IsNullOrEmpty(conn.ExcuteString(strQry2)) ? "bui.hoanvu" : conn.ExcuteString(strQry2);
        }
        private void Load_PONo()
        {
            string strQry = "select  max(substring(po_no,10,2)) from PUR_PO where cast(po_date as date)=N'" + dtpPODate.Value.ToString("yyyy-MM-dd") + "'";
            conn = new CmCn();
            string number = conn.ExcuteString(strQry);
            if (number == "")
            {
                txtPONo.Text = "PO-" + dtpPODate.Value.ToString("yyMMdd") + "01-" + Supplier_shortname + "-W" + txtPickupWeek.Text;
            }
            else
            {
                int stt = int.Parse(number) + 1;
                if (stt<10)
                {
                    txtPONo.Text = "PO-" + dtpPODate.Value.ToString("yyMMdd") + "0" + stt.ToString() + "-" + Supplier_shortname + "-W" + txtPickupWeek.Text;
                }
                else
                {
                    txtPONo.Text = "PO-" + dtpPODate.Value.ToString("yyMMdd") + stt.ToString() + "-" + Supplier_shortname + "-W" + txtPickupWeek.Text;
                }
            }
            //string strQry2 = " delete from PUR_PO where po_no like N'%" + txtPONo.Text.Substring(0, 12) + "' \n";
            //strQry2 += " insert into PUR_PO(po_no,po_date,po_pic,is_active,current_pic,po_status) \n  \n";
            //strQry2 += " select N'"+txtPONo.Text+"',getdate(),N'"+General_Infor.username+ "',N'1',N'" + General_Infor.username + "',N'Draft'";
            //conn.ExcuteQry(strQry2);
        }
        private void Load_combobox()
        {
            string strQry = "Select supplier_name as [Supplier],sup_shortname as [Short name] from PUR_MasterListSupplier where supplier_status=N'Active'";
            conn = new CmCn();
            DataTable dt = conn.ExcuteDataTable(strQry);
            cboSupplier.Properties.DataSource = dt;
            cboSupplier.Properties.ValueMember = "Supplier";
            cboSupplier.Properties.DisplayMember = "Supplier";
            string strQry2 = "";
            strQry2 = "select [item_name] as [Item Name],[supplier_name] as [Supplier name]," +
                "hut_code as [Hutchinson VN Code],unit_currency as [Currency] from PUR_MasterListItem where item_status=N'Active'";
            conn = new CmCn();
            DataTable dt2 = conn.ExcuteDataTable(strQry2);
            cboItemName.DataSource = dt2;
            cboItemName.DisplayMember = "Item Name";
            cboItemName.ValueMember = "Item Name";
        }
        private void Load_PO_From_PR()
        {
            if (PR.Old_pr_no!="")
            {
                string strQry2 = "select po_no from PUR_PR where pr_no=N'" + PR.Old_pr_no + "'";
                conn = new CmCn();
                Old_PO_no = conn.ExcuteString(strQry2);
                if (Old_PO_no!="")
                {
                    PO_Type = "Modify PO";
                    string strQry3 = "select po_date from PUR_PO where po_no=N'"+ Old_PO_no + "'";
                    dtpPODate.Value = DateTime.Parse(conn.ExcuteString(strQry3));
                }
            }
            string strQry = "select a.requester,a.pr_currency,a.advance_payment,a.amount,b.*,c.* \n ";
            strQry += " from PUR_PR a, PUR_PRDetail b,PUR_MasterListSupplier c \n ";
            strQry += " where a.pr_no=N'" + PR.Pr_no + "' \n ";
            strQry += " and a.pr_no=b.pr_no \n ";
            strQry += " and b.supplier_name=c.supplier_name \n ";
            conn = new CmCn();
            DataTable dt = conn.ExcuteDataTable(strQry);
            if (dt.Rows.Count > 0)
            {
                //Load PR infor
                dtpPickupDate.Value = PR.Estimate_received_date;
                txtPRNo.Text = PR.Pr_no;
                txtIssuePODate.Text = PR.Expect_issue_po_date.ToString("dd/MM/yyyy");
                txtRequestType.Text = PR.Pr_type;
                txtCAPEXNo.Text = PR.Capex_no;
                txtDept.Text = PR.Dept;
                txtPurpose.Text = PR.Pr_content;
                txtCustName.Text = PR.Customer_name;
                txtProjname.Text = PR.Project_name;
                txtPurpose.ReadOnly = true;
                txtCustName.ReadOnly = true;
                txtProjname.ReadOnly = true;
                lbVAT.Text = string.Format("{0:N2}", PR.Vat) + " " + PR.Pr_currency;
                total_amount_without_vat = PR.Amount;
                lbAmountNoVAT.Text = string.Format("{0:N2}", PR.Amount) + " " + PR.Pr_currency;
                total_vat = PR.Vat;
                lbAmount.Text = string.Format("{0:N2}", PR.Amount_vat) + " " + PR.Pr_currency;
                total_amount = PR.Amount_vat;
                
                //---------------
                txtPODate.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
                txtPIC.Text = General_Infor.username;
                cboSupplier.Text = dt.Rows[0]["supplier_name"].ToString();
                cboSupplier.ReadOnly = true;
                txtAddress.Text = dt.Rows[0]["sup_address"].ToString();
                txtTelNo.Text = dt.Rows[0]["sup_tel"].ToString();
                txtTaxCode.Text = dt.Rows[0]["tax_code"].ToString();
                txtContact.Text = dt.Rows[0]["contact_pic"].ToString();
                if (dt.Rows[0]["advance_payment"].ToString() != "")
                {
                    txtPaymentTerm.Text = dt.Rows[0]["advance_payment"].ToString();
                }
                else
                {
                    txtPaymentTerm.Text = dt.Rows[0]["payment_term"].ToString();
                }
                txtDeliveryMode.Text = dt.Rows[0]["delivery_mode"].ToString();
                txtEmail.Text = dt.Rows[0]["email_address"].ToString();
                txtIncoterm.Text = dt.Rows[0]["incoterm"].ToString();
                txtCurrency.Text = dt.Rows[0]["pr_currency"].ToString();
                txtPICStatus.Text = "Pending";
                int stt = 1;
                List_item = new List<PUR_PODetail_Entity>();
                foreach (DataRow row in dt.Rows)
                {
                    PUR_PODetail_Entity item = new PUR_PODetail_Entity();
                    item.Stt = stt;
                    item.Po_no = txtPONo.Text;
                    item.Item_name = row["item_name"].ToString();
                    item.Erp_code = row["erp_code"].ToString();
                    item.Hut_code = row["hut_code"].ToString();
                    item.Quantity = string.IsNullOrEmpty(row["quantity"].ToString()) ? 0 : decimal.Parse(row["quantity"].ToString());
                    item.Unit = row["unit"].ToString();
                    item.Unit_currency = row["unit_currency"].ToString();
                    item.Unit_price = string.IsNullOrEmpty(row["unit_price"].ToString()) ? 0 : decimal.Parse(row["unit_price"].ToString());
                    item.Vat = string.IsNullOrEmpty(row["vat"].ToString()) ? 0 : decimal.Parse(row["vat"].ToString());
                    item.Vat_amount = item.Quantity * item.Vat * item.Unit_price;
                    item.Amount = item.Quantity * item.Unit_price + item.Vat_amount;
                    item.Moq = string.IsNullOrEmpty(row["moq"].ToString()) ? 0 : decimal.Parse(row["moq"].ToString());
                    item.Standard_packing = string.IsNullOrEmpty(row["standard_packing"].ToString()) ? 0 : decimal.Parse(row["standard_packing"].ToString());
                    item.Min_price = item.Unit_price;
                    item.Max_price = item.Unit_price;
                    List_item.Add(item);
                    stt++;
                }
                dgvResult.DataSource = List_item.ToList();
                gvResult.OptionsBehavior.Editable = false;
            }

        }
        private void Load_New_PO()
        {
            PR = new PUR_PR_Entity();
            txtDept.Text = General_Infor.myaccount.Department;
            txtPICStatus.Text = "Pending";
            txtPODate.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
            txtPIC.Text = General_Infor.username;
            for (int i = 1; i < 30; i++)
            {
                PUR_PODetail_Entity item = new PUR_PODetail_Entity();
                item.Stt = i;
                item.Po_no = txtPONo.Text;
                List_item.Add(item);
            }
            dgvResult.DataSource = List_item.ToList();
            gvResult.OptionsBehavior.Editable = false;
        }
        private void Load_View_PO()
        {
            //Load PO infor
            txtPONo.Text = PO.Po_no;
            txtIncoterm.Text = PO.Incoterm;
            dtpPODate.Value = PO.Po_date;
            dtpPickupDate.Value = PO.Pickup_date;
            txtPaymentTerm.Text = PO.Payment_term;
            txtDeliveryMode.Text = PO.Delivery_mode;
            txtCurrency.Text = PO.Po_currency;
            txtPRNo.Text = PO.Pr_no;
            txtDept.Text = PO.Dept;
            txtPurpose.Text = PO.Purpose;
            txtCustName.Text = PO.Customer_name;
            txtProjname.Text = PO.Project_name;
            txtDeliveryMode.ReadOnly = true;
            dtpPickupDate.Enabled = false;
            txtPurpose.ReadOnly = true;
            txtCustName.ReadOnly = true;
            txtProjname.ReadOnly = true;
            txtPurchaserComment.Text = PO.Po_pic_comment;
            txtCheckerComment.Text = PO.Po_checker_comment;
            txtApproveComment.Text = PO.Po_approver_comment;
            lbVAT.Text = string.Format("{0:N2}", PO.Vat) + " " + PO.Po_currency;
            lbAmount.Text = string.Format("{0:N2}", PO.Amount_vat) + " " + PO.Po_currency;
            lbAmountNoVAT.Text = string.Format("{0:N2}", PO.Amount) + " " + PO.Po_currency;
            //dtpPODate.Value = DateTime.Now;
            //txtPODate.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
            txtCAPEXNo.Text= PO.Capex_no;
            txtRequestType.Text = PO.Po_type;
            cboSupplier.Text = PO.Supplier_name;
            cboSupplier.ReadOnly = true;
            txtPurpose.Text = PO.Purpose.ToString();
            if (PO_Type== "Modify PO")
            {
                txtPIC.Text = General_Infor.username;
                txtPODate.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
                txtPICStatus.Text = "Pending";
                dtpPODate.Value = DateTime.Now;
            }
            else
            {
                switch (PO.Po_status)
                {
                    case "Pending check PO":
                        txtPIC.Text = PO.Po_pic;
                        txtPODate.Text = PO.Po_pic_date.ToString("dd/MM/yyyy HH:mm");
                        txtPICStatus.Text = "Submitted";
                        txtPICStatus.ForeColor = Color.Green;
                        txtChecker.Text = PO.Current_pic;
                        txtCheckDate.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
                        txtCheck_status.Text = "Pending";
                        txtCheck_status.ForeColor = Color.Red;
                        break;
                    case "Pending approve PO":
                        txtPIC.Text = PO.Po_pic;
                        txtPODate.Text = PO.Po_pic_date.ToString("dd/MM/yyyy HH:mm");
                        txtPICStatus.Text = "Submitted";
                        txtPICStatus.ForeColor = Color.Green;
                        txtChecker.Text = PO.Po_checker;
                        txtCheckDate.Text = PO.Po_check_date.ToString("dd/MM/yyyy HH:mm");
                        txtCheck_status.Text = "Checked";
                        txtCheck_status.ForeColor = Color.Green;
                        txtApprover.Text = PO.Current_pic;
                        txtApproveDate.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
                        txtApprove_status.Text = "Pending";
                        txtApprove_status.ForeColor = Color.Red;
                        break;
                    case "PO approved":
                        DisplayAllSignature();
                        break;
                    case "PO has been sent to supplier":
                        DisplayAllSignature();
                        break;
                    case "Got PO confirmation":
                        DisplayAllSignature();
                        break;
                    default:
                        break;
                }
            }
            //--Load supplier
            string strQry = "select * from PUR_MasterListSupplier where supplier_name=N'" + PO.Supplier_name + "'";
            conn = new CmCn();
            DataTable dt = conn.ExcuteDataTable(strQry);
            if (dt.Rows.Count > 0)
            {
                cboSupplier.Text = dt.Rows[0]["supplier_name"].ToString();
                txtAddress.Text = dt.Rows[0]["sup_address"].ToString();
                txtEmail.Text = dt.Rows[0]["email_address"].ToString();
                txtTelNo.Text = dt.Rows[0]["sup_tel"].ToString();
                txtTaxCode.Text = dt.Rows[0]["tax_code"].ToString();
                txtContact.Text = dt.Rows[0]["contact_pic"].ToString();
            }
            //Load detail PO
            List_item = new List<PUR_PODetail_Entity>();
            string strQry2 = "select * from PUR_PODetail where po_no=N'" + PO.Po_no + "'";
            DataTable dt2 = conn.ExcuteDataTable(strQry2);
            if (dt2.Rows.Count > 0)
            {
                total_amount = 0;
                total_amount_without_vat = 0;
                total_vat = 0; total_quantity = 0;
                decimal stt = 1;
                foreach (DataRow row in dt2.Rows)
                {
                    PUR_PODetail_Entity item = new PUR_PODetail_Entity();
                    item.Stt = stt;
                    item.Po_no = row["Po_no"].ToString();
                    item.Item_name = row["Item_name"].ToString();
                    item.Erp_code = row["Erp_code"].ToString();
                    item.Hut_code = row["Hut_code"].ToString();
                    item.Quantity = decimal.Parse(row["Quantity"].ToString());
                    item.Unit = row["Unit"].ToString();
                    item.Unit_price = decimal.Parse(row["Unit_price"].ToString());
                    item.Unit_currency = row["Unit_currency"].ToString();
                    item.Vat = decimal.Parse(row["Vat"].ToString());
                    item.Vat_amount = item.Quantity * item.Vat * item.Unit_price;
                    item.Amount = item.Quantity * item.Unit_price + item.Vat_amount;
                    item.Moq = decimal.Parse(row["Moq"].ToString());
                    item.Standard_packing = decimal.Parse(row["Standard_packing"].ToString());
                    total_amount += item.Amount;
                    total_vat += item.Vat_amount;
                    total_amount_without_vat += item.Quantity * item.Unit_price;
                    total_quantity += item.Quantity;
                    List_item.Add(item);
                    stt++;
                }
                if (dt2.Rows.Count < 30)
                {
                    for (int i = 0; i < 30 - dt.Rows.Count; i++)
                    {
                        PUR_PODetail_Entity item = new PUR_PODetail_Entity();
                        item.Stt = stt;
                        item.Po_no = txtPONo.Text;
                        item.Quantity = 0;
                        item.Vat_amount = 0;
                        List_item.Add(item);
                        stt++;
                    }
                }
            }
            lbVAT.Text = string.Format("{0:N2}", total_vat) + " " + PO.Po_currency;
            lbAmount.Text = string.Format("{0:N2}", total_amount) + " " + PO.Po_currency;
            lbAmountNoVAT.Text = string.Format("{0:N2}", total_amount_without_vat) + " " + PO.Po_currency;
            lbQuantity.Text = total_quantity.ToString();
            dgvResult.DataSource = List_item.ToList();
            gvResult.OptionsBehavior.Editable = false;
        }
        private void DisplayAllSignature()
        {
            txtPIC.Text = PO.Po_pic;
            txtPODate.Text = PO.Po_pic_date.ToString("dd/MM/yyyy HH:mm");
            txtPICStatus.Text = "Submitted";
            txtPICStatus.ForeColor = Color.Green;
            txtChecker.Text = PO.Po_checker;
            txtCheckDate.Text = PO.Po_check_date.ToString("dd/MM/yyyy HH:mm");
            txtCheck_status.Text = "Checked";
            txtCheck_status.ForeColor = Color.Green;
            txtApprover.Text = PO.Po_approver;
            txtApproveDate.Text = PO.Po_approve_date.ToString("dd/MM/yyyy HH:mm");
            txtApprove_status.Text = "Approved";
            txtApprove_status.ForeColor = Color.Green;
        }
        private void btnSubmit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (XtraMessageBox.Show("Do you want to submit this PO?", "Submit PO", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (PO_Type != "Edit PO")
                {
                    Load_PONo();
                }
                Update_amount();
                string error = "";
                if (txtPickupWeek.Text == "")
                {
                    error += "Not yet input pickup date\n";
                }
                if (cboSupplier.Text=="")
                {
                    error += "Not yet select supplier\n";
                }
                string qry2 = "";
                foreach (PUR_PODetail_Entity item in List_item)
                {
                    if (item.Quantity > 0 && item.Item_name != "")
                    {
                        if (qry2 == "")
                        {
                            qry2 += " select N'" + txtPONo.Text + "',N'" + item.Item_name + "',N'" + item.Erp_code + "',N'" + item.Hut_code + "', \n ";
                            qry2 += " N'" + item.Quantity + "',N'" + item.Unit + "',N'" + item.Unit_price + "',N'" + item.Unit_currency + "', \n ";
                            qry2 += " N'" + item.Vat + "',N'" + item.Amount.ToString() + "',N'" + item.Moq + "',N'" + item.Standard_packing + "',N'" + item.Max_price + "',N'" + item.Min_price + "' \n ";
                        }
                        else
                        {
                            qry2 += " union all select N'" + txtPONo.Text + "',N'" + item.Item_name + "',N'" + item.Erp_code + "',N'" + item.Hut_code + "', \n ";
                            qry2 += " N'" + item.Quantity + "',N'" + item.Unit + "',N'" + item.Unit_price + "',N'" + item.Unit_currency + "', \n ";
                            qry2 += " N'" + item.Vat + "',N'" + item.Amount.ToString() + "',N'" + item.Moq + "',N'" + item.Standard_packing + "',N'" + item.Max_price + "',N'" + item.Min_price + "' \n ";
                        }
                    }
                }
                if (qry2 == "")
                {
                    error += "Not yet input information of item\n";
                }
                if (error=="")
                {
                    string approver = General_Infor.myaccount.Po_approver;
                    string requester = txtPIC.Text;
                    if (PO_Type == "PO from PR")
                    {
                        string strQry2 = "select po_approver from Account where Username=N'" + PR.Requester + "'";
                        conn = new CmCn();
                        approver = conn.ExcuteString(strQry2);
                        requester = PR.Requester;
                    }
                    string strQry = "";
                    strQry += "delete from PUR_PO where po_no=N'" + txtPONo.Text + "' \n";
                    strQry += "delete from PUR_PODetail where po_no=N'" + txtPONo.Text + "' \n";
                    strQry += " insert into PUR_PO(po_no,supplier_name,po_date,payment_term,last_time_update \n ";
                    strQry += " ,delivery_mode,incoterm,pickup_date,po_currency \n ";
                    strQry += " ,po_pic_date,po_pic,po_pic_sign,pr_no,po_type,capex_no \n ";
                    strQry += " ,is_active,po_status,requester,current_pic \n ";
                    strQry += " ,dept,customer_name,project_name,purpose,amount,vat,po_checker,po_approver,po_pic_comment,old_po_no) \n ";
                    strQry += " select N'" + txtPONo.Text + "',N'" + cboSupplier.Text + "',N'"+dtpPODate.Value.ToString("yyyy-MM-dd HH:mm:ss")+"',N'" + txtPaymentTerm.Text + "',getdate() \n ";
                    strQry += " ,N'" + txtDeliveryMode.Text + "',N'" + txtIncoterm.Text + "',N'" + dtpPickupDate.Value.ToString("yyyy-MM-dd") + "',N'" + txtCurrency.Text + "' \n ";
                    strQry += " ,getdate(),N'" + General_Infor.username + "',N'" + General_Infor.myaccount.Signature + "',N'" + txtPRNo.Text + "',N'" + txtRequestType.Text + "',N'" + txtCAPEXNo.Text + "' \n ";
                    strQry += " ,N'1',N'Pending check PO',N'" + requester + "',N'" + txtChecker.Text + "' \n ";
                    strQry += " ,N'" + txtDept.Text + "',N'" + txtCustName.Text + "',N'" + txtProjname.Text + "',N'" + txtPurpose.Text + "',N'" + total_amount.ToString() + "',N'"
                        + total_vat.ToString() + "',N'" + checker_po.Username + "',N'" + approver + "',N'" + txtPurchaserComment.Text + "',N'" + Old_PO_no + "' \n ";
                    if (PO_Type== "Modify PO")
                    {
                        strQry += " update PUR_PO set po_status=N'PO has been replaced by "+ txtPONo.Text + "' where po_no=N'"+Old_PO_no+"'\n";
                    }
                    strQry += " insert into PUR_PODetail(po_no,item_name,erp_code,hut_code, \n ";
                    strQry += " quantity,unit,unit_price,unit_currency, \n ";
                    strQry += " vat,amount,moq,standard_packing,max_price,min_price) \n ";
                    strQry += qry2;
                    if (!string.IsNullOrEmpty(txtPRNo.Text))
                    {
                        strQry += "update PUR_PR set pr_status=N'Pending check PO',po_no=N'"+ txtPONo.Text + "' where pr_no=N'" + PR.Pr_no + "'\n";
                    }
                    conn = new CmCn();
                    try
                    {
                        SplashScreenManager.ShowForm(this, typeof(frmWaitingForm), true, true, false);
                        SplashScreenManager.Default.SetWaitFormCaption("Please wait...");
                        conn.ExcuteQry(strQry);
                        string filepath = @"\\172.16.180.20\05.it\16.HVN\ATTACHMENT\PURCHASE\PO\" + txtPONo.Text + ".pdf";
                        Export_PO(filepath, "begin");
                        Thread.Sleep(3000);
                        string PO_no = txtPONo.Text;
                        string historyPR = "";
                        if (!string.IsNullOrEmpty(PR.Pr_no))
                        {
                            strQry += " update PUR_PR set pr_status=N'Pending approve PO' where pr_no=N'" + PR.Pr_no + " \n ";
                            historyPR += "<p>Request made by " + PR.Requester + " at " + PR.Pr_date.ToString("HH:mm dd/MM/yyyy") + "</p> \n";
                            historyPR += "<p>PR checked by " + PR.Checker + " at " + PR.Check_date.ToString("HH:mm dd/MM/yyyy") + "</p> \n";
                            historyPR += "<p>PR approved by " + PR.Approver + " at " + PR.Approve_date.ToString("HH:mm dd/MM/yyyy") + "</p> \n\n";
                        }
                        string link_approve = "https://drive.google.com/uc?export=view&id=1qycuNDOYPDQk69vCylhOjRpPk0fNlCgB";
                        string link_reject = "https://drive.google.com/uc?export=view&id=19meOrB4l9aIlltXXZLJA_49XnY-dkw8q";
                        string m_body = "<p>Dear,</p> \n\n ";
                        if (PO_Type == "Modify PO")
                        {
                            m_body += "<p>Please check the " + PO_no + " (This PO is cancelling and replacing "+Old_PO_no+") as attached and the content:</p> \n ";
                        }
                        else
                        {
                            m_body += "<p>Please check the " + PO_no + " as attached and the content:</p> \n ";
                        }
                        m_body += "<p>" + txtPurpose.Text + "</p> \n ";
                        m_body += "<p>Then click the button below to approve or reject:</p> \n ";
                        m_body += "<p><a href='mailto:hvn.system@hutchinson.vn?subject=Re:[HVN-System]:[PUR]:[PO]:" + PO_no + ":Yes&body=Note:'><img src='" + link_approve + "' width='108' height='35' alt='Approve' ></a></body></html></p> \n";
                        m_body += "<p><a href='mailto:hvn.system@hutchinson.vn?subject=Re:[HVN-System]:[PUR]:[PO]:" + PO_no + ":No&body=Reason:'><img src='" + link_reject + "' width='94' height='35'alt='Reject'></a></body></html></p> \n ";
                        m_body += historyPR;
                        m_body += "<p>PO made by " + General_Infor.myaccount.Username + " at " + DateTime.Now.ToString("HH:mm dd/MM/yyyy") + "</p> \n";
                        m_body += "<p>Note: After send the email to approve or reject, you will not able to change your decision.</p> \n\n ";
                        m_body += "<p>Regards,</p> \n ";
                        SendEmail("[HVN-System]:[PUR]:[PO]:" + PO_no, checker_po.Email_address, "", m_body, filepath);
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
                    MessageBox.Show(error,"List Error");
                }
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
        private void Export_PO(string file_path, string kind)
        {
            PUR_PO_Entity item = new PUR_PO_Entity();
            item.Po_no = txtPONo.Text;
            item.Supplier_name = cboSupplier.Text;
            item.Po_date = dtpPODate.Value;
            item.Payment_term = txtPaymentTerm.Text;
            item.Delivery_mode = txtDeliveryMode.Text;
            item.Incoterm = txtIncoterm.Text;
            item.Pickup_date = dtpPickupDate.Value;
            item.Po_currency = txtCurrency.Text;
            item.Po_pic = General_Infor.username;
            item.Po_pic_sign = General_Infor.myaccount.Signature;
            item.Po_status = "Pending check PO";
            item.Amount = total_amount;
            item.Vat = total_vat;
            item.Old_po_no = Old_PO_no;
            adoClass = new ADO();
            adoClass.Print_PUR_PO_Detail(item, file_path, kind);
        }
        private ADO adoClass;
        private void cboSupplier_EditValueChanged(object sender, EventArgs e)
        {
            string strQry = "select * from PUR_MasterListSupplier where supplier_name=N'" + cboSupplier.Text + "'";
            conn = new CmCn();
            DataTable dt = conn.ExcuteDataTable(strQry);
            cboSupplier.Text = dt.Rows[0]["supplier_name"].ToString();
            txtAddress.Text = dt.Rows[0]["sup_address"].ToString();
            Supplier_shortname = dt.Rows[0]["sup_shortname"].ToString();
            txtTelNo.Text = dt.Rows[0]["sup_tel"].ToString();
            txtTaxCode.Text = dt.Rows[0]["tax_code"].ToString();
            txtContact.Text = dt.Rows[0]["contact_pic"].ToString();
            txtPaymentTerm.Text = dt.Rows[0]["payment_term"].ToString();
            txtDeliveryMode.Text = dt.Rows[0]["delivery_mode"].ToString();
            txtIncoterm.Text = dt.Rows[0]["incoterm"].ToString();
            txtCurrency.Text = dt.Rows[0]["sup_currency"].ToString();
            txtEmail.Text = dt.Rows[0]["email_address"].ToString();
            List_item = new List<PUR_PODetail_Entity>();
            for (int i = 1; i < 30; i++)
            {
                PUR_PODetail_Entity item = new PUR_PODetail_Entity();
                item.Stt = i;
                item.Po_no = txtPONo.Text;
                List_item.Add(item);
            }
            dgvResult.DataSource = List_item.ToList();
            if (PO_Type == "New PO")
            {
                string strQry2 = "select [item_name] as [Item Name],[supplier_name] as [Supplier name]," +
                "hut_code as [Hutchinson VN Code],unit_currency as [Currency] from PUR_MasterListItem where supplier_name=N'" + cboSupplier.Text + "' and item_status=N'Active' ";
                conn = new CmCn();
                DataTable dt2 = conn.ExcuteDataTable(strQry2);
                cboItemName.DataSource = dt2;
                cboItemName.DisplayMember = "Item Name";
                cboItemName.ValueMember = "Item Name";
                gvResult.OptionsBehavior.Editable = true;
                Load_PONo();
            }
        }

        private void dtpPickupDate_ValueChanged(object sender, EventArgs e)
        {
            DateTimeFormatInfo dfi = DateTimeFormatInfo.CurrentInfo;
            Calendar myCal = dfi.Calendar;
            string WW = myCal.GetWeekOfYear(dtpPickupDate.Value, dfi.CalendarWeekRule, dfi.FirstDayOfWeek).ToString();
            txtPickupWeek.Text = WW;
            if (PO_Type== "New PO")
            {
                Load_PONo();
            }
        }

        private void gvResult_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            PUR_PODetail_Entity Current_exist_item = gvResult.GetRow(gvResult.FocusedRowHandle) as PUR_PODetail_Entity;
            if (e.Column.FieldName == "Item_name")
            {
                string strQry = "select * from PUR_MasterListItem where item_name=N'" + Current_exist_item.Item_name + "'";
                conn = new CmCn();
                DataTable dt = conn.ExcuteDataTable(strQry);
                if (dt.Rows.Count > 0)
                {
                    Current_exist_item.Hut_code = dt.Rows[0]["hut_code"].ToString();
                    Current_exist_item.Erp_code = dt.Rows[0]["erp_code"].ToString();
                    Current_exist_item.Unit_currency = dt.Rows[0]["unit_currency"].ToString();
                    Current_exist_item.Unit = dt.Rows[0]["item_unit"].ToString();
                    Current_exist_item.Unit_price = string.IsNullOrEmpty(dt.Rows[0]["unit_price"].ToString()) ? 0 : decimal.Parse(dt.Rows[0]["unit_price"].ToString());
                    Current_exist_item.Vat = string.IsNullOrEmpty(dt.Rows[0]["unit_vat"].ToString()) ? 0 : decimal.Parse(dt.Rows[0]["unit_vat"].ToString());
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
                else
                {
                    Current_exist_item.Hut_code = "";
                    Current_exist_item.Erp_code = "";
                    Current_exist_item.Unit = "";
                    Current_exist_item.Unit_price = 0;
                    Current_exist_item.Moq = 0;
                    Current_exist_item.Standard_packing = 0;
                }
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
            else if (e.Column.FieldName == "Unit_price")
            {
                if (Current_exist_item.Unit_price < Current_exist_item.Min_price || Current_exist_item.Unit_price > Current_exist_item.Max_price)
                {
                    MessageBox.Show("The price should be >=" + Current_exist_item.Min_price.ToString() + " and <= " + Current_exist_item.Max_price.ToString() + " ", "Error");
                    Current_exist_item.Unit_price = Current_exist_item.Min_price;
                }
                Update_amount();
            }
        }
        private void Update_amount()
        {
            total_amount_without_vat = 0; total_vat = 0; total_amount = 0;
            total_quantity = 0;
            foreach (PUR_PODetail_Entity item in List_item)
            {
                //decimal total_amount = 0;
                if (item.Item_name != "")
                {
                    item.Vat_amount = item.Quantity * item.Vat * item.Unit_price;
                    item.Amount = item.Quantity * item.Unit_price + item.Vat_amount;
                    total_amount += item.Amount;
                    total_vat += item.Vat_amount;
                    total_quantity += item.Quantity;
                }
            }
            total_amount_without_vat = total_amount - total_vat;
            lbAmount.Text = string.Format("{0:N2}", total_amount) + " " + txtCurrency.Text;
            lbAmountNoVAT.Text = string.Format("{0:N2}", total_amount_without_vat) + " " + txtCurrency.Text;
            lbVAT.Text = string.Format("{0:N2}", total_vat) + " " + txtCurrency.Text;
            lbQuantity.Text = total_quantity.ToString();
        }
        decimal total_amount = 0, total_amount_without_vat = 0;
        decimal total_vat = 0, total_quantity = 0;

        private void btnApprove_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (XtraMessageBox.Show("Do you want to APPROVE the PO?", "APPROVE PO", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                SplashScreenManager.ShowForm(this, typeof(frmWaitingForm), true, true, false);
                SplashScreenManager.Default.SetWaitFormCaption("Please wait...");
                string strQry = "insert into [ADM_Inbox_HVN_System] ([subject],date_send,sender,body,is_process,kind) \n ";
                strQry += "   select N'Re:[HVN-System]:[PUR]:[PO]:" + txtPONo.Text + ":Yes',getdate(),N'" + General_Infor.myaccount.Email_address + "', \n ";
                switch (PO.Po_status)
                {
                    case "Pending purchaser":
                        strQry += " N'" + txtPurchaserComment.Text + "',N'No',N'System' \n ";
                        break;
                    case "Pending check PO":
                        strQry += " N'" + txtCheckerComment.Text + "',N'No',N'System' \n ";
                        break;
                    case "Pending approve PO":
                        strQry += " N'" + txtApproveComment.Text + "',N'No',N'System' \n ";
                        break;
                    default:
                        strQry += " N'" + txtPurchaserComment.Text + "',N'No',N'System' \n ";
                        break;
                }
                conn = new CmCn();
                conn.ExcuteQry(strQry);
                Thread.Sleep(3000);
                SplashScreenManager.CloseForm();
                this.Close();
            }
        }

        private void btnF_Approve_ItemClick(object sender, ItemClickEventArgs e)
        {
            SplashScreenManager.ShowForm(this, typeof(frmWaitingForm), true, true, false);
            SplashScreenManager.Default.SetWaitFormCaption("Please wait...");
            string strQry = "insert into [ADM_Inbox_HVN_System] ([subject],date_send,sender,body,is_process,kind) \n ";
            strQry += "   select N'Re:[HVN-System]:[PUR]:[PO]:" + txtPONo.Text + ":Yes',getdate(),N'" + General_Infor.myaccount.Email_address + "', \n ";
            strQry += "   N'" + txtApproveComment.Text + "',N'No',N'System' \n ";
            conn = new CmCn();
            conn.ExcuteQry(strQry);
            Thread.Sleep(5000);
            SplashScreenManager.CloseForm();
            this.Close();
        }

        private void btnF_Reject_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (XtraMessageBox.Show("Do you want to REJECT the PO?", "REJECT PO", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                SplashScreenManager.ShowForm(this, typeof(frmWaitingForm), true, true, false);
                SplashScreenManager.Default.SetWaitFormCaption("Please wait...");
                string strQry = "insert into [ADM_Inbox_HVN_System] ([subject],date_send,sender,body,is_process,kind) \n ";
                strQry += "   select N'Re:[HVN-System]:[PUR]:[PO]:" + txtPONo.Text + ":No',getdate(),N'" + General_Infor.myaccount.Email_address + "', \n ";
                strQry += "   N'" + txtApproveComment.Text + "',N'No',N'System' \n ";
                conn = new CmCn();
                conn.ExcuteQry(strQry);
                Thread.Sleep(5000);
                SplashScreenManager.CloseForm();
                this.Close();
            }
        }

        private void btnSaveDraft_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }

        private void btnReject_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (XtraMessageBox.Show("Do you want to REJECT the PO?", "REJECT PO", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                SplashScreenManager.ShowForm(this, typeof(frmWaitingForm), true, true, false);
                SplashScreenManager.Default.SetWaitFormCaption("Please wait...");
                string strQry = "insert into [ADM_Inbox_HVN_System] ([subject],date_send,sender,body,is_process,kind) \n ";
                strQry += "   select N'Re:[HVN-System]:[PUR]:[PO]:" + txtPONo.Text + ":No',getdate(),N'" + General_Infor.myaccount.Email_address + "', \n ";
                switch (PO.Po_status)
                {
                    case "Pending purchaser":
                        strQry += " N'" + txtPurchaserComment.Text + "',N'No',N'System' \n ";
                        break;
                    case "Pending check PO":
                        strQry += " N'" + txtCheckerComment.Text + "',N'No',N'System' \n ";
                        break;
                    case "Pending approve PO":
                        strQry += " N'" + txtApproveComment.Text + "',N'No',N'System' \n ";
                        break;
                    default:
                        strQry += " N'" + txtPurchaserComment.Text + "',N'No',N'System' \n ";
                        break;
                }
                conn = new CmCn();
                conn.ExcuteQry(strQry);
                Thread.Sleep(3000);
                SplashScreenManager.CloseForm();
                //ntfPO.BalloonTipTitle = "HVN System";
                //ntfPO.BalloonTipText = "The PO has been rejected";
                //ntfPO.ShowBalloonTip(2000);
                this.Close();
            }
        }


        private void btnCancel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (XtraMessageBox.Show("Do you want to Cancel the PO?", "Cancel PO", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                string strQry = "update PUR_PO set is_active=N'0' where po_no =N'" + txtPONo.Text + "'";
                conn = new CmCn();
                conn.ExcuteQry(strQry);
                this.Close();
            }
        }

        private void cboItemName_EditValueChanged(object sender, EventArgs e)
        {
            if (gvResult.PostEditor())
            {
                gvResult.UpdateCurrentRow();
            }
        }
    }
}
