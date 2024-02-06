using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraSplashScreen;
using HVN_System.Entity;
using HVN_System.Util;
using HVN_System.View.Admin;

namespace HVN_System.View.PUR
{
    public partial class frmPURMasterListItem : Form
    {
        public frmPURMasterListItem()
        {
            InitializeComponent();
        }
        private ADO adoClass;
        private CmCn conn;
        List<PUR_MasterListItem_Entity> List_Data;
        private PUR_MasterListItem_Entity current_item;
        private void gvResult_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
           
        }

        private void frmWHCCManagement_Load(object sender, EventArgs e)
        {
            
            Load_Data();
            adoClass = new ADO();
        }
        
        private void Load_Data()
        {
            current_item = new PUR_MasterListItem_Entity();
            string strQry = "select *, \n ";
            strQry += " case  \n ";
            strQry += " when expired_date<CAST( GETDATE() AS Date ) then 'Expired' \n ";
            strQry += " else item_status \n ";
            strQry += " end as [i_status] \n ";
            strQry += " from PUR_MasterListItem \n ";
            conn = new CmCn();
            DataTable dt = conn.ExcuteDataTable(strQry);
            List_Data = new List<PUR_MasterListItem_Entity>();
            foreach (DataRow row in dt.Rows)
            {
                PUR_MasterListItem_Entity item = new PUR_MasterListItem_Entity();
                item.Item_name = row["item_name"].ToString();
                item.Erp_code = row["erp_code"].ToString();
                item.Hut_code = row["hut_code"].ToString();
                item.Item_unit = row["item_unit"].ToString();
                item.Unit_price = string.IsNullOrEmpty(row["unit_price"].ToString()) ? 0 : float.Parse(row["unit_price"].ToString());
                item.Unit_currency = row["unit_currency"].ToString();
                item.Unit_vat = string.IsNullOrEmpty(row["unit_vat"].ToString()) ? 0 : float.Parse(row["unit_vat"].ToString());
                item.Supplier_name = row["supplier_name"].ToString();
                item.Item_type = row["item_type"].ToString();
                item.Moq = string.IsNullOrEmpty(row["moq"].ToString()) ? 0 : float.Parse(row["moq"].ToString());
                item.Standard_packing = string.IsNullOrEmpty(row["standard_packing"].ToString()) ? 0 : float.Parse(row["standard_packing"].ToString());
                item.Item_status = row["i_status"].ToString();
                item.Expired_date = string.IsNullOrEmpty(row["expired_date"].ToString()) ? DateTime.Today : DateTime.Parse(row["expired_date"].ToString());
                item.Delivery_cost = string.IsNullOrEmpty(row["delivery_cost"].ToString()) ? 0 : float.Parse(row["delivery_cost"].ToString());
                item.Ddp_cost = string.IsNullOrEmpty(row["ddp_cost"].ToString()) ? 0 : float.Parse(row["ddp_cost"].ToString());
                item.Max_price = string.IsNullOrEmpty(row["max_price"].ToString()) ? 0 : float.Parse(row["max_price"].ToString());
                item.Min_price = string.IsNullOrEmpty(row["min_price"].ToString()) ? 0 : float.Parse(row["min_price"].ToString());
                List_Data.Add(item);
            }
            dgvResult.DataSource = List_Data.ToList();
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

                       
        private void gvResult_KeyDown(object sender, KeyEventArgs e)
        {
            current_item = gvResult.GetRow(gvResult.FocusedRowHandle) as PUR_MasterListItem_Entity;
        }

        private void btnActive_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            current_item = gvResult.GetRow(gvResult.FocusedRowHandle) as PUR_MasterListItem_Entity;
            if (current_item.Item_name != "")
            {
                if (MessageBox.Show("Do you want to activate item: "+ current_item.Item_name + "?", "Activate item", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    SplashScreenManager.ShowForm(this, typeof(frmWaitingForm), true, true, false);
                    SplashScreenManager.Default.SetWaitFormCaption("Please wait...");
                    string strQry = "update PUR_MasterListItem  \n ";
                    strQry += " set item_status=N'Active',expired_date=DATEADD(YEAR,1,expired_date) \n ";
                    strQry += " where item_name=N'' \n ";
                    strQry += "insert into PUR_MasterListItem_History (item_name,i_transaction,i_content,i_note,pic,input_time) \n";
                    strQry += "select N'" + current_item.Item_name + "',N'Activate item manually',N'',N'',N'" + General_Infor.username + "',N'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'";
                    conn = new CmCn();
                    try
                    {
                        conn.ExcuteQry(strQry);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    SplashScreenManager.CloseForm();
                }
            }
        }

        private void btnDisable_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            current_item = gvResult.GetRow(gvResult.FocusedRowHandle) as PUR_MasterListItem_Entity;
            if (current_item.Item_name != "")
            {
                if (MessageBox.Show("Do you want to disable item: " + current_item.Item_name + "?", "Disable item", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    SplashScreenManager.ShowForm(this, typeof(frmWaitingForm), true, true, false);
                    SplashScreenManager.Default.SetWaitFormCaption("Please wait...");
                    string strQry = "update PUR_MasterListItem  \n ";
                    strQry += " set item_status=N'Disable' \n ";
                    strQry += " where item_name=N'' \n ";
                    strQry += "insert into PUR_MasterListItem_History (item_name,i_transaction,i_content,i_note,pic,input_time) \n";
                    strQry += "select N'" + current_item.Item_name + "',N'Disable item manually',N'',N'',N'" + General_Infor.username + "',N'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'";
                    conn = new CmCn();
                    try
                    {
                        conn.ExcuteQry(strQry);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    SplashScreenManager.CloseForm();
                }
            }
        }
    }
}
