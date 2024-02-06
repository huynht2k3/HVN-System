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

namespace HVN_System.View.QC
{
    public partial class frmQCFGInspectionNGPart : Form
    {
        public frmQCFGInspectionNGPart(P_Label_Entity _item)
        {
            InitializeComponent();
            item = _item;
            txtLabelCode.Text = item.Label_code;
            txtPN.Text = item.Product_customer_code;
            txtQty.Text = item.Product_quantity.ToString();
        }
        private P_Label_Entity item;
        private CmCn conn;
        private void frmQCFGInspectionNGPart_Load(object sender, EventArgs e)
        {
            txtQtyNG.Focus();
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (txtLabelCode.Text!="")
            {
                try
                {
                    if (int.Parse(txtQtyNG.Text)> int.Parse(txtQty.Text))
                    {
                        string strQry = "delete from QC_FG_NGPart where label_code=N'" + txtLabelCode.Text + "' and CAST(time_qc_check AS DATE)=N'" + DateTime.Today.ToString("yyyy-MM-dd") + "'\n";
                        strQry += "insert into QC_FG_NGPart ([label_code],[product_customer_code],[product_name],[product_quantity],[plan_date],[lot_no],[pic_qc],[time_qc_check],[ng_others])\n";
                        strQry += "select N'" + item.Label_code + "',N'" + item.Product_customer_code + "',N'" + item.Product_name + "',N'" + item.Product_quantity.ToString() +
                            "',N'" + item.Plan_date.ToString("yyyy-MM-dd") + "',N'" + item.Lot_no + "',N'" + item.Op_input_wh + "',getdate(),N'" + txtQtyNG.Text + "'";
                        conn = new CmCn();
                        conn.ExcuteQry(strQry);
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("LỖI SỐ LƯỢNG NG KHÔNG HỢP LỆ");
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("LỖI ĐỊNH DẠNG SỐ LƯỢNG NG");
                }
                
                
            }
        }
    }
}
