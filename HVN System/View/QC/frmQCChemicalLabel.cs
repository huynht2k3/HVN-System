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
    public partial class frmQCChemicalLabel : Form
    {
        public frmQCChemicalLabel()
        {
            InitializeComponent();
        }
        private ADO adoClass;
        private CmCn conn;
        int Expiry_day = 0;
        string ch_label_code;
        private void frmQCChemicalLabel_Load(object sender, EventArgs e)
        {
            Load_Combobox();
            DateTimeFormatInfo dfi = DateTimeFormatInfo.CurrentInfo;
            Calendar myCal = dfi.Calendar;
            txtFIFO.Text = myCal.GetWeekOfYear(DateTime.Today, dfi.CalendarWeekRule, dfi.FirstDayOfWeek).ToString() + "/" + DateTime.Today.ToString("yy");
        }
        private void Load_Combobox()
        {
            adoClass = new ADO();
            cboItemNo.Properties.DataSource = adoClass.Load_W_MasterList_Material("m_name as [ITEM NO],expiry_day as [EXPIRY DAYS]", "m_kind=N'Chemical'");
            cboItemNo.Properties.DisplayMember = "ITEM NO";
            cboItemNo.Properties.ValueMember = "ITEM NO";
        }

        private void cboItemNo_EditValueChanged(object sender, EventArgs e)
        {
            GridView view = cboItemNo.Properties.View;
            int rowHandle = view.FocusedRowHandle;
            string fieldName = "EXPIRY DAYS";
            object value = view.GetRowCellValue(rowHandle, fieldName);
            Expiry_day = int.Parse(value.ToString());
            dtpExpDate.Value = dtpLotNo.Value.AddDays(Expiry_day);
        }

        private void dtpLotNo_ValueChanged(object sender, EventArgs e)
        {
            dtpExpDate.Value = dtpLotNo.Value.AddDays(Expiry_day);
        }
        private void Print_List_Label()
        {
            Excel.Application app;
            Excel.Workbook workbook;
            Excel.Sheets mWorkSheets;
            Excel.Worksheet worksheet;
            string file_name = "PRINT_LABEL";
            string pathExcel = @"C:\HVN_SYS\01.Format_Excel\Chemical_label.xlsm";

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
                worksheet.Cells[3, 3] = DateTime.Today.ToString("yyMMdd");
                worksheet.Cells[4, 3] = dtpLotNo.Value.ToString("yyMMdd");
                worksheet.Cells[5, 3] = dtpExpDate.Value.ToString("yyMMdd");
                worksheet.Cells[6, 3] = txtFIFO.Text;
                worksheet.Cells[1, 4] = ch_label_code;
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
            ch_label_code = "CH" + Generate_Label_code().ToString();
            string strQry = "insert into QC_ChemicalLabel(ch_label_code,item_no,quantity,print_date,lot_no,expiration_date) \n";
            strQry += " values (N'" + ch_label_code + "',N'" + cboItemNo.Text + "',N'" + txtQuantity.Text+ "',getdate(),N'" + dtpLotNo.Value.ToString("yyyy-MM-dd") + "',N'" + dtpExpDate.Value.ToString("yyyy-MM-dd") + "')";
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
            string Qry = "SELECT MAX(ch_label_code) FROM QC_ChemicalLabel ";
            conn = new CmCn();
            DataTable dt = conn.ExcuteDataTable(Qry);
            if (dt.Rows[0][0].ToString() != "")
            {
                string max_value = dt.Rows[0][0].ToString().Substring(2, dt.Rows[0][0].ToString().Length - 2);
                return int.Parse(max_value) + 1;
            }
            else
            {
                return 10001;
            }
        }
    }
}
