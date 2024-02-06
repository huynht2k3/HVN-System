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
    public partial class frmWHMaterial_InventoryReport : Form
    {
        public frmWHMaterial_InventoryReport()
        {
            InitializeComponent();
        }
        private ADO adoClass;
        private CmCn conn;
        DataTable dt;
        private void frmWHInventoryReport_Load(object sender, EventArgs e)
        {
            string strQry = "select 1 as [Boxes],m_name as [Part Number],quantity as [Quantity],place as [Place], wh_location as [Location], lot_no as [Lot No] \n ";
            strQry += " ,case \n ";
            strQry += "      when qc_okng='OK' then quantity \n ";
            strQry += "      else 0 \n ";
            strQry += "      end as [OK] \n ";
            strQry += " ,case \n ";
            strQry += "      when qc_okng='NG' then quantity \n ";
            strQry += "      else 0 \n ";
            strQry += "      end as [NG] \n ";
            strQry += " ,case \n ";
            strQry += "      when qc_okng is null then quantity \n ";
            strQry += "      else 0 \n ";
            strQry += "      end as [Not checked] \n ";
            strQry += " from W_M_ReceiveLabel \n ";
            strQry += " where place not in ('') and quantity>0 \n ";

            conn = new CmCn();
            try
            {
                dt = new DataTable();
                dt = conn.ExcuteDataTable(strQry);
                pvResult.DataSource = dt;
                pvResult.Fields.Add("Location", DevExpress.XtraPivotGrid.PivotArea.FilterArea);
                pvResult.Fields.Add("Place", DevExpress.XtraPivotGrid.PivotArea.FilterArea);
                pvResult.Fields.Add("OK", DevExpress.XtraPivotGrid.PivotArea.FilterArea);
                pvResult.Fields.Add("NG", DevExpress.XtraPivotGrid.PivotArea.FilterArea);
                pvResult.Fields.Add("Not checked", DevExpress.XtraPivotGrid.PivotArea.FilterArea);
                pvResult.Fields.Add("Part Number", DevExpress.XtraPivotGrid.PivotArea.RowArea);
                pvResult.Fields.Add("Lot No", DevExpress.XtraPivotGrid.PivotArea.FilterArea);
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
            string strQry = "select 1 as [Boxes],m_name as [Part Number],quantity as [Quantity],place as [Place], wh_location as [Location], lot_no as [Lot No] \n ";
            strQry += " ,case \n ";
            strQry += "      when qc_okng='OK' then quantity \n ";
            strQry += "      else 0 \n ";
            strQry += "      end as [OK] \n ";
            strQry += " ,case \n ";
            strQry += "      when qc_okng='NG' then quantity \n ";
            strQry += "      else 0 \n ";
            strQry += "      end as [NG] \n ";
            strQry += " ,case \n ";
            strQry += "      when qc_okng is null then quantity \n ";
            strQry += "      else 0 \n ";
            strQry += "      end as [Not checked] \n ";
            strQry += " from W_M_ReceiveLabel \n ";
            strQry += " where place not in ('') \n ";

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
