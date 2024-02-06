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
    public partial class frmWHCCManagement : Form
    {
        public frmWHCCManagement()
        {
            InitializeComponent();
        }
        private W_CycleCount_Entity Current_item;
        private List<W_CycleCount_Entity> List_Data;
        private ADO adoClass;
        private void gvResult_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            Current_item = gvResult.GetRow(gvResult.FocusedRowHandle) as W_CycleCount_Entity;
        }

        private void btnEdit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (string.IsNullOrEmpty(Current_item.Cc_name))
            {
                MessageBox.Show("Please choose item before editing");
            }
            else
            {
                frmWHCCInformation frm = new frmWHCCInformation(Current_item);
                frm.ShowDialog();
                Load_List_CC();
            }
        }

        private void btnNew_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmWHCCInformation frm = new frmWHCCInformation();
            frm.ShowDialog();
            Load_List_CC();
        }

        private void frmWHCCManagement_Load(object sender, EventArgs e)
        {
            Load_List_CC();
        }
        private void Load_List_CC()
        {
            Current_item = new W_CycleCount_Entity();
            List_Data = new List<W_CycleCount_Entity>();
            adoClass = new ADO();
            DataTable dt = adoClass.Load_W_CycleCount("", "isActive=N'1'");
            foreach (DataRow row in dt.Rows)
            {
                W_CycleCount_Entity item = new W_CycleCount_Entity();
                item.Cc_name = row["cc_name"].ToString();
                item.Cc_date = DateTime.Parse(row["cc_date"].ToString());
                item.Cc_des = row["cc_des"].ToString();
                item.Cc_type = row["cc_type"].ToString();
                item.IsLock = row["isLock"].ToString();
                List_Data.Add(item);
            }
            dgvResult.DataSource = List_Data.ToList();
        }
        private void gvResult_DoubleClick(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Current_item.Cc_name))
            {
                MessageBox.Show("Please choose item before editing");
            }
            else
            {
                frmWHCCInformation frm = new frmWHCCInformation(Current_item);
                frm.ShowDialog();
                Load_List_CC();
            }
        }

        private void btnCycleCount_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (string.IsNullOrEmpty(Current_item.Cc_name))
            {
                MessageBox.Show("Please choose item before editing");
            }
            else
            {
                if (Current_item.IsLock=="Unblock")
                {
                    frmWHCCHomePage frm = new frmWHCCHomePage(Current_item);
                    frm.Show();
                }
                else
                {
                    MessageBox.Show("This cycle count has been blocked already!");
                }
            }
        }

        private void btnDelete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (MessageBox.Show("Do you want to delete this cycle count?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                if (string.IsNullOrEmpty(Current_item.Cc_name))
                {
                    MessageBox.Show("Please choose item before deleting");
                }
                else
                {
                    adoClass = new ADO();
                    adoClass.Delete_W_CycleCount(Current_item.Cc_name);
                    MessageBox.Show("This item has been deleted");
                    Load_List_CC();
                }
            }
        }

        private void btnBlock_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (string.IsNullOrEmpty(Current_item.Cc_name))
            {
                MessageBox.Show("Please choose item that be deleted");
            }
            else
            {
                if (MessageBox.Show("Do you block this items?", "Block items", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    adoClass = new ADO();
                    adoClass.Block_W_CycleCount(Current_item.Cc_name);
                    Load_List_CC();
                }
            }
        }
    }
}
