using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HVN_System.Entity
{
    public class ADM_Delegation_Entity
    {
        private string dl_id;
        private DateTime dl_time;
        private string dl_requester;
        private DateTime dl_fromdate;
        private DateTime dl_todate;
        private string delegated_pic;
        private string dl_note;
        private string is_active;

        public string Dl_id { get => dl_id; set => dl_id = value; }
        public DateTime Dl_time { get => dl_time; set => dl_time = value; }
        public string Dl_requester { get => dl_requester; set => dl_requester = value; }
        public DateTime Dl_fromdate { get => dl_fromdate; set => dl_fromdate = value; }
        public DateTime Dl_todate { get => dl_todate; set => dl_todate = value; }
        public string Delegated_pic { get => delegated_pic; set => delegated_pic = value; }
        public string Dl_note { get => dl_note; set => dl_note = value; }
        public string Is_active { get => is_active; set => is_active = value; }
    }
}
