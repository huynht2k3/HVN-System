using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HVN_System.Entity
{
    public class PUR_MasterListItem_Change_Entity
    {
        private string request_id;
        private string item_name;
        private string requester;
        private DateTime requester_date;
        private string dept_mgr;
        private DateTime dept_mgr_date;
        private string pur;
        private DateTime pur_date;
        private string pur_mgr;
        private DateTime pur_mgr_date;
        private string fin_mgr;
        private DateTime fin_mgr_date;
        private string plant_mgr;
        private DateTime plant_mgr_date;
        private string current_pic;
        private string request_status;
        private string note;
        private int stt;
        private string dept;
        private string dept_comment;
        private string pur_comment;
        private string fin_mgr_comment;
        private string pur_mgr_comment;
        private string plant_mgr_comment;

        public string Request_id { get => request_id; set => request_id = value; }
        public string Item_name { get => item_name; set => item_name = value; }
        public string Requester { get => requester; set => requester = value; }
        public DateTime Requester_date { get => requester_date; set => requester_date = value; }
        public string Dept_mgr { get => dept_mgr; set => dept_mgr = value; }
        public DateTime Dept_mgr_date { get => dept_mgr_date; set => dept_mgr_date = value; }
        public string Pur { get => pur; set => pur = value; }
        public DateTime Pur_date { get => pur_date; set => pur_date = value; }
        public string Pur_mgr { get => pur_mgr; set => pur_mgr = value; }
        public DateTime Pur_mgr_date { get => pur_mgr_date; set => pur_mgr_date = value; }
        public string Fin_mgr { get => fin_mgr; set => fin_mgr = value; }
        public DateTime Fin_mgr_date { get => fin_mgr_date; set => fin_mgr_date = value; }
        public string Plant_mgr { get => plant_mgr; set => plant_mgr = value; }
        public DateTime Plant_mgr_date { get => plant_mgr_date; set => plant_mgr_date = value; }
        public string Current_pic { get => current_pic; set => current_pic = value; }
        public string Request_status { get => request_status; set => request_status = value; }
        public string Note { get => note; set => note = value; }
        public int Stt { get => stt; set => stt = value; }
        public string Dept_comment { get => dept_comment; set => dept_comment = value; }
        public string Pur_comment { get => pur_comment; set => pur_comment = value; }
        public string Fin_mgr_comment { get => fin_mgr_comment; set => fin_mgr_comment = value; }
        public string Pur_mgr_comment { get => pur_mgr_comment; set => pur_mgr_comment = value; }
        public string Plant_mgr_comment { get => plant_mgr_comment; set => plant_mgr_comment = value; }
        public string Dept { get => dept; set => dept = value; }
    }
}
