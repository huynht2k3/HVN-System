using HVN_System.View_Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HVN_System.Entity
{
    public class P_Scanner_Entity:BaseViewModel
    {
        private string _stt;

        public string Stt
        {
            get { return _stt; }
            set { _stt = value; OnPropertyChanged(); }
        }
        private string _scanner_id;

        public string Scanner_id
        {
            get { return _scanner_id; }
            set { _scanner_id = value; OnPropertyChanged(); }
        }
        private string _scanner_name;

        public string Scanner_name
        {
            get { return _scanner_name; }
            set { _scanner_name = value; OnPropertyChanged(); }
        }
        private string _department;

        public string Department
        {
            get { return _department; }
            set { _department = value; OnPropertyChanged(); }
        }
        private string _des1;

        public string Des1
        {
            get { return _des1; }
            set { _des1 = value; OnPropertyChanged(); }
        }
        private string _des2;

        public string Des2
        {
            get { return _des2; }
            set { _des2 = value; OnPropertyChanged(); }
        }
        private string _des3;

        public string Des3
        {
            get { return _des3; }
            set { _des3 = value; OnPropertyChanged(); }
        }
        private string _last_time_commit;

        public string Last_time_commit
        {
            get { return _last_time_commit; }
            set { _last_time_commit = value; OnPropertyChanged(); }
        }
        private string _last_user_commit;

        public string Last_user_commit
        {
            get { return _last_user_commit; }
            set { _last_user_commit = value; OnPropertyChanged(); }
        }
    }
}
