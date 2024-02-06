using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HVN_System.Entity
{
    public class PUR_MasterListSupplier_Entity
    {
        private string supplier_name;
        private string sup_shortname;
        private string sup_address;
        private string sup_tel;
        private string tax_code;
        private string contact_pic;
        private string email_address;
        private string sup_currency;
        private string payment_term;
        private string delivery_mode;
        private string incoterm;
        private string sup_status;

        public string Supplier_name { get => supplier_name; set => supplier_name = value; }
        public string Sup_shortname { get => sup_shortname; set => sup_shortname = value; }
        public string Sup_address { get => sup_address; set => sup_address = value; }
        public string Sup_tel { get => sup_tel; set => sup_tel = value; }
        public string Tax_code { get => tax_code; set => tax_code = value; }
        public string Contact_pic { get => contact_pic; set => contact_pic = value; }
        public string Email_address { get => email_address; set => email_address = value; }
        public string Sup_currency { get => sup_currency; set => sup_currency = value; }
        public string Payment_term { get => payment_term; set => payment_term = value; }
        public string Delivery_mode { get => delivery_mode; set => delivery_mode = value; }
        public string Incoterm { get => incoterm; set => incoterm = value; }
        public string Sup_status { get => sup_status; set => sup_status = value; }
    }
}
