using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HVN_System.Entity
{
    public class PUR_VS_Entity
    {
        private string vs_id;
        private string vs_requester;
        private DateTime vs_date;
        private string vs_des;
        private string dept;
        private string item_name;
        private string item_unit;
        private float unit_price;
        private string unit_currency;
        private float unit_vat;
        private string supplier_1;
        private string supplier_1_type;
        private string supplier_1_att;
        private string supplier_2;
        private string supplier_2_type;
        private string supplier_2_att;
        private string supplier_3;
        private string supplier_3_type;
        private string supplier_3_att;
        private string selected_supplier;
        private string selected_supplier_name;
        private string selected_supplier_type;
        private string dept_mgr;
        private DateTime dept_mgr_date;
        private string dept_sign;
        private string pur;
        private DateTime pur_date;
        private string fin_mgr;
        private DateTime fin_mgr_date;
        private string fin_mgr_sign;
        private string pur_mgr;
        private DateTime pur_mgr_date;
        private string pur_mgr_sign;
        private string plant_mgr;
        private DateTime plant_mgr_date;
        private string plant_mgr_sign;
        private string vs_status;
        private string vs_comment;
        private decimal estimate_yearly_amount;
        private string isActive;
        private string current_pic;
        private string dept_comment;
        private string pur_comment;
        private string fin_mgr_comment;
        private string pur_mgr_comment;
        private string plant_mgr_comment;
        public string Vs_id { get => vs_id; set => vs_id = value; }
        public string Vs_requester { get => vs_requester; set => vs_requester = value; }
        public DateTime Vs_date { get => vs_date; set => vs_date = value; }
        public string Vs_des { get => vs_des; set => vs_des = value; }
        public string Item_name { get => item_name; set => item_name = value; }
        public string Item_unit { get => item_unit; set => item_unit = value; }
        public float Unit_price { get => unit_price; set => unit_price = value; }
        public string Unit_currency { get => unit_currency; set => unit_currency = value; }
        public float Unit_vat { get => unit_vat; set => unit_vat = value; }
        public string Dept_mgr { get => dept_mgr; set => dept_mgr = value; }
        public DateTime Dept_mgr_date { get => dept_mgr_date; set => dept_mgr_date = value; }
        public string Dept_sign { get => dept_sign; set => dept_sign = value; }
        public string Fin_mgr { get => fin_mgr; set => fin_mgr = value; }
        public DateTime Fin_mgr_date { get => fin_mgr_date; set => fin_mgr_date = value; }
        public string Fin_mgr_sign { get => fin_mgr_sign; set => fin_mgr_sign = value; }
        public string Pur_mgr { get => pur_mgr; set => pur_mgr = value; }
        public DateTime Pur_mgr_date { get => pur_mgr_date; set => pur_mgr_date = value; }
        public string Plant_mgr { get => plant_mgr; set => plant_mgr = value; }
        public DateTime Plant_mgr_date { get => plant_mgr_date; set => plant_mgr_date = value; }
        public string Plant_mgr_sign { get => plant_mgr_sign; set => plant_mgr_sign = value; }
        public string Vs_status { get => vs_status; set => vs_status = value; }
        public string Supplier_1 { get => supplier_1; set => supplier_1 = value; }
        public string Supplier_1_type { get => supplier_1_type; set => supplier_1_type = value; }
        public string Supplier_1_att { get => supplier_1_att; set => supplier_1_att = value; }
        public string Supplier_2 { get => supplier_2; set => supplier_2 = value; }
        public string Supplier_2_type { get => supplier_2_type; set => supplier_2_type = value; }
        public string Supplier_2_att { get => supplier_2_att; set => supplier_2_att = value; }
        public string Supplier_3 { get => supplier_3; set => supplier_3 = value; }
        public string Supplier_3_type { get => supplier_3_type; set => supplier_3_type = value; }
        public string Supplier_3_att { get => supplier_3_att; set => supplier_3_att = value; }
        public string Selected_supplier { get => selected_supplier; set => selected_supplier = value; }
        public string Pur { get => pur; set => pur = value; }
        public DateTime Pur_date { get => pur_date; set => pur_date = value; }
        public string Pur_mgr_sign { get => pur_mgr_sign; set => pur_mgr_sign = value; }
        public string Vs_comment { get => vs_comment; set => vs_comment = value; }
        public decimal Estimate_yearly_amount { get => estimate_yearly_amount; set => estimate_yearly_amount = value; }
        public string IsActive { get => isActive; set => isActive = value; }
        public string Current_pic { get => current_pic; set => current_pic = value; }
        public string Dept { get => dept; set => dept = value; }
        public string Dept_comment { get => dept_comment; set => dept_comment = value; }
        public string Pur_comment { get => pur_comment; set => pur_comment = value; }
        public string Fin_mgr_comment { get => fin_mgr_comment; set => fin_mgr_comment = value; }
        public string Pur_mgr_comment { get => pur_mgr_comment; set => pur_mgr_comment = value; }
        public string Plant_mgr_comment { get => plant_mgr_comment; set => plant_mgr_comment = value; }
        public string Selected_supplier_name { get => selected_supplier_name; set => selected_supplier_name = value; }
        public string Selected_supplier_type { get => selected_supplier_type; set => selected_supplier_type = value; }
    }
}
