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
using HVN_System.Entity;
using HVN_System.Util;
using System.Collections.ObjectModel;

namespace HVN_System.View.Planning
{
    public partial class frmProductionPlanFG : Form
    {
        public frmProductionPlanFG()
        {
            InitializeComponent();
        }
        string status;
        DataTable dt;
        private ADO adoClass;
        private List<PL_PlanFG_Entity> List_Data;
        private void btnOpen_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            OpenFileDialog OpenFile = new OpenFileDialog();
            OpenFile.Title = "Mở tệp tin";
            OpenFile.Filter = "Excel (.xlsx)|*.xlsx";
            if (OpenFile.ShowDialog() != DialogResult.Cancel)
            {
                string FilePath = OpenFile.FileName;
                dt = new DataTable();
                try
                {
                    adoClass = new ADO();
                    dt = adoClass.ReadExcelFile("INPUT", FilePath);
                    List_Data = new List<PL_PlanFG_Entity>();
                    int count = 1;
                    foreach (DataRow row in dt.Rows)
                    {
                        PL_PlanFG_Entity item = new PL_PlanFG_Entity();
                        item.Plan_date = string.IsNullOrEmpty(row["DATE"].ToString()) ? DateTime.Today : DateTime.Parse(row["DATE"].ToString());
                        if (item.Plan_date>=DateTime.Today)
                        {
                            item.Shift = row["SHIFT"].ToString();
                            item.Line_no = row["LINE NO"].ToString().ToUpper();
                            item.Product_code = row["PRODUCT CODE"].ToString();
                            item.Customer_product_code = row["CUSTOMER PRODUCT CODE"].ToString();
                            item.Number_operator = string.IsNullOrEmpty(row["NUMBER OPERATOR"].ToString()) ? 0 : int.Parse(row["NUMBER OPERATOR"].ToString());
                            item.Target = string.IsNullOrEmpty(row["TARGET"].ToString()) ? 0 : int.Parse(row["TARGET"].ToString());
                            item.Start_time = string.IsNullOrEmpty(row["IMPORT START"].ToString()) ? DateTime.Today.AddHours(6) : DateTime.Parse(row["IMPORT START"].ToString());
                            item.End_time = string.IsNullOrEmpty(row["IMPORT END"].ToString()) ? DateTime.Today.AddHours(6) : DateTime.Parse(row["IMPORT END"].ToString());
                            item.Note = string.IsNullOrEmpty(row["NOTE"].ToString()) ? "" : row["NOTE"].ToString();
                            item.Product_type= string.IsNullOrEmpty(row["PRODUCT TYPE"].ToString()) ? "" : row["PRODUCT TYPE"].ToString();
                            item.Is_print= row["PRINT"].ToString();
                            item.Check_id = count;
                            List_Data.Add(item);
                            count++;
                        }
                    }
                    dgvProdPlan.DataSource = List_Data.ToList();
                    status = "isImport";
                    //var Data_Filter = List_Data.Where(s => s.Plan_date >= DateTime.Today).Select(s => s);
                    //dgvProdPlan.DataSource = Data_Filter.ToList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        private void Load_Plan()
        {
            adoClass = new ADO();
            List_Data = new List<PL_PlanFG_Entity>();
            try
            {
                dt = new DataTable();
                dt = adoClass.Load_PL_PlanFG("", "plan_date>=N'2023-01-01'");
                foreach (DataRow row in dt.Rows)
                {
                    PL_PlanFG_Entity item = new PL_PlanFG_Entity();
                    item.Plan_date = string.IsNullOrEmpty(row["plan_date"].ToString()) ? DateTime.Today : DateTime.Parse(row["plan_date"].ToString());
                    item.Shift = row["shift"].ToString();
                    item.Line_no = row["line_no"].ToString();
                    item.Product_code = row["product_code"].ToString();
                    item.Customer_product_code = row["customer_product_code"].ToString();
                    item.Number_operator = string.IsNullOrEmpty(row["number_operator"].ToString()) ? 0 : int.Parse(row["number_operator"].ToString());
                    item.Target = string.IsNullOrEmpty(row["target"].ToString()) ? 0 : int.Parse(row["target"].ToString());
                    item.Start_time = string.IsNullOrEmpty(row["start_time"].ToString()) ? DateTime.Today.AddHours(6) : DateTime.Parse(row["start_time"].ToString());
                    item.End_time = string.IsNullOrEmpty(row["end_time"].ToString()) ? DateTime.Today.AddHours(6) : DateTime.Parse(row["end_time"].ToString());
                    item.Note = string.IsNullOrEmpty(row["note"].ToString()) ? "" : row["note"].ToString();
                    item.Is_print = row["is_print"].ToString();
                    item.Product_type = row["product_type"].ToString();
                    item.Check_id = int.Parse(row["check_id"].ToString());
                    List_Data.Add(item);
                }
                dgvProdPlan.DataSource = List_Data.ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void btnEdit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }

        private void btnSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            adoClass = new ADO();
            if (status=="isImport")
            {
                if (List_Data.Count > 0)
                {
                    adoClass.SaveProductionPlanFG(List_Data, DateTime.Today);
                    MessageBox.Show("SAVE SUCCESSFULLY!");
                }
                else
                {
                    MessageBox.Show("ERROR: NO DATA. PLEASE CHECK");
                }
            }
            status = "isNone";
        }

        private void btnRefresh_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Load_Plan();
        }

        private void frmProductionPlanFG_Load(object sender, EventArgs e)
        {
            status = "isNone";
            Load_permission();
            Load_Plan();
        }
        private void Load_permission()
        {
            adoClass = new ADO();
            btnSave.Enabled = adoClass.Check_permission(this.Name, btnSave.Name, General_Infor.username);
        }

        private void btnCancel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            dgvProdPlan.DataSource = null;
        }
    }
}
