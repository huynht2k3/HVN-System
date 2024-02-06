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
    public partial class frmWHMaterialCCAnalys : Form
    {
        public frmWHMaterialCCAnalys()
        {
            InitializeComponent();
        }
        private ADO adoClass;
        private CmCn conn;
        DataTable dt,dt_Detail;
        private void btnShow_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            string strQry2 = "SELECT * FROM W_M_CCResult where cc_date=N'"+ cboCcDate.SelectedValue+ "'";
            string strQry = "select m_name,isnull(sum(sys_qty),0) as sys_qty,isnull(sum(cc_qty),0) as cc_qty,  \n ";
            strQry += "  count(sys_place) as sys_qty_box,count(cc_place) as cc_qty_box,count(sys_place)-count(cc_place) as gap_box, \n ";
            strQry += "  isnull(sum(sys_qty),0)-isnull(sum(cc_qty),0) as gap_qty \n ";
            strQry += "  from W_M_CCResult where cc_date=N'" + cboCcDate.SelectedValue + "' \n ";
            strQry += "  group by m_name \n ";

            try
            {
                conn = new CmCn();
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
            adoClass = new ADO();
            btnStockAdjustment.Enabled = adoClass.Check_permission(this.Name, btnStockAdjustment.Name, General_Infor.username);
            btnApprovalAdjustment.Enabled = adoClass.Check_permission(this.Name, btnApprovalAdjustment.Name, General_Infor.username);
            Load_Combobox();
            dt_Detail = new DataTable();
        }

        private void btnExportSumary_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            adoClass = new ADO();
            adoClass.Export_Excel(dgvSumary);
        }
        private void Load_Combobox()
        {
            string strQry = "select cast(cc_date as varchar(12)) as cc_date from W_M_CCResult \n ";
            strQry += " group by cc_date \n ";
            strQry += " order by cc_date \n ";
            conn = new CmCn();
            cboCcDate.DataSource = conn.ExcuteDataTable(strQry);
            cboCcDate.ValueMember = "cc_date";
            cboCcDate.DisplayMember= "cc_date";
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
            if (dt_Detail.Rows.Count > 0)
            {
                frmWHCCAdjustment frm = new frmWHCCAdjustment(dt_Detail, cboCcDate.Text, "Material");
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
                    case "Mismatch quantity":
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
            if (e.Column.Caption == "Gap Qty")
            {
                float value = float.Parse(view.GetRowCellValue(e.RowHandle, view.Columns["gap_qty"]).ToString());
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
                frmWHCCAdjustmentApprove frm = new frmWHCCAdjustmentApprove(cboCcDate.Text, "Material");
                frm.ShowDialog();
            }
            else
            {
                MessageBox.Show("Select cycle count name before adjusting");
            }
        }

        private void cboCcDate_SelectionChangeCommitted(object sender, EventArgs e)
        {
            btnShow.PerformClick();
        }

        private void dgvResult_Click(object sender, EventArgs e)
        {

        }
    }
}
