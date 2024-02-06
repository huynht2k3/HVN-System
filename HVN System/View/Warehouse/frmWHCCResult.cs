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
    public partial class frmWHCCResult : Form
    {
        public frmWHCCResult()
        {
            InitializeComponent();
        }
        private ADO adoClass;
        private CmCn conn;
        DataTable dt,dt_Detail;
        bool isStart = true;
        private void btnShow_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (cboCCList.Text=="")
            {
                MessageBox.Show("Please select cycle count name!");
                return;
            }
            string strQry = "SELECT 1 as [Boxes] \n ";
            strQry += "       ,[wh_location] as [Location] \n ";
            strQry += "       ,[pallet_no] as [Pallet No] \n ";
            strQry += "       ,[place] as [Place] \n ";
            strQry += "       ,[product_customer_code] as [Part Number] \n ";
            strQry += "       ,[product_quantity] as [Quantity] \n ";
            strQry += "   FROM [HVN_SYS].[dbo].[W_CycleCountInventory] \n ";
            strQry += "   WHERE cc_name=N'"+cboCCList.Text+"' and isActive=N'1' \n ";
            string strQry2 = "SELECT * FROM W_CycleCountInventory WHERE cc_name=N'" + cboCCList.Text + "' and isActive=N'1'";
            conn = new CmCn();
            try
            {
                dt = new DataTable();
                dt = conn.ExcuteDataTable(strQry);
                pvResult.DataSource = dt;
                if (isStart)
                {
                    pvResult.Fields.Add("Location", DevExpress.XtraPivotGrid.PivotArea.FilterArea);
                    pvResult.Fields.Add("Place", DevExpress.XtraPivotGrid.PivotArea.FilterArea);
                    pvResult.Fields.Add("Pallet No", DevExpress.XtraPivotGrid.PivotArea.FilterArea);
                    pvResult.Fields.Add("Part Number", DevExpress.XtraPivotGrid.PivotArea.RowArea);
                    pvResult.Fields.Add("Quantity", DevExpress.XtraPivotGrid.PivotArea.DataArea);
                    pvResult.Fields.Add("Boxes", DevExpress.XtraPivotGrid.PivotArea.DataArea);
                    isStart = false;
                }
                dt_Detail = new DataTable();
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
            strQry += " where [isActive]=N'1' and isConfirm=N'Confirmed'";
            conn = new CmCn();
            dt_CClist = conn.ExcuteDataTable(strQry);
            cboCCList.Properties.DataSource = dt_CClist;
            cboCCList.Properties.DisplayMember = "CYCLE COUNT NAME";
            cboCCList.Properties.ValueMember = "CYCLE COUNT NAME";
        }

        private void btnExportSumary_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            adoClass = new ADO();
            adoClass.Export_Excel_Pivot(pvResult);
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

        private void dgvResult_Click(object sender, EventArgs e)
        {

        }
    }
}
