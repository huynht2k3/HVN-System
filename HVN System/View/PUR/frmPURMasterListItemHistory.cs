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
    public partial class frmPURMasterListItemHistory : Form
    {
        public frmPURMasterListItemHistory()
        {
            InitializeComponent();
        }
        private CmCn conn;
        private ADO adoClass;
        private P_Label_Entity Current_Item;
        private void Load_Data(string from, string to)
        {
            string FromDate = from + " 00:00:00";
            string ToDate=  to + " 23:59:59";
            conn = new CmCn();
            string field = " select * from [PUR_MasterListItem_History]\n";
            field += " where input_time>N'" + FromDate + "' and input_time<N'" + ToDate + "' order by [item_name],input_time";
            DataTable dt = conn.ExcuteDataTable(field);
            if (dt.Rows.Count>100000)
            {
                MessageBox.Show("Number of row is more than 100,000. The system cannot display","ERROR",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
            else
            {
                dgvResult.DataSource = dt;
            }
        }

        private void frmKPIMyAction_Load(object sender, EventArgs e)
        {
            Load_Data(dtpFromDate.Value.ToString("yyyy-MM-dd"), dtpToDate.Value.ToString("yyyy-MM-dd"));
            dtpToDate.MinDate = dtpFromDate.Value;
            dtpToDate.MaxDate = dtpFromDate.Value.AddMonths(1);
        }
        private void btnRefresh_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Load_Data(dtpFromDate.Value.ToString("yyyy-MM-dd"), dtpToDate.Value.ToString("yyyy-MM-dd"));
            strQry = "";
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
            if (dtpToDate.Value < dtpFromDate.Value)
            {
                dtpToDate.Value = dtpFromDate.Value.AddDays(1);
            }
            else if (dtpToDate.Value > dtpFromDate.Value.AddMonths(3))
            {
                dtpToDate.Value = dtpFromDate.Value.AddMonths(3);
            }
            dtpToDate.MinDate = DateTime.Now.AddYears(-10);
            dtpToDate.MaxDate = DateTime.Now.AddYears(10);
            dtpToDate.MinDate = dtpFromDate.Value;
            dtpToDate.MaxDate = dtpFromDate.Value.AddMonths(3);
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
            
        }
    }
}
