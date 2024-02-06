using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HVN_System.Entity
{
    public class W_M_ReceiveLabel_Entity
    {
        private string whmr_code;
        private string whmr_code_origin;
        private string m_name;
        private float quantity;
        private float quantity_ng;
        private DateTime lot_no;
        private string lot_no_string;
        private DateTime created_date;
        private string created_user;
        private string rm_doc_id;
        private int stt;
        private bool isSelected;
        private string truck_no;
        private string transaction;
        private string wh_op;
        private string ww;
        private string wh_okng;
        private string place;
        private DateTime wh_receive_time;
        private bool check;
        private string is_check;
        private string wh_location;
        private string op_locate;
        private string pic_issue_qc;
        private DateTime time_issue_qc;
        private string rm_plan_id;
        private string pic_qc;
        private DateTime time_qc_check;
        private string p_shift;
        private string qc_shift;
        private string qc_okng;
        private string whmi_code;
        private string transation;
        private bool isEdit;
        public string Whmr_code { get => whmr_code; set => whmr_code = value; }
        public string M_name { get => m_name; set => m_name = value; }
        public float Quantity { get => quantity; set => quantity = value; }
        public DateTime Lot_no { get => lot_no; set => lot_no = value; }
        public DateTime Created_date { get => created_date; set => created_date = value; }
        public string Created_user { get => created_user; set => created_user = value; }
        public string Rm_doc_id { get => rm_doc_id; set => rm_doc_id = value; }
        public int Stt { get => stt; set => stt = value; }
        public bool IsSelected { get => isSelected; set => isSelected = value; }
        public string Truck_no { get => truck_no; set => truck_no = value; }
        public string Transaction { get => transaction; set => transaction = value; }
        public string Wh_okng { get => wh_okng; set => wh_okng = value; }
        public string Wh_op { get => wh_op; set => wh_op = value; }
        public string Place { get => place; set => place = value; }
        public DateTime Wh_receive_time { get => wh_receive_time; set => wh_receive_time = value; }
        public bool Check { get => check; set => check = value; }
        public string Is_check { get => is_check; set => is_check = value; }
        public string Pic_issue_qc { get => pic_issue_qc; set => pic_issue_qc = value; }
        public DateTime Time_issue_qc { get => time_issue_qc; set => time_issue_qc = value; }
        public string Rm_plan_id { get => rm_plan_id; set => rm_plan_id = value; }
        public string Pic_qc { get => pic_qc; set => pic_qc = value; }
        public DateTime Time_qc_check { get => time_qc_check; set => time_qc_check = value; }
        public float Quantity_ng { get => quantity_ng; set => quantity_ng = value; }
        public string Wh_location { get => wh_location; set => wh_location = value; }
        public string Op_locate { get => op_locate; set => op_locate = value; }
        public string P_shift { get => p_shift; set => p_shift = value; }
        public string Qc_shift { get => qc_shift; set => qc_shift = value; }
        public string Whmr_code_origin { get => whmr_code_origin; set => whmr_code_origin = value; }
        public string Qc_okng { get => qc_okng; set => qc_okng = value; }
        public string Whmi_code { get => whmi_code; set => whmi_code = value; }
        public string Lot_no_string { get => lot_no_string; set => lot_no_string = value; }
        public string Transation { get => transation; set => transation = value; }
        public bool IsEdit { get => isEdit; set => isEdit = value; }
        public string Ww { get => ww; set => ww = value; }
    }
}
