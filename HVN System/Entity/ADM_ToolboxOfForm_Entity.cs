using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HVN_System.Entity
{
    public class ADM_ToolboxOfForm_Entity
    {
        private string frm_name;
        private string toolbox_name;
        private string toolbox_des;
        private string toolbox_group;
        private string is_delegate;

        public string Frm_name { get => frm_name; set => frm_name = value; }
        public string Toolbox_name { get => toolbox_name; set => toolbox_name = value; }
        public string Toolbox_des { get => toolbox_des; set => toolbox_des = value; }
        public string Toolbox_group { get => toolbox_group; set => toolbox_group = value; }
        public string Is_delegate { get => is_delegate; set => is_delegate = value; }
    }
}
