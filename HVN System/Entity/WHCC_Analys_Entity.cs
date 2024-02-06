using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HVN_System.Entity
{
    public class WHCC_Analys_Entity
    {
        private bool select_item;
        private string label_code;
        private string sys_place;
        private string cc_place;
        private string sys_location;
        private string cc_location;
        private string sys_pallet_no;
        private string cc_pallet_no;
        private string label_status;
        private string product_customer_code;
        private float product_quantity;

        public bool Select_item { get => select_item; set => select_item = value; }
        public string Label_code { get => label_code; set => label_code = value; }
        public string Sys_place { get => sys_place; set => sys_place = value; }
        public string Cc_place { get => cc_place; set => cc_place = value; }
        public string Sys_location { get => sys_location; set => sys_location = value; }
        public string Cc_location { get => cc_location; set => cc_location = value; }
        public string Sys_pallet_no { get => sys_pallet_no; set => sys_pallet_no = value; }
        public string Cc_pallet_no { get => cc_pallet_no; set => cc_pallet_no = value; }
        public string Label_status { get => label_status; set => label_status = value; }
        public string Product_customer_code { get => product_customer_code; set => product_customer_code = value; }
        public float Product_quantity { get => product_quantity; set => product_quantity = value; }
    }
}
