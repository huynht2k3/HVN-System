using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HVN_System.Entity
{
    public class W_CycleCount_Adjustment_Entity
    {
        private string cc_name;
        private string label_code;
        private string product_customer_code;
        private float product_quantity;
        private string location;
        private string place;
        private string transaction;
        private DateTime input_time;
        private string pic;

        public string Cc_name { get => cc_name; set => cc_name = value; }
        public string Label_code { get => label_code; set => label_code = value; }
        public string Product_customer_code { get => product_customer_code; set => product_customer_code = value; }
        public float Product_quantity { get => product_quantity; set => product_quantity = value; }
        public string Location { get => location; set => location = value; }
        public string Place { get => place; set => place = value; }
        public string Transaction { get => transaction; set => transaction = value; }
        public DateTime Input_time { get => input_time; set => input_time = value; }
        public string Pic { get => pic; set => pic = value; }
    }
}
