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

namespace HVN_System.View.Planning
{
    public partial class frmPURPR : Form
    {
        public frmPURPR()
        {
            InitializeComponent();
        }
        private ADO adoClass;
        private List<PUR_PR_Entity> List_Data;
        private PUR_PR_Entity Current_PR;
        private void btnEdit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (Current_PR.Pr_no!="")
            {
                frmPURPRDetail frm = new frmPURPRDetail(Current_PR,"View");
                frm.ShowDialog();
                btnRefresh.PerformClick();
            }
        }
        private void btnRefresh_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Load_List_PR();
        }
        private void Load_permission()
        {
            adoClass = new ADO();
            btnNew.Enabled = adoClass.Check_permission(this.Name, btnNew.Name, General_Infor.username);
            gridColumn14.Visible = adoClass.Check_permission(this.Name, btnNew.Name, General_Infor.username);
        }
        private void frmProductionPlanFG_Load(object sender, EventArgs e)
        {
            Load_permission();
            if (General_Infor.username != "admin")
            {
                try
                {
                    string filepath = @"\\172.16.180.20\20.Public\05.IT\03.HVN_SYS\01.Format_Excel\PUR_PR_Detail.xlsx";
                    string new_path = @"C:\HVN_SYS\01.Format_Excel\PUR_PR_Detail.xlsx";
                    File.Copy(filepath, new_path, true);
                }
                catch (Exception)
                {

                }
            }
            Load_List_PR();
            Current_PR = new PUR_PR_Entity();
        }
        private void Load_List_PR()
        {
            List_Data = new List<PUR_PR_Entity>();
            adoClass = new ADO();
            DataTable dt = adoClass.Load_PUR_PR("*,amount+vat as amount_vat", "is_active='1' and dept=N'" + General_Infor.myaccount.Department + "'");
            int i = 1;
            foreach (DataRow row in dt.Rows)
            {
                PUR_PR_Entity item = new PUR_PR_Entity();
                item.Stt = i;
                item.Pr_no = row["pr_no"].ToString();
                item.Requester = row["requester"].ToString();
                item.Pr_content = row["pr_content"].ToString();
                item.Pr_type = row["pr_type"].ToString();
                item.Pr_currency = row["pr_currency"].ToString();
                item.Dept = row["dept"].ToString();
                item.Is_active = row["is_active"].ToString();
                item.Create_user = row["create_user"].ToString();
                item.Pr_date = DateTime.Parse(row["pr_date"].ToString());
                item.Estimate_received_date = string.IsNullOrEmpty(row["estimate_received_date"].ToString()) ? DateTime.Now : DateTime.Parse(row["estimate_received_date"].ToString());
                item.Amount = string.IsNullOrEmpty(row["amount"].ToString()) ? 0:decimal.Parse(row["amount"].ToString());
                item.Vat = string.IsNullOrEmpty(row["vat"].ToString()) ? 0 : decimal.Parse(row["vat"].ToString());
                item.Amount_vat = string.IsNullOrEmpty(row["amount_vat"].ToString()) ? 0 : decimal.Parse(row["amount_vat"].ToString());
                item.Checker = row["checker"].ToString();
                item.Approver = row["approver"].ToString();
                item.Requester_date = string.IsNullOrEmpty(row["requester_date"].ToString()) ? DateTime.Now : DateTime.Parse(row["requester_date"].ToString());
                item.Check_date = string.IsNullOrEmpty(row["check_date"].ToString())?DateTime.Now:DateTime.Parse(row["check_date"].ToString());
                item.Approve_date = string.IsNullOrEmpty(row["approve_date"].ToString()) ? DateTime.Now : DateTime.Parse(row["approve_date"].ToString());
                item.Pr_status = row["pr_status"].ToString();
                item.Requester_sign = row["requester_sign"].ToString();
                item.Approve_sign = row["approve_sign"].ToString();
                item.Check_sign = row["check_sign"].ToString();
                item.Capex_no = row["capex_no"].ToString();
                item.Current_pic = row["current_pic"].ToString();
                item.Customer_name = row["customer_name"].ToString();
                item.Project_name = row["project_name"].ToString();
                item.Po_no = row["po_no"].ToString();
                item.Old_pr_no = row["old_pr_no"].ToString();
                item.Checker_comment = row["checker_comment"].ToString();
                item.Approve_comment = row["approve_comment"].ToString();
                item.Supplier_name = row["supplier_name"].ToString();
                item.Advance_payment = row["advance_payment"].ToString();
                item.Expect_issue_po_date= string.IsNullOrEmpty(row["expect_issue_po_date"].ToString()) ? DateTime.Now : DateTime.Parse(row["expect_issue_po_date"].ToString());
                List_Data.Add(item);
                i++;
            }
            dgvResult.DataSource = List_Data.ToList();
        }
        private void btnNew_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            PUR_PR_Entity PR = new PUR_PR_Entity();
            frmPURPRDetail frm= new frmPURPRDetail(PR,"New");
            frm.ShowDialog();
            btnRefresh.PerformClick();
        }

        private void gvResult_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            Current_PR = gvResult.GetRow(gvResult.FocusedRowHandle) as PUR_PR_Entity;
        }

        private void gvResult_DoubleClick(object sender, EventArgs e)
        {
           
        }

        private void btnViewPR_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            Current_PR = gvResult.GetRow(gvResult.FocusedRowHandle) as PUR_PR_Entity;
            frmPURPRDetail frm = new frmPURPRDetail(Current_PR, "View");
            frm.ShowDialog();
            btnRefresh.PerformClick();
        }

        private void btnEditPR_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            Current_PR = gvResult.GetRow(gvResult.FocusedRowHandle) as PUR_PR_Entity;
            frmPURPRDetail frm = new frmPURPRDetail(Current_PR, "Edit");
            frm.ShowDialog();
            btnRefresh.PerformClick();
        }

        private void gvResult_CustomRowCellEdit(object sender, DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventArgs e)
        {
            if (!isExport)
            {
                if (e.Column.Caption == "Edit")
                {
                    string val = gvResult.GetRowCellValue(e.RowHandle, "Pr_status").ToString();
                    if (val != "Pending requester" && val != "Draft")
                    {
                        RepositoryItemButtonEdit ritem = new RepositoryItemButtonEdit();
                        ritem.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
                        ritem.ReadOnly = true;
                        ritem.Buttons[0].Enabled = false;
                        e.RepositoryItem = ritem;
                    }
                }
                else if (e.Column.Caption == "View PO")
                {
                    string val = gvResult.GetRowCellValue(e.RowHandle, "Po_no").ToString();
                    if (val == "")
                    {
                        RepositoryItemButtonEdit ritem = new RepositoryItemButtonEdit();
                        ritem.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
                        ritem.ReadOnly = true;
                        ritem.Buttons[0].Enabled = false;
                        e.RepositoryItem = ritem;
                    }
                }
                else if (e.Column.Caption == "Replace")
                {
                    string val = gvResult.GetRowCellValue(e.RowHandle, "Old_pr_no").ToString();
                    if (val != "")
                    {
                        RepositoryItemButtonEdit ritem = new RepositoryItemButtonEdit();
                        ritem.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
                        ritem.ReadOnly = true;
                        ritem.Buttons[0].Enabled = false;
                        e.RepositoryItem = ritem;
                    }
                }
            }
        }

        private void btnExport_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            SplashScreenManager.ShowForm(this, typeof(frmWaitingForm), true, true, false);
            SplashScreenManager.Default.SetWaitFormCaption("Please wait...");
            Current_PR = gvResult.GetRow(gvResult.FocusedRowHandle) as PUR_PR_Entity;
            string FilePath = @"\\172.16.180.20\05.it\16.HVN\ATTACHMENT\PURCHASE\PR\" + Current_PR.Pr_no + ".pdf";
            adoClass = new ADO();
            adoClass.Print_PUR_PR_Detail(Current_PR, FilePath, "export");
            string new_path = @"C:\HVN_SYS_CONFIG\" + Current_PR.Pr_no + ".pdf";
            File.Copy(FilePath, new_path, true);
            System.Diagnostics.Process.Start(new_path);
            SplashScreenManager.CloseForm();
        }

        private void btnViewPdf_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            Current_PR = gvResult.GetRow(gvResult.FocusedRowHandle) as PUR_PR_Entity;
            string filepath = @"\\172.16.180.20\05.it\16.HVN\ATTACHMENT\PURCHASE\PR\" + Current_PR.Pr_no + ".pdf";
            string new_path = @"C:\HVN_SYS_CONFIG\" + Current_PR.Pr_no + ".pdf";
            try
            {
                File.Copy(filepath, new_path, true);
                System.Diagnostics.Process.Start(filepath);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnCopy_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            Current_PR = gvResult.GetRow(gvResult.FocusedRowHandle) as PUR_PR_Entity;
            if (XtraMessageBox.Show("This action will create new PR to replace "+ Current_PR .Pr_no+ ", the PO related to this PR will be cancelled. Do you still want to process?", "Replace PR", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                frmPURPRDetail frm = new frmPURPRDetail(Current_PR, "Modify PR");
                frm.ShowDialog();
            }    
        }

        private void btnViewPO_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            Current_PR = gvResult.GetRow(gvResult.FocusedRowHandle) as PUR_PR_Entity;
            string filepath = @"\\172.16.180.20\05.it\16.HVN\ATTACHMENT\PURCHASE\PO\" + Current_PR.Po_no + ".pdf";
            string new_path = @"C:\HVN_SYS_CONFIG\" + Current_PR.Po_no + ".pdf";
            if (File.Exists(filepath))
            {
                try
                {
                    File.Copy(filepath, new_path, true);
                    System.Diagnostics.Process.Start(new_path);
                }
                catch (Exception)
                {
                    MessageBox.Show("The PO file is opened already","Error");
                }
            }
            else
            {
                MessageBox.Show("The PR has not issue PO yet \nPlease contact purchaser", "Error");
            }
        }
        private bool isExport = false;
        private void btnExportExcel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            isExport = true;
            adoClass = new ADO();
            adoClass.Export_Excel(dgvResult);
            isExport = false;
        }
    }
}
