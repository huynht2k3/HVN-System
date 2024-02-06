using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HVN_System.Entity
{
    public class HR_CarRegistration_Entity
    {
        private string request_id;
        private DateTime request_date;
        private string requester;
        private string dept;
        private string car_type;
        private DateTime from_date;
        private DateTime to_date;
        private string purpose;
        private float estimated_cost;
        private float actual_cost;
        private string current_pic;
        private string request_status;
        private string dept_mgr;
        private DateTime dept_mgr_date;
        private string hr_pic;
        private DateTime hr_pic_date;
        private string plant_mgr;
        private DateTime plant_mgr_date;
        private string from_loc;
        private string to_loc;
        private string requester_sign;
        private string dept_mgr_sign;
        private string hr_pic_sign;
        private string plant_mgr_sign;

        public string Request_id { get => request_id; set => request_id = value; }
        public DateTime Request_date { get => request_date; set => request_date = value; }
        public string Requester { get => requester; set => requester = value; }
        public string Dept { get => dept; set => dept = value; }
        public string Car_type { get => car_type; set => car_type = value; }
        public DateTime From_date { get => from_date; set => from_date = value; }
        public DateTime To_date { get => to_date; set => to_date = value; }
        public string Purpose { get => purpose; set => purpose = value; }
        public float Estimated_cost { get => estimated_cost; set => estimated_cost = value; }
        public float Actual_cost { get => actual_cost; set => actual_cost = value; }
        public string Current_pic { get => current_pic; set => current_pic = value; }
        public string Request_status { get => request_status; set => request_status = value; }
        public string Dept_mgr { get => dept_mgr; set => dept_mgr = value; }
        public DateTime Dept_mgr_date { get => dept_mgr_date; set => dept_mgr_date = value; }
        public string Hr_pic { get => hr_pic; set => hr_pic = value; }
        public DateTime Hr_pic_date { get => hr_pic_date; set => hr_pic_date = value; }
        public string Plant_mgr { get => plant_mgr; set => plant_mgr = value; }
        public DateTime Plant_mgr_date { get => plant_mgr_date; set => plant_mgr_date = value; }
        public string From_loc { get => from_loc; set => from_loc = value; }
        public string To_loc { get => to_loc; set => to_loc = value; }
        public string Requester_sign { get => requester_sign; set => requester_sign = value; }
        public string Dept_mgr_sign { get => dept_mgr_sign; set => dept_mgr_sign = value; }
        public string Hr_pic_sign { get => hr_pic_sign; set => hr_pic_sign = value; }
        public string Plant_mgr_sign { get => plant_mgr_sign; set => plant_mgr_sign = value; }

    }
}
