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

namespace HVN_System.View.Planning
{
    public partial class frmMasterListFG_BOM : Form
    {
        public frmMasterListFG_BOM()
        {
            InitializeComponent();
        }
        public frmMasterListFG_BOM(string item)
        {
            InitializeComponent();
            txtProductCustomerCode.Text = item;
            txtProductCustomerCode.ReadOnly = true;
        }
        private ADO adoClass;
        private List<P_MasterListProduct_BOM_Entity> List_Data;
        private void btnSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (Check_list_data())
            {
                adoClass = new ADO();
                adoClass.Update_P_MasterListProduct_BOM(List_Data, txtProductCustomerCode.Text);
                MessageBox.Show("Lưu thành công/ Save successfully");
                this.Close();
            }
        }
        private bool Check_list_data()
        {
            bool result = true;
            adoClass = new ADO();
            string List_error = "";
            foreach (P_MasterListProduct_BOM_Entity item in List_Data)
            {
                if (item.M_quantity > 0)
                {
                    DataTable dt = adoClass.Load_W_MasterList_Material("m_name", "m_name=N'" + item.M_name+"'");
                    if (dt.Rows.Count>0)
                    {

                    }
                    else
                    {
                        List_error += item.M_name+"\n";
                    }
                }
            }
            if (List_error != "")
            {
                MessageBox.Show("There are some unknow part number: \n" + List_error, "Error");
                result = false;
            }
            return result;
        }
        private void frmProductionPlanFG_Load(object sender, EventArgs e)
        {
            adoClass = new ADO();
            List_Data = new List<P_MasterListProduct_BOM_Entity>();
            DataTable dt = adoClass.Load_P_MasterListProduct_BOM("", "product_customer_code=N'"+txtProductCustomerCode.Text+"'");
            int stt = 1;
            if (dt.Rows.Count>0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    P_MasterListProduct_BOM_Entity item = new P_MasterListProduct_BOM_Entity();
                    item.Stt = stt;
                    item.M_name = row["m_name"].ToString();
                    item.M_quantity = float.Parse(row["m_quantity"].ToString());
                    List_Data.Add(item);
                    stt++;
                }
            }
            for (int i = dt.Rows.Count; i < 50; i++)
            {
                P_MasterListProduct_BOM_Entity item = new P_MasterListProduct_BOM_Entity();
                item.Stt = stt;
                item.M_quantity = 0;
                List_Data.Add(item);
                stt++;
            }
            dgvResult.DataSource = List_Data.ToList();
        }

        private void btnDelete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }
    }
}
