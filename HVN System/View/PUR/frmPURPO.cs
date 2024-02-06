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
using DevExpress.XtraSplashScreen;
using HVN_System.View.Admin;
using DevExpress.XtraEditors.Repository;

namespace HVN_System.View.Planning
{
    public partial class frmPURPO : Form
    {
        public frmPURPO()
        {
            InitializeComponent();
        }
        private ADO adoClass;
        private List<PUR_PO_Entity> List_Data;
        private PUR_PO_Entity Current_PO;
        private void btnRefresh_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Load_List_PO();
        }
        private void Load_permission()
        {
            adoClass = new ADO();
            btnNew.Enabled = adoClass.Check_permission(this.Name, btnNew.Name, General_Infor.username);
            gridColumn25.Visible = adoClass.Check_permission(this.Name, btnNew.Name, General_Infor.username);
            gcEdit.Visible = adoClass.Check_permission(this.Name, btnNew.Name, General_Infor.username);
        }
        private void frmProductionPlanFG_Load(object sender, EventArgs e)
        {
            Load_permission();
            if (General_Infor.username != "admin")
            {
                try
                {
                    string filepath = @"\\172.16.180.20\20.Public\05.IT\03.HVN_SYS\01.Format_Excel\PUR_PO_Detail.xlsx";
                    string new_path = @"C:\HVN_SYS\01.Format_Excel\PUR_PO_Detail.xlsx";
                    File.Copy(filepath, new_path, true);
                }
                catch (Exception)
                {

                }
            }
            cboTypeSearch.Text = "My pending task";
            Load_List_PO();
            Load_combobox();
            Current_PO = new PUR_PO_Entity();
            layoutControlItem3.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            layoutControlItem4.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            layoutControlItem6.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            layoutControlItem7.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            layoutControlItem8.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
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
        private void Load_List_PO()
        {
           string strQry = "";
            switch (cboTypeSearch.Text)
            {
                case "My pending task":
                    strQry += "select *,amount+vat as amount_vat,datepart(wk,pickup_date) as pkw from PUR_PO where current_pic=N'" + General_Infor.username + "'";
                    break;
                case "Duration PO date":
                    strQry += "select *,amount+vat as amount_vat,datepart(wk,pickup_date) as pkw from PUR_PO where po_date>=N'" + dtpFrom.Value.ToString("yyyy-MM-dd") + " 00:00:00' and po_date<=N'" + dtpTo.Value.ToString("yyyy-MM-dd") + " 23:59:59'";
                    break;
                case "PO No":
                    strQry += "select *,amount+vat as amount_vat,datepart(wk,pickup_date) as pkw from PUR_PO where po_no= N'" + txtSearch.Text + "'";
                    break;
                case "Supplier":
                    strQry += "select *,amount+vat as amount_vat,datepart(wk,pickup_date) as pkw from PUR_PO where supplier_name=N'" + cboSupplier.Text + "'";
                    break;
                case "Department":
                    strQry += "select *,amount+vat as amount_vat,datepart(wk,pickup_date) as pkw from PUR_PO where dept=N'" + cboDept.Text + "'";
                    break;
                default:
                    break;
            }
            List_Data = new List<PUR_PO_Entity>();
            conn = new CmCn();
            if (strQry != "")
            {
                DataTable dt = conn.ExcuteDataTable(strQry);
                foreach (DataRow row in dt.Rows)
                {
                    PUR_PO_Entity item = new PUR_PO_Entity();
                    item.Is_selected = false;
                    item.Po_no = row["po_no"].ToString();
                    item.Supplier_name = row["supplier_name"].ToString();
                    item.Po_date = DateTime.Parse(row["po_date"].ToString());
                    item.Po_pic_date = DateTime.Parse(row["po_pic_date"].ToString());
                    item.Last_time_update = string.IsNullOrEmpty(row["pickup_date"].ToString()) ? item.Po_date : DateTime.Parse(row["last_time_update"].ToString());
                    item.Payment_term = row["payment_term"].ToString();
                    item.Delivery_mode = row["delivery_mode"].ToString();
                    item.Incoterm = row["incoterm"].ToString();
                    item.Pickup_date = string.IsNullOrEmpty(row["pickup_date"].ToString()) ? DateTime.Today : DateTime.Parse(row["pickup_date"].ToString());
                    item.Po_currency = row["po_currency"].ToString();
                    item.Pkw = row["pkw"].ToString();
                    item.Po_pic = row["po_pic"].ToString();
                    item.Po_pic_sign = row["po_pic_sign"].ToString();
                    item.Po_checker = row["po_checker"].ToString();
                    item.Po_check_date = string.IsNullOrEmpty(row["po_check_date"].ToString()) ? DateTime.Today : DateTime.Parse(row["po_check_date"].ToString());
                    item.Po_checker_sign = row["po_checker_sign"].ToString();
                    item.Po_approver = row["po_approver"].ToString();
                    item.Po_approve_date = string.IsNullOrEmpty(row["po_approve_date"].ToString()) ? DateTime.Today : DateTime.Parse(row["po_approve_date"].ToString());
                    item.Po_approver_sign = row["Po_approver_sign"].ToString();
                    item.Pr_no = row["pr_no"].ToString();
                    item.Old_po_no = row["old_po_no"].ToString();
                    item.Is_active = row["is_active"].ToString();
                    item.Po_status = row["po_status"].ToString();
                    item.Requester = row["requester"].ToString();
                    item.Current_pic = row["current_pic"].ToString();
                    item.Dept = row["dept"].ToString();
                    item.Customer_name = row["customer_name"].ToString();
                    item.Project_name = row["project_name"].ToString();
                    item.Purpose = row["purpose"].ToString();
                    item.Amount_vat = string.IsNullOrEmpty(row["amount"].ToString()) ? 0 : decimal.Parse(row["amount"].ToString());
                    item.Vat = string.IsNullOrEmpty(row["vat"].ToString()) ? 0 : decimal.Parse(row["vat"].ToString());
                    item.Amount = item.Amount_vat - item.Vat;
                    item.Po_type = row["po_type"].ToString();
                    item.Capex_no = row["capex_no"].ToString();
                    item.Po_pic_comment = row["po_pic_comment"].ToString();
                    item.Po_checker_comment = row["po_checker_comment"].ToString();
                    item.Po_approver_comment = row["po_approver_comment"].ToString();
                    List_Data.Add(item);
                }
            }
            dgvResult.DataSource = List_Data.ToList();
        }
        private void btnNew_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmPURPODetail frm = new frmPURPODetail();
            frm.ShowDialog();
            btnRefresh.PerformClick();
        }

        private void gvResult_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            Current_PO = gvResult.GetRow(gvResult.FocusedRowHandle) as PUR_PO_Entity;
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            Current_PO = gvResult.GetRow(gvResult.FocusedRowHandle) as PUR_PO_Entity;
            frmPURPODetail frm = new frmPURPODetail(Current_PO, "View PO");
            frm.ShowDialog();
            btnRefresh.PerformClick();
        }

        private void btnPrintPO_Click(object sender, EventArgs e)
        {
            Current_PO = gvResult.GetRow(gvResult.FocusedRowHandle) as PUR_PO_Entity;
            if (Current_PO.Po_no!="")
            {
                SplashScreenManager.ShowForm(this, typeof(frmWaitingForm), true, true, false);
                SplashScreenManager.Default.SetWaitFormCaption("Please wait...");
                string new_path = @"%userprofile%\Downloads\" + Current_PO.Po_no + ".pdf";
                adoClass = new ADO();
                adoClass.Print_PUR_PO_Detail(Current_PO, new_path, "print");
                SplashScreenManager.CloseForm();
            }
        }

        private void ckIsSelected_EditValueChanged(object sender, EventArgs e)
        {
            if (gvResult.PostEditor())
            {
                gvResult.UpdateCurrentRow();
            }
        }

        private void ExportPdf(PUR_PO_Entity _PO,string _file_path)
        {
            bool is_Export = true;
            if (File.Exists(_file_path))
            {
                DateTime last_time_update = System.IO.File.GetLastWriteTime(_file_path);
                if (last_time_update>=_PO.Last_time_update)
                {
                    is_Export = false;
                }
            }
            if (is_Export)
            {
                adoClass = new ADO();
                adoClass.Print_PUR_PO_Detail(Current_PO, _file_path, "export");
            }
        }
        private CmCn conn;
        private void btnSendEmail_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (XtraMessageBox.Show("Do you want to send the email to " + Current_PO.Supplier_name + "?", "Send email", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                string strQry2 = "select email_address from ADM_PersonInChargeOfProcess  \n ";
                strQry2 += " where [procedure_name]=N'PO approval' and [step_name]=N'Send email to supplier' \n ";
                conn = new CmCn();
                DataTable dt = conn.ExcuteDataTable(strQry2);
                string Cc = "";
                foreach (DataRow item in dt.Rows)
                {
                    Cc += item["email_address"].ToString() + ";";
                }
                Current_PO = gvResult.GetRow(gvResult.FocusedRowHandle) as PUR_PO_Entity;
                if (Current_PO.Po_no != "")
                {
                    SplashScreenManager.ShowForm(this, typeof(frmWaitingForm), true, true, false);
                    SplashScreenManager.Default.SetWaitFormCaption("Please wait...");
                    string filepath = @"\\172.16.180.20\05.it\16.HVN\ATTACHMENT\PURCHASE\PO\" + Current_PO.Po_no + ".pdf";
                    string filepath2 = @"\\172.16.180.20\05.it\16.HVN\ATTACHMENT\AMDIN\DOCUMENT\TERMS AND CONDITIONS OF PURCHASE (T&CS).pdf";
                    ExportPdf(Current_PO, filepath);
                    string strQry = "select email_address from PUR_MasterListSupplier  \n ";
                    strQry += " where supplier_name=N'" + Current_PO.Supplier_name + "' \n ";
                    conn = new CmCn();
                    string To = conn.ExcuteString(strQry);
                    string Body = "<p>Dear Supplier,</p> \n\n ";
                    Body += " <p>Please find attached a new order from Hutchinson Vietnam requesting delivery on " + Current_PO.Pickup_date.ToString("dd-MMM-yyyy") + " (W" + Current_PO.Pkw + ")</p> \n ";
                    Body += " </p>Thank you to confirm the availability of the goods on the requested date within 3 working days and send back the Purchase Order with your signature and stamp for payment purpose.</p> \n ";
                    SendEmail(Current_PO.Po_no, To, Cc, Body, filepath, filepath2);
                    string strQry3 = "update PUR_PO set po_status=N'PO has been sent to supplier' where po_no=N'" + Current_PO.Po_no + "'";
                    if (Current_PO.Pr_no != "")
                    {
                        strQry3 += " update PUR_PR set pr_status=N'PO has been sent to supplier' where pr_no=N'" + Current_PO.Pr_no + "' \n ";
                    }
                    conn.ExcuteQry(strQry3);
                    btnRefresh.PerformClick();
                    SplashScreenManager.CloseForm();
                }
            }
        }
        private void SendEmail(string Subject, string To, string Cc, string Body, string filepath1, string filepath2)
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
            mailItem.Display();
            mailItem.HTMLBody = Body + mailItem.HTMLBody;
            mailItem.Attachments.Add(filepath1);
            mailItem.Attachments.Add(filepath2);//logPath is a string holding path to the log.txt file
            //mailItem.Importance = Outlook.OlImportance.olImportanceHigh;
        }

        private void gvResult_CustomRowCellEdit(object sender, DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventArgs e)
        {
            if (!isExport)
            {
                if (e.Column.Name == "gcEmail")
                {
                    string val = gvResult.GetRowCellValue(e.RowHandle, "Po_status").ToString();
                    if (val != "PO approved")
                    {
                        RepositoryItemButtonEdit ritem = new RepositoryItemButtonEdit();
                        ritem.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
                        ritem.ReadOnly = true;
                        ritem.Buttons[0].Enabled = false;
                        e.RepositoryItem = ritem;
                    }
                }
                if (e.Column.Name == "gcEdit")
                {
                    string val = gvResult.GetRowCellValue(e.RowHandle, "Po_status").ToString();
                    if (val != "Pending purchaser")
                    {
                        RepositoryItemButtonEdit ritem = new RepositoryItemButtonEdit();
                        ritem.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
                        ritem.ReadOnly = true;
                        ritem.Buttons[0].Enabled = false;
                        e.RepositoryItem = ritem;
                    }
                }
                if (e.Column.Name == "gcSign")
                {
                    string val = gvResult.GetRowCellValue(e.RowHandle, "Po_status").ToString();
                    if (val != "Waiting Wan Xi Xing approve"&& val != "Waiting Wan Xi Xing & Frederic Le Du approve")
                    {
                        RepositoryItemButtonEdit ritem = new RepositoryItemButtonEdit();
                        ritem.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
                        ritem.ReadOnly = true;
                        ritem.Buttons[0].Enabled = false;
                        e.RepositoryItem = ritem;
                    }
                }
                if (e.Column.Name == "gcComplete")
                {
                    string val = gvResult.GetRowCellValue(e.RowHandle, "Po_status").ToString();
                    if (val != "PO has been sent to supplier")
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

        private void btnEdit_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            Current_PO = gvResult.GetRow(gvResult.FocusedRowHandle) as PUR_PO_Entity;
            frmPURPODetail frm = new frmPURPODetail(Current_PO, "Edit PO");
            frm.ShowDialog();
        }

        private void btnViewPdf_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            //SplashScreenManager.ShowForm(this, typeof(frmWaitingForm), true, true, false);
            //SplashScreenManager.Default.SetWaitFormCaption("Please wait...");
            Current_PO = gvResult.GetRow(gvResult.FocusedRowHandle) as PUR_PO_Entity; 
            string filepath = @"\\172.16.180.20\05.it\16.HVN\ATTACHMENT\PURCHASE\PO\" + Current_PO.Po_no + ".pdf";
            string new_path = @"C:\HVN_SYS_CONFIG\" + Current_PO.Po_no + ".pdf";
            string filepath1 = @"\\172.16.180.20\05.it\16.HVN\ATTACHMENT\PURCHASE\PO\" + Current_PO.Po_no + "_1.pdf";
            string new_path1 = @"C:\HVN_SYS_CONFIG\" + Current_PO.Po_no + "_1.pdf";
            try
            {
                ExportPdf(Current_PO, filepath);
                File.Copy(filepath, new_path, true);
                System.Diagnostics.Process.Start(filepath);
                if (File.Exists(filepath1))
                {
                    File.Copy(filepath1, new_path1, true);
                    System.Diagnostics.Process.Start(filepath1);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            //SplashScreenManager.CloseForm();
        }

       
        private bool isExport = false;
        private void btnExportExcel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            isExport = true;
            adoClass = new ADO();
            adoClass.Export_Excel(dgvResult);
            isExport = false;
        }

        private void btnModify_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            Current_PO = gvResult.GetRow(gvResult.FocusedRowHandle) as PUR_PO_Entity;
            if (XtraMessageBox.Show("This action will create new PO to replace " + Current_PO.Po_no + ". Do you still want to process?", "Replace PO", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                frmPURPODetail frm = new frmPURPODetail(Current_PO, "Modify PO");
                frm.ShowDialog();
                btnRefresh.PerformClick();
            }
        }

        private void btnSign_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            Current_PO = gvResult.GetRow(gvResult.FocusedRowHandle) as PUR_PO_Entity;
            if (Current_PO.Po_status=="Waiting Wan Xi Xing approve"|| Current_PO.Po_status == "Waiting Wan Xi Xing & Frederic Le Du approve")
            {
                if (XtraMessageBox.Show("Do you have the document with full signature for PO "+ Current_PO.Po_no+ "?", "Complete PO", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    OpenFileDialog OpenFile = new OpenFileDialog();
                    OpenFile.Title = "Mở tệp tin";
                    OpenFile.Filter = "PDF (.pdf)|*.pdf";
                    if (OpenFile.ShowDialog() != DialogResult.Cancel)
                    {
                        string filepath = OpenFile.FileName;
                        string new_path = @"\\172.16.180.20\05.it\16.HVN\ATTACHMENT\PURCHASE\PO\" + Current_PO.Po_no + ".pdf";
                        try
                        {
                            File.Copy(filepath, new_path, true);
                            string strQry= "update PUR_PO set po_status=N'PO approved', current_pic=N'' where po_no=N'" + Current_PO.Po_no + "'";
                            if (Current_PO.Pr_no!="")
                            {
                                strQry += " update PUR_PR set pr_status=N'PO approved' where pr_no=N'" + Current_PO.Pr_no + "' \n ";
                            }
                            conn = new CmCn();
                            conn.ExcuteQry(strQry);
                            Current_PO.Po_status = "PO approved";
                            Current_PO.Current_pic = "";
                            gvResult.RefreshData();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select PO before upload signature");
            }
        }

        private void cboTypeSearch_SelectionChangeCommitted(object sender, EventArgs e)
        {
            layoutControlItem3.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            layoutControlItem4.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            layoutControlItem6.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            layoutControlItem7.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            layoutControlItem8.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            switch (cboTypeSearch.Text)
            {
                case "My pending task":

                    btnRefresh.PerformClick();
                    break;
                case "Duration PO date":
                    layoutControlItem3.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                    layoutControlItem4.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                    dtpFrom.Value = DateTime.Today.AddMonths(-1);
                    dtpTo.Value = DateTime.Today;
                    btnRefresh.PerformClick();
                    break;
                case "PO No":
                    layoutControlItem8.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                    break;
                case "Supplier":
                    layoutControlItem7.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                    break;
                case "Department":
                    layoutControlItem6.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                    break;
                default:
                    break;
            }
        }

        private void btnExport_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            SplashScreenManager.ShowForm(this, typeof(frmWaitingForm), true, true, false);
            SplashScreenManager.Default.SetWaitFormCaption("Please wait...");
            Current_PO = gvResult.GetRow(gvResult.FocusedRowHandle) as PUR_PO_Entity;
            string filepath = @"\\172.16.180.20\05.it\16.HVN\ATTACHMENT\PURCHASE\PO\" + Current_PO.Po_no + ".pdf";
            string new_path = @"C:\HVN_SYS_CONFIG\" + Current_PO.Po_no + ".pdf";
            try
            {
                Current_PO = gvResult.GetRow(gvResult.FocusedRowHandle) as PUR_PO_Entity;
                adoClass = new ADO();
                adoClass.Print_PUR_PO_Detail(Current_PO, filepath, "export");
                File.Copy(filepath, new_path, true);
                System.Diagnostics.Process.Start(filepath);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            SplashScreenManager.CloseForm();
           
        }

        private void cboDept_SelectionChangeCommitted(object sender, EventArgs e)
        {
            btnRefresh.PerformClick();
        }

        private void cboSupplier_EditValueChanged(object sender, EventArgs e)
        {
            btnRefresh.PerformClick();
        }

        private void btnComplete_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            Current_PO = gvResult.GetRow(gvResult.FocusedRowHandle) as PUR_PO_Entity;
            if (Current_PO.Po_status == "PO has been sent to supplier")
            {
                if (XtraMessageBox.Show("Have you got PO confirm for PO " + Current_PO.Po_no + "?", "Complete PO", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    OpenFileDialog OpenFile = new OpenFileDialog();
                    OpenFile.Title = "Mở tệp tin";
                    OpenFile.Filter = "PDF (.pdf)|*.pdf";
                    if (OpenFile.ShowDialog() != DialogResult.Cancel)
                    {
                        string filepath = OpenFile.FileName;
                        string new_path = @"\\172.16.180.20\05.it\16.HVN\ATTACHMENT\PURCHASE\PO\" + Current_PO.Po_no + "_1.pdf";
                        try
                        {
                            File.Copy(filepath, new_path, true);
                            string strQry = "update PUR_PO set po_status=N'Got PO confirmation', current_pic=N'' where po_no=N'" + Current_PO.Po_no + "'";
                            if (Current_PO.Pr_no != "")
                            {
                                strQry += " update PUR_PR set pr_status=N'Got PO confirmation' where pr_no=N'" + Current_PO.Pr_no + "' \n ";
                            }
                            conn = new CmCn();
                            conn.ExcuteQry(strQry);
                            Current_PO.Po_status = "Got PO confirmation";
                            Current_PO.Current_pic = "";
                            gvResult.RefreshData();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                }
            }
        }
    }
}
