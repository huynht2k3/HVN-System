using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HVN_System.Entity
{
    public class PUR_MasterListItem_Entity
    {
        private string item_name;
        private string erp_code;
        private string hut_code;
        private string item_unit;
        private float unit_price;
        private float moq;
        private float standard_packing;
        private string unit_currency;
        private float unit_vat;
        private string supplier_name;
        private string item_type;
        private string item_status;
        private DateTime expired_date;
        private float delivery_cost;
        private float ddp_cost;
        private float max_price;
        private float min_price;

        public string Item_name { get => item_name; set => item_name = value; }
        public string Erp_code { get => erp_code; set => erp_code = value; }
        public string Hut_code { get => hut_code; set => hut_code = value; }
        public string Item_unit { get => item_unit; set => item_unit = value; }
        public float Unit_price { get => unit_price; set => unit_price = value; }
        public string Unit_currency { get => unit_currency; set => unit_currency = value; }
        public float Unit_vat { get => unit_vat; set => unit_vat = value; }
        public string Supplier_name { get => supplier_name; set => supplier_name = value; }
        public string Item_type { get => item_type; set => item_type = value; }
        public float Moq { get => moq; set => moq = value; }
        public float Standard_packing { get => standard_packing; set => standard_packing = value; }
        public string Item_status { get => item_status; set => item_status = value; }
        public DateTime Expired_date { get => expired_date; set => expired_date = value; }
        public float Delivery_cost { get => delivery_cost; set => delivery_cost = value; }
        public float Ddp_cost { get => ddp_cost; set => ddp_cost = value; }
        public float Max_price { get => max_price; set => max_price = value; }
        public float Min_price { get => min_price; set => min_price = value; }

    }
}
