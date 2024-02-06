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
    public partial class frmCheckingResult2 : Form
    {
        public frmCheckingResult2()
        {
            InitializeComponent();
        }
        private ADO adoClass;
        private CmCn conn;
        
        private void Load_Data()
        {
            string strQry = "select a.*,h.qty_qc_scan,isnull(g.total_qty,0) as total_qty,isnull(b.target,0) as target,isnull(c.p_qty_ng,0) as qty_ng,isnull(c.p_total_qty,0) as qty_report,   \n ";
            strQry += " round((d.total_min-d.total_min_rest-isnull(e.stop_time,0))/d.total_min,4) as OEE_Avai, \n ";
            strQry += " round(c.OEE_Quality,4) as OEE_Quality,f.qty_entry_wh, \n ";
            strQry += " round(c.OEE_Quality*(d.total_min-d.total_min_rest-isnull(e.stop_time,0))/d.total_min,4) as OEE \n ";
            strQry += " from  \n ";
            strQry += " (select product_customer_code,line,[shift]    \n ";
            strQry += "     from P_Label where lot_no=N'" + dtpSelectDate.Value.ToString("yyyyMMdd") + "'    \n ";
            strQry += "     group by product_customer_code,line,[shift]) as a \n ";
            strQry += "     left join \n ";
            strQry += "     (select product_customer_code,sum(product_quantity) as qty_qc_scan,line,[shift]    \n ";
            strQry += "     from P_Label where lot_no=N'" + dtpSelectDate.Value.ToString("yyyyMMdd") + "'    \n ";
            strQry += "     and patrol_date not in ('')   \n ";
            strQry += "     group by product_customer_code,line,[shift]) as h \n ";
            strQry += "     on a.product_customer_code=h.product_customer_code  \n ";
            strQry += "     and a.shift=h.shift and a.line=h.line \n ";
            strQry += "   left join  \n ";
            strQry += "   (select product_customer_code,sum(product_quantity) as total_qty,line,[shift]   \n ";
            strQry += "   from P_Label where lot_no=N'" + dtpSelectDate.Value.ToString("yyyyMMdd") + "'   \n ";
            strQry += "   and scanned_date not in ('')  \n ";
            strQry += "   group by product_customer_code,line,[shift]) as g  \n ";
            strQry += "   on a.product_customer_code=g.product_customer_code   \n ";
            strQry += "   and a.shift=g.shift and a.line=g.line \n ";
            strQry += " left join \n ";
            strQry += " (select customer_product_code,line_no,shift,[target]  \n ";
            strQry += " from PL_PlanFG where plan_date=N'"+dtpSelectDate.Value.ToString("yyyy-MM-dd")+"') as b \n ";
            strQry += " on a.product_customer_code=b.customer_product_code  \n ";
            strQry += " and a.shift=b.shift and a.line=b.line_no \n ";
            strQry += " left join \n ";
            strQry += " (select *,p_total_qty/(p_total_qty+p_qty_ng) as OEE_Quality from P_ProductionReportSubmit  \n ";
            strQry += " where plan_date=N'"+dtpSelectDate.Value.ToString("yyyy-MM-dd")+"') as c \n ";
            strQry += " on a.product_customer_code=c.product_customer_code  \n ";
            strQry += " and a.shift=c.p_shift and a.line=c.p_line \n ";
            strQry += " left join \n ";
            strQry += " (select * from P_MasterListShift) as d \n ";
            strQry += " on a.shift=d.shift_name \n ";
            strQry += " left join \n ";
            strQry += " (select product_customer_code,p_shift,p_line, \n ";
            strQry += " sum(DATEDIFF(minute,start_time,finish_time)) as stop_time  \n ";
            strQry += " from M_MonitorIssue where plan_date=N'"+dtpSelectDate.Value.ToString("yyyy-MM-dd")+"' \n ";
            strQry += " group by product_customer_code,p_shift,p_line \n ";
            strQry += " ) as e \n ";
            strQry += " on a.product_customer_code=e.product_customer_code  \n ";
            strQry += " and a.shift=e.p_shift and a.line=e.p_line \n ";
            strQry += "  left join  \n ";
            strQry += "   (select a.product_customer_code,b.line,b.shift \n ";
            strQry += "   ,sum(a.product_quantity) as qty_entry_wh \n ";
            strQry += "   from W_HistoryOfTransaction a,P_Label b \n ";
            strQry += "   where a.lot_no=N'" + dtpSelectDate.Value.ToString("yyyyMMdd") + "' and a.label_code=b.label_code \n ";
            strQry += "   and [transaction]=N'[New product] go to Waiting zone' \n ";
            strQry += "   group by a.product_customer_code,b.line,b.shift) f \n ";
            strQry += "   on a.product_customer_code=f.product_customer_code \n ";
            strQry += "   and a.shift=f.shift and a.line=f.line  \n ";

            conn = new CmCn();
            dgvResult.DataSource = conn.ExcuteDataTable(strQry);
        }

        private void btnRefresh_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Load_Data();
        }

        private void gvIncident_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            
        }

        

        private void btnView_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            
        }

        private void btnExport_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            adoClass = new ADO();
            adoClass.Export_Excel(dgvResult);
        }

        private void btnSave_ItemClick_1(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            
        }
        

        private void gvResult_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            P_Label_Entity item_changed = gvResult.GetRow(gvResult.FocusedRowHandle) as P_Label_Entity;
            item_changed.IsEdit = true;
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

        private void btnDelete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            
        }

        private void frmCheckingResult2_Load(object sender, EventArgs e)
        {
            btnRefresh.PerformClick();
        }
    }
}
