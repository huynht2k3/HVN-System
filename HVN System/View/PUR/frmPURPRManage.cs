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
using DevExpress.XtraSplashScreen;
using HVN_System.View.Admin;
using HVN_System.View.PUR;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Columns;

namespace HVN_System.View.Planning
{
    public partial class frmPURPRManage : Form
    {
        public frmPURPRManage()
        {
            InitializeComponent();
        }
        private ADO adoClass;
        private CmCn conn;
        private List<PUR_PR_Entity> List_Data;
        private PUR_PR_Entity Current_PR;
        private void btnRefresh_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Load_List_PR();
        }
        private void Load_permission()
        {
            adoClass = new ADO();
            gcIssue.Visible = adoClass.Check_permission("frmPURPO", "btnNew", General_Infor.username);
        }
        private void Load_combobox()
        {
            string strQry = "Select supplier_name as [Supplier],sup_shortname as [Short name] from PUR_MasterListSupplier where supplier_status=N'Active'";
            conn = new CmCn();
            DataTable dt = conn.ExcuteDataTable(strQry);
            cboSupplier.Properties.DataSource = dt;
            cboSupplier.Properties.ValueMember = "Supplier";
            cboSupplier.Properties.DisplayMember = "Supplier";
        }
        private void frmProductionPlanFG_Load(object sender, EventArgs e)
        {
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
            cboMonth.Text = DateTime.Today.Month.ToString();
            cboYear.Text= DateTime.Today.Year.ToString();
            cboTypeSearch.Text = "My pending task";
            Load_permission();
            Load_List_PR();
            Load_combobox();
            Current_PR = new PUR_PR_Entity();
            layoutControlItem3.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            layoutControlItem4.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            layoutControlItem6.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            layoutControlItem7.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            layoutControlItem8.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            layoutControlItem9.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            layoutControlItem10.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
        }
        private void Load_List_PR()
        {
            string strQry = "";
            switch (cboTypeSearch.Text)
            {
                case "My pending task":
                    strQry += "select *,amount+vat as amount_vat from PUR_PR where current_pic=N'" + General_Infor.username + "' and pr_status<>N'Draft'";
                    break;
                case "Duration PR date":
                    strQry += "select *,amount+vat as amount_vat from PUR_PR where pr_date>=N'" + dtpFrom.Value.ToString("yyyy-MM-dd") + " 00:00:00' and pr_date<=N'"+ dtpTo.Value.ToString("yyyy-MM-dd") + " 23:59:59' and pr_status<>N'Draft'";
                    break;
                case "PR No":
                    strQry += "select *,amount+vat as amount_vat from PUR_PR where pr_no= N'" + txtSearch.Text + "' and pr_status<>N'Draft'";
                    break;
                case "Supplier":
                    strQry += "select *,amount+vat as amount_vat from PUR_PR where supplier_name=N'" + cboSupplier.Text + "' and YEAR(pr_date)=N'" + cboYear.Text + "' and pr_status<>N'Draft'";
                    break;
                case "Department":
                    strQry += "select *,amount+vat as amount_vat from PUR_PR where dept=N'" + cboDept.Text + "' and YEAR(pr_date)=N'" + cboYear.Text + "' and pr_status<>N'Draft'";
                    break;
                case "Month":
                    strQry += "select *,amount+vat as amount_vat from PUR_PR where MONTH(pr_date)=N'"+cboMonth.Text+ "' and YEAR(pr_date)=N'" + cboYear.Text + "' and pr_status<>N'Draft'";
                    break;
                default:
                    break;
            }
            List_Data = new List<PUR_PR_Entity>();
            conn = new CmCn();
            if (strQry!="")
            {
                DataTable dt = conn.ExcuteDataTable(strQry);
                int i = 1;
                foreach (DataRow row in dt.Rows)
                {
                    PUR_PR_Entity item = new PUR_PR_Entity();
                    item.Stt = i;
                    item.IsSelected = false;
                    item.Pr_no = row["pr_no"].ToString();
                    item.Requester = row["requester"].ToString();
                    item.Pr_content = row["pr_content"].ToString();
                    item.Pr_type = row["pr_type"].ToString();
                    item.Pr_currency = row["pr_currency"].ToString();
                    item.Dept = row["dept"].ToString();
                    item.Is_active = row["is_active"].ToString();
                    item.Create_user = row["create_user"].ToString();
                    item.Pr_date = DateTime.Parse(row["pr_date"].ToString());
                    item.Estimate_received_date = string.IsNullOrEmpty(row["estimate_received_date"].ToString())? item.Pr_date:DateTime.Parse(row["estimate_received_date"].ToString());
                    item.Expect_issue_po_date = string.IsNullOrEmpty(row["expect_issue_po_date"].ToString()) ? item.Pr_date : DateTime.Parse(row["expect_issue_po_date"].ToString());
                    item.Amount = decimal.Parse(row["amount"].ToString());
                    item.Amount_vat = decimal.Parse(row["amount_vat"].ToString());
                    item.Vat = decimal.Parse(row["vat"].ToString());
                    item.Checker = row["checker"].ToString();
                    item.Approver = row["approver"].ToString();
                    item.Requester_date = string.IsNullOrEmpty(row["requester_date"].ToString()) ? DateTime.Now : DateTime.Parse(row["requester_date"].ToString());
                    item.Check_date = string.IsNullOrEmpty(row["check_date"].ToString()) ? DateTime.Now : DateTime.Parse(row["check_date"].ToString());
                    item.Approve_date = string.IsNullOrEmpty(row["approve_date"].ToString()) ? DateTime.Now : DateTime.Parse(row["approve_date"].ToString());
                    item.Pr_status = row["pr_status"].ToString();
                    item.Requester_sign = row["requester_sign"].ToString();
                    item.Approve_sign = row["approve_sign"].ToString();
                    item.Check_sign = row["check_sign"].ToString();
                    item.Capex_no = row["capex_no"].ToString();
                    item.Customer_name = row["customer_name"].ToString();
                    item.Project_name = row["project_name"].ToString();
                    item.Current_pic = row["current_pic"].ToString();
                    item.Po_no = row["po_no"].ToString();
                    item.Old_pr_no = row["old_pr_no"].ToString();
                    item.Checker_comment = row["checker_comment"].ToString();
                    item.Approve_comment = row["approve_comment"].ToString();
                    item.Supplier_name = row["supplier_name"].ToString();
                    item.Advance_payment = row["advance_payment"].ToString();
                    List_Data.Add(item);
                    i++;
                }
                dgvResult.DataSource = List_Data.ToList();
            }
            
        }   
        private void gvResult_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            Current_PR = gvResult.GetRow(gvResult.FocusedRowHandle) as PUR_PR_Entity;
        }

        private void btnDelete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (Current_PR.Pr_no!=null)
            {
                if (XtraMessageBox.Show("Do you want to delete this PR?", "Delete PR", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    adoClass = new ADO();
                    adoClass.Delete_PR(Current_PR.Pr_no);
                    Load_List_PR();
                }
            }
            else
            {
                MessageBox.Show("Please choose PR before delete","Error");
            }
        }

        private void gvResult_DoubleClick(object sender, EventArgs e)
        {
            
        }

        private void btnPrint_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (XtraMessageBox.Show("Do you want to print these PR?", "Print multi PR", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {

            }
        }

        private void btnIssuePO_Click(object sender, EventArgs e)
        {
            
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            Current_PR = gvResult.GetRow(gvResult.FocusedRowHandle) as PUR_PR_Entity;
            frmPURPRDetail frm = new frmPURPRDetail(Current_PR, "View");
            frm.ShowDialog();
            btnRefresh.PerformClick();
        }

        private void ckIsSelect_CheckedChanged(object sender, EventArgs e)
        {
            if (gvResult.PostEditor())
            {
                gvResult.UpdateCurrentRow();
            }
        }

        private void gvResult_CustomRowCellEdit(object sender, DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventArgs e)
        {
            if (!isExport)
            {
                if (e.Column.Name == "gcIssue")
                {
                    string val = gvResult.GetRowCellValue(e.RowHandle, "Pr_status").ToString();
                    if (val != "Pending issue PO")
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

        private void btnIssuePO_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            Current_PR = gvResult.GetRow(gvResult.FocusedRowHandle) as PUR_PR_Entity;
            if (string.IsNullOrEmpty(Current_PR.Po_no))
            {
                if (Current_PR.Pr_status == "Pending issue PO")
                {
                    frmPURPODetail frm = new frmPURPODetail(Current_PR);
                    frm.ShowDialog();
                }
                else
                {
                    MessageBox.Show("The PR has not approved yet", "Error");
                }
            }
            else
            {
                MessageBox.Show("The PR has been created PO already", "Error");
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

        private void btnPrintPO_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            Current_PR = gvResult.GetRow(gvResult.FocusedRowHandle) as PUR_PR_Entity;
            if (XtraMessageBox.Show("Do you want to print " + Current_PR.Pr_no + "?", "Print PR", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                SplashScreenManager.ShowForm(this, typeof(frmWaitingForm), true, true, false);
                SplashScreenManager.Default.SetWaitFormCaption("Please wait...");
                string file_path = @"\\172.16.180.20\05.it\16.HVN\ATTACHMENT\PURCHASE\PR\" + Current_PR.Pr_no + ".pdf";
                adoClass = new ADO();
                adoClass.Print_PUR_PR_Detail(Current_PR, file_path, "print");
                SplashScreenManager.CloseForm();
            }
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

        private bool isExport = false;
        private void btnExportExcel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            isExport = true;
            adoClass = new ADO();
            adoClass.Export_Excel(dgvResult);
            isExport = false;
        }

        private void cboTypeSearch_SelectionChangeCommitted(object sender, EventArgs e)
        {
            layoutControlItem3.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            layoutControlItem4.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            layoutControlItem6.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            layoutControlItem7.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            layoutControlItem8.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            layoutControlItem9.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            layoutControlItem10.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            switch (cboTypeSearch.Text)
            {
                case "My pending task":
                    btnRefresh.PerformClick();
                    break;
                case "Duration PR date":
                    layoutControlItem3.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                    layoutControlItem4.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                    dtpFrom.Value = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                    dtpTo.Value = DateTime.Today;
                    btnRefresh.PerformClick();
                    break;
                case "PR No":
                    layoutControlItem6.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                    break;
                case "Supplier":
                    layoutControlItem8.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                    layoutControlItem10.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                    break;
                case "Department":
                    layoutControlItem7.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                    layoutControlItem10.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                    break;
                case "Month":
                    layoutControlItem9.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                    layoutControlItem10.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                    btnRefresh.PerformClick();
                    break;
                default:
                    break;
            }
        }

        private void cboSupplier_EditValueChanged(object sender, EventArgs e)
        {
            btnRefresh.PerformClick();
        }

        private void cboDept_SelectionChangeCommitted(object sender, EventArgs e)
        {
            btnRefresh.PerformClick();
        }
    }
}
