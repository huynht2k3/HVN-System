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
using DevExpress.XtraEditors;
using HVN_System.Entity;
using System.Runtime.InteropServices;

namespace HVN_System.View.Production
{
    public partial class frmDashBoardFG2 : Form
    {
        public frmDashBoardFG2()
        {
            InitializeComponent();
        }
        private ADO adoClass;
        //string Shift_name;
        List<PL_PlanFG_Entity> List_Plan;
        private void Load_Line_Name()
        {
            adoClass = new ADO();
            //foreach (GroupBox item in this.Controls.OfType<GroupBox>())
            //{
            //    string line_id = item.Name.Substring(2, 2);
            //    item.Text = adoClass.Load_Line_name(line_id);
            //}
            List_Plan = new List<PL_PlanFG_Entity>();
            DataTable dt = adoClass.Load_Current_Plan();
            foreach (DataRow row in dt.Rows)
            {
                PL_PlanFG_Entity item = new PL_PlanFG_Entity();
                item.Plan_date = string.IsNullOrEmpty(row["plan_date"].ToString()) ? DateTime.Today : DateTime.Parse(row["plan_date"].ToString());
                item.Line_no = row["line_no"].ToString();
                item.Shift= row["shift"].ToString();
                item.Product_code = row["product_code"].ToString();
                item.Target = int.Parse(row["target"].ToString());
                item.Customer_product_code = row["customer_product_code"].ToString();
                item.Start_time = string.IsNullOrEmpty(row["start_time"].ToString()) ? DateTime.Today.AddHours(6) : DateTime.Parse(row["start_time"].ToString());
                item.End_time = string.IsNullOrEmpty(row["end_time"].ToString()) ? DateTime.Today.AddHours(6) : DateTime.Parse(row["end_time"].ToString());
                item.Line_id = row["line_id"].ToString();
                item.Standard_time_FG = double.Parse(row["standard_time_FG"].ToString());
                item.Number_operator = int.Parse(row["number_operator"].ToString());
                List_Plan.Add(item);
            }
        }
        private void Load_Information()
        {
            //adoClass = new ADO();
            //---Line 01
            var result_01 = List_Plan.FirstOrDefault(x => x.Line_id == "01");
            if (result_01 == null)
            {
                lbpro01.Text = "";
                lb01.BackColor = Color.Purple;
            }
            else
            {
                lbpro01.Text = result_01.Customer_product_code;
                lbtarget01.Text= result_01.Target.ToString();
                lbshift01.Text = result_01.Shift;
                //lbpla01.Text = result_01.Target.ToString();
                DateTime begin_time01 = result_01.Start_time;
                DateTime finish_time01 = result_01.End_time;
                lbqty01.Text = adoClass.Load_Qty(begin_time01, finish_time01, lbpro01.Text);

                double std01 = (DateTime.Now - begin_time01).TotalMinutes * result_01.Target / (finish_time01 - begin_time01).TotalMinutes;
                lbpla01.Text = Math.Round(std01, 0).ToString();
                lbmps01.Text = Math.Round(double.Parse(lbqty01.Text) * 100 / std01, 0).ToString() + "%";
                if (result_01.Standard_time_FG.ToString() != "0")
                {
                    lbeff01.Text = (Math.Round((result_01.Standard_time_FG * double.Parse(lbqty01.Text))*100 / ((DateTime.Now - begin_time01).TotalSeconds* result_01.Number_operator), 0)).ToString() + "%";
                }
                else
                {
                    lbeff01.Text = "#NA";
                }
                lb01.BackColor = Color.Green;
                lbsta01.Text = adoClass.Load_status_of_Line(lb01.Text);
                //lbdowntime01.Text=Load_downtime_of_Line("K1");
                if (lbsta01.Text == "")
                {
                    lb01.BackColor = Color.Green;
                }
                else
                {
                    lb01.BackColor = Color.Red;
                }
            }
            //---Line 02
            var result_02 = List_Plan.FirstOrDefault(x => x.Line_id == "02");
            if (result_02 == null)
            {
                lbpro02.Text = "";
                lb02.BackColor = Color.Purple;
            }
            else
            {
                lbpro02.Text = result_02.Customer_product_code;
                //lbpla02.Text = result_02.Target.ToString();
                lbtarget02.Text = result_02.Target.ToString();
                lbshift02.Text = result_02.Shift;
                DateTime begin_time02 = result_02.Start_time;
                DateTime finish_time02 = result_02.End_time;
                lbqty02.Text = adoClass.Load_Qty(begin_time02, finish_time02, lbpro02.Text);
                double std02 = (DateTime.Now - begin_time02).TotalMinutes * result_02.Target / (finish_time02 - begin_time02).TotalMinutes;
                lbpla02.Text = Math.Round(std02, 0).ToString();
                lbmps02.Text = Math.Round(double.Parse(lbqty02.Text) * 100 / std02, 0).ToString() + "%";
                if (result_02.Standard_time_FG.ToString() != "0")
                {
                    lbeff02.Text = (Math.Round((result_02.Standard_time_FG * double.Parse(lbqty02.Text)) * 100 / ((DateTime.Now - begin_time02).TotalSeconds * result_02.Number_operator), 0)).ToString() + "%";
                }
                else
                {
                    lbeff02.Text = "#NA";
                }
                lb02.BackColor = Color.Green;
                lbsta02.Text = adoClass.Load_status_of_Line(lb02.Text);
                if (lbsta02.Text == "")
                {
                    lb02.BackColor = Color.Green;
                }
                else
                {
                    lb02.BackColor = Color.Red;
                }
            }
            //---Line 03
            var result_03 = List_Plan.FirstOrDefault(x => x.Line_id == "03");
            if (result_03 == null)
            {
                lbpro03.Text = "";
                lb03.BackColor = Color.Purple;
            }
            else
            {
                lbpro03.Text = result_03.Customer_product_code;
                //lbpla03.Text = result_03.Target.ToString();
                lbtarget03.Text = result_03.Target.ToString();
                lbshift03.Text = result_03.Shift;
                DateTime begin_time03 = result_03.Start_time;
                DateTime finish_time03 = result_03.End_time;
                lbqty03.Text = adoClass.Load_Qty(begin_time03, finish_time03, lbpro03.Text);
                double std03 = (DateTime.Now - begin_time03).TotalMinutes * result_03.Target / (finish_time03 - begin_time03).TotalMinutes;
                lbpla03.Text = Math.Round(std03, 0).ToString();
                lbmps03.Text = Math.Round(double.Parse(lbqty03.Text) * 100 / std03, 0).ToString() + "%";
                if (result_03.Standard_time_FG.ToString() != "0")
                {
                    lbeff03.Text = (Math.Round((result_03.Standard_time_FG * double.Parse(lbqty03.Text)) * 100 / ((DateTime.Now - begin_time03).TotalSeconds * result_03.Number_operator), 0)).ToString() + "%";
                }
                else
                {
                    lbeff03.Text = "#NA";
                }
                lb03.BackColor = Color.Green;
                lbsta03.Text = adoClass.Load_status_of_Line(lb03.Text);
                if (lbsta03.Text == "")
                {
                    lb03.BackColor = Color.Green;
                }
                else
                {
                    lb03.BackColor = Color.Red;
                }
            }
            //---Line 04
            var result_04 = List_Plan.FirstOrDefault(x => x.Line_id == "04");
            if (result_04 == null)
            {
                lbpro04.Text = "";
                lb04.BackColor = Color.Purple;
            }
            else
            {
                lbpro04.Text = result_04.Customer_product_code;
                //lbpla04.Text = result_04.Target.ToString();
                lbtarget04.Text = result_04.Target.ToString();
                lbshift04.Text = result_04.Shift;
                DateTime begin_time04 = result_04.Start_time;
                DateTime finish_time04 = result_04.End_time;
                lbqty04.Text = adoClass.Load_Qty(begin_time04, finish_time04, lbpro04.Text);
                double std04 = (DateTime.Now - begin_time04).TotalMinutes * result_04.Target / (finish_time04 - begin_time04).TotalMinutes;
                lbpla04.Text = Math.Round(std04, 0).ToString();
                lbmps04.Text = Math.Round(double.Parse(lbqty04.Text) * 100 / std04, 0).ToString() + "%";
                if (result_04.Standard_time_FG.ToString() != "0")
                {
                    lbeff04.Text = (Math.Round((result_04.Standard_time_FG * double.Parse(lbqty04.Text)) / (DateTime.Now - begin_time04).TotalSeconds * 100, 0)).ToString() + "%";
                }
                else
                {
                    lbeff04.Text = "#NA";
                }
                lb04.BackColor = Color.Green;
                lbsta04.Text = adoClass.Load_status_of_Line(lb04.Text);
                if (lbsta04.Text == "")
                {
                    lb04.BackColor = Color.Green;
                }
                else
                {
                    lb04.BackColor = Color.Red;
                }
            }
            //---Line 05
            var result_05 = List_Plan.FirstOrDefault(x => x.Line_id == "05");
            if (result_05 == null)
            {
                lbpro05.Text = "";
                lb05.BackColor = Color.Purple;
            }
            else
            {
                lbpro05.Text = result_05.Customer_product_code;
                //lbpla05.Text = result_05.Target.ToString();
                lbtarget05.Text = result_05.Target.ToString();
                lbshift05.Text = result_05.Shift;
                DateTime begin_time05 = result_05.Start_time;
                DateTime finish_time05 = result_05.End_time;
                lbqty05.Text = adoClass.Load_Qty(begin_time05, finish_time05, lbpro05.Text);
                double std05 = (DateTime.Now - begin_time05).TotalMinutes * result_05.Target / (finish_time05 - begin_time05).TotalMinutes;
                lbpla05.Text = Math.Round(std05, 0).ToString();
                lbmps05.Text = Math.Round(double.Parse(lbqty05.Text) * 100 / std05, 0).ToString() + "%";
                if (result_05.Standard_time_FG.ToString() != "0")
                {
                    lbeff05.Text = (Math.Round((result_05.Standard_time_FG * double.Parse(lbqty05.Text)) * 100 / ((DateTime.Now - begin_time05).TotalSeconds * result_05.Number_operator), 0)).ToString() + "%";
                }
                else
                {
                    lbeff05.Text = "#NA";
                }
                lb05.BackColor = Color.Green;
                lbsta05.Text = adoClass.Load_status_of_Line(lb05.Text);
                if (lbsta05.Text == "")
                {
                    lb05.BackColor = Color.Green;
                }
                else
                {
                    lb05.BackColor = Color.Red;
                }
            }
            //---Line 06
            var result_06 = List_Plan.FirstOrDefault(x => x.Line_id == "06");
            if (result_06 == null)
            {
                lbpro06.Text = "";
                lb06.BackColor = Color.Purple;
            }
            else
            {
                lbpro06.Text = result_06.Customer_product_code;
                //lbpla06.Text = result_06.Target.ToString();
                lbtarget06.Text = result_06.Target.ToString();
                lbshift06.Text = result_06.Shift;
                DateTime begin_time06 = result_06.Start_time;
                DateTime finish_time06 = result_06.End_time;
                lbqty06.Text = adoClass.Load_Qty(begin_time06, finish_time06, lbpro06.Text);
                double std06 = (DateTime.Now - begin_time06).TotalMinutes * result_06.Target / (finish_time06 - begin_time06).TotalMinutes;
                lbpla06.Text = Math.Round(std06, 0).ToString();
                lbmps06.Text = Math.Round(double.Parse(lbqty06.Text) * 100 / std06, 0).ToString() + "%";
                if (result_06.Standard_time_FG.ToString() != "0")
                {
                    lbeff06.Text = (Math.Round((result_06.Standard_time_FG * double.Parse(lbqty06.Text)) * 100 / ((DateTime.Now - begin_time06).TotalSeconds * result_06.Number_operator), 0)).ToString() + "%";
                }
                else
                {
                    lbeff06.Text = "#NA";
                }
                lb06.BackColor = Color.Green;
                lbsta06.Text = adoClass.Load_status_of_Line(lb06.Text);
                if (lbsta06.Text == "")
                {
                    lb06.BackColor = Color.Green;
                }
                else
                {
                    lb06.BackColor = Color.Red;
                }
            }
            //---Line 07
            var result_07 = List_Plan.FirstOrDefault(x => x.Line_id == "07");
            if (result_07 == null)
            {
                lbpro07.Text = "";
                lb07.BackColor = Color.Purple;
            }
            else
            {
                lbpro07.Text = result_07.Customer_product_code;
                //lbpla07.Text = result_07.Target.ToString();
                lbtarget07.Text = result_07.Target.ToString();
                lbshift07.Text = result_07.Shift;
                DateTime begin_time07 = result_07.Start_time;
                DateTime finish_time07 = result_07.End_time;
                lbqty07.Text = adoClass.Load_Qty(begin_time07, finish_time07, lbpro07.Text);
                double std07 = (DateTime.Now - begin_time07).TotalMinutes * result_07.Target / (finish_time07 - begin_time07).TotalMinutes;
                lbpla07.Text = Math.Round(std07, 0).ToString();
                lbmps07.Text = Math.Round(double.Parse(lbqty07.Text) * 100 / std07, 0).ToString() + "%";
                if (result_07.Standard_time_FG.ToString() != "0")
                {
                    lbeff07.Text = (Math.Round((result_07.Standard_time_FG * double.Parse(lbqty07.Text)) * 100 / ((DateTime.Now - begin_time07).TotalSeconds * result_07.Number_operator), 0)).ToString() + "%";
                }
                else
                {
                    lbeff07.Text = "#NA";
                }
                lb07.BackColor = Color.Green;
                lbsta07.Text = adoClass.Load_status_of_Line(lb07.Text);
                if (lbsta07.Text == "")
                {
                    lb07.BackColor = Color.Green;
                }
                else
                {
                    lb07.BackColor = Color.Red;
                }
            }
            //---Line 08
            var result_08 = List_Plan.FirstOrDefault(x => x.Line_id == "08");
            if (result_08 == null)
            {
                lbpro08.Text = "";
                lb08.BackColor = Color.Purple;
            }
            else
            {
                lbpro08.Text = result_08.Customer_product_code;
                //lbpla08.Text = result_08.Target.ToString();
                lbtarget08.Text = result_08.Target.ToString();
                lbshift08.Text = result_08.Shift;
                DateTime begin_time08 = result_08.Start_time;
                DateTime finish_time08 = result_08.End_time;
                lbqty08.Text = adoClass.Load_Qty(begin_time08, finish_time08, lbpro08.Text);
                double std08 = (DateTime.Now - begin_time08).TotalMinutes * result_08.Target / (finish_time08 - begin_time08).TotalMinutes;
                lbpla08.Text = Math.Round(std08, 0).ToString();
                lbmps08.Text = Math.Round(double.Parse(lbqty08.Text) * 100 / std08, 0).ToString() + "%";
                if (result_08.Standard_time_FG.ToString() != "0")
                {
                    lbeff08.Text = (Math.Round((result_08.Standard_time_FG * double.Parse(lbqty08.Text)) * 100 / ((DateTime.Now - begin_time08).TotalSeconds * result_08.Number_operator), 0)).ToString() + "%";
                }
                else
                {
                    lbeff08.Text = "#NA";
                }
                lb08.BackColor = Color.Green;
                lbsta08.Text = adoClass.Load_status_of_Line(lb08.Text);
                if (lbsta08.Text == "")
                {
                    lb08.BackColor = Color.Green;
                }
                else
                {
                    lb08.BackColor = Color.Red;
                }
            }
            //---Line 09
            var result_09 = List_Plan.FirstOrDefault(x => x.Line_id == "09");
            if (result_09 == null)
            {
                lbpro09.Text = "";
                lb09.BackColor = Color.Purple;
            }
            else
            {
                lbpro09.Text = result_09.Customer_product_code;
                //lbpla09.Text = result_09.Target.ToString();
                lbtarget09.Text = result_09.Target.ToString();
                lbshift09.Text = result_09.Shift;
                DateTime begin_time09 = result_09.Start_time;
                DateTime finish_time09 = result_09.End_time;
                lbqty09.Text = adoClass.Load_Qty(begin_time09, finish_time09, lbpro09.Text);
                double std09 = (DateTime.Now - begin_time09).TotalMinutes * result_09.Target / (finish_time09 - begin_time09).TotalMinutes;
                lbpla09.Text = Math.Round(std09, 0).ToString();
                lbmps09.Text = Math.Round(double.Parse(lbqty09.Text) * 100 / std09, 0).ToString() + "%";
                if (result_09.Standard_time_FG.ToString() != "0")
                {
                    lbeff09.Text = (Math.Round((result_09.Standard_time_FG * double.Parse(lbqty09.Text)) * 100 / ((DateTime.Now - begin_time09).TotalSeconds * result_09.Number_operator), 0)).ToString() + "%";
                }
                else
                {
                    lbeff09.Text = "#NA";
                }
                lb09.BackColor = Color.Green;
                lbsta09.Text = adoClass.Load_status_of_Line(lb09.Text);
                if (lbsta09.Text == "")
                {
                    lb09.BackColor = Color.Green;
                }
                else
                {
                    lb09.BackColor = Color.Red;
                }
            }
            //lbpla01.Text = result_01.Target.ToString();
            //lbmps01.Text = "";
        }
        private void Load_Infor_Each_line(Label pro, Label qty, Label pla, Label mps, GroupBox Line_group_box)
        {

        }
        private void Load_Other_Infor()
        {
            txtTime.Text = DateTime.Now.ToString("hh:mm tt");
            txtDate.Text = DateTime.Now.ToString("dd-MMM");
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            prgCountTimer.Value = Convert.ToInt32((DateTime.Now - DateTime.Today.AddHours(6)).TotalMinutes / 8);
            Load_Other_Infor();
            Load_Information();
        }

        private void frmDashBoardFG_Load(object sender, EventArgs e)
        {
            //ModifyProgressBarColor.SetState(progressBar1, 2);
            //progressBarControl1.EditValue = 50;

            prgCountTimer.Value = Convert.ToInt32((DateTime.Now - DateTime.Today.AddHours(6)).TotalMinutes / 8);
            Load_Other_Infor();
            Load_Line_Name();
            Load_Information();
            textBox10.Focus();
        }

        private void btnPADashboard_Click(object sender, EventArgs e)
        {
            frmPDDashBoardPA frm = new frmPDDashBoardPA();
            frm.Show();
            this.Close();
        }
    }
    public static class ModifyProgressBarColor
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = false)]
        static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr w, IntPtr l);
        public static void SetState(this System.Windows.Forms.ProgressBar pBar, int state)
        {
            SendMessage(pBar.Handle, 1040, (IntPtr)state, IntPtr.Zero);
        }
    }
}
