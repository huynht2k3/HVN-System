using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HVN_System.Entity
{
    public class PUR_PRDetail_Entity
    {
        private string pr_no;
        private string item_name;
        private string hut_code;
        private string erp_code;
        private string supplier_name;
        private string unit;
        private decimal quantity;
        private decimal unit_price;
        private decimal vat;
        private decimal vat_amount;
        private decimal amount;
        private int stt;
        private decimal moq;
        private decimal standard_packing;
        private string unit_currency;
        private decimal min_price;
        private decimal max_price;

        public string Pr_no { get => pr_no; set => pr_no = value; }
        public string Item_name { get => item_name; set => item_name = value; }
        public string Hut_code { get => hut_code; set => hut_code = value; }
        public string Supplier_name { get => supplier_name; set => supplier_name = value; }
        public string Unit { get => unit; set => unit = value; }
        public decimal Quantity { get => quantity; set => quantity = value; }
        public decimal Unit_price { get => unit_price; set => unit_price = value; }
        public decimal Vat { get => vat; set => vat = value; }
        public decimal Amount { get => amount; set => amount = value; }
        public int Stt { get => stt; set => stt = value; }
        public decimal Vat_amount { get => vat_amount; set => vat_amount = value; }
        public string Erp_code { get => erp_code; set => erp_code = value; }
        public decimal Moq { get => moq; set => moq = value; }
        public decimal Standard_packing { get => standard_packing; set => standard_packing = value; }
        public string Unit_currency { get => unit_currency; set => unit_currency = value; }
        public decimal Min_price { get => min_price; set => min_price = value; }
        public decimal Max_price { get => max_price; set => max_price = value; }
    }
}
