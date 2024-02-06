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
using Outlook = Microsoft.Office.Interop.Outlook;
using System.IO;

namespace HVN_System.View.Warehouse
{
    public partial class frmPDManageProdIssue : Form
    {
        public frmPDManageProdIssue()
        {
            InitializeComponent();
        }
        private ADO adoClass;
        private List<P_MonitorIssue> List_Item;
        private P_MonitorIssue Current_Item;
        private void Load_Data(DateTime FromDate, DateTime ToDate)
        {
            adoClass = new ADO();
            string condition = "start_time>N'" + FromDate.ToString("yyyy-MM-dd") + " 00:00:00" + "' and start_time <N'" + ToDate.ToString("yyyy-MM-dd") + " 23:59:00" + "'";
            DataTable dt = adoClass.Load_Monitor_Issue("DATEDIFF(minute,start_time,finish_time) as duration,issue_id,issue_name,start_time,finish_time,status,location", condition);
            List_Item = new List<P_MonitorIssue>();
            foreach (DataRow row in dt.Rows)
            {
                P_MonitorIssue item = new P_MonitorIssue();
                item.Issue_id = row["issue_id"].ToString();
                item.Issue_name = row["issue_name"].ToString();
                item.Location = row["location"].ToString();
                item.Status = row["status"].ToString();
                item.Start_time = DateTime.Parse( row["start_time"].ToString());
                item.Finish_time = DateTime.Parse(row["finish_time"].ToString());
                item.Duration = float.Parse(row["duration"].ToString());
                List_Item.Add(item);
            }
            dgvResult.DataSource = List_Item.ToList();
        }

        private void frmKPIMyAction_Load(object sender, EventArgs e)
        {
            dtpFromDate.Value = DateTime.Today;
            dtpToDate.Value = DateTime.Today;
            Load_Data(dtpFromDate.Value, dtpToDate.Value);
        }
        private void btnRefresh_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Load_Data(dtpFromDate.Value, dtpToDate.Value);
        }

        private void gvIncident_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            Current_Item = gvResult.GetRow(gvResult.FocusedRowHandle) as P_MonitorIssue;
        }
        private void btnLookup_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Load_Data(dtpFromDate.Value, dtpToDate.Value);
        }

        private void dgvResult_Click(object sender, EventArgs e)
        {

        }

        private void btnExport_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            SaveFileDialog SaveDialog = new SaveFileDialog();
            SaveDialog.Filter = "Excel (.xlsx)|*.xlsx";
            if (SaveDialog.ShowDialog() != DialogResult.Cancel)
            {
                string ExportFilePath = SaveDialog.FileName;
                //Using System.IO;
                string FileExtenstion = Path.GetExtension(ExportFilePath);
                switch (FileExtenstion)
                {
                    case ".xlsx":
                        dgvResult.ExportToXlsx(ExportFilePath);
                        break;
                    default:
                        break;
                }
                if (File.Exists(ExportFilePath))
                {
                    try
                    {
                        //Try to open the file and let windows decide how to open it.
                        System.Diagnostics.Process.Start(ExportFilePath);
                    }
                    catch
                    {
                        String msg = "The file could not be opened." + Environment.NewLine + Environment.NewLine + "Path: " + ExportFilePath;
                        MessageBox.Show(msg, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    String msg = "The file could not be saved." + Environment.NewLine + Environment.NewLine + "Path: " + ExportFilePath;
                    MessageBox.Show(msg, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
