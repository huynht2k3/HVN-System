using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HVN_System.Entity
{
    public class W_M_IssueLabel_Entity
    {
        private string whmi_code;
        private string m_name;
        private float quantity;
        private float qty_in_box;
        private DateTime issue_date;
        private string p_shift;
        private DateTime lot_no;
        private string p_line;
        private float weight;
        private string product_customer_code;
        private DateTime supply_date;
        private string m_doc_id;
        private string whmr_code;
        private string note;
        private string transation;
        private string pic;

        public string Whmi_code { get => whmi_code; set => whmi_code = value; }
        public string M_name { get => m_name; set => m_name = value; }
        public float Quantity { get => quantity; set => quantity = value; }
        public DateTime Issue_date { get => issue_date; set => issue_date = value; }
        public string P_shift { get => p_shift; set => p_shift = value; }
        public DateTime Lot_no { get => lot_no; set => lot_no = value; }
        public string P_line { get => p_line; set => p_line = value; }
        public float Weight { get => weight; set => weight = value; }
        public string Product_customer_code { get => product_customer_code; set => product_customer_code = value; }
        public DateTime Supply_date { get => supply_date; set => supply_date = value; }
        public string M_doc_id { get => m_doc_id; set => m_doc_id = value; }
        public string Whmr_code { get => whmr_code; set => whmr_code = value; }
        public string Note { get => note; set => note = value; }
        public string Transation { get => transation; set => transation = value; }
        public float Qty_in_box { get => qty_in_box; set => qty_in_box = value; }
        public string Pic { get => pic; set => pic = value; }
    }
}
