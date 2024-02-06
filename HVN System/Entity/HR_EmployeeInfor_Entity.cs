using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HVN_System.Entity
{
    public class HR_EmployeeInfor_Entity
    {
        private string emp_id;
        private string emp_name;
        private string emp_dept;
        private string emp_area;
        private DateTime onboard_date;

        public string Emp_id { get => emp_id; set => emp_id = value; }
        public string Emp_name { get => emp_name; set => emp_name = value; }
        public string Emp_dept { get => emp_dept; set => emp_dept = value; }
        public string Emp_area { get => emp_area; set => emp_area = value; }
        public DateTime Onboard_date { get => onboard_date; set => onboard_date = value; }
    }
}
