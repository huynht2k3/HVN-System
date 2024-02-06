using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HVN_System.Entity
{
    public class W_CycleCountListPartial_Entity
    {
        private string cc_name;
        private string product_code;
        private string product_customer_code;

        public string Cc_name { get => cc_name; set => cc_name = value; }
        public string Product_code { get => product_code; set => product_code = value; }
        public string Product_customer_code { get => product_customer_code; set => product_customer_code = value; }
    }
}
