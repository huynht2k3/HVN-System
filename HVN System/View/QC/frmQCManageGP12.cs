using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Data.SqlClient;
using System.IO;

namespace HVN_System
{
    public partial class frmQCManageGP12 : DevExpress.XtraEditors.XtraForm
    {
        public frmQCManageGP12()
        {
            InitializeComponent();
        }
        private CmCn conn;
        private ADO adoClass;
        //SqlDataAdapter da;
        //DataTable dt;
        private void frmCheckingResult_Load(object sender, EventArgs e)
        {
            Load_Combobox();
            cboMonth.Text = DateTime.Now.ToString("MMMM");
            cboYear.Text = DateTime.Now.ToString("yyyy");
            Load_Grid();
        }

        private void btnShow_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Load_Grid();
        }
        private void Load_Combobox()
        {
            adoClass = new ADO();
            cboMonth.DataSource = adoClass.Load_Parameter("month_in_year");
            cboMonth.DisplayMember = "child_name";
            cboMonth.ValueMember = "child_id";
            DataTable dt2 = adoClass.Load_Parameter("year");
            cboYear.DataSource = dt2;
            cboYear.DisplayMember = "child_name";
            cboYear.ValueMember = "child_id";
        }
        private void Load_Grid()
        {
            string strQry1 = "select day(plan_date) as [DAY],sum(product_quantity) as QUANTITY from P_Label where patrol_date not in ('') and month(patrol_date)=" + cboMonth.SelectedValue+ " and year(patrol_date)=" + cboYear.SelectedValue + " group by day(plan_date)";
            conn = new CmCn();
            dgvQtyByDay.DataSource = conn.ExcuteDataTable(strQry1);
            //gvResult.BestFitColumns();
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
                        //dgvResult.ExportToXlsx(ExportFilePath);
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
                        XtraMessageBox.Show(msg, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    String msg = "The file could not be saved." + Environment.NewLine + Environment.NewLine + "Path: " + ExportFilePath;
                    XtraMessageBox.Show(msg, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void cboMonth_SelectionChangeCommitted(object sender, EventArgs e)
        {
            Load_Grid();
            dgvQtyByPN.DataSource = null;
        }
        string DAY = "01";
        private void gvQtyByDay_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            DAY= gvQtyByDay.GetRowCellValue(gvQtyByDay.FocusedRowHandle, "DAY").ToString();
            string strQry1 = "select product_customer_code as [PART NUMBER],sum(product_quantity) as QUANTITY,COUNT(product_quantity) as [NUMBER BOX] from P_Label where patrol_date not in ('') and MONTH(patrol_date)=" + cboMonth.SelectedValue+ " and DAY(patrol_date)=" + DAY+ "  and year(patrol_date)=" + cboYear.SelectedValue + " group by product_customer_code";
            conn = new CmCn();
            dgvQtyByPN.DataSource=conn.ExcuteDataTable(strQry1);
        }

        private void gvQtyByPN_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            string PN= gvQtyByPN.GetRowCellValue(gvQtyByPN.FocusedRowHandle, "PART NUMBER").ToString();
            adoClass = new ADO();
            DataTable dt = adoClass.Load_Label_FG_Data("label_code,product_customer_code,product_quantity,plan_date,patrol_date,patrol_op", "patrol_date not in ('') and MONTH(patrol_date)=" + cboMonth.SelectedValue + " and DAY(patrol_date)=" + DAY + " and product_customer_code =N'"+ PN + "' and year(patrol_date)=" + cboYear.SelectedValue);
            dgvResult.DataSource = dt;
        }

        private void cboYear_SelectionChangeCommitted(object sender, EventArgs e)
        {
            Load_Grid();
            dgvQtyByPN.DataSource = null;
        }
    }
}