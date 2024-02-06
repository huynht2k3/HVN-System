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
    public partial class frmWHCCHomePage : Form
    {
        public frmWHCCHomePage()
        {
            InitializeComponent();
        }
        public frmWHCCHomePage(W_CycleCount_Entity _Cc)
        {
            InitializeComponent();
            txtCCName.Text = _Cc.Cc_name;
            txtCCType.Text = _Cc.Cc_type;
            CycleCount_Info = _Cc;
        }
        private ADO adoClass;
        private CmCn conn;
        private W_CycleCount_Entity CycleCount_Info;
        private DataTable dt_Partial;
        private void frmWHCCHomePage_Load(object sender, EventArgs e)
        {
            txtBarcode.Focus();
            if (txtCCType.Text== "Partial cycle count")
            {
                try
                {
                    adoClass = new ADO();
                    dt_Partial = new DataTable();
                    dt_Partial = adoClass.Load_W_Cycle_Count_List_Partial("product_customer_code as [PART NUMBER],product_code as [PRODUCT CODE]", "cc_name=N'" + txtCCName.Text + "'");
                    cboPartial.Properties.DataSource = dt_Partial;
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

        private void btnCCFGZone_Click(object sender, EventArgs e)
        {
            if (txtPIC.Text!="")
            {
                frmWHCCFGHomePage frm = new frmWHCCFGHomePage(CycleCount_Info, dt_Partial, txtPIC.Text);
                frm.ShowDialog();
                Load_Data();
                lbError.Text = "";
            }
            else
            {
                lbError.Text = "MISSING NAME OF PIC/ THIẾU TÊN NHÂN VIÊN KIỂM KÊ";
            }
        }

        private void btnCCWaitingZone_Click(object sender, EventArgs e)
        {
            
            if (txtPIC.Text != "")
            {
                frmWHCCWaitingZone frm = new frmWHCCWaitingZone(CycleCount_Info, dt_Partial,txtPIC.Text);
                frm.ShowDialog();
                Load_Data();
            }
            else
            {
                lbError.Text = "MISSING NAME OF PIC/ THIẾU TÊN NHÂN VIÊN KIỂM KÊ";
            }
        }

        private void btnCCPackingZone_Click(object sender, EventArgs e)
        {
            
            if (txtPIC.Text != "")
            {
                frmWHCCPackingZone frm = new frmWHCCPackingZone(CycleCount_Info, dt_Partial, txtPIC.Text);
                frm.ShowDialog();
                Load_Data();
            }
            else
            {
                lbError.Text = "MISSING NAME OF PIC/ THIẾU TÊN NHÂN VIÊN KIỂM KÊ";
            }
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            //if (MessageBox.Show("Do you want to confirm the data", "CONFIRM", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            //{
            //    adoClass = new ADO();
            //    adoClass.Confirm_W_CycleCountInventory(txtCCName.Text);
            //    MessageBox.Show("Confirm successfully");
            //}
            if (MessageBox.Show("Do you want to confirm the data", "CONFIRM", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                string strQry2 = "";
                if (txtCCType.Text== "Partial cycle count")
                {
                    string strQry_check = "select product_customer_code from W_CycleCountListPartial where cc_name=N'" + txtCCName.Text + "'";
                    DataTable dt_list = conn.ExcuteDataTable(strQry_check);
                    string list_PN = "'0'";
                    foreach (DataRow item in dt_list.Rows)
                    {
                        list_PN += ",'" + item["product_customer_code"].ToString() + "'";
                    }
                    strQry2 += "delete from W_CycleCountResult where cc_name=N'" + txtCCName.Text + "'";
                    strQry2 += "insert into W_CycleCountResult(cc_name,label_code,product_customer_code,product_quantity,sys_place,cc_place,sys_location,cc_location,sys_pallet_no,cc_pallet_no,label_status)\n";
                    strQry2 += "select N'" + txtCCName.Text + "' as cc_name,a.label_code,a.product_customer_code,a.product_quantity,b.sys_place,c.cc_place,b.sys_location,c.cc_location,b.sys_pallet_no,c.cc_pallet_no \n ";
                    strQry2 += " ,case \n ";
                    strQry2 += "     when b.sys_place<>c.cc_place then N'Mismatch place' \n ";
                    strQry2 += "     when b.sys_location<>c.cc_location then N'Mismatch location' \n ";
                    strQry2 += "     when b.sys_place<>c.cc_place and b.sys_location<>c.cc_location then N'Mismatch location and place' \n ";
                    strQry2 += "     when c.cc_place is null then N'Product found in system but not found during cycle count' \n ";
                    strQry2 += "     when b.sys_place is null then N'Product found in cycle count but not found during system' \n ";
                    strQry2 += "     else N'OK' \n ";
                    strQry2 += "     end as label_status \n ";
                    strQry2 += " from \n ";
                    strQry2 += " (select product_quantity,label_code,product_customer_code from P_Label where place in (select place from W_CycleCountArea where cc_name=N'" + txtCCName.Text + "') and product_customer_code in (" + list_PN + ") \n ";
                    strQry2 += " UNION select product_quantity,label_code,product_customer_code from W_CycleCountInventory where cc_name=N'" + txtCCName.Text + "') a \n ";
                    strQry2 += " left join  \n ";
                    strQry2 += " (select label_code,place as sys_place,wh_location as sys_location,pallet_no as sys_pallet_no  \n ";
                    strQry2 += " from P_Label where place in (select place from W_CycleCountArea where cc_name=N'" + txtCCName.Text + "') and product_customer_code in (" + list_PN + ")) b \n ";
                    strQry2 += " on a.label_code=b.label_code \n ";
                    strQry2 += " left join \n ";
                    strQry2 += " (select label_code,place as cc_place,wh_location as cc_location , pallet_no as cc_pallet_no \n ";
                    strQry2 += " from W_CycleCountInventory where cc_name=N'" + txtCCName.Text + "') c \n ";
                    strQry2 += " on a.label_code=c.label_code \n ";

                }
                else
                {
                    strQry2 += "delete from W_CycleCountResult where cc_name=N'" + txtCCName.Text + "'";
                    strQry2 += "insert into W_CycleCountResult(cc_name,label_code,product_customer_code,product_quantity,sys_place,cc_place,sys_location,cc_location,sys_pallet_no,cc_pallet_no,label_status)\n";
                    strQry2 += "select N'" + txtCCName.Text + "' as cc_name,a.label_code,a.product_customer_code,a.product_quantity,b.sys_place,c.cc_place,b.sys_location,c.cc_location,b.sys_pallet_no,c.cc_pallet_no \n ";
                    strQry2 += " ,case \n ";
                    strQry2 += "     when b.sys_place<>c.cc_place then N'Mismatch place' \n ";
                    strQry2 += "     when b.sys_location<>c.cc_location then N'Mismatch location' \n ";
                    strQry2 += "     when b.sys_place<>c.cc_place and b.sys_location<>c.cc_location then N'Mismatch location and place' \n ";
                    strQry2 += "     when c.cc_place is null then N'Product found in system but not found during cycle count' \n ";
                    strQry2 += "     when b.sys_place is null then N'Product found in cycle count but not found during system' \n ";
                    strQry2 += "     else N'OK' \n ";
                    strQry2 += "     end as label_status \n ";
                    strQry2 += " from \n ";
                    strQry2 += " (select product_quantity,label_code,product_customer_code from P_Label where place in (select place from W_CycleCountArea where cc_name=N'" + txtCCName.Text + "') \n ";
                    strQry2 += " UNION select product_quantity,label_code,product_customer_code from W_CycleCountInventory where cc_name=N'" + txtCCName.Text + "') a \n ";
                    strQry2 += " left join  \n ";
                    strQry2 += " (select label_code,place as sys_place,wh_location as sys_location,pallet_no as sys_pallet_no  \n ";
                    strQry2 += " from P_Label where place in (select place from W_CycleCountArea where cc_name=N'" + txtCCName.Text + "')) b \n ";
                    strQry2 += " on a.label_code=b.label_code \n ";
                    strQry2 += " left join \n ";
                    strQry2 += " (select label_code,place as cc_place,wh_location as cc_location , pallet_no as cc_pallet_no \n ";
                    strQry2 += " from W_CycleCountInventory where cc_name=N'" + txtCCName.Text + "') c \n ";
                    strQry2 += " on a.label_code=c.label_code \n ";
                }
                conn = new CmCn();
                conn.ExcuteQry(strQry2);
                MessageBox.Show("Confirm successfully");
            }
        }
        private void Load_Data()
        {
            string strQry = "select a.place,a.wh_location,a.product_customer_code,a.Qty_box,a.Qty_pcs,b.Qty_pallet from  \n ";
            strQry += " (select place,wh_location,product_customer_code,COUNT(label_code) as Qty_box, SUM(product_quantity) as Qty_pcs \n ";
            strQry += " from W_CycleCountInventory \n ";
            strQry += " where cc_name = N'"+txtCCName.Text+"' \n ";
            strQry += " group by place,wh_location,product_customer_code) as a \n ";
            strQry += " left join \n ";
            strQry += " (select dt.place,dt.wh_location, dt.product_customer_code, count(dt.pallet_no) as Qty_pallet from \n ";
            strQry += " (select place,product_customer_code,pallet_no,wh_location  \n ";
            strQry += " from W_CycleCountInventory \n ";
            strQry += " where cc_name = N'" + txtCCName.Text + "' and pallet_no not in ('') \n ";
            strQry += " group by product_customer_code,pallet_no,wh_location,place) as dt  \n ";
            strQry += " group by dt.product_customer_code,dt.wh_location,dt.place) as b \n ";
            strQry += " on a.product_customer_code=b.product_customer_code and a.wh_location = b.wh_location \n ";
            strQry += " and a.place=b.place \n ";
            strQry += " order by a.place \n ";
            conn = new CmCn();
            try
            {
                DataTable dt = new DataTable();
                dt=conn.ExcuteDataTable(strQry);
                dgvResult.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void txtBarcode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                lbError.Text = "";
                if (txtBarcode.Text.Substring(2, 4) == "WHOP")
                {
                    txtPIC.Text = txtBarcode.Text.Substring(6, txtBarcode.Text.Length - 6);
                }
                else
                {
                    lbError.Text = "WRONG QR CODE, SCAN ONLY QR CODE OF PIC/ SAI MÃ QR, VUI LÒNG CHỈ QUÉT MÃ NHÂN VIÊN";
                }
                txtBarcode.Text = "";
                txtBarcode.Focus();
            }
        }

        private void btnCCContainer_Click(object sender, EventArgs e)
        {
            if (txtPIC.Text != "")
            {
                frmWHCCContainer frm = new frmWHCCContainer(CycleCount_Info, dt_Partial, txtPIC.Text);
                frm.ShowDialog();
                Load_Data();
            }
            else
            {
                lbError.Text = "MISSING NAME OF PIC/ THIẾU TÊN NHÂN VIÊN KIỂM KÊ";
            }
        }
    }
}
