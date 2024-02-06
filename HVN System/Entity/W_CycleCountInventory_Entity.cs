using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HVN_System.Entity
{
    public class W_CycleCountInventory_Entity
    {
        private string cc_name;
        private string label_code;
        private string wh_location;
        private string pallet_no;
        private string place;
        private string pIC;
        private DateTime last_time_commit;
        private string product_customer_code;
        private string product_quantity;
        private DateTime plan_date;

        public string Cc_name { get => cc_name; set => cc_name = value; }
        public string Label_code { get => label_code; set => label_code = value; }
        public string Wh_location { get => wh_location; set => wh_location = value; }
        public string Pallet_no { get => pallet_no; set => pallet_no = value; }
        public string Place { get => place; set => place = value; }
        public DateTime Last_time_commit { get => last_time_commit; set => last_time_commit = value; }
        public string PIC { get => pIC; set => pIC = value; }
        public string Product_customer_code { get => product_customer_code; set => product_customer_code = value; }
        public string Product_quantity { get => product_quantity; set => product_quantity = value; }
        public DateTime Plan_date { get => plan_date; set => plan_date = value; }
    }
}
