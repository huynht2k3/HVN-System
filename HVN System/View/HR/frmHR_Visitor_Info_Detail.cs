using HVN_System.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HVN_System.View.HR
{
    public partial class frmHR_Visitor_Info_Detail : Form
    {
        public frmHR_Visitor_Info_Detail()
        {
            InitializeComponent();
        }
        public frmHR_Visitor_Info_Detail(string regID,bool _isAdmin)
        {
            InitializeComponent();
            isAdmin = _isAdmin;
            if (regID!="")
            {
                isEdit = true;
                txtRegID.Text = regID;
            }
        }
        private CmCn conn;
        bool isEdit = false,isAdmin=false;
        string user_commit;
        private void frmHR_Visitor_Info_Detail_Load(object sender, EventArgs e)
        {
            Load_combobox();
            if (isEdit)
            {
                Load_Reg_Info(txtRegID.Text);
                layoutControlItem3.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            }
            else
            {
                Load_RegID();
                txtRegistor.Text = General_Infor.myaccount.Name;
                txtDept.Text = General_Infor.myaccount.Department;
            }
        }
        private void Load_combobox()
        {
            string strQry = "select supplier_name from LOG_S_MasterlistSupplier";
            conn = new CmCn();
            DataTable dt = conn.ExcuteDataTable(strQry);
            txtVisitorCompany.Properties.DataSource = dt;
            txtVisitorCompany.Properties.DisplayMember = "supplier_name";
            txtVisitorCompany.Properties.ValueMember = "supplier_name";
        }
        private void Load_RegID()
        {
            string strQry = "select MAX(hr_reg_id) from HR_VR_VisitorInfor";
            conn = new CmCn();
            if (string.IsNullOrEmpty(conn.ExcuteString(strQry)))
            {
                txtRegID.Text = "HRVR10001";
            }
            else
            {
                int new_id = int.Parse(conn.ExcuteString(strQry).Substring(4, conn.ExcuteString(strQry).Length - 4)) + 1;
                txtRegID.Text = "HRVR" + new_id;
            }
        }
        private void Load_Reg_Info(string RegID)
        {
            string strQry = "select * from HR_VR_VisitorInfor where hr_reg_id=N'"+txtRegID.Text+"'";
            conn = new CmCn();
            DataTable dt = conn.ExcuteDataTable(strQry);
            if (dt.Rows.Count<1)
            {
                MessageBox.Show("Cannot find the information of the request " + txtRegID.Text);
                this.Close();
            }
            else
            {
                dtpFromDate.Value = DateTime.Parse(dt.Rows[0]["reg_date"].ToString());
                dtpTodate.Value = dtpFromDate.Value;
                txtDept.Text = dt.Rows[0]["department"].ToString();
                txtLicPlate.Text = dt.Rows[0]["lic_plate"].ToString();
                txtProtective.Text = dt.Rows[0]["protect"].ToString();
                txtRegistor.Text = dt.Rows[0]["registor"].ToString();
                cboRegLunch.Text = dt.Rows[0]["reg_lunch"].ToString();
                txtRegSpecial.Text = dt.Rows[0]["reg_special"].ToString();
                txtCarryItem.Text = dt.Rows[0]["carry_on_item"].ToString();
                txtIDNo.Text = dt.Rows[0]["id_no"].ToString();
                cboTraining.Text = dt.Rows[0]["training"].ToString();
                txtVisitor.Text = dt.Rows[0]["visitor"].ToString();
                txtVisitorCompany.Text= dt.Rows[0]["visitor_company"].ToString();
                cboPurpose.Text= dt.Rows[0]["purpose"].ToString();
                nmNumberVisitor.Value = int.Parse(dt.Rows[0]["number_visitor"].ToString());
                DateTime Timein = DateTime.Parse(dt.Rows[0]["time_in"].ToString());
                cboTimeIn.Text = Timein.ToString("HH:mm:ss");
                DateTime Timeout = DateTime.Parse(dt.Rows[0]["time_out"].ToString());
                cboTimeOut.Text= Timeout.ToString("HH:mm:ss");
                user_commit= dt.Rows[0]["user_commit"].ToString();
            }
        }

        private void btnSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            string strQry = "";
            if (isEdit)
            {
                strQry += "delete from HR_VR_VisitorInfor where hr_reg_id=N'" + txtRegID.Text + "' \n";
                strQry += "insert into HR_VR_VisitorInfor([hr_reg_id],[reg_date],[department],[registor],[visitor],[visitor_company],[id_no],[lic_plate]\n";
                strQry += ",[carry_on_item],[time_in],[time_out],[training],[protect],[reg_lunch],[reg_special],[last_user_commit],[last_time_commit],[user_commit],[is_active],number_visitor,purpose,[status])\n";
                strQry += "select N'" + txtRegID.Text + "',N'" + dtpFromDate.Value.ToString("yyyy-MM-dd") + "',N'" + txtDept.Text + "',N'" + txtRegistor.Text + "',N'" + txtVisitor.Text + "'" +
                        ",N'" + txtVisitorCompany.Text + "',N'" + txtIDNo.Text + "',N'" + txtLicPlate.Text + "',N'" + txtCarryItem.Text + "',N'" + dtpFromDate.Value.ToString("yyyy-MM-dd") + " " + cboTimeIn.Text + "'" +
                        ",N'" + dtpFromDate.Value.ToString("yyyy-MM-dd") + " " + cboTimeOut.Text + "',N'" + cboTraining.Text + "',N'" + txtProtective.Text + "',N'" + cboRegLunch.Text + "',N'" + txtRegSpecial.Text + "'" +
                        ",N'" + General_Infor.username + "',getdate(),N'" + user_commit + "',N'1',N'" + nmNumberVisitor.Value + "',N'" + cboPurpose.Text + "',N'Pending'";
            }
            else
            {
                user_commit = General_Infor.username;
                strQry += "insert into HR_VR_VisitorInfor([hr_reg_id],[reg_date],[department],[registor],[visitor],[visitor_company],[id_no],[lic_plate]\n";
                strQry += ",[carry_on_item],[time_in],[time_out],[training],[protect],[reg_lunch],[reg_special],[last_user_commit],[last_time_commit],[user_commit],[is_active],number_visitor,purpose,[status])\n";
                double numberday = (dtpTodate.Value - dtpFromDate.Value).TotalDays;
                string qry2 = "";
                string qry_new = "select MAX(hr_reg_id) from HR_VR_VisitorInfor";
                int new_id = int.Parse(conn.ExcuteString(qry_new).Substring(4, conn.ExcuteString(qry_new).Length - 4)) + 1;
                conn = new CmCn();
                for (int i = 0; i <= numberday; i++)
                {
                   
                    if (string.IsNullOrEmpty(qry2))
                    {
                        qry2 += "select N'" + "HRVR" + new_id + "',N'" + dtpFromDate.Value.AddDays(i).ToString("yyyy-MM-dd") + "',N'" + txtDept.Text + "',N'" + txtRegistor.Text + "',N'" + txtVisitor.Text + "'" +
                        ",N'" + txtVisitorCompany.Text + "',N'" + txtIDNo.Text + "',N'" + txtLicPlate.Text + "',N'" + txtCarryItem.Text + "',N'" + dtpFromDate.Value.AddDays(i).ToString("yyyy-MM-dd") + " " + cboTimeIn.Text + "'" +
                        ",N'" + dtpFromDate.Value.AddDays(i).ToString("yyyy-MM-dd") + " " + cboTimeOut.Text + "',N'" + cboTraining.Text + "',N'" + txtProtective.Text + "',N'" + cboRegLunch.Text + "',N'" + txtRegSpecial.Text + "'" +
                        ",N'" + General_Infor.username + "',getdate(),N'" + user_commit + "',N'1',N'" + nmNumberVisitor.Value + "',N'" + cboPurpose.Text + "',N'Pending'";
                    }
                    else
                    {
                        qry2 += "union all select N'" + "HRVR" + new_id + "',N'" + dtpFromDate.Value.AddDays(i).ToString("yyyy-MM-dd") + "',N'" + txtDept.Text + "',N'" + txtRegistor.Text + "',N'" + txtVisitor.Text + "'" +
                       ",N'" + txtVisitorCompany.Text + "',N'" + txtIDNo.Text + "',N'" + txtLicPlate.Text + "',N'" + txtCarryItem.Text + "',N'" + dtpFromDate.Value.AddDays(i).ToString("yyyy-MM-dd") + " " + cboTimeIn.Text + "'" +
                       ",N'" + dtpFromDate.Value.AddDays(i).ToString("yyyy-MM-dd") + " " + cboTimeOut.Text + "',N'" + cboTraining.Text + "',N'" + txtProtective.Text + "',N'" + cboRegLunch.Text + "',N'" + txtRegSpecial.Text + "'" +
                       ",N'" + General_Infor.username + "',getdate(),N'" + user_commit + "',N'1',N'" + nmNumberVisitor.Value + "',N'" + cboPurpose.Text + "',N'Pending'";
                    }
                    new_id++;
                }
                strQry += qry2;
            }
            try
            {
                if (txtVisitor.Text==""|| txtVisitorCompany.Text==""||cboTimeIn.Text==""||cboTimeOut.Text==""||cboTraining.Text==""||txtProtective.Text==""||cboRegLunch.Text==""||nmNumberVisitor.Value==0||dtpFromDate.Value<DateTime.Today.AddMonths(-1))
                {  
                    MessageBox.Show("Missing information.Please full fill all (*) row\nThiếu thông tin. Vui lòng điền đầy đủ các mục (*)","Error");
                }
                else
                {
                    if (isAdmin)
                    {
                        conn = new CmCn();
                        conn.ExcuteQry(strQry);
                        MessageBox.Show("Register successfully\nĐăng ký thành công");
                        this.Close();
                    }
                    else
                    {
                        if (DateTime.Now < DateTime.Today.AddHours(16))
                        {
                            conn = new CmCn();
                            conn.ExcuteQry(strQry);
                            MessageBox.Show("Register successfully\nĐăng ký thành công");
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Bạn không thể đăng ký sau 4h. Vui lòng liên hệ bộ phần Nhân sự \nYou cannot register after 4PM. Please contact HR for urgent case");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void txtRegSpecial_TextChanged(object sender, EventArgs e)
        {
            if (txtRegSpecial.Text.Length > 200)
            {
                MessageBox.Show("The number of charactor cannot more than 200\nSố lượng ký tự không được vượt quá 200", "Error");
            }
        }

        private void txtCarryItem_TextChanged(object sender, EventArgs e)
        {
            if (txtCarryItem.Text.Length > 200)
            {
                MessageBox.Show("The number of charactor cannot more than 200\nSố lượng ký tự không được vượt quá 200", "Error");
            }
        }

        private void dtpFromDate_ValueChanged(object sender, EventArgs e)
        {
            dtpFromDate.CustomFormat = "dd/MM/yyyy";
            dtpTodate.Value = dtpFromDate.Value;
        }

        private void cboPurpose_SelectionChangeCommitted(object sender, EventArgs e)
        {
            switch (cboPurpose.Text)
            {
                case "Survey/ Khảo sát":
                    cboTraining.Text = "Training safety at survey area/ Đào tạo an toàn tại khu vực khảo sát";
                    break;
                case "Install, setup machine/ Lắp đặt thi công máy móc thiết bị":
                    cboTraining.Text = "Work permit and planning form/ Có giấy phép và kế hoạch thi công";
                    break;
                case "Maintenance machine/ Bảo trì bảo dưỡng máy móc thiết bị":
                    cboTraining.Text = "Work permit and planning form/ Có giấy phép và kế hoạch thi công";
                    break;
                case "Check, warranty machine and equipment/ Kiểm tra, bảo hành máy móc thiết bị":
                    cboTraining.Text = "Work permit and planning form/ Có giấy phép và kế hoạch thi công";
                    break;
                case "Visit factory/ Tham quan nhà máy":
                    cboTraining.Text = "Training safety at visit area/ Đào tạo an toàn tại khu vực tham quan";
                    break;
                case "Delivery goods/ Vận chuyển hàng":
                    cboTraining.Text = "Trainning safety about delivery area/ Đào tạo an toàn về khu vực vận chuyển";
                    break;
                case "Kiểm toán/ thanh tra/ Auditing":
                    cboTraining.Text = "Basic training about safety/ Đào tạo cơ bản về an toàn";
                    break;
                case "Phỏng vấn/ interview":
                    cboTraining.Text = "Basic training about safety/ Đào tạo cơ bản về an toàn";
                    break;
                default:
                    break;
            }
        }

        private void dtpTodate_ValueChanged(object sender, EventArgs e)
        {
            dtpTodate.CustomFormat = "dd/MM/yyyy";
        }
    }
}
