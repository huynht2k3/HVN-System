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
using HVN_System.View.PUR;
using Outlook = Microsoft.Office.Interop.Outlook;
using HVN_System.View.Admin;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraSplashScreen;
using System.Threading;
using DevExpress.XtraBars;

namespace HVN_System.View.PUR
{
    public partial class frmPURAddNewItemDetail : Form
    {
        public frmPURAddNewItemDetail()
        {
            InitializeComponent();
        }
        public frmPURAddNewItemDetail(PUR_VS_Entity vS_Entity, string _type)
        {
            InitializeComponent();
            type = _type;
            Current_VS = vS_Entity;
            
        }
        public frmPURAddNewItemDetail(string _item_name,string _type,float _newprice)
        {
            InitializeComponent();
            type = _type;
            Current_VS = new PUR_VS_Entity();
            Current_VS.Item_name = _item_name;
            Current_VS.Unit_price = _newprice;
        }
        private CmCn conn;
        //private ADO adoClass;
        private PUR_VS_Entity Current_VS;
        private string type,china_pur_mgr;
        private List<PUR_VS_SupplierInfo_Entity> Info_vendor1;
        private List<PUR_VS_SupplierInfo_Entity> Info_vendor2;
        private List<PUR_VS_SupplierInfo_Entity> Info_vendor3;
        private List<PUR_VS_SupplierInfo_Entity> selected_vendor;
        private PUR_VS_SupplierInfo_Entity current_item_vendor1;
        private PUR_VS_SupplierInfo_Entity current_item_vendor2;
        private PUR_VS_SupplierInfo_Entity current_item_vendor3;
        private string selected_vendor_type, delegate_pic;
        private string pur_email, finance_mgr_email, pur_mgr_email, plant_mgr_email, china_pur_mgr_email;
        private string attached1 = "", attached2 = "", attached3 = "";
        private List<string> list_pur_unit;
        
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
            btnApprove.Enabled = adoClass.Check_permission(this.Name, btnApprove.Name, General_Infor.username);
            btnReject.Enabled = adoClass.Check_permission(this.Name, btnApprove.Name, General_Infor.username);
            btnSubmit.Enabled = adoClass.Check_permission("frmPURAddNewItem", "btnNew", General_Infor.username);
            btnCancel.Enabled = adoClass.Check_permission("frmPURAddNewItem", "btnNew", General_Infor.username);
            btnSaveDraft.Enabled = adoClass.Check_permission("frmPURAddNewItem", "btnNew", General_Infor.username);
        }
        private void frmPURRegisterNewItem_Load(object sender, EventArgs e)
        {
            Load_permission();
            Load_delegate();
            btnCancel.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            btnSaveDraft.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            btnApprove.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            btnReject.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            Load_List_Pur_unit();
            if (type == "New")
            {
                Load_VSNo();
                Load_PIC();
                Load_vendor_field();
                Load_supplier_list();
                cboVendorType1.Text = "Exist Supplier";
                cboVendorType2.Text = "Exist Supplier";
                cboVendorType3.Text = "Exist Supplier";
                layoutControlItem12.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                layoutControlItem44.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                layoutControlItem45.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                layoutControlItem54.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                btnCancel.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                btnSaveDraft.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                lbError.Text = "2 suppliers must be fulfilled";
            }
            else if (type == "View VS")
            {
                Load_supplier_list();
                Load_VS_from_Request();
                btnSubmit.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                layoutControlItem2.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                cboSupplier1.Enabled = false;
                cboSupplier2.Enabled = false;
                cboSupplier3.Enabled = false;
                txtSupplier1.ReadOnly = true;
                txtSupplier2.ReadOnly = true;
                txtSupplier3.ReadOnly = true;
                btnAttach1.Enabled = false;
                btnAttach2.Enabled = false;
                btnAttach3.Enabled = false;
                cboVendorType1.Enabled = false;
                cboVendorType2.Enabled = false;
                cboVendorType3.Enabled = false;
                cboSelectedSupplier.Enabled = false;
                if (Current_VS.Current_pic == General_Infor.username|| Current_VS.Current_pic == delegate_pic)
                {
                    if (Current_VS.Vs_status != "Pending requester")
                    {
                        btnApprove.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                        btnReject.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                    }
                }
                switch (Current_VS.Vs_status)
                {
                    case "Pending requester":
                        break;
                    case "Pending department mgr":
                        txtRequesterStatus.Text = "Submitted";
                        txtDeptMgrStatus.Text = "pending";
                        Block_input();
                        break;
                    case "Pending purchaser":
                        txtRequesterStatus.Text = "Submitted";
                        txtDeptMgrStatus.Text = "approved";
                        txtPurStatus.Text = "pending";
                        txtDeptmgrDate.Text = Current_VS.Dept_mgr_date.ToString("dd/MM/yyyy HH:mm");
                        txtPurDate.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
                        break;
                    case "Pending finance mgr":
                        txtRequesterStatus.Text = "Submitted";
                        txtDeptMgrStatus.Text = "approved";
                        txtPurStatus.Text = "approved";
                        txtFinMgrStatus.Text = "pending";
                        txtDeptmgrDate.Text = Current_VS.Dept_mgr_date.ToString("dd/MM/yyyy HH:mm");
                        txtPurDate.Text = Current_VS.Pur_date.ToString("dd/MM/yyyy HH:mm");
                        txtFinDate.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
                        Block_input();
                        break;
                    case "Pending purchasing mgr":
                        txtRequesterStatus.Text = "Submitted";
                        txtDeptMgrStatus.Text = "approved";
                        txtPurStatus.Text = "approved";
                        txtFinMgrStatus.Text = "approved";
                        txtPurMgrStatus.Text = "pending";
                        txtDeptmgrDate.Text = Current_VS.Dept_mgr_date.ToString("dd/MM/yyyy HH:mm");
                        txtPurDate.Text = Current_VS.Pur_date.ToString("dd/MM/yyyy HH:mm");
                        txtFinDate.Text = Current_VS.Fin_mgr_date.ToString("dd/MM/yyyy HH:mm");
                        txtPurMgrDate.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
                        Block_input();
                        break;
                    case "Pending plant mgr":
                        txtRequesterStatus.Text = "Submitted";
                        txtDeptMgrStatus.Text = "approved";
                        txtPurStatus.Text = "approved";
                        txtFinMgrStatus.Text = "approved";
                        txtPurMgrStatus.Text = "approved";
                        txtPlantMgrStatus.Text = "pending";
                        txtDeptmgrDate.Text = Current_VS.Dept_mgr_date.ToString("dd/MM/yyyy HH:mm");
                        txtPurDate.Text = Current_VS.Pur_date.ToString("dd/MM/yyyy HH:mm");
                        txtPurMgrDate.Text = Current_VS.Pur_mgr_date.ToString("dd/MM/yyyy HH:mm");
                        txtFinDate.Text = Current_VS.Fin_mgr_date.ToString("dd/MM/yyyy HH:mm");
                        txtPlantDate.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
                        Block_input();
                        break;
                    case "Fully approved":
                        txtRequesterStatus.Text = "Submitted";
                        txtDeptMgrStatus.Text = "approved";
                        txtPurStatus.Text = "approved";
                        txtFinMgrStatus.Text = "approved";
                        txtPurMgrStatus.Text = "approved";
                        txtPlantMgrStatus.Text = "approved";
                        txtDeptmgrDate.Text = Current_VS.Dept_mgr_date.ToString("dd/MM/yyyy HH:mm");
                        txtPurDate.Text = Current_VS.Pur_date.ToString("dd/MM/yyyy HH:mm");
                        txtPurMgrDate.Text = Current_VS.Pur_mgr_date.ToString("dd/MM/yyyy HH:mm");
                        txtFinDate.Text = Current_VS.Fin_mgr_date.ToString("dd/MM/yyyy HH:mm");
                        txtPlantDate.Text = Current_VS.Plant_mgr_date.ToString("dd/MM/yyyy HH:mm");
                        Block_input();
                        break;
                    default:
                        break;
                }
            }
            else if (type == "Copy")
            {
                Current_VS.Vs_status = "Pending requester";
                Load_PIC();
                Load_VS_from_Request();
                Load_supplier_list();
                btnSubmit.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                btnSaveDraft.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                btnCancel.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                layoutControlItem54.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                txtRequester.Text = General_Infor.username;
                txtRequestDate.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
                txtRequesterStatus.Text = "pending";
                Load_VSNo();
            }
            else if (type=="Edit")
            {
                Load_supplier_list();
                Load_VS_from_Request();
                Load_PIC();
                btnSubmit.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                layoutControlItem54.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                layoutControlItem2.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                btnSaveDraft.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                btnCancel.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                txtRequesterStatus.Text = "pending";
            }
            else if (type == "VS from Master Item")
            {
                Load_VSNo();
                Load_PIC();
                Load_vendor_field();
                Load_supplier_list();
                Load_info_for_item(Current_VS.Item_name);
                cboVendorType1.Text = "Exist Supplier";
                cboVendorType2.Text = "Exist Supplier";
                cboVendorType3.Text = "Exist Supplier";
                layoutControlItem12.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                layoutControlItem44.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                layoutControlItem45.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                layoutControlItem54.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                btnCancel.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                btnSaveDraft.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                lbError.Text = "2 suppliers must be fulfilled";
                txtRequester.Text = General_Infor.myaccount.Username;
                txtRequestDate.Text = DateTime.Now.ToString("dd/MM/yyyy HH:ss");
                txtRequesterStatus.Text = "Pending";
            }
        }
        private void Load_info_for_item(string item_name)
        {
            string strQry = "select * from PUR_MasterListItem where item_name=N'" + item_name + "'";
            conn = new CmCn();
            DataTable dt = conn.ExcuteDataTable(strQry);
            Auto_insert_sup_info_2("EXW price", Current_VS.Unit_price.ToString(), Info_vendor1);
            Auto_insert_sup_info_2("Standard transportation costs", "0", Info_vendor1);
            Auto_insert_sup_info_2("DDP cost", Current_VS.Unit_price.ToString(), Info_vendor1);
            Auto_insert_sup_info_2("Item name", item_name, Info_vendor1);
            Auto_insert_sup_info_2("ERP item code", dt.Rows[0]["erp_code"].ToString(), Info_vendor1);
            Auto_insert_sup_info_2("HVN code", dt.Rows[0]["hut_code"].ToString(), Info_vendor1);
            Auto_insert_sup_info_2("Unit", dt.Rows[0]["item_unit"].ToString(), Info_vendor1);
            Auto_insert_sup_info_2("MOQ", dt.Rows[0]["moq"].ToString(), Info_vendor1);
            Auto_insert_sup_info_2("Unit of purchase", dt.Rows[0]["standard_packing"].ToString(), Info_vendor1);
            string vat = string.IsNullOrEmpty(dt.Rows[0]["unit_vat"].ToString()) ? "0%" : (float.Parse(dt.Rows[0]["unit_vat"].ToString()) * 100).ToString() + "%";
            Auto_insert_sup_info("VAT", vat, Info_vendor1);
            cboSupplier1.Text = dt.Rows[0]["supplier_name"].ToString();
        }
        private void Load_List_Pur_unit()
        {
            list_pur_unit = new List<string>();
            string strQry = "select child_name from ADM_MasterListParameter where parent_id=N'pur_unit'";
            conn = new CmCn();
            DataTable dt = conn.ExcuteDataTable(strQry);
            string hint_unit = "";
            foreach (DataRow item in dt.Rows)
            {
                list_pur_unit.Add(item["child_name"].ToString());
                hint_unit += item["child_name"].ToString() + "/";
            }
            lbUnitHint.Text = "Note 3: List accepted unit:" + hint_unit;
        }
        private void Block_input()
        {
            gvVendor1.OptionsBehavior.Editable = false;
            gvVendor2.OptionsBehavior.Editable = false;
            gvVendor3.OptionsBehavior.Editable = false;
            cboSupplier1.Enabled = false;
            cboSupplier2.Enabled = false;
            cboSupplier3.Enabled = false;
            txtSupplier1.ReadOnly = true;
            txtSupplier2.ReadOnly = true;
            txtSupplier3.ReadOnly = true;
            btnAttach1.Enabled = false;
            btnAttach2.Enabled = false;
            btnAttach3.Enabled = false;
        }
        private void Load_VS_from_Request()
        {
            txtVsNo.Text = Current_VS.Vs_id;
            txtDescription.Text = Current_VS.Vs_des;
            txtRequester.Text = Current_VS.Vs_requester;
            txtRequestDate.Text = Current_VS.Vs_date.ToString("dd/MM/yyyy HH:mm");
            txtDeptMgr.Text = Current_VS.Dept_mgr;
            txtFinMgr.Text = Current_VS.Fin_mgr;
            txtPurMgr.Text = Current_VS.Pur_mgr;
            txtPlantMgr.Text = Current_VS.Plant_mgr;
            txtPur.Text = Current_VS.Pur;
            nmEstimatedCost.Value = Current_VS.Estimate_yearly_amount;
            //Load Comment
            txtRequesterComment.Text = Current_VS.Vs_comment;
            txtPurComment.Text= Current_VS.Pur_comment;
            txtDeptMgrComment.Text = Current_VS.Dept_comment;
            txtFinMgrComment.Text = Current_VS.Fin_mgr_comment;
            txtPurMgrComment.Text = Current_VS.Pur_mgr_comment;
            txtPlantMgrComment.Text = Current_VS.Plant_mgr_comment;
            //----
            conn = new CmCn();
            string strQry1 = "select a.*,b.vs_des_status,b.vs_score_status,b.vs_des_pur  \n ";
            strQry1 += " from PUR_VS_SupplierInfo a,PUR_VS_ListSubjectInfoSup b \n ";
            strQry1 += " where a.vs_id=N'" + Current_VS.Vs_id + "'  \n ";
            strQry1 += " and a.supplier_name=N'" + Current_VS.Supplier_1 + "' \n ";
            strQry1 += " and a.vs_stt=b.vs_stt \n ";
            strQry1 += " order by a.vs_stt \n ";
            DataTable dt1 = conn.ExcuteDataTable(strQry1);
            Info_vendor1 = new List<PUR_VS_SupplierInfo_Entity>();
            foreach (DataRow row in dt1.Rows)
            {
                PUR_VS_SupplierInfo_Entity item = new PUR_VS_SupplierInfo_Entity();
                item.Vs_id = row["vs_id"].ToString();
                item.Supplier_name = row["supplier_name"].ToString();
                item.Vs_stt = float.Parse(row["vs_stt"].ToString());
                item.Vs_subject = row["vs_subject"].ToString();
                item.Vs_des = row["vs_des"].ToString();
                if (type=="New")
                {
                    item.Vs_des_status = row["vs_des_status"].ToString();
                    item.Vs_score_status = row["vs_score_status"].ToString();
                }
                else
                {
                    if (Current_VS.Vs_status=="Draft")
                    {
                        Current_VS.Vs_status = "Pending requester";
                    }
                    switch (Current_VS.Vs_status)
                    {
                        case "Pending requester":
                            item.Vs_des_status = row["vs_des_status"].ToString();
                            item.Vs_score_status = row["vs_score_status"].ToString();
                            break;
                        case "Pending purchaser":
                            item.Vs_des_status = row["vs_des_pur"].ToString();
                            item.Vs_score_status = "Read Only";
                            break;
                        default:
                            item.Vs_des_status = "Read Only";
                            item.Vs_score_status = "Read Only";
                            break;
                    }
                }
                item.Vs_score = float.Parse(row["vs_score"].ToString());
                Info_vendor1.Add(item);
            }
            string strQry2 = "select a.*,b.vs_des_status,b.vs_score_status,b.vs_des_pur  \n ";
            strQry2 += " from PUR_VS_SupplierInfo a,PUR_VS_ListSubjectInfoSup b \n ";
            strQry2 += " where a.vs_id=N'" + Current_VS.Vs_id + "'  \n ";
            strQry2 += " and a.supplier_name=N'" + Current_VS.Supplier_2 + "' \n ";
            strQry2 += " and a.vs_stt=b.vs_stt \n ";
            strQry2 += " order by a.vs_stt \n ";
            DataTable dt2 = conn.ExcuteDataTable(strQry2);
            Info_vendor2 = new List<PUR_VS_SupplierInfo_Entity>();
            foreach (DataRow row in dt2.Rows)
            {
                PUR_VS_SupplierInfo_Entity item = new PUR_VS_SupplierInfo_Entity();
                item.Vs_id = row["vs_id"].ToString();
                item.Supplier_name = row["supplier_name"].ToString();
                item.Vs_stt = float.Parse(row["vs_stt"].ToString());
                item.Vs_subject = row["vs_subject"].ToString();
                item.Vs_des = row["vs_des"].ToString();
                if (type == "New")
                {
                    item.Vs_des_status = row["vs_des_status"].ToString();
                    item.Vs_score_status = row["vs_score_status"].ToString();
                }
                else
                {
                    switch (Current_VS.Vs_status)
                    {
                        case "Pending requester":
                            item.Vs_des_status = row["vs_des_status"].ToString();
                            item.Vs_score_status = row["vs_score_status"].ToString();
                            break;
                        case "Pending purchaser":
                            item.Vs_des_status = row["vs_des_pur"].ToString();
                            item.Vs_score_status = "Read Only";
                            break;
                        default:
                            item.Vs_des_status = "Read Only";
                            item.Vs_score_status = "Read Only";
                            break;
                    }
                }
                item.Vs_score = float.Parse(row["vs_score"].ToString());
                Info_vendor2.Add(item);
            }
            string strQry3 = "select a.*,b.vs_des_status,b.vs_score_status,b.vs_des_pur  \n ";
            strQry3 += " from PUR_VS_SupplierInfo a,PUR_VS_ListSubjectInfoSup b \n ";
            strQry3 += " where a.vs_id=N'" + Current_VS.Vs_id + "'  \n ";
            strQry3 += " and a.supplier_name=N'" + Current_VS.Supplier_3 + "' \n ";
            strQry3 += " and a.vs_stt=b.vs_stt \n ";
            strQry3 += " order by a.vs_stt \n ";
            DataTable dt3 = conn.ExcuteDataTable(strQry3);
            Info_vendor3 = new List<PUR_VS_SupplierInfo_Entity>();
            foreach (DataRow row in dt3.Rows)
            {
                PUR_VS_SupplierInfo_Entity item = new PUR_VS_SupplierInfo_Entity();
                item.Vs_id = row["vs_id"].ToString();
                item.Supplier_name = row["supplier_name"].ToString();
                item.Vs_stt = float.Parse(row["vs_stt"].ToString());
                item.Vs_subject = row["vs_subject"].ToString();
                item.Vs_des = row["vs_des"].ToString();
                if (type == "New")
                {
                    item.Vs_des_status = row["vs_des_status"].ToString();
                    item.Vs_score_status = row["vs_score_status"].ToString();
                }
                else
                {
                    switch (Current_VS.Vs_status)
                    {
                        case "Pending requester":
                            item.Vs_des_status = row["vs_des_status"].ToString();
                            item.Vs_score_status = row["vs_score_status"].ToString();
                            break;
                        case "Pending purchaser":
                            item.Vs_des_status = row["vs_des_pur"].ToString();
                            item.Vs_score_status = "Read Only";
                            break;
                        default:
                            item.Vs_des_status = "Read Only";
                            item.Vs_score_status = "Read Only";
                            break;
                    }
                }
                item.Vs_score = float.Parse(row["vs_score"].ToString());
                Info_vendor3.Add(item);
            }
            dgvVendor1.DataSource = Info_vendor1.ToList();
            dgvVendor2.DataSource = Info_vendor2.ToList();
            dgvVendor3.DataSource = Info_vendor3.ToList();
            txtSupplier1.Text = Current_VS.Supplier_1;
            txtSupplier2.Text = Current_VS.Supplier_2;
            txtSupplier3.Text = Current_VS.Supplier_3;
            cboVendorType1.Text = Current_VS.Supplier_1_type;
            cboVendorType2.Text = Current_VS.Supplier_2_type;
            cboVendorType3.Text = Current_VS.Supplier_3_type;
            layoutControlItem6.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            layoutControlItem8.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            layoutControlItem9.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            attached1 = Current_VS.Supplier_1_att;
            attached2 = Current_VS.Supplier_2_att;
            attached3 = Current_VS.Supplier_3_att;
            switch (Current_VS.Selected_supplier)
            {
                case "SUPPLIER 1":
                    selected_vendor_type = Current_VS.Supplier_1_type;
                    selected_vendor = Info_vendor1;
                    break;
                case "SUPPLIER 2":
                    selected_vendor_type = Current_VS.Supplier_2_type;
                    selected_vendor = Info_vendor2;
                    break;
                case "SUPPLIER 3":
                    selected_vendor_type = Current_VS.Supplier_3_type;
                    selected_vendor = Info_vendor3;
                    break;
                default:
                    selected_vendor_type = Current_VS.Supplier_1_type;
                    selected_vendor = Info_vendor1;
                    break;
            }
            cboSelectedSupplier.Text = Current_VS.Selected_supplier;
        }
        private void Load_PIC()
        {
            txtRequester.Text = General_Infor.myaccount.Username;
            txtRequestDate.Text = DateTime.Now.ToString("dd/MM/yyyy HH:ss");
            txtRequesterStatus.Text = "Pending";
            txtDeptMgr.Text = General_Infor.myaccount.Direct_manager;
            string strQry2 = "select *  \n ";
            strQry2 += " from ADM_PersonInChargeOfProcess \n ";
            strQry2 += " where [procedure_name]=N'VS approval' \n ";
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
                        txtFinMgr.Text = item["pic_user"].ToString();
                        finance_mgr_email = item["email_address"].ToString();
                        break;
                    case "3":
                        txtPurMgr.Text = item["pic_user"].ToString();
                        pur_mgr_email = item["email_address"].ToString();
                        break;
                    case "4":
                        txtPlantMgr.Text = item["pic_user"].ToString();
                        plant_mgr_email = item["email_address"].ToString();
                        break;
                    case "5":
                        china_pur_mgr = item["pic_user"].ToString();
                        china_pur_mgr_email = item["email_address"].ToString();
                        break;
                    default:
                        break;
                }
            }
        }
        private void Load_VSNo()
        {
            string strQry = "select max(RIGHT((vs_id),3)) from PUR_VS where cast(vs_date as date)=N'" + DateTime.Today.ToString("yyyy-MM-dd") + "'";
            conn = new CmCn();
            string number = conn.ExcuteString(strQry);
            if (number == "")
            {
                txtVsNo.Text = "VS-" + DateTime.Today.ToString("yyMMdd") + "001";
            }
            else
            {
                int stt = int.Parse(number) + 1;
                if (stt<10)
                {
                    txtVsNo.Text = "VS-" + DateTime.Today.ToString("yyMMdd") + "00" + stt.ToString();
                }
                else if (stt < 100)
                {
                    txtVsNo.Text = "VS-" + DateTime.Today.ToString("yyMMdd") + "0" + stt.ToString();
                }
                else
                {
                    txtVsNo.Text = "VS-" + DateTime.Today.ToString("yyMMdd") + stt.ToString();
                }
            }
            string strQry2 = "insert into PUR_VS (vs_id,vs_requester,vs_date,vs_status,current_pic,dept) \n";
            strQry2 += "select N'"+txtVsNo.Text+ "',N'" + General_Infor.username + "',getdate(),N'Draft',N'" + General_Infor.username + "',N'" + General_Infor.myaccount.Department + "'\n";
            conn.ExcuteQry(strQry2);
        }
        private bool check_suplier_info(List<PUR_VS_SupplierInfo_Entity> list)
        {
            bool result = true;
            foreach (PUR_VS_SupplierInfo_Entity item in list)
            {
                if (item.Vs_des_status == "Mandatory" || item.Vs_des_status == "False")
                {
                    if (string.IsNullOrEmpty(item.Vs_des))
                    {
                        item.Vs_des_status = "False";
                    }
                    else
                    {

                        switch (item.Vs_stt)
                        {
                            case 1:
                                if (isNumber(item.Vs_des))
                                {
                                    item.Vs_des_status = "Mandatory";
                                }
                                else
                                {
                                    item.Vs_des_status = "False";
                                    item.Vs_des = "Input number";
                                }
                                break;
                            case 2:
                                if (isNumber(item.Vs_des))
                                {
                                    item.Vs_des_status = "Mandatory";
                                }
                                else
                                {
                                    item.Vs_des_status = "False";
                                    item.Vs_des = "Input number";
                                }
                                break;
                            case 8:
                                if (item.Vs_des == "VND" || item.Vs_des == "USD" || item.Vs_des == "EUR")
                                {
                                    item.Vs_des_status = "Mandatory";
                                }
                                else
                                {
                                    item.Vs_des_status = "False";
                                    item.Vs_des = "USD/VND/EUR";
                                }
                                break;
                            case 9:
                                var check_Invoice_PN = list_pur_unit.Where(c => c == item.Vs_des);
                                if (check_Invoice_PN.Count() > 0)
                                {
                                    item.Vs_des_status = "Mandatory";
                                }
                                else
                                {
                                    item.Vs_des_status = "False";
                                    item.Vs_des = "Check list accepted unit";
                                }
                                break;
                            case 10:
                                if (isNumber(item.Vs_des))
                                {
                                    item.Vs_des_status = "Mandatory";
                                }
                                else
                                {
                                    item.Vs_des_status = "False";
                                    item.Vs_des = "Input number";
                                }
                                break;
                            case 11:
                                if (isNumber(item.Vs_des))
                                {
                                    item.Vs_des_status = "Mandatory";
                                }
                                else
                                {
                                    item.Vs_des_status = "False";
                                    item.Vs_des = "Input number";
                                }
                                break;
                            case 12:
                                if (isDate(item.Vs_des))
                                {
                                    item.Vs_des_status = "Mandatory";
                                }
                                else
                                {
                                    item.Vs_des_status = "False";
                                    item.Vs_des = "Input date";
                                }
                                break;
                            case 13:
                                if (isDate(item.Vs_des))
                                {
                                    item.Vs_des_status = "Mandatory";
                                }
                                else
                                {
                                    item.Vs_des_status = "False";
                                    item.Vs_des = "Input date";
                                }
                                break;
                            case 14:
                                if (isPercent(item.Vs_des))
                                {
                                    item.Vs_des_status = "Mandatory";
                                }
                                else
                                {
                                    item.Vs_des_status = "False";
                                    item.Vs_des = "%";
                                }
                                break;
                            case 30:
                                if (isNumber(item.Vs_des))
                                {
                                    item.Vs_des_status = "Mandatory";
                                }
                                else
                                {
                                    item.Vs_des_status = "False";
                                    item.Vs_des = "Input number";
                                }
                                break;
                            case 32:
                                if (isNumber(item.Vs_des))
                                {
                                    item.Vs_des_status = "Mandatory";
                                }
                                else
                                {
                                    item.Vs_des_status = "False";
                                    item.Vs_des = "Input number";
                                }
                                break;
                            case 37:
                                if (isPercent(item.Vs_des))
                                {
                                    item.Vs_des_status = "Mandatory";
                                }
                                else
                                {
                                    item.Vs_des_status = "False";
                                    item.Vs_des = "%";
                                }
                                break;
                            default:
                                item.Vs_des_status = "";
                                break;
                        }
                    }
                }
                if (item.Vs_score_status == "Mandatory" || item.Vs_score_status == "False")
                {
                    if (item.Vs_score == 0)
                    {
                        item.Vs_score_status = "False";
                        result = false;
                    }
                    else
                    {
                        item.Vs_score_status = "Mandatory";
                    }
                }
                if (item.Vs_des_status == "False")
                {
                    result = false;
                }
            }
            return result;
        }
        private bool isNumber(string number)
        {
            float retNum;
            bool isNum = float.TryParse(number, out retNum);
            return isNum;
        }
        private bool isPercent(string number)
        {
            float retPercent;
            bool isNum;
            if (number.Length>=2)
            {
                isNum = float.TryParse(number.Substring(0, number.Length - 1), out retPercent);
                if (isNum)
                {
                    if (retPercent < 101)
                    {
                        isNum = true;
                    }
                    else
                    {
                        isNum = false;
                    }
                }
            }
            else
            {
                isNum = false;
            }
            return isNum;
        }
        private bool isDate(string date)
        {
            DateTime retNum;
            bool isNum = DateTime.TryParse(date, out retNum);
            return isNum;
        }
        private void Load_supplier_list()
        {
            string strQry = "Select supplier_name as [Supplier],sup_shortname as [Short name] from PUR_MasterListSupplier where supplier_status=N'Active'";
            conn = new CmCn();
            DataTable dt = conn.ExcuteDataTable(strQry);
            cboSupplier1.Properties.DataSource = dt;
            cboSupplier1.Properties.ValueMember = "Supplier";
            cboSupplier1.Properties.DisplayMember = "Supplier";
            DataTable dt2 = conn.ExcuteDataTable(strQry);
            cboSupplier2.Properties.DataSource = dt;
            cboSupplier2.Properties.ValueMember = "Supplier";
            cboSupplier2.Properties.DisplayMember = "Supplier";
            DataTable dt3 = conn.ExcuteDataTable(strQry);
            cboSupplier3.Properties.DataSource = dt;
            cboSupplier3.Properties.ValueMember = "Supplier";
            cboSupplier3.Properties.DisplayMember = "Supplier";
        }
        private void Load_vendor_field()
        {
            Info_vendor1 = new List<PUR_VS_SupplierInfo_Entity>();
            Info_vendor2 = new List<PUR_VS_SupplierInfo_Entity>();
            Info_vendor3 = new List<PUR_VS_SupplierInfo_Entity>();
            conn = new CmCn();
            string strQry = "select * from PUR_VS_ListSubjectInfoSup order by vs_stt";
            DataTable dt = conn.ExcuteDataTable(strQry);
            foreach (DataRow row in dt.Rows)
            {
                PUR_VS_SupplierInfo_Entity item = new PUR_VS_SupplierInfo_Entity();
                item.Vs_stt = string.IsNullOrEmpty(row["vs_stt"].ToString()) ? 0 : float.Parse(row["vs_stt"].ToString());
                item.Vs_id = txtVsNo.Text;
                item.Supplier_name = "";
                item.Vs_des = row["vs_des_hint"].ToString();
                item.Vs_subject = row["vs_subject"].ToString();
                item.Vs_des_status = row["vs_des_status"].ToString();
                item.Vs_score_status = row["vs_score_status"].ToString();
                item.Vs_score = 0;
                Info_vendor1.Add(item);
            }
            foreach (DataRow row in dt.Rows)
            {
                PUR_VS_SupplierInfo_Entity item = new PUR_VS_SupplierInfo_Entity();
                item.Vs_stt = string.IsNullOrEmpty(row["vs_stt"].ToString()) ? 0 : float.Parse(row["vs_stt"].ToString());
                item.Vs_id = txtVsNo.Text;
                item.Supplier_name = "";
                item.Vs_des = row["vs_des_hint"].ToString();
                item.Vs_subject = row["vs_subject"].ToString();
                item.Vs_des_status = row["vs_des_status"].ToString();
                item.Vs_score_status = row["vs_score_status"].ToString();
                item.Vs_score = 0;
                Info_vendor2.Add(item);
            }
            foreach (DataRow row in dt.Rows)
            {
                PUR_VS_SupplierInfo_Entity item = new PUR_VS_SupplierInfo_Entity();
                item.Vs_stt = string.IsNullOrEmpty(row["vs_stt"].ToString()) ? 0 : float.Parse(row["vs_stt"].ToString());
                item.Vs_id = txtVsNo.Text;
                item.Supplier_name = "";
                item.Vs_des = row["vs_des_hint"].ToString();
                item.Vs_subject = row["vs_subject"].ToString();
                item.Vs_des_status = row["vs_des_status"].ToString();
                item.Vs_score_status = row["vs_score_status"].ToString();
                item.Vs_score = 0;
                Info_vendor3.Add(item);
            }
            dgvVendor1.DataSource = Info_vendor1.ToList();
            dgvVendor2.DataSource = Info_vendor2.ToList();
            dgvVendor3.DataSource = Info_vendor3.ToList();
        }

        private void txtSupplier1_TextChanged(object sender, EventArgs e)
        {
            gvVendor1.OptionsBehavior.Editable = true;
        }

        private void dgvVendor1_Click(object sender, EventArgs e)
        {

        }

        private void cboVendorType1_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (cboVendorType1.Text == "New Supplier")
            {
                layoutControlItem6.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                layoutControlItem12.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
            }
            else
            {
                layoutControlItem6.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                layoutControlItem12.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            }
            Info_vendor1 = new List<PUR_VS_SupplierInfo_Entity>();
            string strQry = "select * from PUR_VS_ListSubjectInfoSup order by vs_stt";
            DataTable dt = conn.ExcuteDataTable(strQry);
            foreach (DataRow row in dt.Rows)
            {
                PUR_VS_SupplierInfo_Entity item = new PUR_VS_SupplierInfo_Entity();
                item.Vs_stt = string.IsNullOrEmpty(row["vs_stt"].ToString()) ? 0 : float.Parse(row["vs_stt"].ToString());
                item.Vs_id = txtVsNo.Text;
                item.Supplier_name = "";
                item.Vs_des = row["vs_des_hint"].ToString();
                item.Vs_subject = row["vs_subject"].ToString();
                item.Vs_des_status = row["vs_des_status"].ToString();
                item.Vs_score_status = row["vs_score_status"].ToString();
                item.Vs_score = 0;
                Info_vendor1.Add(item);
            }
            dgvVendor1.DataSource = Info_vendor1.ToList();
        }

        private void cboVendorType2_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (cboVendorType2.Text == "New Supplier")
            {
                layoutControlItem8.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                layoutControlItem44.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
            }
            else
            {
                layoutControlItem8.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                layoutControlItem44.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            }
            Info_vendor2 = new List<PUR_VS_SupplierInfo_Entity>();
            string strQry = "select * from PUR_VS_ListSubjectInfoSup order by vs_stt";
            DataTable dt = conn.ExcuteDataTable(strQry);
            foreach (DataRow row in dt.Rows)
            {
                PUR_VS_SupplierInfo_Entity item = new PUR_VS_SupplierInfo_Entity();
                item.Vs_stt = string.IsNullOrEmpty(row["vs_stt"].ToString()) ? 0 : float.Parse(row["vs_stt"].ToString());
                item.Vs_id = txtVsNo.Text;
                item.Supplier_name = "";
                item.Vs_des = row["vs_des_hint"].ToString();
                item.Vs_subject = row["vs_subject"].ToString();
                item.Vs_des_status = row["vs_des_status"].ToString();
                item.Vs_score_status = row["vs_score_status"].ToString();
                item.Vs_score = 0;
                Info_vendor2.Add(item);
            }
            dgvVendor2.DataSource = Info_vendor2.ToList();
        }

        private void cboVendorType3_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (cboVendorType3.Text == "New Supplier")
            {
                layoutControlItem9.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                layoutControlItem45.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
            }
            else
            {
                layoutControlItem9.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                layoutControlItem45.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            }
            Info_vendor3 = new List<PUR_VS_SupplierInfo_Entity>();
            string strQry = "select * from PUR_VS_ListSubjectInfoSup order by vs_stt";
            DataTable dt = conn.ExcuteDataTable(strQry);
            foreach (DataRow row in dt.Rows)
            {
                PUR_VS_SupplierInfo_Entity item = new PUR_VS_SupplierInfo_Entity();
                item.Vs_stt = string.IsNullOrEmpty(row["vs_stt"].ToString()) ? 0 : float.Parse(row["vs_stt"].ToString());
                item.Vs_id = txtVsNo.Text;
                item.Supplier_name = "";
                item.Vs_des = row["vs_des_hint"].ToString();
                item.Vs_subject = row["vs_subject"].ToString();
                item.Vs_des_status = row["vs_des_status"].ToString();
                item.Vs_score_status = row["vs_score_status"].ToString();
                item.Vs_score = 0;
                Info_vendor3.Add(item);
            }
            dgvVendor3.DataSource = Info_vendor3.ToList();
        }

        private void cboSupplier1_EditValueChanged(object sender, EventArgs e)
        {
            if (cboSupplier1.Text!="")
            {
                txtSupplier1.Text = cboSupplier1.Text;
                string strQry = "select * from PUR_MasterListSupplier where supplier_name=N'" + txtSupplier1.Text + "'";
                conn = new CmCn();
                DataTable dt = conn.ExcuteDataTable(strQry);
                if (dt.Rows.Count > 0)
                {
                    Auto_insert_sup_info("Short name of supplier", dt.Rows[0]["sup_shortname"].ToString(), Info_vendor1);
                    Auto_insert_sup_info("Supplier Address:", dt.Rows[0]["sup_address"].ToString(), Info_vendor1);
                    Auto_insert_sup_info("PIC phone no.", dt.Rows[0]["sup_tel"].ToString(), Info_vendor1);
                    Auto_insert_sup_info("PIC name", dt.Rows[0]["contact_pic"].ToString(), Info_vendor1);
                    Auto_insert_sup_info("PIC email", dt.Rows[0]["email_address"].ToString(), Info_vendor1);
                    Auto_insert_sup_info("Currency", dt.Rows[0]["sup_currency"].ToString(), Info_vendor1);
                    Auto_insert_sup_info("Payment term", dt.Rows[0]["payment_term"].ToString(), Info_vendor1);
                    Auto_insert_sup_info("Standard Transportation mode", dt.Rows[0]["delivery_mode"].ToString(), Info_vendor1);
                    Auto_insert_sup_info("Incoterms", dt.Rows[0]["incoterm"].ToString(), Info_vendor1);
                    Auto_insert_sup_info("TYPE OF COMPANY (PTY / CC)", dt.Rows[0]["type_company"].ToString(), Info_vendor1);
                    Auto_insert_sup_info("Country", dt.Rows[0]["sup_country"].ToString(), Info_vendor1);
                    Auto_insert_sup_info("WEBSITE:", dt.Rows[0]["sup_website"].ToString(), Info_vendor1);
                    Auto_insert_sup_info("POSTAL CODE:", dt.Rows[0]["sup_postal_code"].ToString(), Info_vendor1);
                    Auto_insert_sup_info("Supplier QMS", dt.Rows[0]["sup_qms"].ToString(), Info_vendor1);
                    Auto_insert_sup_info("Standard Manufacturing leadtime", dt.Rows[0]["std_manufact_leadtime"].ToString(), Info_vendor1);
                    Auto_insert_sup_info("Standard Transportation Leadtime", dt.Rows[0]["std_transport_leadtime"].ToString(), Info_vendor1);
                    Auto_insert_sup_info("Premium Transportation Leadtime", dt.Rows[0]["pre_transport_leadtime"].ToString(), Info_vendor1);
                    Auto_insert_sup_info("Premium Transportation mode", dt.Rows[0]["pre_transport_mode"].ToString(), Info_vendor1);
                    dgvVendor1.DataSource = Info_vendor1.ToList();
                }
            }
        }
        private void Auto_insert_sup_info(string row_name,string row_data, List<PUR_VS_SupplierInfo_Entity> List_data)
        {
            var find = List_data.FirstOrDefault(x => x.Vs_subject == row_name);
            if (find!=null)
            {
                find.Vs_des = row_data;
                if (!string.IsNullOrEmpty(row_data))
                {
                    find.Vs_des_status = "Read Only";
                }
                else
                {
                    find.Vs_des_status = "Mandatory";
                }
            }
        }
        private void Auto_insert_sup_info_2(string row_name, string row_data, List<PUR_VS_SupplierInfo_Entity> List_data)
        {
            var find = List_data.FirstOrDefault(x => x.Vs_subject == row_name);
            if (find != null)
            {
                find.Vs_des = row_data;
            }
        }
        private void Auto_insert_sup_info_3(string row_name, string row_data, List<PUR_VS_SupplierInfo_Entity> List_data)
        {
            var find = List_data.FirstOrDefault(x => x.Vs_subject == row_name);
            if (find != null)
            {
                if (row_data== "Read Only")
                {
                    find.Vs_score = 0;
                    find.Vs_score_status = row_data;
                }
                else
                {
                    find.Vs_score_status = row_data;
                }
            }
        }
        private bool is_warning_Sup_no = false;
        private bool General_condition()
        {
            string Error = "";
            if (cboSelectedSupplier.Text != "")
            {
                if (type=="New"|| type == "Edit" || type == "Copy")
                {
                    int count_sup = 0;
                    if (txtSupplier1.Text != "")
                    {
                        count_sup = 1;
                    }
                    if (txtSupplier1.Text != "" && txtSupplier2.Text != "")
                    {
                        count_sup = 2;
                    }
                    if (txtSupplier1.Text != "" && txtSupplier2.Text != "" && txtSupplier3.Text != "")
                    {
                        count_sup = 3;
                    }
                    if (nmEstimatedCost.Value >= 30000000)
                    {
                        if (count_sup < 3)
                        {
                            if (!is_warning_Sup_no)
                            {
                                Error += "Following the estimated yearly purchase amount, 3 suppliers must be fulfilled.\n In case of exceptional situation, the justification must be fulfilled in comment section";
                                is_warning_Sup_no = true;
                            }
                        }
                    }
                    else
                    {
                        if (count_sup < 2)
                        {
                            if (!is_warning_Sup_no)
                            {
                                Error += "Following the estimated yearly purchase amount, 2 suppliers must be fulfilled.\n In case of exceptional situation, the justification must be fulfilled in comment section";
                                is_warning_Sup_no = true;
                            }
                        }
                    }
                }
            }
            else
            {
                Error += "Not select supplier yet \n";
            }
            if (Error == "")
            {
                if (is_warning_Sup_no && txtComment.Text == "")
                {
                    string x = "2";
                    if (nmEstimatedCost.Value >= 30000000)
                    {
                        x = "3";
                    }
                    Error += "Following the estimated yearly purchase amount, " + x + " suppliers must be fulfilled.\n " +
                    "In case of exceptional situation, the justification must be fulfilled in comment section\n";
                }
                if (txtSupplier1.Text != "")
                {
                    if (!check_suplier_info(Info_vendor1))
                    {
                        Error += "Please check red line in the table for supplier 1\n";
                    }
                    if (attached1 == "")
                    {
                        Error += "There is no attached for supplier 1\n";
                    }
                }
                if (txtSupplier2.Text != "")
                {
                    if (!check_suplier_info(Info_vendor2))
                    {
                        Error += "*Please check red line in the table for supplier 2\n";
                    }
                    if (attached2 == "")
                    {
                        Error += "There is no attached for supplier 2\n";
                    }
                }
                if (txtSupplier3.Text != "")
                {
                    if (!check_suplier_info(Info_vendor3))
                    {
                        Error += "*Please check red line in the table for supplier 3\n";
                    }
                    if (attached3 == "")
                    {
                        Error += "There is no attached for supplier 3\n";
                    }
                }
            }
            if (Error == "")
            {
                return true;
            }
            else
            {
                dgvVendor1.DataSource = Info_vendor1.ToList();
                dgvVendor2.DataSource = Info_vendor2.ToList();
                dgvVendor3.DataSource = Info_vendor3.ToList();
                MessageBox.Show(Error, "ERROR");
                return false;
            }
        }

        private void btnAttach1_Click(object sender, EventArgs e)
        {
            OpenFileDialog OpenFile = new OpenFileDialog();
            OpenFile.Title = "Mở tệp tin";
            OpenFile.Filter = "PDF (.pdf)|*.pdf";
            if (OpenFile.ShowDialog() != DialogResult.Cancel)
            {
                string des = @"\\172.16.180.20\05.it\16.HVN\ATTACHMENT\PURCHASE\VS\" + txtVsNo.Text+"_01.pdf";
                string source = OpenFile.FileName;
                try
                {
                    File.Copy(source, des, true);
                    attached1 = des;
                }
                catch (Exception)
                {
                    MessageBox.Show("The attached file is opened. Please close then attach again");
                }
            }
        }

        private void btnAttach2_Click(object sender, EventArgs e)
        {
            OpenFileDialog OpenFile = new OpenFileDialog();
            OpenFile.Title = "Mở tệp tin";
            OpenFile.Filter = "PDF (.pdf)|*.pdf";
            if (OpenFile.ShowDialog() != DialogResult.Cancel)
            {
                string des = @"\\172.16.180.20\05.it\16.HVN\ATTACHMENT\PURCHASE\VS\" + txtVsNo.Text + "_02.pdf";
                string source = OpenFile.FileName;
                try
                {
                    File.Copy(source, des, true);
                    attached2 = des;
                }
                catch (Exception)
                {
                    MessageBox.Show("The attached file is opened. Please close then attach again");
                }
            }
        }

        private void btnAttach3_Click(object sender, EventArgs e)
        {
            OpenFileDialog OpenFile = new OpenFileDialog();
            OpenFile.Title = "Mở tệp tin";
            OpenFile.Filter = "PDF (.pdf)|*.pdf";
            if (OpenFile.ShowDialog() != DialogResult.Cancel)
            {
                string des = @"\\172.16.180.20\05.it\16.HVN\ATTACHMENT\PURCHASE\VS\" + txtVsNo.Text + "_03.pdf";
                string source = OpenFile.FileName;
                try
                {
                    File.Copy(source, des, true);
                    attached3 = des;
                }
                catch (Exception)
                {
                    MessageBox.Show("The attached file is opened. Please close then attach again");
                }
            }
        }

        private void btnViewAttach1_Click(object sender, EventArgs e)
        {
            if (attached1 != "")
            {
                System.Diagnostics.Process.Start(attached1);
            }
            else
            {
                MessageBox.Show("There is no attachment");
            }
        }

        private void btnViewAttach2_Click(object sender, EventArgs e)
        {
            if (attached2 != "")
            {
                System.Diagnostics.Process.Start(attached2);
            }
            else
            {
                MessageBox.Show("There is no attachment");
            }
        }

        private void btnViewAttach3_Click(object sender, EventArgs e)
        {
            if (attached3 != "")
            {
                System.Diagnostics.Process.Start(attached3);
            }
            else
            {
                MessageBox.Show("There is no attachment");
            }
        }
        private void nmEstimatedCost_ValueChanged(object sender, EventArgs e)
        {
            if (nmEstimatedCost.Value >= 30000000)
            {
                lbError.Text = "3 suppliers must be fulfilled";
            }
            else
            {
                lbError.Text = "At least 2 suppliers must be fulfilled";
            }
        }

        private void txtSupplier2_TextChanged(object sender, EventArgs e)
        {
            gvVendor2.OptionsBehavior.Editable = true;
        }

        private void txtSupplier3_TextChanged(object sender, EventArgs e)
        {
            gvVendor3.OptionsBehavior.Editable = true;
        }

        private void gvVendor1_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            GridView view = sender as GridView;
            if (e.Column.Caption == "Description")
            {
                string status = view.GetRowCellValue(e.RowHandle, view.Columns["Vs_des_status"]).ToString();
                switch (status)
                {
                    case "Mandatory":
                        e.Appearance.BackColor = Color.LightGreen;
                        break;
                    case "False":
                        e.Appearance.BackColor = Color.Red;
                        break;
                    case "Read Only":
                        e.Appearance.BackColor = Color.LightGray;
                        break;
                    default:
                        break;
                }
            }
            else if (e.Column.Caption == "Score")
            {
                string status = view.GetRowCellValue(e.RowHandle, view.Columns["Vs_score_status"]).ToString();
                switch (status)
                {
                    case "Mandatory":
                        e.Appearance.BackColor = Color.LightGreen;
                        break;
                    case "False":
                        e.Appearance.BackColor = Color.Red;
                        break;
                    case "Read Only":
                        e.Appearance.BackColor = Color.LightGray;
                        break;
                    default:
                        break;
                }
            }
        }

        private void gvVendor2_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            GridView view = sender as GridView;
            if (e.Column.Caption == "Description")
            {
                string status = view.GetRowCellValue(e.RowHandle, view.Columns["Vs_des_status"]).ToString();
                switch (status)
                {
                    case "Mandatory":
                        e.Appearance.BackColor = Color.LightGreen;
                        break;
                    case "False":
                        e.Appearance.BackColor = Color.Red;
                        break;
                    case "Read Only":
                        e.Appearance.BackColor = Color.LightGray;
                        break;
                    default:
                        break;
                }
            }
            else if (e.Column.Caption == "Score")
            {
                string status = view.GetRowCellValue(e.RowHandle, view.Columns["Vs_score_status"]).ToString();
                switch (status)
                {
                    case "Mandatory":
                        e.Appearance.BackColor = Color.LightGreen;
                        break;
                    case "False":
                        e.Appearance.BackColor = Color.Red;
                        break;
                    case "Read Only":
                        e.Appearance.BackColor = Color.LightGray;
                        break;
                    default:
                        break;
                }
            }
        }

        private void gvVendor3_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            GridView view = sender as GridView;
            if (e.Column.Caption == "Description")
            {
                string status = view.GetRowCellValue(e.RowHandle, view.Columns["Vs_des_status"]).ToString();
                switch (status)
                {
                    case "Mandatory":
                        e.Appearance.BackColor = Color.LightGreen;
                        break;
                    case "False":
                        e.Appearance.BackColor = Color.Red;
                        break;
                    case "Read Only":
                        e.Appearance.BackColor = Color.LightGray;
                        break;
                    default:
                        break;
                }
            }
            else if (e.Column.Caption == "Score")
            {
                string status = view.GetRowCellValue(e.RowHandle, view.Columns["Vs_score_status"]).ToString();
                switch (status)
                {
                    case "Mandatory":
                        e.Appearance.BackColor = Color.LightGreen;
                        break;
                    case "False":
                        e.Appearance.BackColor = Color.Red;
                        break;
                    case "Read Only":
                        e.Appearance.BackColor = Color.LightGray;
                        break;
                    default:
                        break;
                }
            }
        }

        private void btnApprove_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (XtraMessageBox.Show("Do you want to APPROVE the request?", "APPROVE NEW ITEM", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                string strQry = "";
                bool isClose = true;
                switch (Current_VS.Vs_status)
                {
                    case "Pending requester":
                        isClose = false;
                        break;
                    case "Pending department mgr":
                        strQry += " update PUR_VS set dept_mgr=N'" + General_Infor.username + "', dept_mgr_date=getdate(),dept_sign=N'" + General_Infor.myaccount.Signature + "', vs_status=N'Pending purchaser',current_pic=N'"+txtPur.Text+ "',dept_comment=N'" + txtCheckerComment.Text + "' \n ";
                        strQry += " where vs_id=N'" + Current_VS.Vs_id + "' \n ";
                        break;
                    case "Pending purchaser":
                        if (General_condition())
                        {
                            strQry += " update PUR_VS set pur=N'" + General_Infor.username + "', pur_date=getdate(),pur_sign=N'" + General_Infor.myaccount.Signature + "', vs_status=N'Pending finance mgr',current_pic=N'" + txtFinMgr.Text + "',pur_comment=N'" + txtCheckerComment.Text + "' \n ";
                            strQry += " where vs_id=N'" + Current_VS.Vs_id + "' \n ";
                            Pur_update_Infor();
                        }
                        else
                        {
                            isClose = false;
                        }
                        break;
                    case "Pending finance mgr":
                        strQry += " update PUR_VS set fin_mgr=N'" + General_Infor.username + "', fin_mgr_date=getdate(), fin_mgr_sign=N'" + General_Infor.myaccount.Signature + "',vs_status=N'Pending purchasing mgr',current_pic=N'" + txtPurMgr.Text + "',fin_mgr_comment=N'" + txtCheckerComment.Text + "' \n ";
                        strQry += " where vs_id=N'" + Current_VS.Vs_id + "' \n ";
                        break;
                    case "Pending purchasing mgr":
                        strQry += " update PUR_VS set pur_mgr=N'" + General_Infor.username + "', pur_mgr_date=getdate(), pur_mgr_sign=N'" + General_Infor.myaccount.Signature + "', vs_status=N'Pending plant mgr',current_pic=N'" + txtPlantMgr.Text + "',pur_mgr_comment=N'" + txtCheckerComment.Text + "' \n ";
                        strQry += " where vs_id=N'" + Current_VS.Vs_id + "' \n ";
                        break;
                    case "Pending plant mgr":
                        strQry += " update PUR_VS set plant_mgr=N'" + General_Infor.username + "', plant_mgr_date=getdate(), plant_mgr_sign=N'" + General_Infor.myaccount.Signature + "', vs_status=N'Fully approved',current_pic=N'',plant_mgr_comment=N'" + txtCheckerComment.Text + "' \n ";
                        strQry += " where vs_id=N'" + Current_VS.Vs_id + "' \n ";
                        Plant_mgr_update_info();
                        break;
                    default:
                        isClose = false;
                        break;
                }
                if (isClose)
                {
                    SplashScreenManager.ShowForm(this, typeof(frmWaitingForm), true, true, false);
                    SplashScreenManager.Default.SetWaitFormCaption("Please wait...");
                    try
                    {
                        string filepath = @"\\172.16.180.20\05.it\16.HVN\ATTACHMENT\PURCHASE\VS\" + Current_VS.Vs_id + ".pdf";
                        adoClass = new ADO();
                        adoClass.Print_PUR_VS_Detail(Current_VS, filepath, "export");
                        strQry += "\ninsert into [ADM_Inbox_HVN_System] ([subject],date_send,sender,body,is_process,kind) \n ";
                        strQry += "   select N'Re:[HVN-System]:[PUR]:[Regis Item]:" + txtVsNo.Text + ":Yes',getdate(),N'" + General_Infor.myaccount.Email_address + "', \n ";
                        strQry += "   N'" + txtCheckerComment.Text + "',N'No',N'System' \n ";
                        conn = new CmCn();
                        conn.ExcuteQry(strQry);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                   
                    SplashScreenManager.CloseForm();
                    this.Close();
                }
            }
        }
        private void Plant_mgr_update_info()
        {
            string item_name, erp_code, hut_code, item_unit, unit_price, unit_currency, unit_vat, supplier_name, moq, standard_packing;
            string sup_shortname, sup_address, sup_tel, tax_code, contact_pic, email_address, sup_currency, payment_term, delivery_mode, incoterm;
            string type_company, sup_country, sup_website, sup_postal_code, sup_qms, std_manufact_leadtime, std_transport_leadtime, pre_transport_leadtime, pre_transport_mode, volume_auto_business;
            item_name = Find_info("Item name", selected_vendor);
            erp_code = Find_info("ERP item code", selected_vendor);
            hut_code = Find_info("HVN code", selected_vendor);
            item_unit = Find_info("Unit", selected_vendor);
            unit_price = Find_info("EXW price", selected_vendor);
            unit_currency = Find_info("Currency", selected_vendor);
            unit_vat = Find_info("VAT", selected_vendor);
            supplier_name = selected_vendor[0].Supplier_name;
            moq = Find_info("MOQ", selected_vendor);
            sup_shortname = Find_info("Short name of supplier", selected_vendor);
            sup_address = Find_info("Supplier Address:", selected_vendor);
            sup_tel = Find_info("PIC phone no.", selected_vendor);
            tax_code = "";
            contact_pic = Find_info("PIC name", selected_vendor);
            email_address = Find_info("PIC email", selected_vendor);
            sup_currency = unit_currency;
            payment_term = Find_info("Payment term", selected_vendor);
            delivery_mode = Find_info("Standard Transportation mode", selected_vendor);
            incoterm = Find_info("Incoterms ", selected_vendor);
            standard_packing = Find_info("Unit of purchase", selected_vendor);
            type_company = Find_info("TYPE OF COMPANY (PTY / CC)", selected_vendor);
            sup_country = Find_info("Country", selected_vendor);
            sup_website = Find_info("WEBSITE:", selected_vendor);
            sup_postal_code = Find_info("POSTAL CODE:", selected_vendor);
            sup_qms = Find_info("Supplier QMS", selected_vendor);
            std_manufact_leadtime = Find_info("Standard Manufacturing leadtime", selected_vendor);
            std_transport_leadtime = Find_info("Standard Transportation Leadtime", selected_vendor);
            pre_transport_leadtime = Find_info("Premium Transportation Leadtime", selected_vendor);
            pre_transport_mode = Find_info("Premium Transportation mode", selected_vendor);
            volume_auto_business = Find_info("Volume of automotive business (%)", selected_vendor);

            string strQry = "";
            string strQry2 = "select item_name from PUR_MasterListItem where item_name=N'"+ item_name + "'";
            conn = new CmCn();
            if (conn.ExcuteString(strQry2)=="")
            {
                strQry =" insert into PUR_MasterListItem (item_name,erp_code,hut_code,item_unit, \n ";
                strQry += " unit_price,unit_currency,unit_vat, \n ";
                strQry += " supplier_name,moq,standard_packing,item_status,expired_date) \n ";
                strQry += " select N'" + item_name + "',N'" + erp_code + "',N'" + hut_code + "',N'" + item_unit + "' \n ";
                strQry += " ,N'" + unit_price + "',N'" + unit_currency + "',N'" + float.Parse(unit_vat.Substring(0, unit_vat.Length-1))/100 + "' \n ";
                strQry += " ,N'" + supplier_name + "',N'" + moq + "',N'" + standard_packing + "',N'Active',N'"+DateTime.Today.AddYears(2).ToString("yyyy-MM-dd")+"' \n ";
            }
            else
            {
                strQry = " update PUR_MasterListItem set unit_price=N'" + unit_price + "',unit_currency=N'" + unit_currency + "',item_unit=N'"+ item_unit + "'" +
                    ",unit_vat=N'" + float.Parse(unit_vat.Substring(0, unit_vat.Length - 1)) / 100 + "',supplier_name=N'" + supplier_name + "',moq=N'" + moq + "',standard_packing=N'" + standard_packing + "',expired_date=N'" + DateTime.Today.AddYears(2).ToString("yyyy-MM-dd") + "' \n";
                strQry += " where item_name=N'" + item_name + "' \n ";
            }
            string strQry5 = "select supplier_name from PUR_MasterListSupplier where supplier_name=N'" + supplier_name + "'";
            if (conn.ExcuteString(strQry5) == "")
            {
                strQry += " insert into PUR_MasterListSupplier (supplier_name,sup_shortname,sup_tel \n ";
                strQry += " ,tax_code,contact_pic,email_address,sup_currency \n ";
                strQry += " ,payment_term,delivery_mode,incoterm,supplier_status \n ";
                strQry += " ,type_company,sup_country,sup_website \n ";
                strQry += " ,sup_postal_code,sup_qms,std_manufact_leadtime,std_transport_leadtime \n ";
                strQry += " ,pre_transport_leadtime,pre_transport_mode,volume_auto_business) \n ";
                strQry += " select N'" + supplier_name + "',N'" + sup_shortname + "',N'" + sup_tel + "' \n ";
                strQry += " ,N'" + tax_code + "',N'" + contact_pic + "',N'" + email_address + "',N'" + sup_currency + "' \n ";
                strQry += " ,N'" + payment_term + "',N'" + delivery_mode + "',N'" + incoterm + "',N'Active' \n ";
                strQry += " ,N'" + type_company + "',N'" + sup_country + "',N'" + sup_website + "' \n ";
                strQry += " ,N'" + sup_postal_code + "',N'" + sup_qms + "',N'" + std_manufact_leadtime + "',N'" + std_transport_leadtime + "' \n ";
                strQry += " ,N'" + pre_transport_leadtime + "',N'" + pre_transport_mode + "',N'" + volume_auto_business + "' \n ";

            }
            else
            {
                strQry += "update PUR_MasterListSupplier set sup_shortname=N'" + sup_shortname + "',sup_address=N'" + sup_address + "', \n ";
                strQry += " sup_tel=N'" + sup_tel + "',tax_code=N'" + tax_code + "',contact_pic=N'" + contact_pic + "',email_address=N'" + email_address + "', \n ";
                strQry += " sup_currency=N'" + sup_currency + "',payment_term=N'" + payment_term + "',delivery_mode=N'" + delivery_mode + "', \n ";
                strQry += " incoterm=N'" + incoterm + "',supplier_status=N'Active' \n ";
                strQry += " ,type_company=N'" + type_company + "',sup_country=N'" + sup_country + "',sup_website=N'" + sup_website + "' \n ";
                strQry += " ,sup_postal_code=N'" + sup_postal_code + "',sup_qms=N'" + sup_qms + "',std_manufact_leadtime=N'" + std_manufact_leadtime + "',std_transport_leadtime=N'" + std_transport_leadtime + "' \n ";
                strQry += " ,pre_transport_leadtime=N'" + pre_transport_leadtime + "',pre_transport_mode=N'" 
                    + pre_transport_mode + "',volume_auto_business=N'" + float.Parse(volume_auto_business.Substring(0, volume_auto_business.Length - 1)) / 100 + "' \n ";
                strQry += " where supplier_name=N'"+supplier_name+"'\n";
            }
            string regis_content = "Price=" + unit_price + ",Currency=" + unit_currency + ",Unit=" + item_unit + "" +
                    ",VAT=" + unit_vat + ",Supplier=" + supplier_name + ",MOQ=" + moq + ",Standard packing=" + standard_packing + " \n";
            strQry += "insert into PUR_MasterListItem_History (item_name,i_transaction,i_content,i_note,pic,input_time) \n";
            strQry += "select N'" + Current_VS.Item_name + "',N'Regis vendor selection',N'" + regis_content + "',N'',N'" + Current_VS.Vs_requester + "',N'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'";
            conn.ExcuteQry(strQry);
            this.Close();
        }
        private string Find_info(string infor_name, List<PUR_VS_SupplierInfo_Entity> List_data)
        {
            string result = "";
            var find = List_data.FirstOrDefault(x => x.Vs_subject == infor_name);
            result = find.Vs_des;
            return result;
        }
        private void Pur_update_Infor()
        {
            string strQry = "";
            strQry += " delete from PUR_VS_SupplierInfo where vs_id=N'" + txtVsNo.Text + "' \n ";
            string qr1 = "";
            foreach (PUR_VS_SupplierInfo_Entity item in Info_vendor1)
            {
                if (qr1 == "")
                {
                    qr1 += "select N'" + txtVsNo.Text + "',N'" + txtSupplier1.Text + "',N'" + item.Vs_stt + "',N'" + item.Vs_subject + "',N'" + item.Vs_des + "',N'" + item.Vs_score + "'\n";
                }
                else
                {
                    qr1 += "union all select N'" + txtVsNo.Text + "',N'" + txtSupplier1.Text + "',N'" + item.Vs_stt + "',N'" + item.Vs_subject + "',N'" + item.Vs_des + "',N'" + item.Vs_score + "'\n";
                }
            }
            strQry += "insert into PUR_VS_SupplierInfo (vs_id,supplier_name,vs_stt,vs_subject,vs_des,vs_score)\n";
            strQry += qr1;
            if (txtSupplier2.Text != "")
            {
                string qr2 = "";
                foreach (PUR_VS_SupplierInfo_Entity item in Info_vendor2)
                {
                    if (qr2 == "")
                    {
                        qr2 += "select N'" + txtVsNo.Text + "',N'" + txtSupplier2.Text + "',N'" + item.Vs_stt + "',N'" + item.Vs_subject + "',N'" + item.Vs_des + "',N'" + item.Vs_score + "'\n";
                    }
                    else
                    {
                        qr2 += "union all select N'" + txtVsNo.Text + "',N'" + txtSupplier2.Text + "',N'" + item.Vs_stt + "',N'" + item.Vs_subject + "',N'" + item.Vs_des + "',N'" + item.Vs_score + "'\n";
                    }
                }
                strQry += "insert into PUR_VS_SupplierInfo (vs_id,supplier_name,vs_stt,vs_subject,vs_des,vs_score)\n";
                strQry += qr2;
            }
            if (txtSupplier3.Text != "")
            {
                string qr3 = "";
                foreach (PUR_VS_SupplierInfo_Entity item in Info_vendor3)
                {
                    if (qr3 == "")
                    {
                        qr3 += "select N'" + txtVsNo.Text + "',N'" + txtSupplier3.Text + "',N'" + item.Vs_stt + "',N'" + item.Vs_subject + "',N'" + item.Vs_des + "',N'" + item.Vs_score + "'\n";
                    }
                    else
                    {
                        qr3 += "union all select N'" + txtVsNo.Text + "',N'" + txtSupplier3.Text + "',N'" + item.Vs_stt + "',N'" + item.Vs_subject + "',N'" + item.Vs_des + "',N'" + item.Vs_score + "'\n";
                    }
                }
                strQry += "insert into PUR_VS_SupplierInfo (vs_id,supplier_name,vs_stt,vs_subject,vs_des,vs_score)\n";
                strQry += qr3;
            }
            conn = new CmCn();
            conn.ExcuteQry(strQry);
        }

        private void gvVendor1_ShowingEditor(object sender, CancelEventArgs e)
        {
            //e.Cancel = gvVendor1.FocusedColumn.FieldName == "Vs_des" && gvVendor1.GetRowCellValue(gvVendor1.FocusedRowHandle, "Vs_des_status").ToString() == "Read Only";
            //e.Cancel = gvVendor1.FocusedColumn.FieldName == "Vs_score" && gvVendor1.GetRowCellValue(gvVendor1.FocusedRowHandle, "Vs_score_status").ToString() == "Read Only";
            if (gvVendor1.FocusedColumn.FieldName == "Vs_des" && gvVendor1.GetRowCellValue(gvVendor1.FocusedRowHandle, "Vs_des_status").ToString() == "Read Only")
            {
                e.Cancel = true;
            }
            else if (gvVendor1.FocusedColumn.FieldName == "Vs_score" && gvVendor1.GetRowCellValue(gvVendor1.FocusedRowHandle, "Vs_score_status").ToString() == "Read Only")
            {
                e.Cancel = true;
            }
            else
            {
                e.Cancel = false;
            }
        }
        private void gvVendor2_ShowingEditor(object sender, CancelEventArgs e)
        {
            if (gvVendor2.FocusedColumn.FieldName == "Vs_des" && gvVendor2.GetRowCellValue(gvVendor2.FocusedRowHandle, "Vs_des_status").ToString() == "Read Only")
            {
                e.Cancel = true;
            }
            else if (gvVendor2.FocusedColumn.FieldName == "Vs_score" && gvVendor2.GetRowCellValue(gvVendor2.FocusedRowHandle, "Vs_score_status").ToString() == "Read Only")
            {
                e.Cancel = true;
            }
            else
            {
                e.Cancel = false;
            }
        }
        private void gvVendor3_ShowingEditor(object sender, CancelEventArgs e)
        {
            if (gvVendor3.FocusedColumn.FieldName == "Vs_des" && gvVendor3.GetRowCellValue(gvVendor3.FocusedRowHandle, "Vs_des_status").ToString() == "Read Only")
            {
                e.Cancel = true;
            }
            else if (gvVendor3.FocusedColumn.FieldName == "Vs_score" && gvVendor3.GetRowCellValue(gvVendor3.FocusedRowHandle, "Vs_score_status").ToString() == "Read Only")
            {
                e.Cancel = true;
            }
            else
            {
                e.Cancel = false;
            }
        }
        private void btnReject_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (XtraMessageBox.Show("Do you want to REJECT the request?", "REJECT NEW ITEM", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                string strQry = "";
                strQry += " update PUR_VS set vs_status=N'Pending requester',dept_sign=null,fin_mgr_sign=null,pur_mgr_sign=null,pur_sign=null,plant_mgr_sign=null,current_pic=N'"+Current_VS.Vs_requester+"' \n ";
                switch (Current_VS.Vs_status)
                {
                    case "Pending department mgr":
                        strQry += ",dept_comment=N'" + txtCheckerComment.Text + "' \n";
                        break;
                    case "Pending purchaser":
                        strQry += ",pur_comment=N'" + txtCheckerComment.Text + "' \n";
                        break;
                    case "Pending finance mgr":
                        strQry += ",fin_mgr_comment=N'" + txtCheckerComment.Text + "' \n";
                        break;
                    case "Pending purchasing mgr":
                        strQry += ",pur_mgr_comment=N'" + txtCheckerComment.Text + "' \n";
                        break;
                    case "Pending plant mgr":
                        strQry += ",plant_mgr_comment=N'" + txtCheckerComment.Text + "' \n";
                        break;
                    default:
                        break;
                }
                strQry += " where vs_id=N'" + Current_VS.Vs_id + "' \n ";
                strQry += "\ninsert into [ADM_Inbox_HVN_System] ([subject],date_send,sender,body,is_process,kind) \n ";
                strQry += "   select N'Re:[HVN-System]:[PUR]:[Regis Item]:" + txtVsNo.Text + ":No',getdate(),N'" + General_Infor.myaccount.Email_address + "', \n ";
                strQry += "   N'" + txtCheckerComment.Text + "',N'No',N'System' \n ";
                conn = new CmCn();
                conn.ExcuteQry(strQry);
                this.Close();
            }
        }

        private void btnSubmit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (XtraMessageBox.Show("Do you want to submit this request?", "Submit", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (General_condition())
                {
                    try
                    {
                        SplashScreenManager.ShowForm(this, typeof(frmWaitingForm), true, true, false);
                        SplashScreenManager.Default.SetWaitFormCaption("Please wait...");
                        if (selected_vendor_type== "New Supplier")
                        {
                            txtPurMgr.Text = china_pur_mgr;
                        }
                        string vat = selected_vendor[13].Vs_des.Substring(0, selected_vendor[13].Vs_des.Length - 1);
                        string strQry = "delete from PUR_VS where vs_id=N'" + txtVsNo.Text + "'  \n ";
                        strQry += " insert into PUR_VS \n ";
                        strQry += " (vs_id,vs_requester,vs_date,vs_des, \n ";
                        strQry += " item_name,item_unit,unit_price,unit_currency \n ";
                        strQry += " ,unit_vat,supplier_1,supplier_1_type,supplier_1_att \n ";
                        strQry += " ,supplier_2,supplier_2_type,supplier_2_att,selected_supplier \n ";
                        strQry += " ,supplier_3,supplier_3_type,supplier_3_att,vs_status \n ";
                        strQry += " ,dept_mgr,fin_mgr,pur_mgr,plant_mgr,estimate_yearly_amount \n ";
                        strQry += " ,pur,current_pic,vs_comment,dept) \n ";
                        strQry += "  select N'" + txtVsNo.Text + "',N'" + General_Infor.username + "',getdate(),N'" + txtDescription.Text + "', \n ";
                        strQry += "  N'" + selected_vendor[3].Vs_des + "',N'" + selected_vendor[8].Vs_des + "',N'" + selected_vendor[0].Vs_des + "',N'" + selected_vendor[7].Vs_des + "', \n ";
                        strQry += "  N'" + float.Parse(vat) / 100 + "',N'" + txtSupplier1.Text + "',N'" + cboVendorType1.Text + "',N'" + attached1 + "', \n ";
                        strQry += "  N'" + txtSupplier2.Text + "',N'" + cboVendorType2.Text + "',N'" + attached2 + "',N'" + cboSelectedSupplier.Text + "', \n ";
                        strQry += "  N'" + txtSupplier3.Text + "',N'" + cboVendorType3.Text + "',N'" + attached3 + "',N'Pending department mgr', \n ";
                        strQry += "  N'" + txtDeptMgr.Text + "',N'" + txtFinMgr.Text + "',N'" + txtPurMgr.Text + "',N'" + txtPlantMgr.Text + "',N'" + nmEstimatedCost.Value.ToString() + "', \n ";
                        strQry += "  N'" + txtPur.Text + "',N'" + txtDeptMgr.Text + "',N'" + txtComment.Text + "',N'" + General_Infor.myaccount.Department + "' \n ";
                        strQry += " delete from PUR_VS_SupplierInfo where vs_id=N'" + txtVsNo.Text + "' \n ";
                        string qr1 = "";
                        foreach (PUR_VS_SupplierInfo_Entity item in Info_vendor1)
                        {
                            if (qr1 == "")
                            {
                                qr1 += "select N'" + txtVsNo.Text + "',N'" + txtSupplier1.Text + "',N'" + item.Vs_stt + "',N'" + item.Vs_subject + "',N'" + item.Vs_des + "',N'" + item.Vs_score + "'\n";
                            }
                            else
                            {
                                qr1 += "union all select N'" + txtVsNo.Text + "',N'" + txtSupplier1.Text + "',N'" + item.Vs_stt + "',N'" + item.Vs_subject + "',N'" + item.Vs_des + "',N'" + item.Vs_score + "'\n";
                            }
                        }
                        strQry += "insert into PUR_VS_SupplierInfo (vs_id,supplier_name,vs_stt,vs_subject,vs_des,vs_score)\n";
                        strQry += qr1;
                        if (txtSupplier2.Text != "")
                        {
                            string qr2 = "";
                            foreach (PUR_VS_SupplierInfo_Entity item in Info_vendor2)
                            {
                                if (qr2 == "")
                                {
                                    qr2 += "select N'" + txtVsNo.Text + "',N'" + txtSupplier2.Text + "',N'" + item.Vs_stt + "',N'" + item.Vs_subject + "',N'" + item.Vs_des + "',N'" + item.Vs_score + "'\n";
                                }
                                else
                                {
                                    qr2 += "union all select N'" + txtVsNo.Text + "',N'" + txtSupplier2.Text + "',N'" + item.Vs_stt + "',N'" + item.Vs_subject + "',N'" + item.Vs_des + "',N'" + item.Vs_score + "'\n";
                                }
                            }
                            strQry += "insert into PUR_VS_SupplierInfo (vs_id,supplier_name,vs_stt,vs_subject,vs_des,vs_score)\n";
                            strQry += qr2;
                        }
                        if (txtSupplier3.Text != "")
                        {
                            string qr3 = "";
                            foreach (PUR_VS_SupplierInfo_Entity item in Info_vendor3)
                            {
                                if (qr3 == "")
                                {
                                    qr3 += "select N'" + txtVsNo.Text + "',N'" + txtSupplier3.Text + "',N'" + item.Vs_stt + "',N'" + item.Vs_subject + "',N'" + item.Vs_des + "',N'" + item.Vs_score + "'\n";
                                }
                                else
                                {
                                    qr3 += "union all select N'" + txtVsNo.Text + "',N'" + txtSupplier3.Text + "',N'" + item.Vs_stt + "',N'" + item.Vs_subject + "',N'" + item.Vs_des + "',N'" + item.Vs_score + "'\n";
                                }
                            }
                            strQry += "insert into PUR_VS_SupplierInfo (vs_id,supplier_name,vs_stt,vs_subject,vs_des,vs_score)\n";
                            strQry += qr3;
                        }
                        conn = new CmCn();
                        conn.ExcuteQry(strQry);
                        string filepath = @"\\172.16.180.20\05.it\16.HVN\ATTACHMENT\PURCHASE\VS\" + txtVsNo.Text + ".pdf";
                        Current_VS = new PUR_VS_Entity();
                        Current_VS.Vs_id = txtVsNo.Text;
                        Current_VS.Supplier_1 = txtSupplier1.Text;
                        Current_VS.Supplier_2= txtSupplier2.Text;
                        Current_VS.Supplier_3 = txtSupplier3.Text;
                        Current_VS.Vs_requester = txtRequester.Text;
                        Current_VS.Vs_date = DateTime.Now;
                        Current_VS.Vs_comment = txtComment.Text;
                        Current_VS.Vs_des = txtDescription.Text;
                        adoClass = new ADO();
                        adoClass.Print_PUR_VS_Detail(Current_VS, filepath, "export");
                        string VS_no = txtVsNo.Text;
                        string historyPR = "";
                        string link_approve = "https://drive.google.com/uc?export=view&id=1qycuNDOYPDQk69vCylhOjRpPk0fNlCgB";
                        string link_reject = "https://drive.google.com/uc?export=view&id=19meOrB4l9aIlltXXZLJA_49XnY-dkw8q";
                        string m_body = "<p>Dear,</p> \n\n ";
                        m_body += "<p>Please check the " + VS_no + " as attached and the content:</p> \n ";
                        m_body += "<p>" + txtDescription.Text + "</p> \n ";
                        m_body += "<p>Then click the button below to approve or reject:</p> \n ";
                        m_body += "<p><a href='mailto:hvn.system@hutchinson.vn?subject=Re:[HVN-System]:[PUR]:[Regis Item]:" + VS_no + ":Yes&body=Note:'><img src='" + link_approve + "' width='108' height='35' alt='Approve' ></a></body></html></p> \n";
                        m_body += "<p><a href='mailto:hvn.system@hutchinson.vn?subject=Re:[HVN-System]:[PUR]:[Regis Item]:" + VS_no + ":No&body=Reason:'><img src='" + link_reject + "' width='94' height='35'alt='Reject'></a></body></html></p> \n ";
                        m_body += historyPR;
                        m_body += "<p>VS made by " + General_Infor.myaccount.Username + " at " + DateTime.Now.ToString("HH:mm dd/MM/yyyy") + "</p> \n";
                        m_body += "<p>Note: After send the email to approve or reject, you will not able to change your decision.</p> \n\n ";
                        m_body += "<p>Regards,</p> \n ";
                        SendEmail("[HVN-System]:[PUR]:[Regis Item]:" + VS_no,General_Infor.myaccount.Direct_manager+"@hutchinson.com", "", m_body, filepath,attached1,attached2,attached3);
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

        private void btnSaveDraft_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            General_condition();
            SplashScreenManager.ShowForm(this, typeof(frmWaitingForm), true, true, false);
            SplashScreenManager.Default.SetWaitFormCaption("Please wait...");
            try
            {
                string strQry = "delete from PUR_VS where vs_id=N'" + txtVsNo.Text + "'  \n ";
                strQry += " insert into PUR_VS \n ";
                strQry += " (vs_id,vs_requester,vs_date,vs_des,item_name, \n ";
                strQry += " supplier_1,supplier_1_type,supplier_1_att \n ";
                strQry += " ,supplier_2,supplier_2_type,supplier_2_att,selected_supplier \n ";
                strQry += " ,supplier_3,supplier_3_type,supplier_3_att,vs_status \n ";
                strQry += " ,dept_mgr,fin_mgr,pur_mgr,plant_mgr,estimate_yearly_amount \n ";
                strQry += " ,pur,current_pic,vs_comment,dept) \n ";
                strQry += "  select N'" + txtVsNo.Text + "',N'" + General_Infor.username + "',getdate(),N'" + txtDescription.Text + "',N'" + Info_vendor1[3].Vs_des + "', \n ";
                strQry += "  N'" + txtSupplier1.Text + "',N'" + cboVendorType1.Text + "',N'" + attached1 + "', \n ";
                strQry += "  N'" + txtSupplier2.Text + "',N'" + cboVendorType2.Text + "',N'" + attached2 + "',N'" + cboSelectedSupplier.Text + "', \n ";
                strQry += "  N'" + txtSupplier3.Text + "',N'" + cboVendorType3.Text + "',N'" + attached3 + "',N'Draft', \n ";
                strQry += "  N'" + txtDeptMgr.Text + "',N'" + txtFinMgr.Text + "',N'" + txtPurMgr.Text + "',N'" + txtPlantMgr.Text + "',N'" + nmEstimatedCost.Value.ToString() + "', \n ";
                strQry += "  N'" + txtPur.Text + "',N'" + General_Infor.username + "',N'" + txtComment.Text + "',N'" + General_Infor.myaccount.Department + "' \n ";
                strQry += " delete from PUR_VS_SupplierInfo where vs_id=N'" + txtVsNo.Text + "' \n ";
                string qr1 = "";
                foreach (PUR_VS_SupplierInfo_Entity item in Info_vendor1)
                {
                    if (qr1 == "")
                    {
                        qr1 += "select N'" + txtVsNo.Text + "',N'" + txtSupplier1.Text + "',N'" + item.Vs_stt + "',N'" + item.Vs_subject + "',N'" + item.Vs_des + "',N'" + item.Vs_score + "'\n";
                    }
                    else
                    {
                        qr1 += "union all select N'" + txtVsNo.Text + "',N'" + txtSupplier1.Text + "',N'" + item.Vs_stt + "',N'" + item.Vs_subject + "',N'" + item.Vs_des + "',N'" + item.Vs_score + "'\n";
                    }
                }
                strQry += "insert into PUR_VS_SupplierInfo (vs_id,supplier_name,vs_stt,vs_subject,vs_des,vs_score)\n";
                strQry += qr1;
                if (txtSupplier2.Text != "")
                {
                    string qr2 = "";
                    foreach (PUR_VS_SupplierInfo_Entity item in Info_vendor2)
                    {
                        if (qr2 == "")
                        {
                            qr2 += "select N'" + txtVsNo.Text + "',N'" + txtSupplier2.Text + "',N'" + item.Vs_stt + "',N'" + item.Vs_subject + "',N'" + item.Vs_des + "',N'" + item.Vs_score + "'\n";
                        }
                        else
                        {
                            qr2 += "union all select N'" + txtVsNo.Text + "',N'" + txtSupplier2.Text + "',N'" + item.Vs_stt + "',N'" + item.Vs_subject + "',N'" + item.Vs_des + "',N'" + item.Vs_score + "'\n";
                        }
                    }
                    strQry += "insert into PUR_VS_SupplierInfo (vs_id,supplier_name,vs_stt,vs_subject,vs_des,vs_score)\n";
                    strQry += qr2;
                }
                if (txtSupplier3.Text != "")
                {
                    string qr3 = "";
                    foreach (PUR_VS_SupplierInfo_Entity item in Info_vendor3)
                    {
                        if (qr3 == "")
                        {
                            qr3 += "select N'" + txtVsNo.Text + "',N'" + txtSupplier3.Text + "',N'" + item.Vs_stt + "',N'" + item.Vs_subject + "',N'" + item.Vs_des + "',N'" + item.Vs_score + "'\n";
                        }
                        else
                        {
                            qr3 += "union all select N'" + txtVsNo.Text + "',N'" + txtSupplier3.Text + "',N'" + item.Vs_stt + "',N'" + item.Vs_subject + "',N'" + item.Vs_des + "',N'" + item.Vs_score + "'\n";
                        }
                    }
                    strQry += "insert into PUR_VS_SupplierInfo (vs_id,supplier_name,vs_stt,vs_subject,vs_des,vs_score)\n";
                    strQry += qr3;
                }
                conn = new CmCn();
                conn.ExcuteQry(strQry);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            Thread.Sleep(1000);
            SplashScreenManager.CloseForm();
        }

        private void btnCancel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (XtraMessageBox.Show("Do you want to CANCEL this request?", "CANCEL", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                string strQry= "delete from PUR_VS where vs_id=N'"+txtVsNo.Text+"'\n";
                strQry += "delete from PUR_VS_SupplierInfo where vs_id=N'" + txtVsNo.Text + "'\n";
                conn = new CmCn();
                conn.ExcuteQry(strQry);
                this.Close();
            }
        }

        private void gvVendor1_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            current_item_vendor1 = gvVendor1.GetRow(gvVendor1.FocusedRowHandle) as PUR_VS_SupplierInfo_Entity;
            if (e.Column.FieldName == "Vs_des")
            {
                if (current_item_vendor1.Vs_subject== "EXW price")
                {
                    Auto_insert_sup_info_2("Maximum price", current_item_vendor1.Vs_des, Info_vendor1);
                    Auto_insert_sup_info_2("Minimum price", current_item_vendor1.Vs_des, Info_vendor1);
                    Auto_insert_sup_info_2("DDP cost", current_item_vendor1.Vs_des, Info_vendor1);
                    Auto_insert_sup_info_2("Standard transportation costs", "0", Info_vendor1);
                    dgvVendor1.DataSource = Info_vendor1.ToList();
                }
                else if (current_item_vendor1.Vs_subject == "Standard transportation costs")
                {
                    string EXW_price = Info_vendor1[0].Vs_des;
                    string transportation_cost = Info_vendor1[1].Vs_des;
                    if (current_item_vendor1.Vs_des!="0")
                    {
                        
                        try
                        {
                            string DDP_cost = (float.Parse(EXW_price) + float.Parse(transportation_cost)).ToString();
                            Auto_insert_sup_info_3("DDP cost", "Mandatory", Info_vendor1);
                            Auto_insert_sup_info_2("DDP cost", DDP_cost, Info_vendor1);
                            dgvVendor1.DataSource = Info_vendor1.ToList();
                        }
                        catch (Exception)
                        {
                            MessageBox.Show("EXW Price or Transportation cost has wrong format");
                        }
                    }
                    else
                    {

                        try
                        {
                            string DDP_cost = (float.Parse(EXW_price) + float.Parse(transportation_cost)).ToString();
                            Auto_insert_sup_info_3("DDP cost", "Read Only", Info_vendor1);
                            dgvVendor1.DataSource = Info_vendor1.ToList();
                        }
                        catch (Exception)
                        {
                            MessageBox.Show("EXW Price or Transportation cost has wrong format");
                        }
                       
                    }
                }
            }
        }

        private void gvVendor2_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            current_item_vendor2 = gvVendor2.GetRow(gvVendor2.FocusedRowHandle) as PUR_VS_SupplierInfo_Entity;
            if (e.Column.FieldName == "Vs_des")
            {
                if (current_item_vendor2.Vs_subject == "EXW price")
                {
                    Auto_insert_sup_info_2("Maximum price", current_item_vendor2.Vs_des, Info_vendor2);
                    Auto_insert_sup_info_2("Minimum price", current_item_vendor2.Vs_des, Info_vendor2);
                    Auto_insert_sup_info_2("DDP cost", current_item_vendor2.Vs_des, Info_vendor2);
                    Auto_insert_sup_info_2("Standard transportation costs", "0", Info_vendor2);
                    dgvVendor2.DataSource = Info_vendor2.ToList();
                }
            }
        }

        private void gvVendor3_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            current_item_vendor3 = gvVendor3.GetRow(gvVendor3.FocusedRowHandle) as PUR_VS_SupplierInfo_Entity;
            if (e.Column.FieldName == "Vs_des")
            {
                if (current_item_vendor3.Vs_subject == "EXW price")
                {
                    Auto_insert_sup_info_2("Maximum price", current_item_vendor3.Vs_des, Info_vendor3);
                    Auto_insert_sup_info_2("Minimum price", current_item_vendor3.Vs_des, Info_vendor3);
                    Auto_insert_sup_info_2("DDP cost", current_item_vendor3.Vs_des, Info_vendor3);
                    Auto_insert_sup_info_2("Standard transportation costs", "0", Info_vendor3);
                    dgvVendor3.DataSource = Info_vendor3.ToList();
                }
            }
        }

        private ADO adoClass;
        private void SendEmail(string Subject, string To, string Cc, string Body, string filepath, string att1, string att2, string att3)
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
            if (att1!="")
            {
                mailItem.Attachments.Add(att1);
            }
            if (att2 != "")
            {
                mailItem.Attachments.Add(att2);
            }
            if (att3 != "")
            {
                mailItem.Attachments.Add(att3);
            }
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
        private void cboSelectedSupplier_SelectionChangeCommitted(object sender, EventArgs e)
        {
            switch (cboSelectedSupplier.Text)
            {
                case "SUPPLIER 1":
                    if (txtSupplier1.Text!="")
                    {
                        selected_vendor = Info_vendor1;
                        selected_vendor_type = cboVendorType1.Text;
                    }
                    else
                    {
                        MessageBox.Show("ERROR: Supplier has not been input information");
                        cboSelectedSupplier.Text = "";
                    }
                    break;
                case "SUPPLIER 2":
                    if (txtSupplier2.Text != "")
                    {
                        selected_vendor = Info_vendor2;
                        selected_vendor_type = cboVendorType2.Text;
                    }
                    else
                    {
                        MessageBox.Show("ERROR: Supplier has not been input information");
                        cboSelectedSupplier.Text = "";
                    }
                    break;
                case "SUPPLIER 3":
                    if (txtSupplier3.Text != "")
                    {
                        selected_vendor = Info_vendor3;
                        selected_vendor_type = cboVendorType3.Text;
                    }
                    else
                    {
                        MessageBox.Show("ERROR: Supplier has not been input information");
                        cboSelectedSupplier.Text = "";
                    }
                    break;
                default:
                    break;
            }
        }

        private void cboSupplier2_EditValueChanged(object sender, EventArgs e)
        {
            txtSupplier2.Text = cboSupplier2.Text;
            string strQry = "select * from PUR_MasterListSupplier where supplier_name=N'" + txtSupplier2.Text + "'";
            conn = new CmCn();
            DataTable dt = conn.ExcuteDataTable(strQry);
            Auto_insert_sup_info("Short name of supplier", dt.Rows[0]["sup_shortname"].ToString(), Info_vendor2);
            Auto_insert_sup_info("Supplier Address:", dt.Rows[0]["sup_address"].ToString(), Info_vendor2);
            Auto_insert_sup_info("PIC phone no.", dt.Rows[0]["sup_tel"].ToString(), Info_vendor2);
            Auto_insert_sup_info("PIC name", dt.Rows[0]["contact_pic"].ToString(), Info_vendor2);
            Auto_insert_sup_info("PIC email", dt.Rows[0]["email_address"].ToString(), Info_vendor2);
            Auto_insert_sup_info("Currency", dt.Rows[0]["sup_currency"].ToString(), Info_vendor2);
            Auto_insert_sup_info("Payment term", dt.Rows[0]["payment_term"].ToString(), Info_vendor2);
            Auto_insert_sup_info("Standard Transportation mode", dt.Rows[0]["delivery_mode"].ToString(), Info_vendor2);
            Auto_insert_sup_info("Incoterms", dt.Rows[0]["incoterm"].ToString(), Info_vendor2);
            dgvVendor2.DataSource = Info_vendor2.ToList();
        }

        private void cboSupplier3_EditValueChanged(object sender, EventArgs e)
        {
            txtSupplier3.Text = cboSupplier3.Text;
            string strQry = "select * from PUR_MasterListSupplier where supplier_name=N'" + txtSupplier3.Text + "'";
            conn = new CmCn();
            DataTable dt = conn.ExcuteDataTable(strQry);
            Auto_insert_sup_info("Short name of supplier", dt.Rows[0]["sup_shortname"].ToString(), Info_vendor3);
            Auto_insert_sup_info("Supplier Address:", dt.Rows[0]["sup_address"].ToString(), Info_vendor3);
            Auto_insert_sup_info("PIC phone no.", dt.Rows[0]["sup_tel"].ToString(), Info_vendor3);
            Auto_insert_sup_info("PIC name", dt.Rows[0]["contact_pic"].ToString(), Info_vendor3);
            Auto_insert_sup_info("PIC email", dt.Rows[0]["email_address"].ToString(), Info_vendor3);
            Auto_insert_sup_info("Currency", dt.Rows[0]["sup_currency"].ToString(), Info_vendor3);
            Auto_insert_sup_info("Payment term", dt.Rows[0]["payment_term"].ToString(), Info_vendor3);
            Auto_insert_sup_info("Standard Transportation mode", dt.Rows[0]["delivery_mode"].ToString(), Info_vendor3);
            Auto_insert_sup_info("Incoterms", dt.Rows[0]["incoterm"].ToString(), Info_vendor3);
            dgvVendor3.DataSource = Info_vendor3.ToList();
        }
    }
}
