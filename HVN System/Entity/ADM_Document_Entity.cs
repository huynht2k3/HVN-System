using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HVN_System.Entity
{
    public class ADM_Document_Entity
    {
        private string doc_kind;
        private string doc_content;
        private string doc_link;
        private string doc_date;
        private string time_commit;
        private string doc_note;
        private int stt;
        private string doc_id;

        public string Doc_kind { get => doc_kind; set => doc_kind = value; }
        public string Doc_content { get => doc_content; set => doc_content = value; }
        public string Doc_link { get => doc_link; set => doc_link = value; }
        public string Doc_date { get => doc_date; set => doc_date = value; }
        public string Time_commit { get => time_commit; set => time_commit = value; }
        public string Doc_note { get => doc_note; set => doc_note = value; }
        public int Stt { get => stt; set => stt = value; }
        public string Doc_id { get => doc_id; set => doc_id = value; }
    }
}
