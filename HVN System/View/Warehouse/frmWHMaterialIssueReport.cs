using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HVN_System.Entity;
using HVN_System.Util;
using Outlook = Microsoft.Office.Interop.Outlook;

namespace HVN_System.View.Warehouse
{
    public partial class frmWHMaterialIssueReport : Form
    {
        public frmWHMaterialIssueReport()
        {
            InitializeComponent();
        }
        private ADO adoClass;
        private CmCn conn;
        private List<W_M_IssueLabel_Entity> List_Item;
        private W_M_IssueLabel_Entity Current_Item;
        private void Load_Data()
        {
            adoClass = new ADO();
            DataTable dt = adoClass.Load_W_M_IssueLabel("", "supply_date>=N'"+dtpFromDate.Value.ToString("yyyy-MM-dd")+"' and supply_date<=N'"+ dtpToDate.Value.ToString("yyyy-MM-dd") + "'");
            adoClass = new ADO();
            List_Item = new List<W_M_IssueLabel_Entity>();
            foreach (DataRow row in dt.Rows)
            {
                W_M_IssueLabel_Entity item = new W_M_IssueLabel_Entity();
                item.M_name = row["m_name"].ToString();
                item.Lot_no = string.IsNullOrEmpty(row["lot_no"].ToString()) ? DateTime.Today : DateTime.Parse(row["lot_no"].ToString());
                item.Whmr_code = row["whmr_code"].ToString();
                item.Quantity = float.Parse(row["quantity"].ToString());
                item.Whmi_code = row["whmi_code"].ToString();
                item.Product_customer_code= row["product_customer_code"].ToString();
                item.P_line= row["p_line"].ToString();
                item.P_shift = row["p_shift"].ToString();
                item.Supply_date =DateTime.Parse(row["supply_date"].ToString());
                List_Item.Add(item);
            }
            dgvResult.DataSource = List_Item.ToList();
        }

        private void frmKPIMyAction_Load(object sender, EventArgs e)
        {
            dtpFromDate.Value = DateTime.Today.AddDays(-1);
            Load_Data();
            adoClass = new ADO();
            btnDelete.Enabled = adoClass.Check_permission(this.Name, btnDelete.Name, General_Infor.username);
        }
        private void btnRefresh_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Load_Data();
        }

        private void gvIncident_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            Current_Item = gvResult.GetRow(gvResult.FocusedRowHandle) as W_M_IssueLabel_Entity;
        }

        private void btnSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (MessageBox.Show("Do you want to change information?", "Save change", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {

            }
        }

        private void btnExport_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            adoClass = new ADO();
            adoClass.Export_Excel(dgvResult);
        }

        private void gvResult_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {

        }

        private void btnCheck_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }

        private void repositoryItemComboBox1_EditValueChanged(object sender, EventArgs e)
        {
            if (gvResult.PostEditor())
            {
                gvResult.UpdateCurrentRow();
            }
        }

        private void btnDelete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (MessageBox.Show("Do you want to delete this box?", "Save change", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Current_Item = gvResult.GetRow(gvResult.FocusedRowHandle) as W_M_IssueLabel_Entity;
                string strQry = "update W_M_ReceiveLabel set quantity=quantity+N'"+Current_Item.Quantity+"' \n";
                strQry += " where whmr_code=N'" + Current_Item.Whmr_code + "' \n";
                strQry += " update W_M_IssueLabel set quantity=N'0' where whmr_code=N'" + Current_Item.Whmr_code + "' \n";
                strQry += "insert into W_M_HistoryOfTransaction([whmr_code],[m_name],[quantity],[lot_no],[transaction],[location],[input_time],[PIC],[invoice_no],[place]) \n";
                strQry += "select N'" + Current_Item.Whmr_code + "',N'" + Current_Item.M_name + "',N'" + Current_Item.Quantity + "',N'" + Current_Item.Lot_no + "'";
                strQry += ",N'Rervert qty from issue label manually',N'',getdate(),N'" + General_Infor.username + "',N'',N'' \n";
                try
                {
                    conn = new CmCn();
                    conn.ExcuteQry(strQry);
                    btnRefresh.PerformClick();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}
