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

namespace HVN_System.View.Warehouse
{
    public partial class frmWHRubberAgeingBalanceReport : Form
    {
        public frmWHRubberAgeingBalanceReport()
        {
            InitializeComponent();
        }
        private ADO adoClass;
        private CmCn conn;
        DataTable dt;
        private string strQry = "";
        private void frmWHInventoryReport_Load(object sender, EventArgs e)
        {
            strQry += "select 1 as [Pallet],r_name as [Part Number]   \n ";
            strQry += "   ,CASE   \n ";
            strQry += "   WHEN lot_no >= DATEADD(month, -3, getdate()) THEN '3M'   \n ";
            strQry += "   WHEN lot_no>= DATEADD(month, -6, getdate()) and lot_no<DATEADD(month,-3,getdate()) THEN '3M-6M'   \n ";
            strQry += "   WHEN lot_no>= DATEADD(month, -9, getdate()) and lot_no<DATEADD(month,-6,getdate()) THEN '6M-9M'   \n ";
            strQry += "   WHEN lot_no>= DATEADD(month, -12, getdate()) and lot_no<DATEADD(month,-9,getdate()) THEN '9M-12M'   \n ";
            strQry += "   WHEN lot_no is null THEN 'QC NOT CHECK'  \n ";
            strQry += "   ELSE 'MORE THAN 1Y'   \n ";
            strQry += "   END as [Ageing EA(Lot No)]   \n ";
            strQry += "   ,CASE   \n ";
            strQry += "   WHEN created_time >= DATEADD(month, -3, getdate()) THEN '3M'   \n ";
            strQry += "   WHEN created_time>= DATEADD(month, -6, getdate()) and created_time<DATEADD(month,-3,getdate()) THEN '3M-6M'   \n ";
            strQry += "   WHEN created_time>= DATEADD(month, -9, getdate()) and created_time<DATEADD(month,-6,getdate()) THEN '6M-9M'   \n ";
            strQry += "   WHEN created_time>= DATEADD(month, -12, getdate()) and created_time<DATEADD(month,-9,getdate()) THEN '9M-12M'   \n ";
            strQry += "   ELSE 'MORE THAN 1Y'   \n ";
            strQry += "   END as [Ageing EA(Entry WH Date)]   \n ";
            strQry += "   ,lot_no as [Lot No],[Weight],created_time as [Entry Date],place as [Place] from W_M_RubberLabel   \n ";
            strQry += "   where place in ('WH Rubber') and [weight]>0   \n ";
            strQry += "   \n ";

            conn = new CmCn();
            try
            {
                dt = new DataTable();
                dt = conn.ExcuteDataTable(strQry);
                pvResult.DataSource = dt;
                //pvResult.Fields.Add("Location", DevExpress.XtraPivotGrid.PivotArea.FilterArea);
                //pvResult.Fields.Add("Status", DevExpress.XtraPivotGrid.PivotArea.FilterArea);
                pvResult.Fields.Add("Place", DevExpress.XtraPivotGrid.PivotArea.FilterArea);
                //pvResult.Fields.Add("Pallet No", DevExpress.XtraPivotGrid.PivotArea.FilterArea);
                pvResult.Fields.Add("Part Number", DevExpress.XtraPivotGrid.PivotArea.RowArea);
                pvResult.Fields.Add("Lot No", DevExpress.XtraPivotGrid.PivotArea.FilterArea);
                pvResult.Fields.Add("Ageing EA(Lot No)", DevExpress.XtraPivotGrid.PivotArea.FilterArea);
                pvResult.Fields.Add("Ageing EA(Entry WH Date)", DevExpress.XtraPivotGrid.PivotArea.FilterArea);
                //pvResult.Fields.Add("Lot No", DevExpress.XtraPivotGrid.PivotArea.RowArea);
                pvResult.Fields.Add("Weight", DevExpress.XtraPivotGrid.PivotArea.DataArea);
                pvResult.Fields.Add("Pallet", DevExpress.XtraPivotGrid.PivotArea.DataArea);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void Load_Data()
        {
            conn = new CmCn();
            try
            {
                dt = new DataTable();
                dt = conn.ExcuteDataTable(strQry);
                pvResult.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnRefresh_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Load_Data();
        }

        private void btnExport_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            adoClass = new ADO();
            adoClass.Export_Excel_Pivot(pvResult);
        }
    }
}
