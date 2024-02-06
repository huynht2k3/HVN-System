using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HVN_System.Entity;
using HVN_System.Util;
using Outlook = Microsoft.Office.Interop.Outlook;

namespace HVN_System.View.Warehouse
{
    public partial class frmWHScanWeeklyStockReport : Form
    {
        public frmWHScanWeeklyStockReport()
        {
            InitializeComponent();
        }
        private CmCn conn;
        private ADO adoClass;
        private void Load_Data()
        {
            //string strQry = "select * from RPT_W_FG_WeeklyStock where report_date =N'" + dtpReportDate.Value.ToString("yyyy-MM-dd")+"'";

            string strQry = "select a.product_customer_code,a.qty,a.qty_not_count,ISNULL(b.ship_qty,0) as ship_qty, \n ";
            strQry += "   a.qty-a.qty_not_count+ISNULL(b.ship_qty,0) as qty_result  \n ";
            strQry += " from \n ";
            strQry += " (select * from RPT_W_FG_WeeklyStock where report_date =N'" + dtpReportDate.Value.ToString("yyyy-MM-dd")+"') a \n ";
            strQry += " left join \n ";
            strQry += " (select product_customer_code,sum(product_quantity) as ship_qty  \n ";
            strQry += " from W_HistoryOfTransaction where  \n ";
            strQry += " input_time>N'" + dtpReportDate.Value.ToString("yyyy-MM-dd")+" 00:00'  \n ";
            strQry += " and input_time <N'" + dtpReportDate.Value.ToString("yyyy-MM-dd")+" 09:30' \n ";
            strQry += " and [transaction] like N'Shipping%' \n ";
            strQry += " group by product_customer_code)b \n ";
            strQry += " on a.product_customer_code=b.product_customer_code \n ";

            conn = new CmCn();
            DataTable dt = conn.ExcuteDataTable(strQry);
            dgvResult.DataSource = dt;
        }

        private void frmKPIMyAction_Load(object sender, EventArgs e)
        {
            Load_Data();
        }
        private void btnRefresh_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Load_Data();
        }

        private void gvIncident_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {

        }

        private void gvResult_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            
        }

        private void btnCheck_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }

        private void repositoryItemComboBox1_EditValueChanged(object sender, EventArgs e)
        {
            if (gvResult.PostEditor())
            {
                gvResult.UpdateCurrentRow();
            }
        }

        private void btnExport_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            adoClass = new ADO();
            adoClass.Export_Excel(dgvResult);
        }
    }
}
