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

namespace HVN_System
{
    public partial class frmCreateLabel_Temp2 : DevExpress.XtraEditors.XtraForm
    {
        public frmCreateLabel_Temp2()
        {
            InitializeComponent();
        }
        public frmCreateLabel_Temp2(bool _isPrintOddBox)
        {
            InitializeComponent();
            isPrintOddBox = _isPrintOddBox;
            if (_isPrintOddBox)
            {
                txtQuantity.ReadOnly = false;
                txtFGLine.ReadOnly = false;
                cboSource.Enabled = true;
            }
        }
        private ADO adoClass;
        private CmCn conn;
        string WW, YY;
        //string label_code;
        bool isPrintOddBox = false;
        private P_Label_Entity Current_Label;
        private List<P_Label_Entity> List_data;
        string product_weight, product_price, project_name, customer_name, standard_time,product_rev;//, Shift;
        decimal Qty_label;
        int product_qty_check;

        private bool CheckInfoLabel()
        {
            adoClass = new ADO();
            //DataTable dt = adoClass.Load_MasterListProduct2("",cboProductCode.EditValue.ToString()); -- Lay value selected
            DataTable dt = adoClass.Load_MasterListProduct("", "product_code=N'"+cboProductCode.Text+"'");
            if (dt.Rows.Count == 0)
            {
                MessageBox.Show(" Mã hàng '" + cboProductCode.Text + "' không tồn tại \n Item '" + cboProductCode.Text + "' doesn't exist", "ERROR");
                cboProductCode.Text = "";
                return false;
            }
            else
            {
                txtProductName.Text = dt.Rows[0]["product_name"].ToString();
                txtProdCustCode.Text = dt.Rows[0]["product_customer_code"].ToString();
                txtQuantity.Text = dt.Rows[0]["product_quantity"].ToString();
                product_qty_check = int.Parse(txtQuantity.Text);
                product_weight = dt.Rows[0]["product_weight"].ToString();
                txtFGLine.Text = dt.Rows[0]["product_line"].ToString();
                product_weight = dt.Rows[0]["product_weight"].ToString();
                product_price = dt.Rows[0]["product_price"].ToString();
                project_name = dt.Rows[0]["project_name"].ToString();
                customer_name = dt.Rows[0]["customer_name"].ToString();
                standard_time = dt.Rows[0]["standard_time"].ToString();
                product_rev = dt.Rows[0]["product_rev"].ToString();
                //------- Get value of selected Row in SearchLookupEdit
                GridView view = cboProductCode.Properties.View;
                int rowHandle = view.FocusedRowHandle;
                string fieldName = "SHIFT";
                object value = view.GetRowCellValue(rowHandle, fieldName);
                cboShift.Text = value.ToString();
                object quantity_value = view.GetRowCellValue(rowHandle, "QUANTITY");
                object product_type = view.GetRowCellValue(rowHandle, "PRODUCT_TYPE");
                cboProduct_type.Text = product_type.ToString();
                txtQtyRequest.Text = quantity_value.ToString();
                if (txtQuantity.Text == "0")
                {
                    nmNumberLabel.Value = 1;
                }
                else
                {
                    double result = Math.Ceiling(double.Parse(quantity_value.ToString()) / double.Parse(txtQuantity.Text));
                    nmNumberLabel.Value = Convert.ToInt32(result);
                }
                //----------------------------------------------------------
                product_price = dt.Rows[0]["product_price"].ToString();
                project_name = dt.Rows[0]["project_name"].ToString();
                customer_name = dt.Rows[0]["customer_name"].ToString();
                standard_time = dt.Rows[0]["standard_time"].ToString();
                txtCheckType.Text = dt.Rows[0]["check_type"].ToString();
                txtLotNo.Text = dtpOutdate.Value.ToString("yyyyMMdd");
                cboExpiredDateIn.Focus();
                return true;
            }
        }
        private void Load_Label(string product_code, string Shift, int quantity, string OddOrNormalLabel,string line,string product_type)
        {
            adoClass = new ADO();
            //DataTable dt = adoClass.Load_MasterListProduct2("",cboProductCode.EditValue.ToString()); -- Lay value selected
            DataTable dt = adoClass.Load_MasterListProduct("", "product_code=N'"+ product_code + "'");
            if (dt.Rows.Count == 0)
            {
                MessageBox.Show(" Mã hàng '" + product_code + "' không tồn tại \n Item '" + product_code + "' doesn't exist", "ERROR");
            }
            else
            {
                Current_Label = new P_Label_Entity();
                Current_Label.Product_code = product_code;
                Current_Label.Product_name = dt.Rows[0]["product_name"].ToString();
                Current_Label.Product_customer_code = dt.Rows[0]["product_customer_code"].ToString();
                Current_Label.Product_rev = dt.Rows[0]["product_rev"].ToString();
                Current_Label.Product_weight = float.Parse(dt.Rows[0]["product_weight"].ToString());
                Current_Label.Line = line;
                Current_Label.Product_price = dt.Rows[0]["product_price"].ToString();
                Current_Label.Project_name = dt.Rows[0]["project_name"].ToString();
                Current_Label.Customer_name = dt.Rows[0]["customer_name"].ToString();
                Current_Label.Standard_time = dt.Rows[0]["standard_time"].ToString();
                Current_Label.Product_type = product_type;
                Current_Label.Shift = Shift;
                if (OddOrNormalLabel=="Normal")
                {
                    Current_Label.Product_quantity = int.Parse(dt.Rows[0]["product_quantity"].ToString());
                    decimal prod_qty = Current_Label.Product_quantity;
                    Qty_label = Math.Ceiling(quantity / prod_qty);
                }
                else
                {
                    Current_Label.Product_quantity = int.Parse(txtQuantity.Text);
                    Qty_label = nmNumberLabel.Value;
                }
                Current_Label.Product_price = dt.Rows[0]["product_price"].ToString();
                Current_Label.Project_name = dt.Rows[0]["project_name"].ToString();
                Current_Label.Customer_name = dt.Rows[0]["customer_name"].ToString();
                Current_Label.Check_type = dt.Rows[0]["check_type"].ToString();
                Current_Label.Lot_no = dtpOutdate.Value.ToString("yyyyMMdd");
                Current_Label.Plan_date = dtpOutdate.Value;
                switch (cboExpiredDateIn.Text)
                {
                    case "24h":
                        Current_Label.Expired_date = DateTime.Today.AddDays(2);
                        break;
                    case "48h":
                        Current_Label.Expired_date = DateTime.Today.AddDays(3);
                        break;
                    case "72h":
                        Current_Label.Expired_date = DateTime.Today.AddDays(4);
                        break;
                    case "2PM next day":
                        Current_Label.Expired_date = DateTime.Today.AddHours(38);
                        break;
                    default:
                        Current_Label.Expired_date = DateTime.Today.AddDays(2);
                        break;
                }
                for (int i = 1; i <= Qty_label; i++)
                {
                    Current_Label.Label_code = "FG" + Generate_Label_code().ToString();
                    adoClass.Insert_Label(Current_Label);
                    Print_List_Label(Current_Label);
                    ////printDocument1.DefaultPageSettings.Landscape = true;
                    ////printPreviewDialog1.Document = printDocument1;
                    ////printPreviewDialog1.ShowDialog();
                    //printDocument1.Print();
                }
            }
        }
        private void Load_Combobox()
        {
            adoClass = new ADO();
            cboExpiredDateIn.DataSource = adoClass.Load_Parameter("expired_type");
            cboExpiredDateIn.DisplayMember = "child_name";
            cboExpiredDateIn.ValueMember = "child_name";
            //
            cboFGType.DataSource = adoClass.Load_Parameter("product_type");
            cboFGType.DisplayMember = "child_name";
            cboFGType.ValueMember = "child_name";
            //foreach (DataRow item in dt.Rows)
            //{
            //    txtProductCode.AutoCompleteCustomSource.Add(item["product_code"].ToString());
            //}
        }
        private void Load_Infor_Label()
        {
            cboProductCode.Properties.DataSource = null;
            adoClass = new ADO();
            DataTable dt;
            if (cboSource.Text == "IN TEM THEO KE HOACH SAN XUAT")
            {
                dt = adoClass.LoadProductionPlanFG("product_code as [PRODUCT_CODE],customer_product_code as [PROD CUST CODE],shift as [SHIFT],line_no as [LINE], target as [QUANTITY],product_type as [PRODUCT_TYPE] \n", dtpOutdate.Value.ToString("yyyy-MM-dd"));
            }
            else
            {
                cboShift.Text = DateTime.Now < DateTime.Today.AddHours(18) ? "C1_12H" : "C2_12H";
                dt = adoClass.Load_MasterListProduct("product_code as [PRODUCT_CODE],product_customer_code as [PROD CUST CODE],'" + cboShift.Text + "' as [SHIFT],product_line as LINE,product_quantity as [QUANTITY],N'Normal' as [PRODUCT_TYPE]", "");
            }
            cboProductCode.Properties.DataSource = dt;
            cboProductCode.Properties.DisplayMember = "PRODUCT_CODE";
            cboProductCode.Properties.ValueMember = "PRODUCT_CODE";
        }
        private void frmCreateLabel_Load(object sender, EventArgs e)
        {
            dtpOutdate.Value = DateTime.Now;
            DateTimeFormatInfo dfi = DateTimeFormatInfo.CurrentInfo;
            Calendar myCal = dfi.Calendar;
            WW = myCal.GetWeekOfYear(dtpOutdate.Value, dfi.CalendarWeekRule, dfi.FirstDayOfWeek).ToString();
            YY = DateTime.Now.ToString("yy");
            txtWeekOfYear.Text = WW + "/" + YY;
            Current_Label = new P_Label_Entity();
            cboSource.Text = "IN TEM THEO KE HOACH SAN XUAT";
            Load_Infor_Label();
            Load_Combobox();
            
            adoClass = new ADO();
            adoClass.Create_Grid(gvPrintedLabel, dgvPrintedLabel.Name);
            Load_Grid();
            //---- set printer control for hide printing dialog
            PrintController printController = new StandardPrintController();
            printDocument1.PrintController = printController;
        }
        
        private int Generate_Label_code()
        {
            string Qry = "select max(label_code) from P_Label where len(label_code)>7";
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

        private void btnRefresh_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Load_Infor_Label();
            Load_Grid();
        }

        private void cboProductCode_EditValueChanged(object sender, EventArgs e)
        {
            CheckInfoLabel();
            //MessageBox.Show(cboProductCode.EditValue.ToString());
        }

        private void cboSource_SelectionChangeCommitted(object sender, EventArgs e)
        {
            Load_Infor_Label();
        }

        private void btnPrintAll_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (MessageBox.Show("Accept print all?", "Print All", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                DataTable list_print = adoClass.LoadProductionPlanFG("product_code,customer_product_code,shift,line_no, target \n", dtpOutdate.Value.ToString("yyyy-MM-dd"));
                List_data = new List<P_Label_Entity>();
                foreach (DataRow row in list_print.Rows)
                {
                    string pl_prod_code = row["product_code"].ToString();
                    string pl_shift = row["shift"].ToString();
                    string line = row["line_no"].ToString();
                    string product_type = row["product_type"].ToString();
                    int pl_target = string.IsNullOrEmpty(row["target"].ToString()) ? 0 : int.Parse(row["target"].ToString());
                    Load_Label(pl_prod_code, pl_shift, pl_target, "Normal", line, product_type);
                }
                MessageBox.Show("IN THÀNH CÔNG \nPrint successfully!");
            }
        }

        private void dtpOutdate_ValueChanged(object sender, EventArgs e)
        {
            if (!isPrintOddBox)
            {
                DateTimeFormatInfo dfi = DateTimeFormatInfo.CurrentInfo;
                Calendar myCal = dfi.Calendar;
                WW = myCal.GetWeekOfYear(dtpOutdate.Value, dfi.CalendarWeekRule, dfi.FirstDayOfWeek).ToString();
                YY = dtpOutdate.Value.ToString("yy");
                txtWeekOfYear.Text = WW + "/" + YY;
                txtLotNo.Text = dtpOutdate.Value.ToString("yyyyMMdd");
                txtProdCustCode.Text = "";
                txtFGLine.Text = "";
                txtProductName.Text = "";
                txtQuantity.Text = "";
                txtCheckType.Text = "";
                txtQtyRequest.Text = "";
                Load_Infor_Label();
            }
            else
            {
                DateTimeFormatInfo dfi = DateTimeFormatInfo.CurrentInfo;
                Calendar myCal = dfi.Calendar;
                WW = myCal.GetWeekOfYear(dtpOutdate.Value, dfi.CalendarWeekRule, dfi.FirstDayOfWeek).ToString();
                YY = dtpOutdate.Value.ToString("yy");
                txtWeekOfYear.Text = WW + "/" + YY;
                txtLotNo.Text = dtpOutdate.Value.ToString("yyyyMMdd");
            }
        }
        private void printDocument1_PrintPage(object sender, PrintPageEventArgs e)
        {
            Bitmap bmp;
            MessagingToolkit.QRCode.Codec.QRCodeEncoder encoder = new MessagingToolkit.QRCode.Codec.QRCodeEncoder();
            encoder.QRCodeScale = 2;
            string encoding = string.IsNullOrEmpty(Current_Label.Label_code)?"ERROR": Current_Label.Label_code;
            bmp = encoder.Encode(encoding);
            //e.Graphics.DrawImage(bmp, 0, 0);
            int Start = 10;//Bắt đầu ngang
            int Doc = 20;//Bắt đầu dọc

            e.Graphics.DrawImage(bmp, Doc + 480, Start+8);
            e.Graphics.DrawString(Current_Label.Label_code, new Font("Arial", 11, FontStyle.Regular), Brushes.Black, new Point(Doc + 490, Start + 103));
            e.Graphics.DrawString("Item No.", new Font("Arial", 11, FontStyle.Bold), Brushes.Black, new Point(2, Start + 5));
            if (Current_Label.Product_customer_code.Length>16)
            {
                e.Graphics.DrawString(Current_Label.Product_customer_code, new Font("Arial", 30, FontStyle.Bold), Brushes.Black, new Point(2, Start + 20));
            }
            else
            {
                e.Graphics.DrawString(Current_Label.Product_customer_code, new Font("Arial", 36, FontStyle.Bold), Brushes.Black, new Point(2, Start + 20));
            }
            e.Graphics.DrawString("Rev No.", new Font("Arial", 11, FontStyle.Bold), Brushes.Black, new Point(2, Start + 73));
            e.Graphics.DrawString(Current_Label.Product_rev, new Font("Arial", 18, FontStyle.Bold), Brushes.Black, new Point(2, Start + 88));
            e.Graphics.DrawString("Item Name", new Font("Arial", 11, FontStyle.Bold), Brushes.Black, new Point(2, Start + 125));
            e.Graphics.DrawString(Current_Label.Product_name, new Font("Arial", 22, FontStyle.Bold), Brushes.Black, new Point(2, Start + 140));
            e.Graphics.DrawString("Quantity", new Font("Arial", 11, FontStyle.Bold), Brushes.Black, new Point(2, Start + 185));
            e.Graphics.DrawString(Current_Label.Product_quantity+ " EA", new Font("Arial", 36, FontStyle.Bold), Brushes.Black, new Point(2, Start + 200));
            e.Graphics.DrawString("Out date", new Font("Arial", 11, FontStyle.Bold), Brushes.Black, new Point(2, Start + 265));
            e.Graphics.DrawString(Current_Label.Plan_date.ToString("dd/MM/yyyy"), new Font("Arial", 26, FontStyle.Bold), Brushes.Black, new Point(2, Start + 280));
            e.Graphics.DrawString("Lot No.", new Font("Arial", 11, FontStyle.Bold), Brushes.Black, new Point(Doc + 182, Start + 265));
            e.Graphics.DrawString(txtLotNo.Text, new Font("Arial", 26, FontStyle.Bold), Brushes.Black, new Point(Doc + 182, Start + 280));
            e.Graphics.DrawString("Safety mark", new Font("Arial", 11, FontStyle.Bold), Brushes.Black, new Point(2, Start + 325));
            e.Graphics.DrawString(Current_Label.Shift, new Font("Arial", 11, FontStyle.Bold), Brushes.Black, new Point(2, Start + 360));
            e.Graphics.DrawString("FIFO: WW/YY", new Font("Arial", 12, FontStyle.Bold), Brushes.Black, new Point(Doc + 470, Start + 185));
            e.Graphics.DrawString(WW, new Font("Arial", 72, FontStyle.Bold), Brushes.Black, new Point(Doc + 455, Start + 200));
            e.Graphics.DrawString(YY, new Font("Arial", 72, FontStyle.Bold), Brushes.Black, new Point(Doc + 455, Start + 290));
            //---------------------------------
            e.Graphics.DrawLine(new Pen(Color.Black, 2), 0, Start, 0, Start + 400);
            e.Graphics.DrawLine(new Pen(Color.Black, 2), Doc + 180, Start+ 260, Doc + 180, Start + 320);// ngan cach outdate & Lot
            e.Graphics.DrawLine(new Pen(Color.Black, 2), Doc + 450, Start, Doc + 450, Start + 120);
            e.Graphics.DrawLine(new Pen(Color.Black, 2), Doc + 450, Start+180, Doc + 450, Start + 400);

            //----------------------------------
            e.Graphics.DrawLine(new Pen(Color.Black, 2), 0, Start, Doc + 600, Start);//line 0
            e.Graphics.DrawLine(new Pen(Color.Black, 2), 0, Start + 120, Doc + 600, Start + 120); // line 1
            e.Graphics.DrawLine(new Pen(Color.Black, 2), 0, Start + 180, Doc + 600, Start + 180);//line 2
            e.Graphics.DrawLine(new Pen(Color.Black, 2), 0, Start + 260, Doc + 450, Start + 260);// line 3
            e.Graphics.DrawLine(new Pen(Color.Black, 2), 0, Start + 320, Doc + 450, Start + 320);// line 4
            e.Graphics.DrawLine(new Pen(Color.Black, 2), 0, Start + 400, Doc + 600, Start + 400);// line 5
            //---------
            e.Graphics.DrawLine(new Pen(Color.Black, 5), Doc + 470, Start + 300, Doc + 580, Start + 300);//line 2
        }

        private void nmNumberLabel_ValueChanged(object sender, EventArgs e)
        {
            Qty_label = nmNumberLabel.Value;
        }

        private void btnPrintByShift_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (MessageBox.Show("Accept printby Shift?", "Print By Shift", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                DataTable list_print = adoClass.LoadProductionPlanFG("product_code,customer_product_code,shift,line_no, target,is_print,product_type \n", dtpOutdate.Value.ToString("yyyy-MM-dd"));
                List_data = new List<P_Label_Entity>();
                foreach (DataRow row in list_print.Rows)
                {
                    string pl_shift = row["shift"].ToString();
                    string is_print= row["is_print"].ToString();
                    if (pl_shift==cboShift.Text)
                    {
                        if (is_print=="Yes")
                        {
                            string pl_prod_code = row["product_code"].ToString();
                            string line = row["line_no"].ToString();
                            int pl_target = string.IsNullOrEmpty(row["target"].ToString()) ? 0 : int.Parse(row["target"].ToString());
                            string product_type = row["product_type"].ToString();
                            Load_Label(pl_prod_code, pl_shift, pl_target, "Normal", line, product_type);
                        }
                    }
                }
                MessageBox.Show("IN THÀNH CÔNG \nPrint successfully!");
            }
        }

        private void dgvPrintedLabel_Click(object sender, EventArgs e)
        {

        }

        private void btnAddNew_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridView view = cboProductCode.Properties.View;
            int rowHandle = view.FocusedRowHandle;
            string fieldName = "SHIFT";
            object value = view.GetRowCellValue(rowHandle, fieldName);
            MessageBox.Show(value.ToString());
        }

        private void btnPrintLabel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (string.IsNullOrEmpty(cboProductCode.Text))
            {
                MessageBox.Show("Vui lòng chọn mã hàng trước khi in");
            }
            else
            {
                int qty = string.IsNullOrEmpty(txtQuantity.Text) ? 0 : int.Parse(txtQuantity.Text);
                if (qty> product_qty_check)
                {
                    MessageBox.Show("BẠN KHÔNG THỂ IN TEM VỚI SỐ LƯỢNG NHIỀU HƠN TIÊU CHUẨN. \nSỐ LƯỢNG TIÊU CHUẨN TRÊN HỆ THỐNG LÀ: "+ product_qty_check+"<"+ txtQuantity.Text);
                }
                else
                {
                    try
                    {
                        Load_Label(cboProductCode.Text, cboShift.Text, qty, "ODD", txtFGLine.Text,cboProduct_type.Text);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    nmNumberLabel.Value = 1;
                    MessageBox.Show("IN THÀNH CÔNG \nPrint successfully!");
                }
            }
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
                worksheet.Cells[10, 1] = dtpOutdate.Value.ToString("dd/MM/yyyy");
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
        private void Load_Grid()
        {
            adoClass = new ADO();
            adoClass.Load_Data_Grid(dgvPrintedLabel, "P_Label", "created_date >= N'" + DateTime.Today.ToString("yyyy-MM-dd hh:mm") + "'");
        }
    }
}