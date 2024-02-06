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

namespace HVN_System.View.PUR
{
    public partial class frmPURMasterListSupplier : Form
    {
        public frmPURMasterListSupplier()
        {
            InitializeComponent();
        }
        private ADO adoClass;
        private CmCn conn;
        List<PUR_MasterListSupplier_Entity> List_Data;
        private PUR_MasterListSupplier_Entity current_item;
        private bool isAddNew = true;
        private void gvResult_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            current_item = gvResult.GetRow(gvResult.FocusedRowHandle) as PUR_MasterListSupplier_Entity;
            txtSupplierName.Text = current_item.Supplier_name;
            txtShortname.Text= current_item.Sup_shortname;
            txtAddress.Text = current_item.Sup_address;
            txtTelNo.Text = current_item.Sup_tel;
            txtTaxCode.Text = current_item.Tax_code;
            txtContact.Text = current_item.Contact_pic;
            txtEmail.Text = current_item.Email_address;
            cboCurrency.Text = current_item.Sup_currency;
            txtDeliveryMode.Text = current_item.Delivery_mode;
            txtIncoTerm.Text = current_item.Incoterm;
            txtPaymentTerm.Text= current_item.Payment_term;
            txtSupplierName.ReadOnly = true;
            isAddNew = false;
        }

        private void frmWHCCManagement_Load(object sender, EventArgs e)
        {
            txtTaxCode.Text = "0";
            txtContact.Text = "0";
            txtSupplierName.ReadOnly = true;
            Load_Data();
            adoClass = new ADO();
            btnAddNew.Enabled = adoClass.Check_permission(this.Name, btnAddNew.Name, General_Infor.username);
            btnDelete.Enabled = adoClass.Check_permission(this.Name, btnDelete.Name, General_Infor.username);
            btnSave.Enabled = adoClass.Check_permission(this.Name, btnSave.Name, General_Infor.username);
        }
        private void Load_Data()
        {
            current_item = new PUR_MasterListSupplier_Entity();
            string strQry = "select * from PUR_MasterListSupplier";
            conn = new CmCn();
            DataTable dt = conn.ExcuteDataTable(strQry);
            List_Data = new List<PUR_MasterListSupplier_Entity>();
            foreach (DataRow row in dt.Rows)
            {
                PUR_MasterListSupplier_Entity item = new PUR_MasterListSupplier_Entity();
                item.Supplier_name = row["Supplier_name"].ToString();
                item.Sup_address = row["Sup_address"].ToString();
                item.Sup_currency = row["Sup_currency"].ToString();
                item.Sup_shortname = row["Sup_shortname"].ToString();
                item.Sup_tel = row["Sup_tel"].ToString();
                item.Email_address = row["Email_address"].ToString();
                item.Contact_pic = row["Contact_pic"].ToString();
                item.Tax_code = row["Tax_code"].ToString();
                item.Payment_term = row["Payment_term"].ToString();
                item.Incoterm = row["Incoterm"].ToString();
                item.Delivery_mode = row["Delivery_mode"].ToString();
                List_Data.Add(item);
            }
            dgvResult.DataSource = List_Data.ToList();
        }
        private void gvResult_DoubleClick(object sender, EventArgs e)
        {

        }

        private void btnSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (MessageBox.Show("Do you want to save data for : " + txtSupplierName.Text + " ?", "Save Data", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                string strQry = "";
                if (isAddNew)
                {
                    strQry += "insert into PUR_MasterListSupplier (supplier_name,sup_shortname,sup_address,sup_tel" +
                        ",tax_code,contact_pic,email_address,sup_currency" +
                        ",payment_term,delivery_mode,incoterm,supplier_status) \n ";
                    strQry += "select N'"+txtSupplierName.Text+ "',N'" + txtShortname.Text + "',N'" + txtAddress.Text + "'," +"N'" + txtTelNo.Text 
                        + "',N'" + txtTaxCode.Text + "',N'" + txtContact.Text + "'," +"N'" + txtEmail.Text+ "',N'" + cboCurrency.Text
                        + "',N'" + txtPaymentTerm.Text + "',N'" + txtDeliveryMode.Text + "',N'" + txtIncoTerm.Text + "',N'Active'";
                }
                else
                {
                    strQry = "update PUR_MasterListSupplier set  \n ";
                    strQry += " sup_shortname=N'" + txtShortname.Text + "',sup_address=N'" + txtAddress.Text + "',sup_tel=N'" + txtTelNo.Text + "' \n ";
                    strQry += " ,tax_code=N'" + txtTaxCode.Text + "',contact_pic=N'" + txtContact.Text + "',email_address=N'" + txtEmail.Text + "',sup_currency=N'" + cboCurrency.Text+"' \n ";
                    strQry += " ,payment_term=N'" + txtPaymentTerm.Text + "',delivery_mode=N'" + txtDeliveryMode.Text + "',incoterm=N'" + txtIncoTerm.Text + "' \n ";
                    strQry += " where supplier_name=N'" + txtSupplierName.Text + "' \n ";
                }
                conn = new CmCn();
                try
                {
                    conn.ExcuteQry(strQry);
                    txtSupplierName.ReadOnly = true;
                    Load_Data();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void btnRefresh_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Load_Data();
        }

        private void btnExport_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            adoClass = new ADO();
            adoClass.Export_Excel(dgvResult);
        }

        private void btnDelete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (!string.IsNullOrEmpty(current_item.Supplier_name))
            {
                if (MessageBox.Show("Do you want to inactive supplier: "+ current_item.Supplier_name + " ?", "Inactive Item", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    string strQry= "update PUR_MasterListSupplier set sup_status=N'inactive' where supplier_name=N'" + current_item.Supplier_name + "'\n";
                    try
                    {
                        conn = new CmCn();
                        conn.ExcuteQry(strQry);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    Load_Data();
                    MessageBox.Show("Inactive successfully.");
                }
            } 
            
        }

        private void gvResult_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode==Keys.Delete)
            {
                btnDelete.PerformClick();
            }
        }

        private void dgvResult_Click(object sender, EventArgs e)
        {

        }

        private void btnAddNew_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            txtTelNo.Text = "";
            txtShortname.Text = "";
            txtTaxCode.Text = "";
            txtContact.Text = "";
            txtAddress.Text = "";
            txtSupplierName.Text = "";
            txtEmail.Text = "";
            cboCurrency.Text = "";
            txtIncoTerm.Text = "";
            txtDeliveryMode.Text = "";
            txtPaymentTerm.Text = "";
            txtSupplierName.ReadOnly = false;
            isAddNew = true;
        }

    }
}
