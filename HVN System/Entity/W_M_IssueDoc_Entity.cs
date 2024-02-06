using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HVN_System.Entity
{
    public class W_M_IssueDoc_Entity
    {
        private string m_doc_id;
        private DateTime last_time_commit;
        private string m_doc_status;
        private DateTime m_doc_complete_time;
        private string m_doc_complete_time_str;
        private DateTime m_doc_supply_date;
        public string M_doc_id { get => m_doc_id; set => m_doc_id = value; }
        public DateTime Last_time_commit { get => last_time_commit; set => last_time_commit = value; }
        public string M_doc_status { get => m_doc_status; set => m_doc_status = value; }
        public DateTime M_doc_complete_time { get => m_doc_complete_time; set => m_doc_complete_time = value; }
        public string M_doc_complete_time_str { get => m_doc_complete_time_str; set => m_doc_complete_time_str = value; }
        public DateTime M_doc_supply_date { get => m_doc_supply_date; set => m_doc_supply_date = value; }
    }
}
