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
    public partial class frmWHRubberStockByLocation : Form
    {
        public frmWHRubberStockByLocation()
        {
            InitializeComponent();
        }
        private ADO adoClass;
        private CmCn conn;
        private List<W_M_RubberLabel_Entity> List_Item;
        private W_M_RubberLabel_Entity Current_Item;
        private void Load_Data()
        {
            string strQry = "select a.*,b.expiry_day as expiry_date_no \n ";
            strQry += " from W_M_RubberLabel a,W_MasterList_Material b \n ";
            strQry += " where a.r_name=b.m_name \n ";
            strQry += " and a.place=N'WH Rubber' \n ";
            conn = new CmCn();
            DataTable dt = conn.ExcuteDataTable(strQry);
            List_Item = new List<W_M_RubberLabel_Entity>();
            foreach (DataRow row in dt.Rows)
            {
                W_M_RubberLabel_Entity item = new W_M_RubberLabel_Entity();
                item.Whrr_code= row["whrr_code"].ToString();
                item.R_name = row["r_name"].ToString();
                item.Weight = float.Parse (row["weight"].ToString());
                item.Lot_no =DateTime.Parse(row["lot_no"].ToString());
                item.Expired_date = DateTime.Parse(row["expired_date"].ToString());
                item.Expiry_date_no = int.Parse(row["expiry_date_no"].ToString());
                item.IsEdit = false;
                List_Item.Add(item);
            }
            dgvResult.DataSource = List_Item.ToList();
        }

        private void frmKPIMyAction_Load(object sender, EventArgs e)
        {
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
            Current_Item = gvResult.GetRow(gvResult.FocusedRowHandle) as W_M_RubberLabel_Entity;
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
            Current_Item = gvResult.GetRow(gvResult.FocusedRowHandle) as W_M_RubberLabel_Entity;
            Current_Item.Expired_date = Current_Item.Lot_no.AddDays(Current_Item.Expiry_date_no);
            Current_Item.IsEdit = true;
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
            if (MessageBox.Show("Do you want to delete this pallet?", "Save change", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Current_Item = gvResult.GetRow(gvResult.FocusedRowHandle) as W_M_RubberLabel_Entity;
                string strQry = " update  W_M_RubberLabel  set  place =N'' where whrr_code=N'" + Current_Item.Whrr_code + "' \n";
                strQry += "insert into W_M_RubberTransaction([whrr_code],[r_name],[weight],[lot_no],[transaction],[input_time],[PIC]) \n";
                strQry += "select N'" + Current_Item.Whrr_code + "',N'" + Current_Item.R_name + "',N'" + Current_Item.Weight + "',N'" + Current_Item.Lot_no + "'";
                strQry += ",N'Delete the pallet manually',getdate(),N'" + General_Infor.username + "' \n";
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
