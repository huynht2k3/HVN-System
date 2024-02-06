using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HVN_System.Entity
{
    public class QC_SM_Score_Entity
    {
        private string emp_id;
        private string emp_project;
        private float emp_score;

        public string Emp_id { get => emp_id; set => emp_id = value; }
        public string Emp_project { get => emp_project; set => emp_project = value; }
        public float Emp_score { get => emp_score; set => emp_score = value; }
    }
}
