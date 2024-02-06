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

namespace HVN_System.View.HR
{
    public partial class frmHRAbsenteeism : Form
    {
        public frmHRAbsenteeism()
        {
            InitializeComponent();
        }
        private CmCn conn;
        private ADO adoClass;
        string Current_Id = "";
        private void btnSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            conn = new CmCn();
            string strQry = "insert into KPI_HR_Absenteeism (Date,Absent_type,Employee_no,Comment) \n";
            strQry += "values(N'"+dtpAbsentDate.Value.ToString("yyyy-MM-dd")+"', N'"+cboAbsentType.Text+ "', N'" + nmEmployeeNo.Text + "', N'" + txtComment.Text + "') \n";
            if (cboAbsentType.Text!="" &&nmEmployeeNo.Value>0)
            {
                try
                {
                    conn.ExcuteQry(strQry);
                    Load_Data();
                    MessageBox.Show("Thêm thành công!");
                    txtComment.Text = "";
                    nmEmployeeNo.Value = 0;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Lỗi điền thiếu thông tin");
            }
        }

        private void frmHRSafetyAlert_Load(object sender, EventArgs e)
        {
            Load_Combobox();
            Load_Data();
        }
        private void Load_Data()
        {
            string strQry = "select * from KPI_HR_Absenteeism";
            DataTable dt = new DataTable();
            conn = new CmCn();
            dt = conn.ExcuteDataTable(strQry);
            dgvResult.DataSource = dt;
        }
        private void Load_Combobox()
        {
            adoClass = new ADO();
            cboAbsentType.DataSource = adoClass.Load_Parameter_Detail("", "parent_id='absent_type'");
            cboAbsentType.DisplayMember = "child_name";
            cboAbsentType.ValueMember = "child_name";
        }
        private void btnDelete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (XtraMessageBox.Show("Do you want to delete this item?", "Delete Item", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                string strQry = "delete from KPI_HR_Absenteeism where Absent_id =N'"+Current_Id+"'";
                conn = new CmCn();
                conn.ExcuteQry(strQry);
                Load_Data();
            }
        }

        private void gvResult_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            Current_Id = gvResult.GetRowCellValue(gvResult.FocusedRowHandle, "Absent_id").ToString();
        }

        private void btnRefresh_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Load_Data();
        }
    }
}
