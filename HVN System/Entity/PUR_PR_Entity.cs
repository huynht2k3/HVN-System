using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HVN_System.Entity
{
    public class PUR_PR_Entity
    {
        private string pr_no;
        private DateTime pr_date;
        private DateTime estimate_received_date;
        private DateTime expect_issue_po_date;
        private string pr_content;
        private string pr_type;
        private string requester;
        private DateTime requester_date;
        private string dept;
        private decimal amount;
        private decimal amount_vat;
        private decimal vat;
        private string pr_currency;
        private string checker;
        private DateTime check_date;
        private string approver;
        private DateTime approve_date;
        private string is_active;
        private string create_user;
        private string pr_status;
        private string current_pic;
        private decimal amount_vnd;
        private string check_sign;
        private string approve_sign;
        private string requester_sign;
        private string capex_no;
        private string customer_name;
        private string project_name;
        private string po_no;
        private string old_pr_no;
        private string supplier_name;
        private int stt;
        private bool isSelected;
        private string checker_comment;
        private string approve_comment;
        private string advance_payment;
        public string Pr_no { get => pr_no; set => pr_no = value; }
        public string Requester { get => requester; set => requester = value; }
        public string Pr_type { get => pr_type; set => pr_type = value; }
        public string Pr_content { get => pr_content; set => pr_content = value; }
        public string Dept { get => dept; set => dept = value; }
        public string Pr_currency { get => pr_currency; set => pr_currency = value; }
        public string Is_active { get => is_active; set => is_active = value; }
        public string Create_user { get => create_user; set => create_user = value; }
        public DateTime Pr_date { get => pr_date; set => pr_date = value; }
        public DateTime Approve_date { get => approve_date; set => approve_date = value; }
        public DateTime Check_date { get => check_date; set => check_date = value; }
        public decimal Amount { get => amount; set => amount = value; }
        public decimal Vat { get => vat; set => vat = value; }
        public int Stt { get => stt; set => stt = value; }
        public DateTime Estimate_received_date { get => estimate_received_date; set => estimate_received_date = value; }
        public string Checker { get => checker; set => checker = value; }
        public string Approver { get => approver; set => approver = value; }
        public string Pr_status { get => pr_status; set => pr_status = value; }
        public string Current_pic { get => current_pic; set => current_pic = value; }
        public decimal Amount_vnd { get => amount_vnd; set => amount_vnd = value; }
        public decimal Amount_vat { get => amount_vat; set => amount_vat = value; }
        public string Check_sign { get => check_sign; set => check_sign = value; }
        public string Approve_sign { get => approve_sign; set => approve_sign = value; }
        public string Requester_sign { get => requester_sign; set => requester_sign = value; }
        public string Capex_no { get => capex_no; set => capex_no = value; }
        public string Customer_name { get => customer_name; set => customer_name = value; }
        public string Project_name { get => project_name; set => project_name = value; }
        public string Po_no { get => po_no; set => po_no = value; }
        public bool IsSelected { get => isSelected; set => isSelected = value; }
        public DateTime Expect_issue_po_date { get => expect_issue_po_date; set => expect_issue_po_date = value; }
        public string Checker_comment { get => checker_comment; set => checker_comment = value; }
        public string Approve_comment { get => approve_comment; set => approve_comment = value; }
        public string Supplier_name { get => supplier_name; set => supplier_name = value; }
        public string Advance_payment { get => advance_payment; set => advance_payment = value; }
        public DateTime Requester_date { get => requester_date; set => requester_date = value; }
        public string Old_pr_no { get => old_pr_no; set => old_pr_no = value; }
    }
}
