using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HVN_System.Entity
{
    public class KPI_PlantDashBoard
    {
        private string _item_name;
        private float _value;
        private string _value_string;
        private string _color;
        private DateTime _date;

        public string Item_name { get => _item_name; set => _item_name = value; }
        public float Value { get => _value; set => _value = value; }
        public string Value_string { get => _value_string; set => _value_string = value; }
        public string Color { get => _color; set => _color = value; }
        public DateTime Date { get => _date; set => _date = value; }
    }
}
