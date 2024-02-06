using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HVN_System.View.PlantKPI
{
    public partial class frmKPIHREditSafetyData : Form
    {
        public frmKPIHREditSafetyData()
        {
            InitializeComponent();
        }
        private CmCn conn;
        public frmKPIHREditSafetyData(string LastDateAccident,string NoDS,string DSDone,string NoDSNotDone)
        {
            InitializeComponent();
            dtpLastAccDate.Value = DateTime.Parse(LastDateAccident);
            txtNoDS.Text = NoDS;
            txtNoDSDone.Text = DSDone;
            txtNoDSNotDone.Text = NoDSNotDone;
        }
        private void frmKPIHREditSafetyData_Load(object sender, EventArgs e)
        {

        }

        private void btnSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            string strQry = "update KPI_PlantDashBoard set value =N'" + txtNoDS.Text + "' where item_name=N'txtNumberDS'\n";
            strQry += "update KPI_PlantDashBoard set value_string =N'" + dtpLastAccDate.Value.ToString("yyyy-MM-dd") + "' where item_name=N'txtLastDateAccident'\n";
            strQry += "update KPI_PlantDashBoard set value =N'" + txtNoDSDone.Text + "' where item_name=N'txtNumberDSDone'\n";
            strQry += "update KPI_PlantDashBoard set value =N'" + txtNoDSNotDone.Text + "' where item_name=N'txtNumberDSNotDone'\n";
            conn = new CmCn();
            conn.ExcuteQry(strQry);
            MessageBox.Show("The data has been saved");
            this.Close();
        }
    }
}
