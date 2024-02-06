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
using DevExpress.XtraSplashScreen;
using HVN_System.View.Admin;

namespace HVN_System.View.Admin
{
    public partial class frmWHReprintPalletLabel : Form
    {
        public frmWHReprintPalletLabel()
        {
            InitializeComponent();
        }
        private ADO adoClass;

        private void frmWHReprintPalletLabel_Load(object sender, EventArgs e)
        {
           
        }

        private void btnPrint_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            adoClass = new ADO();
            if (txtPalletNo.Text != "" && txtPIC.Text != "")
            {
                SplashScreenManager.ShowForm(this, typeof(frmWaitingForm), true, true, false);
                SplashScreenManager.Default.SetWaitFormCaption("Printing...");
                adoClass.Print_Pallet_Label(txtPalletNo.Text, txtPIC.Text);
                SplashScreenManager.CloseForm();
                txtPalletNo.Text = "";
                txtPIC.Text = "";
                MessageBox.Show("Print Successfully!");
            }
            else
            {
                MessageBox.Show("Need fill out all information");
            }
        }
    }
}
