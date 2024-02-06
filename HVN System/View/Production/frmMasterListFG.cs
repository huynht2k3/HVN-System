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
using System.Runtime.CompilerServices;
using Outlook = Microsoft.Office.Interop.Outlook;
using HVN_System.View.Planning;

namespace HVN_System
{
    public partial class frmMasterListFG : DevExpress.XtraEditors.XtraForm, INotifyPropertyChanged
    {
        public frmMasterListFG()
        {
            InitializeComponent();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private ObservableCollection<P_FG_Entity> List_Data;
        private ObservableCollection<P_FG_Entity> List_Submit;
        private ObservableCollection<P_ChangingFGData_Entity> _list_Submit_Change;
        private ObservableCollection<P_ChangingFGData_Entity> List_Submit_Change
        {
            get { return _list_Submit_Change; }
            set
            {
                _list_Submit_Change = value; OnPropertyChanged();
                dgvPending.DataSource = List_Submit_Change;
            }
        }

        private P_FG_Entity selected_FG_entity, changed_value_FG;
        private CmCn conn;
        private DataTable dt, dt_pending;
        private bool isAddNew;
        private ADO adoClass;
        private int stt_HVNCode;
        //string PN, PN_HVN, Product_Name, STD_Weight, STD_Quantity;
        private void _CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            dgvPending.DataSource = List_Submit_Change;
        }
        private void Load_permission()
        {
            adoClass = new ADO();
            btnSubmit.Enabled = adoClass.Check_permission(this.Name, btnSubmit.Name, General_Infor.username);
            btnAddNew.Enabled = adoClass.Check_permission(this.Name, btnSubmit.Name, General_Infor.username);
            btnAddToRequest.Enabled = adoClass.Check_permission(this.Name, btnSubmit.Name, General_Infor.username);
            btnEditBOM.Enabled = adoClass.Check_permission(this.Name, btnEditBOM.Name, General_Infor.username);
        }
        private void frmMasterListFG_Load(object sender, EventArgs e)
        {
            Load_permission();
            Load_Masterlist_FG_data();
            List_Submit_Change = new ObservableCollection<P_ChangingFGData_Entity>();
            _list_Submit_Change.CollectionChanged += _CollectionChanged;
            adoClass = new ADO();
            DataTable dt_check_type = adoClass.Load_Parameter("check_type");
            cboCheckType.DataSource = dt_check_type;
            cboCheckType.DisplayMember = "child_name";
            cboCheckType.ValueMember = "child_name";
            DataTable dt_Prod_line = adoClass.Load_MasterListLine("line_name","");
            cboProdLine.DataSource = dt_Prod_line;
            cboProdLine.DisplayMember = "line_name";
            cboProdLine.ValueMember = "line_name";
            changed_value_FG = new P_FG_Entity();
            stt_HVNCode = adoClass.Load_HVN_Product_Code();
            btnAddNew.PerformClick();
            //----Add data to combobox inside Gridcontrol
            //foreach (DataRow item in dt_check_type.Rows)
            //{
            //    cboCheck_type.Items.Add(item["child_name"]);
            //}
        }
        private void Load_Masterlist_FG_data()
        {
            conn = new CmCn();
            string StrQry = " SELECT * FROM P_MasterListProduct";
            dt = conn.ExcuteDataTable(StrQry);
            List_Data = new ObservableCollection<P_FG_Entity>();
            int j = 1;
            foreach (DataRow row in dt.Rows)
            {
                P_FG_Entity item = new P_FG_Entity();
                item.Stt = j;
                item.Product_code = row["product_code"].ToString(); 
                item.Product_customer_code = row["product_customer_code"].ToString();
                item.Product_name = row["product_name"].ToString();
                item.Project_name = row["project_name"].ToString();
                item.Customer_name = row["customer_name"].ToString();
                item.Standard_time = string.IsNullOrEmpty(row["standard_time"].ToString()) ? 0 : float.Parse(row["standard_time"].ToString());
                item.Price = string.IsNullOrEmpty(row["product_price"].ToString()) ? 0 : float.Parse(row["product_price"].ToString());
                item.Standard_time_finishing = string.IsNullOrEmpty(row["standard_time_FG"].ToString()) ? 0 : float.Parse(row["standard_time_FG"].ToString());
                item.Product_quantity = string.IsNullOrEmpty(row["product_quantity"].ToString()) ? 0 : int.Parse(row["product_quantity"].ToString());
                item.Product_weight = string.IsNullOrEmpty(row["product_weight"].ToString()) ? 0 : float.Parse(row["product_weight"].ToString());
                item.Carton_type = row["carton_type"].ToString();
                item.Number_operator = string.IsNullOrEmpty(row["number_operator"].ToString()) ? 0 : int.Parse(row["number_operator"].ToString());
                item.Check_type = row["check_type"].ToString();
                item.Product_line = row["product_line"].ToString();
                item.Product_rev= row["product_rev"].ToString();
                item.Remark = row["remark"].ToString();
                item.Last_time_commit = row["last_time_commit"].ToString();
                item.Last_user_commit = row["last_user_commit"].ToString();
                item.Prod_att_link = row["prod_att_link"].ToString();
                item.Prod_att_name = row["prod_att_name"].ToString();
                item.Edit = false;
                List_Data.Add(item);
                j++;
            }
            dgvResult.DataSource = List_Data.ToList();
        }
        private void Load_Masterlist_FG_Submit()
        {
            conn = new CmCn();
            string StrQry = " SELECT * FROM P_MasterListProductSubmit";
            dt_pending = conn.ExcuteDataTable(StrQry);
            List_Submit = new ObservableCollection<P_FG_Entity>();
            foreach (DataRow row in dt_pending.Rows)
            {
               
            }
            dgvPending.DataSource = List_Submit.ToList();
        }
        private void btnSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            
        }

        private void btnAddNew_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            isAddNew = true;
            selected_FG_entity = new P_FG_Entity();
            adoClass = new ADO();
            selected_FG_entity.Product_code= "HVN"+stt_HVNCode.ToString("D5");
            GetSelectedDataToToolbox();
            //MessageBox.Show(5.ToString("D5"));
        }

        private void btnRefresh_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Load_Masterlist_FG_data();
        }

        private void btnExport_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            adoClass = new ADO();
            adoClass.Export_Excel(dgvResult);
        }

        private void gvResult_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            isAddNew = false; 
            selected_FG_entity = gvResult.GetRow(gvResult.FocusedRowHandle) as P_FG_Entity;
            GetSelectedDataToToolbox();
            changed_value_FG = new P_FG_Entity();
        }
        private void GetSelectedDataToToolbox()
        {
            txtProdName.Text = selected_FG_entity.Product_name;
            txtPJName.Text= selected_FG_entity.Project_name;
            txtHVNProdCode.Text= selected_FG_entity.Product_code;
            txtCustProdCode.Text= selected_FG_entity.Product_customer_code;
            txtCartonType.Text= selected_FG_entity.Carton_type;
            txtCustName.Text= selected_FG_entity.Customer_name;
            txtNumberOperator.Text = selected_FG_entity.Number_operator.ToString();
            txtPrice.Text= selected_FG_entity.Price.ToString();
            txtStandardQty.Text = selected_FG_entity.Product_quantity.ToString();
            txtStandardTime.Text = selected_FG_entity.Standard_time.ToString();
            txtStdTimeFinishing.Text = selected_FG_entity.Standard_time_finishing.ToString();
            txtStandardWeight.Text = selected_FG_entity.Product_weight.ToString();
            cboProdLine.Text = selected_FG_entity.Product_line;
            cboCheckType.Text= selected_FG_entity.Check_type;
            txtRevNo.Text = selected_FG_entity.Product_rev;
            txtRemark.Text= selected_FG_entity.Remark;
            txtAttName.Text = selected_FG_entity.Prod_att_name;
        }
        #region Changed_Data_Event
        private void txtCustProdCode_TextChanged(object sender, EventArgs e)
        {
            if (selected_FG_entity.Product_customer_code != txtCustProdCode.Text)
            {
                changed_value_FG.Product_customer_code = txtCustProdCode.Text;
            }
            else
            {
                changed_value_FG.Product_customer_code = null;
            }
        }

        private void txtProdName_TextChanged(object sender, EventArgs e)
        {
            if (selected_FG_entity.Product_name != txtProdName.Text)
            {
                changed_value_FG.Product_name = txtProdName.Text;
            }
            else
            {
                changed_value_FG.Product_name = null;
            }
        }

        private void txtPJName_TextChanged(object sender, EventArgs e)
        {
            if (selected_FG_entity.Project_name != txtPJName.Text)
            {
                changed_value_FG.Project_name = txtPJName.Text;
            }
            else
            {
                changed_value_FG.Project_name = null;
            }
        }

        private void txtCustName_TextChanged(object sender, EventArgs e)
        {
            if (selected_FG_entity.Customer_name != txtCustName.Text)
            {
                changed_value_FG.Customer_name = txtCustName.Text;
            }
            else
            {
                changed_value_FG.Customer_name = null;
            }
        }

        private void txtStandardTime_TextChanged(object sender, EventArgs e)
        {
            if (selected_FG_entity.Standard_time.ToString()!= txtStandardTime.Text)
            {
                try
                {
                    changed_value_FG.Standard_time = float.Parse(txtStandardTime.Text);
                }
                catch (Exception)
                {
                    MessageBox.Show("Input Standard_time wrong value!");
                }
            }
            else
            {
                changed_value_FG.Standard_time = 0;
            }
        }

        private void txtPrice_TextChanged(object sender, EventArgs e)
        {
            if (selected_FG_entity.Price.ToString() != txtPrice.Text)
            {
                try
                {
                    changed_value_FG.Price = float.Parse(txtPrice.Text);
                }
                catch (Exception)
                {
                    MessageBox.Show("Input Price wrong value!");
                }
            }
            else
            {
                changed_value_FG.Price = 0;
            }
        }

        private void txtStdTimeFinishing_TextChanged(object sender, EventArgs e)
        {
            if (selected_FG_entity.Standard_time_finishing.ToString() != txtStdTimeFinishing.Text)
            {
                try
                {
                    changed_value_FG.Standard_time_finishing = float.Parse(txtStdTimeFinishing.Text);
                }
                catch (Exception)
                {
                    MessageBox.Show("Input Standard_time_finishing wrong value!");
                }
            }
            else
            {
                changed_value_FG.Standard_time_finishing = 0;
            }
        }

        private void txtStandardQty_TextChanged(object sender, EventArgs e)
        {

            if (selected_FG_entity.Product_quantity.ToString() != txtStandardQty.Text)
            {
                try
                {
                    changed_value_FG.Product_quantity = int.Parse(txtStandardQty.Text);
                }
                catch (Exception)
                {
                    MessageBox.Show("Input Product_quantity wrong value!");
                }
            }
            else
            {
                changed_value_FG.Product_quantity = 0;
            }
        }

        private void txtStandardWeight_TextChanged(object sender, EventArgs e)
        {
            if (selected_FG_entity.Product_weight.ToString() != txtStandardWeight.Text)
            {
                try
                {
                    changed_value_FG.Product_weight = float.Parse(txtStandardWeight.Text);
                }
                catch (Exception)
                {
                    MessageBox.Show("Input Product_weight wrong value!");
                }
            }
            else
            {
                changed_value_FG.Product_quantity = 0;
            }
        }

        private void txtCartonType_TextChanged(object sender, EventArgs e)
        {
            if (selected_FG_entity.Carton_type != txtCartonType.Text)
            {
                changed_value_FG.Carton_type = txtCartonType.Text;
            }
            else
            {
                changed_value_FG.Carton_type = null;
            }
        }
        private void txtNumberOperator_TextChanged(object sender, EventArgs e)
        {
            if (selected_FG_entity.Number_operator.ToString() != txtNumberOperator.Text)
            {
                try
                {
                    changed_value_FG.Number_operator = int.Parse(txtNumberOperator.Text);
                }
                catch (Exception)
                {
                    MessageBox.Show("Input Number_operator wrong value!");
                }
            }
            else
            {
                changed_value_FG.Number_operator = 0;
            }
        }

        private void btnAddToRequest_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (isAddNew)
            {
                string Message_in_email = "*" + changed_value_FG.Product_customer_code + " <new product>:";
                Message_in_email+="\n \t Product name: " + changed_value_FG.Product_name + "\n \t Product revision: " + changed_value_FG.Product_rev + "\n \t Project: " + changed_value_FG.Project_name + "\n \t Customer: " + changed_value_FG.Customer_name;
                Message_in_email += "\n \t Price: " + changed_value_FG.Price + "\n \t Standard time: " + changed_value_FG.Standard_time_finishing + "\n \t Quantity per box: " + changed_value_FG.Product_quantity;
                Message_in_email += "\n \t Weight: " + changed_value_FG.Product_weight + "\n \t Carton type: " + changed_value_FG.Carton_type + "\n \t Check type: " + changed_value_FG.Check_type;
                string info_change = "<New product> Product name: " + changed_value_FG.Product_name + ", Project: " + changed_value_FG.Project_name + ", Customer: " + changed_value_FG.Customer_name ;
                string sql_query = " insert into P_MasterListProduct ([product_code],[product_customer_code],[product_name],[project_name],[customer_name],[last_time_commit],[last_user_commit],[standard_time],[product_price],[standard_time_FG],[product_quantity],[product_weight],[carton_type],[number_operator],[product_line],[check_type],[hs_code],[product_rev]) select N@" + selected_FG_entity.Product_code + "@,N@" + changed_value_FG.Product_customer_code + "@,N@" + changed_value_FG.Product_name + "@,N@" + changed_value_FG.Project_name + "@,N@" + changed_value_FG.Customer_name + "@,getdate(),N@" + General_Infor.username + "@,N@" + changed_value_FG.Standard_time + "@,N@" + changed_value_FG.Price + "@,N@" + changed_value_FG.Standard_time_finishing + "@,N@" + changed_value_FG.Product_quantity  + "@,N@" + changed_value_FG.Product_weight + "@,N@" + changed_value_FG.Carton_type + "@,N@" + changed_value_FG.Number_operator + "@,N@" + changed_value_FG.Product_line + "@,N@" + changed_value_FG.Check_type + "@,N@" + changed_value_FG.Product_customer_code + "@,N@" + changed_value_FG.Product_rev + "@";
                string revert_query = " delete from P_MasterListProduct () where product_code=N@" + changed_value_FG.Product_customer_code + "@";
                P_ChangingFGData_Entity item = new P_ChangingFGData_Entity();
                item.Product_code = selected_FG_entity.Product_code;
                item.Product_customer_code = changed_value_FG.Product_customer_code;
                item.Modified_content = info_change;
                item.Modified_sql_query = sql_query;
                item.Recover_sql_query = revert_query;
                item.Message_in_email = Message_in_email;
                item.Request_time = DateTime.Now;
                item.Request_user = General_Infor.username;
                List_Submit_Change.Add(item);
                stt_HVNCode++;
            }
            else
            {
                if (txtHVNProdCode.Text != "")
                {
                    string Message_in_email = "*"+selected_FG_entity.Product_customer_code + ":";
                    string info_change="";
                    string sql_query = " update P_MasterListProduct set ";
                    string revert_query= " update P_MasterListProduct set ";
                    int count = 0;
                    if (!string.IsNullOrEmpty(changed_value_FG.Product_name))
                    {
                        Message_in_email += "\n \t Product name:" + selected_FG_entity.Product_name + "->" + changed_value_FG.Product_name;
                        info_change += "Product name:" + selected_FG_entity.Product_name + "->" + changed_value_FG.Product_name;
                        count++;
                        sql_query += " product_name=N@"+ changed_value_FG.Product_name + "@ ";
                        revert_query += " product_name=N@" + selected_FG_entity.Product_name + "@ ";
                    }
                    if (!string.IsNullOrEmpty(changed_value_FG.Product_customer_code))
                    {
                        Message_in_email += "\n \t Product_customer_code:" + selected_FG_entity.Product_customer_code + "->" + changed_value_FG.Product_customer_code;
                        info_change += ", Product_customer_code:" + selected_FG_entity.Product_customer_code + "->" + changed_value_FG.Product_customer_code;
                        count++;
                        if (count==1)
                        {
                            sql_query += " product_customer_code=N@" + changed_value_FG.Product_customer_code + "@ ";
                            revert_query += " product_customer_code=N@" + selected_FG_entity.Product_customer_code + "@ ";
                        }
                        else
                        {
                            sql_query += " ,product_customer_code=N@" + changed_value_FG.Product_customer_code + "@ ";
                            revert_query += " ,product_customer_code=N@" + selected_FG_entity.Product_customer_code + "@ ";
                        }
                    }
                    if (!string.IsNullOrEmpty(changed_value_FG.Project_name))
                    {
                        Message_in_email += "\n \t Project_name:" + selected_FG_entity.Project_name + "->" + changed_value_FG.Project_name;
                        info_change += ", Project_name:" + selected_FG_entity.Project_name + "->" + changed_value_FG.Project_name;
                        count++;
                        if (count == 1)
                        {
                            sql_query += " project_name=N@" + changed_value_FG.Project_name + "@ ";
                            revert_query += " project_name=N@" + selected_FG_entity.Project_name + "@ ";
                        }
                        else
                        {
                            sql_query += " ,project_name=N@" + changed_value_FG.Project_name + "@ ";
                            revert_query += " ,project_name=N@" + selected_FG_entity.Project_name + "@ ";
                        }
                    }
                    if (!string.IsNullOrEmpty(changed_value_FG.Customer_name))
                    {
                        Message_in_email += "\n \t Customer_name:" + selected_FG_entity.Customer_name + "->" + changed_value_FG.Customer_name;
                        info_change += ", Customer_name:" + selected_FG_entity.Customer_name + "->" + changed_value_FG.Customer_name;
                        count++;
                        if (count == 1)
                        {
                            sql_query += " customer_name=N@" + changed_value_FG.Customer_name + "@ ";
                            revert_query += " customer_name=N@" + selected_FG_entity.Customer_name + "@ ";
                        }
                        else
                        {
                            sql_query += " ,customer_name=N@" + changed_value_FG.Customer_name + "@ ";
                            revert_query += " ,customer_name=N@" + selected_FG_entity.Customer_name + "@ ";
                        }
                    }
                    if (!string.IsNullOrEmpty(changed_value_FG.Remark))
                    {
                        Message_in_email += "\n \t Remark:" + selected_FG_entity.Remark + "->" + changed_value_FG.Remark;
                        info_change += ", Remark:" + selected_FG_entity.Remark + "->" + changed_value_FG.Remark;
                        count++;
                        if (count == 1)
                        {
                            sql_query += " remark=N@" + changed_value_FG.Remark + "@ ";
                            revert_query += " remark=N@" + selected_FG_entity.Remark + "@ ";
                        }
                        else
                        {
                            sql_query += " ,remark=N@" + changed_value_FG.Remark + "@ ";
                            revert_query += " ,remark=N@" + selected_FG_entity.Remark + "@ ";
                        }
                    }
                    if (changed_value_FG.Standard_time != 0)
                    {
                        Message_in_email += "\n \t Standard_time:" + selected_FG_entity.Standard_time + "->" + changed_value_FG.Standard_time;
                        count++;
                        info_change += ", Standard_time:" + selected_FG_entity.Standard_time + "->" + changed_value_FG.Standard_time;
                        if (count == 1)
                        {
                            sql_query += " Standard_time=N@" + changed_value_FG.Standard_time + "@ ";
                            revert_query += " Standard_time=N@" + selected_FG_entity.Standard_time + "@ ";
                        }
                        else
                        {
                            sql_query += " ,Standard_time=N@" + changed_value_FG.Standard_time + "@ ";
                            revert_query += " ,Standard_time=N@" + selected_FG_entity.Standard_time + "@ ";
                        }
                    }
                    if (changed_value_FG.Price != 0)
                    {
                        Message_in_email += "\n \t Price:" + selected_FG_entity.Price + "->" + changed_value_FG.Price; count++;
                        info_change += ", Price:" + selected_FG_entity.Price + "->" + changed_value_FG.Price;
                        if (count == 1)
                        {
                            sql_query += " product_price=N@" + changed_value_FG.Price + "@ ";
                            revert_query += " product_price=N@" + selected_FG_entity.Price + "@ ";
                        }
                        else
                        {
                            sql_query += " ,product_price=N@" + changed_value_FG.Price + "@ ";
                            revert_query += " ,product_price=N@" + selected_FG_entity.Price + "@ ";
                        }
                    }
                    if (changed_value_FG.Standard_time_finishing != 0)
                    {
                        Message_in_email += "\n \t Standard_time_finishing:" + selected_FG_entity.Standard_time_finishing + "->" + changed_value_FG.Standard_time_finishing; count++;
                        info_change += ", Standard_time_finishing:" + selected_FG_entity.Standard_time_finishing + "->" + changed_value_FG.Standard_time_finishing;
                        if (count == 1)
                        {
                            sql_query += " standard_time_FG=N@" + changed_value_FG.Standard_time_finishing + "@ ";
                            revert_query += " standard_time_FG=N@" + selected_FG_entity.Standard_time_finishing + "@ ";
                        }
                        else
                        {
                            sql_query += " ,standard_time_FG=N@" + changed_value_FG.Standard_time_finishing + "@ ";
                            revert_query += " ,standard_time_FG=N@" + selected_FG_entity.Standard_time_finishing + "@ ";
                        }
                    }
                    if (changed_value_FG.Product_quantity != 0)
                    {
                        Message_in_email += "\n \t Product_quantity:" + selected_FG_entity.Product_quantity + "->" + changed_value_FG.Product_quantity; count++;
                        info_change += ", Product_quantity:" + selected_FG_entity.Product_quantity + "->" + changed_value_FG.Product_quantity;
                        if (count == 1)
                        {
                            sql_query += " product_quantity=N@" + changed_value_FG.Product_quantity + "@ ";
                            revert_query += " product_quantity=N@" + selected_FG_entity.Product_quantity + "@ ";
                        }
                        else
                        {
                            sql_query += " ,product_quantity=N@" + changed_value_FG.Product_quantity + "@ ";
                            revert_query += " ,product_quantity=N@" + selected_FG_entity.Product_quantity + "@ ";
                        }
                    }
                    if (changed_value_FG.Product_weight != 0)
                    {
                        Message_in_email += "\n \t Product_weight:" + selected_FG_entity.Product_weight + "->" + changed_value_FG.Product_weight; count++;
                        info_change += ", Product_weight:" + selected_FG_entity.Product_weight + "->" + changed_value_FG.Product_weight; 
                        if (count == 1)
                        {
                            sql_query += " product_weight=N@" + changed_value_FG.Product_weight + "@ ";
                            revert_query += " product_weight=N@" + selected_FG_entity.Product_weight + "@ ";
                        }
                        else
                        {
                            sql_query += " ,product_weight=N@" + changed_value_FG.Product_weight + "@ ";
                            revert_query += " ,product_weight=N@" + selected_FG_entity.Product_weight + "@ ";
                        }
                    }
                    if (!string.IsNullOrEmpty(changed_value_FG.Carton_type))
                    {
                        Message_in_email += "\n \t Carton_type:" + selected_FG_entity.Carton_type + "->" + changed_value_FG.Carton_type; count++;
                        info_change += ", Carton_type:" + selected_FG_entity.Carton_type + "->" + changed_value_FG.Carton_type;
                        if (count == 1)
                        {
                            sql_query += " carton_type=N@" + changed_value_FG.Carton_type + "@ ";
                            revert_query += " carton_type=N@" + selected_FG_entity.Carton_type + "@ ";
                        }
                        else
                        {
                            sql_query += " ,carton_type=N@" + changed_value_FG.Carton_type + "@ ";
                            revert_query += " ,carton_type=N@" + selected_FG_entity.Carton_type + "@ ";
                        }
                    }
                    if (changed_value_FG.Number_operator != 0)
                    {
                        Message_in_email += "\n \t Number_operator:" + selected_FG_entity.Number_operator + "->" + changed_value_FG.Number_operator; count++;
                        info_change += ", Number_operator:" + selected_FG_entity.Number_operator + "->" + changed_value_FG.Number_operator;
                        if (count == 1)
                        {
                            sql_query += " number_operator=N@" + changed_value_FG.Number_operator + "@ ";
                            revert_query += " number_operator=N@" + selected_FG_entity.Number_operator + "@ ";
                        }
                        else
                        {
                            sql_query += " ,number_operator=N@" + changed_value_FG.Number_operator + "@ ";
                            revert_query += " ,number_operator=N@" + selected_FG_entity.Number_operator + "@ ";
                        }
                    }
                    if (!string.IsNullOrEmpty(changed_value_FG.Product_line))
                    {
                        Message_in_email += "\n \t Product_line:" + selected_FG_entity.Product_line + "->" + changed_value_FG.Product_line; count++;
                        info_change += ", Product_line:" + selected_FG_entity.Product_line + "->" + changed_value_FG.Product_line;
                        if (count == 1)
                        {
                            sql_query += " product_line=N@" + changed_value_FG.Product_line + "@ ";
                            revert_query += " product_line=N@" + selected_FG_entity.Product_line + "@ ";
                        }
                        else
                        {
                            sql_query += " ,product_line=N@" + changed_value_FG.Product_line + "@ ";
                            revert_query += " ,product_line=N@" + selected_FG_entity.Product_line + "@ ";
                        }
                    }
                    if (!string.IsNullOrEmpty(changed_value_FG.Check_type))
                    {
                        Message_in_email += "\n \t Check_type:" + selected_FG_entity.Check_type + "->" + changed_value_FG.Check_type; count++;
                        info_change += ", Check_type:" + selected_FG_entity.Check_type + "->" + changed_value_FG.Check_type;
                        if (count == 1)
                        {
                            sql_query += " check_type=N@" + changed_value_FG.Check_type + "@ ";
                            revert_query += " check_type=N@" + selected_FG_entity.Check_type + "@ ";
                        }
                        else
                        {
                            sql_query += " ,check_type=N@" + changed_value_FG.Check_type + "@ ";
                            revert_query += " ,check_type=N@" + selected_FG_entity.Check_type + "@ ";
                        }
                    }
                    if (!string.IsNullOrEmpty(changed_value_FG.Product_rev))
                    {
                        Message_in_email += "\n \t Product_rev:" + selected_FG_entity.Product_rev + "->" + changed_value_FG.Product_rev; count++;
                        info_change += ", Product_rev:" + selected_FG_entity.Product_rev + "->" + changed_value_FG.Product_rev;
                        if (count == 1)
                        {
                            sql_query += " product_rev=N@" + changed_value_FG.Product_rev + "@ ";
                            revert_query += " product_rev=N@" + selected_FG_entity.Product_rev + "@ ";
                        }
                        else
                        {
                            sql_query += " ,product_rev=N@" + changed_value_FG.Product_rev + "@ ";
                            revert_query += " ,product_rev=N@" + selected_FG_entity.Product_rev + "@ ";
                        }
                    }
                    sql_query += ",last_user_commit =N@" + General_Infor.username + "@,last_time_commit=N@" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "@";
                    sql_query += " where product_code=@" + selected_FG_entity.Product_code + "@";
                    revert_query += " where product_code=@" + selected_FG_entity.Product_code + "@";
                    if (count>0)
                    {
                        P_ChangingFGData_Entity item = new P_ChangingFGData_Entity();
                        item.Product_code = selected_FG_entity.Product_code;
                        item.Product_customer_code = selected_FG_entity.Product_customer_code;
                        item.Modified_content = info_change;
                        item.Modified_sql_query = sql_query;
                        item.Recover_sql_query = revert_query;
                        item.Message_in_email = Message_in_email;
                        item.Request_time = DateTime.Now;
                        item.Request_user = General_Infor.username;
                        List_Submit_Change.Add(item);
                    }
                    else
                    {
                        MessageBox.Show("You donot change anything.Please check again");
                    }       
                }
            }
        }

        private void btnSubmit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (XtraMessageBox.Show("Do you want to submit the request?", "Submit Request", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                adoClass = new ADO();
                adoClass.Submit_Changing_FG_Data(List_Submit_Change);
                SendEmail();
                List_Submit_Change = new ObservableCollection<P_ChangingFGData_Entity>();
                MessageBox.Show("Submit successfully");
            }
        }
        private void cboProdLine_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (selected_FG_entity.Product_line != cboProdLine.Text)
            {
                changed_value_FG.Product_line = cboProdLine.Text;
            }
            else
            {
                changed_value_FG.Product_line = null;
            }
        }

        private void repositoryItemButtonEdit1_Click(object sender, EventArgs e)
        {
            //P_ChangingFGData_Entity item = gvPending.GetRow(gvResult.FocusedRowHandle) as P_ChangingFGData_Entity;
            //List_Submit_Change.Re
        }

        private void txtRevNo_TextChanged(object sender, EventArgs e)
        {
            if (selected_FG_entity.Product_rev != txtRevNo.Text)
            {
                changed_value_FG.Product_rev = txtRevNo.Text;
            }
            else
            {
                changed_value_FG.Product_rev = null;
            }
        }

        private void btnEditBOM_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (txtCustProdCode.Text!="")
            {
                frmMasterListFG_BOM frm = new frmMasterListFG_BOM(txtCustProdCode.Text);
                frm.ShowDialog();
            }
        }

        private void txtRemark_TextChanged(object sender, EventArgs e)
        {
            if (selected_FG_entity.Remark != txtRemark.Text)
            {
                changed_value_FG.Remark = txtRemark.Text;
            }
            else
            {
                changed_value_FG.Remark = null;
            }
        }

        private void btnAttUpload_Click(object sender, EventArgs e)
        {
            OpenFileDialog OpenFile = new OpenFileDialog();
            OpenFile.Title = "Open File";
            OpenFile.Filter = "ZIP (.zip)|*.zip";
            if (OpenFile.ShowDialog() != DialogResult.Cancel)
            {
                if (txtCustProdCode.Text != "")
                {
                   
                    string link = OpenFile.FileName;
                    string UploadFilePath = @"\\172.16.180.20\20.Public\05.IT\05.HVN_TOOL\ATTACHMENT\PRODUCTION\MasterListFG\" + txtCustProdCode.Text+".zip";
                    string strQry = "update P_MasterListProduct set prod_att_name=N'"+ txtCustProdCode.Text + ".zip', prod_att_link  =N'"+ UploadFilePath + "' where product_customer_code=N'"+ txtCustProdCode.Text + "'";
                    conn = new CmCn();
                    conn.ExcuteQry(strQry);
                    if (File.Exists(UploadFilePath))
                    {
                        File.Delete(UploadFilePath);
                    }
                    File.Copy(link, UploadFilePath);
                    MessageBox.Show("Upload successful");
                    //System.Diagnostics.Process.Start(ExportFilePath);
                }
                else
                {
                    MessageBox.Show("Please select the PN before click upload","ERROR");
                }
            }
        }

        private void btnAttDownload_Click(object sender, EventArgs e)
        {
            SaveFileDialog SaveDialog = new SaveFileDialog();
            SaveDialog.Title = "Save File";
            SaveDialog.Filter = "ZIP (.zip)|*.zip";
            if (SaveDialog.ShowDialog() != DialogResult.Cancel)
            {
                if (txtCustProdCode.Text != "")
                {
                    string DownloadFilePath = SaveDialog.FileName;
                    if (selected_FG_entity.Prod_att_link!="")
                    {
                        if (!File.Exists(DownloadFilePath))
                        {
                            File.Copy(selected_FG_entity.Prod_att_link, DownloadFilePath);
                            System.Diagnostics.Process.Start(DownloadFilePath);
                        }
                        else
                        {
                            String msg = "The file could not be saved." + Environment.NewLine + Environment.NewLine + "Path: " + DownloadFilePath;
                            MessageBox.Show(msg, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("There is no attachment", "ERROR");
                    }
                }
                else
                {
                    MessageBox.Show("Please select the PN before click download", "ERROR");
                }
            }
        }

        private void btnExportBOM_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            string strQry = "select product_customer_code as [FG Part Number],m_name as [RM Part Number],m_quantity as [Quantity] from P_MasterListProduct_BOM";
            conn = new CmCn();
            DataTable dt = conn.ExcuteDataTable(strQry);
            dgvBOM.DataSource = dt;
            SaveFileDialog SaveDialog = new SaveFileDialog();
            SaveDialog.Filter = "Excel (.xlsx)|*.xlsx";
            if (SaveDialog.ShowDialog() != DialogResult.Cancel)
            {
                string ExportFilePath = SaveDialog.FileName;
                //Using System.IO;
                string FileExtenstion = Path.GetExtension(ExportFilePath);
                switch (FileExtenstion)
                {
                    case ".xlsx":
                        dgvBOM.ExportToXlsx(ExportFilePath);
                        break;
                    default:
                        break;
                }
                if (File.Exists(ExportFilePath))
                {
                    try
                    {
                        //Try to open the file and let windows decide how to open it.
                        System.Diagnostics.Process.Start(ExportFilePath);
                    }
                    catch
                    {
                        String msg = "The file could not be opened." + Environment.NewLine + Environment.NewLine + "Path: " + ExportFilePath;
                        MessageBox.Show(msg, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    String msg = "The file could not be saved." + Environment.NewLine + Environment.NewLine + "Path: " + ExportFilePath;
                    MessageBox.Show(msg, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void cboCheckType_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (selected_FG_entity.Check_type != cboCheckType.Text)
            {
                changed_value_FG.Check_type = cboCheckType.Text;
            }
            else
            {
                changed_value_FG.Check_type = null;
            }
        }
        #endregion
        private void SendEmail()
        {
            adoClass = new ADO();
            DataTable dt = adoClass.Load_PIC_Of_Process("Changing master data of FG", "");
            string Receiver="", Emaill_address="";
            foreach (DataRow item in dt.Rows)
            {
                Receiver += item["Name"].ToString()+",";
                Emaill_address+= item["Email_address"].ToString() + ";";
            }
            //Outlook.MailItem mailItem = (Outlook.MailItem)
            //this.Application.CreateItem(Outlook.OlItemType.olMailItem);
            Outlook.Application app = new Outlook.Application();
            Outlook.MailItem mailItem = app.CreateItem(Outlook.OlItemType.olMailItem);
            mailItem.Subject = "[HVN System] Changing masterlist data of finished goods";
            mailItem.To = Emaill_address;
            string Message = "";
            foreach (P_ChangingFGData_Entity item in List_Submit_Change)
            {
                Message += "\n" + item.Message_in_email;
            }
            mailItem.Body = "Dear "+ Receiver + " \n\nI would like to change the data as below "+ Message + "\n \n Best regards, \n";
            //mailItem.Attachments.Add(logPath);//logPath is a string holding path to the log.txt file
            //mailItem.Importance = Outlook.OlImportance.olImportanceHigh;
            try
            {
                if (!string.IsNullOrEmpty(Emaill_address))
                {
                    mailItem.Send();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message); 
            }

        }
    }
}