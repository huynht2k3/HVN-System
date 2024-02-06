using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HVN_System.Entity
{
    public class W_MasterList_Rubber_Entity
    {
        private string m_name;
        private string m_des;
        private float pallet_weight;
        private int expired_date;

        public string R_name { get => m_name; set => m_name = value; }
        public string R_des { get => m_des; set => m_des = value; }
        public float Pallet_weight { get => pallet_weight; set => pallet_weight = value; }
        public int Expired_date { get => expired_date; set => expired_date = value; }
    }
}
