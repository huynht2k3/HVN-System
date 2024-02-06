using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HVN_System.Entity
{
    public class PUR_PR_CostSaving_Entity
    {
        private string pr_no;
        private string item_name;
        private string is_purchased;
        private decimal after_price;
        private decimal before_price;
        private decimal volume;
        private int stt;

        public string Pr_no { get => pr_no; set => pr_no = value; }
        public string Item_name { get => item_name; set => item_name = value; }
        public string Is_purchased { get => is_purchased; set => is_purchased = value; }
        public decimal After_price { get => after_price; set => after_price = value; }
        public decimal Before_price { get => before_price; set => before_price = value; }
        public decimal Volume { get => volume; set => volume = value; }
        public int Stt { get => stt; set => stt = value; }
    }
}
