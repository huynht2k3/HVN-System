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
    public partial class frmWHInventoryReport : Form
    {
        public frmWHInventoryReport()
        {
            InitializeComponent();
        }
        private ADO adoClass;
        private CmCn conn;
        DataTable dt;
        private void frmWHInventoryReport_Load(object sender, EventArgs e)
        {
            string strQry = "select 1 as [Boxes],product_code,pallet_no as [Pallet No],product_customer_code as [Part Number]  \n ";
            strQry += " ,lot_no as [Lot No],product_quantity as [Quantity],wh_location as [Location],place as [Place] \n ";
            strQry += " ,case  \n ";
            strQry += "      when isLock=N'Unblock' then 0 \n ";
            strQry += "      else 1 \n ";
            strQry += " end as [Block Qty] \n ";
            strQry += " from P_Label  \n ";
            strQry += " where place not in ('','Shipped') and date_input_packing_zone is null  \n ";
            strQry += " union all select 1 as [Boxes],product_code,pallet_no as [Pallet No],product_customer_code as [Part Number]  \n ";
            strQry += " ,lot_no as [Lot No],product_quantity as [Quantity],location_packed as [Location],place as [Place] \n ";
            strQry += " ,case  \n ";
            strQry += "      when isLock=N'Unblock' then 0 \n ";
            strQry += "      else 1 \n ";
            strQry += " end as [Block Qty] \n ";
            strQry += " from P_Label  \n ";
            strQry += " where place not in ('Shipped') and date_input_packing_zone not in ('') \n ";

            conn = new CmCn();
            try
            {
                dt = new DataTable();
                dt = conn.ExcuteDataTable(strQry);
                pvResult.DataSource = dt;
                pvResult.Fields.Add("Location", DevExpress.XtraPivotGrid.PivotArea.FilterArea);
                pvResult.Fields.Add("Status", DevExpress.XtraPivotGrid.PivotArea.FilterArea);
                pvResult.Fields.Add("Place", DevExpress.XtraPivotGrid.PivotArea.FilterArea);
                pvResult.Fields.Add("Pallet No", DevExpress.XtraPivotGrid.PivotArea.FilterArea);
                pvResult.Fields.Add("Part Number", DevExpress.XtraPivotGrid.PivotArea.RowArea);
                pvResult.Fields.Add("Lot No", DevExpress.XtraPivotGrid.PivotArea.FilterArea);
                //pvResult.Fields.Add("Lot No", DevExpress.XtraPivotGrid.PivotArea.RowArea);
                pvResult.Fields.Add("Quantity", DevExpress.XtraPivotGrid.PivotArea.DataArea);
                pvResult.Fields.Add("Boxes", DevExpress.XtraPivotGrid.PivotArea.DataArea);
                pvResult.Fields.Add("Block Qty", DevExpress.XtraPivotGrid.PivotArea.DataArea);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void Load_Data()
        {
            string strQry = "select 1 as [Boxes],product_code,pallet_no as [Pallet No],product_customer_code as [Part Number]  \n ";
            strQry += " ,lot_no as [Lot No],product_quantity as [Quantity],wh_location as [Location],place as [Place] \n ";
            strQry += " ,case  \n ";
            strQry += "      when isLock=N'Unblock' then 0 \n ";
            strQry += "      else 1 \n ";
            strQry += " end as [Block Qty] \n ";
            strQry += " from P_Label  \n ";
            strQry += " where place not in ('','Shipped') and date_input_packing_zone is null  \n ";
            strQry += " union all select 1 as [Boxes],product_code,pallet_no as [Pallet No],product_customer_code as [Part Number]  \n ";
            strQry += " ,lot_no as [Lot No],product_quantity as [Quantity],location_packed as [Location],place as [Place] \n ";
            strQry += " ,case  \n ";
            strQry += "      when isLock=N'Unblock' then 0 \n ";
            strQry += "      else 1 \n ";
            strQry += " end as [Block Qty] \n ";
            strQry += " from P_Label  \n ";
            strQry += " where place not in ('Shipped') and date_input_packing_zone not in ('') \n ";
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
