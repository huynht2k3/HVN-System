using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace HVN_System.Entity
{
    public class P_ChangingFGData_Entity: INotifyPropertyChanged
    {
        private DateTime request_time;
        private DateTime approval_time;
        private string request_user;
        private string is_approval;
        private string approval_user;
        private string product_code;
        private string product_customer_code;
        private string modified_content;
        private string modified_sql_query;
        private string recover_sql_query;
        private string message_in_email;
        private string row_id;
        private string email_address;
        private string requester_name;
        private bool selected;

        public DateTime Request_time { get => request_time; set => request_time = value; }
        public DateTime Approval_time { get => approval_time; set => approval_time = value; }
        public string Request_user { get => request_user; set => request_user = value; }
        public string Is_approval { get => is_approval; set => is_approval = value; }
        public string Approval_user { get => approval_user; set => approval_user = value; }
        public string Product_code { get => product_code; set => product_code = value; }
        public string Product_customer_code { get => product_customer_code; set => product_customer_code = value; }
        public string Modified_content { get => modified_content; set => modified_content = value; }
        public string Modified_sql_query { get => modified_sql_query; set => modified_sql_query = value; }
        public string Recover_sql_query { get => recover_sql_query; set => recover_sql_query = value; }
        public string Message_in_email { get => message_in_email; set => message_in_email = value; }
        public string Email_address { get => email_address; set => email_address = value; }
        public string Requester_name { get => requester_name; set => requester_name = value; }
        public bool Selected { get => selected; set => selected = value; }
        public string Row_id { get => row_id; set => row_id = value; }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
