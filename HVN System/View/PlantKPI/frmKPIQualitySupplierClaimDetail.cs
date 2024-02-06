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

namespace HVN_System.View.PlantKPI
{
    public partial class frmKPIQualitySupplierClaimDetail : Form
    {
        public frmKPIQualitySupplierClaimDetail()
        {
            InitializeComponent();
        }
        public frmKPIQualitySupplierClaimDetail(string _Claim_ID)
        {
            InitializeComponent();
            isEdit = true;
            Claim_Id = _Claim_ID;
        }
        private ADO adoClass;
        private CmCn conn;
        bool isEdit = false;
        string Claim_Id;
        private void btnExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void btnAdd_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            string strQry = "";
            if (isEdit)
            {
                strQry += "delete from KPI_QC_SupplierClaim where claim_id=N'" + txtClaimID.Text + "'\n";
            }
            strQry += "insert into KPI_QC_SupplierClaim ([claim_id],[detect_date],[claim_date],[supplier],[part_name] \n ";
            strQry += " ,[part_no],[lot_no],[description],[ng_qty],[ppm],[admin_cost],[ng_material_cost],[ng_fg_cost] \n ";
            strQry += " ,[ng_sorting_cost],[ng_compensafe_cost],[ng_logistic_cost],[ng_scrap_cost],[ng_total_cost] \n ";
            strQry += " ,[debit_note_no],[debit_note_date],[sent_claim],[debit_accept_date],[payment_date],[payment_status],[reported_date]) \n ";
            strQry += " select N'" + txtClaimID.Text + "',N'" + dtpDetectDate.Value.ToString("yyyy-MM-dd") + "',N'" + dtpClaimDate.Value.ToString("yyyy-MM-dd") + "',N'" + txtSupplier.Text + "', \n ";
            strQry += " N'" + txtPartName.Text + "',N'" + txtPartNo.Text + "',N'" + dtpLotNo.Value.ToString("yyyy-MM-dd") + "',N'" + txtDes.Text + "',N'" + txtNG_Quantity.Text + "',N'" + txtPPM.Text + "' \n ";
            strQry += " ,N'" + txtNG_AdminCost.Text + "',N'" + txtNG_MaterialCost.Text + "',N'" + txtNG_FGCost.Text + "',N'" + txtNG_SortingCost.Text + "' \n ";
            strQry += " ,N'" + txtNG_ConpensafeCost.Text + "',N'" + txtNG_LogisticCost.Text + "',N'" + txtNG_ScrapCost.Text + "',N'" + txtNG_TotalCost.Text + "' \n ";
            strQry += " ,N'" + txtDebitNoteNo.Text + "',N'" + dtpDebitNoteSendDate.Value.ToString("yyyy-MM-dd") + "',N'" + txtSendClaim.Text + "',N'" + dtpDebitNoteAcceptDate.Value.ToString("yyyy-MM-dd") + "' \n ";
            strQry += " ,N'" + dtpPaymentDate.Value.ToString("yyyy-MM-dd") + "',N'" + txtPaymentStatus.Text + "',N'" + dtpReportReceiveDate.Value.ToString("yyyy-MM-dd")  + "'\n ";
             try
            {
                conn = new CmCn();
                conn.ExcuteQry(strQry);
                MessageBox.Show("Lưu thành công");
                this.Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show("Lỗi nhập liệu, vui lòng kiểm tra lại \nError:"+ex.Message);
            }
            
        }
        private void Load_ID_NewClaimID()
        {
            adoClass = new ADO();
            DataTable dt = adoClass.Load_KPI_QC_SupplierClaim("max([claim_id]) as ID", "");
            if (dt.Rows[0][0].ToString()=="")
            {
                txtClaimID.Text = "0";
            }
            else
            {
                int id = int.Parse(dt.Rows[0][0].ToString()) + 1;
                txtClaimID.Text = id.ToString();
            }
            txtSendClaim.Text = "Y";
        }
        private void frmKPIAddNewIncident_Load(object sender, EventArgs e)
        {
            if (isEdit == false)
            {
                Load_ID_NewClaimID();
            }
            else
            {
                string strQry = "select * from KPI_QC_SupplierClaim where claim_id=N'"+ Claim_Id + "'";
                conn = new CmCn();
                DataTable dt = conn.ExcuteDataTable(strQry);
                txtClaimID.Text = Claim_Id;
                txtDebitNoteNo.Text = dt.Rows[0]["debit_note_no"].ToString();
                txtDes.Text = dt.Rows[0]["description"].ToString();
                txtNG_AdminCost.Text = dt.Rows[0]["admin_cost"].ToString();
                txtNG_ConpensafeCost.Text = dt.Rows[0]["ng_compensafe_cost"].ToString();
                txtNG_FGCost.Text = dt.Rows[0]["ng_fg_cost"].ToString();
                txtNG_LogisticCost.Text = dt.Rows[0]["ng_logistic_cost"].ToString();
                txtNG_MaterialCost.Text = dt.Rows[0]["ng_material_cost"].ToString();
                txtNG_Quantity.Text = dt.Rows[0]["ng_qty"].ToString();
                txtNG_ScrapCost.Text = dt.Rows[0]["ng_scrap_cost"].ToString();
                txtNG_SortingCost.Text = dt.Rows[0]["ng_sorting_cost"].ToString();
                txtNG_TotalCost.Text = dt.Rows[0]["ng_total_cost"].ToString();
                txtPartName.Text = dt.Rows[0]["part_name"].ToString();
                txtPartNo.Text = dt.Rows[0]["part_no"].ToString();
                txtPaymentStatus.Text = dt.Rows[0]["payment_status"].ToString();
                txtPPM.Text = dt.Rows[0]["ppm"].ToString();
                txtSendClaim.Text = dt.Rows[0]["sent_claim"].ToString();
                txtSupplier.Text = dt.Rows[0]["supplier"].ToString();
                dtpClaimDate.Value = string.IsNullOrEmpty(dt.Rows[0]["claim_date"].ToString()) ? DateTime.Today : DateTime.Parse(dt.Rows[0]["claim_date"].ToString());
                dtpDebitNoteAcceptDate.Value = string.IsNullOrEmpty(dt.Rows[0]["debit_accept_date"].ToString()) ? DateTime.Today : DateTime.Parse(dt.Rows[0]["debit_accept_date"].ToString());
                dtpDebitNoteSendDate.Value = string.IsNullOrEmpty(dt.Rows[0]["debit_note_date"].ToString()) ? DateTime.Today : DateTime.Parse(dt.Rows[0]["debit_note_date"].ToString());
                dtpDetectDate.Value = string.IsNullOrEmpty(dt.Rows[0]["detect_date"].ToString()) ? DateTime.Today : DateTime.Parse(dt.Rows[0]["detect_date"].ToString());
                dtpLotNo.Value = string.IsNullOrEmpty(dt.Rows[0]["lot_no"].ToString()) ? DateTime.Today : DateTime.Parse(dt.Rows[0]["lot_no"].ToString());
                dtpPaymentDate.Value = string.IsNullOrEmpty(dt.Rows[0]["payment_date"].ToString()) ? DateTime.Today : DateTime.Parse(dt.Rows[0]["payment_date"].ToString());
                dtpReportReceiveDate.Value = string.IsNullOrEmpty(dt.Rows[0]["reported_date"].ToString()) ? DateTime.Today : DateTime.Parse(dt.Rows[0]["reported_date"].ToString());
                //dtpClaimDate.Value = string.IsNullOrEmpty(dt.Rows[0][""].ToString()) ? DateTime.Today : DateTime.Parse(dt.Rows[0][""].ToString());
            }
            //layoutControl1.Controls.Remove(cbo8D);
           
        }

        private void txtInc_location_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSave.PerformClick();
            }
        }

        private void cboIncidentType_SelectionChangeCommitted(object sender, EventArgs e)
        {
        }

        private void cboInc_theme_SelectionChangeCommitted(object sender, EventArgs e)
        {
        }

        private void txtSortingTime_TextChanged(object sender, EventArgs e)
        {
            //try
            //{
            //    float test = float.Parse(txtSortingTime.Text);
            //}
            //catch (Exception)
            //{
            //    txtSortingTime.Text = "";
            //    MessageBox.Show("Cannot convert sorting time to number");
            //}
        }
    }
}
