using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraSplashScreen;
using HVN_System.Entity;
using HVN_System.Util;
using HVN_System.View.Admin;
using Outlook = Microsoft.Office.Interop.Outlook;

namespace HVN_System.View.Warehouse
{
    public partial class frmWHMaterialHistoryOfTransaction : Form
    {
        public frmWHMaterialHistoryOfTransaction()
        {
            InitializeComponent();
        }
        private CmCn conn;
        private ADO adoClass;
        private P_Label_Entity Current_Item;
        private void Load_Data()
        {
            string strQry = "";
            switch (cboSeachBy.Text)
            {
                case "Duration":
                    strQry += "select * from W_M_HistoryOfTransaction where input_time>=N'" + dtpFrom.Value.ToString("yyyy-MM-dd") + " 00:00:00' and input_time<=N'" + dtpTo.Value.ToString("yyyy-MM-dd") + " 23:59:59'";
                    break;
                case "Part number":
                    strQry += "select * from W_M_HistoryOfTransaction where m_name = N'" + cboPN.Text + "' and YEAR(input_time)=N'" + cboYear.Text + "'";
                    break;
                case "Carton No":
                    strQry += "select * from W_M_HistoryOfTransaction where whmr_code=N'" + txtCartonNo.Text + "'";
                    break;
                default:
                    break;
            }
            if (strQry != "")
            {
                conn = new CmCn();
                DataTable dt = conn.ExcuteDataTable(strQry);
                if (dt.Rows.Count > 100000)
                {
                    MessageBox.Show("Number of row is more than 100,000. The system cannot display", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    dgvResult.DataSource = dt;
                }
            }
        }
        private void Load_combobox()
        {
            string strQry = "Select m_name as [Part number] from W_MasterList_Material where m_kind=N'Material'";
            conn = new CmCn();
            DataTable dt = conn.ExcuteDataTable(strQry);
            cboPN.Properties.DataSource = dt;
            cboPN.Properties.ValueMember = "Part number";
            cboPN.Properties.DisplayMember = "Part number";
        }
        private void frmKPIMyAction_Load(object sender, EventArgs e)
        {
            dtpTo.Value = DateTime.Today;
            dtpFrom.Value = DateTime.Today.AddMonths(-1);
            cboYear.Text = DateTime.Today.Year.ToString();
            cboSeachBy.Text = "Duration";
            layoutControlItem5.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            layoutControlItem6.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            layoutControlItem7.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            Load_combobox();
            Load_Data();
        }
        private void btnRefresh_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Load_Data();
        }

        private void gvIncident_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            Current_Item = gvResult.GetRow(gvResult.FocusedRowHandle) as P_Label_Entity;
        }

        private void btnSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (MessageBox.Show("Do you want to change information?", "Save change", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {

            }
        }

        private void dtpFromDate_ValueChanged(object sender, EventArgs e)
        {
            if (dtpTo.Value < dtpFrom.Value)
            {
                dtpTo.Value = dtpFrom.Value.AddDays(1);
            }
            else if (dtpTo.Value > dtpFrom.Value.AddMonths(3))
            {
                dtpTo.Value = dtpFrom.Value.AddMonths(3);
            }
            dtpTo.MinDate = DateTime.Now.AddYears(-10);
            dtpTo.MaxDate = DateTime.Now.AddYears(10);
            dtpTo.MinDate = dtpFrom.Value;
            dtpTo.MaxDate = dtpFrom.Value.AddMonths(3);
        }

        private void btnExport_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            adoClass = new ADO();
            adoClass.Export_Excel(dgvResult);
        }

        private void btnDownload_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            conn = new CmCn();
            string field = " select * from [W_M_HistoryOfTransaction]\n";
            field += " where input_time>N'" + DateTime.Now.AddYears(-1).ToString("yyyy-MM-dd") + "' order by [whmr_code],input_time";
            DataTable dt_inf = conn.ExcuteDataTable(field);
            dgvDowload.DataSource = dt_inf;
            SplashScreenManager.ShowForm(this, typeof(frmWaitingForm), true, true, false);
            SplashScreenManager.Default.SetWaitFormCaption("Processing data...");
            Thread.Sleep(dt_inf.Rows.Count);
            try
            {
                adoClass = new ADO();
                adoClass.Export_Excel(dgvDowload);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            SplashScreenManager.CloseForm();
        }

        private void btnSave_ItemClick_1(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            conn = new CmCn();
            if (string.IsNullOrEmpty(strQry))
            {
                MessageBox.Show("You have not changed anything!");
            }
            else
            {
                conn = new CmCn();
                try
                {
                    conn.ExcuteQry(strQry);
                    MessageBox.Show("Save successfully");
                    strQry = "";
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void gvResult_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            //string Scale_ID = gvResult.GetRowCellValue(gvResult.FocusedRowHandle, "Ma_trong").ToString();
        }
        string strQry;
        private void gvResult_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            string label_code= gvResult.GetRowCellValue(gvResult.FocusedRowHandle, "whmr_code").ToString();
            string comment = gvResult.GetRowCellValue(gvResult.FocusedRowHandle, "m_note").ToString();
            strQry += "update [W_M_HistoryOfTransaction] set m_note=N'" + comment + "' where [whmr_code]=N'" + label_code + "'";
        }

        private void cboSeachBy_SelectionChangeCommitted(object sender, EventArgs e)
        {
            layoutControlItem1.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            layoutControlItem2.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            layoutControlItem5.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            layoutControlItem6.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            layoutControlItem7.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            switch (cboSeachBy.Text)
            {
                case "Duration":
                    layoutControlItem1.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                    layoutControlItem2.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                    btnRefresh.PerformClick();
                    break;
                case "Part number":
                    layoutControlItem6.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                    layoutControlItem7.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                    break;
                case "Carton No":
                    layoutControlItem5.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                    break;
                default:
                    break;
            }
        }
    }
}
