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
using DevExpress.XtraGrid.Views.Base;
using HVN_System.Entity;
using HVN_System.Util;
using Outlook = Microsoft.Office.Interop.Outlook;

namespace HVN_System.View.Warehouse
{
    public partial class frmWHStockByLocation : Form
    {
        public frmWHStockByLocation()
        {
            InitializeComponent();
        }
        private ADO adoClass;
        private CmCn conn;
        private List<P_Label_Entity> List_Item;
        private P_Label_Entity Current_Item;
        private bool isShipped;
        private void Load_Data()
        {
            if (isShipped)
            {
                adoClass = new ADO();
                DataTable dt = adoClass.Load_Label_FG_Data("product_code,label_code,product_customer_code,lot_no,product_quantity,place,isLock,plan_date,shift", "place ='Shipped'");
                List_Item = new List<P_Label_Entity>();
                foreach (DataRow row in dt.Rows)
                {
                    P_Label_Entity item = new P_Label_Entity();
                    item.Product_code = row["product_code"].ToString();
                    item.Label_code = row["label_code"].ToString();
                    item.Product_customer_code = row["product_customer_code"].ToString();
                    item.Lot_no = row["lot_no"].ToString();
                    item.Plan_date = DateTime.Parse(row["plan_date"].ToString());
                    item.Shift = row["shift"].ToString();
                    item.Product_quantity = int.Parse(row["product_quantity"].ToString());
                    item.Place = row["place"].ToString();
                    item.IsLock = row["isLock"].ToString();
                    List_Item.Add(item);
                }
            }
            else
            {
                adoClass = new ADO();
                string condition = "place not in ('','Shipped')";
                DataTable dt = adoClass.Load_Label_FG_Data("product_code,label_code,pallet_no,product_customer_code,lot_no,product_quantity,wh_location,place,isLock,plan_date,comment,shift", condition);
                List_Item = new List<P_Label_Entity>();
                foreach (DataRow row in dt.Rows)
                {
                    P_Label_Entity item = new P_Label_Entity();
                    item.Product_code = row["product_code"].ToString();
                    item.Label_code = row["label_code"].ToString();
                    item.Pallet_no = row["pallet_no"].ToString();
                    item.Shift = row["shift"].ToString();
                    item.Product_customer_code = row["product_customer_code"].ToString();
                    item.Lot_no = row["lot_no"].ToString();
                    item.Plan_date = DateTime.Parse(row["plan_date"].ToString());
                    item.Product_quantity = int.Parse(row["product_quantity"].ToString());
                    item.Place = row["place"].ToString();
                    if (item.Place == "FG Zone")
                    {
                        item.Wh_location = row["wh_location"].ToString();
                    }
                    item.Comment = row["comment"].ToString();
                    item.IsLock = row["isLock"].ToString();
                    item.Note = "Edit information manually";
                    List_Item.Add(item);
                }
            }
            dgvResult.DataSource = List_Item.ToList();
        }

        private void frmKPIMyAction_Load(object sender, EventArgs e)
        {
            isShipped = false;
            Load_Data();
            adoClass = new ADO();
            btnSave.Enabled = adoClass.Check_permission(this.Name, btnSave.Name, General_Infor.username);
            btnBlockAllFiltered.Enabled = adoClass.Check_permission(this.Name, btnBlockAllFiltered.Name, General_Infor.username);
            btnUnblockAllFiltered.Enabled= adoClass.Check_permission(this.Name, btnUnblockAllFiltered.Name, General_Infor.username);
        }
        private void btnRefresh_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Load_Data();
        }

        private void gvIncident_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            Current_Item = gvResult.GetRow(gvResult.FocusedRowHandle) as P_Label_Entity;
        }

        private void btnSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (MessageBox.Show("Do you want to change information?", "Save change", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {

            }
        }

        private void btnView_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (isShipped)
            {
                btnView.Caption = "View Shipped";
                isShipped = false;
                Load_Data();
            }
            else
            {
                btnView.Caption = "View Stock";
                isShipped = true;
                Load_Data();
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
                foreach (P_Label_Entity item in List_Item)
                {
                    if (item.IsEdit == true)
                    {
                        string strQry5 = "select label_code from P_Label where place<>N'Shipped' and label_code=N'" + item.Label_code + "'";
                        conn = new CmCn();
                        if (conn.ExcuteString(strQry5) != "")
                        {
                            if (string.IsNullOrEmpty(item.Pallet_no))
                            {
                                strQry += "update P_Label set wh_location=N'" + item.Wh_location + "',pallet_no=N'" + item.Pallet_no + "',place=N'" + item.Place + "',isLock=N'" + item.IsLock + "',comment=N'" + item.Comment + "'\n";
                                strQry += " where label_code=N'" + item.Label_code + "'\n";
                            }
                            else
                            {
                                strQry += "update P_Label set location_packed=N'" + item.Wh_location + "',pallet_no=N'" + item.Pallet_no + "',place=N'" + item.Place + "',isLock=N'" + item.IsLock + "',comment=N'" + item.Comment + "'\n";
                                strQry += " where label_code=N'" + item.Label_code + "'\n";
                            }
                            if (string.IsNullOrEmpty(list))
                            {
                                list += "select N'" + item.Label_code + "',N'" + item.Product_code + "',N'" + item.Product_customer_code + "',N'" + item.Product_quantity + "',N'" + item.Lot_no + "'";
                                list += ",N'" + item.Note + "',N'" + item.Wh_location + "',N'" + item.Pallet_no + "',getdate(),N'" + General_Infor.username + "',N'" + item.Invoice_no + "' \n";
                            }
                            else
                            {
                                list += "union all select N'" + item.Label_code + "',N'" + item.Product_code + "',N'" + item.Product_customer_code + "',N'" + item.Product_quantity + "',N'" + item.Lot_no + "'";
                                list += ",N'" + item.Note + "',N'" + item.Wh_location + "',N'" + item.Pallet_no + "',getdate(),N'" + General_Infor.username + "',N'" + item.Invoice_no + "' \n";
                            }
                        }
                    }
                }
                if (!string.IsNullOrEmpty(list))
                {
                    strQry += "insert into W_HistoryOfTransaction(label_code,product_code,product_customer_code,product_quantity,lot_no,[transaction],[location],pallet_no,input_time,PIC,invoice_no) \n";
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

        private void gvResult_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            P_Label_Entity item_changed = gvResult.GetRow(gvResult.FocusedRowHandle) as P_Label_Entity;
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
                Current_Item = gvResult.GetRow(gvResult.FocusedRowHandle) as P_Label_Entity;
                string strQry = "update P_Label set date_input_wh=null,op_input_wh=null,wh_locate_date=null,wh_location=null, \n";
                strQry += " wh_op_locate=null,date_input_packing_zone=null,op_input_packing_zone=null,date_packed=null \n";
                strQry += " , op_packed = null, pallet_no = null, date_locate_packed = null, location_packed = null, op_locate_packed = null, place = null \n";
                strQry += " where label_code=N'" + Current_Item.Label_code + "' \n";
                strQry += "insert into W_HistoryOfTransaction(label_code,product_code,product_customer_code,product_quantity,lot_no,[transaction],[location],pallet_no,input_time,PIC,invoice_no) \n";
                strQry += "select N'" + Current_Item.Label_code + "',N'" + Current_Item.Product_code + "',N'" + Current_Item.Product_customer_code + "',N'" + Current_Item.Product_quantity + "',N'" + Current_Item.Lot_no + "'";
                strQry += ",N'Remove the box manually',N'" + Current_Item.Wh_location + "',N'" + Current_Item.Pallet_no + "',getdate(),N'" + General_Infor.username + "',N'" + Current_Item.Invoice_no + "' \n";
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

        private void btnSelectAllFiltered_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (MessageBox.Show("Do you want to BLOCK all filtered item?", "BLOCK ITEM", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                List<P_Label_Entity> list_selected = GetDataRows(gvResult);
                string strQry = "";
                string qry2 = "";
                if (list_selected!=null)
                {
                    foreach (P_Label_Entity item in list_selected)
                    {
                        strQry += "update P_label set isLock=N'Block' where label_code=N'" + item.Label_code + "' \n";
                        if (qry2=="")
                        {
                            qry2 += "select N'" + item.Label_code + "',N'" + item.Product_code + "',N'" + item.Product_customer_code + "',N'" + item.Product_quantity + "',N'" + item.Lot_no + "'";
                            qry2 += ",N'Block carton',N'" + item.Wh_location + "',N'" + item.Pallet_no + "',getdate(),N'" + General_Infor.username + "',N'" + item.Invoice_no + "' \n";
                        }
                        else
                        {
                            qry2 += "union all select N'" + item.Label_code + "',N'" + item.Product_code + "',N'" + item.Product_customer_code + "',N'" + item.Product_quantity + "',N'" + item.Lot_no + "'";
                            qry2 += ",N'Block carton',N'" + item.Wh_location + "',N'" + item.Pallet_no + "',getdate(),N'" + General_Infor.username + "',N'" + item.Invoice_no + "' \n";
                        }
                    }
                }
                if (strQry!="")
                {
                    strQry += "insert into W_HistoryOfTransaction(label_code,product_code,product_customer_code,product_quantity,lot_no,[transaction],[location],pallet_no,input_time,PIC,invoice_no) \n";
                    strQry += qry2;
                }
                conn = new CmCn();
                conn.ExcuteQry(strQry);
                Load_Data();
            }
        }
        List<P_Label_Entity> GetDataRows(ColumnView view)
        {
            if (view == null) return null;
            List<P_Label_Entity> rowList = new List<P_Label_Entity>();
            for (int i = 0; i < view.DataRowCount; i++)
                rowList.Add(gvResult.GetRow(i) as P_Label_Entity);

            return rowList;
        }

        private void btnUnblockAllFiltered_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (MessageBox.Show("Do you want to UNBLOCK all filtered item?", "UNBLOCK ITEM", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                List<P_Label_Entity> list_selected = GetDataRows(gvResult);
                string strQry = "";
                string qry2 = "";
                if (list_selected != null)
                {
                    foreach (P_Label_Entity item in list_selected)
                    {
                        strQry += "update P_label set isLock=N'Unblock' where label_code=N'" + item.Label_code + "' \n";
                        if (qry2 == "")
                        {
                            qry2 += "select N'" + item.Label_code + "',N'" + item.Product_code + "',N'" + item.Product_customer_code + "',N'" + item.Product_quantity + "',N'" + item.Lot_no + "'";
                            qry2 += ",N'Unblock carton',N'" + item.Wh_location + "',N'" + item.Pallet_no + "',getdate(),N'" + General_Infor.username + "',N'" + item.Invoice_no + "' \n";
                        }
                        else
                        {
                            qry2 += "union all select N'" + item.Label_code + "',N'" + item.Product_code + "',N'" + item.Product_customer_code + "',N'" + item.Product_quantity + "',N'" + item.Lot_no + "'";
                            qry2 += ",N'Unblock carton',N'" + item.Wh_location + "',N'" + item.Pallet_no + "',getdate(),N'" + General_Infor.username + "',N'" + item.Invoice_no + "' \n";
                        }
                    }
                }
                if (strQry != "")
                {
                    strQry += "insert into W_HistoryOfTransaction(label_code,product_code,product_customer_code,product_quantity,lot_no,[transaction],[location],pallet_no,input_time,PIC,invoice_no) \n";
                    strQry += qry2;
                }
                conn = new CmCn();
                conn.ExcuteQry(strQry);
                Load_Data();
            }
        }
    }
}
