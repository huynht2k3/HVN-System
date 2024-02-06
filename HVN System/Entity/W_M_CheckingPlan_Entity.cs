using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HVN_System.Entity
{
    public class W_M_CheckingPlan_Entity
    {
        private string rm_plan_id;
        private DateTime check_date;

        public string Rm_plan_id { get => rm_plan_id; set => rm_plan_id = value; }
        public DateTime Check_date { get => check_date; set => check_date = value; }
    }
}
