using DevExpress.XtraGrid.Views.Grid;
using HVN_System.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HVN_System.View.Warehouse
{
    public partial class frmWHCCAnalys : Form
    {
        public frmWHCCAnalys()
        {
            InitializeComponent();
        }
        private ADO adoClass;
        private CmCn conn;
        DataTable dt,dt_Detail;
        private void btnShow_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (cboCCList.Text=="")
            {
                MessageBox.Show("Please select cycle count name!");
                return;
            }
            conn = new CmCn();
           

            string strQry2 = "select * from W_CycleCountResult where cc_name=N'" + cboCCList.Text + "'";

            //---------------------------
            string strQry = "select a.product_customer_code,isnull(b.cc_qty,0) as cc_qty,isnull(b.cc_box_qty,0) as cc_box_qty,isnull(c.sys_qty,0) as sys_qty" +
                ",isnull(c.sys_box_qty,0) as sys_box_qty,isnull(d.cc_pallet_qty,0) as cc_pallet_qty,isnull(e.sys_pallet_qty,0) as sys_pallet_qty from \n ";
            strQry += " (select product_customer_code from W_CycleCountResult  \n ";
            strQry += " where cc_name=N'"+cboCCList.Text+"' group by product_customer_code) a \n ";
            strQry += " left join \n ";
            strQry += " (select product_customer_code, sum(product_quantity) as cc_qty,count (label_code) as cc_box_qty \n ";
            strQry += " from W_CycleCountResult  \n ";
            strQry += " where cc_name=N'"+cboCCList.Text+"' and cc_place not in ('') \n ";
            strQry += " group by product_customer_code) b \n ";
            strQry += " on a.product_customer_code=b.product_customer_code \n ";
            strQry += " left join \n ";
            strQry += " (select product_customer_code, sum(product_quantity) as sys_qty ,count (label_code) as sys_box_qty \n ";
            strQry += " from W_CycleCountResult  \n ";
            strQry += " where cc_name=N'"+cboCCList.Text+"' and sys_place not in ('') \n ";
            strQry += " group by product_customer_code) c \n ";
            strQry += " on a.product_customer_code=c.product_customer_code \n ";
            strQry += " left join \n ";
            strQry += " (select product_customer_code, count(product_customer_code) as cc_pallet_qty from \n ";
            strQry += " (select product_customer_code,cc_pallet_no \n ";
            strQry += " from W_CycleCountResult  \n ";
            strQry += " where cc_name=N'"+cboCCList.Text+"' and cc_place not in ('') and cc_pallet_no not in ('') \n ";
            strQry += " group by product_customer_code,cc_pallet_no) f \n ";
            strQry += " group by product_customer_code) d \n ";
            strQry += " on a.product_customer_code=d.product_customer_code \n ";
            strQry += " left join \n ";
            strQry += " (select product_customer_code, count(product_customer_code) as sys_pallet_qty from \n ";
            strQry += " (select product_customer_code,sys_pallet_no \n ";
            strQry += " from W_CycleCountResult  \n ";
            strQry += " where cc_name=N'"+cboCCList.Text+"' and sys_pallet_no not in ('') \n ";
            strQry += " group by product_customer_code,sys_pallet_no) g \n ";
            strQry += " group by product_customer_code) e \n ";
            strQry += " on a.product_customer_code=e.product_customer_code \n ";

            try
            {
                dt = conn.ExcuteDataTable(strQry);
                dgvSumary.DataSource = dt;
                dt_Detail = conn.ExcuteDataTable(strQry2);
                dgvResult.DataSource = dt_Detail;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void frmWHCCResult_Load(object sender, EventArgs e)
        {
            DataTable dt_CClist = new DataTable();
            string strQry = "select [cc_name] as [CYCLE COUNT NAME],[cc_date] as [CYCLE COUNT DATE],[cc_type] as [CYCLE COUNT TYPE],[cc_des] as [DESCRIPTION] from [W_CycleCount] \n";
            strQry += " ORDER BY cc_date desc";
            dt_Detail = new DataTable();
            conn = new CmCn();
            dt_CClist = conn.ExcuteDataTable(strQry);
            cboCCList.Properties.DataSource = dt_CClist;
            cboCCList.Properties.DisplayMember = "CYCLE COUNT NAME";
            cboCCList.Properties.ValueMember = "CYCLE COUNT NAME";
            adoClass = new ADO();
            btnStockAdjustment.Enabled = adoClass.Check_permission(this.Name, btnStockAdjustment.Name, General_Infor.username);
            btnApprovalAdjustment.Enabled = adoClass.Check_permission(this.Name, btnApprovalAdjustment.Name, General_Infor.username);
        }

        private void btnExportSumary_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            adoClass = new ADO();
            adoClass.Export_Excel(dgvSumary);
        }

        private void btnExportDetail_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            adoClass = new ADO();
            adoClass.Export_Excel(dgvResult);
        }

        private void cboCCList_EditValueChanged(object sender, EventArgs e)
        {
            btnShow.PerformClick();
        }

        private void gvResult_RowStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs e)
        {
            //GridView view = sender as GridView;
            //if (e.RowHandle >= 0)
            //{
            //    string status = view.GetRowCellValue(e.RowHandle, view.Columns["label_status"]).ToString();
            //    switch (status)
            //    {
            //        case "Mismatch place":
            //            e.Appearance.BackColor = Color.Orange;
            //            break;
            //        case "Mismatch location":
            //            e.Appearance.BackColor = Color.Orange;
            //            break;
            //        case "Mismatch location and place":
            //            e.Appearance.BackColor = Color.Orange;
            //            break;
            //        case "Product found in system but not found during cycle count":
            //            e.Appearance.BackColor = Color.Red;
            //            break;
            //        case "Product found in cycle count but not found during system":
            //            e.Appearance.BackColor = Color.Red;
            //            break;
            //        case "OK":
            //            e.Appearance.BackColor = Color.Chartreuse;
            //            break;
            //        default:
            //            break;
            //    }
            //}
        }

        private void btnStockAdjustment_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (dt_Detail.Rows.Count>0)
            {
                frmWHCCAdjustment frm = new frmWHCCAdjustment(dt_Detail, cboCCList.Text,"FG");
                frm.Show();
            }
            else
            {
                MessageBox.Show("Select cycle count name before adjusting");
            }
            
        }

        private void gvResult_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            GridView view = sender as GridView;
            if (e.Column == view.Columns["label_status"])
            {
                string status = view.GetRowCellValue(e.RowHandle, view.Columns["label_status"]).ToString();
                switch (status)
                {
                    case "Mismatch place":
                        e.Appearance.BackColor = Color.Orange;
                        break;
                    case "Mismatch location":
                        e.Appearance.BackColor = Color.Orange;
                        break;
                    case "Mismatch location and place":
                        e.Appearance.BackColor = Color.Orange;
                        break;
                    case "Product found in system but not found during cycle count":
                        e.Appearance.BackColor = Color.Red;
                        break;
                    case "Product found in cycle count but not found during system":
                        e.Appearance.BackColor = Color.Red;
                        break;
                    case "OK":
                        e.Appearance.BackColor = Color.Chartreuse;
                        break;
                    default:
                        break;
                }
            }
            
        }

        private void gvSumary_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            GridView view = sender as GridView;
            if (e.Column == view.Columns["gridColumn10"])
            {
                int value = int.Parse(view.GetRowCellValue(e.RowHandle, view.Columns["gridColumn10"]).ToString());
                if (value != 0)
                {
                    e.Appearance.BackColor = Color.Red;
                }
            }
            if (e.Column == view.Columns["gridColumn15"])
            {
                int value = int.Parse(view.GetRowCellValue(e.RowHandle, view.Columns["gridColumn10"]).ToString());
                if (value != 0)
                {
                    e.Appearance.BackColor = Color.Red;
                }
            }
            if (e.Column == view.Columns["gridColumn18"])
            {
                int value = int.Parse(view.GetRowCellValue(e.RowHandle, view.Columns["gridColumn10"]).ToString());
                if (value != 0)
                {
                    e.Appearance.BackColor = Color.Red;
                }
            }
        }

        private void btnApprovalAdjustment_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (dt_Detail.Rows.Count > 0)
            {
                frmWHCCAdjustmentApprove frm = new frmWHCCAdjustmentApprove(cboCCList.Text, "FG");
                frm.ShowDialog();
            }
            else
            {
                MessageBox.Show("Select cycle count name before adjusting");
            }
            
        }

        private void dgvResult_Click(object sender, EventArgs e)
        {

        }
    }
}
