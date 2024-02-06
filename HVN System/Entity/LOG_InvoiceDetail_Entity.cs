using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HVN_System.Entity
{
    public class LOG_InvoiceDetail_Entity
    {
        private string invoice_no;
        private string product_customer_code;
        private string product_code;
        private string product_name;
        private int quantity;
        private int actual_quantity;
        private string unit;
        private int stt;
        private string status;
        private string hs_code;
        private string truck_no;
        private string last_user_commit;
        private DateTime ship_date;

        public string Invoice_no { get => invoice_no; set => invoice_no = value; }
        public string Product_customer_code { get => product_customer_code; set => product_customer_code = value; }
        public string Product_name { get => product_name; set => product_name = value; }
        public int Quantity { get => quantity; set => quantity = value; }
        public string Unit { get => unit; set => unit = value; }
        public int Stt { get => stt; set => stt = value; }
        public int Actual_quantity { get => actual_quantity; set => actual_quantity = value; }
        public string Status { get => status; set => status = value; }
        public string Hs_code { get => hs_code; set => hs_code = value; }
        public string Product_code { get => product_code; set => product_code = value; }
        public string Truck_no { get => truck_no; set => truck_no = value; }
        public string Last_user_commit { get => last_user_commit; set => last_user_commit = value; }
        public DateTime Ship_date { get => ship_date; set => ship_date = value; }
    }
}
