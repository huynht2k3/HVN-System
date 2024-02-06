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
    public partial class frmWHCCAdjustmentApprove : Form
    {
        public frmWHCCAdjustmentApprove()
        {
            InitializeComponent();
        }
        public frmWHCCAdjustmentApprove(string cc_name_, string _type)
        {
            InitializeComponent();
            txtCcName.Text = cc_name_;
            type = _type;
        }
        private CmCn conn;
        private ADO adoClass;
        private string type;
        private List<W_CycleCount_Adjustment_Entity> List_Data;
        private void frmWHCCAdjustment_Load(object sender, EventArgs e)
        {
            switch (type)
            {
                case "FG":
                    Load_FG_adjustment();
                    break;
                case "Material":
                    Load_Material_adjustment();
                    break;
                case "Rubber":
                    Load_Rubber_adjustment();
                    break;
                default:
                    break;
            }
        }
        private void Load_FG_adjustment()
        {
            string strQry = "select * from W_CycleCount_Adjustment  \n ";
            strQry += " where cc_name=N'" + txtCcName.Text + "' \n ";
            conn = new CmCn();
            DataTable dt_cc = conn.ExcuteDataTable(strQry);
            List_Data = new List<W_CycleCount_Adjustment_Entity>();
            foreach (DataRow row in dt_cc.Rows)
            {
                W_CycleCount_Adjustment_Entity item = new W_CycleCount_Adjustment_Entity();
                item.Cc_name = row["cc_name"].ToString();
                item.Label_code = row["label_code"].ToString();
                item.Product_customer_code = row["product_customer_code"].ToString();
                item.Product_quantity = float.Parse(row["product_quantity"].ToString());
                item.Location = row["location"].ToString();
                item.Place = row["place"].ToString();
                item.Transaction = row["transaction"].ToString();
                item.Input_time = DateTime.Parse(row["input_time"].ToString());
                item.Pic = row["pic"].ToString();
                List_Data.Add(item);
            }
            dgvDetail.DataSource = List_Data.ToList();
            string strQry2 = "select product_customer_code,[transaction],sum(product_quantity) as product_quantity   \n ";
            strQry2 += " from W_CycleCount_Adjustment  \n ";
            strQry2 += " where cc_name=N'" + txtCcName.Text + "' \n ";
            strQry2 += " group by product_customer_code,[transaction] \n ";
            dgvResult.DataSource = conn.ExcuteDataTable(strQry2);
        }
        private void Load_Material_adjustment()
        {
            string strQry = "select * from [W_M_CycleCount_Adjustment]  \n ";
            strQry += " where cc_name=N'" + txtCcName.Text + "' \n ";
            conn = new CmCn();
            DataTable dt_cc = conn.ExcuteDataTable(strQry);
            List_Data = new List<W_CycleCount_Adjustment_Entity>();
            foreach (DataRow row in dt_cc.Rows)
            {
                W_CycleCount_Adjustment_Entity item = new W_CycleCount_Adjustment_Entity();
                item.Cc_name = row["cc_name"].ToString();
                item.Label_code = row["whmr_code"].ToString();
                item.Product_customer_code = row["m_name"].ToString();
                item.Product_quantity = float.Parse(row["quantity"].ToString());
                item.Location = row["location"].ToString();
                item.Place = row["place"].ToString();
                item.Transaction = row["transaction"].ToString();
                item.Input_time = DateTime.Parse(row["input_time"].ToString());
                item.Pic = row["pic"].ToString();
                List_Data.Add(item);
            }
            dgvDetail.DataSource = List_Data.ToList();
            string strQry2 = "select m_name as product_customer_code,[transaction],sum(quantity) as product_quantity   \n ";
            strQry2 += " from W_M_CycleCount_Adjustment  \n ";
            strQry2 += " where cc_name=N'" + txtCcName.Text + "' \n ";
            strQry2 += " group by m_name,[transaction] \n ";
            dgvResult.DataSource = conn.ExcuteDataTable(strQry2);
        }
        private void Load_Rubber_adjustment()
        {
            string strQry = "select * from [W_R_CycleCount_Adjustment]  \n ";
            strQry += " where cc_name=N'" + txtCcName.Text + "' \n ";
            conn = new CmCn();
            DataTable dt_cc = conn.ExcuteDataTable(strQry);
            List_Data = new List<W_CycleCount_Adjustment_Entity>();
            foreach (DataRow row in dt_cc.Rows)
            {
                W_CycleCount_Adjustment_Entity item = new W_CycleCount_Adjustment_Entity();
                item.Cc_name = row["cc_name"].ToString();
                item.Label_code = row["whrr_code"].ToString();
                item.Product_customer_code = row["r_name"].ToString();
                item.Product_quantity = float.Parse(row["weight"].ToString());
                item.Location = row["location"].ToString();
                item.Place = row["place"].ToString();
                item.Transaction = row["transaction"].ToString();
                item.Input_time = DateTime.Parse(row["input_time"].ToString());
                item.Pic = row["pic"].ToString();
                List_Data.Add(item);
            }
            dgvDetail.DataSource = List_Data.ToList();
            string strQry2 = "select r_name as product_customer_code,[transaction],sum(weight) as product_quantity   \n ";
            strQry2 += " from W_R_CycleCount_Adjustment  \n ";
            strQry2 += " where cc_name=N'" + txtCcName.Text + "' \n ";
            strQry2 += " group by r_name,[transaction] \n ";
            dgvResult.DataSource = conn.ExcuteDataTable(strQry2);
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
            if (MessageBox.Show("Do you want to approve this request?", "Approve FG adjustment", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                string strQry = "";
                if (type == "FG")
                {
                    foreach (W_CycleCount_Adjustment_Entity item in List_Data)
                    {

                        if (item.Transaction == "Stock adjustment - remove")
                        {
                            strQry += "update P_label set date_input_wh=null,wh_location=null,place=null,pallet_no=null where label_code=N'" + item.Label_code + "' \n";
                        }
                        else
                        {
                            strQry += "update P_label set date_input_wh=getdate(),wh_location=N'" + item.Location + "',place=N'" + item.Place + "' where label_code=N'" + item.Label_code + "' \n";
                        }
                    }
                    strQry += " insert into W_HistoryOfTransaction  \n ";
                    strQry += " (label_code,product_customer_code,product_quantity,[transaction],[place],input_time,PIC,comment) \n ";
                    strQry += " select label_code,product_customer_code,product_quantity,[transaction],[place],getdate(),PIC,N'" + General_Infor.username + " approved'  \n ";
                    strQry += " from W_CycleCount_Adjustment  \n ";
                    strQry += " where cc_name=N'" + txtCcName.Text + "' \n ";
                    strQry += " delete from W_CycleCount_Adjustment where cc_name=N'" + txtCcName.Text + "' \n ";
                }
                else if (type == "Material")
                {
                    strQry += " update W_M_ReceiveLabel set place=null where whmr_code in \n ";
                    strQry += "   (select whmr_code from W_M_CycleCount_Adjustment   \n ";
                    strQry += "   where cc_name=N'" + txtCcName.Text + "' and [transaction]=N'Stock adjustment - remove') \n ";

                    strQry += " update W_M_ReceiveLabel set place=N'WH Material' where whmr_code in \n ";
                    strQry += "   (select whmr_code from W_M_CycleCount_Adjustment   \n ";
                    strQry += "   where cc_name=N'" + txtCcName.Text + "' and [transaction]=N'Stock adjustment - add') \n ";

                    strQry += " insert into W_M_HistoryOfTransaction  \n "; 
                    strQry += " (whmr_code,m_name,quantity,[transaction],[place],input_time,PIC,m_note) \n ";
                    strQry += " select whmr_code,m_name,quantity,[transaction],[place],getdate(),PIC,N'" + General_Infor.username + " approved'  \n ";
                    strQry += " from W_M_CycleCount_Adjustment  \n ";
                    strQry += " where cc_name=N'" + txtCcName.Text + "' \n ";
                    strQry += " delete from W_M_CycleCount_Adjustment where cc_name=N'" + txtCcName.Text + "' \n ";
                }
                else
                {
                    strQry += " update W_M_RubberLabel set place=null where whrr_code in \n ";
                    strQry += "   (select whrr_code from W_R_CycleCount_Adjustment   \n ";
                    strQry += "   where cc_name=N'" + txtCcName.Text + "' and [transaction]=N'Stock adjustment - remove') \n ";

                    strQry += " update W_M_RubberLabel set place=N'WH Rubber' where whrr_code in \n ";
                    strQry += "   (select whrr_code from W_R_CycleCount_Adjustment   \n ";
                    strQry += "   where cc_name=N'" + txtCcName.Text + "' and [transaction]=N'Stock adjustment - add') \n ";

                    strQry += " insert into W_M_RubberTransaction  \n ";
                    strQry += " (whrr_code,r_name,weight,[transaction],[place],input_time,PIC,r_note) \n ";
                    strQry += " select whrr_code,r_name,weight,[transaction],[place],getdate(),PIC,N'" + General_Infor.username + " approved'  \n ";
                    strQry += " from W_R_CycleCount_Adjustment  \n ";
                    strQry += " where cc_name=N'" + txtCcName.Text + "' \n ";
                    strQry += " delete from W_R_CycleCount_Adjustment where cc_name=N'" + txtCcName.Text + "' \n ";
                }
                conn = new CmCn();
                try
                {
                    if (List_Data.Count > 0)
                    {
                        conn.ExcuteQry(strQry);
                        string strQry2 = "select Email_address from Account where Username=N'" + List_Data[0].Pic + "'";
                        adoClass = new ADO();
                        string subject = "[HVN System][Inventory][Stock adjustment]:" + type + " inventory";
                        string to = conn.ExcuteString(strQry);
                        string body = "Dear,\n\n Your request has been approved.\nCycle count name:" + txtCcName.Text;
                        adoClass.SendEmail(subject, to, "quentin.hovsepian@hutchinson.com", body);
                        MessageBox.Show("The request has been approved");
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("There is not any information need to adjust");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

        }

        private void btnReject_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (MessageBox.Show("Do you want to reject this request?", "Reject FG adjustment", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                string strQry = "";
                if (type == "FG")
                {
                    strQry = " delete from W_CycleCount_Adjustment where cc_name=N'" + txtCcName.Text + "' \n ";
                }
                else if (type == "Material")
                {
                    strQry = " delete from [W_M_CycleCount_Adjustment] where cc_name=N'" + txtCcName.Text + "' \n ";
                }
                else
                {
                    strQry = " delete from [W_R_CycleCount_Adjustment] where cc_name=N'" + txtCcName.Text + "' \n ";
                }
                conn = new CmCn();
                try
                {
                    conn.ExcuteQry(strQry);
                    MessageBox.Show("The request has been rejected");
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}
