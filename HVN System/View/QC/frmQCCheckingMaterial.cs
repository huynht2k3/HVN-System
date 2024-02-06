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

namespace HVN_System.View.QC
{
    public partial class frmQCCheckingMaterial : Form
    {
        public frmQCCheckingMaterial()
        {
            InitializeComponent();
        }
        private ADO adoClass;
        private CmCn conn;
        private void gvInfo_RowClick(object sender, RowClickEventArgs e)
        {
        }

        private void gvInfo_DoubleClick(object sender, EventArgs e)
        {

        }
        private void Load_Doc_Info_Detail(string PlanID)
        {
            if (PlanID!="")
            {
                string strQry = "select whmr_code,m_name,quantity,lot_no \n ";
                strQry += " from W_M_HistoryOfTransaction \n ";
                strQry += " where [transaction]=N'QC checking material' and plan_no=N'" + PlanID + "' \n ";
                conn = new CmCn();
                DataTable dt2 = conn.ExcuteDataTable(strQry);
                dgvDetail.DataSource = dt2;
            }
        }
        private void Load_Doc_Info(string PlanID)
        {
            string strQry = "select a.m_name,a.p_shift,a.quantity,isnull(b.quantity_ok,0)+isnull(c.quantity_ng,0) as Actual_qty,(a.quantity-isnull(b.quantity_ok,0)-isnull(c.quantity_ng,0)) as Remain_qty,b.quantity_ok,c.quantity_ng,  \n ";
            strQry += "   case  \n ";
            strQry += "       when a.quantity = isnull(b.quantity_ok,0)+isnull(c.quantity_ng,0) then 'ok'  \n ";
            strQry += "       else 'wait'  \n ";
            strQry += "       end as [Status]  \n ";
            strQry += "   from  \n ";
            strQry += "   (select * from [W_M_CheckingPlanDetail] where [rm_plan_id]=N'"+ PlanID + "') a  \n ";
            strQry += "   left join  \n ";
            strQry += "   (select m_name,sum(quantity) as quantity_ok \n ";
            strQry += "   from W_M_HistoryOfTransaction \n ";
            strQry += "   where [transaction]=N'QC checking material' and plan_no=N'" + PlanID + "' \n ";
            strQry += "   group by m_name) b  \n ";
            strQry += "   on a.m_name = b.m_name \n ";
            strQry += "   left join \n ";
            strQry += "   (select m_name,sum(quantity) as quantity_ng  \n ";
            strQry += "   from W_M_ReceiveLabel  \n ";
            strQry += "   where [rm_plan_id]=N'"+ PlanID + "' and qc_okng =N'NG' \n ";
            strQry += "   group by m_name) c \n ";
            strQry += "   on a.m_name=c.m_name \n ";
            conn = new CmCn();
            try
            {
                DataTable dt = conn.ExcuteDataTable(strQry);
                dgvInfo.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void frmWHMaterialIssueToPD_Load(object sender, EventArgs e)
        {
            dtpSupplyDate.Value = DateTime.Today;
            Load_PlanID();
        }
        private void Load_PlanID()
        {
            dgvInfo.DataSource = null;
            dgvDetail.DataSource = null;
            lbError.Text = "";
            string strQry = "SELECT * FROM W_M_CheckingPlan where check_date=N'" + dtpSupplyDate.Value.ToString("yyyy-MM-dd") + "'";
            conn = new CmCn();
            DataTable dt = conn.ExcuteDataTable(strQry);
            if (dt.Rows.Count>0)
            {
                txtPlanID.Text = dt.Rows[0]["rm_plan_id"].ToString();
                Load_Doc_Info(txtPlanID.Text);
                Load_Doc_Info_Detail(txtPlanID.Text);
            }
            else
            {
                lbError.Text = "KHÔNG TÌM THẤY KẾ HOẠCH CẤP HÀNG CHO QC NGÀY "+ dtpSupplyDate.Value.ToString("dd/MM/yyyy");
                txtPlanID.Text = "";
            }
        }
        private void gvInfo_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            GridView view = sender as GridView;
            if (e.RowHandle >= 0)
            {
                string status = view.GetRowCellValue(e.RowHandle, view.Columns["Status"]).ToString();
                if (status == "fail")
                {
                    e.Appearance.BackColor = Color.Red;
                }
                else if (status == "ok")
                {
                    e.Appearance.BackColor = Color.Chartreuse;
                }
                else if (status == "wait")
                {
                    e.Appearance.BackColor = Color.Yellow;
                }
            }
        }


        private void txtBarcode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                lbError.Text = "";
                string QR_Code = txtBarcode.Text.Substring(2, txtBarcode.Text.Length - 2);
                if (QR_Code.Substring(0,4)=="QCOP")
                {
                    txtPIC.Text = QR_Code.Substring(4, QR_Code.Length - 4);
                }
                else if (QR_Code.Substring(0, 4) == "WHMR")
                {
                    if (txtPIC.Text.Trim()=="")
                    {
                        MessageBox.Show("BẠN CẦN NHẬP TÊN TRƯỚC KHI KIỂM TRA LINH KIỆN");
                    }
                    else
                    {
                        InsertData(QR_Code);
                    }
                }
                txtBarcode.Text = "";
            }
        }

        private void InsertData(string label_code)
        {
            adoClass = new ADO();
            W_M_ReceiveLabel_Entity item = new W_M_ReceiveLabel_Entity();
            DataTable dt = adoClass.Load_W_M_ReceiveLabel("", "whmr_code=N'" + label_code + "'");
            if (dt.Rows[0]["place"].ToString() != "QC Area")
            {
                lbError.Text = label_code + ": THÙNG CHƯA ĐƯỢC BÀN GIAO CHO QC/ BOX HAS NOT BEEN TRANSFERED TO QC";
            }
            else if (dt.Rows[0]["qc_okng"].ToString() == "NG")
            {
                lbError.Text = label_code + ": THÙNG CHỨA HÀNG NG, KHÔNG CẦN KIỂM LẠI/ BOX CONTAIN NG PART ALREADY";
            }
            else
            {
                item.Whmr_code = label_code;
                item.M_name = dt.Rows[0]["m_name"].ToString();
                item.Quantity = string.IsNullOrEmpty(dt.Rows[0]["quantity"].ToString())?0:float.Parse(dt.Rows[0]["quantity"].ToString());
                item.Pic_qc = txtPIC.Text;
                item.Rm_plan_id= txtPlanID.Text;
                item.Created_date= string.IsNullOrEmpty(dt.Rows[0]["created_time"].ToString()) ? DateTime.Today : DateTime.Parse(dt.Rows[0]["created_time"].ToString());
                DateTime last_time_checking= string.IsNullOrEmpty(dt.Rows[0]["time_qc_check"].ToString()) ? DateTime.Today.AddDays(-1) : DateTime.Parse(dt.Rows[0]["time_qc_check"].ToString());
                if (last_time_checking>DateTime.Today)
                {
                    lbError.Text = "LỖI THÙNG ĐÃ ĐƯỢC KIỂM TRA.VUI LÒNG XEM MỤC CHI TIẾT";
                }
                else
                {
                    item.Time_qc_check = DateTime.Now;
                }
                item.Transaction = "Issue material to QC";
                item.Place = "QC Area";
            }
            if (lbError.Text=="")
            {
                frmQCCheckingMaterialDetail frm = new frmQCCheckingMaterialDetail(item);
                frm.ShowDialog();
                Load_Doc_Info(txtPlanID.Text);
                Load_Doc_Info_Detail(txtPlanID.Text);
            }
        }

        private void ckMultiOP_CheckedChanged(object sender, EventArgs e)
        {
        }
        private void dgvInfo_Click(object sender, EventArgs e)
        {

        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtPIC.Text = "";
        }

        private void dtpSupplyDate_ValueChanged(object sender, EventArgs e)
        {
            Load_PlanID();
        }
    }
}
