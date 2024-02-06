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

namespace HVN_System.View.Admin
{
    public partial class frmADMChangePassword : Form
    {
        public frmADMChangePassword()
        {
            InitializeComponent();
        }
        private CmCn conn;
        private ADO adoClass;
        private void btnOK_Click(object sender, EventArgs e)
        {
            if (Check_password())
            {
                if (txtPassword.Text == txtRePassword.Text)
                {
                    try
                    {
                        string qry2 = "select Username from ADM_Account_History where Username=N'" + General_Infor.username + "' and Password=N'"+ txtPassword.Text + "'";
                        conn = new CmCn();
                        if (conn.ExcuteDataTable(qry2).Rows.Count>0)
                        {
                            MessageBox.Show("Your password has been used in the history. Please type another password.\nMật khẩu đã được sử dụng trong lịch sử. Vui lòng chọn mật khẩu khác");
                            txtPassword.Text = "";
                            txtRePassword.Text = "";
                            txtPassword.Focus();
                        }
                        else
                        {
                            adoClass = new ADO();
                            string encrypt_pw = adoClass.Encrypt(txtPassword.Text);
                            string strQry = "Update Account set Password = N'" + encrypt_pw + "',expired_date=N'" + DateTime.Today.AddMonths(2).ToString("yyyy-MM-dd") + "'\nwhere Username=N'" + General_Infor.username + "'\n";
                            strQry += "insert into ADM_Account_History(Username,Password,time_update)\n";
                            strQry += "select N'"+General_Infor.username+ "',N'" + txtPassword.Text + "',getdate()\n";
                            conn = new CmCn();
                            conn.ExcuteQry(strQry);
                            MessageBox.Show("Password has been changed", "Notification");
                            frmMain frm = new frmMain();
                            frm.Show();
                            this.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error");
                        
                    }
                }
                else
                {
                    MessageBox.Show("Password are not same/ Lỗi mật khẩu nhập lại không giống nhau","Error");
                }
            }
            txtPassword.Text = "";
            txtRePassword.Text = "";
            txtPassword.Focus();
        }
        private bool Check_password()
        {
            string Error = "";
            string passwd = txtPassword.Text;
            if (passwd.Length < 12)
            {
                Error += "The number of charactor less than 12/ Lỗi số ký tự ít hơn 12 \n";
            }
            if (!passwd.Any(char.IsLower))
            {
                Error += "The password not include lower case charactor/ Lỗi không chứa ký tự thường \n";
            }
            if (!passwd.Any(char.IsUpper))
            {
                Error += "The password not include upper case charactor/ Lỗi không chưa ký chữ in hoa \n";
            }
            if (!passwd.Any(char.IsNumber))
            {
                Error += "The password not include number/ Lỗi không chưa số \n";
            }
            if (passwd.Contains(" "))
            {
                Error += "The password include space/ Lỗi mật khẩu chứa dấu cách\n";
            }
            string specialCh = @"%!@#$%^&*()?/>.<,:;'\|}]{[_~`+=-" + "\"";
            char[] specialChArray = specialCh.ToCharArray();
            bool is_contain = false;
            foreach (char ch in specialChArray)
            {
                if (passwd.Contains(ch))
                {
                    is_contain = true;
                }
            }
            if (!is_contain)
            {
                Error += "The password not include special charactor/ Lỗi không chứa ký tứ đặc biệt\n";
            }
            if (Error=="")
            {
                return true;
            }
            else
            {
                MessageBox.Show("YOU GOT SOME ERROR AS BELLOW/ BẠN ĐANG MẮC CÁC LỖI SAU: \n"+Error);
                return false;
            }
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            //string strQry = "select * from Account";
            //conn = new CmCn();
            //DataTable dt = conn.ExcuteDataTable(strQry);
            //string strQry2 = "";
            //foreach (DataRow item in dt.Rows)
            //{
            //    adoClass = new ADO();
            //    string encrypt_pw = adoClass.Encrypt(item["Password"].ToString());
            //    strQry2 += "update Account set pw=N'"+ encrypt_pw + "' where Username=N'"+item["Username"].ToString()+"'\n";
            //}
            //MessageBox.Show(strQry2);
            this.Close();
        }

        private void txtRePassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode==Keys.Enter)
            {
                btnOK.PerformClick();
            }
        }

        private void frmADMChangePassword_Load(object sender, EventArgs e)
        {

        }
    }
}
