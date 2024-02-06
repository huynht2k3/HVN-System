using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HVN_System.Entity;

namespace HVN_System.View.Warehouse
{
    public partial class frmWHMaterialIssuePending : Form
    {
        public frmWHMaterialIssuePending()
        {
            InitializeComponent();
        }
        private CmCn conn;
        private void txtBarcode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode==Keys.Enter)
            {
                lbError.Text = "";
                if (txtBarcode.Text.Substring(2, 4) == "WHOP")
                {
                    txtPIC.Text = txtBarcode.Text.Substring(6, txtBarcode.Text.Length - 6);
                }
                txtBarcode.Text = "";
            }
        }
        private void gvResult_Click(object sender, EventArgs e)
        {
            lbError.Text = "";
            if (txtPIC.Text!="")
            {
                DateTime plant_date;
                string shift, zone;
                plant_date = dtpSupplyDate.Value;
                shift = gvResult.GetRowCellValue(gvResult.FocusedRowHandle, "shift").ToString();
                zone = gvResult.GetRowCellValue(gvResult.FocusedRowHandle, "line_area").ToString();
                frmWHMaterialIssueToPD2 frm = new frmWHMaterialIssueToPD2(plant_date, shift, zone, txtPIC.Text);
                frm.ShowDialog();
            }
            else
            {
                lbError.Text = "LỖI CHƯA QUÉT MÃ NHÂN VIÊN/ ERROR NOT YET SCAN NAME";
            }
        }

        private void frmWHMaterialIssuePending_Load(object sender, EventArgs e)
        {
            Load_Data();
        }
        private void Load_Data()
        {
            try
            {
                conn = new CmCn();
                string strQry = "select a.[shift],b.line_area \n ";
                strQry += "   from PL_PlanFG a,P_MasterListLine b \n ";
                strQry += "   where a.line_no=b.line_name \n ";
                strQry += "   and a.issue_material is null \n ";
                strQry += "   and a.plan_date=N'" + dtpSupplyDate.Value.ToString("yyyy-MM-dd") + "' \n ";
                strQry += "   group by a.[shift],b.line_area \n ";

                DataTable dt = new DataTable();
                dt = conn.ExcuteDataTable(strQry);
                dgvResult.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void btnPrint_Click(object sender, EventArgs e)
        {
            MessageBox.Show("In thành công");
        }

        private void dtpSupplyDate_ValueChanged(object sender, EventArgs e)
        {
            Load_Data();
        }
    }
}
