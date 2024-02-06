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
    public partial class frmPDDashBoardPA : Form
    {
        public frmPDDashBoardPA()
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
            //---Line 11
            var result_11 = List_Plan.FirstOrDefault(x => x.Line_id == "11");
            if (result_11 == null)
            {
                lbpro11.Text = "";
                lb11.BackColor = Color.Purple;
            }
            else
            {
                lbpro11.Text = result_11.Customer_product_code;
                //lbpla11.Text = result_11.Target.ToString();
                DateTime begin_time11 = result_11.Start_time;
                DateTime finish_time11 = result_11.End_time;
                lbqty11.Text = adoClass.Load_Qty(begin_time11, finish_time11, lbpro11.Text);
                double std11 = (DateTime.Now - begin_time11).TotalMinutes * result_11.Target / (finish_time11 - begin_time11).TotalMinutes;
                lbpla11.Text = Math.Round(std11, 0).ToString();
                lbmps11.Text = Math.Round(double.Parse(lbqty11.Text) * 100 / std11, 0).ToString() + "%";
                if (result_11.Standard_time_FG.ToString() != "0")
                {
                    lbeff11.Text = (Math.Round((result_11.Standard_time_FG * double.Parse(lbqty11.Text)) * 100 / ((DateTime.Now - begin_time11).TotalSeconds * result_11.Number_operator), 0)).ToString() + "%";
                }
                else
                {
                    lbeff11.Text = "#NA";
                }
                lb11.BackColor = Color.Green;
                lbsta11.Text = adoClass.Load_status_of_Line(lb11.Text);
                if (lbsta11.Text == "")
                {
                    lb11.BackColor = Color.Green;
                }
                else
                {
                    lb11.BackColor = Color.Red;
                }
            }
            //---Line 12
            var result_12 = List_Plan.FirstOrDefault(x => x.Line_id == "12");
            if (result_12 == null)
            {
                lbpro12.Text = "";
                lb12.BackColor = Color.Purple;
            }
            else
            {
                lbpro12.Text = result_12.Customer_product_code;
                //lbpla12.Text = result_12.Target.ToString();
                DateTime begin_time12 = result_12.Start_time;
                DateTime finish_time12 = result_12.End_time;
                lbqty12.Text = adoClass.Load_Qty(begin_time12, finish_time12, lbpro12.Text);
                double std12 = (DateTime.Now - begin_time12).TotalMinutes * result_12.Target / (finish_time12 - begin_time12).TotalMinutes;
                lbpla12.Text = Math.Round(std12, 0).ToString();
                lbmps12.Text = Math.Round(double.Parse(lbqty12.Text) * 100 / std12, 0).ToString() + "%";
                if (result_12.Standard_time_FG.ToString() != "0")
                {
                    lbeff12.Text = (Math.Round((result_12.Standard_time_FG * double.Parse(lbqty12.Text)) * 100 / ((DateTime.Now - begin_time12).TotalSeconds * result_12.Number_operator), 0)).ToString() + "%";
                }
                else
                {
                    lbeff12.Text = "#NA";
                }
                lb12.BackColor = Color.Green;
                lbsta12.Text = adoClass.Load_status_of_Line(lb12.Text);
                if (lbsta12.Text == "")
                {
                    lb12.BackColor = Color.Green;
                }
                else
                {
                    lb12.BackColor = Color.Red;
                }
            }
            //---Line 13
            var result_13 = List_Plan.FirstOrDefault(x => x.Line_id == "13");
            if (result_13 == null)
            {
                lbpro13.Text = "";
                lb13.BackColor = Color.Purple;
            }
            else
            {
                lbpro13.Text = result_13.Customer_product_code;
                //lbpla13.Text = result_13.Target.ToString();
                DateTime begin_time13 = result_13.Start_time;
                DateTime finish_time13 = result_13.End_time;
                lbqty13.Text = adoClass.Load_Qty(begin_time13, finish_time13, lbpro13.Text);
                double std13 = (DateTime.Now - begin_time13).TotalMinutes * result_13.Target / (finish_time13 - begin_time13).TotalMinutes;
                lbpla13.Text = Math.Round(std13, 0).ToString();
                lbmps13.Text = Math.Round(double.Parse(lbqty13.Text) * 100 / std13, 0).ToString() + "%";
                if (result_13.Standard_time_FG.ToString() != "0")
                {
                    lbeff13.Text = (Math.Round((result_13.Standard_time_FG * double.Parse(lbqty13.Text)) * 100 / ((DateTime.Now - begin_time13).TotalSeconds * result_13.Number_operator), 0)).ToString() + "%";
                }
                else
                {
                    lbeff13.Text = "#NA";
                }
                lb13.BackColor = Color.Green;
                lbsta13.Text = adoClass.Load_status_of_Line(lb13.Text);
                if (lbsta13.Text == "")
                {
                    lb13.BackColor = Color.Green;
                }
                else
                {
                    lb13.BackColor = Color.Red;
                }
            }
            //---Line 14
            var result_14 = List_Plan.FirstOrDefault(x => x.Line_id == "14");
            if (result_14 == null)
            {
                lbpro14.Text = "";
                lb14.BackColor = Color.Purple;
            }
            else
            {
                lbpro14.Text = result_14.Customer_product_code;
                //lbpla14.Text = result_14.Target.ToString();
                DateTime begin_time14 = result_14.Start_time;
                DateTime finish_time14 = result_14.End_time;
                lbqty14.Text = adoClass.Load_Qty(begin_time14, finish_time14, lbpro14.Text);
                double std14 = (DateTime.Now - begin_time14).TotalMinutes * result_14.Target / (finish_time14 - begin_time14).TotalMinutes;
                lbpla14.Text = Math.Round(std14, 0).ToString();
                lbmps14.Text = Math.Round(double.Parse(lbqty14.Text) * 100 / std14, 0).ToString() + "%";
                if (result_14.Standard_time_FG.ToString() != "0")
                {
                    lbeff14.Text = (Math.Round((result_14.Standard_time_FG * double.Parse(lbqty14.Text)) * 100 / ((DateTime.Now - begin_time14).TotalSeconds * result_14.Number_operator), 0)).ToString() + "%";
                }
                else
                {
                    lbeff14.Text = "#NA";
                }
                lb14.BackColor = Color.Green;
                lbsta14.Text = adoClass.Load_status_of_Line(lb14.Text);
                if (lbsta14.Text == "")
                {
                    lb14.BackColor = Color.Green;
                }
                else
                {
                    lb14.BackColor = Color.Red;
                }
            }
            //---Line 15
            var result_15 = List_Plan.FirstOrDefault(x => x.Line_id == "15");
            if (result_15 == null)
            {
                lbpro15.Text = "";
                lb15.BackColor = Color.Purple;
            }
            else
            {
                lbpro15.Text = result_15.Customer_product_code;
                //lbpla15.Text = result_15.Target.ToString();
                DateTime begin_time15 = result_15.Start_time;
                DateTime finish_time15 = result_15.End_time;
                lbqty15.Text = adoClass.Load_Qty(begin_time15, finish_time15, lbpro15.Text);
                double std15 = (DateTime.Now - begin_time15).TotalMinutes * result_15.Target / (finish_time15 - begin_time15).TotalMinutes;
                lbpla15.Text = Math.Round(std15, 0).ToString();
                lbmps15.Text = Math.Round(double.Parse(lbqty15.Text) * 100 / std15, 0).ToString() + "%";
                if (result_15.Standard_time_FG.ToString() != "0")
                {
                    lbeff15.Text = (Math.Round((result_15.Standard_time_FG * double.Parse(lbqty15.Text)) * 100 / ((DateTime.Now - begin_time15).TotalSeconds * result_15.Number_operator), 0)).ToString() + "%";
                }
                else
                {
                    lbeff15.Text = "#NA";
                }
                lb15.BackColor = Color.Green;
                lbsta15.Text = adoClass.Load_status_of_Line(lb15.Text);
                if (lbsta15.Text == "")
                {
                    lb15.BackColor = Color.Green;
                }
                else
                {
                    lb15.BackColor = Color.Red;
                }
            }
            //---Line 16
            var result_16 = List_Plan.FirstOrDefault(x => x.Line_id == "16");
            if (result_16 == null)
            {
                lbpro16.Text = "";
                lb16.BackColor = Color.Purple;
            }
            else
            {
                lbpro16.Text = result_16.Customer_product_code;
                //lbpla16.Text = result_16.Target.ToString();
                DateTime begin_time16 = result_16.Start_time;
                DateTime finish_time16 = result_16.End_time;
                lbqty16.Text = adoClass.Load_Qty(begin_time16, finish_time16, lbpro16.Text);
                double std16 = (DateTime.Now - begin_time16).TotalMinutes * result_16.Target / (finish_time16 - begin_time16).TotalMinutes;
                lbpla16.Text = Math.Round(std16, 0).ToString();
                lbmps16.Text = Math.Round(double.Parse(lbqty16.Text) * 100 / std16, 0).ToString() + "%";
                if (result_16.Standard_time_FG.ToString() != "0")
                {
                    lbeff16.Text = (Math.Round((result_16.Standard_time_FG * double.Parse(lbqty16.Text)) * 100 / ((DateTime.Now - begin_time16).TotalSeconds * result_16.Number_operator), 0)).ToString() + "%";
                }
                else
                {
                    lbeff16.Text = "#NA";
                }
                lb16.BackColor = Color.Green;
                lbsta16.Text = adoClass.Load_status_of_Line(lb16.Text);
                if (lbsta16.Text == "")
                {
                    lb16.BackColor = Color.Green;
                }
                else
                {
                    lb16.BackColor = Color.Red;
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
            timer1.Enabled = true;
            prgCountTimer.Value = Convert.ToInt32((DateTime.Now - DateTime.Today.AddHours(6)).TotalMinutes / 8);
            Load_Other_Infor();
            Load_Line_Name();
            Load_Information();
            textBox10.Focus();
        }

        private void btnFinishingDashboard_Click(object sender, EventArgs e)
        {
            //timer1.Enabled = false;
            frmDashBoardFG frm = new frmDashBoardFG();
            frm.Show();
            this.Close();
        }
    }
}
