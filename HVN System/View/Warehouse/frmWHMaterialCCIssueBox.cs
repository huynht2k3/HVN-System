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
    public partial class frmWHMaterialCCIssueBox : Form
    {
        public frmWHMaterialCCIssueBox()
        {
            InitializeComponent();
        }
        public frmWHMaterialCCIssueBox(string PIC,DateTime Cc_date)
        {
            InitializeComponent();
            txtPIC.Text = PIC;
            dtpCCDate.Value = Cc_date;
        }
        private CmCn conn;
        //private ADO adoClass;
        private void ckAdd_CheckedChanged(object sender, EventArgs e)
        {
            if (ckAdd.Checked)
            {
                ckAdd.Text = "REMOVE/ XÓA";
            }
            else
            {
                ckAdd.Text = "ADD/ THÊM";
            }
        }

        private void txtBarcode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode==Keys.Enter)
            {
                string QR_code= txtBarcode.Text.Substring(2, txtBarcode.Text.Length - 2);
                if (QR_code.Length>=6)
                {
                    lbError.Text = "";
                    if (txtBarcode.Text.Substring(2, 4) == "WHOP")
                    {
                        txtPIC.Text= txtBarcode.Text.Substring(6, txtBarcode.Text.Length - 6);
                    }
                    else if(txtBarcode.Text.Substring(2, 4) == "WHMI")
                    {
                        if (ckAdd.Checked)
                        {
                            Remove_Item(QR_code);
                        }
                        else
                        {
                            Add_Item(QR_code);
                        }
                    }
                    else
                    {
                        lbError.Text = QR_code+":KHÔNG PHẢI TEM CẤP HÀNG, CÓ THỂ BẠN ĐÃ NHẦM SANG TEM TRONG KHO";
                    }
                }
                txtBarcode.Text = "";
                txtBarcode.Focus();
                Load_data(dtpCCDate.Value,QR_code);
            }
        }
        private void Remove_Item(string QRCode)
        {
            if (txtPIC.Text == "")
            {
                lbError.Text = "LỖI: BẠN CHƯA QUÉT MÃ NHÂN VIÊN";
                return;
            }
            conn = new CmCn();
            string strQry = " delete from W_M_CCInventory where whmr_code=N'" + QRCode + "' and cc_date=N'" + dtpCCDate.Value.ToString("yyyy-MM-dd") + "'";
            conn.ExcuteQry(strQry);
        }
        private void Add_Item(string QRCode)
        {
            if (txtPIC.Text=="")
            {
                lbError.Text = "LỖI: BẠN CHƯA QUÉT MÃ NHÂN VIÊN";
                return;
            }
            string qry = "select pic from  W_M_CCInventory where whmr_code=N'"+QRCode+ "' and cc_date=N'"+ dtpCCDate.Value.ToString("yyyy-MM-dd") + "'";
            conn = new CmCn();
            string PIC = conn.ExcuteString(qry);
            if (string.IsNullOrEmpty(PIC))
            {
                string strQry = " insert into W_M_CCInventory (cc_date,whmr_code,m_name,quantity,place,pic,time_commit,m_kind) \n ";
                strQry += " select N'"+dtpCCDate.Value.ToString("yyyy-MM-dd")+ "',whmi_code,m_name,quantity \n ";
                strQry += " ,N'" + cboPlace.Text + "',N'" + txtPIC.Text.ToUpper() + "',getdate(),N'ISSUE BOX' \n ";
                strQry += " from W_M_IssueLabel where whmi_code=N'" + QRCode + "' \n ";
                conn.ExcuteQry(strQry);
            }
            else
            {
                lbError.Text = QRCode + ":THÙNG ĐÃ ĐƯỢC KIỂM BỞI "+PIC;
            }
        }
        private void Load_data(DateTime Cc_date,string QR_Code)
        {
            string strQry = "select place,m_name,sum(quantity) as qty_pcs,count(m_name) as qty_box from W_M_CCInventory \n ";
            if (txtPIC.Text == "")
            {
                strQry += " where cc_date=N'" + dtpCCDate.Value.ToString("yyyy-MM-dd") + "' and m_kind=N'ISSUE BOX' \n ";
            }
            else
            {
                strQry += " where cc_date=N'" + dtpCCDate.Value.ToString("yyyy-MM-dd") + "' and m_kind=N'ISSUE BOX' and pic=N'" + txtPIC.Text.ToUpper() + "' \n ";
            }
            strQry += " group by place,m_name \n ";
            conn = new CmCn();
            dgvResult.DataSource = conn.ExcuteDataTable(strQry);
            string strQry2 = "select * from W_M_CCInventory \n ";
            if (txtPIC.Text == "")
            {
                strQry2 += " where cc_date=N'" + dtpCCDate.Value.ToString("yyyy-MM-dd") + "' and m_kind=N'ISSUE BOX' \n ";
            }
            else
            {
                strQry2 += " where cc_date=N'" + dtpCCDate.Value.ToString("yyyy-MM-dd") + "' and m_kind=N'ISSUE BOX' and pic=N'" + txtPIC.Text.ToUpper() + "' \n ";
            }
            DataTable dt = conn.ExcuteDataTable(strQry2);
            dgvDetail.DataSource= dt;
            txtTotalBox.Text = dt.Rows.Count.ToString();
            string strQry3= "select m_name from W_M_IssueLabel where whmi_code=N'" + QR_Code + "' \n ";
            string M_name = conn.ExcuteString(strQry3);
            if (QR_Code!="")
            {
                gvResult.ActiveFilterString = "[m_name]='" + M_name + "'";
                gvDetail.ActiveFilterString = "[m_name]='" + M_name + "'";
            }
        }

        private void frmWHMaterialCCFullBox_Load(object sender, EventArgs e)
        {
            txtBarcode.Focus();
            cboPlace.Text = "WH Material";
            Load_data(dtpCCDate.Value,"");
        }
    }
}
