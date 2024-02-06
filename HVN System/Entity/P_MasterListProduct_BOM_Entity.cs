using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HVN_System.Entity
{
    public class P_MasterListProduct_BOM_Entity
    {
        private string product_customer_code;
        private string m_name;
        private float m_quantity;
        private int stt;

        public string Product_customer_code { get => product_customer_code; set => product_customer_code = value; }
        public string M_name { get => m_name; set => m_name = value; }
        public float M_quantity { get => m_quantity; set => m_quantity = value; }
        public int Stt { get => stt; set => stt = value; }
    }
}
