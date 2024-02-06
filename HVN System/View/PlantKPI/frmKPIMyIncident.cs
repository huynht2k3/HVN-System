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
using System.IO;
using System.Collections;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;

namespace HVN_System.View.Production
{
    public partial class frmKPIMyIncident : Form
    {
        public frmKPIMyIncident()
        {
            InitializeComponent();
        }
        private ADO adoClass;
        private CmCn conn;
        private List<KPI_IncidentMonitoring> List_Incident;
        private KPI_IncidentMonitoring Current_Incident;
        private void Load_My_Incident()
        {
            adoClass = new ADO();
            DataTable dt = adoClass.KPI_Load_KPI_Incident("", "inc_theme=N'Quality'");
            List_Incident = new List<KPI_IncidentMonitoring>();
            foreach (DataRow row in dt.Rows)
            {

                KPI_IncidentMonitoring item = new KPI_IncidentMonitoring();
                item.Inc_name = row["inc_name"].ToString();
                item.Inc_level = row["inc_level"].ToString();
                item.Inc_type = row["inc_type"].ToString();
                item.Inc_theme = row["inc_theme"].ToString();
                item.Inc_des = row["inc_des"].ToString();
                item.Author = row["author"].ToString();
                item.Location = row["location"].ToString();
                item.Created_time = string.IsNullOrEmpty(row["created_time"].ToString()) ? DateTime.Today : DateTime.Parse(row["created_time"].ToString());
                item.Created_for = string.IsNullOrEmpty(row["created_for"].ToString()) ? DateTime.Today : DateTime.Parse(row["created_for"].ToString());
                item.Update_time = string.IsNullOrEmpty(row["update_time"].ToString()) ? DateTime.Today : DateTime.Parse(row["update_time"].ToString());
                item.IsAction = row["isAction"].ToString();
                item.Check_id= row["check_id"].ToString();
                item.Sorting_time = string.IsNullOrEmpty(row["sort_time"].ToString()) ? 0:float.Parse(row["sort_time"].ToString());
                item.Is8D = row["is8D"].ToString();
                item.Inc_customer= row["inc_customer"].ToString();
                item.Inc_status= row["inc_status"].ToString();
                item.Cost= string.IsNullOrEmpty(row["cost"].ToString()) ? 0 : decimal.Parse(row["cost"].ToString());
                item.Sort_ext_cost = string.IsNullOrEmpty(row["sort_ext_cost"].ToString()) ? 0 : decimal.Parse(row["sort_ext_cost"].ToString());
                item.Sort_int_cost = string.IsNullOrEmpty(row["sort_int_cost"].ToString()) ? 0 : decimal.Parse(row["sort_int_cost"].ToString());
                item.Trans_cost = string.IsNullOrEmpty(row["trans_cost"].ToString()) ? 0 : decimal.Parse(row["trans_cost"].ToString());
                item.Other_cost = string.IsNullOrEmpty(row["other_cost"].ToString()) ? 0 : decimal.Parse(row["other_cost"].ToString());
                item.Customer_claim_cost = string.IsNullOrEmpty(row["customer_claim_cost"].ToString()) ? 0 : decimal.Parse(row["customer_claim_cost"].ToString());
                item.Finishing_lot = string.IsNullOrEmpty(row["finishing_lot"].ToString()) ? DateTime.Today : DateTime.Parse(row["finishing_lot"].ToString());
                item.Extrus_lot = string.IsNullOrEmpty(row["extrus_lot"].ToString()) ? DateTime.Today : DateTime.Parse(row["extrus_lot"].ToString());
                item.Finishing_lot_string = string.IsNullOrEmpty(row["finishing_lot"].ToString()) ?"": DateTime.Parse(row["finishing_lot"].ToString()).ToString("dd/MM/yyyy");
                item.Extrus_lot_string = string.IsNullOrEmpty(row["extrus_lot"].ToString()) ? "": DateTime.Parse(row["extrus_lot"].ToString()).ToString("dd/MM/yyyy");
                item.Image_link = row["inc_customer"].ToString();
                List_Incident.Add(item);
                item.Image_link = row["image_link"].ToString();
            }
            dgvIncident.DataSource = List_Incident.ToList();
        }

        private void frmKPIMyAction_Load(object sender, EventArgs e)
        {
            Load_My_Incident();
            Current_Incident = new KPI_IncidentMonitoring();
        }
        
        private void btnRefresh_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Load_My_Incident();
        }

        private void gvIncident_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            Current_Incident = gvIncident.GetRow(gvIncident.FocusedRowHandle) as KPI_IncidentMonitoring;
        }

        private void btnSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (Current_Incident.Inc_name != null)
            {
                frmKPIAddNewIncident frm = new frmKPIAddNewIncident(Current_Incident);
                frm.ShowDialog();
                Load_My_Incident();
            }
            else
            {
                MessageBox.Show("Please select the incident before edit");
            }
        }

        private void btnDelete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (MessageBox.Show("Do you want to delete information?", "Delete item", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (Current_Incident.IsAction=="Yes")
                {
                    MessageBox.Show("You cannot delete incident that has been assigned action");
                }
                else
                {
                    string strQry = "delete from KPI_IncidentMonitoring \n";
                    strQry += " where check_id=N'" + Current_Incident.Check_id + "' \n";
                    conn = new CmCn();
                    try
                    {
                        conn.ExcuteQry(strQry);
                        Load_My_Incident();
                        MessageBox.Show("Delete sucessfully");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }

        private void btnNewIncident_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmKPIAddNewIncident frm = new frmKPIAddNewIncident("QC");
            frm.ShowDialog();
            Load_My_Incident();
        }

        private void gvIncident_DoubleClick(object sender, EventArgs e)
        {
            Current_Incident = gvIncident.GetRow(gvIncident.FocusedRowHandle) as KPI_IncidentMonitoring;
            frmKPIAddNewIncident frm = new frmKPIAddNewIncident(Current_Incident);
            frm.ShowDialog();
            Load_My_Incident();
        }

        private void btnExport_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            adoClass = new ADO();
            adoClass.Export_Excel(dgvIncident);
        }
        Dictionary<string, Image> imageCache = new Dictionary<string, Image>(StringComparer.OrdinalIgnoreCase);
        private void gvIncident_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            if (e.Column.FieldName == "Image" && e.IsGetData)
            {
                GridView view = sender as GridView;
                string fileName = view.GetRowCellValue(view.GetRowHandle(e.ListSourceRowIndex), "Image_link") as string ?? string.Empty;
                if (!imageCache.ContainsKey(fileName))
                {
                    Image img = null;
                    if (File.Exists(fileName))
                        img = Image.FromFile(fileName); 
                    else
                        img = Image.FromFile(@"\\172.16.180.20\20.Public\05.IT\05.HVN_TOOL\ATTACHMENT\no-photo.png");
                    imageCache.Add(fileName, img);
                }
                e.Value = imageCache[fileName];
            }
        }

        private void gvIncident_RowCellClick(object sender, RowCellClickEventArgs e)
        {
            if (e.Column.FieldName == "Image")
            {
                string fileName = gvIncident.GetRowCellValue(gvIncident.FocusedRowHandle, "Image_link").ToString();
                if (File.Exists(fileName))
                {
                    System.Diagnostics.Process.Start(fileName);
                }
                else
                {

                }
            }
        }
    }
}
