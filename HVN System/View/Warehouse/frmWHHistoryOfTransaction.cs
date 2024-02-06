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
    public partial class frmWHScanMagagermentLocation : Form
    {
        public frmWHScanMagagermentLocation()
        {
            InitializeComponent();
        }
        private CmCn conn;
        private ADO adoClass;
        private P_Label_Entity Current_Item;
        private void Load_Data(string from, string to)
        {
            string FromDate = from;
            string ToDate=  to;
            conn = new CmCn();
            string field = " select * from W_HistoryOfTransaction\n";
            field += " where input_time>N'" + FromDate + "' and input_time<N'" + ToDate + "' order by label_code,input_time";
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
            dtpFromDate.Value = dtpToDate.Value.AddDays(-1);
            dtpToDate.MinDate = dtpFromDate.Value;
            dtpFromTime.Value = DateTime.Today.AddHours(12);
            dtpToTime.Value = DateTime.Today.AddHours(12);
            dtpToDate.MaxDate = dtpFromDate.Value.AddMonths(1);
            Load_Data(dtpFromDate.Value.ToString("yyyy-MM-dd") + " " + dtpFromTime.Value.ToString("HH:mm:ss"), dtpToDate.Value.ToString("yyyy-MM-dd") + " " + dtpToTime.Value.ToString("HH:mm:ss"));
        }
        private void btnRefresh_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Load_Data(dtpFromDate.Value.ToString("yyyy-MM-dd")+" "+dtpFromTime.Value.ToString("HH:mm:ss"), dtpToDate.Value.ToString("yyyy-MM-dd")+ " " + dtpToTime.Value.ToString("HH:mm:ss"));
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
            string field = " select * from W_HistoryOfTransaction\n";
            field += " where input_time>N'" + DateTime.Now.AddYears(-1).ToString("yyyy-MM-dd") + "' order by label_code,input_time";
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
            string label_code= gvResult.GetRowCellValue(gvResult.FocusedRowHandle, "label_code").ToString();
            string comment = gvResult.GetRowCellValue(gvResult.FocusedRowHandle, "comment").ToString();
            strQry += "update W_HistoryOfTransaction set comment=N'"+ comment + "' where label_code=N'" + label_code + "'";
        }
    }
}
