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
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraBars;

namespace HVN_System.View.Planning
{
    public partial class frmPLA_M_ReceivingPlanDeital : Form
    {
        public frmPLA_M_ReceivingPlanDeital()
        {
            InitializeComponent();
        }
        public frmPLA_M_ReceivingPlanDeital(W_M_CheckingPlan_Entity item)
        {
            InitializeComponent();
            txtDocID.Text = item.Rm_plan_id;
            dtpCheckingDate.Value = item.Check_date;
            txtDocID.ReadOnly = true;
            isEdit = true;
        }
        private ADO adoClass;
        private CmCn conn;
        private List<W_M_CheckingPlanDetail_Entity> List_Data;
        bool isEdit = false;
        private void btnSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            adoClass = new ADO();
            bool isOk = false;
            if (Check_list_data())
            {
                if (isEdit)
                {
                    isOk = true;
                }
                else
                {
                    if (txtDocID.Text == "")
                    {
                        MessageBox.Show("Please fill the Plan ID");
                    }
                    else
                    {
                        DataTable dt = adoClass.Load_W_M_CheckingPlan("", "rm_plan_id=N'" + txtDocID.Text + "'");
                        if (dt.Rows.Count > 0)
                        {
                            MessageBox.Show("This Plan ID is already exist. Please choose another number");
                        }
                        else
                        {
                            isOk = true;
                        }
                    }
                }
                if (isOk)
                {
                    adoClass.Update_W_M_CheckingPlan(List_Data, txtDocID.Text, dtpCheckingDate.Value);
                    MessageBox.Show("Saving successfully");
                    this.Close();
                }
            }
        }
        private bool Check_list_data()
        {
            bool result = true;
            adoClass = new ADO();
            string List_error = "";
            foreach (W_M_CheckingPlanDetail_Entity item in List_Data)
            {
                if (item.M_name!=null)
                {
                    DataTable dt = adoClass.Load_W_MasterList_Material("m_name", "m_name=N'" + item.M_name + "'");
                    if (dt.Rows.Count >= 1)
                    {

                    }
                    else
                    {
                        result = false;
                        List_error += item.M_name + "\n";
                    }
                }
            }
            if (List_error != "")
            {
                MessageBox.Show("There are some unknow material: \n" + List_error, "Error");
            }
            return result;
        }
        private void frmProductionPlanFG_Load(object sender, EventArgs e)
        {
            if (isEdit)
            {
                List_Data = new List<W_M_CheckingPlanDetail_Entity>();
                adoClass = new ADO();
                DataTable dt = adoClass.Load_W_M_CheckingPlanDetail("", "rm_plan_id=N'" + txtDocID.Text+"'");
                int i = 1;
                foreach (DataRow row in dt.Rows)
                {
                    W_M_CheckingPlanDetail_Entity item = new W_M_CheckingPlanDetail_Entity();
                    item.Stt = i;
                    item.M_name = row["m_name"].ToString();
                    item.Quantity = string.IsNullOrEmpty(row["quantity"].ToString()) ? 0 : float.Parse(row["quantity"].ToString());
                    item.P_shift= row["p_shift"].ToString();
                    item.Plan_type= row["plan_type"].ToString();
                    List_Data.Add(item);
                    i++;
                }
            }
            else
            {
                txtDocID.Text = "WHMP" + Generate_Label_code();
                List_Data = new List<W_M_CheckingPlanDetail_Entity>();
                for (int i = 1; i < 30; i++)
                {
                    W_M_CheckingPlanDetail_Entity item = new W_M_CheckingPlanDetail_Entity();
                    item.Stt = i;
                    List_Data.Add(item);
                }
            }
            dgvResult.DataSource = List_Data.ToList();
        }
        private int Generate_Label_code()
        {
            string Qry = "SELECT MAX(rm_plan_id) FROM W_M_CheckingPlan ";
            conn = new CmCn();
            DataTable dt = conn.ExcuteDataTable(Qry);
            if (dt.Rows[0][0].ToString() != "")
            {
                string max_value = dt.Rows[0][0].ToString().Substring(4, dt.Rows[0][0].ToString().Length - 4);
                return int.Parse(max_value) + 1;
            }
            else
            {
                return 10001;
            }
        }
        private void btnDelete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }

        private void gvResult_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            //GridView view = sender as GridView;
            //if (e.Column == view.Columns["Status"])
            //{
            //    string status = view.GetRowCellValue(e.RowHandle, view.Columns["Status"]).ToString();
            //    switch (status)
            //    {
            //        case "OK":
            //            e.Appearance.BackColor = Color.Chartreuse;
            //            break;
            //        case "Not OK":
            //            e.Appearance.BackColor = Color.Orange;
            //            break;
            //        default:
            //            break;
            //    }
            //}
        }

       

        private void repositoryItemCheckEdit1_EditValueChanged(object sender, EventArgs e)
        {
            if (gvResult.PostEditor())
            {
                gvResult.UpdateCurrentRow();
            }
        }

        private void btnImport_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                List_Data = new List<W_M_CheckingPlanDetail_Entity>();
                OpenFileDialog OpenFile = new OpenFileDialog();
                OpenFile.Title = "Mở tệp tin";
                OpenFile.Filter = "Excel (.xlsx)|*.xlsx";
                if (OpenFile.ShowDialog() != DialogResult.Cancel)
                {
                    string FilePath = OpenFile.FileName;
                    adoClass = new ADO();
                    DataTable dt = adoClass.ReadExcelFile("INPUT", FilePath);
                    int i = 1;
                    foreach (DataRow row in dt.Rows)
                    {
                        W_M_CheckingPlanDetail_Entity item = new W_M_CheckingPlanDetail_Entity();
                        item.Stt = i;
                        item.M_name = row["Material"].ToString();
                        item.Quantity = string.IsNullOrEmpty(row["Quantity"].ToString()) ? 0 : float.Parse(row["Quantity"].ToString());
                        item.P_shift = row["Shift"].ToString();
                        item.Plan_type = row["Type of plan"].ToString();
                        List_Data.Add(item);
                        i++;
                    }
                    dgvResult.DataSource = List_Data.ToList();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }
    }
}
