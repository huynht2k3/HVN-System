﻿using System;
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

namespace HVN_System.View.Warehouse
{
    public partial class frmWHRubberIssueToQuality : Form
    {
        public frmWHRubberIssueToQuality()
        {
            InitializeComponent();
        }
        //private ADO adoClass;
        private CmCn conn;
        private DataTable dt,dt_detail;
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
                string strQry = "select * from TEMP_W_M_IssueToQC where [rm_plan_id]=N'"+PlanID+"' and qc_shift='"+cboShift.Text+"'";
                conn = new CmCn();
                dt_detail = conn.ExcuteDataTable(strQry);
                dgvDetail.DataSource = dt_detail;
            }
        }
        private void Load_Doc_Info(string Doc_id)
        {
            string strQry = "select a.*,b.Actual_qty,b.act_qty_box, \n ";
            strQry += " case \n ";
            strQry += "     when a.quantity = b.Actual_qty then 'ok' \n ";
            strQry += "     else 'wait' \n ";
            strQry += "     end as [Status] \n ";
            strQry += " from \n ";
            strQry += " (select * from [W_M_CheckingPlanDetail] where [rm_plan_id]=N'"+Doc_id+ "' and p_shift=N'"+cboShift.Text+"') a \n ";
            strQry += " left join \n ";
            strQry += " (select m_name,SUM(quantity) as Actual_qty,isnull(COUNT(m_name),0) as act_qty_box from TEMP_W_M_IssueToQC where [rm_plan_id]=N'" + Doc_id + "' and qc_shift='" + cboShift.Text + "' \n ";
            strQry += " group by m_name) b \n ";
            strQry += " on a.m_name = b.m_name \n ";

            conn = new CmCn();
            try
            {
                dt = conn.ExcuteDataTable(strQry);
                dgvInfo.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void frmWHMaterialIssueToPD_Load(object sender, EventArgs e)
        {
            dt = new DataTable();
            dt_detail = new DataTable();
            lbDetailLabel.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            lbDetailTable.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            dtpSupplyDate.Value = DateTime.Today;
        }
        private void gvInfo_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            GridView view = sender as GridView;
            if (e.RowHandle >= 0)
            {
                string status = view.GetRowCellValue(e.RowHandle, view.Columns["Status"]).ToString();
                switch (status)
                {
                    case "ok":
                        e.Appearance.BackColor = Color.Chartreuse;
                        break;
                    case "wait":
                        e.Appearance.BackColor = Color.Yellow;
                        break;
                    default:
                        break;
                }
            }
        }

        //private void btnConfirm_Click(object sender, EventArgs e)
        //{
        //    bool check = true;
        //    if (dt_detail.Rows.Count > 1)
        //    {
        //        foreach (DataRow item in dt_detail.Rows)
        //        {
        //            if (item["is_check"].ToString() == "False")
        //            {
        //                check = false;
        //            }
        //        }
        //        //foreach (DataRow item in dt.Rows)
        //        //{
        //        //    if (item["Status"].ToString() == "wait")
        //        //    {
        //        //        check = false;
        //        //    }
        //        //}
        //    }
        //    if (check)
        //    {
        //        string strQry = "";
        //        string qry2 = "";
        //        foreach (DataRow item in dt_detail.Rows)
        //        {
        //            string whmr_code, pic_issue_qc, rm_plan_id, place, m_name, quantity;
        //            whmr_code = item["whmr_code"].ToString();
        //            quantity = item["quantity"].ToString();
        //            m_name = item["m_name"].ToString();
        //            pic_issue_qc = item["wh_op"].ToString();
        //            rm_plan_id = item["rm_plan_id"].ToString();
        //            place = "QC Area";
        //            strQry += "update W_M_ReceiveLabel set pic_issue_qc=N'" + pic_issue_qc + "',rm_plan_id=N'" + rm_plan_id + "',time_issue_qc=getdate(),place=N'" + place + "' where whmr_code=N'" + whmr_code + "' \n";
        //            if (string.IsNullOrEmpty(qry2))
        //            {
        //                qry2 += "select N'" + whmr_code + "',N'" + m_name + "',N'" + quantity + "',N'WH transfer to QC',getdate(),N'" + pic_issue_qc + "',N'" + place + "'\n";
        //            }
        //            else
        //            {
        //                qry2 += "union all select N'" + whmr_code + "',N'" + m_name + "',N'" + quantity + "',N'WH transfer to QC',getdate(),N'" + pic_issue_qc + "',N'" + place + "'\n";
        //            }
        //        }
        //        strQry += "insert into W_M_HistoryOfTransaction([whmr_code],[m_name],[quantity],[transaction],[input_time],[PIC],[place]) \n";
        //        strQry += qry2;
        //        try
        //        {
        //            conn = new CmCn();
        //            conn.ExcuteQry(strQry);
        //        }
        //        catch (Exception ex)
        //        {
        //            lbError.Text = ex.Message;
        //        }
        //        frmNotification frm = new frmNotification("XÁC NHẬN THÀNH CÔNG \nCONFIRM SUCCESSFULLY", "notification", 5);
        //        frm.ShowDialog();
        //    }
        //    else
        //    {
        //        lbError.Text = "LỖI THIẾU HÀNG HOẶC SAI HÀNG/ ISSUE MISSING BOX OR WRONG BOX";
        //    }

        //}

        private void txtBarcode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode==Keys.Enter)
            {
                lbError.Text = "";
                string QR_Code = txtBarcode.Text.Substring(2, txtBarcode.Text.Length - 2);
                if (QR_Code == "CLEAR")
                {
                    btnClear.PerformClick();
                }
                else if (txtBarcode.Text.Length <= 6)
                {
                    lbError.Text = "TEM LỖI /WRONG LABEL";
                }
                else
                {
                    if (txtBarcode.Text.Substring(2, 4) == "WHOP")
                    {
                        txtPIC.Text = txtBarcode.Text.Substring(6, txtBarcode.Text.Length - 6);
                    }
                    else if(txtBarcode.Text.Substring(2, 4) == "WHMR")
                    {
                        if (ckRemoveBox.Checked)
                        {
                            RemoveData(QR_Code);
                        }
                        else
                        {
                            InsertData(QR_Code);
                        }
                    }
                }
                txtBarcode.Text = "";
                txtBarcode.Focus();
            }
            
        }

        //private void InsertData(string label_code)
        //{
        //    adoClass = new ADO();
        //    DataTable dt2 = adoClass.Load_TEMP_W_M_IssueToQC("whmr_code", "whmr_code=N'" + label_code + "'");
        //    W_M_ReceiveLabel_Entity item = new W_M_ReceiveLabel_Entity();
        //    if (dt2.Rows.Count > 0)
        //    {
        //        lbError.Text = label_code + ": TEM ĐÃ ĐƯỢC THÊM VÀO DANH SÁCH CHỜ/ LABEL HAS BEEN ALREADY ADDED IN LIST";
        //    }
        //    else
        //    {
        //        DataTable dt = adoClass.Load_W_M_ReceiveLabel("", "whmr_code=N'" + label_code + "'");
        //        if (dt.Rows[0]["place"].ToString() != "WH Material")
        //        {
        //            lbError.Text = label_code + ": LỖI THÙNG HÀNG KHÔNG CÓ TRONG KHO/ ERROR: THIS BOX IS NOT IN WAREHOUSE";
        //        }
        //        else
        //        {
        //            item.Whmr_code = label_code;
        //            item.M_name = dt.Rows[0]["m_name"].ToString();
        //            item.Quantity= string.IsNullOrEmpty(dt.Rows[0]["quantity"].ToString()) ? 0 : float.Parse(dt.Rows[0]["quantity"].ToString());
        //            item.Rm_plan_id = txtPlanID.Text;
        //            item.Qc_shift=cboShift.Text;
        //            //Check over qty
        //            //if (!Check_Over_qty(item.Rm_plan_id, item.Quantity))
        //            //{
        //            //    lbError.Text = label_code + ": LỖI " + item.M_name + " VƯỢT QUÁ SỐ LƯỢNG/ ERROR: OVER QTY";
        //            //}
        //            //--------------
        //            item.Pic_issue_qc= txtPIC.Text;
        //            item.Time_issue_qc = DateTime.Now;
        //            item.Transaction = "Issue material to QC";
        //            item.Place = "QC Area";
        //            if (lbError.Text=="")
        //            {
        //                item.Is_check = "True";
        //            }
        //            else
        //            {
        //                item.Is_check = "False";
        //            }
        //        }
        //    }
        //    if (lbError.Text=="")
        //    {
        //        adoClass = new ADO();
        //        adoClass.Insert_TEMP_W_M_IssueToQC(item);
        //        string strQry = "update W_M_ReceiveLabel set pic_issue_qc=N'" + item.Pic_issue_qc + "',rm_plan_id=N'" + item.Rm_plan_id + "',time_issue_qc=getdate(),place=N'" + item.Place + "' where whmr_code=N'" + item.Whmr_code + "' \n";
        //        strQry += "insert into W_M_HistoryOfTransaction([whmr_code],[m_name],[quantity],[transaction],[input_time],[PIC],[place]) \n";
        //        strQry+= "select N'" + item.Whmr_code + "',N'" + item.M_name + "',N'" + item.Quantity + "',N'WH transfer to QC',getdate(),N'" + item.Pic_issue_qc + "',N'" + item.Place + "'";
        //        conn = new CmCn();
        //        conn.ExcuteQry(strQry);
        //    }
        //}
        private void InsertData(string label_code)
        {
            
            
        }
        private void RemoveData(string QR_code)
        {
            
        }
        
        private void ckRemoveBox_CheckedChanged(object sender, EventArgs e)
        {
            if (ckRemoveBox.Checked)
            {
                ckRemoveBox.Text = "REMOVE/ XÓA";
            }
            else
            {
                ckRemoveBox.Text = "ADD/ THÊM";
            }
        }

        private void gvDetail_RowStyle(object sender, RowStyleEventArgs e)
        {
            GridView view = sender as GridView;
            if (e.RowHandle >= 0)
            {
                string status = view.GetRowCellValue(e.RowHandle, view.Columns["is_check"]).ToString();
                if (status == "False")
                {
                    e.Appearance.BackColor = Color.Red;
                }
                else if (status == "True")
                {
                    e.Appearance.BackColor = Color.Chartreuse;
                }
            }
        }

        private void cboShift_SelectionChangeCommitted(object sender, EventArgs e)
        {
           
        }

        private void dtpSupplyDate_ValueChanged(object sender, EventArgs e)
        {
            
        }

        private void btnShowDetail_Click(object sender, EventArgs e)
        {
            if (btnShowDetail.Text == "HIỆN CHI TIẾT")
            {
                lbDetailLabel.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                lbDetailTable.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                btnShowDetail.Text = "ẨN CHI TIẾT";
            }
            else
            {
                lbDetailLabel.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                lbDetailTable.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                btnShowDetail.Text = "HIỆN CHI TIẾT";
            }
        }
    }
}
