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
using HVN_System.View.PlantKPI;
using Outlook = Microsoft.Office.Interop.Outlook;
using DevExpress.XtraGrid.Views.BandedGrid;

namespace HVN_System.View.Production
{
    public partial class frmADMPermissionReport : Form
    {
        public frmADMPermissionReport()
        {
            InitializeComponent();
        }
        private CmCn conn;
        private ADO adoClass;

        private void frmKPIMyAction_Load(object sender, EventArgs e)
        {
            Generate_bgv();
            Load_data();
        }

        private void gvAction_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            
        }
        private void Load_data()
        {
            try
            {
                string strQry1 = "Select toolbox_des from ADM_ToolboxOfForm";
                conn = new CmCn();
                DataTable dt = conn.ExcuteDataTable(strQry1);
                string list_toolbox = "";
                foreach (DataRow item in dt.Rows)
                {
                    if (item["toolbox_des"].ToString() != "")
                    {
                        if (list_toolbox == "")
                        {
                            list_toolbox += "[" + item["toolbox_des"].ToString() + "]";
                        }
                        else
                        {
                            list_toolbox += ",[" + item["toolbox_des"].ToString() + "]";
                        }
                    }
                }
                string strQry = "select * from ( \n ";
                strQry += " select b.toolbox_des,a.username as [Username],N'RW' as OK --delete  \n ";
                strQry += " from ADM_ToolboxPermission a, ADM_ToolboxOfForm b \n ";
                strQry += " where a.toolbox_name=b.toolbox_name and a.frm_name=b.frm_name \n ";
                strQry += " ) p \n ";
                strQry += " pivot  \n ";
                strQry += " ( \n ";
                strQry += "     MAX (OK) \n ";
                strQry += "     FOR toolbox_des in (" + list_toolbox + ") \n ";
                strQry += " ) as pvt \n ";
                dgvResult.DataSource = conn.ExcuteDataTable(strQry);
                bgvResult.BestFitColumns();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void Generate_bgv()
        {
            string strQry = "select toolbox_group from ADM_ToolboxOfForm group by toolbox_group";
            conn = new CmCn();
            //--------------------------
            GridBand band1 = new GridBand();
            band1.Caption = "Username";
            bgvResult.Bands.Add(band1);
            DataTable dt = conn.ExcuteDataTable(strQry);
            bgvResult.Columns.AddField("Username");
            BandedGridColumn col1 = new BandedGridColumn();
            col1.FieldName = "Username";
            col1.Visible = true;
            col1.OwnerBand = band1;
            //----------------------------
            foreach (DataRow item in dt.Rows)
            {
                GridBand band = new GridBand();
                band.Caption = item["toolbox_group"].ToString();
                bgvResult.Bands.Add(band);
                string strQry2 = "select toolbox_des from ADM_ToolboxOfForm where toolbox_group=N'" + item["toolbox_group"].ToString() + "' group by toolbox_des";
                DataTable dt2 = conn.ExcuteDataTable(strQry2);
                foreach (DataRow row in dt2.Rows)
                {
                    string fieldName= row["toolbox_des"].ToString();
                    bgvResult.Columns.AddField(fieldName);
                    BandedGridColumn col = new BandedGridColumn();
                    //col.UnboundType = DevExpress.Data.UnboundColumnType.String;
                    col.FieldName = fieldName;
                    col.Visible = true;
                    col.OwnerBand = band;
                }
            }
        }


        private void btnRefresh_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Load_data();
        }


        private void gvAction_DoubleClick(object sender, EventArgs e)
        {
            
        }

        private void btnExport_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            adoClass = new ADO();
            adoClass.Export_Excel(dgvResult);
        }
    }
}
