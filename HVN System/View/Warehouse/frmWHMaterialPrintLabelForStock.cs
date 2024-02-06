using HVN_System.Entity;
using HVN_System.Util;
using HVN_System.View.Planning;
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
    public partial class frmWHMaterialPrintLabelForStock : Form
    {
        public frmWHMaterialPrintLabelForStock()
        {
            InitializeComponent();
        }
        private ADO adoClass;
        private CmCn conn;
        private void frmWHMaterialPrintLabelForStock_Load(object sender, EventArgs e)
        {
            Load_combobox();
        }
        private void Load_combobox()
        {
            cboMaterial.Properties.DataSource = null;
            conn = new CmCn();
            string strQry = "Select m_name as [MATERIAL] from W_MasterList_Material where is_active=N'1'";
            DataTable dt = conn.ExcuteDataTable(strQry);
            cboMaterial.Properties.DataSource = dt;
            cboMaterial.Properties.DisplayMember = "MATERIAL";
            cboMaterial.Properties.ValueMember = "MATERIAL";
        }

        private void btnPrint_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            adoClass = new ADO();
            List<W_M_ReceiveLabel_Entity> List_Print = new List<W_M_ReceiveLabel_Entity>();
            int start_Id = Generate_Label_code();
            int stt = 1;
            int number_box = int.Parse(txtQtyBox.Text);
            for (int i = 1; i <= number_box; i++)   
            {
                W_M_ReceiveLabel_Entity item = new W_M_ReceiveLabel_Entity();
                item.Stt = stt;
                item.Whmr_code = "WHMR" + start_Id;
                item.M_name = cboMaterial.Text;
                item.Quantity = float.Parse(txtQuantity.Text);
                item.Lot_no = dtpLotNo.Value;
                item.Rm_doc_id = txtDocName.Text;
                item.Created_date = DateTime.Now;
                item.Created_user = General_Infor.username;
                item.IsSelected = true;
                List_Print.Add(item);
                start_Id++;
                stt++;
            }
            frmWHMaterial_ReceiveDocumentPrint frm = new frmWHMaterial_ReceiveDocumentPrint(List_Print,"stock");
            frm.ShowDialog();

        }
        private int Generate_Label_code()
        {
            string Qry = "SELECT MAX(whmr_code) FROM W_M_ReceiveLabel ";
            conn = new CmCn();
            DataTable dt = conn.ExcuteDataTable(Qry);
            if (dt.Rows[0][0].ToString() != "")
            {
                string max_value = dt.Rows[0][0].ToString().Substring(4, dt.Rows[0][0].ToString().Length - 4);
                return int.Parse(max_value) + 1;
            }
            else
            {
                return 10001;
            }
        }

        private void txtBarcode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode==Keys.Enter)
            {
                if (txtBarcode.Text.Length>6)
                {
                    string QR_code = "";
                    if (txtBarcode.Text.Substring(0,4)=="WHMR")
                    {
                        QR_code = txtBarcode.Text;
                    }
                    else
                    {
                        QR_code = txtBarcode.Text.Substring(2, txtBarcode.Text.Length - 2);
                    }
                    string strQry = "select * from W_M_ReceiveLabel where whmr_code=N'" +QR_code+ "'";
                    conn = new CmCn();
                    DataTable dt = conn.ExcuteDataTable(strQry);
                    W_M_ReceiveLabel_Entity item = new W_M_ReceiveLabel_Entity();
                    item.Whmr_code = dt.Rows[0]["whmr_code"].ToString();
                    item.M_name = dt.Rows[0]["m_name"].ToString();
                    item.Quantity = float.Parse(dt.Rows[0]["quantity"].ToString());
                    item.Created_date = string.IsNullOrEmpty(dt.Rows[0]["quantity"].ToString()) ? DateTime.Now : DateTime.Parse(dt.Rows[0]["created_time"].ToString());
                    if (item.Quantity > 0)
                    {
                        if (dt.Rows[0]["lot_no"].ToString()!="")
                        {
                            item.Lot_no = DateTime.Parse(dt.Rows[0]["lot_no"].ToString());
                            if (dt.Rows[0]["time_qc_check"].ToString()!="")
                            {
                                item.Time_qc_check = DateTime.Parse(dt.Rows[0]["time_qc_check"].ToString());
                            }
                            adoClass = new ADO();
                            adoClass.Print_W_M_ReceiveLabel(item, "QCOK");
                        }
                        else
                        {
                            adoClass = new ADO();
                            adoClass.Print_W_M_ReceiveLabel(item, "WH");
                        }
                        
                    }
                }
                txtBarcode.Text = "";
            }
        }

        private void txtBarcode2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode==Keys.Enter)
            {
                if (txtBarcode2.Text.Length > 6)
                {
                    string QR_code = "";
                    if (txtBarcode2.Text.Substring(0, 4) == "WHMI")
                    {
                        QR_code = txtBarcode2.Text;
                    }
                    else
                    {
                        QR_code = txtBarcode2.Text.Substring(2, txtBarcode2.Text.Length - 2);
                    }
                    string strQry = "select * from W_M_IssueLabel where whmi_code=N'" + QR_code + "'";
                    conn = new CmCn();
                    DataTable dt = conn.ExcuteDataTable(strQry);
                    W_M_IssueLabel_Entity item = new W_M_IssueLabel_Entity();
                    item.Whmi_code = dt.Rows[0]["whmi_code"].ToString();
                    item.M_name = dt.Rows[0]["m_name"].ToString();
                    item.Product_customer_code = dt.Rows[0]["product_customer_code"].ToString();
                    item.Quantity = float.Parse(dt.Rows[0]["quantity"].ToString());
                    item.Lot_no = string.IsNullOrEmpty(dt.Rows[0]["lot_no"].ToString()) ? DateTime.Now : DateTime.Parse(dt.Rows[0]["lot_no"].ToString());
                    item.Issue_date = string.IsNullOrEmpty(dt.Rows[0]["issue_date"].ToString()) ? DateTime.Now : DateTime.Parse(dt.Rows[0]["issue_date"].ToString());
                    item.P_shift = dt.Rows[0]["p_shift"].ToString();
                    item.P_line = dt.Rows[0]["p_line"].ToString();
                    item.Weight = string.IsNullOrEmpty(dt.Rows[0]["weight"].ToString()) ? 0:float.Parse(dt.Rows[0]["weight"].ToString());
                    item.Supply_date = string.IsNullOrEmpty(dt.Rows[0]["supply_date"].ToString()) ? DateTime.Now : DateTime.Parse(dt.Rows[0]["supply_date"].ToString());
                    item.M_doc_id = dt.Rows[0]["m_doc_id"].ToString();
                    item.Whmr_code = dt.Rows[0]["whmr_code"].ToString();
                    adoClass = new ADO();
                    adoClass.Print_W_M_IssueLabel(item);
                }
                txtBarcode2.Text = "";
            }
        }
    }
}
