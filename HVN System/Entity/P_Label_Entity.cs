    using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HVN_System.Entity
{
    public class P_Label_Entity
    {
        private string label_code;
        private string product_code;
        private string product_customer_code;
        private string product_name;
        private int product_quantity;
        private float product_weight;
        private string lot_no;
        private DateTime created_date;
        private string line;
        private DateTime scanned_date;
        private string check_type;
        private DateTime patrol_date;
        private DateTime date_input_wh;
        private DateTime expired_date;
        private string product_type;
        private string product_price;
        private string project_name;
        private string customer_name;
        private string standard_time;
        private string shift;
        private DateTime plan_date;
        private string op_input_wh;
        private string patrol_op;
        private string stt;
        private string place;
        private DateTime wh_locate_date;
        private string wh_locate_date_string;
        private string wh_location;
        private string wh_op_locate;
        private DateTime date_input_packing_zone;
        private string date_input_packing_zone_string;
        private string op_input_packing_zone;
        private DateTime date_packed;
        private string op_packed;
        private string pallet_no;
        private DateTime date_locate_packed;
        private string location_packed;
        private string op_locate_packed;
        private string note;
        private string invoice_no;
        private DateTime ship_date;
        private string ship_op;
        private bool check;
        private string isLock;
        private string comment; 
        private bool isEdit;
        private string truck_no;
        private string product_rev;
        private string patrol_result;
        private string error;
        public string Label_code { get => label_code; set => label_code = value; }
        public string Product_code { get => product_code; set => product_code = value; }
        public string Product_customer_code { get => product_customer_code; set => product_customer_code = value; }
        public string Product_name { get => product_name; set => product_name = value; }
        public int Product_quantity { get => product_quantity; set => product_quantity = value; }
        public float Product_weight { get => product_weight; set => product_weight = value; }
        public string Lot_no { get => lot_no; set => lot_no = value; }
        public DateTime Created_date { get => created_date; set => created_date = value; }
        public string Line { get => line; set => line = value; }
        public DateTime Scanned_date { get => scanned_date; set => scanned_date = value; }
        public string Check_type { get => check_type; set => check_type = value; }
        public DateTime Patrol_date { get => patrol_date; set => patrol_date = value; }
        public string Wh_location { get => wh_location; set => wh_location = value; }
        public DateTime Date_input_wh { get => date_input_wh; set => date_input_wh = value; }
        public DateTime Expired_date { get => expired_date; set => expired_date = value; }
        public string Product_type { get => product_type; set => product_type = value; }
        public string Product_price { get => product_price; set => product_price = value; }
        public string Project_name { get => project_name; set => project_name = value; }
        public string Customer_name { get => customer_name; set => customer_name = value; }
        public string Standard_time { get => standard_time; set => standard_time = value; }
        public string Shift { get => shift; set => shift = value; }
        public DateTime Plan_date { get => plan_date; set => plan_date = value; }
        public string Op_input_wh { get => op_input_wh; set => op_input_wh = value; }
        public string Stt { get => stt; set => stt = value; }
        public string Place { get => place; set => place = value; }
        public DateTime Wh_locate_date { get => wh_locate_date; set => wh_locate_date = value; }
        public string Wh_op_locate { get => wh_op_locate; set => wh_op_locate = value; }
        public string Wh_locate_date_string { get => wh_locate_date_string; set => wh_locate_date_string = value; }
        public DateTime Date_input_packing_zone { get => date_input_packing_zone; set => date_input_packing_zone = value; }
        public string Date_input_packing_zone_string { get => date_input_packing_zone_string; set => date_input_packing_zone_string = value; }
        public string Op_input_packing_zone { get => op_input_packing_zone; set => op_input_packing_zone = value; }
        public DateTime Date_packed { get => date_packed; set => date_packed = value; }
        public string Op_packed { get => op_packed; set => op_packed = value; }
        public string Pallet_no { get => pallet_no; set => pallet_no = value; }
        public DateTime Date_locate_packed { get => date_locate_packed; set => date_locate_packed = value; }
        public string Location_packed { get => location_packed; set => location_packed = value; }
        public string Op_locate_packed { get => op_locate_packed; set => op_locate_packed = value; }
        public string Note { get => note; set => note = value; }
        public string Invoice_no { get => invoice_no; set => invoice_no = value; }
        public DateTime Ship_date { get => ship_date; set => ship_date = value; }
        public string Ship_op { get => ship_op; set => ship_op = value; }
        public bool Check { get => check; set => check = value; }
        public string IsLock { get => isLock; set => isLock = value; }
        public string Comment { get => comment; set => comment = value; }
        public bool IsEdit { get => isEdit; set => isEdit = value; }
        public string Truck_no { get => truck_no; set => truck_no = value; }
        public string Product_rev { get => product_rev; set => product_rev = value; }
        public string Patrol_result { get => patrol_result; set => patrol_result = value; }
        public string Error { get => error; set => error = value; }
        public string Patrol_op { get => patrol_op; set => patrol_op = value; }
    }
}
