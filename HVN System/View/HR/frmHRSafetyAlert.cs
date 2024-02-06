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

namespace HVN_System.View.HR
{
    public partial class frmHRSafetyAlert : Form
    {
        public frmHRSafetyAlert()
        {
            InitializeComponent();
        }
        private CmCn conn;
        private ADO adoClass;
        private ADM_Document_Entity Current_Doc;
        private List<ADM_Document_Entity> List_Data;
        private void btnOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog OpenFile = new OpenFileDialog();
            OpenFile.Title = "Mở tệp tin";
            OpenFile.Filter = "PDF (.pdf)|*.pdf";
            if (OpenFile.ShowDialog() != DialogResult.Cancel)
            {
                txtLink.Text = OpenFile.FileName;
            }
        }

        private void btnSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            conn = new CmCn();
            string strQry = " select max(doc_id) from ADM_Document";
            int Stt = int.Parse(conn.ExcuteString(strQry)) + 1;
            string doc_name = "";
            string des = "";
            switch (cboKindReport.Text)
            {
                case "Safety Alert":
                    doc_name = "\\SAFETY_ALERT_" + Stt + ".pdf";
                    des = @"\\172.16.180.20\20.Public\05.IT\05.HVN_TOOL\ATTACHMENT" + doc_name;
                    break;
                case "Safety TRIR":
                    doc_name = "\\SAFETY_TRIR_" + Stt + ".pdf";
                    des = @"\\172.16.180.20\20.Public\05.IT\05.HVN_TOOL\ATTACHMENT" + doc_name;
                    break;
                default:
                    break;
            }
            if (doc_name != "" && txtLink.Text != "")
            {
                string source = @txtLink.Text;
                try
                {
                    File.Copy(source, des);
                    string strQry2 = "insert into ADM_Document (doc_kind,doc_content,doc_link,doc_date,time_commit) \n";
                    strQry2 += "values (N'" + cboKindReport.Text + "',N'" + txtContent.Text + "',N'" + des + "',N'" + dtpNotificationDate.Value.ToString("yyyy-MM-dd") + "',getdate())";
                    conn.ExcuteQry(strQry2);
                    Load_Doc();
                    txtContent.Text = "";
                    txtLink.Text = "";
                    MessageBox.Show("Tải lên thành công!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Lỗi điền thiếu thông tin loại thông báo hoặc chưa chọn file");
            }
        }
        private void Load_Doc()
        {
            adoClass = new ADO();
            DataTable dt = adoClass.Load_ADM_Document("", "");
            List_Data = new List<ADM_Document_Entity>();
            foreach (DataRow row in dt.Rows)
            {
                Current_Doc = new ADM_Document_Entity();
                Current_Doc.Doc_id = row["doc_id"].ToString();
                Current_Doc.Doc_kind = row["doc_kind"].ToString();
                Current_Doc.Doc_content = row["doc_content"].ToString();
                Current_Doc.Doc_link = row["doc_link"].ToString();
                Current_Doc.Doc_date = row["doc_date"].ToString();
                Current_Doc.Doc_note = row["doc_note"].ToString();
                List_Data.Add(Current_Doc);
            }
            dgvResult.DataSource = List_Data;
        }
        private void frmHRSafetyAlert_Load(object sender, EventArgs e)
        {
            Load_Doc();
        }

        private void btnDelete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (XtraMessageBox.Show("Do you want to delete this item?", "Delete Item", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    conn = new CmCn();
                    string strQry = "delete from ADM_Document where doc_id=N'" + Current_Doc.Doc_id + "'";
                    conn.ExcuteQry(strQry);
                    File.Delete(Current_Doc.Doc_link);
                    Load_Doc();
                    MessageBox.Show("Delete successfully");
                }
                catch (Exception)
                {
                    MessageBox.Show("Delete successfully");
                }
            }
        }

        private void gvResult_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            Current_Doc = gvResult.GetRow(gvResult.FocusedRowHandle) as ADM_Document_Entity;
        }

        private void btnRefresh_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Load_Doc();
        }
    }
}
