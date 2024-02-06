using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HVN_System.Entity
{
    public class ADM_LogActivities_Entity
    {
        private string user_name;
        private string computer_name;
        private string action;
        private DateTime last_time_commit;

        public string User_name { get => user_name; set => user_name = value; }
        public string Computer_name { get => computer_name; set => computer_name = value; }
        public string Action { get => action; set => action = value; }
        public DateTime Last_time_commit { get => last_time_commit; set => last_time_commit = value; }
    }
}
