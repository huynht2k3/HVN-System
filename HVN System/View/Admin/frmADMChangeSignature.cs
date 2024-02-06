using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HVN_System.Entity;
using HVN_System.Util;

namespace HVN_System.View.Admin
{
    public partial class frmADMChangeSignature : Form
    {
        public frmADMChangeSignature()
        {
            InitializeComponent();
        }
        string link="";
        private CmCn conn;
        private void btnOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog OpenFile = new OpenFileDialog();
            OpenFile.Title = "Open Image File";
            OpenFile.Filter = "JPG (.jpg)|*.jpg";
            if (OpenFile.ShowDialog() != DialogResult.Cancel)
            {
                link = OpenFile.FileName;
                ptSignature.Image = new Bitmap(@link);
                var imageSize = ptSignature.Image.Size;
                var fitSize = ptSignature.ClientSize;
                ptSignature.SizeMode = imageSize.Width > fitSize.Width || imageSize.Height > fitSize.Height ?
                    PictureBoxSizeMode.Zoom : PictureBoxSizeMode.CenterImage;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (link!="")
            {
                string des= @"\\172.16.180.20\20.Public\05.IT\05.HVN_TOOL\ATTACHMENT\AMDIN\Signature\" + General_Infor.username + ".jpg";
                if (File.Exists(des))
                {
                    File.Delete(des);
                }
                File.Copy(link, des);
                conn = new CmCn();
                string strQry = "Update Account set signature=N'" + des + "' where Username =N'" + General_Infor.username + "'";
                conn.ExcuteQry(strQry);
                MessageBox.Show("The signature has been changed!");
                this.Close();
            }
            else
            {
                MessageBox.Show("Please open the picture before save.");
            }
        }
    }
}
