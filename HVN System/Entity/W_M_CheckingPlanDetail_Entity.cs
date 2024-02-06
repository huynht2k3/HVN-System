using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HVN_System.Entity
{
    public class W_M_CheckingPlanDetail_Entity
    {
        private string rm_plan_id;
        private string m_name;
        private string p_shift;
        private float quantity;
        private int stt;
        private string plan_type;   

        public string Rm_plan_id { get => rm_plan_id; set => rm_plan_id = value; }
        public string M_name { get => m_name; set => m_name = value; }
        public float Quantity { get => quantity; set => quantity = value; }
        public int Stt { get => stt; set => stt = value; }
        public string P_shift { get => p_shift; set => p_shift = value; }
        public string Plan_type { get => plan_type; set => plan_type = value; }
    }
}
