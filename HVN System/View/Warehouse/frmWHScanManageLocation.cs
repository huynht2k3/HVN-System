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

namespace HVN_System.View.Warehouse
{
    public partial class frmWHScanManageLocation : Form
    {
        public frmWHScanManageLocation()
        {
            InitializeComponent();
        }
        private ADO adoClass;
        private CmCn conn;
        private void gvResult_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            txtLoc.Text = gvResult.GetRowCellValue(gvResult.FocusedRowHandle, "loc_name").ToString();
            txtDes.Text = gvResult.GetRowCellValue(gvResult.FocusedRowHandle, "loc_des").ToString();
        }

        private void frmWHCCManagement_Load(object sender, EventArgs e)
        {
            cboPlace.Text = "FG Zone";
            Load_Loc();
        }
        private void Load_Loc()
        {
            adoClass = new ADO();
            dgvResult.DataSource = adoClass.Load_W_MasterList_Location("", "place =N'"+cboPlace.Text+"'");
        }
        private void gvResult_DoubleClick(object sender, EventArgs e)
        {

        }


        private void btnDelete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (txtLoc.Text != "")
            {
                if (MessageBox.Show("Do you want to delete location?", "Delete Location", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    string strQry = "delete from W_MasterList_Location where loc_name=N'" + txtLoc.Text + "'and place =N'" + cboPlace.Text + "'";
                    conn = new CmCn();
                    conn.ExcuteQry(strQry);
                    MessageBox.Show("The location " + txtLoc.Text + " has been deleted");
                    ClearData();
                    Load_Loc();
                }
            }
        }
        private void ClearData()
        {
            txtDes.Text="";
            txtLoc.Text = "";
            txtLoc.Focus();
        }
        private void btnSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (txtLoc.Text != "")
            {
                if (MessageBox.Show("Do you want to save information?", "Save Information", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    string strQry = "update W_MasterList_Location set loc_des=N'"+txtDes.Text+"' where loc_name=N'" + txtLoc.Text + "' and place =N'" + cboPlace.Text + "'";
                    conn = new CmCn();
                    conn.ExcuteQry(strQry);
                    MessageBox.Show("The location " + txtLoc.Text + " has been updated");
                    ClearData();
                    Load_Loc();
                }
            }
        }

        private void btnNew_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (txtLoc.Text != "")
            {
                string strQry = "insert into W_MasterList_Location (loc_name,loc_des,place) values (N'" + txtLoc.Text + "',N'"+txtDes.Text+ "',N'" + cboPlace.Text + "')";
                try
                {
                    conn = new CmCn();
                    conn.ExcuteQry(strQry);
                    MessageBox.Show("The location " + txtLoc.Text + " has been added");
                    ClearData();
                    Load_Loc();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Location is already exist. Cannot add new location has same name \n\n"+ex.Message);
                }
            }
        }

        private void cboPlace_SelectionChangeCommitted(object sender, EventArgs e)
        {
            Load_Loc();
        }
    }
}
