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

namespace HVN_System.View.Warehouse
{
    public partial class frmWHMasterDataMaterial : Form
    {
        public frmWHMasterDataMaterial()
        {
            InitializeComponent();
        }
        private ADO adoClass;
        private CmCn conn;
        List<W_MasterList_Material_Entity> List_Data;
        private W_MasterList_Material_Entity current_item;
        private bool isAddNew = true;
        private void gvResult_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            current_item = gvResult.GetRow(gvResult.FocusedRowHandle) as W_MasterList_Material_Entity;
            txtPN.Text = current_item.M_name;
            txtRefQty.Text= current_item.Raw_qty.ToString();
            txtRefWeight.Text = current_item.Raw_weight.ToString();
            nmStdQty.Value = int.Parse(current_item.M_qty.ToString());
            cboScaleType.Text = current_item.Scale_type;
            txtDes.Text = current_item.M_des;
            txtSupplier.Text = current_item.Supplier;
            nmExpiryDay.Value = current_item.Expiry_day;
            cboMKind.Text = current_item.M_kind;
            txtPalletWeight.Text = current_item.Pallet_weight.ToString();
            txtPN.Enabled = false;
            isAddNew = false;
        }
        private void Load_permission()
        {
            adoClass = new ADO();
            btnNew.Enabled = adoClass.Check_permission(this.Name, btnNew.Name, General_Infor.username);
            btnSave.Enabled = adoClass.Check_permission(this.Name, btnNew.Name, General_Infor.username);
            btnDelete.Enabled = adoClass.Check_permission(this.Name, btnNew.Name, General_Infor.username);
        }
        private void frmWHCCManagement_Load(object sender, EventArgs e)
        {
            txtRefQty.Text = "0";
            txtRefWeight.Text = "0";
            txtPalletWeight.Text = "0";
            Load_permission();
            Load_Data();
            adoClass = new ADO();
            btnSave.Enabled = adoClass.Check_permission(this.Name, btnSave.Name, General_Infor.username);
            btnDelete.Enabled = adoClass.Check_permission(this.Name, btnSave.Name, General_Infor.username);
        }
        private void Load_Data()
        {
            current_item = new W_MasterList_Material_Entity();
            //gvResult.OptionsBehavior.Editable = true;
            adoClass = new ADO();
            DataTable dt = adoClass.Load_W_MasterList_Material("", "");
            List_Data = new List<W_MasterList_Material_Entity>();
            foreach (DataRow row in dt.Rows)
            {
                W_MasterList_Material_Entity item = new W_MasterList_Material_Entity();
                item.M_name = row["m_name"].ToString();
                item.M_des = row["m_des"].ToString();
                item.M_qty = string.IsNullOrEmpty(row["m_qty"].ToString()) ? 0 : float.Parse(row["m_qty"].ToString());
                item.Raw_qty = string.IsNullOrEmpty(row["raw_qty"].ToString()) ? 0 : float.Parse(row["raw_qty"].ToString());
                item.Raw_weight = string.IsNullOrEmpty(row["raw_weight"].ToString()) ? 0 : float.Parse(row["raw_weight"].ToString());
                item.Pallet_weight = string.IsNullOrEmpty(row["pallet_weight"].ToString()) ? 0 : float.Parse(row["pallet_weight"].ToString());
                item.Expiry_day = string.IsNullOrEmpty(row["expiry_day"].ToString()) ? 0 : int.Parse(row["expiry_day"].ToString());
                item.Scale_type = row["scale_type"].ToString();
                item.M_kind = row["m_kind"].ToString();
                item.Supplier = row["supplier"].ToString();
                List_Data.Add(item);
            }
            dgvResult.DataSource = List_Data.ToList();
        }
        private void gvResult_DoubleClick(object sender, EventArgs e)
        {

        }

        private void btnSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (MessageBox.Show("Do you want to save data for : " + txtPN.Text + " ?", "Save Data", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                string strQry = "";
                if (isAddNew)
                {
                    strQry += "insert into W_MasterList_Material ([m_name],[is_active],[m_des],[raw_qty],[raw_weight],[m_qty],[scale_type],m_kind,expiry_day,supplier,pallet_weight) \n ";
                    strQry += "select N'" + txtPN.Text + "',N'1',N'" + txtDes.Text + "',N'" + txtRefQty.Text + "',N'" + txtRefWeight.Text 
                        + "',N'" + nmStdQty.Text + "',N'" + cboScaleType.Text + "',N'" + cboMKind.Text + "',N'" + nmExpiryDay.Text + "',N'" + txtSupplier.Text + "',N'" + txtPalletWeight.Text + "'";
                }
                else
                {
                    strQry += "update W_MasterList_Material set raw_qty=N'" + txtRefQty.Text + "',raw_weight=N'" + txtRefWeight.Text + "',m_qty=N'"
                        + nmStdQty.Text + "',scale_type=N'" + cboScaleType.Text + "',m_des=N'" + txtDes.Text
                        + "',m_kind=N'" + cboMKind.Text + "',expiry_day=N'" + nmExpiryDay.Text + "',supplier=N'" + txtSupplier.Text + "',pallet_weight=N'"+txtPalletWeight.Text+"'  \n ";
                    strQry += "where m_name=N'" + txtPN.Text + "'";
                }
                conn = new CmCn();
                try
                {
                    conn.ExcuteQry(strQry);
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

        private void btnImportNew_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                OpenFileDialog OpenFile = new OpenFileDialog();
                OpenFile.Title = "Mở tệp tin";
                OpenFile.Filter = "Excel (.xlsx)|*.xlsx";
                if (OpenFile.ShowDialog() != DialogResult.Cancel)
                {
                    string FilePath = OpenFile.FileName;
                    adoClass = new ADO();
                    DataTable dt = adoClass.ReadExcelFile("Sheet1", FilePath);
                    string List_Error = "";
                    string listItem = "";
                    foreach (DataRow row in dt.Rows)
                    {
                        string PN = row["Material"].ToString();
                        string Ref_Qty = row["Reference Qty"].ToString();
                        string Ref_Weight = row["Reference Weight"].ToString();
                        string Standard_Qty = row["Standard Qty"].ToString();
                        string Scale_type = row["Type Scale"].ToString();
                        if (PN != "")
                        {
                            string strQry = "select m_name from W_MasterList_Material where m_name=N'" + PN + "'";
                            conn = new CmCn();
                            if (string.IsNullOrEmpty(conn.ExcuteString(strQry)))
                            {
                                
                                if (listItem == "")
                                {
                                    listItem += "select N'" + PN + "',N'1',N'" + Ref_Qty + "',N'" + Ref_Weight + "',N'" + Standard_Qty + "',N'" + Scale_type + "'\n";
                                }
                                else
                                {
                                    listItem += "union all select N'" + PN + "',N'1',N'" + Ref_Qty + "',N'" + Ref_Weight + "',N'" + Standard_Qty + "',N'" + Scale_type + "'\n";
                                }
                            }
                            else
                            {
                                List_Error += PN + "\n";
                            }
                        }
                    }
                    if (List_Error != "")
                    {
                        MessageBox.Show("Loi khong import duoc do cac item duoi day da co trong danh sach:\n" + List_Error);
                    }
                    else
                    {
                        conn = new CmCn();
                        string Qry2 = "insert into W_MasterList_Material (m_name,is_active,raw_qty,raw_weight,m_qty,scale_type) \n";
                        Qry2 += listItem;
                        conn.ExcuteQry(Qry2);
                        MessageBox.Show("Import successfully");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnDelete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (!string.IsNullOrEmpty(current_item.M_name))
            {
                if (MessageBox.Show("Do you want to delete material: "+ current_item.M_name + " ?", "Delete Item", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    string strQry= "delete from W_MasterList_Material where m_name=N'"+current_item.M_name+"'\n";
                    strQry += "delete from P_MasterListProduct_BOM where m_name=N'" + current_item.M_name + "'\n";
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
                    MessageBox.Show("Delete successfully.");
                }
            } 
            
        }

        private void btnImportEdit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            OpenFileDialog OpenFile = new OpenFileDialog();
            OpenFile.Title = "Mở tệp tin";
            OpenFile.Filter = "Excel (.xlsx)|*.xlsx";
            if (OpenFile.ShowDialog() != DialogResult.Cancel)
            {
                string FilePath = OpenFile.FileName;
                adoClass = new ADO();
                DataTable dt = adoClass.ReadExcelFile("Sheet1", FilePath);
                string strQry = "";
                foreach (DataRow row in dt.Rows)
                {
                    string PN = row["Material"].ToString();
                    string Ref_Qty = row["Reference Qty"].ToString();
                    string Ref_Weight = row["Reference Weight"].ToString();
                    string Standard_Qty = row["Standard Qty"].ToString();
                    string Scale_type = row["Type Scale"].ToString();
                    strQry += "update W_MasterList_Material set raw_qty=N'" + Ref_Qty + "',scale_type=N'" + Scale_type + "',m_qty=N'" + Standard_Qty + "',raw_weight=N'" + Ref_Weight + "'\n";
                    strQry += "where m_name=N'"+PN+"'\n";
                }
                conn = new CmCn();
                conn.ExcuteQry(strQry);
                MessageBox.Show("Import successfully");
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
            txtPN.Text = "";
            txtDes.Text = "";
            txtRefQty.Text = "0";
            txtRefWeight.Text = "0";
            txtPalletWeight.Text = "0";
            cboScaleType.Text = "";
            nmStdQty.Value = 0;
            txtPN.Enabled = true;
            isAddNew = true;
        }

    }
}
