using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HVN_System.View.Warehouse
{
    public partial class frmWHMaterialCCHomePage : Form
    {
        public frmWHMaterialCCHomePage()
        {
            InitializeComponent();
        }
        private CmCn conn;
        private void frmWHMaterialCCHomePage_Load(object sender, EventArgs e)
        {
            txtBarcode.Focus();
            Load_data(dtpCCDate.Value);
        }
        private void Load_data(DateTime Cc_date)
        {
            string strQry = "select place,m_name,m_kind,sum(quantity) as qty_pcs,count(m_name) as qty_box from W_M_CCInventory \n ";
            strQry += " where cc_date=N'"+Cc_date.ToString("yyyy-MM-dd")+"' \n ";
            strQry += " group by place,m_name,m_kind \n ";
            conn = new CmCn();
            dgvResult.DataSource = conn.ExcuteDataTable(strQry);
        }

        private void btnCCFullbox_Click(object sender, EventArgs e)
        {
            if (txtPIC.Text=="")
            {
                MessageBox.Show("LỖI CHƯA QUÉT MÃ QR NHÂN VIÊN");
            }
            else
            {
                frmWHMaterialCCFullBox frm = new frmWHMaterialCCFullBox(txtPIC.Text, dtpCCDate.Value);
                frm.ShowDialog();
                Load_data(dtpCCDate.Value);
            }
        }

        private void txtBarcode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode==Keys.Enter)
            {
                if (txtBarcode.Text.Length >= 6)
                {
                    if (txtBarcode.Text.Substring(2, 4) == "WHOP")
                    {
                        txtPIC.Text = txtBarcode.Text.Substring(6, txtBarcode.Text.Length - 6);
                    }
                }
                txtBarcode.Text = "";
                txtBarcode.Focus();
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            Load_data(dtpCCDate.Value);
        }

        private void dtpCCDate_ValueChanged(object sender, EventArgs e)
        {
            Load_data(dtpCCDate.Value);
        }

        private void btnCCOddbox_Click(object sender, EventArgs e)
        {
            if (txtPIC.Text == "")
            {
                MessageBox.Show("LỖI CHƯA QUÉT MÃ QR NHÂN VIÊN");
            }
            else
            {
                frmWHMaterialCCOddBox frm = new frmWHMaterialCCOddBox(txtPIC.Text, dtpCCDate.Value);
                frm.ShowDialog();
                Load_data(dtpCCDate.Value);
            }
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("BẠN CÓ MUỐN LƯU THÀNH BÁO CÁO?\n DO YOU WANT TO SUBMIT REPORT?", "SUBMIT REPORT", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (dtpCCDate.Value>=DateTime.Today)
                {
                    string strQry = "delete from W_M_CCResult where cc_date=N'" + dtpCCDate.Value.ToString("yyyy-MM-dd") + "' \n ";
                    strQry += " insert into W_M_CCResult(cc_date,whmr_code,m_name,sys_qty,cc_qty,sys_place,cc_place,label_status) \n ";
                    strQry += " select N'" + dtpCCDate.Value.ToString("yyyy-MM-dd") + "',a.whmr_code,a.m_name,b.sys_qty,c.cc_qty,b.sys_place,c.cc_place \n ";
                    strQry += " ,case \n ";
                    strQry += "       when b.sys_place<>c.cc_place then N'Mismatch place'  \n ";
                    strQry += "       when c.cc_place is null then N'Product found in system but not found during cycle count'  \n ";
                    strQry += "       when b.sys_place is null then N'Product found in cycle count but not found during system'  \n ";
                    strQry += "       when b.sys_qty<>c.cc_qty then N'Mismatch quantity' \n ";
                    strQry += "       else N'OK'  \n ";
                    strQry += "       end as label_status \n ";
                    strQry += " from  \n ";
                    strQry += " (SELECT whmr_code,m_name from W_M_ReceiveLabel where place not in ('') and quantity>0 \n ";
                    strQry += " union select whmr_code,m_name from W_M_CCInventory where cc_date=N'" + dtpCCDate.Value.ToString("yyyy-MM-dd") + "') a \n ";
                    strQry += " left join \n ";
                    strQry += " (select whmr_code,place as sys_place,quantity as sys_qty from W_M_ReceiveLabel where place not in ('') and quantity>0 \n ";
                    strQry += "    union select whmr_code,place as sys_place,quantity as sys_qty from W_M_CCInventory where cc_date=N'"+ dtpCCDate.Value.ToString("yyyy-MM-dd") + "' and  \n ";
                    strQry += "    m_kind in ('RETURN BOX','ISSUE BOX') \n ";
                    strQry += "   ) b  \n ";
                    strQry += " on a.whmr_code=b.whmr_code \n ";
                    strQry += " left join \n ";
                    strQry += " (select whmr_code,place as cc_place,quantity as cc_qty from W_M_CCInventory where cc_date=N'" + dtpCCDate.Value.ToString("yyyy-MM-dd") + "') c \n ";
                    strQry += " on a.whmr_code=c.whmr_code \n ";

                    conn = new CmCn();
                    conn.ExcuteQry(strQry);
                    MessageBox.Show("LƯU THÀNH CÔNG\n CONFIRM SUCCESSFULLY");
                }
                else
                {
                    MessageBox.Show("KHÔNG THỂ LƯU CHO KIỂM KÊ TRONG QUÁ KHỨ\n COULD NOT SAVE REPORT FOR PREVIOUS DAY");
                }
            }
        }

        private void btnCCIssuebox_Click(object sender, EventArgs e)
        {
            if (txtPIC.Text == "")
            {
                MessageBox.Show("LỖI CHƯA QUÉT MÃ QR NHÂN VIÊN");
            }
            else
            {
                frmWHMaterialCCIssueBox frm = new frmWHMaterialCCIssueBox(txtPIC.Text, dtpCCDate.Value);
                frm.ShowDialog();
                Load_data(dtpCCDate.Value);
            }
        }
    }
}
