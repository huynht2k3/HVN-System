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
    public partial class frmWHCCFGHomePage : Form
    {
        public frmWHCCFGHomePage()
        {
            InitializeComponent();
        }
        public frmWHCCFGHomePage(W_CycleCount_Entity _Cc,DataTable dt_partial,string _PIC)
        {
            InitializeComponent();
            txtCCName.Text = _Cc.Cc_name;
            txtCCType.Text = _Cc.Cc_type;
            dt_Parital = dt_partial;
            CycleCount_Info = _Cc;
            PIC = _PIC;
        }
        private DataTable dt_Parital;
        private W_CycleCount_Entity CycleCount_Info;
        private CmCn conn;
        string PIC;
        private void frmWHCCHomePage_Load(object sender, EventArgs e)
        {
            if (txtCCType.Text== "Partial cycle count")
            {
                try
                {
                    cboPartial.Properties.DataSource = dt_Parital;
                    cboPartial.Properties.DisplayMember = "PART NUMBER";
                    cboPartial.Properties.ValueMember = "PART NUMBER";
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
               
            }
            else
            {
                layoutControlItem7.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            }
            Load_Data();
        }

        private void btnCc_Click(object sender, EventArgs e)
        {
            frmWHCCFGZone frm = new frmWHCCFGZone(CycleCount_Info, dt_Parital,PIC);
            frm.ShowDialog();
            Load_Data();
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void Load_Data()
        {
            string strQry = "select a.wh_location,a.product_customer_code,a.Qty_box,a.Qty_pcs,b.Qty_pallet from  \n ";
            strQry += " (select wh_location,product_customer_code,COUNT(label_code) as Qty_box, SUM(product_quantity) as Qty_pcs \n ";
            strQry += " from W_CycleCountInventory \n ";
            strQry += " where cc_name = N'"+txtCCName.Text+"' and place=N'FG Zone' \n ";
            strQry += " group by wh_location,product_customer_code) as a \n ";
            strQry += " left join \n ";
            strQry += " (select dt.wh_location, dt.product_customer_code, count(dt.pallet_no) as Qty_pallet from \n ";
            strQry += " (select product_customer_code,pallet_no,wh_location  \n ";
            strQry += " from W_CycleCountInventory \n ";
            strQry += " where cc_name = N'" + txtCCName.Text + "' and place=N'FG Zone' and pallet_no not in ('')\n ";
            strQry += " group by product_customer_code,pallet_no,wh_location) as dt  \n ";
            strQry += " group by dt.product_customer_code,dt.wh_location) as b \n ";
            strQry += " on a.product_customer_code=b.product_customer_code and a.wh_location = b.wh_location \n ";
            conn = new CmCn();
            try
            {
                DataTable dt= new DataTable();
                dt = conn.ExcuteDataTable(strQry);
                dgvResult.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

    }
}
