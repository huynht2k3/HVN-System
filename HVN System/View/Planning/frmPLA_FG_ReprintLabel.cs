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
using DevExpress.XtraSplashScreen;
using HVN_System.View.Admin;

namespace HVN_System.View.Planning
{
    public partial class frmPLA_FG_ReprintLabel : Form
    {
        public frmPLA_FG_ReprintLabel()
        {
            InitializeComponent();
        }
        private P_Label_Entity Current_Label;
        private CmCn conn;
        private void btnPrint_Click(object sender, EventArgs e)
        {
            string strQry = "select * from P_label where label_code=N'" + txtBarcode.Text + "'";
            conn = new CmCn();
            DataTable dt = conn.ExcuteDataTable(strQry);
            if (dt.Rows.Count>0)
            {
                SplashScreenManager.ShowForm(this, typeof(frmWaitingForm), true, true, false);
                SplashScreenManager.Default.SetWaitFormCaption("Please wait...");
                Current_Label = new P_Label_Entity();
                Current_Label.Label_code = dt.Rows[0]["label_code"].ToString();
                Current_Label.Product_code = dt.Rows[0]["product_code"].ToString();
                Current_Label.Product_quantity = int.Parse(dt.Rows[0]["product_quantity"].ToString());
                Current_Label.Product_name = dt.Rows[0]["product_name"].ToString();
                Current_Label.Product_customer_code = dt.Rows[0]["product_customer_code"].ToString();
                Current_Label.Product_rev = dt.Rows[0]["product_rev"].ToString();
                Current_Label.Product_weight = float.Parse(dt.Rows[0]["product_weight"].ToString());
                Current_Label.Line = dt.Rows[0]["line"].ToString(); ;
                Current_Label.Product_price = dt.Rows[0]["product_price"].ToString();
                Current_Label.Project_name = dt.Rows[0]["project_name"].ToString();
                Current_Label.Customer_name = dt.Rows[0]["customer_name"].ToString();
                Current_Label.Standard_time = dt.Rows[0]["standard_time"].ToString();
                Current_Label.Product_type = dt.Rows[0]["product_type"].ToString(); ;
                Current_Label.Shift = dt.Rows[0]["shift"].ToString(); ;
                Current_Label.Product_price = dt.Rows[0]["product_price"].ToString();
                Current_Label.Project_name = dt.Rows[0]["project_name"].ToString();
                Current_Label.Customer_name = dt.Rows[0]["customer_name"].ToString();
                Current_Label.Check_type = dt.Rows[0]["check_type"].ToString();
                Current_Label.Lot_no = dt.Rows[0]["lot_no"].ToString();
                Current_Label.Plan_date = DateTime.Parse(dt.Rows[0]["plan_date"].ToString());
                Print_List_Label(Current_Label);
                SplashScreenManager.CloseForm();
            }
            txtBarcode.Text = "";
            txtBarcode.Focus();
        }
        private void Print_List_Label(P_Label_Entity p_Label)
        {
            Excel.Application app;
            Excel.Workbook workbook;
            Excel.Sheets mWorkSheets;
            Excel.Worksheet worksheet;
            string file_name = "PRINT_LABEL";
            string pathExcel = @"C:\HVN_SYS\01.Format_Excel\P_FG_Label.xlsm";

            if (System.IO.File.Exists(pathExcel))
            {
                app = new Excel.Application();
                app.Visible = false;
                app.DisplayAlerts = false;
                workbook = app.Workbooks.Open(pathExcel, 0, false, 5, "", "", false, Excel.XlPlatform.xlWindows, "", true, false, 0, true, false, false);
                mWorkSheets = workbook.Worksheets;
                worksheet = (Excel.Worksheet)mWorkSheets.get_Item("Sheet1");
                worksheet = workbook.ActiveSheet;
                worksheet.Cells[1, 6] = p_Label.Label_code;
                worksheet.Cells[2, 1] = p_Label.Product_customer_code;
                worksheet.Cells[4, 1] = p_Label.Product_rev;
                worksheet.Cells[6, 1] = p_Label.Product_name;
                worksheet.Cells[8, 1] = p_Label.Product_quantity;
                worksheet.Cells[10, 3] = p_Label.Lot_no;
                worksheet.Cells[10, 1] = p_Label.Plan_date.ToString("dd/MM/yyyy");
                DateTimeFormatInfo dfi = DateTimeFormatInfo.CurrentInfo;
                Calendar myCal = dfi.Calendar;
                string WW = myCal.GetWeekOfYear(p_Label.Plan_date, dfi.CalendarWeekRule, dfi.FirstDayOfWeek).ToString();
                string YY = p_Label.Plan_date.ToString("yy");
                worksheet.Cells[8, 6] = WW;
                worksheet.Cells[10, 6] = YY;
                worksheet.Cells[12, 1] = p_Label.Shift + "  |  " + p_Label.Check_type;
                worksheet.Unprotect();
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
            else
            {
                MessageBox.Show("KHÔNG TÌM THẤY FORMAT TEM FG. VUI LÒNG LIÊN HỆ IT");
                return;
            }
        }

        private void txtBarcode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode==Keys.Enter)
            {
                btnPrint.PerformClick();
            }
        }
    }
}
