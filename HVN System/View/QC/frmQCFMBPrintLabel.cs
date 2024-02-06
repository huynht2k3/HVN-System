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
using System.Globalization;
using System.Data.SqlClient;
using Excel = Microsoft.Office.Interop.Excel;
using System.IO;
using HVN_System.Entity;
using System.Drawing.Printing;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;


namespace HVN_System.View.QC
{
    public partial class frmQCFMBPrintLabel : Form
    {
        public frmQCFMBPrintLabel()
        {
            InitializeComponent();
        }
        private ADO adoClass;
        private CmCn conn;
        //int Expiry_month = 0;
        string cart_id;
        private void frmQCChemicalLabel_Load(object sender, EventArgs e)
        {
            Load_Combobox();
            //DateTimeFormatInfo dfi = DateTimeFormatInfo.CurrentInfo;
            //Calendar myCal = dfi.Calendar;
        }
        private void Load_Combobox()
        {
            adoClass = new ADO();
            cboItemNo.Properties.DataSource = adoClass.Load_P_FMB_MasterListRubber("rubber_name as [ITEM NO]", "");
            cboItemNo.Properties.DisplayMember = "ITEM NO";
            cboItemNo.Properties.ValueMember = "ITEM NO";
            cboRubberType.DataSource = adoClass.Load_Parameter_Detail("", "parent_id=N'qc_rubber_type_check'");
            cboRubberType.DisplayMember = "child_des";
            cboRubberType.ValueMember = "child_name";
        }

        private void cboItemNo_EditValueChanged(object sender, EventArgs e)
        {
            //GridView view = cboItemNo.Properties.View;
            //int rowHandle = view.FocusedRowHandle;
            //string fieldName = "EXPIRY MONTHS";
            //object value = view.GetRowCellValue(rowHandle, fieldName);
            //Expiry_month = int.Parse(value.ToString());
        }

        private void dtpLotNo_ValueChanged(object sender, EventArgs e)
        {

        }
        private void Print_List_Label()
        {
            Excel.Application app;
            Excel.Workbook workbook;
            Excel.Sheets mWorkSheets;
            Excel.Worksheet worksheet;
            string file_name = "PRINT_LABEL";
            string pathExcel = @"C:\HVN_SYS\01.Format_Excel\FMB_label.xlsm";

            if (System.IO.File.Exists(pathExcel))
            {
                app = new Excel.Application();
                app.Visible = false;
                app.DisplayAlerts = false;
                workbook = app.Workbooks.Open(pathExcel, 0, false, 5, "", "", false, Excel.XlPlatform.xlWindows, "", true, false, 0, true, false, false);
                mWorkSheets = workbook.Worksheets;
                worksheet = (Excel.Worksheet)mWorkSheets.get_Item("Sheet1");
                worksheet = workbook.ActiveSheet;
                worksheet.Cells[1, 3] = cboItemNo.Text;
                worksheet.Cells[2, 3] = txtQuantity.Text;
                worksheet.Cells[3, 3] = cboMixingDate.Value.ToString("dd/MM/yyyy");
                worksheet.Cells[4, 3] = cboRubberType.SelectedValue;
                worksheet.Cells[1, 4] = cart_id;
                worksheet.Unprotect();
                if (File.Exists("" + file_name.Replace("/", "-") + ".xls"))
                {
                    File.Delete("" + file_name.Replace("/", "-") + ".xls");
                }
                app.DisplayAlerts = false;
                workbook.SaveAs("" + file_name + ".xls", Excel.XlFileFormat.xlAddIn,
                     Type.Missing, Type.Missing, Type.Missing, Type.Missing, Excel.XlSaveAsAccessMode.xlNoChange,
                     Type.Missing, Type.Missing, Type.Missing,
                     Type.Missing, Type.Missing);
                var printerSettings = new PrinterSettings();
                workbook.PrintOutEx(Type.Missing, Type.Missing, Type.Missing, Type.Missing, printerSettings.PrinterName, Type.Missing, Type.Missing, Type.Missing);
                workbook.Close();
                app.Quit();
            }
        }
        private bool Insert_data()
        {
            if (cboItemNo.Text==""||txtQuantity.Text=="")
            {
                return false;
            }
            cart_id = "PFMB" + Generate_Label_code().ToString();
            string strQry = "insert into P_FMB_Label(cart_id,rubber_name,rubber_weight,mixing_date,lab_kind) \n";
            strQry += " values (N'" + cart_id + "',N'" + cboItemNo.Text + "',N'" + txtQuantity.Text+ "',N'" + cboMixingDate.Value.ToString("yyyy-MM-dd") + "',N'" + cboRubberType.SelectedValue + "')";
            try
            {
                conn = new CmCn();
                conn.ExcuteQry(strQry);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        private void btnPrint_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (Insert_data())
            {
                Print_List_Label();
                cboItemNo.Text = "";
                txtQuantity.Text = "";
                MessageBox.Show("Print successfully \nIn thành công");
            }
            else
            {
                MessageBox.Show("Missing data or input wrong data \nLỗi nhập thiếu hoặc sai thông tin!");
            }
        }
        private int Generate_Label_code()
        {
            string Qry = "SELECT MAX(cart_id) FROM P_FMB_Label ";
            conn = new CmCn();
            DataTable dt = conn.ExcuteDataTable(Qry);
            if (dt.Rows[0][0].ToString() != "")
            {
                string max_value = dt.Rows[0][0].ToString().Substring(4, dt.Rows[0][0].ToString().Length - 2);
                return int.Parse(max_value) + 1;
            }
            else
            {
                return 10001;
            }
        }
    }
}
