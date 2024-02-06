using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HVN_System.Util;
using HVN_System.Entity;

namespace HVN_System.View.Warehouse
{
    public partial class frmWHCCRemoveItem : Form
    {
        public frmWHCCRemoveItem()
        {
            InitializeComponent();
        }
        public frmWHCCRemoveItem(string cc_name_,string place)
        {
            InitializeComponent();
            Cc_name = cc_name_;
            Place = place;
        }
        string Cc_name;
        string Place;
        private List<P_Label_Entity> List_data;
        private ADO adoClass;
        private CmCn conn;
        private void frmWHCCRemoveItem_Load(object sender, EventArgs e)
        {
            List_data = new List<P_Label_Entity>();
            adoClass = new ADO();
            DataTable dt = adoClass.Load_W_CycleCountInventory("", "cc_name=N'"+ Cc_name + "' and place=N'"+ Place + "'");
            foreach (DataRow row in dt.Rows)
            {
                P_Label_Entity item = new P_Label_Entity();
                item.Label_code = row["label_code"].ToString();
                item.Product_customer_code = row["product_customer_code"].ToString();
                item.Pallet_no = row["pallet_no"].ToString();
                item.Product_quantity = int.Parse(row["product_quantity"].ToString());
                item.Check = false;
                List_data.Add(item);
            }
            dgvResult.DataSource = List_data.ToList();
        }

        private void btnSelectAll_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            foreach (P_Label_Entity item in List_data)
            {
                item.Check = true;
            }
            dgvResult.DataSource = List_data.ToList();
        }

        private void btnSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (MessageBox.Show("Do you want to remove selected items?", "Save change", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                string strQry = "";
                foreach (P_Label_Entity item in List_data)
                {
                    if (item.Check == true)
                    {
                        strQry += "delete from W_CycleCountInventory where label_code=N'" + item.Label_code + "' and cc_name=N'" + Cc_name + "' \n";
                    }
                }
                conn = new CmCn();
                conn.ExcuteQry(strQry);
                MessageBox.Show("Remove successfully");
                this.Close();
            }
        }

        private void repositoryItemCheckEdit1_EditValueChanged(object sender, EventArgs e)
        {
            if (gvResult.PostEditor())
            {
                gvResult.UpdateCurrentRow();
            }
        }
    }
}
