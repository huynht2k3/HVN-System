using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HVN_System.Entity
{
    public class W_M_ReceiveDocDetail_Entity
    {
        private string rm_doc_id;
        private string m_name;
        private string m_lot_no;
        private float quantity;
        private float number_carton;
        private int stt;
        private bool isSelect;
        private float qty_receive;
        public string Rm_doc_id { get => rm_doc_id; set => rm_doc_id = value; }
        public string M_name { get => m_name; set => m_name = value; }
        public string Lot_no { get => m_lot_no; set => m_lot_no = value; }
        public float Quantity { get => quantity; set => quantity = value; }
        public float Number_carton { get => number_carton; set => number_carton = value; }
        public int Stt { get => stt; set => stt = value; }
        public bool IsSelect { get => isSelect; set => isSelect = value; }
        public float Qty_receive { get => qty_receive; set => qty_receive = value; }
    }
}
