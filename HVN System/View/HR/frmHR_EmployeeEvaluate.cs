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
    public partial class frmHR_EmployeeEvaluate : Form
    {
        public frmHR_EmployeeEvaluate(HR_EmployeeInfor_Entity _current_item)
        {
            InitializeComponent();
            txtEmployeeID.Text = _current_item.Emp_id;
            txtDepartment.Text = _current_item.Emp_dept;
            txtFullname.Text = _current_item.Emp_name;
            txtArea.Text = _current_item.Emp_area;
            dtpOnboardDate.Value = _current_item.Onboard_date;
        }
        private ADO adoClass;
        private CmCn conn;
        List<QC_SM_Score_Entity> List_Data;
        private QC_SM_Score_Entity current_item;
        private void gvResult_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {

        }

        private void frmWHCCManagement_Load(object sender, EventArgs e)
        {
            Load_Data();
            //btnSave.Enabled = adoClass.Check_permission(this.Name, btnSave.Name, General_Infor.username);
            //btnDelete.Enabled = adoClass.Check_permission(this.Name, btnSave.Name, General_Infor.username);
        }
        private void Load_Data()
        {
            current_item = new QC_SM_Score_Entity();
            string strQry = "select * from QC_SM_Score where emp_id=N'"+txtEmployeeID.Text+"'";
            conn = new CmCn();
            DataTable dt = conn.ExcuteDataTable(strQry);
            List_Data = new List<QC_SM_Score_Entity>();
            foreach (DataRow row in dt.Rows)
            {
                QC_SM_Score_Entity item = new QC_SM_Score_Entity();
                item.Emp_id = row["emp_id"].ToString();
                item.Emp_project = row["emp_project"].ToString();
                item.Emp_score = float.Parse(row["emp_score"].ToString());
                List_Data.Add(item);
            }
            dgvResult.DataSource = List_Data.ToList();
        }
        

        private void btnSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (MessageBox.Show("Do you want to save data for : " + txtFullname.Text + " ?", "Save Data", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                string strQry = "delete from QC_SM_Score where emp_id=N'"+txtEmployeeID.Text+"'";
                string qry2 = "";
                foreach (QC_SM_Score_Entity item in List_Data)
                {
                    if (qry2=="")
                    {
                        qry2 += "select N'" + item.Emp_id + "',N'" + item.Emp_project + "',N'" + item.Emp_score + "' \n";
                    }
                    else
                    {
                        qry2 += "union all select N'" + item.Emp_id + "',N'" + item.Emp_project + "',N'" + item.Emp_score + "' \n";
                    }
                }
                if (qry2!="")
                {
                    strQry = "delete from QC_SM_Score where emp_id=N'" + txtEmployeeID.Text + "'\n";
                    strQry += "insert into QC_SM_Score (emp_id,emp_project,emp_score) \n";
                    strQry += qry2;
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
 

        private void gvResult_KeyDown(object sender, KeyEventArgs e)
        {
            
        }


    }
}
