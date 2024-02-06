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
    public partial class frmKPIProductionFGQuantity : Form
    {
        public frmKPIProductionFGQuantity()
        {
            InitializeComponent();
        }
        private CmCn conn;
        private ADO adoClass;
        private List<KPI_PD_QtyFG2_Entity> List_data;
        private void Load_Data(string from, string to)
        {
            conn = new CmCn();
            string strQry = "select a.product_customer_code,a.qty,b.standard_time as std_time,b.product_weight as std_weight \n ";
            strQry += " ,a.qty*b.standard_time/3600 as total_time,a.qty*b.product_weight/1000 as total_weight from \n ";
            strQry += " (select product_customer_code,sum(product_quantity) as qty \n ";
            strQry += " from W_HistoryOfTransaction  \n ";
            strQry += " where input_time> N'" + from + "'  \n ";
            strQry += " and input_time< N'" + to + "' \n ";
            strQry += " and [transaction]=N'[New product] go to Waiting zone' \n ";
            strQry += " group by product_customer_code) a, P_MasterListProduct b \n ";
            strQry += " where a.product_customer_code=b.product_customer_code \n ";
            DataTable dt = conn.ExcuteDataTable(strQry);

            List_data = new List<KPI_PD_QtyFG2_Entity>();
            foreach (DataRow row in dt.Rows)
            {
                KPI_PD_QtyFG2_Entity item = new KPI_PD_QtyFG2_Entity();
                item.P_date = dtpFromDate.Value;
                item.Product_customer_code = row["product_customer_code"].ToString();
                item.P_qty =float.Parse(row["qty"].ToString());
                item.Std_time = float.Parse(row["std_time"].ToString());
                item.Std_weight = float.Parse(row["std_weight"].ToString());
                item.Total_time = float.Parse(row["total_time"].ToString());
                item.Total_weight = float.Parse(row["total_weight"].ToString());
                List_data.Add(item);
            }
            dgvResult.DataSource = List_data.ToList();
        }

        private void frmKPIMyAction_Load(object sender, EventArgs e)
        {
            dtpFromDate.Value = dtpToDate.Value.AddDays(-1);
            dtpToDate.MinDate = dtpFromDate.Value;
            dtpFromTime.Value = DateTime.Today.AddHours(9);
            dtpToTime.Value = DateTime.Today.AddHours(9);
            dtpToDate.MaxDate = dtpFromDate.Value.AddMonths(1);
            Load_Data(dtpFromDate.Value.ToString("yyyy-MM-dd") + " " + dtpFromTime.Value.ToString("HH:mm:ss"), dtpToDate.Value.ToString("yyyy-MM-dd") + " " + dtpToTime.Value.ToString("HH:mm:ss"));
        }
        private void btnRefresh_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Load_Data(dtpFromDate.Value.ToString("yyyy-MM-dd")+" "+dtpFromTime.Value.ToString("HH:mm:ss"), dtpToDate.Value.ToString("yyyy-MM-dd")+ " " + dtpToTime.Value.ToString("HH:mm:ss"));
            strQry = "";
        }

        private void btnSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (MessageBox.Show("Do you want to change information?", "Save change", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {

            }
        }

        private void dtpFromDate_ValueChanged(object sender, EventArgs e)
        {
            if (dtpToDate.Value<dtpFromDate.Value)
            {
                dtpToDate.Value = dtpFromDate.Value.AddDays(1);
            }
            else if (dtpToDate.Value> dtpFromDate.Value.AddMonths(3))
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
            KPI_PD_QtyFG2_Entity item_changed = gvResult.GetRow(gvResult.FocusedRowHandle) as KPI_PD_QtyFG2_Entity;
            item_changed.Total_time = item_changed.P_qty * item_changed.Std_time;
            item_changed.Total_weight = item_changed.P_qty * item_changed.Std_weight;
        }

        private void btnSaveReport_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            string strQry = "";
            string P_date = "";
            foreach (KPI_PD_QtyFG2_Entity item in List_data)
            {
                if (string.IsNullOrEmpty(strQry))
                {
                    strQry += "delete from KPI_PD_QtyFG2 where p_date=N'" + item.P_date.ToString("yyyy-MM-dd") + "'\n";
                    strQry += "insert into KPI_PD_QtyFG2(p_date,product_customer_code,p_qty,std_time,std_weight,total_time,total_weight)\n";
                    strQry += "select N'"+item.P_date.ToString("yyyy-MM-dd")+"',N'"+item.Product_customer_code+ "',N'" 
                        + item.P_qty + "',N'" + item.Std_time + "',N'" + item.Std_weight + "',N'" + item.Total_time + "',N'" + item.Total_weight + "'\n";
                    P_date = item.P_date.ToString("dd/MM/yyyy");
                }
                else
                {
                    strQry += "union all select N'" + item.P_date.ToString("yyyy-MM-dd") + "',N'" + item.Product_customer_code + "',N'"
                       + item.P_qty + "',N'" + item.Std_time + "',N'" + item.Std_weight + "',N'" + item.Total_time + "',N'" + item.Total_weight + "'\n";
                }
            }
            conn = new CmCn();
            conn.ExcuteQry(strQry);
            MessageBox.Show("Save the report of "+ P_date + " successfully");
        }
    }
}
