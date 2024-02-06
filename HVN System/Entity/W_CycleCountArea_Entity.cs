using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HVN_System.Entity
{
    public class W_CycleCountArea_Entity
    {
        private string cc_name;
        private string place;
        private bool isSelected;

        public string Cc_name { get => cc_name; set => cc_name = value; }
        public string Place { get => place; set => place = value; }
        public bool IsSelected { get => isSelected; set => isSelected = value; }
    }
}
