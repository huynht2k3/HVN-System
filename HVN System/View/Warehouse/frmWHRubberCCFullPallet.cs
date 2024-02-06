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
    public partial class frmWHRubberCCFullPallet : Form
    {
        public frmWHRubberCCFullPallet()
        {
            InitializeComponent();
        }
        public frmWHRubberCCFullPallet(string PIC,DateTime Cc_date)
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
                    else if(txtBarcode.Text.Substring(2, 4) == "WHRR")
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
                        lbError.Text = QR_code+":KHÔNG PHẢI TEM TRONG KHO";
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
            string strQry = " delete from W_R_CCInventory where whrr_code=N'" + QRCode + "' and cc_date=N'" + dtpCCDate.Value.ToString("yyyy-MM-dd") + "'";
            conn.ExcuteQry(strQry);
        }
        private void Add_Item(string QRCode)
        {
            if (txtPIC.Text=="")
            {
                lbError.Text = "LỖI: BẠN CHƯA QUÉT MÃ NHÂN VIÊN";
                return;
            }
            string qry = "select r_name from  W_R_CCInventory where whrr_code=N'"+QRCode+ "' and cc_date=N'"+ dtpCCDate.Value.ToString("yyyy-MM-dd") + "'";
            conn = new CmCn();
            if (string.IsNullOrEmpty(conn.ExcuteString(qry)))
            {
                string strQry = " insert into W_R_CCInventory (cc_date,whrr_code,r_name,weight,place,pic,time_commit,r_kind) \n ";
                strQry += " select N'"+dtpCCDate.Value.ToString("yyyy-MM-dd")+"',whrr_code,r_name,weight \n ";
                strQry += " ,N'" + cboPlace.Text + "',N'" + txtPIC.Text + "',getdate(),N'FULL PALLET' \n ";
                strQry += " from W_M_RubberLabel where whrr_code=N'" + QRCode + "' \n ";
                conn.ExcuteQry(strQry);
            }
            else
            {
                lbError.Text = QRCode + ":THÙNG ĐÃ ĐƯỢC KIỂM";
            }
        }
        private void Load_data(DateTime Cc_date,string QR_Code)
        {
            string strQry = "select place,r_name,sum(weight) as total_weight,count(r_name) as qty_box from W_R_CCInventory \n ";
            if (txtPIC.Text == "")
            {
                strQry += " where cc_date=N'" + dtpCCDate.Value.ToString("yyyy-MM-dd") + "' and r_kind=N'FULL PALLET' \n ";
            }
            else
            {
                strQry += " where cc_date=N'" + dtpCCDate.Value.ToString("yyyy-MM-dd") + "' and r_kind=N'FULL PALLET' and pic=N'" + txtPIC.Text.ToUpper() + "' \n ";
            }
            strQry += " group by place,r_name \n ";
            conn = new CmCn();
            dgvResult.DataSource = conn.ExcuteDataTable(strQry);
            string strQry2 = "select * from W_R_CCInventory \n ";
            
            if (txtPIC.Text == "")
            {
                strQry2 += " where cc_date=N'" + dtpCCDate.Value.ToString("yyyy-MM-dd") + "' and r_kind=N'FULL PALLET' \n ";
            }
            else
            {
                strQry2 += " where cc_date=N'" + dtpCCDate.Value.ToString("yyyy-MM-dd") + "' and r_kind=N'FULL PALLET' and pic=N'" + txtPIC.Text.ToUpper() + "' \n ";
            }
            DataTable dt = conn.ExcuteDataTable(strQry2);
            dgvDetail.DataSource= dt;
            txtTotalPallet.Text = dt.Rows.Count.ToString();
            string strQry3= "select r_name from W_M_RubberLabel where whrr_code=N'" + QR_Code + "' \n ";
            string r_name = conn.ExcuteString(strQry3);
            if (QR_Code!="")
            {
                gvResult.ActiveFilterString = "[r_name]='" + r_name + "'";
                gvDetail.ActiveFilterString = "[r_name]='" + r_name + "'";
            }
        }

        private void frmWHMaterialCCFullBox_Load(object sender, EventArgs e)
        {
            txtBarcode.Focus();
            cboPlace.Text = "WH Rubber";
            Load_data(dtpCCDate.Value,"");
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("BẠN CÓ MUỐN LƯU THÀNH BÁO CÁO?\n DO YOU WANT TO SUBMIT REPORT?", "SUBMIT REPORT", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (dtpCCDate.Value >= DateTime.Today)
                {
                    string strQry = "delete from W_R_CCResult where cc_date=N'" + dtpCCDate.Value.ToString("yyyy-MM-dd") + "' \n ";
                    strQry += " insert into W_R_CCResult(cc_date,whrr_code,r_name,sys_weight,cc_weight,sys_place,cc_place,label_status) \n ";
                    strQry += " select N'" + dtpCCDate.Value.ToString("yyyy-MM-dd") + "',a.whrr_code,a.r_name,b.sys_weight,c.cc_weight,b.sys_place,c.cc_place \n ";
                    strQry += " ,case \n ";
                    strQry += "       when b.sys_place<>c.cc_place then N'Mismatch place'  \n ";
                    strQry += "       when c.cc_place is null then N'Product found in system but not found during cycle count'  \n ";
                    strQry += "       when b.sys_place is null then N'Product found in cycle count but not found during system'  \n ";
                    strQry += "       when b.sys_weight<>c.cc_weight then N'Mismatch weight' \n ";
                    strQry += "       else N'OK'  \n ";
                    strQry += "       end as label_status \n ";
                    strQry += " from  \n ";
                    strQry += " (SELECT whrr_code,r_name from W_M_RubberLabel where place =N'WH Rubber' and weight>0 \n ";
                    strQry += " union select whrr_code,r_name from W_R_CCInventory where cc_date=N'" + dtpCCDate.Value.ToString("yyyy-MM-dd") + "') a \n ";
                    strQry += " left join \n ";
                    strQry += " (select whrr_code,place as sys_place,weight as sys_weight from W_M_RubberLabel where place =N'WH Rubber' and weight>0) b \n ";
                    strQry += " on a.whrr_code=b.whrr_code \n ";
                    strQry += " left join \n ";
                    strQry += " (select whrr_code,place as cc_place,weight as cc_weight from W_R_CCInventory where cc_date=N'" + dtpCCDate.Value.ToString("yyyy-MM-dd") + "') c \n ";
                    strQry += " on a.whrr_code=c.whrr_code \n ";

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
    }
}
