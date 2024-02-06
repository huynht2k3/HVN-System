using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HVN_System.Entity
{
    public class KPI_ActionMonitoring_Entity
    {
        private string act_name;
        private string act_des;
        private string inc_name;
        private string priority;
        private string location;
        private string assigned_user;
        private string status;
        private string last_user_commit;
        private string theme;
        private DateTime planned_for;
        private DateTime created_date;
        private DateTime last_time_commit;
        private string check_id;
        public string Act_name { get => act_name; set => act_name = value; }
        public string Act_des { get => act_des; set => act_des = value; }
        public string Inc_name { get => inc_name; set => inc_name = value; }
        public string Priority { get => priority; set => priority = value; }
        public string Location { get => location; set => location = value; }
        public string Assigned_user { get => assigned_user; set => assigned_user = value; }
        public string Status { get => status; set => status = value; }
        public string Last_user_commit { get => last_user_commit; set => last_user_commit = value; }
        public DateTime Planned_for { get => planned_for; set => planned_for = value; }
        public DateTime Created_date { get => created_date; set => created_date = value; }
        public DateTime Last_time_commit { get => last_time_commit; set => last_time_commit = value; }
        public string Theme { get => theme; set => theme = value; }
        public string Check_id { get => check_id; set => check_id = value; }
    }
}
