using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HVN_System.Entity
{
    public class KPI_PD_QtyFG2_Entity
    {
        private DateTime p_date;
        private string product_customer_code;
        private float p_qty;
        private float std_time;
        private float std_weight;
        private float total_time;
        private float total_weight;

        public DateTime P_date { get => p_date; set => p_date = value; }
        public string Product_customer_code { get => product_customer_code; set => product_customer_code = value; }
        public float P_qty { get => p_qty; set => p_qty = value; }
        public float Std_time { get => std_time; set => std_time = value; }
        public float Std_weight { get => std_weight; set => std_weight = value; }
        public float Total_time { get => total_time; set => total_time = value; }
        public float Total_weight { get => total_weight; set => total_weight = value; }
    }
}
