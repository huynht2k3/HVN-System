using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HVN_System.Entity
{
    public class ADM_Account
    {
        private string username;
        private string password;
        private string position;
        private string department;
        private string description;
        private string name;
        private string signature;
        private string email_address;
        private string direct_manager;
        private string direct_checker;
        private string po_approver;
        private string account_status;
        private DateTime expired_date;
        public string Username { get => username; set => username = value; }
        public string Password { get => password; set => password = value; }
        public string Position { get => position; set => position = value; }
        public string Department { get => department; set => department = value; }
        public string Description { get => description; set => description = value; }
        public string Name { get => name; set => name = value; }
        public string Email_address { get => email_address; set => email_address = value; }
        public string Direct_manager { get => direct_manager; set => direct_manager = value; }
        public string Direct_checker { get => direct_checker; set => direct_checker = value; }
        public string Po_approver { get => po_approver; set => po_approver = value; }
        public string Signature { get => signature; set => signature = value; }
        public DateTime Expired_date { get => expired_date; set => expired_date = value; }
        public string Account_status { get => account_status; set => account_status = value; }
    }
}
