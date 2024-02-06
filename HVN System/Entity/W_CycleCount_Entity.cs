using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HVN_System.Entity
{
    public class W_CycleCount_Entity
    {
        private string cc_name;
        private DateTime cc_date;
        private string cc_type;
        private string cc_des;
        private string isLock;

        public string Cc_name { get => cc_name; set => cc_name = value; }
        public DateTime Cc_date { get => cc_date; set => cc_date = value; }
        public string Cc_type { get => cc_type; set => cc_type = value; }
        public string Cc_des { get => cc_des; set => cc_des = value; }
        public string IsLock { get => isLock; set => isLock = value; }
    }
}
