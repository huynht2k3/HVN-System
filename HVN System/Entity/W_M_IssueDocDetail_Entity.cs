using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HVN_System.Entity
{
    public class W_M_IssueDocDetail_Entity
    {
        private string m_doc_id;
        private string m_name;
        private string product_customer_code;
        private float m_demand;
        private float qty_bom;
        private string p_line;
        private DateTime last_time_commit;
        private int stt;
        private string p_shift;
        private float actual_qty;
        private string status;
        private string check;
        private string fg_qty;

        public string M_doc_id { get => m_doc_id; set => m_doc_id = value; }
        public string M_name { get => m_name; set => m_name = value; }
        public string Product_customer_code { get => product_customer_code; set => product_customer_code = value; }
        public float M_demand { get => m_demand; set => m_demand = value; }
        public string P_line { get => p_line; set => p_line = value; }
        public DateTime Last_time_commit { get => last_time_commit; set => last_time_commit = value; }
        public int Stt { get => stt; set => stt = value; }
        public string P_shift { get => p_shift; set => p_shift = value; }
        public float Actual_qty { get => actual_qty; set => actual_qty = value; }
        public string Status { get => status; set => status = value; }
        public string Check { get => check; set => check = value; }
        public string Fg_qty { get => fg_qty; set => fg_qty = value; }
        public float Qty_bom { get => qty_bom; set => qty_bom = value; }
    }
}
