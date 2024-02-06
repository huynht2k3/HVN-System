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
using HVN_System.Util;
using HVN_System.Entity;
using System.IO;
using System.Diagnostics;
using HVN_System.View.Admin;

namespace HVN_System.View
{
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
        }
        private ADO adoClass;
        private CmCn conn;
        SqlDataAdapter da;
        DataTable dt;
        //1.0.0.2: fix report of checking result to see the quantity
        //1.0.0.3: add function import Master data
        //10.0.5: add function Scan barcode FG
        string Version = General_Infor.version;
        bool isSavePassword;
        private void btnOK_Click(object sender, EventArgs e)
        {
            General_Infor.username = txtUserName.Text.ToLower();
            adoClass = new ADO();
            General_Infor.KPI_month = adoClass.Current_month();
            General_Infor.KPI_month_name = adoClass.Current_month_name();
            General_Infor.KPI_year = DateTime.Today.AddDays(-1).ToString("yyyy");
            string pw = adoClass.Encrypt(txtPassword.Text);
            string strQry = "select * from Account where Username=N'" + txtUserName.Text + "' and Password =N'" + pw + "'";
            conn = new CmCn();
            DataTable dt = conn.ExcuteDataTable(strQry);
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["account_status"].ToString()=="Disable")
                {
                    MessageBox.Show("Your account has been disabled. Please contact administrator.");
                    this.Close();
                }
                else
                {
                    General_Infor.myaccount = new ADM_Account();
                    General_Infor.myaccount.Username = txtUserName.Text.ToLower();
                    General_Infor.myaccount.Direct_checker = dt.Rows[0]["direct_checker"].ToString();
                    General_Infor.myaccount.Direct_manager = dt.Rows[0]["Direct_manager"].ToString();
                    General_Infor.myaccount.Name = dt.Rows[0]["Name"].ToString();
                    General_Infor.myaccount.Signature = dt.Rows[0]["Signature"].ToString();
                    General_Infor.myaccount.Department = dt.Rows[0]["Department"].ToString();
                    General_Infor.myaccount.Position = dt.Rows[0]["Position"].ToString();
                    General_Infor.myaccount.Email_address = dt.Rows[0]["Email_address"].ToString();
                    General_Infor.myaccount.Po_approver = dt.Rows[0]["Po_approver"].ToString();
                    DateTime Expry_date = string.IsNullOrEmpty(dt.Rows[0]["expired_date"].ToString()) ? DateTime.Today.AddDays(1) : DateTime.Parse(dt.Rows[0]["expired_date"].ToString());
                    if ((DateTime.Today - Expry_date).TotalDays >= 0)
                    {
                        MessageBox.Show("Your password has been expried. Please change the password\nTài khoản của bạn đã hết hạn. Vui lòng thay đổi mật khẩu");
                        frmADMChangePassword frm = new frmADMChangePassword();
                        frm.Show();
                        this.Hide();
                    }
                    else if (dt.Rows[0]["device_name"].ToString() != System.Environment.MachineName)
                    {
                        if (dt.Rows[0]["device_name"].ToString() == "")
                        {
                            string[] line = File.ReadAllLines(@"C:\HVN_SYS_CONFIG\Config.txt", Encoding.UTF8);
                            if (isSavePassword)
                            {
                                line[0] = "Save password:Yes";
                                line[1] = "Username:" + txtUserName.Text;
                                line[2] = "Password:" + txtPassword.Text;
                            }
                            else
                            {
                                line[0] = "Save password:No";
                                line[1] = "Username:";
                                line[2] = "Password:";
                            }
                            File.WriteAllLines(@"C:\HVN_SYS_CONFIG\Config.txt", line);
                            frmMain frm = new frmMain();
                            frm.Show();
                            txtPassword.Text = "";
                            txtUserName.Text = "";
                            txtUserName.Focus();
                            this.Hide();
                        }
                        else
                        {
                            MessageBox.Show("Error: Could not log in account " + txtUserName.Text + " on this device\n\n Lỗi không thể đăng nhập tài khoản trên thiết bị này");
                            txtPassword.Text = "";
                            txtUserName.Text = "";
                            txtUserName.Focus();
                        }
                    }
                    else
                    {
                        string[] line = File.ReadAllLines(@"C:\HVN_SYS_CONFIG\Config.txt", Encoding.UTF8);
                        if (isSavePassword)
                        {
                            line[0] = "Save password:Yes";
                            line[1] = "Username:" + txtUserName.Text;
                            line[2] = "Password:" + txtPassword.Text;
                        }
                        else
                        {
                            line[0] = "Save password:No";
                            line[1] = "Username:";
                            line[2] = "Password:";
                        }
                        File.WriteAllLines(@"C:\HVN_SYS_CONFIG\Config.txt", line);
                        frmMain frm = new frmMain();
                        frm.Show();
                        txtPassword.Text = "";
                        txtUserName.Text = "";
                        txtUserName.Focus();
                        this.Hide();
                    }
                }
            }
            else
            {
                MessageBox.Show("Wrong user or password!", "Error");
                txtPassword.Text = "";
                txtUserName.Text = "";
                txtUserName.Focus();
            }
        }
        private void Insert_Log(string action)
        {
            adoClass = new ADO();
            ADM_LogActivities_Entity item = new ADM_LogActivities_Entity();
            item.User_name = General_Infor.username;
            item.Action = action;
            item.Computer_name = System.Environment.MachineName;
            adoClass.Insert_ADM_LogActivities(item);
        }
        private void ckIsOperator_CheckedChanged(object sender, EventArgs e)
        {
            if (ckIsSavePassword.Checked == true)
            {
                isSavePassword = true;
            }
            else
            {
                isSavePassword = false;
            }
        }
        private void Check_Version()
        {
            try
            {
                
                SqlCommand cmd = ConnectDatabase.Connect.CreateCommand();
                cmd.CommandText = "SELECT Version FROM About Where Application='HVN System'";
                da = new SqlDataAdapter(cmd);
                dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows[0][0].ToString() != Version)
                {
                    //MessageBox.Show("You are using old version. Please update the software.");
                    
                    System.Diagnostics.Process.Start(@"C:\HVN_SYS\Update.exe");
                    Environment.Exit(1);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Cannot connect to the server. Maybe update can solve problem");
                System.Diagnostics.Process.Start(@"C:\HVN_SYS\Update.exe");
                Environment.Exit(1);
            }
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            Check_Version();
            Load_Setting();
        }
        private void Load_Setting()
        {
            string[] line = File.ReadAllLines(@"C:\HVN_SYS\Config.txt", Encoding.UTF8);
            string[] line2 = File.ReadAllLines(@"C:\HVN_SYS_CONFIG\Config.txt", Encoding.UTF8);
            string IsSavePassword = line2[0].Substring(line[0].LastIndexOf(":") + 1);
            if (IsSavePassword == "Yes")
            {
                ckIsSavePassword.Checked = true;
                txtUserName.Text = line2[1].Substring(line[1].LastIndexOf(":") + 1);
                txtPassword.Text = line2[2].Substring(line[2].LastIndexOf(":") + 1);
                txtPassword.Focus();
            }
            else
            {
                ckIsSavePassword.Checked = false;
            }
        }
        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnOK.PerformClick();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            string strQry = "select a.vs_id,a.vs_requester,vs_status,a.current_pic, \n ";
            strQry += " a.vs_requester,a.vs_date,a.dept_mgr,a.dept_mgr_date,a.pur,a.pur_date, \n ";
            strQry += " a.plant_mgr_date,a.plant_mgr,a.pur_mgr_date,a.pur_mgr,a.fin_mgr_date,a.fin_mgr, \n ";
            strQry += " a.supplier_1,a.supplier_2,a.supplier_3,a.selected_supplier,a.supplier_1_type,a.supplier_2_type,a.supplier_3_type, \n ";
            strQry += " a.supplier_1_att,a.supplier_2_att,a.supplier_3_att,\n ";
            strQry += " b.Email_address as _dept_mgr,b.[signature] as _dept_sign, \n ";
            strQry += " c.Email_address as _pur,c.[signature] as _pur_sign, \n ";
            strQry += " d.Email_address as _fin_mgr,d.[signature] as _fin_mgr_sign, \n ";
            strQry += " e.Email_address as _pur_mgr,e.[signature] as _pur_mgr_sign, \n ";
            strQry += " f.Email_address as _plant_mgr,f.[signature] as _plant_mgr_sign, \n ";
            strQry += " g.Email_address as _vs_requester \n ";
            strQry += " from  \n ";
            strQry += " (select a.*  \n ";
            strQry += " from PUR_VS a,Account b \n ";
            strQry += " where a.vs_id=N'@@@@' \n ";
            strQry += " and a.current_pic=b.Username \n ";
            strQry += " and b.Email_address=N'@@@@') a \n ";
            strQry += " left join  \n ";
            strQry += " (select Email_address,Username,[signature] from Account)b \n ";
            strQry += " on a.dept_mgr=b.Username \n ";
            strQry += " left join  \n ";
            strQry += " (select Email_address,Username,[signature] from Account)c \n ";
            strQry += " on a.pur=c.Username \n ";
            strQry += " left join  \n ";
            strQry += " (select Email_address,Username,[signature] from Account)d \n ";
            strQry += " on a.fin_mgr=d.Username \n ";
            strQry += " left join  \n ";
            strQry += " (select Email_address,Username,[signature] from Account)e \n ";
            strQry += " on a.pur_mgr=e.Username \n ";
            strQry += " left join  \n ";
            strQry += " (select Email_address,Username,[signature] from Account)f \n ";
            strQry += " on a.plant_mgr=f.Username \n ";
            strQry += " left join  \n ";
            strQry += " (select Email_address,Username,[signature] from Account)g \n ";
            strQry += " on a.vs_requester=g.Username \n ";
            //MessageBox.Show(strQry);
            Environment.Exit(1);
        }

        private void frmLogin_FormClosing(object sender, FormClosingEventArgs e)
        {
            Environment.Exit(1);
        }
    }
}
