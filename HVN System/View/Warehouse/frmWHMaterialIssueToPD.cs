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
    public partial class frmWHMaterialIssueToPD : Form
    {
        public frmWHMaterialIssueToPD()
        {
            InitializeComponent();
        }
        private ADO adoClass;
        private CmCn conn;
        private List<W_M_IssueDocDetail_Entity> List_Data;
        private W_M_IssueDocDetail_Entity Current_item;
        DateTime supply_date = DateTime.Today;
        private void gvInfo_RowClick(object sender, RowClickEventArgs e)
        {
            Current_item = gvInfo.GetRow(gvInfo.FocusedRowHandle) as W_M_IssueDocDetail_Entity;
        }

        private void gvInfo_DoubleClick(object sender, EventArgs e)
        {
            btnStart.PerformClick();
        }
        private void Load_Doc_Info_Detail()
        {
            adoClass = new ADO();
            DataTable dt2 = adoClass.Load_W_M_IssueLabel("", "supply_date=N'" + dtpSupplyDate.Value.ToString("yyyy-MM-dd")+ "' and m_doc_id not in ('')");
            dgvDetail.DataSource = dt2;
        }
        private void Load_Doc_Info()
        {
            List_Data = new List<W_M_IssueDocDetail_Entity>();
            string strQry = "select a.*,b.Actual_qty  \n ";
            strQry += "   from   \n ";
            strQry += "   (select doc_dt.*  \n ";
            strQry += "   from W_M_IssueDoc doc,W_M_IssueDocDetail doc_dt \n ";
            strQry += "   where doc.m_doc_supply_date=N'" + dtpSupplyDate.Value.ToString("yyyy-MM-dd") + "' \n ";
            strQry += "   and doc.m_doc_id=doc_dt.m_doc_id and [m_demand]>0) as a  \n ";
            strQry += "   left join   \n ";
            strQry += "   ( select m_name,product_customer_code,p_line,m_doc_id, SUM(quantity) as Actual_qty  \n ";
            strQry += "   from W_M_IssueLabel where supply_date=N'"+ dtpSupplyDate.Value.ToString("yyyy-MM-dd") + "' and m_doc_id not in ('')  \n ";
            strQry += "   group by m_name,product_customer_code,p_line,m_doc_id  \n ";
            strQry += "   )as b  \n ";
            strQry += "   on a.m_name=b.m_name   \n ";
            strQry += "   and a.product_customer_code = b.product_customer_code  \n ";
            strQry += "   and a.p_line=b.p_line  \n ";
            strQry += "   and a.m_doc_id=b.m_doc_id  \n ";
            strQry += "   \n ";

            conn = new CmCn();
            DataTable dt = new DataTable();
            try
            {
                dt = conn.ExcuteDataTable(strQry);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            foreach (DataRow row in dt.Rows)
            {
                W_M_IssueDocDetail_Entity item = new W_M_IssueDocDetail_Entity();
                item.M_doc_id = row["m_doc_id"].ToString();
                item.M_name = row["m_name"].ToString();
                item.P_shift = row["p_shift"].ToString();
                item.Product_customer_code = row["product_customer_code"].ToString();
                item.M_demand = float.Parse(row["m_demand"].ToString());
                item.P_line = row["p_line"].ToString();
                if (row["Actual_qty"].ToString()=="")
                {
                    item.Actual_qty = 0;
                }
                else
                {
                    item.Actual_qty = float.Parse(row["Actual_qty"].ToString());
                }
                if (item.Actual_qty==item.M_demand)
                {
                    item.Status = "OK";
                }
                else
                {
                    item.Status = "Not OK";
                }
                List_Data.Add(item);
            }
            dgvInfo.DataSource = List_Data.ToList();
        }
        
        private void frmWHMaterialIssueToPD_Load(object sender, EventArgs e)
        {
            List_Data = new List<W_M_IssueDocDetail_Entity>();
            Current_item = new W_M_IssueDocDetail_Entity();
            cboScaleType.Text = "CAN NHO";
            Load_Doc_Info();
            Load_Doc_Info_Detail();
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
                    case "Not OK":
                        e.Appearance.BackColor = Color.Orange;
                        break;
                    default:
                        break;
                }
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (Current_item.M_doc_id!="")
            {
                string strQry = "select [raw_qty] from [W_MasterList_Material] where m_name=N'" + Current_item.M_name + "'";
                conn = new CmCn();
                string raw_qty = conn.ExcuteString(strQry);
                if (raw_qty==""||raw_qty=="0")
                {
                    MessageBox.Show("NGUYÊN VẬT LIỆU NÀY CHƯA CÓ THÔNG TIN CÂN NẶNG TIÊU CHUẨN \nTHIS MATERIAL HAS NOT STANDARD WEIGHT YET", "ERROR");
                }
                else
                {
                    if (Current_item.Status == "Not OK")
                    {
                        if (txtPIC.Text.Trim()!="")
                        {
                            if (cboScaleType.Text == "KHONG DUNG CAN")
                            {
                                frmWHMaterialIssueToPDDetail frm = new frmWHMaterialIssueToPDDetail(Current_item, dtpSupplyDate.Value, txtPIC.Text);
                                frm.ShowDialog();
                                Load_Doc_Info();
                                Load_Doc_Info_Detail();
                            }
                            else
                            {
                                frmWHMaterialIssueToPDDetail2 frm = new frmWHMaterialIssueToPDDetail2(Current_item, dtpSupplyDate.Value, cboScaleType.Text, txtPIC.Text);
                                frm.ShowDialog();
                                Load_Doc_Info();
                                Load_Doc_Info_Detail();
                            }
                        }
                        else
                        {
                            MessageBox.Show("LỖI CHƯA NHẬP TÊN NGƯỜI THỰC HIỆN", "ERROR");
                        }
                    }
                    else
                    {
                        MessageBox.Show("NGUYÊN VẬT LIỆU NÀY ĐÃ XUẤT ĐỦ \nTHIS MATERIAL HAS BEEN ISSUED COMPLETELY", "ERROR");
                    }
                }
            }
        }

        private void dtpSupplyDate_ValueChanged(object sender, EventArgs e)
        {
            Load_Doc_Info();
            Load_Doc_Info_Detail();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            Load_Doc_Info();
            Load_Doc_Info_Detail();
        }
    }
}
