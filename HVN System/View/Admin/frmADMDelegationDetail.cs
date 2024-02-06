using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using HVN_System.Entity;
using System.Collections.ObjectModel;
using HVN_System.Util;

namespace HVN_System.View.Admin
{
    public partial class frmADMDelegationDetail : Form
    {
        public frmADMDelegationDetail()
        {
            InitializeComponent();
            request_type = "New";
        }
        public frmADMDelegationDetail(ADM_Delegation_Entity _request, string kind)
        {
            InitializeComponent();
            request = _request;
            request_type = kind;
        }
        string request_type;
        private List<ADM_DelegationDetail_Entity> List_Data;
        private ADM_Delegation_Entity request;
        private CmCn conn;
        private void repositoryItemCheckEdit1_CheckedChanged(object sender, EventArgs e)
        {
            if (gvResult.PostEditor())
            {
                gvResult.UpdateCurrentRow();
            }
        }
        private void Load_combobox()
        {
            string strQry = "Select Username from Account";
            conn = new CmCn();
            DataTable dt = conn.ExcuteDataTable(strQry);
            cboUser.Properties.DataSource = dt;
            cboUser.Properties.ValueMember = "Username";
            cboUser.Properties.DisplayMember = "Username";
        }
        private void dgvResult_Click(object sender, EventArgs e)
        {

        }
        private void Load_data()
        {
            string strQry = "";
            if (request_type == "New")
            {
                strQry += "select a.username,b.toolbox_des,b.toolbox_name,b.frm_name, N'' as isSelection \n ";
                strQry += " from ADM_ToolboxPermission a,ADM_ToolboxOfForm b \n ";
                strQry += " where username=N'" + General_Infor.username + "' \n ";
                strQry += " and a.frm_name=b.frm_name \n ";
                strQry += " and a.toolbox_name=b.toolbox_name \n ";
                strQry += " and b.is_delegate=N'True' \n ";
            }
            else
            {
                txtID.Text = request.Dl_id;
                dtpRequestDate.Value = request.Dl_time;
                dtpFrom.Value = request.Dl_fromdate;
                dtpTo.Value = request.Dl_todate;
                cboUser.Text = request.Delegated_pic;
                txtNote.Text= request.Dl_note;
                strQry += "select a.*,b.dl_id as isSelection from  \n ";
                strQry += " (select a.username,b.toolbox_des,b.toolbox_name,b.frm_name \n ";
                strQry += " from ADM_ToolboxPermission a,ADM_ToolboxOfForm b \n ";
                strQry += " where username=N'" + General_Infor.username + "' \n ";
                strQry += " and a.frm_name=b.frm_name \n ";
                strQry += " and a.toolbox_name=b.toolbox_name \n ";
                strQry += " and b.is_delegate=N'True') a \n ";
                strQry += " left join \n ";
                strQry += " (select * from ADM_DelegationDetail \n ";
                strQry += " where dl_id=N'" + txtID.Text + "') b \n ";
                strQry += " on a.toolbox_name=b.toolbox_name \n ";
                strQry += " and a.frm_name=b.frm_name \n ";

            }
            conn = new CmCn();
            DataTable dt = conn.ExcuteDataTable(strQry);
            List_Data = new List<ADM_DelegationDetail_Entity>();
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    ADM_DelegationDetail_Entity item = new ADM_DelegationDetail_Entity();
                    item.Dl_id = txtID.Text;
                    item.Toolbox_name = row["toolbox_name"].ToString();
                    item.Toolbox_des = row["toolbox_des"].ToString();
                    item.Frm_name = row["frm_name"].ToString();
                    item.IsSelect = string.IsNullOrEmpty(row["isSelection"].ToString()) ? false : true;
                    List_Data.Add(item);
                }
            }
            dgvResult.DataSource = List_Data.ToList();
        }
        private void frmADMDelegationDetail_Load(object sender, EventArgs e)
        {
            Load_combobox();
            Load_data();
        }
        private bool Check_error()
        {
            string error = "";
            if (dtpTo.Value <= dtpFrom.Value)
            {
                error += "To date must be > From date \n";
            }
            if (cboUser.Text=="")
            {
                error += "Missing delegated user \n";
            }
            if (error=="")
            {
                return true;
            }
            else
            {
                MessageBox.Show("Please check error below:\n"+error);
                return false;
            }
            
        }
        private void Load_request_no()
        {
            string strQry = "select max(RIGHT((dl_id),2)) from ADM_Delegation where cast(dl_time as date)=N'" + DateTime.Today.ToString("yyyy-MM-dd") + "'";
            conn = new CmCn();
            string number = conn.ExcuteString(strQry);
            if (number == "")
            {
                txtID.Text = "ADDL-" + DateTime.Today.ToString("yyMMdd") + "01";
            }
            else
            {
                int stt = int.Parse(number) + 1;
                if (stt<10)
                {
                    txtID.Text = "ADDL-" + DateTime.Today.ToString("yyMMdd") + "0" + stt.ToString();
                }
                else
                {
                    txtID.Text = "ADDL-" + DateTime.Today.ToString("yyMMdd") + stt.ToString();
                }
            }
        }
        private void btnSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (MessageBox.Show("Do you want to submit this request?", "SUBMIT", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                if (Check_error())
                {
                    if (request_type == "New")
                    {
                        Load_request_no();
                    }
                    string qry2 = "";
                    foreach (ADM_DelegationDetail_Entity item in List_Data)
                    {
                        if (item.IsSelect == true)
                        {
                            if (qry2 == "")
                            {
                                qry2 += " select N'" + txtID.Text + "',N'" + item.Toolbox_name + "',N'" + item.Frm_name + "' \n";
                            }
                            else
                            {
                                qry2 += " union all select N'" + txtID.Text + "',N'" + item.Toolbox_name + "',N'" + item.Frm_name + "' \n";
                            }
                        }
                    }
                    if (qry2 != "")
                    {
                        string strQry = "";
                        if (request_type != "New")
                        {
                            strQry += "delete from ADM_Delegation where dl_id=N'" + txtID.Text + "' \n";
                            strQry += "delete from ADM_DelegationDetail where dl_id=N'" + txtID.Text + "' \n";
                        }
                        strQry += "insert into ADM_Delegation (dl_id,dl_time,dl_requester";
                        strQry += ",dl_fromdate,dl_todate,delegated_pic,dl_note,is_active) \n";
                        strQry += "select N'" + txtID.Text + "',getdate(),N'" + General_Infor.username + "', \n";
                        strQry += "N'" + dtpFrom.Value.ToString("yyyy-MM-dd") + " 00:00:00',N'" + dtpTo.Value.ToString("yyyy-MM-dd") + " 23:59:00',N'" + cboUser.Text + "',N'" + txtNote.Text + "',N'1' \n";
                        strQry += "insert into ADM_DelegationDetail (dl_id,toolbox_name,frm_name) \n";
                        strQry += qry2;
                        conn = new CmCn();
                        try
                        {
                            conn.ExcuteQry(strQry);
                            MessageBox.Show("Request has been saved");
                            this.Close();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                }
            }
        }

        private void gvResult_DoubleClick(object sender, EventArgs e)
        {

        }
    }
}
