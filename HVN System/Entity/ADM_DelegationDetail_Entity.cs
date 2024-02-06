using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HVN_System.Entity
{
    public class ADM_DelegationDetail_Entity
    {
        private string dl_id;
        private string toolbox_name;
        private string toolbox_des;
        private string frm_name;
        private bool isSelect;

        public string Dl_id { get => dl_id; set => dl_id = value; }
        public string Toolbox_name { get => toolbox_name; set => toolbox_name = value; }
        public string Toolbox_des { get => toolbox_des; set => toolbox_des = value; }
        public string Frm_name { get => frm_name; set => frm_name = value; }
        public bool IsSelect { get => isSelect; set => isSelect = value; }
    }
}
