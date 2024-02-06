using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HVN_System.Entity
{
    public class KPI_IncidentMonitoring
    {
        private string inc_name;
        private string inc_type;
        private string inc_level;
        private string inc_theme;
        private string inc_des;
        private string author;
        private string location;
        private DateTime created_time;
        private DateTime created_for;
        private DateTime update_time;
        private string isAction;
        private string last_user_commit;
        private string check_id;
        private string is8D;
        private string inc_customer;
        private string inc_status;
        private float sorting_time;
        private decimal cost;
        private string string_cost;
        private decimal sort_ext_cost;
        private decimal sort_int_cost;
        private decimal trans_cost;
        private decimal customer_claim_cost;
        private decimal other_cost;
        private DateTime extrus_lot;
        private string extrus_lot_string;
        private DateTime finishing_lot;
        private string finishing_lot_string;
        private string image_link;
        public string Inc_name { get => inc_name; set => inc_name = value; }
        public string Inc_level { get => inc_level; set => inc_level = value; }
        public string Inc_theme { get => inc_theme; set => inc_theme = value; }
        public string Inc_des { get => inc_des; set => inc_des = value; }
        public string Author { get => author; set => author = value; }
        public string Location { get => location; set => location = value; }
        public DateTime Created_time { get => created_time; set => created_time = value; }
        public DateTime Created_for { get => created_for; set => created_for = value; }
        public DateTime Update_time { get => update_time; set => update_time = value; }
        public string IsAction { get => isAction; set => isAction = value; }
        public string Inc_type { get => inc_type; set => inc_type = value; }
        public string Last_user_commit { get => last_user_commit; set => last_user_commit = value; }
        public string Check_id { get => check_id; set => check_id = value; }
        public string Is8D { get => is8D; set => is8D = value; }
        public string Inc_customer { get => inc_customer; set => inc_customer = value; }
        public string Inc_status { get => inc_status; set => inc_status = value; }
        public float Sorting_time { get => sorting_time; set => sorting_time = value; }
        public decimal Cost { get => cost; set => cost = value; }
        public string String_cost { get => string_cost; set => string_cost = value; }
        public decimal Sort_ext_cost { get => sort_ext_cost; set => sort_ext_cost = value; }
        public decimal Sort_int_cost { get => sort_int_cost; set => sort_int_cost = value; }
        public decimal Trans_cost { get => trans_cost; set => trans_cost = value; }
        public decimal Customer_claim_cost { get => customer_claim_cost; set => customer_claim_cost = value; }
        public decimal Other_cost { get => other_cost; set => other_cost = value; }
        public DateTime Extrus_lot { get => extrus_lot; set => extrus_lot = value; }
        public DateTime Finishing_lot { get => finishing_lot; set => finishing_lot = value; }
        public string Image_link { get => image_link; set => image_link = value; }
        public string Extrus_lot_string { get => extrus_lot_string; set => extrus_lot_string = value; }
        public string Finishing_lot_string { get => finishing_lot_string; set => finishing_lot_string = value; }
    }
}
