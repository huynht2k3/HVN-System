using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HVN_System.Entity
{
    public class P_MonitorIssue
    {
        private string issue_id;
        private string issue_name;
        private string location;
        private string status;
        private DateTime start_time;
        private DateTime finish_time;
        private float duration;

        public string Issue_id { get => issue_id; set => issue_id = value; }
        public string Issue_name { get => issue_name; set => issue_name = value; }
        public string Location { get => location; set => location = value; }
        public string Status { get => status; set => status = value; }
        public DateTime Start_time { get => start_time; set => start_time = value; }
        public DateTime Finish_time { get => finish_time; set => finish_time = value; }
        public float Duration { get => duration; set => duration = value; }
    }
}
