using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Data.SqlClient;
using System.IO;
using HVN_System.Entity;
using HVN_System.Util;
using System.Collections.ObjectModel;
using DevExpress.XtraEditors.Repository;
using HVN_System.View.PUR;
using DevExpress.XtraSplashScreen;
using HVN_System.View.Admin;

namespace HVN_System.View.Planning
{
    public partial class frmPURAddNewItem : Form
    {
        public frmPURAddNewItem()
        {
            InitializeComponent();
        }
        private ADO adoClass;
        private List<PUR_VS_Entity> List_Data;
        private PUR_VS_Entity Current_VS;
        private void btnEdit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (Current_VS.Vs_id != "")
            {
                frmPURAddNewItemDetail frm = new frmPURAddNewItemDetail(Current_VS, "View");
                frm.ShowDialog();
                btnRefresh.PerformClick();
            }
        }
        private void btnRefresh_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Load_List_VS();
        }
        private void Load_permission()
        {
            adoClass = new ADO();
            btnNew.Enabled = adoClass.Check_permission(this.Name, btnNew.Name, General_Infor.username);
            gridColumn13.Visible = adoClass.Check_permission(this.Name, btnNew.Name, General_Infor.username);
        }
        private void frmProductionPlanFG_Load(object sender, EventArgs e)
        {
            Load_permission();
            if (General_Infor.username!="admin")
            {
                try
                {
                    string filepath = @"\\172.16.180.20\20.Public\05.IT\03.HVN_SYS\01.Format_Excel\PUR_VS_Detail.xlsx";
                    string new_path = @"C:\HVN_SYS\01.Format_Excel\PUR_VS_Detail.xlsx";
                    File.Copy(filepath, new_path, true);
                }
                catch (Exception)
                {

                }
            }
            Load_List_VS();
        }
        private void Load_List_VS()
        {
            List_Data = new List<PUR_VS_Entity>();
            adoClass = new ADO();
            DataTable dt = adoClass.Load_PUR_VS("", "dept=N'"+General_Infor.myaccount.Department+"'");
            int i = 1;
            foreach (DataRow row in dt.Rows)
            {
                PUR_VS_Entity item = new PUR_VS_Entity();
                item.Vs_id = row["vs_id"].ToString();
                item.Vs_requester = row["vs_requester"].ToString();
                item.Vs_date = string.IsNullOrEmpty(row["vs_date"].ToString()) ? DateTime.Now : DateTime.Parse(row["vs_date"].ToString());
                item.Vs_des = row["vs_des"].ToString();
                item.Dept = row["dept"].ToString();
                item.Item_name = row["item_name"].ToString();
                item.Item_unit = row["item_unit"].ToString();
                item.Unit_price = string.IsNullOrEmpty(row["unit_price"].ToString()) ? 0 : float.Parse(row["unit_price"].ToString());
                item.Unit_currency = row["unit_currency"].ToString();
                item.Unit_vat = string.IsNullOrEmpty(row["unit_vat"].ToString()) ? 0 : float.Parse(row["unit_vat"].ToString());
                item.Supplier_1 = row["supplier_1"].ToString();
                item.Supplier_1_type = row["supplier_1_type"].ToString();
                item.Supplier_1_att = row["supplier_1_att"].ToString();
                item.Supplier_2 = row["supplier_2"].ToString();
                item.Supplier_2_type = row["supplier_2_type"].ToString();
                item.Supplier_2_att = row["supplier_2_att"].ToString();
                item.Supplier_3 = row["supplier_3"].ToString();
                item.Supplier_3_type = row["supplier_3_type"].ToString();
                item.Supplier_3_att = row["supplier_3_att"].ToString();
                item.Selected_supplier = row["selected_supplier"].ToString();
                item.Dept_mgr = row["dept_mgr"].ToString();
                item.Dept_mgr_date = string.IsNullOrEmpty(row["dept_mgr_date"].ToString()) ? DateTime.Now : DateTime.Parse(row["dept_mgr_date"].ToString());
                item.Dept_sign = row["dept_sign"].ToString();
                item.Pur = row["pur"].ToString();
                item.Pur_date = string.IsNullOrEmpty(row["pur_date"].ToString()) ? DateTime.Now : DateTime.Parse(row["pur_date"].ToString()); row["pur_date"].ToString();
                item.Fin_mgr = row["fin_mgr"].ToString();
                item.Fin_mgr_date = string.IsNullOrEmpty(row["fin_mgr_date"].ToString()) ? DateTime.Now : DateTime.Parse(row["fin_mgr_date"].ToString());
                item.Fin_mgr_sign = row["fin_mgr_sign"].ToString();
                item.Pur_mgr = row["pur_mgr"].ToString();
                item.Pur_mgr_date = string.IsNullOrEmpty(row["pur_mgr_date"].ToString()) ? DateTime.Now : DateTime.Parse(row["pur_mgr_date"].ToString());
                item.Pur_mgr_sign = row["pur_mgr_sign"].ToString();
                item.Plant_mgr = row["plant_mgr"].ToString();
                item.Plant_mgr_date = string.IsNullOrEmpty(row["plant_mgr_date"].ToString()) ? DateTime.Now : DateTime.Parse(row["plant_mgr_date"].ToString());
                item.Plant_mgr_sign = row["plant_mgr_sign"].ToString();
                item.Vs_status = row["vs_status"].ToString();
                item.Vs_comment = row["vs_comment"].ToString();
                item.Estimate_yearly_amount = string.IsNullOrEmpty(row["estimate_yearly_amount"].ToString()) ? 0 : decimal.Parse(row["estimate_yearly_amount"].ToString());
                item.IsActive = row["isActive"].ToString();
                item.Current_pic = row["current_pic"].ToString();
                item.Dept_comment = row["dept_comment"].ToString();
                item.Pur_comment = row["pur_comment"].ToString();
                item.Fin_mgr_comment = row["fin_mgr_comment"].ToString();
                item.Pur_mgr_comment = row["pur_mgr_comment"].ToString();
                item.Plant_mgr_comment = row["plant_mgr_comment"].ToString();
                List_Data.Add(item);
                i++;
            }
            dgvResult.DataSource = List_Data.ToList();
        }
        private void btnNew_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            PUR_VS_Entity item = new PUR_VS_Entity();
            frmPURAddNewItemDetail frm = new frmPURAddNewItemDetail(item,"New");
            frm.ShowDialog();
            btnRefresh.PerformClick();
        }

        private void gvResult_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            Current_VS = gvResult.GetRow(gvResult.FocusedRowHandle) as PUR_VS_Entity;
        }

        private void btnViewPR_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            Current_VS = gvResult.GetRow(gvResult.FocusedRowHandle) as PUR_VS_Entity;
            frmPURAddNewItemDetail frm = new frmPURAddNewItemDetail(Current_VS, "View VS");
            frm.ShowDialog();
            btnRefresh.PerformClick();
        }

        private void btnPrintVS_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            Current_VS = gvResult.GetRow(gvResult.FocusedRowHandle) as PUR_VS_Entity;
            if (Current_VS.Vs_id != "")
            {
                SplashScreenManager.ShowForm(this, typeof(frmWaitingForm), true, true, false);
                SplashScreenManager.Default.SetWaitFormCaption("Please wait...");
                string filepath = @"\\172.16.180.20\05.it\16.HVN\ATTACHMENT\PURCHASE\VS\" + Current_VS.Vs_id + ".pdf";
                adoClass = new ADO();
                adoClass.Print_PUR_VS_Detail(Current_VS, filepath, "export");
                string new_path = @"C:\HVN_SYS_CONFIG\" + Current_VS.Vs_id + ".pdf";
                File.Copy(filepath, new_path, true);
                System.Diagnostics.Process.Start(new_path);
                SplashScreenManager.CloseForm();
            }
        }

        private void btnCopy_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            Current_VS = gvResult.GetRow(gvResult.FocusedRowHandle) as PUR_VS_Entity;
            if (!string.IsNullOrEmpty(Current_VS.Vs_id))
            {
                DateTime check_date = new DateTime(2023, 12, 26);
                if (Current_VS.Vs_date>=check_date)
                {
                    frmPURAddNewItemDetail frm = new frmPURAddNewItemDetail(Current_VS, "Copy");
                    frm.ShowDialog();
                    btnRefresh.PerformClick();
                }
                else
                {
                    MessageBox.Show("This is old version of vendor selection. Please add new request then you can copy");
                }
            }
        }

        private void btnViewPdf_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            Current_VS = gvResult.GetRow(gvResult.FocusedRowHandle) as PUR_VS_Entity;
            string new_path = @"C:\HVN_SYS_CONFIG\" + Current_VS.Vs_id + ".pdf";
            string filepath = @"\\172.16.180.20\05.it\16.HVN\ATTACHMENT\PURCHASE\VS\" + Current_VS.Vs_id + ".pdf";
            try
            {
                File.Copy(filepath, new_path, true);
                System.Diagnostics.Process.Start(filepath);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void gvResult_CustomRowCellEdit(object sender, DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventArgs e)
        {
            if (e.Column.Caption == "Edit")
            {
                string val = gvResult.GetRowCellValue(e.RowHandle, "Vs_status").ToString();
                if (val != "Pending requester" && val != "Draft")
                {
                    RepositoryItemButtonEdit ritem = new RepositoryItemButtonEdit();
                    ritem.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
                    ritem.ReadOnly = true;
                    ritem.Buttons[0].Enabled = false;
                    e.RepositoryItem = ritem;
                }
            }
        }

        private void btnEdit_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            Current_VS = gvResult.GetRow(gvResult.FocusedRowHandle) as PUR_VS_Entity;
            if (!string.IsNullOrEmpty(Current_VS.Vs_id))
            {
                frmPURAddNewItemDetail frm = new frmPURAddNewItemDetail(Current_VS, "Edit");
                frm.ShowDialog();
                btnRefresh.PerformClick();
            }
        }
    }
}
