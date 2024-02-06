using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HVN_System.Entity;
using HVN_System.Util;

namespace HVN_System.View.HR
{
    public partial class frmHR_EmployeeInfor : Form
    {
        public frmHR_EmployeeInfor()
        {
            InitializeComponent();
        }
        private ADO adoClass;
        private CmCn conn;
        List<HR_EmployeeInfor_Entity> List_Data;
        private HR_EmployeeInfor_Entity current_item;
        private bool isAddNew = true;
        private void gvResult_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            current_item = gvResult.GetRow(gvResult.FocusedRowHandle) as HR_EmployeeInfor_Entity;
            txtEmployeeID.Text = current_item.Emp_id;
            txtDepartment.Text= current_item.Emp_dept;
            txtFullname.Text = current_item.Emp_name;
            txtArea.Text= current_item.Emp_area;
            dtpOnboardDate.Value = current_item.Onboard_date;
            txtEmployeeID.ReadOnly = true;
            isAddNew = false;
        }

        private void frmWHCCManagement_Load(object sender, EventArgs e)
        {
            Load_Data();
            //btnSave.Enabled = adoClass.Check_permission(this.Name, btnSave.Name, General_Infor.username);
            //btnDelete.Enabled = adoClass.Check_permission(this.Name, btnSave.Name, General_Infor.username);
        }
        private void Load_Data()
        {
            current_item = new HR_EmployeeInfor_Entity();
            string strQry = "select * from HR_EmployeeInfor";
            conn = new CmCn();
            DataTable dt = conn.ExcuteDataTable(strQry);
            List_Data = new List<HR_EmployeeInfor_Entity>();
            foreach (DataRow row in dt.Rows)
            {
                HR_EmployeeInfor_Entity item = new HR_EmployeeInfor_Entity();
                item.Emp_id = row["Emp_id"].ToString();
                item.Emp_dept = row["Emp_dept"].ToString();
                item.Emp_name = row["Emp_name"].ToString();
                item.Emp_area = row["Emp_area"].ToString();
                item.Onboard_date = string.IsNullOrEmpty(row["Onboard_date"].ToString()) ? DateTime.Today : DateTime.Parse(row["Onboard_date"].ToString());
                List_Data.Add(item);
            }
            dgvResult.DataSource = List_Data.ToList();
        }
        private void gvResult_DoubleClick(object sender, EventArgs e)
        {

        }

        private void btnSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (MessageBox.Show("Do you want to save data for : " + txtFullname.Text + " ?", "Save Data", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                string strQry = "";
                if (isAddNew)
                {
                    strQry += "insert into HR_EmployeeInfor (emp_id,emp_name,emp_dept,emp_area,onboard_date) \n";
                    strQry += "select N'"+txtEmployeeID.Text+ "',N'" + txtFullname.Text + "',N'" + txtDepartment.Text 
                        + "',N'" + txtArea.Text + "',N'" + dtpOnboardDate.Value.ToString("yyyy-MM-dd") + "'";
                }
                else
                {
                    strQry = "update HR_EmployeeInfor set  \n ";
                    strQry += " emp_dept=N'" + txtDepartment.Text + "',emp_name=N'" + txtFullname.Text + "',emp_area=N'" + txtArea.Text + "',onboard_date=N'" + dtpOnboardDate.Value.ToString("yyyy-MM-dd") + "' \n ";
                    strQry += " where emp_id=N'" + txtEmployeeID.Text + "' \n ";
                }
                conn = new CmCn();
                try
                {
                    conn.ExcuteQry(strQry);
                    Load_Data();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void btnRefresh_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Load_Data();
        }

        private void btnExport_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            adoClass = new ADO();
            adoClass.Export_Excel(dgvResult);
        }

        private void btnDelete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (!string.IsNullOrEmpty(current_item.Emp_id))
            {
                if (MessageBox.Show("Do you want to delete employee: "+ current_item.Emp_name + " ?", "Delete Item", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    string strQry= "delete from HR_EmployeeInfor where emp_id=N'" + current_item.Emp_id + "'\n";
                    try
                    {
                        conn = new CmCn();
                        conn.ExcuteQry(strQry);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    Load_Data();
                    MessageBox.Show("Delete successfully.");
                }
            } 
        }

        private void btnImportEdit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            OpenFileDialog OpenFile = new OpenFileDialog();
            OpenFile.Title = "Mở tệp tin";
            OpenFile.Filter = "Excel (.xlsx)|*.xlsx";
            if (OpenFile.ShowDialog() != DialogResult.Cancel)
            {
                string FilePath = OpenFile.FileName;
                adoClass = new ADO();
                DataTable dt = adoClass.ReadExcelFile("Sheet1", FilePath);
                string strQry = "";
                foreach (DataRow row in dt.Rows)
                {
                    string PN = row["Material"].ToString();
                    string Ref_Qty = row["Reference Qty"].ToString();
                    string Ref_Weight = row["Reference Weight"].ToString();
                    string Standard_Qty = row["Standard Qty"].ToString();
                    string Scale_type = row["Type Scale"].ToString();
                    strQry += "update W_MasterList_Material set raw_qty=N'" + Ref_Qty + "',scale_type=N'" + Scale_type + "',m_qty=N'" + Standard_Qty + "',raw_weight=N'" + Ref_Weight + "'\n";
                    strQry += "where m_name=N'"+PN+"'\n";
                }
                conn = new CmCn();
                conn.ExcuteQry(strQry);
                MessageBox.Show("Import successfully");
            }
        }

        private void gvResult_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode==Keys.Delete)
            {
                btnDelete.PerformClick();
            }
        }

        private void dgvResult_Click(object sender, EventArgs e)
        {

        }

        private void btnAddNew_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            txtEmployeeID.Text = "";
            txtDepartment.Text = "";
            txtFullname.Text = "";
            txtArea.Text = "";
            txtEmployeeID.ReadOnly = false;
            isAddNew = true;
        }

        private void btnEvaluateEmp_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmHR_EmployeeEvaluate frm = new frmHR_EmployeeEvaluate(current_item);
            frm.ShowDialog();
        }
    }
}
