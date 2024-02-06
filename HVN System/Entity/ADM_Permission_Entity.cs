using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HVN_System.Entity
{
    public class ADM_Permission_Entity
    {
        private string _stt;
        private string _frm_name;
        private string _toolbox_name;
        private string _username;
        private string department;
        private string position;
        private string _toolbox_des;
        private string _last_user_commit;
        private string _last_time_commit;
        private bool _current_status;
        private bool _edit;

        public string Stt { get => _stt; set => _stt = value; }
        public string Frm_name { get => _frm_name; set => _frm_name = value; }
        public string Toolbox_des { get => _toolbox_des; set => _toolbox_des = value; }
        public string Username { get => _username; set => _username = value; }
        public string Toolbox_name { get => _toolbox_name; set => _toolbox_name = value; }
        public string Last_user_commit { get => _last_user_commit; set => _last_user_commit = value; }
        public string Last_time_commit { get => _last_time_commit; set => _last_time_commit = value; }
        public bool Edit { get => _edit; set => _edit = value; }
        public string Department { get => department; set => department = value; }
        public string Position { get => position; set => position = value; }
        public bool Current_status { get => _current_status; set => _current_status = value; }
    }
}
