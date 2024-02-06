using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using HVN_System;
using HVN_System.View;
using HVN_System.Entity;
using HVN_System.Util;
using HVN_System.View.Production;
using HVN_System.View.Planning;
using HVN_System.View.PlantKPI;
using HVN_System.View.Warehouse;
using HVN_System.View.Admin;
using HVN_System.View.HR;
using HVN_System.View.QC;
using Microsoft.Win32;
using System.Diagnostics;
using DevExpress.XtraBars;
using DevExpress.XtraSplashScreen;
using System.Threading;
using HVN_System.View.PUR;

namespace HVN_System
{
    public partial class frmMain : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        public frmMain()
        {
            InitializeComponent();
        }
        private ADO adoClass;
        public string test;
        private CmCn conn;
        private void CloseChildForm()
        {
            foreach (Form f in this.MdiChildren)
            {
                f.Close();
            }
        }
        private Form Kiemtratontai(Type fType)
        {
            foreach (Form f in this.MdiChildren)
            {
                if (f.GetType() == fType)
                {
                    return f;
                }
            }
            return null;
        }
        private void btnCreateLabel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Form frm = this.Kiemtratontai(typeof(frmCreateLabel_Temp2));
            if (frm != null)
            {
                frm.Activate();
                frm.WindowState = FormWindowState.Maximized;
            }
            else
            {
                frmCreateLabel_Temp2 f = new frmCreateLabel_Temp2();
                f.MdiParent = this;
                f.Show();
            }
        }
        private void Load_permission()
        {
           
            adoClass = new ADO();
            List<BarButtonItem> item = new List<BarButtonItem>();
            item.Add(btnApprovalFG);
            item.Add(btnManagePermission);
            item.Add(btnCheckingResult);
            item.Add(btnCreateLabel);
            item.Add(btnMasterListFG);
            item.Add(btnUploadSoftware);
            item.Add(btnKPI_QC_CustomerClaim);
            item.Add(btnKPI_QC_SupplierClaim);
            item.Add(btnSafetyAlert);
            item.Add(btnPrintOddBox);
            item.Add(btnADMMangeAcount);
            item.Add(btnAgeingBalanceReport);
            item.Add(btnADMLogAction);
            item.Add(btnPermisionReport);
            item.Add(btnCycleCountAnalys);
            item.Add(btnWH_MasterDataMaterial);
            item.Add(btnHRVisitorSummary);
            item.Add(btnPURMasterlistSupplier);
            item.Add(btnPURMasterListItem);
            item.Add(btnPURPRManage);
            item.Add(btnPURPOManage);
            item.Add(btnPURMangeRegisItem);
            item.Add(btnPURMasterListItemChangeManage);
            item.Add(btnDashboardKPI);
            item.Add(btnQCCheckingMaterial);
            item.Add(btnQCMaterialNG);
            item.Add(btnQC_ReceiveFG);
            item.Add(btnHRCarRegistration);
            item.Add(btnHRCarRegistrationManage);
            item.Add(btnManagePermissionByUser);
            foreach (BarButtonItem row in item)
            {
                row.Enabled = adoClass.Check_permission(this.Name, row.Name, General_Infor.username);
            }
        }
        private void btnCheckingResult_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Form frm = this.Kiemtratontai(typeof(frmCheckingResult2));
            if (frm != null)
            {
                frm.Activate();
                frm.WindowState = FormWindowState.Maximized;

            }
            else
            {
                frmCheckingResult2 f = new frmCheckingResult2();
                f.MdiParent = this;
                f.Show();
            }
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            Load_permission_Database();
            Load_permission();
            btnDownloadLibrary.PerformClick();
        }
        private void Load_permission_Database()
        {
            string strQry = "Select * from ADM_ToolboxPermission where username=N'" + General_Infor.username + "'";
            conn = new CmCn();
            DataTable dt = conn.ExcuteDataTable(strQry);
            General_Infor.List_permission = new List<ADM_Permission_Entity>();
            if (dt.Rows.Count>0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    ADM_Permission_Entity item = new ADM_Permission_Entity();
                    item.Username = row["username"].ToString();
                    item.Frm_name = row["frm_name"].ToString();
                    item.Toolbox_name = row["toolbox_name"].ToString();
                    General_Infor.List_permission.Add(item);
                }
            }
        }
        private void btnUpdate_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            System.Diagnostics.Process.Start(@"C:\HVN_SYS\Update.exe");
            this.Close();
        }

        private void btnMasterListFG_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Form frm = this.Kiemtratontai(typeof(frmMasterListFG));
            if (frm != null)
            {
                frm.Activate();
                frm.WindowState = FormWindowState.Maximized;
            }
            else
            {
                frmMasterListFG f = new frmMasterListFG();
                f.MdiParent = this;
                f.Show();
            }
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                ADM_LogActivities_Entity item = new ADM_LogActivities_Entity();
                item.User_name = General_Infor.username;
                item.Action = "Log out";
                item.Computer_name = System.Environment.MachineName;
                adoClass = new ADO();
                adoClass.Insert_ADM_LogActivities(item);
            }
            catch (Exception)
            {
                var check_Processes = Process.GetProcesses().Where(pr => pr.ProcessName == "HVN System"); // without '.exe'
                foreach (var process in check_Processes)
                {
                    process.Kill();
                }
            }

            //-------
            //frmLogin frm = new frmLogin("Exit");
            //frm.Show();
            //this.Hide();
        }

        private void btnManagePermission_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmADMManagePermissionByFunction frm = new frmADMManagePermissionByFunction();
            frm.Show();
        }


        private void btnApprovalFG_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Form frm = this.Kiemtratontai(typeof(View.Production.frmApproval));
            if (frm != null)
            {
                frm.Activate();
                frm.WindowState = FormWindowState.Maximized;
            }
            else
            {
                View.Production.frmApproval f = new View.Production.frmApproval();
                f.MdiParent = this;
                f.Show();
            }
        }

        private void btnSignOut_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmLogin frm = new frmLogin();
            frm.Show();
            this.Hide();
        }

        private void btnUploadSoftware_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ProcessStartInfo ps = new ProcessStartInfo();
            ps.FileName = "cmd.exe";
            ps.WindowStyle = ProcessWindowStyle.Hidden;
            ps.Arguments = @"/c robocopy D:\10.C#\01.Debug\01.HVN_SYSTEM \\172.16.180.20\20.Public\05.IT\03.HVN_SYS /MIR";
            Process.Start(ps);
            MessageBox.Show("The software has been uploaded!");
        }

        

        private void btnFGDashBoard_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmDashBoardFG2 frm = new frmDashBoardFG2();
            frm.Show();
        }

        private void btnProductionPlanFG_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form frm = this.Kiemtratontai(typeof(frmProductionPlanFG));
            if (frm != null)
            {
                frm.Activate();
                frm.WindowState = FormWindowState.Maximized;
            }
            else
            {
                frmProductionPlanFG f = new frmProductionPlanFG();
                f.MdiParent = this;
                f.Show();
            }
        }

        private void btnTestFunction_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmWHScanInLocation2 frm = new frmWHScanInLocation2();
            frm.ShowDialog();
            this.Hide();
        }

        private void btnCheckQuantityFinishingDaily_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmCheckQuantityFinishingDaily frm = new frmCheckQuantityFinishingDaily();
            frm.Show();
        }

        private void btnDashboardKPI_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmDashboardPlantKPI frm = new frmDashboardPlantKPI();
            frm.Show();
        }

        private void btnKPIMyAction_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form frm = this.Kiemtratontai(typeof(frmKPIMyAction));
            if (frm != null)
            {
                frm.Activate();
                frm.WindowState = FormWindowState.Maximized;
            }
            else
            {
                frmKPIMyAction f = new frmKPIMyAction();
                f.MdiParent = this;
                f.Show();
            }
        }

        private void btnKPIMyIncident_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form frm = this.Kiemtratontai(typeof(frmKPIMyIncident));
            if (frm != null)
            {
                frm.Activate();
                frm.WindowState = FormWindowState.Maximized;
            }
            else
            {
                frmKPIMyIncident f = new frmKPIMyIncident();
                f.MdiParent = this;
                f.Show();
            }
        }
        private void btnKPIManageIncident_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form frm = this.Kiemtratontai(typeof(frmKPIManageIncident));
            if (frm != null)
            {
                frm.Activate();
                frm.WindowState = FormWindowState.Maximized;
            }
            else
            {
                frmKPIManageIncident f = new frmKPIManageIncident();
                f.MdiParent = this;
                f.Show();
            }
        }

        private void btnManageAction_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form frm = this.Kiemtratontai(typeof(frmKPIManageAction));
            if (frm != null)
            {
                frm.Activate();
                frm.WindowState = FormWindowState.Maximized;
            }
            else
            {
                frmKPIManageAction f = new frmKPIManageAction();
                f.MdiParent = this;
                f.Show();
            }
        }

        private void btnDownloadLibrary_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (General_Infor.username!="wh"&& General_Infor.username != "admin")
            {
                ProcessStartInfo ps = new ProcessStartInfo();
                ps.FileName = "cmd.exe";
                ps.WindowStyle = ProcessWindowStyle.Hidden;
                ps.Arguments = @"robocopy \\172.16.180.20\20.Public\05.IT\03.HVN_SYS C:\HVN_SYS ";
                Process.Start(ps);
                ProcessStartInfo ps2 = new ProcessStartInfo();
                ps2.FileName = "cmd.exe";
                ps2.WindowStyle = ProcessWindowStyle.Hidden;
                ps2.Arguments += @"robocopy \\172.16.180.20\20.Public\05.IT\03.HVN_SYS\01.Format_Excel C:\HVN_SYS\01.Format_Excel ";
                Process.Start(ps2);
            }
        }

        private void btnWHScanProduct_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmWHScaninWaitingZone frm = new frmWHScaninWaitingZone();
            frm.Show();
            this.Hide();
        }

        private void btnWaiting_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form frm = this.Kiemtratontai(typeof(frmWHStockByLocation));
            if (frm != null)
            {
                frm.Activate();
                frm.WindowState = FormWindowState.Maximized;
            }
            else
            {
                frmWHStockByLocation f = new frmWHStockByLocation();
                f.MdiParent = this;
                f.Show();
            }
        }

        private void btnLineStop_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form frm = this.Kiemtratontai(typeof(frmPDManageProdIssue));
            if (frm != null)
            {
                frm.Activate();
                frm.WindowState = FormWindowState.Maximized;
            }
            else
            {
                frmPDManageProdIssue f = new frmPDManageProdIssue();
                f.MdiParent = this;
                f.Show();
            }
        }

        private void btnWHScanLocation_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmWHScanInLocation2 f = new frmWHScanInLocation2();
            f.Show();
            this.Hide();
        }

        private void btnWHScanPacking_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmWHScanInPackingZone2 f = new frmWHScanInPackingZone2();
            f.Show();
            this.Hide();
        }

        private void btnSafetyAlert_ItemClick(object sender, ItemClickEventArgs e)
        {

            Form frm = this.Kiemtratontai(typeof(frmHRSafetyAlert));
            if (frm != null)
            {
                frm.Activate();
                frm.WindowState = FormWindowState.Maximized;
            }
            else
            {
                frmHRSafetyAlert f = new frmHRSafetyAlert();
                f.MdiParent = this;
                f.Show();
            }
        }

        private void btnWHUnpacking_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmWHScanUnpacking f = new frmWHScanUnpacking();
            f.Show();
            this.Hide();
        }

        private void btnWHScanShipping_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmWHScanInShippingTruck f = new frmWHScanInShippingTruck();
            f.Show();
            this.Hide();
        }

        private void btnQCManageGP12_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form frm = this.Kiemtratontai(typeof(frmQCManageGP12));
            if (frm != null)
            {
                frm.Activate();
                frm.WindowState = FormWindowState.Maximized;
            }
            else
            {
                frmQCManageGP12 f = new frmQCManageGP12();
                f.MdiParent = this;
                f.Show();
            }
        }

        private void btnPrintOddBox_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form frm = this.Kiemtratontai(typeof(frmCreateLabel_Temp2));
            if (frm != null)
            {
                frm.Close();
                frmCreateLabel_Temp2 f = new frmCreateLabel_Temp2(true);
                f.MdiParent = this;
                f.Show();
            }
            else
            {
                frmCreateLabel_Temp2 f = new frmCreateLabel_Temp2(true);
                f.MdiParent = this;
                f.Show();
            }
        }

        private void btnManageInvoice_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form frm = this.Kiemtratontai(typeof(frmLOGManageInvoice));
            if (frm != null)
            {
                frm.Activate();
                frm.WindowState = FormWindowState.Maximized;
            }
            else
            {
                frmLOGManageInvoice f = new frmLOGManageInvoice();
                f.MdiParent = this;
                f.Show();
            }
        }

        private void btnADMMangeAcount_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form frm = this.Kiemtratontai(typeof(frmADMManageAccount));
            if (frm != null)
            {
                frm.Activate();
                frm.WindowState = FormWindowState.Maximized;
            }
            else
            {
                frmADMManageAccount f = new frmADMManageAccount();
                f.MdiParent = this;
                f.Show();
            }
        }

        private void btnPUR_PR_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form frm = this.Kiemtratontai(typeof(frmPURPR));
            if (frm != null)
            {
                frm.Activate();
                frm.WindowState = FormWindowState.Maximized;
            }
            else
            {
                frmPURPR f = new frmPURPR();
                f.MdiParent = this;
                f.Show();
            }
        }

        private void btnChangePassword_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmADMChangePassword frm = new frmADMChangePassword();
            frm.Show();
            this.Hide();
        }

        private void btnWHInventoryReport_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form frm = this.Kiemtratontai(typeof(frmWHInventoryReport));
            if (frm != null)
            {
                frm.Activate();
                frm.WindowState = FormWindowState.Maximized;
            }
            else
            {
                frmWHInventoryReport f = new frmWHInventoryReport();
                f.MdiParent = this;
                f.Show();
            }
        }

        private void btnAgeingBalanceReport_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form frm = this.Kiemtratontai(typeof(frmFINAgeingBalanceReport));
            if (frm != null)
            {
                frm.Activate();
                frm.WindowState = FormWindowState.Maximized;
            }
            else
            {
                frmFINAgeingBalanceReport f = new frmFINAgeingBalanceReport();
                f.MdiParent = this;
                f.Show();
            }
        }

        private void btnWHScanRemove_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmWHScanRemoveFromWH f = new frmWHScanRemoveFromWH();
            f.Show();
            this.Hide();
        }

        private void btnWHReprintPalletLabel_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form frm = this.Kiemtratontai(typeof(frmWHReprintPalletLabel));
            if (frm != null)
            {
                frm.Activate();
                frm.WindowState = FormWindowState.Maximized;
            }
            else
            {
                frmWHReprintPalletLabel f = new frmWHReprintPalletLabel();
                f.MdiParent = this;
                f.Show();
            }
        }

        private void btnCycleCount_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form frm = this.Kiemtratontai(typeof(frmWHCCManagement));
            if (frm != null)
            {
                frm.Activate();
                frm.WindowState = FormWindowState.Maximized;
            }
            else
            {
                frmWHCCManagement f = new frmWHCCManagement();
                f.MdiParent = this;
                f.Show();
            }
        }

        private void btnHRAbsenteeism_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form frm = this.Kiemtratontai(typeof(frmHRAbsenteeism));
            if (frm != null)
            {
                frm.Activate();
                frm.WindowState = FormWindowState.Maximized;
            }
            else
            {
                frmHRAbsenteeism f = new frmHRAbsenteeism();
                f.MdiParent = this;
                f.Show();
            }
        }

        private void btnHRKPI_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form frm = this.Kiemtratontai(typeof(frmHRKPI));
            if (frm != null)
            {
                frm.Activate();
                frm.WindowState = FormWindowState.Maximized;
            }
            else
            {
                frmHRKPI f = new frmHRKPI();
                f.MdiParent = this;
                f.Show();
            }
        }

        private void btnCycleCountResult_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form frm = this.Kiemtratontai(typeof(frmWHCCResult));
            if (frm != null)
            {
                frm.Activate();
                frm.WindowState = FormWindowState.Maximized;
            }
            else
            {
                frmWHCCResult f = new frmWHCCResult();
                f.MdiParent = this;
                f.Show();
            }
        }

        private void btnCycleCountAnalys_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form frm = this.Kiemtratontai(typeof(frmWHCCAnalys));
            if (frm != null)
            {
                frm.Activate();
                frm.WindowState = FormWindowState.Maximized;
            }
            else
            {
                frmWHCCAnalys f = new frmWHCCAnalys();
                f.MdiParent = this;
                f.Show();
            }
        }

        private void btnChemicalLabel_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form frm = this.Kiemtratontai(typeof(frmQCChemicalLabel));
            if (frm != null)
            {
                frm.Activate();
                frm.WindowState = FormWindowState.Maximized;
            }
            else
            {
                frmQCChemicalLabel f = new frmQCChemicalLabel();
                f.MdiParent = this;
                f.Show();
            }
        }

        private void btnADMLogAction_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form frm = this.Kiemtratontai(typeof(frmADMLogActivities));
            if (frm != null)
            {
                frm.Activate();
                frm.WindowState = FormWindowState.Maximized;
            }
            else
            {
                frmADMLogActivities f = new frmADMLogActivities();
                f.MdiParent = this;
                f.Show();
            }
        }

        private void btnReprintLabel_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form frm = this.Kiemtratontai(typeof(frmPLA_FG_ReprintLabel));
            if (frm != null)
            {
                frm.Activate();
                frm.WindowState = FormWindowState.Maximized;
            }
            else
            {
                frmPLA_FG_ReprintLabel f = new frmPLA_FG_ReprintLabel();
                f.MdiParent = this;
                f.Show();
            }
        }

        private void btnPermisionReport_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form frm = this.Kiemtratontai(typeof(frmADMPermissionReport));
            if (frm != null)
            {
                frm.Activate();
                frm.WindowState = FormWindowState.Maximized;
            }
            else
            {
                frmADMPermissionReport f = new frmADMPermissionReport();
                f.MdiParent = this;
                f.Show();
            }
        }

        private void btnWHHistoryOfTransaction_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form frm = this.Kiemtratontai(typeof(frmWHScanMagagermentLocation));
            if (frm != null)
            {
                frm.Activate();
                frm.WindowState = FormWindowState.Maximized;
            }
            else
            {
                frmWHScanMagagermentLocation f = new frmWHScanMagagermentLocation();
                f.MdiParent = this;
                f.Show();
            }
        }

        private void btnWHManageLocation_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form frm = this.Kiemtratontai(typeof(frmWHScanManageLocation));
            if (frm != null)
            {
                frm.Activate();
                frm.WindowState = FormWindowState.Maximized;
            }
            else
            {
                frmWHScanManageLocation f = new frmWHScanManageLocation();
                f.MdiParent = this;
                f.Show();
            }
        }

        private void btnIssueMaterial_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form frm = this.Kiemtratontai(typeof(frmWHMaterialIssueToPD));
            if (frm != null)
            {
                frm.Activate();
                frm.WindowState = FormWindowState.Maximized;
            }
            else
            {
                frmWHMaterialIssueToPD f = new frmWHMaterialIssueToPD();
                f.MdiParent = this;
                f.Show();
            }
        }

        private void btnWH_ManageIssueDocument_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form frm = this.Kiemtratontai(typeof(frmWHMaterial_IssueDocument));
            if (frm != null)
            {
                frm.Activate();
                frm.WindowState = FormWindowState.Maximized;
            }
            else
            {
                frmWHMaterial_IssueDocument f = new frmWHMaterial_IssueDocument();
                f.MdiParent = this;
                f.Show();
            }
        }

        private void btnHRVisitorRegistraion_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form frm = this.Kiemtratontai(typeof(frmHR_VisitorRegistration));
            if (frm != null)
            {
                frm.Activate();
                frm.WindowState = FormWindowState.Maximized;
            }
            else
            {
                frmHR_VisitorRegistration f = new frmHR_VisitorRegistration();
                f.MdiParent = this;
                f.Show();
            }
        }

        private void btnHRVisitorSummary_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form frm = this.Kiemtratontai(typeof(frmHR_VisitorRegistration));
            if (frm != null)
            {
                frm.Activate();
                frm.WindowState = FormWindowState.Maximized;
            }
            else
            {
                frmHR_VisitorRegistration f = new frmHR_VisitorRegistration("ViewAll");
                f.MdiParent = this;
                f.Show();
            }
        }

        private void btnWH_MasterDataMaterial_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form frm = this.Kiemtratontai(typeof(frmWHMasterDataMaterial));
            if (frm != null)
            {
                frm.Activate();
                frm.WindowState = FormWindowState.Maximized;
            }
            else
            {
                frmWHMasterDataMaterial f = new frmWHMasterDataMaterial();
                f.MdiParent = this;
                f.Show();
            }

        }

        private void btnWH_ManageReceiveDocument_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form frm = this.Kiemtratontai(typeof(frmWHMaterial_ReceiveDocument));
            if (frm != null)
            {
                frm.Activate();
                frm.WindowState = FormWindowState.Maximized;
            }
            else
            {
                frmWHMaterial_ReceiveDocument f = new frmWHMaterial_ReceiveDocument();
                f.MdiParent = this;
                f.Show();
            }
        }

        private void btnWH_ReceiveMaterial_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmWHMaterialReceivingDetail frm = new frmWHMaterialReceivingDetail();
            frm.Show();
            this.Hide();
        }

        private void btnPLA_M_ReceivingPlan_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form frm = this.Kiemtratontai(typeof(frmPLA_M_ReceivingPlan));
            if (frm != null)
            {
                frm.Activate();
                frm.WindowState = FormWindowState.Maximized;
            }
            else
            {
                frmPLA_M_ReceivingPlan f = new frmPLA_M_ReceivingPlan();
                f.MdiParent = this;
                f.Show();
            }
        }

        private void btnWH_SupplyIQC_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form frm = this.Kiemtratontai(typeof(frmWHMaterialIssueToQuality));
            if (frm != null)
            {
                frm.Activate();
                frm.WindowState = FormWindowState.Maximized;
            }
            else
            {
                frmWHMaterialIssueToQuality f = new frmWHMaterialIssueToQuality();
                f.MdiParent = this;
                f.Show();
            }
        }

        private void btnQCCheckingMaterial_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmQCCheckingMaterial frm = new frmQCCheckingMaterial();
            frm.Show();
        }

        private void btnKPI_QC_SupplierClaim_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form frm = this.Kiemtratontai(typeof(frmKPIQualitySupplierClaim));
            if (frm != null)
            {
                frm.Activate();
                frm.WindowState = FormWindowState.Maximized;
            }
            else
            {
                frmKPIQualitySupplierClaim f = new frmKPIQualitySupplierClaim();
                f.MdiParent = this;
                f.Show();
            }
        }

        private void btnWH_MaterialLocation_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmWHMaterialLocation frm = new frmWHMaterialLocation();
            frm.Show();
            this.Hide();
        }

        private void btnFGBOM_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        private void btnWHMaterialTrackingIssue_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmWHMaterialIssuePending frm = new frmWHMaterialIssuePending();
            frm.Show();
        }

        private void btnQC_ReceiveFG_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmQCFGReceiveFG frm = new frmQCFGReceiveFG();
            frm.ShowDialog();
        }

        private void btnQC_InspectionFG_ItemClick_1(object sender, ItemClickEventArgs e)
        {
            frmQCFGInspection frm = new frmQCFGInspection();
            frm.ShowDialog();
        }

        private void btnWH_MPrintStock_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form frm = this.Kiemtratontai(typeof(frmWHMaterialPrintLabelForStock));
            if (frm != null)
            {
                frm.Activate();
                frm.WindowState = FormWindowState.Maximized;
            }
            else
            {
                frmWHMaterialPrintLabelForStock f = new frmWHMaterialPrintLabelForStock();
                f.MdiParent = this;
                f.Show();
            }
        }

        private void btnScanStockLabel_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmWHMaterialScanForStock frm = new frmWHMaterialScanForStock();
            frm.Show();
            this.Hide();
        }

        private void btnWH_MaterialStock_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form frm = this.Kiemtratontai(typeof(frmWHMaterialStockByLocation));
            if (frm != null)
            {
                frm.Activate();
                frm.WindowState = FormWindowState.Maximized;
            }
            else
            {
                frmWHMaterialStockByLocation f = new frmWHMaterialStockByLocation();
                f.MdiParent = this;
                f.Show();
            }
        }

        private void btnWHMaterialInventory_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form frm = this.Kiemtratontai(typeof(frmWHMaterial_InventoryReport));
            if (frm != null)
            {
                frm.Activate();
                frm.WindowState = FormWindowState.Maximized;
            }
            else
            {
                frmWHMaterial_InventoryReport f = new frmWHMaterial_InventoryReport();
                f.MdiParent = this;
                f.Show();
            }
        }

        private void btnWHMaterialReturnFromPD_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmWHMaterialReturnMaterial f = new frmWHMaterialReturnMaterial();
            f.Show();
        }

        private void btnQC_RemoveFG_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmQCFGRemoveFG frm = new frmQCFGRemoveFG();
            frm.ShowDialog();
        }

        private void btnQCPrintFMBLabel_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form frm = this.Kiemtratontai(typeof(frmQCFMBPrintLabel));
            if (frm != null)
            {
                frm.Activate();
                frm.WindowState = FormWindowState.Maximized;
            }
            else
            {
                frmQCFMBPrintLabel f = new frmQCFMBPrintLabel();
                f.MdiParent = this;
                f.Show();
            }
        }

        private void btnWH_HistoryMaterialTrans_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form frm = this.Kiemtratontai(typeof(frmWHMaterialHistoryOfTransaction));
            if (frm != null)
            {
                frm.Activate();
                frm.WindowState = FormWindowState.Maximized;
            }
            else
            {
                frmWHMaterialHistoryOfTransaction f = new frmWHMaterialHistoryOfTransaction();
                f.MdiParent = this;
                f.Show();
            }
        }

        private void btnQCMaterialNG_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form frm = this.Kiemtratontai(typeof(frmQCMaterialNGPart));
            if (frm != null)
            {
                frm.Activate();
                frm.WindowState = FormWindowState.Maximized;
            }
            else
            {
                frmQCMaterialNGPart f = new frmQCMaterialNGPart();
                f.MdiParent = this;
                f.Show();
            }
        }

        private void btnWH_M_QtyAdjust_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmWHMaterialAdjustQuantity2 f = new frmWHMaterialAdjustQuantity2();
            f.Show();
        }

        private void btnKPIPQuantityProduce_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form frm = this.Kiemtratontai(typeof(frmKPIProductionFGQuantity));
            if (frm != null)
            {
                frm.Activate();
                frm.WindowState = FormWindowState.Maximized;
            }
            else
            {
                frmKPIProductionFGQuantity f = new frmKPIProductionFGQuantity();
                f.MdiParent = this;
                f.Show();
            }
        }

        private void btnWHMCC_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmWHMaterialCCHomePage frm = new frmWHMaterialCCHomePage();
            frm.Show();
        }

        private void btnWHMCCAnalys_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form frm = this.Kiemtratontai(typeof(frmWHMaterialCCAnalys));
            if (frm != null)
            {
                frm.Activate();
                frm.WindowState = FormWindowState.Maximized;
            }
            else
            {
                frmWHMaterialCCAnalys f = new frmWHMaterialCCAnalys();
                f.MdiParent = this;
                f.Show();
            }
        }

        private void btnWHRubberPrintStockLabel_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmWHRubberPrintStockLabel frm = new frmWHRubberPrintStockLabel();
            frm.Show();
        }

        private void btnWHMaterial_WeeklyStock_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form frm = this.Kiemtratontai(typeof(frmWHMaterialStockWeekly));
            if (frm != null)
            {
                frm.Activate();
                frm.WindowState = FormWindowState.Maximized;
            }
            else
            {
                frmWHMaterialStockWeekly f = new frmWHMaterialStockWeekly();
                f.MdiParent = this;
                f.Show();
            }
        }

        private void btnWHRubberIssueToPD_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmWHRubberIssueToPD frm = new frmWHRubberIssueToPD();
            frm.Show();
        }

        private void btnWHRubberReturnFromPD_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmWHRubberReturnFromPD frm = new frmWHRubberReturnFromPD();
            frm.Show();
        }

        private void btnWHMaterialIssueReport_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form frm = this.Kiemtratontai(typeof(frmWHMaterialIssueReport));
            if (frm != null)
            {
                frm.Activate();
                frm.WindowState = FormWindowState.Maximized;
            }
            else
            {
                frmWHMaterialIssueReport f = new frmWHMaterialIssueReport();
                f.MdiParent = this;
                f.Show();
            }
        }

        private void btnWHRubberStock_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form frm = this.Kiemtratontai(typeof(frmWHRubberStockByLocation));
            if (frm != null)
            {
                frm.Activate();
                frm.WindowState = FormWindowState.Maximized;
            }
            else
            {
                frmWHRubberStockByLocation f = new frmWHRubberStockByLocation();
                f.MdiParent = this;
                f.Show();
            }
        }

        private void btnWHFGMaterial_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form frm = this.Kiemtratontai(typeof(frmWHScanWeeklyStockReport));
            if (frm != null)
            {
                frm.Activate();
                frm.WindowState = FormWindowState.Maximized;
            }
            else
            {
                frmWHScanWeeklyStockReport f = new frmWHScanWeeklyStockReport();
                f.MdiParent = this;
                f.Show();
            }
        }

        private void btnWHRubberReceiving_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmWHRubberReceivingDetail frm = new frmWHRubberReceivingDetail();
            frm.Show();
        }

        private void btnWHRubberCycleCount_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmWHRubberCCFullPallet frm = new frmWHRubberCCFullPallet();
            frm.Show();
        }

        private void btnWHRubberCCAnalys_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form frm = this.Kiemtratontai(typeof(frmWHRubberCCAnalys));
            if (frm != null)
            {
                frm.Activate();
                frm.WindowState = FormWindowState.Maximized;
            }
            else
            {
                frmWHRubberCCAnalys f = new frmWHRubberCCAnalys();
                f.MdiParent = this;
                f.Show();
            }
        }

        private void btnWHRubberAdjustWeight_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmWHRubberReturnFromPD frm = new frmWHRubberReturnFromPD("Adjust Weight");
            frm.Show();
        }

        private void btnWHRubberTransaction_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form frm = this.Kiemtratontai(typeof(frmWHRubberHistoryOfTransaction));
            if (frm != null)
            {
                frm.Activate();
                frm.WindowState = FormWindowState.Maximized;
            }
            else
            {
                frmWHRubberHistoryOfTransaction f = new frmWHRubberHistoryOfTransaction();
                f.MdiParent = this;
                f.Show();
            }
        }


        private void btnWHMaterialAgingBalance_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form frm = this.Kiemtratontai(typeof(frmWHMaterialAgeingBalanceReport));
            if (frm != null)
            {
                frm.Activate();
                frm.WindowState = FormWindowState.Maximized;
            }
            else
            {
                frmWHMaterialAgeingBalanceReport f = new frmWHMaterialAgeingBalanceReport();
                f.MdiParent = this;
                f.Show();
            }
        }

        private void btnWHScanMoveLocation_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmWHScanInLocationToPacking frm = new frmWHScanInLocationToPacking();
            frm.Show();
        }

        private void btnPURMasterListItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form frm = this.Kiemtratontai(typeof(frmPURMasterListItem));
            if (frm != null)
            {
                frm.Activate();
                frm.WindowState = FormWindowState.Maximized;
            }
            else
            {
                frmPURMasterListItem f = new frmPURMasterListItem();
                f.MdiParent = this;
                f.Show();
            }
        }

        private void btnPURMasterlistSupplier_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form frm = this.Kiemtratontai(typeof(frmPURMasterListSupplier));
            if (frm != null)
            {
                frm.Activate();
                frm.WindowState = FormWindowState.Maximized;
            }
            else
            {
                frmPURMasterListSupplier f = new frmPURMasterListSupplier();
                f.MdiParent = this;
                f.Show();
            }
        }

        private void btnPUR_ManagePR_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form frm = this.Kiemtratontai(typeof(frmPURPRManage));
            if (frm != null)
            {
                frm.Activate();
                frm.WindowState = FormWindowState.Maximized;
            }
            else
            {
                frmPURPRManage f = new frmPURPRManage();
                f.MdiParent = this;
                f.Show();
            }
        }

        private void btnHR_EmployeeInfor_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form frm = this.Kiemtratontai(typeof(frmHR_EmployeeInfor));
            if (frm != null)
            {
                frm.Activate();
                frm.WindowState = FormWindowState.Maximized;
            }
            else
            {
                frmHR_EmployeeInfor f = new frmHR_EmployeeInfor();
                f.MdiParent = this;
                f.Show();
            }
        }


        private void btnADMChangeSignature_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmADMChangeSignature frm = new frmADMChangeSignature();
            frm.ShowDialog();
        }

        private void btnPURPOManage_ItemClick(object sender, ItemClickEventArgs e)
        {

            Form frm = this.Kiemtratontai(typeof(frmPURPO));
            if (frm != null)
            {
                frm.Activate();
                frm.WindowState = FormWindowState.Maximized;
            }
            else
            {
                frmPURPO f = new frmPURPO();
                f.MdiParent = this;
                f.Show();
            }
        }

        private void btnPURVendorSelection_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form frm = this.Kiemtratontai(typeof(frmPURAddNewItem));
            if (frm != null)
            {
                frm.Activate();
                frm.WindowState = FormWindowState.Maximized;
            }
            else
            {
                frmPURAddNewItem f = new frmPURAddNewItem();
                f.MdiParent = this;
                f.Show();
            }
        }

        private void btnPURMangeRegisItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form frm = this.Kiemtratontai(typeof(frmPURManageRegisItem));
            if (frm != null)
            {
                frm.Activate();
                frm.WindowState = FormWindowState.Maximized;
            }
            else
            {
                frmPURManageRegisItem f = new frmPURManageRegisItem();
                f.MdiParent = this;
                f.Show();
            }
        }

        private void btnPURMasterListItemChange_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form frm = this.Kiemtratontai(typeof(frmPURMasterListItemChange));
            if (frm != null)
            {
                frm.Activate();
                frm.WindowState = FormWindowState.Maximized;
            }
            else
            {
                frmPURMasterListItemChange f = new frmPURMasterListItemChange();
                f.MdiParent = this;
                f.Show();
            }
        }

        private void btnPURMasterListItemHistory_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form frm = this.Kiemtratontai(typeof(frmPURMasterListItemHistory));
            if (frm != null)
            {
                frm.Activate();
                frm.WindowState = FormWindowState.Maximized;
            }
            else
            {
                frmPURMasterListItemHistory f = new frmPURMasterListItemHistory();
                f.MdiParent = this;
                f.Show();
            }
        }

        private void btnPURMasterListItemChangeManage_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form frm = this.Kiemtratontai(typeof(frmPURMasterListItemChangeManage));
            if (frm != null)
            {
                frm.Activate();
                frm.WindowState = FormWindowState.Maximized;
            }
            else
            {
                frmPURMasterListItemChangeManage f = new frmPURMasterListItemChangeManage();
                f.MdiParent = this;
                f.Show();
            }
        }

        private void btnWHRubberAgingBalance_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form frm = this.Kiemtratontai(typeof(frmWHRubberAgeingBalanceReport));
            if (frm != null)
            {
                frm.Activate();
                frm.WindowState = FormWindowState.Maximized;
            }
            else
            {
                frmWHRubberAgeingBalanceReport f = new frmWHRubberAgeingBalanceReport();
                f.MdiParent = this;
                f.Show();
            }
        }

        private void btnManagePermission2_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmADMManageFunction frm = new frmADMManageFunction();
            frm.Show();
        }

        private void btnADMDelegation_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form frm = this.Kiemtratontai(typeof(frmADMDelegation));
            if (frm != null)
            {
                frm.Activate();
                frm.WindowState = FormWindowState.Maximized;
            }
            else
            {
                frmADMDelegation f = new frmADMDelegation();
                f.MdiParent = this;
                f.Show();
            }
        }

        private void btnHRCarRegistration_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form frm = this.Kiemtratontai(typeof(frmHR_CarRegistration));
            if (frm != null)
            {
                frm.Activate();
                frm.WindowState = FormWindowState.Maximized;
            }
            else
            {
                frmHR_CarRegistration f = new frmHR_CarRegistration();
                f.MdiParent = this;
                f.Show();
            }
        }

        private void btnPLA_M_RubberReceivingPlan_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form frm = this.Kiemtratontai(typeof(frmPLA_M_RubberIncomingPlan));
            if (frm != null)
            {
                frm.Activate();
                frm.WindowState = FormWindowState.Maximized;
            }
            else
            {
                frmPLA_M_RubberIncomingPlan f = new frmPLA_M_RubberIncomingPlan();
                f.MdiParent = this;
                f.Show();
            }
        }

        private void btnWHRubberIssueToQC_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form frm = this.Kiemtratontai(typeof(frmWHRubberIssueToQuality));
            if (frm != null)
            {
                frm.Activate();
                frm.WindowState = FormWindowState.Maximized;
            }
            else
            {
                frmWHRubberIssueToQuality f = new frmWHRubberIssueToQuality();
                f.MdiParent = this;
                f.Show();
            }
        }

        private void btnManagePermissionByUser_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form frm = this.Kiemtratontai(typeof(frmADMManagePermissionByUser));
            if (frm != null)
            {
                frm.Activate();
                frm.WindowState = FormWindowState.Maximized;
            }
            else
            {
                frmADMManagePermissionByUser f = new frmADMManagePermissionByUser();
                f.MdiParent = this;
                f.Show();
            }
        }

        private void btnHRCarRegistrationManage_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form frm = this.Kiemtratontai(typeof(frmHR_CarRegistrationManage));
            if (frm != null)
            {
                frm.Activate();
                frm.WindowState = FormWindowState.Maximized;
            }
            else
            {
                frmHR_CarRegistrationManage f = new frmHR_CarRegistrationManage();
                f.MdiParent = this;
                f.Show();
            }
        }
    }
}
