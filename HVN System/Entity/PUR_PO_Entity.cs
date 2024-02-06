using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HVN_System.Entity
{
    public class PUR_PO_Entity
    {
        private string po_no;
        private string supplier_name;
        private DateTime po_date;
        private DateTime last_time_update;
        private string payment_term;
        private string delivery_mode;
        private string incoterm;
        private DateTime pickup_date;
        private string po_currency;
        private string po_pic;
        private DateTime po_pic_date;
        private string po_pic_sign;
        private string po_checker;
        private DateTime po_check_date;
        private string po_checker_sign;
        private string po_approver;
        private DateTime po_approve_date;
        private string po_approver_sign;
        private string pr_no;
        private string is_active;
        private string po_status;
        private string requester;
        private string current_pic;
        private string dept;
        private string customer_name;
        private string project_name;
        private string purpose;
        private string pkw;
        private string old_po_no;
        private decimal amount;
        private decimal amount_vat;
        private decimal vat;
        private string po_type;
        private string capex_no;
        private bool is_selected;
        private string po_pic_comment;
        private string po_checker_comment;
        private string po_approver_comment;
        public string Po_no { get => po_no; set => po_no = value; }
        public string Supplier_name { get => supplier_name; set => supplier_name = value; }
        public DateTime Po_date { get => po_date; set => po_date = value; }
        public string Payment_term { get => payment_term; set => payment_term = value; }
        public string Delivery_mode { get => delivery_mode; set => delivery_mode = value; }
        public string Incoterm { get => incoterm; set => incoterm = value; }
        public DateTime Pickup_date { get => pickup_date; set => pickup_date = value; }
        public string Po_currency { get => po_currency; set => po_currency = value; }
        public string Po_pic { get => po_pic; set => po_pic = value; }
        public string Po_checker { get => po_checker; set => po_checker = value; }
        public DateTime Po_check_date { get => po_check_date; set => po_check_date = value; }
        public string Po_checker_sign { get => po_checker_sign; set => po_checker_sign = value; }
        public string Po_approver { get => po_approver; set => po_approver = value; }
        public DateTime Po_approve_date { get => po_approve_date; set => po_approve_date = value; }
        public string Po_approver_sign { get => po_approver_sign; set => po_approver_sign = value; }
        public string Pr_no { get => pr_no; set => pr_no = value; }
        public string Is_active { get => is_active; set => is_active = value; }
        public string Po_status { get => po_status; set => po_status = value; }
        public string Po_pic_sign { get => po_pic_sign; set => po_pic_sign = value; }
        public string Requester { get => requester; set => requester = value; }
        public string Current_pic { get => current_pic; set => current_pic = value; }
        public string Dept { get => dept; set => dept = value; }
        public string Customer_name { get => customer_name; set => customer_name = value; }
        public string Project_name { get => project_name; set => project_name = value; }
        public string Purpose { get => purpose; set => purpose = value; }
        public decimal Amount { get => amount; set => amount = value; }
        public decimal Vat { get => vat; set => vat = value; }
        public bool Is_selected { get => is_selected; set => is_selected = value; }
        public decimal Amount_vat { get => amount_vat; set => amount_vat = value; }
        public string Pkw { get => pkw; set => pkw = value; }
        public string Old_po_no { get => old_po_no; set => old_po_no = value; }
        public DateTime Last_time_update { get => last_time_update; set => last_time_update = value; }
        public string Capex_no { get => capex_no; set => capex_no = value; }
        public string Po_type { get => po_type; set => po_type = value; }
        public string Po_pic_comment { get => po_pic_comment; set => po_pic_comment = value; }
        public string Po_checker_comment { get => po_checker_comment; set => po_checker_comment = value; }
        public string Po_approver_comment { get => po_approver_comment; set => po_approver_comment = value; }
        public DateTime Po_pic_date { get => po_pic_date; set => po_pic_date = value; }
    }
}
