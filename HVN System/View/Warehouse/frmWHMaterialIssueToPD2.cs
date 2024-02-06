using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using HVN_System.Entity;
using System.IO.Ports;
using System.Threading;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraSplashScreen;
using HVN_System.View.Admin;

namespace HVN_System.View.Warehouse
{
    public partial class frmWHMaterialIssueToPD2 : Form
    {
        public frmWHMaterialIssueToPD2()
        {
            InitializeComponent();
        }
        public frmWHMaterialIssueToPD2(DateTime supply_date_, string shift_, string zone_, string _pic)
        {
            InitializeComponent();
            dtpSupplyDate.Value = supply_date_;
            txtShift.Text = shift_;
            txtZone.Text = zone_;
            txtPIC.Text = _pic;
        }
        private ADO adoClass;
        private CmCn conn;
        private List<W_M_IssueDocDetail_Entity> List_Data;
        private W_M_IssueDocDetail_Entity Current_item;
        float add_percent=1;
        private void gvInfo_RowClick(object sender, RowClickEventArgs e)
        {
            Current_item = gvInfo.GetRow(gvInfo.FocusedRowHandle) as W_M_IssueDocDetail_Entity;
            Load_Doc_Info_Detail(Current_item);
        }

        private void gvInfo_DoubleClick(object sender, EventArgs e)
        {
            Current_item = gvInfo.GetRow(gvInfo.FocusedRowHandle) as W_M_IssueDocDetail_Entity;
            btnStart.PerformClick();
        }
        private void Load_Doc_Info_Detail(W_M_IssueDocDetail_Entity item)
        {
            adoClass = new ADO();
            DataTable dt2 = adoClass.Load_W_M_IssueLabel("", "m_name=N'" + item.M_name + "' and product_customer_code=N'" + item.Product_customer_code + "' and p_line=N'" + item.P_line + "' and p_shift=N'" + item.P_shift + "' and issue_date=N'"+dtpSupplyDate.Value.ToString("yyyy-MM-dd")+"'");
            dgvDetail.DataSource = dt2;
        }
        private void Load_AddPercent()
        {
            try
            {
                conn = new CmCn();
                string strQry = "select [child_name] from [ADM_MasterListParameter] where [parent_id]=N'wh_add_percent' and [child_id]=N'1'";
                add_percent = float.Parse(conn.ExcuteString(strQry));
            }
            catch (Exception)
            {

            }
        }
        private void Load_data()
        {
            List_Data = new List<W_M_IssueDocDetail_Entity>();
            string strQry = "select plan_FG.*,actual.Actual_qty,M_Data.Check_std from  \n ";
            strQry += "   (select a.plan_date,a.[shift],a.line_no,c.line_area,b.m_name,  \n ";
            strQry += "   a.customer_product_code,round(b.m_quantity*a.[target]*"+add_percent+ ",0) as [m_demand],a.[target] as fg_qty   \n ";
            strQry += "   from PL_PlanFG a, P_MasterListProduct_BOM b, P_MasterListLine c \n ";
            strQry += "   where a.customer_product_code=b.product_customer_code   \n ";
            strQry += "   and a.issue_material is null  \n ";
            strQry += "   and a.plan_date=N'"+dtpSupplyDate.Value.ToString("yyyy-MM-dd")+"' and a.shift=N'"+txtShift.Text+"' \n ";
            strQry += "   and a.line_no=c.line_name and c.line_area=N'"+txtZone.Text+"' \n ";
            strQry += "   ) as plan_FG  \n ";
            strQry += "   left join  \n ";
            strQry += "   (select m_name,product_customer_code,sum(quantity) as Actual_qty,p_line   \n ";
            strQry += "   from W_M_IssueLabel   \n ";
            strQry += "   where supply_date=N'" + dtpSupplyDate.Value.ToString("yyyy-MM-dd") + "'   \n ";
            strQry += "   and p_shift=N'" + txtShift.Text + "' and m_doc_id =N''  \n ";
            strQry += "   group by product_customer_code,m_name,p_line) as actual  \n ";
            strQry += "   on plan_FG.m_name=actual.m_name and plan_FG.customer_product_code=actual.product_customer_code \n ";
            strQry += "   and plan_FG.line_no=actual.p_line \n ";
            strQry += "   left join  \n ";
            strQry += "     (select m_name,  \n ";
            strQry += "     case  \n ";
            strQry += "     when raw_qty>0 then 'OK'  \n ";
            strQry += "     else 'NOK'  \n ";
            strQry += "     end as Check_std  \n ";
            strQry += "     from W_MasterList_Material) as M_Data  \n ";
            strQry += "     on plan_FG.m_name=M_Data.m_name  \n ";
            strQry += "   \n ";


            conn = new CmCn();
            DataTable dt = conn.ExcuteDataTable(strQry);
            foreach (DataRow row in dt.Rows)
            {
                W_M_IssueDocDetail_Entity item = new W_M_IssueDocDetail_Entity();
                item.M_name = row["m_name"].ToString();
                item.Product_customer_code = row["customer_product_code"].ToString();
                item.M_demand = float.Parse(row["m_demand"].ToString());
                item.P_line = row["line_no"].ToString();
                item.P_shift = row["shift"].ToString();
                item.Check = row["Check_std"].ToString();
                item.Fg_qty= row["fg_qty"].ToString();
                if (row["Actual_qty"].ToString() == "")
                {
                    item.Actual_qty = 0;
                }
                else
                {
                    item.Actual_qty = float.Parse(row["Actual_qty"].ToString());
                }
                if (item.Actual_qty == item.M_demand)
                {
                    item.Status = "OK";
                }
                else if(item.Actual_qty<item.M_demand)
                {
                    item.Status = "Wait";
                }
                else
                {
                    item.Status = "Over";
                }
                List_Data.Add(item);
            }
            dgvInfo.DataSource = List_Data.ToList();
        }

        private void frmWHMaterialIssueToPD_Load(object sender, EventArgs e)
        {
            List_Data = new List<W_M_IssueDocDetail_Entity>();
            Current_item = new W_M_IssueDocDetail_Entity();
            Load_AddPercent();
            Load_data();
            cboScaleType.Text = "CAN NHO";
        }

        private void gvInfo_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            GridView view = sender as GridView;
            if (e.Column == view.Columns["Actual_qty"])
            {
                string status = view.GetRowCellValue(e.RowHandle, view.Columns["Status"]).ToString();
                switch (status)
                {
                    case "OK":
                        e.Appearance.BackColor = Color.Chartreuse;
                        break;
                    case "Wait":
                        e.Appearance.BackColor = Color.Orange;
                        break;
                    case "Over":
                        e.Appearance.BackColor = Color.Red;
                        break;
                    default:
                        break;
                }
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (cboScaleType.Text=="KHONG DUNG CAN")
            {
                frmWHMaterialIssueToPDDetail frm = new frmWHMaterialIssueToPDDetail(Current_item, dtpSupplyDate.Value, txtPIC.Text);
                frm.ShowDialog();
                Load_data();
            }
            else
            {
                frmWHMaterialIssueToPDDetail2 frm = new frmWHMaterialIssueToPDDetail2(Current_item, dtpSupplyDate.Value, cboScaleType.Text, txtPIC.Text);
                frm.ShowDialog();
                Load_data();
            }
            
            //if (Current_item.Status == "Wait")
            //{
            //    frmWHMaterialIssueToPDDetail2 frm = new frmWHMaterialIssueToPDDetail2(Current_item, supply_date);
            //    frm.ShowDialog();
            //    Load_data();
            //}
            //else
            //{
            //    MessageBox.Show("NGUYÊN VẬT LIỆU NÀY ĐÃ XUẤT ĐỦ \nTHIS MATERIAL HAS BEEN ISSUED COMPLETELY", "ERROR");
            //}
        }

        private void frmWHMaterialIssueToPD2_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            bool check = true;
            foreach (W_M_IssueDocDetail_Entity item in List_Data)
            {
                if (item.Status != "OK")
                {
                    check = false;
                }
            }
            if (check)
            {

            }
            else
            {
                MessageBox.Show("LỖI: CÓ NNGUYÊN VẬT LIỆU CÒN CHƯA CẤP");
            }

        }

        private void cboDocID_EditValueChanged(object sender, EventArgs e)
        {
            List_Data = new List<W_M_IssueDocDetail_Entity>();
            dgvDetail.DataSource = null;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            SaveFileDialog SaveDialog = new SaveFileDialog();
            SaveDialog.Filter = "Excel (.xlsx)|*.xlsx";
            if (SaveDialog.ShowDialog() != DialogResult.Cancel)
            {
                SplashScreenManager.ShowForm(this, typeof(frmWaitingForm), true, true, false);
                SplashScreenManager.Default.SetWaitFormCaption("Exporting...");
                string ExportFilePath = SaveDialog.FileName;
                adoClass = new ADO();
                adoClass.Print_W_M_SupplyDocument(List_Data, false, dtpSupplyDate.Value, txtShift.Text, ExportFilePath);
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

        private void btnExport_Click(object sender, EventArgs e)
        {
            adoClass = new ADO();
            adoClass.Export_Excel(dgvInfo);
        }

        private void btnPrintResult_Click(object sender, EventArgs e)
        {
            SaveFileDialog SaveDialog = new SaveFileDialog();
            SaveDialog.Filter = "Excel (.xlsx)|*.xlsx";
            if (SaveDialog.ShowDialog() != DialogResult.Cancel)
            {
                SplashScreenManager.ShowForm(this, typeof(frmWaitingForm), true, true, false);
                SplashScreenManager.Default.SetWaitFormCaption("Exporting...");
                string ExportFilePath = SaveDialog.FileName;
                adoClass = new ADO();
                adoClass.Print_W_M_SupplyDocument(List_Data, true, dtpSupplyDate.Value, txtShift.Text, ExportFilePath);
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
