using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HVN_System.Entity
{
    public class PUR_VS_SupplierInfo_Entity
    {
        private string vs_id;
        private string supplier_name;
        private string vs_subject;
        private string vs_des_status;
        private string vs_score_status;
        private string vs_des;
        private float vs_score;
        private float vs_stt;

        public string Vs_id { get => vs_id; set => vs_id = value; }
        public string Supplier_name { get => supplier_name; set => supplier_name = value; }
        public string Vs_subject { get => vs_subject; set => vs_subject = value; }
        public string Vs_des { get => vs_des; set => vs_des = value; }
        public float Vs_score { get => vs_score; set => vs_score = value; }
        public float Vs_stt { get => vs_stt; set => vs_stt = value; }
        public string Vs_des_status { get => vs_des_status; set => vs_des_status = value; }
        public string Vs_score_status { get => vs_score_status; set => vs_score_status = value; }

        public static implicit operator PUR_VS_SupplierInfo_Entity(PUR_PRDetail_Entity v)
        {
            throw new NotImplementedException();
        }
    }
}
