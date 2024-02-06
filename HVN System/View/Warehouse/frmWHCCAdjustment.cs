using DevExpress.XtraGrid.Views.Grid;
using HVN_System.Entity;
using HVN_System.Util;
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
    public partial class frmWHCCAdjustment : Form
    {
        public frmWHCCAdjustment()
        {
            InitializeComponent();
        }
        public frmWHCCAdjustment(DataTable dt_cc_,string cc_name_,string _kind_cc)
        {
            InitializeComponent();
            dt_cc = dt_cc_;
            txtCcName.Text = cc_name_;
            kind_cc = _kind_cc;
        }
        private CmCn conn;
        private ADO adoClass;
        DataTable dt_cc;
        private string kind_cc;
        private List<WHCC_Analys_Entity> List_Data;
        private void frmWHCCAdjustment_Load(object sender, EventArgs e)
        {
            List_Data = new List<WHCC_Analys_Entity>();
            foreach (DataRow row in dt_cc.Rows)
            {
                if (kind_cc=="FG")
                {
                    if (row["label_status"].ToString() == "Product found in system but not found during cycle count" || row["label_status"].ToString() == "Product found in cycle count but not found during system")
                    {
                        WHCC_Analys_Entity item = new WHCC_Analys_Entity();
                        item.Select_item = false;
                        item.Sys_location = row["sys_location"].ToString();
                        item.Sys_pallet_no = row["sys_pallet_no"].ToString();
                        item.Sys_place = row["sys_place"].ToString();
                        item.Cc_location = row["cc_location"].ToString();
                        item.Cc_pallet_no = row["cc_pallet_no"].ToString();
                        item.Cc_place = row["cc_place"].ToString();
                        item.Label_code = row["label_code"].ToString();
                        item.Label_status = row["label_status"].ToString();
                        item.Product_customer_code = row["product_customer_code"].ToString();
                        item.Product_quantity = float.Parse(row["product_quantity"].ToString());
                        List_Data.Add(item);
                    }
                }
                else if (kind_cc == "Material")
                {
                    if (row["label_status"].ToString() == "Product found in system but not found during cycle count" || row["label_status"].ToString() == "Product found in cycle count but not found during system")
                    {
                        WHCC_Analys_Entity item = new WHCC_Analys_Entity();
                        item.Select_item = false;
                        item.Sys_place = row["sys_place"].ToString();
                        item.Cc_place = row["cc_place"].ToString();
                        item.Label_code = row["whmr_code"].ToString();
                        item.Label_status = row["label_status"].ToString();
                        item.Product_customer_code = row["m_name"].ToString();
                        float sys_qty=string.IsNullOrEmpty(row["sys_qty"].ToString())?0: float.Parse(row["sys_qty"].ToString());
                        float cc_qty = string.IsNullOrEmpty(row["cc_qty"].ToString()) ? 0 : float.Parse(row["cc_qty"].ToString());
                        if (row["label_status"].ToString() == "Product found in system but not found during cycle count")
                        {
                            item.Product_quantity = sys_qty;
                        }
                        else
                        {
                            item.Product_quantity = cc_qty;
                        }
                        List_Data.Add(item);
                    }
                }
                else
                {
                    if (row["label_status"].ToString() == "Product found in system but not found during cycle count" || row["label_status"].ToString() == "Product found in cycle count but not found during system")
                    {
                        WHCC_Analys_Entity item = new WHCC_Analys_Entity();
                        item.Select_item = false;
                        item.Sys_place = row["sys_place"].ToString();
                        item.Cc_place = row["cc_place"].ToString();
                        item.Label_code = row["whrr_code"].ToString();
                        item.Label_status = row["label_status"].ToString();
                        item.Product_customer_code = row["r_name"].ToString();
                        float sys_weight = string.IsNullOrEmpty(row["sys_weight"].ToString()) ? 0 : float.Parse(row["sys_weight"].ToString());
                        float cc_weight = string.IsNullOrEmpty(row["cc_weight"].ToString()) ? 0 : float.Parse(row["cc_weight"].ToString());
                        if (row["label_status"].ToString() == "Product found in system but not found during cycle count")
                        {
                            item.Product_quantity = sys_weight;
                        }
                        else
                        {
                            item.Product_quantity = cc_weight;
                        }
                        List_Data.Add(item);
                    }
                }
            }
            dgvResult.DataSource = List_Data.ToList();
        }

        private void repositoryItemCheckEdit1_CheckedChanged(object sender, EventArgs e)
        {
            if (gvResult.PostEditor())
            {
                gvResult.UpdateCurrentRow();
            }
        }

        private void btnAdjust_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (MessageBox.Show("Do you make sure submit request to adjust as selected item?", "Adjust stock", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                string strQry = "";
                string qry2 = "";
                foreach (WHCC_Analys_Entity item in List_Data)
                {
                    if (item.Select_item == true)
                    {
                        if (qry2!="")
                        {
                            qry2 += "union all ";
                        }
                        string transaction = "";
                        if (item.Label_status== "Product found in system but not found during cycle count")
                        {
                            transaction = "Stock adjustment - remove";
                            qry2 += "select N'" + txtCcName.Text + "',N'" + item.Label_code + "',N'" + item.Product_customer_code + "',N'" + item.Product_quantity + "',N'" + transaction + "',N'" + item.Sys_place + "',N'" + item.Sys_location + "',getdate(),N'" + General_Infor.username + "'\n";
                        }
                        else
                        {
                            transaction = "Stock adjustment - add";
                            qry2 += "select N'" + txtCcName.Text + "',N'" + item.Label_code + "',N'" + item.Product_customer_code + "',N'" + item.Product_quantity + "',N'" + transaction + "',N'" + item.Cc_place + "',N'" + item.Cc_location + "',getdate(),N'" + General_Infor.username + "'\n";
                        }
                        
                    }
                }
                if (kind_cc=="FG")
                {
                    strQry += "delete from W_CycleCount_Adjustment where cc_name=N'" + txtCcName.Text + "'\n";
                    strQry += "insert into W_CycleCount_Adjustment(cc_name,label_code,product_customer_code,product_quantity,[transaction],[place],[location],input_time,PIC) \n";
                }
                else if (kind_cc=="Material")
                {
                    strQry += "delete from W_M_CycleCount_Adjustment where cc_name=N'" + txtCcName.Text + "'\n";
                    strQry += "insert into W_M_CycleCount_Adjustment (cc_name,[whmr_code],[m_name],[quantity],[transaction],[place],[location],input_time,PIC) \n";
                }
                else
                {
                    strQry += "delete from W_R_CycleCount_Adjustment where cc_name=N'" + txtCcName.Text + "'\n";
                    strQry += "insert into W_R_CycleCount_Adjustment (cc_name,[whrr_code],[r_name],[weight],[transaction],[place],[location],input_time,PIC) \n";
                }
                strQry += qry2;
                conn = new CmCn();
                try
                {
                    conn.ExcuteQry(strQry);
                    adoClass = new ADO();
                    string subject = "[HVN System][Inventory][Stock adjustment]:" + kind_cc + " inventory";
                    string to = adoClass.List_email_of_PIC("procedure_name=N'Inventory approval' and step_name=N'receive email'");
                    string body = "Dear,\n\n Please check and approve for the request adjusting " + kind_cc + " inventory on "+txtCcName.Text+"\nCycle count name:" + txtCcName.Text;
                    adoClass.SendEmail(subject, to, "", body);
                    MessageBox.Show("the request has been submit");
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
                
        }

        private void btnSelectAll_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            foreach (WHCC_Analys_Entity item in List_Data)
            {
                item.Select_item = true;
            }
            dgvResult.DataSource = List_Data.ToList();
        }
    }
}
