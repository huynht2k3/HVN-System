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
using DevExpress.XtraCharts;
using DevExpress.Utils.TouchHelpers;

namespace HVN_System.View.PlantKPI
{
    public partial class frmKPIHRDisplaySafetyTRIR : Form
    {
        public frmKPIHRDisplaySafetyTRIR()
        {
            InitializeComponent();
        }
        public frmKPIHRDisplaySafetyTRIR(string NoDayWithoutAcc,string NoDone,string NoNotDone,string NumberDS)
        {
            InitializeComponent();
            NDWA = NoDayWithoutAcc;
            txtNumberDSDone.Text = NoDone;
            txtNumberDSNotDone.Text = NoNotDone;
            txtNumberDS.Text = NumberDS;
        }
        private string NDWA;
        private ADO adoClass;
        private ADM_Document_Entity Current_Doc;
        private List<ADM_Document_Entity> List_Data;
        private int Number_Doc;

        private void btnHome_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            Number_Doc += 1;
            var result = List_Data.FirstOrDefault(x => x.Stt == Number_Doc);
            this.pdfViewer1.LoadDocument(result.Doc_link);
            if (List_Data.Count <= Number_Doc)
            {
                btnBack.Enabled = false;
            }
            if (Number_Doc > 1)
            {
                btnNext.Enabled = true;
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            Number_Doc += -1;
            var result = List_Data.FirstOrDefault(x => x.Stt == Number_Doc);
            this.pdfViewer1.LoadDocument(result.Doc_link);
            if (Number_Doc == 1)
            {
                btnNext.Enabled = false;
            }
            if (List_Data.Count > Number_Doc)
            {
                btnBack.Enabled = true;
            }
        }

        private void frmDashboardPlantKPI_Load(object sender, EventArgs e)
        {
            Load_Source_Data();
            txtNoDayWithoutAccident.Text = NDWA;
            lbCurrentDateTime.Text = DateTime.Now.ToString("MM/dd/yyyy hh:mm");
            this.WindowState= FormWindowState.Maximized;
            Load_Source_Data();
            btnNext.Enabled = false;
            if (List_Data.Count == 1)
            {
                btnBack.Enabled = false;
            }
        }
        private void cboYear_SelectionChangeCommitted(object sender, EventArgs e)
        {
            Load_Source_Data();
        }
        

        private void btnDisplayTRIR_Click(object sender, EventArgs e)
        {
            frmKPIHRDisplaySafetyTRIRDetail frm = new frmKPIHRDisplaySafetyTRIRDetail(txtNoDayWithoutAccident.Text);
            frm.ShowDialog();
        }

        private void Load_Source_Data()
        {
            adoClass = new ADO();
            DataTable dt = adoClass.Load_ADM_Document("", "doc_kind='Safety TRIR' order by doc_date desc");
            List_Data = new List<ADM_Document_Entity>();
            int Stt = 1;
            foreach (DataRow row in dt.Rows)
            {
                Current_Doc = new ADM_Document_Entity();
                Current_Doc.Stt = Stt;
                Current_Doc.Doc_id = row["doc_id"].ToString();
                Current_Doc.Doc_kind = row["doc_kind"].ToString();
                Current_Doc.Doc_content = row["doc_content"].ToString();
                Current_Doc.Doc_link = row["doc_link"].ToString();
                Current_Doc.Doc_date = row["doc_date"].ToString();
                Current_Doc.Doc_note = row["doc_note"].ToString();
                List_Data.Add(Current_Doc);
                Stt++;
            }
            var result = List_Data.FirstOrDefault(x => x.Stt == 1);
            Number_Doc = 1;
            this.pdfViewer1.LoadDocument(result.Doc_link);
        }
    }
}
