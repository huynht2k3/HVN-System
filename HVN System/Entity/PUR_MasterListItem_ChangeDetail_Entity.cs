using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HVN_System.Entity
{
    public class PUR_MasterListItem_ChangeDetail_Entity
    {
        private string request_id;
        private string item_field;
        private string item_field_name;
        private string item_value;
        private string item_value_change;

        public string Request_id { get => request_id; set => request_id = value; }
        public string Item_field { get => item_field; set => item_field = value; }
        public string Item_field_name { get => item_field_name; set => item_field_name = value; }
        public string Item_value { get => item_value; set => item_value = value; }
        public string Item_value_change { get => item_value_change; set => item_value_change = value; }
    }
}
