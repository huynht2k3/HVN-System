using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HVN_System.Entity
{
    public class W_MasterList_Material_Entity
    {
        private string m_name;
        private string m_des;
        private float m_qty;
        private int expiry_day;
        private float raw_weight;
        private float raw_qty;
        private float pallet_weight;
        private string scale_type;
        private string m_kind;
        private string supplier;

        public string M_name { get => m_name; set => m_name = value; }
        public string M_des { get => m_des; set => m_des = value; }
        public float M_qty { get => m_qty; set => m_qty = value; }
        public float Raw_weight { get => raw_weight; set => raw_weight = value; }
        public float Raw_qty { get => raw_qty; set => raw_qty = value; }
        public string Scale_type { get => scale_type; set => scale_type = value; }
        public int Expiry_day { get => expiry_day; set => expiry_day = value; }
        public string M_kind { get => m_kind; set => m_kind = value; }
        public string Supplier { get => supplier; set => supplier = value; }
        public float Pallet_weight { get => pallet_weight; set => pallet_weight = value; }
    }
}
