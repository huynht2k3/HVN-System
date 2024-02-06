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
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraBars;
using DevExpress.XtraSplashScreen;
using HVN_System.View.Admin;

namespace HVN_System.View.Planning
{
    public partial class frmWHMaterial_ReceiveDocumentPrint : Form
    {
        public frmWHMaterial_ReceiveDocumentPrint()
        {
            InitializeComponent();
        }
        public frmWHMaterial_ReceiveDocumentPrint(List<W_M_ReceiveLabel_Entity> item,string kind_print)
        {
            InitializeComponent();
            List_Data = item;
            dgvResult.DataSource = List_Data.ToList();
            kind_printing = kind_print;
        }
        private ADO adoClass;
        private List<W_M_ReceiveLabel_Entity> List_Data;
        string kind_printing;
        
        private void frmProductionPlanFG_Load(object sender, EventArgs e)
        {
            
        }

        private void gvResult_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            
        }

       

        private void repositoryItemCheckEdit1_EditValueChanged(object sender, EventArgs e)
        {
            if (gvResult.PostEditor())
            {
                gvResult.UpdateCurrentRow();
            }
        }

        private void btnPrint_ItemClick(object sender, ItemClickEventArgs e)
        {
            SplashScreenManager.ShowForm(this, typeof(frmWaitingForm), true, true, false);
            SplashScreenManager.Default.SetWaitFormCaption("Printing...");
            adoClass = new ADO();
            foreach (W_M_ReceiveLabel_Entity row in List_Data)
            {
                if (row.IsSelected)
                {
                    if (kind_printing=="stock")
                    {
                        adoClass.Insert_W_M_ReceiveLabel_Stock(row);
                        adoClass.Print_W_M_ReceiveLabel(row, "QCOK");
                    }
                    else
                    {
                        adoClass.Insert_W_M_ReceiveLabel(row);
                        adoClass.Print_W_M_ReceiveLabel(row, "WH");
                    }
                }
            }
            SplashScreenManager.CloseForm();
            MessageBox.Show("Print Successfully");
            this.Close();
        }

        private void dgvResult_Click(object sender, EventArgs e)
        {

        }
    }
}
