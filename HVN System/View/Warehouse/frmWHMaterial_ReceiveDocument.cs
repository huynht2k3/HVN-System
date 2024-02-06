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
using HVN_System.View.Planning;
using DevExpress.XtraSplashScreen;
using HVN_System.View.Admin;

namespace HVN_System.View.Warehouse
{
    public partial class frmWHMaterial_ReceiveDocument : Form
    {
        public frmWHMaterial_ReceiveDocument()
        {
            InitializeComponent();
        }
        private ADO adoClass;
        private List<W_M_ReceiveDoc_Entity> List_Data;
        private W_M_ReceiveDoc_Entity Current_Doc;
        private void btnEdit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (Current_Doc.Rm_doc_id!=null)
            {
                frmWHMaterial_ReceiveDocumentDetail frm = new frmWHMaterial_ReceiveDocumentDetail(Current_Doc,false);
                frm.ShowDialog();
                Load_List_Doc();
            }
        }
        private void btnRefresh_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Load_List_Doc();
        }

        private void frmProductionPlanFG_Load(object sender, EventArgs e)
        {
            Load_List_Doc();
        }
        private void Load_List_Doc()
        {
            List_Data = new List<W_M_ReceiveDoc_Entity>();
            adoClass = new ADO();
            DataTable dt = adoClass.Load_W_M_ReceiveDoc("", "");
            foreach (DataRow row in dt.Rows)
            {
                W_M_ReceiveDoc_Entity item = new W_M_ReceiveDoc_Entity();
                item.Rm_doc_id = row["rm_doc_id"].ToString();
                item.Supplier = row["supplier"].ToString();
                item.Receive_date = string.IsNullOrEmpty(row["receive_date"].ToString())?DateTime.Today:DateTime.Parse(row["receive_date"].ToString());
                item.Rm_doc_link = row["rm_doc_link"].ToString();
                item.Rm_doc_link2 = row["rm_doc_link2"].ToString();
                item.Rm_doc_link3 = row["rm_doc_link3"].ToString();
                item.Last_user_commit = row["last_user_commit"].ToString();
                item.Rm_doc_name = row["rm_doc_name"].ToString();
                item.Rm_doc_name2 = row["rm_doc_name2"].ToString();
                item.Rm_doc_name3 = row["rm_doc_name3"].ToString();
                item.Rm_kind= row["rm_kind"].ToString();
                item.Truck_no= row["truck_no"].ToString();
                List_Data.Add(item);
            }
            dgvResult.DataSource = List_Data.OrderByDescending(x =>x.Receive_date).ToList();
        }
        private void btnNew_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmWHMaterial_ReceiveDocumentDetail frm = new frmWHMaterial_ReceiveDocumentDetail();
            frm.ShowDialog();
            Load_List_Doc();
        }

        private void gvResult_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            Current_Doc = gvResult.GetRow(gvResult.FocusedRowHandle) as W_M_ReceiveDoc_Entity;
        }

        private void btnDelete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (XtraMessageBox.Show("Do you want to delete this documment?", "Delete documment", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                adoClass = new ADO();
                adoClass.Delete_W_M_ReceiveDocDetail(Current_Doc.Rm_doc_id);
                try
                {
                    File.Delete(Current_Doc.Rm_doc_link);
                    File.Delete(Current_Doc.Rm_doc_link2);
                    File.Delete(Current_Doc.Rm_doc_link3);
                }
                catch (Exception)
                {
                    
                }
                Load_List_Doc();
            }
        }

        private void gvResult_DoubleClick(object sender, EventArgs e)
        {
            btnEdit.PerformClick();
        }

        private void dgvResult_Click(object sender, EventArgs e)
        {

        }

        private void btnPrint_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmWHMaterial_ReceiveDocumentDetail frm = new frmWHMaterial_ReceiveDocumentDetail(Current_Doc,true);
            frm.Show();
        }

        private void btnExportResult_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            SaveFileDialog SaveDialog = new SaveFileDialog();
            SaveDialog.Filter = "Excel (.xlsx)|*.xlsx";
            if (SaveDialog.ShowDialog() != DialogResult.Cancel)
            {
                SplashScreenManager.ShowForm(this, typeof(frmWaitingForm), true, true, false);
                SplashScreenManager.Default.SetWaitFormCaption("Exporting...");
                string ExportFilePath = SaveDialog.FileName;
                adoClass = new ADO();
                adoClass.Print_W_M_Receive_Document(Current_Doc.Rm_doc_id,Current_Doc.Truck_no,"OK","OK","OK",true, ExportFilePath);
                SplashScreenManager.CloseForm();
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
    }
}
