using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HVN_System.Entity
{
    public class PUR_PR_CompareCDE_Entity
    {
        private string pr_no;
        private string item_name;
        private decimal cde_budget;
        private decimal actual_cost;
        private decimal remain_budget;
        private decimal ultili_budget;
        private int stt;

        public string Pr_no { get => pr_no; set => pr_no = value; }
        public string Item_name { get => item_name; set => item_name = value; }
        public decimal Cde_budget { get => cde_budget; set => cde_budget = value; }
        public decimal Actual_cost { get => actual_cost; set => actual_cost = value; }
        public decimal Remain_budget { get => remain_budget; set => remain_budget = value; }
        public int Stt { get => stt; set => stt = value; }
        public decimal Ultili_budget { get => ultili_budget; set => ultili_budget = value; }
    }
}
