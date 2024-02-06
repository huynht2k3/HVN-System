using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HVN_System.Entity
{
    public class PL_PlanFG_Entity
    {
        private DateTime _plan_date;
        private string _shift;
        private string _line_no;
        private string _product_code;
        private string _customer_product_code;
        private int _number_operator;
        private int _target;
        private DateTime _start_time;
        private DateTime _end_time;
        private string _priority;
        private string _note;
        private int _check_id;
        private string _line_id;
        private double _standard_time_FG;
        private string is_print;
        private string product_type;
        public DateTime Plan_date { get => _plan_date; set => _plan_date = value; }
        public string Shift { get => _shift; set => _shift = value; }
        public string Line_no { get => _line_no; set => _line_no = value; }
        public string Product_code { get => _product_code; set => _product_code = value; }
        public string Customer_product_code { get => _customer_product_code; set => _customer_product_code = value; }
        public int Number_operator { get => _number_operator; set => _number_operator = value; }
        public int Target { get => _target; set => _target = value; }
        public DateTime Start_time { get => _start_time; set => _start_time = value; }
        public DateTime End_time { get => _end_time; set => _end_time = value; }
        public string Priority { get => _priority; set => _priority = value; }
        public string Note { get => _note; set => _note = value; }
        public int Check_id { get => _check_id; set => _check_id = value; }
        public string Line_id { get => _line_id; set => _line_id = value; }
        public double Standard_time_FG { get => _standard_time_FG; set => _standard_time_FG = value; }
        public string Is_print { get => is_print; set => is_print = value; }
        public string Product_type { get => product_type; set => product_type = value; }
    }
}
