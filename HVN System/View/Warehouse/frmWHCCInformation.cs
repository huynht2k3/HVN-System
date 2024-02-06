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
    public partial class frmWHCCInformation : Form
    {
        public frmWHCCInformation()
        {
            InitializeComponent();
        }
        public frmWHCCInformation(W_CycleCount_Entity _cc)
        {
            InitializeComponent();
            txtCCName.Text = _cc.Cc_name;
            dtpCCDate.Value = _cc.Cc_date;
            txtCCDes.Text = _cc.Cc_des;
            cboCCType.Text = _cc.Cc_type;
            if (cboCCType.Text == "Partial cycle count")
            {
                layoutControlItem6.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                
            }
            else
            {
                layoutControlItem6.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            }
            txtCCName.ReadOnly = true;
            cboCCType.Enabled = false;
            isEdit = true;
        }
        bool isEdit = false;
        private ADO adoClass;
        private CmCn conn;
        private List<P_FG_Entity> List_Parital_PN;
        private List<W_CycleCountArea_Entity> List_CC_Place;
        private void frmWHCCInformation_Load(object sender, EventArgs e)
        {
            if (isEdit)
            {
                List_Parital_PN = new List<P_FG_Entity>();
                adoClass = new ADO();
                DataTable dt = adoClass.Load_Cycle_Count_Partial_Item(txtCCName.Text);
                foreach (DataRow row in dt.Rows)
                {
                    P_FG_Entity item = new P_FG_Entity();
                    item.Product_customer_code = row["product_customer_code"].ToString();
                    item.Product_code = row["product_code"].ToString();
                    item.Edit = string.IsNullOrEmpty(row["selected"].ToString()) ? false : true;
                    List_Parital_PN.Add(item);
                }
                dgvResult.DataSource = List_Parital_PN.ToList();
                List_CC_Place = new List<W_CycleCountArea_Entity>();
                conn = new CmCn();
                string strQry = "select * from";
                strQry += " (select child_name from ADM_MasterListParameter where parent_id=N'wh_place') a \n ";
                strQry += " left join \n";
                strQry += " (select *,1 as isSelect from W_CycleCountArea where cc_name=N'" + txtCCName.Text + "') b\n";
                strQry += " on a.child_name=b.place \n";
                DataTable dt2 = conn.ExcuteDataTable(strQry);
                foreach (DataRow row in dt2.Rows)
                {
                    W_CycleCountArea_Entity item = new W_CycleCountArea_Entity();
                    item.Place = row["child_name"].ToString();
                    item.IsSelected = string.IsNullOrEmpty(row["isSelect"].ToString()) ? false : true; ;
                    List_CC_Place.Add(item);
                }
                dgvCCPlace.DataSource = List_CC_Place.ToList();
            }
            else
            {
                cboCCType.Text = "All PN cycle count";
                layoutControlItem6.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                List_Parital_PN = new List<P_FG_Entity>();
                adoClass = new ADO();
                DataTable dt = adoClass.Load_MasterListProduct("product_customer_code,product_code", "");
                foreach (DataRow row in dt.Rows)
                {
                    P_FG_Entity item = new P_FG_Entity();
                    item.Product_customer_code = row["product_customer_code"].ToString();
                    item.Product_code = row["product_code"].ToString();
                    item.Edit = false;
                    List_Parital_PN.Add(item);
                }
                dgvResult.DataSource = List_Parital_PN.ToList();
                List_CC_Place = new List<W_CycleCountArea_Entity>();
                string strQry = "select child_name from ADM_MasterListParameter where parent_id=N'wh_place'";
                conn = new CmCn();
                DataTable dt2 = conn.ExcuteDataTable(strQry);
                foreach (DataRow row in dt2.Rows)
                {
                    W_CycleCountArea_Entity item = new W_CycleCountArea_Entity();
                    item.Place = row["child_name"].ToString();
                    item.IsSelected = false;
                    List_CC_Place.Add(item);
                }
                dgvCCPlace.DataSource = List_CC_Place.ToList();
            }
        }

        private void cboCCType_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (cboCCType.Text == "Partial cycle count")
            {
                layoutControlItem6.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
            }
            else
            {
                layoutControlItem6.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            }
        }

        private void btnSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (MessageBox.Show("Do you want to save?", "Save", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                W_CycleCount_Entity CC_item = new W_CycleCount_Entity();
                CC_item.Cc_name = txtCCName.Text;
                CC_item.Cc_type = cboCCType.Text;
                CC_item.Cc_date = dtpCCDate.Value;
                CC_item.Cc_des = txtCCDes.Text;
                adoClass = new ADO();
                if (isEdit)
                {
                    adoClass.Update_Cycle_Count_Information(CC_item);
                    if (CC_item.Cc_type == "Partial cycle count")
                    {
                        List<W_CycleCountListPartial_Entity> List_Parital_CC = new List<W_CycleCountListPartial_Entity>();
                        foreach (P_FG_Entity row in List_Parital_PN)
                        {
                            if (row.Edit == true)
                            {
                                W_CycleCountListPartial_Entity item = new W_CycleCountListPartial_Entity();
                                item.Cc_name = txtCCName.Text;
                                item.Product_code = row.Product_code;
                                item.Product_customer_code = row.Product_customer_code;
                                List_Parital_CC.Add(item);
                            }
                        }
                        adoClass.Update_Cycle_Count_Partial(List_Parital_CC);
                    }
                    List<W_CycleCountArea_Entity> List_Parital_Place = new List<W_CycleCountArea_Entity>();
                    foreach (W_CycleCountArea_Entity row in List_CC_Place)
                    {
                        if (row.IsSelected == true)
                        {
                            row.Cc_name = txtCCName.Text;
                            List_Parital_Place.Add(row);
                        }
                    }
                    adoClass.Update_Cycle_Count_Place(List_Parital_Place);
                    MessageBox.Show("Saving successfully");
                    this.Close();
                }
                else
                {
                    DataTable dt_check = adoClass.Load_W_CycleCount("", "cc_name=N'" + CC_item.Cc_name + "'");
                    try
                    {
                        if (dt_check.Rows.Count == 0)
                        {
                            adoClass.Update_Cycle_Count_Information(CC_item);
                            if (CC_item.Cc_type == "Partial cycle count")
                            {
                                List<W_CycleCountListPartial_Entity> List_Parital_CC = new List<W_CycleCountListPartial_Entity>();
                                foreach (P_FG_Entity row in List_Parital_PN)
                                {
                                    if (row.Edit == true)
                                    {
                                        W_CycleCountListPartial_Entity item = new W_CycleCountListPartial_Entity();
                                        item.Cc_name = txtCCName.Text;
                                        item.Product_code = row.Product_code;
                                        item.Product_customer_code = row.Product_customer_code;
                                        List_Parital_CC.Add(item);
                                    }
                                }
                                adoClass.Update_Cycle_Count_Partial(List_Parital_CC);
                            }
                            List<W_CycleCountArea_Entity> List_Parital_Place = new List<W_CycleCountArea_Entity>();
                            foreach (W_CycleCountArea_Entity row in List_CC_Place)
                            {
                                if (row.IsSelected == true)
                                {
                                    row.Cc_name = txtCCName.Text;
                                    List_Parital_Place.Add(row);
                                }
                            }
                            adoClass.Update_Cycle_Count_Place(List_Parital_Place);
                            MessageBox.Show("Saving successfully");
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("The cycle count name was exist, please choose other name");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }

            }
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            foreach (P_FG_Entity row in List_Parital_PN)
            {
                row.Edit = true;
            }
            dgvCCPlace.DataSource = List_Parital_PN.ToList();
        }

        private void repositoryItemCheckEdit1_EditValueChanged(object sender, EventArgs e)
        {
            if (gvResult.PostEditor())
            {
                gvResult.UpdateCurrentRow();
            }
        }

        private void repositoryItemCheckEdit2_EditValueChanged(object sender, EventArgs e)
        {
            if (gvCCPlace.PostEditor())
            {
                gvCCPlace.UpdateCurrentRow();
            }
        }
    }
}
