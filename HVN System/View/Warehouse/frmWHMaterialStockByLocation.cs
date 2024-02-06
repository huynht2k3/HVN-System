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
    public partial class frmWHMaterialStockByLocation : Form
    {
        public frmWHMaterialStockByLocation()
        {
            InitializeComponent();
        }
        private ADO adoClass;
        private CmCn conn;
        private List<W_M_ReceiveLabel_Entity> List_Item;
        private W_M_ReceiveLabel_Entity Current_Item;
        private void Load_Data()
        {
            adoClass = new ADO();
            DataTable dt = adoClass.Load_W_M_ReceiveLabel("*,datepart(wk,created_time) as ww", "place not in ('') and quantity>0");
            adoClass = new ADO();
            List_Item = new List<W_M_ReceiveLabel_Entity>();
            foreach (DataRow row in dt.Rows)
            {
                W_M_ReceiveLabel_Entity item = new W_M_ReceiveLabel_Entity();
                item.M_name = row["m_name"].ToString();
                item.Lot_no = string.IsNullOrEmpty(row["lot_no"].ToString()) ? DateTime.Today : DateTime.Parse(row["lot_no"].ToString());
                item.Lot_no_string = string.IsNullOrEmpty(row["lot_no"].ToString()) ? "" : item.Lot_no.ToString("yyyy-MM-dd");
                item.Whmr_code = row["whmr_code"].ToString();
                item.Quantity = float.Parse(row["quantity"].ToString());
                item.Wh_okng = row["wh_okng"].ToString();
                //item.QC = row["m_name"].ToString();
                item.Qc_okng = row["qc_okng"].ToString();
                item.Place = row["place"].ToString();
                item.Wh_location = row["wh_location"].ToString();
                item.Rm_doc_id = row["rm_doc_id"].ToString();
                item.Ww = row["ww"].ToString();
                item.Whmr_code_origin = row["whmr_code_origin"].ToString();
                item.IsEdit = false;
                List_Item.Add(item);
            }
            dgvResult.DataSource = List_Item.ToList();
        }

        private void frmKPIMyAction_Load(object sender, EventArgs e)
        {
            Load_Data();
            adoClass = new ADO();
            btnSave.Enabled = adoClass.Check_permission(this.Name, btnSave.Name, General_Infor.username);
        }
        private void btnRefresh_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Load_Data();
        }

        private void gvIncident_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            Current_Item = gvResult.GetRow(gvResult.FocusedRowHandle) as W_M_ReceiveLabel_Entity;
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

        private void btnSave_ItemClick_1(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (MessageBox.Show("Do you want to change information?", "Save change", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                string strQry = "";
                string list = "";
                foreach (W_M_ReceiveLabel_Entity item in List_Item)
                {
                    if (item.IsEdit == true)
                    {
                        string strQry5 = "select whmr_code from W_M_ReceiveLabel where place not in ('') and whmr_code=N'" + item.Whmr_code + "'";
                        conn = new CmCn();
                        if (conn.ExcuteString(strQry5) != "")
                        {
                            strQry += "update W_M_ReceiveLabel set wh_location=N'" + item.Wh_location + "',quantity=N'" + item.Quantity + "',place=N'" + item.Place + "'\n";
                            if (item.Lot_no_string!="")
                            {
                                strQry += ",lot_no=N'"+item.Lot_no_string+"' \n";
                            }
                            else
                            {
                                strQry += ",lot_no=null \n";
                            }
                            strQry += " where whmr_code=N'" + item.Whmr_code + "'\n";

                            if (string.IsNullOrEmpty(list))
                            {
                                list += "select N'" + item.Whmr_code + "',N'" + item.M_name + "',N'" + item.Quantity + "',N'" + item.Lot_no + "'";
                                list += ",N'Edit infomation of box manually',N'" + item.Wh_location + "',getdate(),N'" + General_Infor.username + "',N'" + item.Rm_doc_id + "',N'" + item.Place + "' \n";
                            }
                            else
                            {
                                list += "union all select N'" + item.Whmr_code + "',N'" + item.M_name + "',N'" + item.Quantity + "',N'" + item.Lot_no + "'";
                                list += ",N'Edit infomation of box manually',N'" + item.Wh_location + "',getdate(),N'" + General_Infor.username + "',N'" + item.Rm_doc_id + "',N'" + item.Place + "' \n";
                            }
                        }
                    }
                }
                if (!string.IsNullOrEmpty(list))
                {
                    strQry += "insert into W_M_HistoryOfTransaction([whmr_code],[m_name],[quantity],[lot_no],[transaction],[location],[input_time],[PIC],[invoice_no],[place]) \n";
                    strQry += list;
                }
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
        private void repositoryItemCheckEdit1_EditValueChanged(object sender, EventArgs e)
        {
            if (gvResult.PostEditor())
            {
                gvResult.UpdateCurrentRow();
            }
        }

        private void gvResult_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            W_M_ReceiveLabel_Entity item_changed = gvResult.GetRow(gvResult.FocusedRowHandle) as W_M_ReceiveLabel_Entity;
            item_changed.IsEdit = true;
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
                Current_Item = gvResult.GetRow(gvResult.FocusedRowHandle) as W_M_ReceiveLabel_Entity;
                string strQry = "update W_M_ReceiveLabel set place=null,wh_okng=null,qc_okng=null,wh_location=null \n";
                strQry += " where whmr_code=N'" + Current_Item.Whmr_code + "' \n";
                strQry += "insert into W_M_HistoryOfTransaction([whmr_code],[m_name],[quantity],[lot_no],[transaction],[location],[input_time],[PIC],[invoice_no],[place]) \n";
                strQry += "select N'" + Current_Item.Whmr_code + "',N'" + Current_Item.M_name + "',N'" + Current_Item.Quantity + "',N'" + Current_Item.Lot_no + "'";
                strQry += ",N'Remove the box manually',N'" + Current_Item.Wh_location + "',getdate(),N'" + General_Infor.username + "',N'" + Current_Item.Rm_doc_id + "',N'" + Current_Item.Place + "' \n";
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
