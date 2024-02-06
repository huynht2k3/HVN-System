using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HVN_System.Entity
{
    public class P_FG_Entity
    {
        public int Stt { get => _Stt; set => _Stt = value; }
        public string Product_code { get => _product_code; set => _product_code = value; }
        public string Product_name { get => _product_name; set => _product_name = value; }
        public int Product_quantity { get => _product_quantity; set => _product_quantity = value; }
        public float Product_weight { get => _product_weight; set => _product_weight = value; }
        public string Last_time_commit { get => _last_time_commit; set => _last_time_commit = value; }
        public string Last_user_commit { get => _last_user_commit; set => _last_user_commit = value; }
        public string Time_submit { get => _time_submit; set => _time_submit = value; }
        public bool Edit { get => _edit; set => _edit = value; }
        public string Reason_change { get => _reason_change; set => _reason_change = value; }
        public string Product_customer_code { get => _product_customer_code; set => _product_customer_code = value; }
        public string Type_change { get => _type_change; set => _type_change = value; }
        public string Product_line { get => _product_line; set => _product_line = value; }
        public string Check_type { get => _check_type; set => _check_type = value; }
        public string Last_user_approve { get => _last_user_approve; set => _last_user_approve = value; }
        public string Last_time_approve { get => _last_time_approve; set => _last_time_approve = value; }
        public string Project_name { get => _project_name; set => _project_name = value; }
        public string Customer_name { get => _customer_name; set => _customer_name = value; }
        public float Standard_time { get => _standard_time; set => _standard_time = value; }
        public float Standard_time_finishing { get => _standard_time_finishing; set => _standard_time_finishing = value; }
        public float Price { get => _price; set => _price = value; }
        public decimal Number_operator { get => _number_operator; set => _number_operator = value; }
        public string Carton_type { get => _carton_type; set => _carton_type = value; }
        public string Product_rev { get => product_rev; set => product_rev = value; }
        public string Remark { get => remark; set => remark = value; }
        public string Prod_att_name { get => prod_att_name; set => prod_att_name = value; }
        public string Prod_att_link { get => prod_att_link; set => prod_att_link = value; }

        private string _product_code;
        private string _product_name;
        private string _project_name;
        private string _customer_name;
        private int _product_quantity;
        private float _product_weight;
        private float _standard_time;
        private float _standard_time_finishing;
        private float _price;
        private string _carton_type;
        private decimal _number_operator;
        private string _last_time_commit;
        private string _last_user_commit;
        private int _Stt;
        private string _time_submit;
        private bool _edit;
        private string _reason_change;
        private string _product_customer_code;
        private string _type_change;
        private string _product_line;
        private string _check_type;
        private string _last_user_approve;
        private string _last_time_approve;
        private string product_rev;
        private string remark;
        private string prod_att_name;
        private string prod_att_link;
    }
}
