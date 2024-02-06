using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HVN_System.View.Warehouse
{
    public partial class frmNotification : Form
    {
        public frmNotification()
        {
            InitializeComponent();
        }
        public frmNotification(string content,string notify_type, int time_display)
        {
            InitializeComponent();
            lbNotification.Text = content;
            second = time_display;
            if (notify_type=="notification")
            {
                lbNotification.BackColor = Color.Chartreuse;
                btnOK.Hide();
                btnCancel.Hide();
            }
            else
            {
                lbNotification.BackColor = Color.Red;
            }
            txtType.Focus();
        }
        public static bool isOK = true;
        private int second = 0,count =1;
        private void frmNotification_Load(object sender, EventArgs e)
        {
            txtType.Focus();
        }

        private void txtType_KeyDown(object sender, KeyEventArgs e)
        {
            btnCancel.PerformClick();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            txtType.Focus();
            if (count==second)
            {
                this.Close();
            }
            else
            {
                count++;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            isOK = false;
            this.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            isOK = true;
            this.Close();
        }
    }
}
