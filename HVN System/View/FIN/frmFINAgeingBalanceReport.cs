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
    public partial class frmFINAgeingBalanceReport : Form
    {
        public frmFINAgeingBalanceReport()
        {
            InitializeComponent();
        }
        private ADO adoClass;
        private CmCn conn;
        DataTable dt;
        private void frmWHInventoryReport_Load(object sender, EventArgs e)
        {
            string strQry = "select 1 as [Boxes],product_code,pallet_no as [Pallet No],product_customer_code as [Part Number] \n";
            strQry += ",CASE \n";
            strQry += "WHEN plan_date >= DATEADD(month, -3, getdate()) THEN '3M' \n";
            strQry += "WHEN plan_date>= DATEADD(month, -6, getdate()) and plan_date<DATEADD(month,-3,getdate()) THEN '3M-6M' \n";
            strQry += "WHEN plan_date>= DATEADD(month, -9, getdate()) and plan_date<DATEADD(month,-6,getdate()) THEN '6M-9M' \n";
            strQry += "WHEN plan_date>= DATEADD(month, -12, getdate()) and plan_date<DATEADD(month,-9,getdate()) THEN '9M-12M' \n";
            strQry += "ELSE 'MORE THAN 1Y' \n";
            strQry += "END as [Ageing EA(Production date)] \n";
            strQry += ",CASE \n";
            strQry += "WHEN date_input_wh >= DATEADD(month, -3, getdate()) THEN '3M' \n";
            strQry += "WHEN date_input_wh>= DATEADD(month, -6, getdate()) and date_input_wh<DATEADD(month,-3,getdate()) THEN '3M-6M' \n";
            strQry += "WHEN date_input_wh>= DATEADD(month, -9, getdate()) and date_input_wh<DATEADD(month,-6,getdate()) THEN '6M-9M' \n";
            strQry += "WHEN date_input_wh>= DATEADD(month, -12, getdate()) and date_input_wh<DATEADD(month,-9,getdate()) THEN '9M-12M' \n";
            strQry += "ELSE 'MORE THAN 1Y' \n";
            strQry += "END as [Ageing EA(Entry WH Date)] \n";
            strQry += ",lot_no as [Lot No],product_quantity as [Quantity],wh_location as [Location],place as [Place],isLock as [Status] from P_Label \n";
            strQry += "where place not in ('','Shipped') and date_input_packing_zone is null \n";
            strQry += "union all select 1 as [Boxes],product_code,pallet_no as [Pallet No],product_customer_code as [Part Number] \n";
            strQry += ",CASE \n";
            strQry += "WHEN plan_date >= DATEADD(month, -3, getdate()) THEN '3M' \n";
            strQry += "WHEN plan_date>= DATEADD(month, -6, getdate()) and plan_date<DATEADD(month,-3,getdate()) THEN '3M-6M' \n";
            strQry += "WHEN plan_date>= DATEADD(month, -9, getdate()) and plan_date<DATEADD(month,-6,getdate()) THEN '6M-9M' \n";
            strQry += "WHEN plan_date>= DATEADD(month, -12, getdate()) and plan_date<DATEADD(month,-9,getdate()) THEN '9M-12M' \n";
            strQry += "ELSE 'MORE THAN 1Y' \n";
            strQry += "END as [Ageing EA(Production date)] \n";
            strQry += ",CASE \n";
            strQry += "WHEN date_input_wh >= DATEADD(month, -3, getdate()) THEN '3M' \n";
            strQry += "WHEN date_input_wh>= DATEADD(month, -6, getdate()) and date_input_wh<DATEADD(month,-3,getdate()) THEN '3M-6M' \n";
            strQry += "WHEN date_input_wh>= DATEADD(month, -9, getdate()) and date_input_wh<DATEADD(month,-6,getdate()) THEN '6M-9M' \n";
            strQry += "WHEN date_input_wh>= DATEADD(month, -12, getdate()) and date_input_wh<DATEADD(month,-9,getdate()) THEN '9M-12M' \n";
            strQry += "ELSE 'MORE THAN 1Y' \n";
            strQry += "END as [Ageing EA(Entry WH Date)] \n";
            strQry += ",lot_no as [Lot No],product_quantity as [Quantity],location_packed as [Location],place as [Place],isLock as [Status] from P_Label \n";
            strQry += "where place not in ('Shipped') and date_input_packing_zone not in ('')";
            conn = new CmCn();
            try
            {
                dt = new DataTable();
                dt = conn.ExcuteDataTable(strQry);
                pvResult.DataSource = dt;
                pvResult.Fields.Add("Location", DevExpress.XtraPivotGrid.PivotArea.FilterArea);
                //pvResult.Fields.Add("Status", DevExpress.XtraPivotGrid.PivotArea.FilterArea);
                pvResult.Fields.Add("Place", DevExpress.XtraPivotGrid.PivotArea.FilterArea);
                pvResult.Fields.Add("Pallet No", DevExpress.XtraPivotGrid.PivotArea.FilterArea);
                pvResult.Fields.Add("Part Number", DevExpress.XtraPivotGrid.PivotArea.RowArea);
                pvResult.Fields.Add("Lot No", DevExpress.XtraPivotGrid.PivotArea.FilterArea);
                pvResult.Fields.Add("Ageing EA(Production date)", DevExpress.XtraPivotGrid.PivotArea.FilterArea);
                pvResult.Fields.Add("Ageing EA(Entry WH Date)", DevExpress.XtraPivotGrid.PivotArea.FilterArea);
                //pvResult.Fields.Add("Lot No", DevExpress.XtraPivotGrid.PivotArea.RowArea);
                pvResult.Fields.Add("Quantity", DevExpress.XtraPivotGrid.PivotArea.DataArea);
                pvResult.Fields.Add("Boxes", DevExpress.XtraPivotGrid.PivotArea.DataArea);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void Load_Data()
        {
            string strQry = "select 1 as [Boxes],product_code,pallet_no as [Pallet No],product_customer_code as [Part Number] \n";
            strQry += ",CASE \n";
            strQry += "WHEN plan_date >= DATEADD(month, -3, getdate()) THEN '3M' \n";
            strQry += "WHEN plan_date>= DATEADD(month, -6, getdate()) and plan_date<DATEADD(month,-3,getdate()) THEN '3M-6M' \n";
            strQry += "WHEN plan_date>= DATEADD(month, -9, getdate()) and plan_date<DATEADD(month,-6,getdate()) THEN '6M-9M' \n";
            strQry += "WHEN plan_date>= DATEADD(month, -12, getdate()) and plan_date<DATEADD(month,-9,getdate()) THEN '9M-12M' \n";
            strQry += "ELSE 'MORE THAN 1Y' \n";
            strQry += "END as [Ageing EA(Production date)] \n";
            strQry += ",CASE \n";
            strQry += "WHEN date_input_wh >= DATEADD(month, -3, getdate()) THEN '3M' \n";
            strQry += "WHEN date_input_wh>= DATEADD(month, -6, getdate()) and date_input_wh<DATEADD(month,-3,getdate()) THEN '3M-6M' \n";
            strQry += "WHEN date_input_wh>= DATEADD(month, -9, getdate()) and date_input_wh<DATEADD(month,-6,getdate()) THEN '6M-9M' \n";
            strQry += "WHEN date_input_wh>= DATEADD(month, -12, getdate()) and date_input_wh<DATEADD(month,-9,getdate()) THEN '9M-12M' \n";
            strQry += "ELSE 'MORE THAN 1Y' \n";
            strQry += "END as [Ageing EA(Entry WH Date)] \n";
            strQry += ",lot_no as [Lot No],product_quantity as [Quantity],wh_location as [Location],place as [Place],isLock as [Status] from P_Label \n";
            strQry += "where place not in ('','Shipped') and date_input_packing_zone is null \n";
            strQry += "union all select 1 as [Boxes],product_code,pallet_no as [Pallet No],product_customer_code as [Part Number] \n";
            strQry += ",CASE \n";
            strQry += "WHEN plan_date >= DATEADD(month, -3, getdate()) THEN '3M' \n";
            strQry += "WHEN plan_date>= DATEADD(month, -6, getdate()) and plan_date<DATEADD(month,-3,getdate()) THEN '3M-6M' \n";
            strQry += "WHEN plan_date>= DATEADD(month, -9, getdate()) and plan_date<DATEADD(month,-6,getdate()) THEN '6M-9M' \n";
            strQry += "WHEN plan_date>= DATEADD(month, -12, getdate()) and plan_date<DATEADD(month,-9,getdate()) THEN '9M-12M' \n";
            strQry += "ELSE 'MORE THAN 1Y' \n";
            strQry += "END as [Ageing EA(Production date)] \n";
            strQry += ",CASE \n";
            strQry += "WHEN date_input_wh >= DATEADD(month, -3, getdate()) THEN '3M' \n";
            strQry += "WHEN date_input_wh>= DATEADD(month, -6, getdate()) and date_input_wh<DATEADD(month,-3,getdate()) THEN '3M-6M' \n";
            strQry += "WHEN date_input_wh>= DATEADD(month, -9, getdate()) and date_input_wh<DATEADD(month,-6,getdate()) THEN '6M-9M' \n";
            strQry += "WHEN date_input_wh>= DATEADD(month, -12, getdate()) and date_input_wh<DATEADD(month,-9,getdate()) THEN '9M-12M' \n";
            strQry += "ELSE 'MORE THAN 1Y' \n";
            strQry += "END as [Ageing EA(Entry WH Date)] \n";
            strQry += ",lot_no as [Lot No],product_quantity as [Quantity],location_packed as [Location],place as [Place],isLock as [Status] from P_Label \n";
            strQry += "where place not in ('Shipped') and date_input_packing_zone not in ('')";
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
