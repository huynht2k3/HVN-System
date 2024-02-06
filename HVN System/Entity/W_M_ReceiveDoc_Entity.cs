using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HVN_System.Entity
{
    public class W_M_ReceiveDoc_Entity
    {
        private string rm_doc_id;
        private string supplier;
        private string last_user_commit;
        private string rm_doc_link;
        private string rm_doc_name;
        private string rm_doc_link2;
        private string rm_doc_name2;
        private string rm_doc_link3;
        private string rm_doc_name3;
        private string rm_kind;
        private string truck_no;
        private DateTime receive_date;
        private DateTime last_time_commit;

        public string Rm_doc_id { get => rm_doc_id; set => rm_doc_id = value; }
        public string Supplier { get => supplier; set => supplier = value; }
        public string Last_user_commit { get => last_user_commit; set => last_user_commit = value; }
        public string Rm_doc_link { get => rm_doc_link; set => rm_doc_link = value; }
        public string Truck_no { get => truck_no; set => truck_no = value; }
        public DateTime Receive_date { get => receive_date; set => receive_date = value; }
        public DateTime Last_time_commit { get => last_time_commit; set => last_time_commit = value; }
        public string Rm_doc_name { get => rm_doc_name; set => rm_doc_name = value; }
        public string Rm_doc_link2 { get => rm_doc_link2; set => rm_doc_link2 = value; }
        public string Rm_doc_link3 { get => rm_doc_link3; set => rm_doc_link3 = value; }
        public string Rm_doc_name2 { get => rm_doc_name2; set => rm_doc_name2 = value; }
        public string Rm_doc_name3 { get => rm_doc_name3; set => rm_doc_name3 = value; }
        public string Rm_kind { get => rm_kind; set => rm_kind = value; }
    }
}
