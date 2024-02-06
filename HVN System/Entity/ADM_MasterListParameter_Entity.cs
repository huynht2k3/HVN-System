using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HVN_System.Entity
{
    public class ADM_MasterListParameter_Entity
    {
        private string parent_id;
        private string child_id;
        private string child_name;
        private string child_des;
        private float child_value;
        private float child_value_des;

        public string Parent_id { get => parent_id; set => parent_id = value; }
        public string Child_id { get => child_id; set => child_id = value; }
        public string Child_name { get => child_name; set => child_name = value; }
        public string Child_des { get => child_des; set => child_des = value; }
        public float Child_value { get => child_value; set => child_value = value; }
        public float Child_value_des { get => child_value_des; set => child_value_des = value; }
    }
}
