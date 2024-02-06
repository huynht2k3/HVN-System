using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Data.OleDb;
using HVN_System.Entity;
using System.Collections.ObjectModel;
using HVN_System.Util;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using Outlook = Microsoft.Office.Interop.Outlook;
using Excel = Microsoft.Office.Interop.Excel;
using System.Drawing.Printing;
using System.Windows.Forms;
using System.Globalization;
using System.Security.Cryptography;

namespace HVN_System
{
    public class ADO
    {
        private CmCn conn;

        public string Encrypt(string password)
        {
            byte[] encData_byte = new byte[password.Length];
            encData_byte = System.Text.Encoding.UTF8.GetBytes(password);
            string encodedData = Convert.ToBase64String(encData_byte);
            return encodedData;
        }

        public string Decrypt(string encodedData)
        {
            System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
            System.Text.Decoder utf8Decode = encoder.GetDecoder();
            byte[] todecode_byte = Convert.FromBase64String(encodedData);
            int charCount = utf8Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
            char[] decoded_char = new char[charCount];
            utf8Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
            string result = new String(decoded_char);
            return result;
        }
        public void Insert_W_CycleCountInventory(W_CycleCountInventory_Entity item)
        {
            string strQry = "insert into W_CycleCountInventory([cc_name],[label_code],[wh_location],[pallet_no],[place],[PIC],[last_time_commit],[product_customer_code],[product_quantity],[plan_date],[isActive]) \n";
            strQry += "values (N'" + item.Cc_name + "',N'" + item.Label_code + "',N'" + item.Wh_location + "',N'" + item.Pallet_no + "',N'" + item.Place + "',N'" + item.PIC + "',getdate(),N'" + item.Product_customer_code + "',N'" + item.Product_quantity + "',N'" + item.Plan_date.ToString("yyyy-MM-dd") + "',N'0')";
            try
            {
                conn = new CmCn();
                conn.ExcuteQry(strQry);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void Confirm_W_CycleCountInventory(string cc_name)
        {
            string strQry = " update W_CycleCountInventory set isActive =N'1' \n ";
            strQry += " where cc_name =N'" + cc_name + "' \n";
            strQry += " update W_CycleCount set isConfirm=N'Confirmed'";
            try
            {
                conn = new CmCn();
                conn.ExcuteQry(strQry);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void Delete_W_CycleCount(string cc_name)
        {
            string strQry = "update W_CycleCount set isActive=N'0' \n ";
            strQry += " where cc_name =N'" + cc_name + "'";
            try
            {
                conn = new CmCn();
                conn.ExcuteQry(strQry);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void Update_status_FG(string barcode)
        {
            string strQry = "update P_Label set scanned_date=getdate() \n";
            strQry += " WHERE label_code='" + barcode + "' and scanned_date is NULL";
            try
            {
                conn = new CmCn();
                conn.ExcuteQry(strQry);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Insert_Label(P_Label_Entity p_Label)
        {
            string strQry = "Insert into P_Label(label_code,product_code,product_name,lot_no,created_date,product_quantity \n";
            strQry += " , product_weight,product_customer_code,check_type,expired_date,product_type,shift,plan_date, product_price, project_name, customer_name, standard_time,line,product_rev,isLock) \n";
            strQry += " select N'" + p_Label.Label_code + "', N'" + p_Label.Product_code + "','" + p_Label.Product_name + "','" + p_Label.Lot_no + "', getdate() \n";
            strQry += ",N'" + p_Label.Product_quantity + "',N'" + p_Label.Product_weight + "',N'" + p_Label.Product_customer_code + "',N'" + p_Label.Check_type + "',N'" + p_Label.Expired_date.ToString("yyyy-MM-dd") + "' \n";
            strQry += ", N'" + p_Label.Product_type + "', N'" + p_Label.Shift + "',N'" + p_Label.Plan_date.ToString("yyyy-MM-dd") + "',N'" + p_Label.Product_price + "' \n";
            strQry += ",N'" + p_Label.Project_name + "',N'" + p_Label.Customer_name + "',N'" + p_Label.Standard_time + "',N'" + p_Label.Line + "',N'" + p_Label.Product_rev + "',N'Unblock' \n";
            try
            {
                conn = new CmCn();
                conn.ExcuteQry(strQry);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void Insert_ADM_LogActivities(ADM_LogActivities_Entity item)
        {
            string strQry = "insert into ADM_LogActivities(user_name,computer_name,last_time_commit,action) values (N'" + item.User_name + "',N'" + item.Computer_name + "',getdate(),N'" + item.Action + "')";
            try
            {
                conn = new CmCn();
                conn.ExcuteQry(strQry);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool Check_Login(string Username, string Password)
        {
            string StrQry = "select Username from Account where Username='" + Username + "' and Password='" + Password + "'";
            conn = new CmCn();
            DataTable dt = conn.ExcuteDataTable(StrQry);
            if (dt.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public DataTable Load_Label_FG_Data(string field, string condition)
        {
            string strQry = "select ";
            if (string.IsNullOrEmpty(field))
            {
                strQry += "* from P_Label \n ";
            }
            else
            {
                strQry += field + " from P_Label \n ";
            }
            if (!string.IsNullOrEmpty(condition))
            {
                strQry += "where " + condition;
            }
            conn = new CmCn();
            try
            {
                return conn.ExcuteDataTable(strQry);
            }
            catch (Exception)
            {
                return null;
            }
        }
        public DataTable Load_QC_MasterList_Chemical(string field, string condition)
        {
            string strQry = "select ";
            if (string.IsNullOrEmpty(field))
            {
                strQry += "* from QC_MasterList_Chemical \n ";
            }
            else
            {
                strQry += field + " from QC_MasterList_Chemical \n ";
            }
            if (!string.IsNullOrEmpty(condition))
            {
                strQry += "where " + condition;
            }
            conn = new CmCn();
            try
            {
                return conn.ExcuteDataTable(strQry);
            }
            catch (Exception)
            {
                return null;
            }
        }
        public DataTable Load_P_FMB_MasterListRubber(string field, string condition)
        {
            string strQry = "select ";
            if (string.IsNullOrEmpty(field))
            {
                strQry += "* from P_FMB_MasterListRubber \n ";
            }
            else
            {
                strQry += field + " from P_FMB_MasterListRubber \n ";
            }
            if (!string.IsNullOrEmpty(condition))
            {
                strQry += "where " + condition;
            }
            conn = new CmCn();
            try
            {
                return conn.ExcuteDataTable(strQry);
            }
            catch (Exception)
            {
                return null;
            }
        }
        public DataTable Load_ADM_Document(string field, string condition)
        {
            string strQry = "select ";
            if (string.IsNullOrEmpty(field))
            {
                strQry += "* from ADM_Document \n ";
            }
            else
            {
                strQry += field + " from ADM_Document \n ";
            }
            if (!string.IsNullOrEmpty(condition))
            {
                strQry += "where " + condition;
            }
            conn = new CmCn();
            try
            {
                return conn.ExcuteDataTable(strQry);
            }
            catch (Exception)
            {
                return null;
            }
        }
        public DataTable Load_W_MasterList_Location(string field, string condition)
        {
            string strQry = "select ";
            if (string.IsNullOrEmpty(field))
            {
                strQry += "* from W_MasterList_Location \n ";
            }
            else
            {
                strQry += field + " from W_MasterList_Location \n ";
            }
            if (!string.IsNullOrEmpty(condition))
            {
                strQry += "where " + condition;
            }
            conn = new CmCn();
            try
            {
                return conn.ExcuteDataTable(strQry);
            }
            catch (Exception)
            {
                return null;
            }
        }
        public DataTable Load_PL_PlanFG(string field, string condition)
        {
            string strQry = "select ";
            if (string.IsNullOrEmpty(field))
            {
                strQry += "* from PL_PlanFG \n ";
            }
            else
            {
                strQry += field + " from PL_PlanFG \n ";
            }
            if (!string.IsNullOrEmpty(condition))
            {
                strQry += "where " + condition;
            }
            conn = new CmCn();
            try
            {
                return conn.ExcuteDataTable(strQry);
            }
            catch (Exception)
            {
                return null;
            }
        }
        public DataTable Load_TEMP_W_ShippingInfor(string field, string condition)
        {
            string strQry = "select ";
            if (string.IsNullOrEmpty(field))
            {
                strQry += "* from TEMP_W_ShippingInfor \n ";
            }
            else
            {
                strQry += field + " from TEMP_W_ShippingInfor \n ";
            }
            if (!string.IsNullOrEmpty(condition))
            {
                strQry += "where " + condition;
            }
            conn = new CmCn();
            try
            {
                return conn.ExcuteDataTable(strQry);
            }
            catch (Exception)
            {
                return null;
            }
        }
        public DataTable Load_data_Account(string field, string condition)
        {
            string strQry = "select ";
            if (string.IsNullOrEmpty(field))
            {
                strQry += "* from Account \n ";
            }
            else
            {
                strQry += field + " from Account \n ";
            }
            if (!string.IsNullOrEmpty(condition))
            {
                strQry += "where " + condition;
            }
            conn = new CmCn();
            try
            {
                return conn.ExcuteDataTable(strQry);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public DataTable Form_Information()
        {
            string StrQry = "select frm_name,frm_des_VN,frm_des_EN from ADM_MasterListForm";
            conn = new CmCn();
            DataTable dt = conn.ExcuteDataTable(StrQry);
            return dt;
        }

        public DataTable ToolboxInForm(string frm_name, string username)
        {
            string StrQry = "select * from (select a.frm_name,a.toolbox_des,a.toolbox_name,b.username from ADM_ToolboxOfForm a left join ADM_ToolboxPermission b  \n";
            StrQry += "on a.toolbox_name=b.toolbox_name and b.username=N'" + username + "' and a.frm_name=b.frm_name) as c where frm_name='" + frm_name + "' \n";
            conn = new CmCn();
            DataTable dt = conn.ExcuteDataTable(StrQry);
            return dt;
        }
        public void UpdatePermission(Collection<ADM_Permission_Entity> list_data, string frm_name, string username)
        {
            string StrQry = "delete from ADM_ToolboxPermission where frm_name ='" + frm_name + "' and username ='" + username + "' \n";
            StrQry += "insert into ADM_ToolboxPermission(frm_name,toolbox_name,username,last_user_commit,last_time_commit) \n";
            string qry2 = "";
            foreach (ADM_Permission_Entity item in list_data)
            {
                if (item.Edit)
                {
                    if (string.IsNullOrEmpty(qry2))
                    {
                        qry2 += " select N'" + item.Frm_name + "',N'" + item.Toolbox_name + "',N'" + item.Username + "',N'" + item.Last_user_commit + "',getdate() \n";
                    }
                    else
                    {
                        qry2 += " union all select N'" + item.Frm_name + "',N'" + item.Toolbox_name + "',N'" + item.Username + "',N'" + item.Last_user_commit + "',getdate() \n";
                    }
                }
            }
            try
            {
                if (!string.IsNullOrEmpty(qry2))
                {
                    StrQry += qry2;
                }
                conn = new CmCn();
                conn.ExcuteQry(StrQry);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void Update_Cycle_Count_Information(W_CycleCount_Entity _CycleCount_Entity)
        {
            string strQry = "delete from W_CycleCount where cc_name=N'" + _CycleCount_Entity.Cc_name + "' \n";
            strQry += "delete from W_CycleCountListPartial where cc_name=N'" + _CycleCount_Entity.Cc_name + "' \n";
            strQry += "delete from W_CycleCountArea where cc_name=N'" + _CycleCount_Entity.Cc_name + "' \n";
            strQry += "insert into W_CycleCount ([cc_name],[cc_date],[cc_type],[cc_des],[last_user_commit],[last_time_commit],isActive,isLock) \n ";
            strQry += "values (N'" + _CycleCount_Entity.Cc_name + "',N'" + _CycleCount_Entity.Cc_date.ToString("yyyy-MM-dd") + "',N'" 
                + _CycleCount_Entity.Cc_type + "',N'" + _CycleCount_Entity.Cc_des + "',N'" + General_Infor.username + "',getdate(),N'1',N'Unblock') \n";
            conn = new CmCn();
            try
            {
                conn.ExcuteQry(strQry);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public string List_email_of_PIC(string condition)
        {
            string list_email="";
            string strQry = "select email_address from ADM_PersonInChargeOfProcess \n";
            strQry += "where " + condition;
            if (condition!="")
            {
                conn = new CmCn();
                DataTable dt = conn.ExcuteDataTable(strQry);
                foreach (DataRow item in dt.Rows)
                {
                    list_email += item["email_address"].ToString()+";";
                }
            }
            return list_email;
        }
        public void Block_W_CycleCount(string CC_name)
        {
            string strQry = "update W_CycleCount set isLock='Block' where cc_name=N'" + CC_name + "'";
            conn = new CmCn();
            try
            {
                conn.ExcuteQry(strQry);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public void Update_Cycle_Count_Partial(List<W_CycleCountListPartial_Entity> _list_partial)
        {
            string strQry = "";
            string qry2 = "";
            foreach (W_CycleCountListPartial_Entity item in _list_partial)
            {
                if (string.IsNullOrEmpty(qry2))
                {
                    qry2 += "select N'" + item.Cc_name + "',N'" + item.Product_code + "',N'" + item.Product_customer_code + "' \n";
                }
                else
                {
                    qry2 += "union all select N'" + item.Cc_name + "',N'" + item.Product_code + "',N'" + item.Product_customer_code + "' \n";
                }
            }
            if (!string.IsNullOrEmpty(qry2))
            {
                strQry += "insert into [W_CycleCountListPartial] ([cc_name],[product_code],[product_customer_code]) \n ";
                strQry += qry2;
            }
            try
            {
                conn = new CmCn();
                conn.ExcuteQry(strQry);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public void Update_Cycle_Count_Place(List<W_CycleCountArea_Entity> _list_Parital_Place)
        {
            string strQry = "";
            string qry2 = "";
            foreach (W_CycleCountArea_Entity item in _list_Parital_Place)
            {
                if (string.IsNullOrEmpty(qry2))
                {
                    qry2 += "select N'" + item.Cc_name  + "',N'" + item.Place + "' \n";
                }
                else
                {
                    qry2 += "union all select N'" + item.Cc_name + "',N'" + item.Place + "' \n";
                }
            }
            if (!string.IsNullOrEmpty(qry2))
            {
                strQry += "insert into [W_CycleCountArea] ([cc_name],[place]) \n ";
                strQry += qry2;
            }
            try
            {
                conn = new CmCn();
                conn.ExcuteQry(strQry);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public DataTable LoadProductionPlanFG(string need_infor, string Plan_date)
        {
            string strQry = "select ";
            if (!string.IsNullOrEmpty(need_infor))
            {
                strQry += need_infor;
            }
            else
            {
                strQry += "* \n";
            }
            strQry += " from PL_PlanFG \n";
            if (!string.IsNullOrEmpty(Plan_date))
            {
                strQry += " Where plan_date='" + Plan_date + "'";
            }
            conn = new CmCn();
            try
            {
                return conn.ExcuteDataTable(strQry);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void SaveProductionPlanFG(List<PL_PlanFG_Entity> list_data, DateTime Plan_date)
        {
            string StrQry = "delete from PL_PlanFG where plan_date>='" + Plan_date.ToString("yyyy-MM-dd HH:mm:ss") + "' \n";
            StrQry += "insert into PL_PlanFG(plan_date,shift,line_no,product_code,customer_product_code,number_operator,target,start_time,end_time,note,is_print,product_type) \n";
            string qry2 = "";
            foreach (PL_PlanFG_Entity item in list_data)
            {
                if (string.IsNullOrEmpty(qry2))
                {
                    qry2 += " select N'" + item.Plan_date.ToString("yyyy-MM-dd") + "',N'" + item.Shift + "',N'" + item.Line_no + "',N'" + item.Product_code + "',";
                    qry2 += "N'" + item.Customer_product_code + "',N'" + item.Number_operator + "',N'" + item.Target + "',N'" + item.Start_time.ToString("yyyy-MM-dd HH:mm:ss") + "',";
                    qry2 += "N'" + item.End_time.ToString("yyyy-MM-dd HH:mm:ss") + "',N'" + item.Note + "',N'" + item.Is_print + "',N'" + item.Product_type + "' \n";
                }
                else
                {
                    qry2 += " union all select N'" + item.Plan_date.ToString("yyyy-MM-dd") + "',N'" + item.Shift + "',N'" + item.Line_no + "',N'" + item.Product_code + "',";
                    qry2 += "N'" + item.Customer_product_code + "',N'" + item.Number_operator + "',N'" + item.Target + "',N'" + item.Start_time.ToString("yyyy-MM-dd HH:mm:ss") + "',";
                    qry2 += "N'" + item.End_time.ToString("yyyy-MM-dd HH:mm:ss") + "',N'" + item.Note + "',N'" + item.Is_print + "',N'" + item.Product_type + "' \n";
                }
            }
            try
            {
                if (!string.IsNullOrEmpty(qry2))
                {
                    StrQry += qry2;
                }
                conn = new CmCn();
                conn.ExcuteQry(StrQry);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void ADM_Add_New_Account(ADM_Account account)
        {
            string pw = Encrypt(account.Password);
            string strQry = " delete from Account where Username=N'" + account.Username + "' \n";
            strQry += " insert into Account (Username,Password,Position,Department,Description,Name,Email_address,Direct_manager,direct_checker,po_approver,signature,expired_date) \n";
            strQry += " values (N'" + account.Username + "',N'" + pw + "',N'" + account.Position + "',N'" + account.Department
                + "',N'" + account.Description + "',N'" + account.Name + "',N'" + account.Email_address + "',N'" 
                + account.Direct_manager + "',N'" + account.Direct_checker + "',N'" + account.Po_approver + "',N'" + account.Signature + "',N'" + account.Expired_date.ToString("yyyy-MM-dd") + "')";
            try
            {
                conn = new CmCn();
                conn.ExcuteQry(strQry);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        public bool Check_permission(string frm_name, string toolbox_name, string username)
        {
            //string strQry = "select * from ADM_ToolboxPermission where frm_name='" + frm_name + "' and toolbox_name='" + toolbox_name + "' and username='" + username + "'";
            //conn = new CmCn();
            //string result = conn.ExcuteString(strQry);
            //if (result == "")
            //{
            //    //Neu khong tim thay thi enable = false
            //    return false;
            //}
            //else
            //{
            //    //Neu tim thay thi enable=true
            //    return true;
            //}
            var Result = General_Infor.List_permission.Where(x => x.Frm_name == frm_name && x.Toolbox_name == toolbox_name);
            if (Result.Count()>0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        //public bool W_Material_CheckingBlock(string whmr_code)
        //{
        //    string 
        //}
        public int Load_HVN_Product_Code()
        {
            string strQry = "select MAX(product_code) from P_MasterListProduct";
            conn = new CmCn();
            if (string.IsNullOrEmpty(conn.ExcuteString(strQry)))
            {
                return 1;
            }
            else
            {
                return int.Parse(conn.ExcuteString(strQry).Substring(3, conn.ExcuteString(strQry).Length - 3)) + 1;
            }
        }
        public void Submit_Change_FG(P_FG_Entity FG_Entity)
        {
            string strQry = "Delete from P_MasterListProductSubmit Where product_code='" + FG_Entity.Product_code + "' \n";
            strQry += " Insert into P_MasterListProductSubmit(product_code,product_code_HVN,product_name,product_weight,product_quantity,product_line,last_time_commit,last_user_commit,type_change,reason_change,check_type) \n";
            strQry += " select N'" + FG_Entity.Product_code + "',N'" + FG_Entity.Product_customer_code + "',N'" + FG_Entity.Product_name + "',";
            strQry += "N'" + FG_Entity.Product_weight + "',N'" + FG_Entity.Product_quantity + "',N'" + FG_Entity.Product_line + "',";
            strQry += "getdate(),N'" + General_Infor.username + "',N'" + FG_Entity.Type_change + "',N'" + FG_Entity.Reason_change + "',N'" + FG_Entity.Check_type + "'";
            conn = new CmCn();
            try
            {
                conn.ExcuteQry(strQry);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void Submit_Changing_FG_Data(ObservableCollection<P_ChangingFGData_Entity> list_Changing)
        {
            string strQry = " Insert into P_MasterListProductSubmit(request_time,request_user,product_code,product_customer_code,modified_content,modified_sql_query,recover_sql_query) \n";
            string qry2 = "";
            foreach (P_ChangingFGData_Entity item in list_Changing)
            {
                if (string.IsNullOrEmpty(qry2))
                {
                    qry2 += " select N'" + item.Request_time.ToString("yyyy-MM-dd hh:mm:ss") + "',N'" + item.Request_user + "',N'" + item.Product_code + "',N'" + item.Product_customer_code + "',";
                    qry2 += "N'" + item.Modified_content + "',N'" + item.Modified_sql_query + "',N'" + item.Recover_sql_query + "' \n";
                }
                else
                {
                    qry2 += " union all select N'" + item.Request_time.ToString("yyyy-MM-dd hh:mm:ss") + "',N'" + item.Request_user + "',N'" + item.Product_code + "',N'" + item.Product_customer_code + "',";
                    qry2 += "N'" + item.Modified_content + "',N'" + item.Modified_sql_query + "',N'" + item.Recover_sql_query + "' \n";
                }
            }
            strQry += qry2;
            try
            {
                conn = new CmCn();
                conn.ExcuteQry(strQry);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable Load_PIC_Of_Process(string process_name, string step_name)
        {
            string strQry = "select a.Name,a.Email_address from Account a,ADM_PersonInChargeOfProcess b where a.Username=b.pic_user and b.procedure_name='" + process_name + "' ";
            if (!string.IsNullOrEmpty(step_name))
            {
                strQry += " and step_name='" + step_name + "'";
            }
            try
            {
                conn = new CmCn();
                return conn.ExcuteDataTable(strQry);
            }
            catch (Exception)
            {
                return null;
            }
        }
        public void Add_new_FG(P_FG_Entity FG_Entity)
        {
            string strQry = " Insert into P_MasterListProduct(product_code,product_code_HVN,product_name,product_weight,product_quantity,product_line,last_time_commit,last_user_commit) \n";
            strQry += " select N'" + FG_Entity.Product_code + "',N'" + FG_Entity.Product_customer_code + "',N'" + FG_Entity.Product_name + "',";
            strQry += "N'" + FG_Entity.Product_weight + "',N'" + FG_Entity.Product_quantity + "',N'" + FG_Entity.Product_line + "',";
            strQry += "getdate(),N'" + General_Infor.username + "'";
            conn = new CmCn();
            try
            {
                conn.ExcuteQry(strQry);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable Load_Parameter(string kind_parameter)
        {
            string strQry = "select child_id,child_name from ADM_MasterListParameter where parent_id='" + kind_parameter + "' ";
            if (kind_parameter == "month_in_year")
            {
                strQry += "order by cast((child_id) as int)";
            }
            conn = new CmCn();
            try
            {
                return conn.ExcuteDataTable(strQry);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable Load_W_CycleCount(string need_infor, string condition)
        {
            string strQry = "select ";
            if (!string.IsNullOrEmpty(need_infor))
            {
                strQry += need_infor;
            }
            else
            {
                strQry += " * \n";
            }
            strQry += " FROM [HVN_SYS].[dbo].[W_CycleCount]  \n";
            if (!string.IsNullOrEmpty(condition))
            {
                strQry += " where " + condition;
            }
            conn = new CmCn();
            try
            {
                return conn.ExcuteDataTable(strQry);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable Load_W_CycleCountInventory(string need_infor, string condition)
        {
            string strQry = "select ";
            if (!string.IsNullOrEmpty(need_infor))
            {
                strQry += need_infor;
            }
            else
            {
                strQry += " * \n";
            }
            strQry += " FROM [HVN_SYS].[dbo].[W_CycleCountInventory]  \n";
            if (!string.IsNullOrEmpty(condition))
            {
                strQry += " where " + condition;
            }
            conn = new CmCn();
            try
            {
                return conn.ExcuteDataTable(strQry);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable Load_W_Cycle_Count_List_Partial(string need_infor, string condition)
        {
            string strQry = "select ";
            if (!string.IsNullOrEmpty(need_infor))
            {
                strQry += need_infor;
            }
            else
            {
                strQry += " * \n";
            }
            strQry += " FROM [HVN_SYS].[dbo].[W_CycleCountListPartial]  \n";
            if (!string.IsNullOrEmpty(condition))
            {
                strQry += " where " + condition;
            }
            conn = new CmCn();
            try
            {
                return conn.ExcuteDataTable(strQry);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable Load_Cycle_Count_Partial_Item(string cc_name)
        {
            string strQry = " select a.product_code,a.product_customer_code,b.selected from P_MasterListProduct a left join \n";
            strQry += " (select product_code, product_customer_code as selected from W_CycleCountListPartial where cc_name = N'" + cc_name + "') " +
                "as b on a.product_code = b.product_code \n";
            conn = new CmCn();
            try
            {
                return conn.ExcuteDataTable(strQry);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable Load_Parameter_Detail(string need_infor, string condition)
        {
            string strQry = "select ";
            if (!string.IsNullOrEmpty(need_infor))
            {
                strQry += need_infor;
            }
            else
            {
                strQry += " * \n";
            }
            strQry += " FROM [HVN_SYS].[dbo].[ADM_MasterListParameter]  \n";
            if (!string.IsNullOrEmpty(condition))
            {
                strQry += " where " + condition;
            }
            conn = new CmCn();
            try
            {
                return conn.ExcuteDataTable(strQry);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable Load_Invoice_Detail(string need_infor, string condition)
        {
            string strQry = "select ";
            if (!string.IsNullOrEmpty(need_infor))
            {
                strQry += need_infor;
            }
            else
            {
                strQry += " * \n";
            }
            strQry += " FROM [HVN_SYS].[dbo].[LOG_InvoiceDetail]  \n";
            if (!string.IsNullOrEmpty(condition))
            {
                strQry += " where " + condition;
            }
            conn = new CmCn();
            try
            {
                return conn.ExcuteDataTable(strQry);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable Load_LOG_Invoice(string need_infor, string condition)
        {
            string strQry = "select ";
            if (!string.IsNullOrEmpty(need_infor))
            {
                strQry += need_infor;
            }
            else
            {
                strQry += " * \n";
            }
            strQry += " FROM [HVN_SYS].[dbo].[LOG_Invoice]  \n";
            if (!string.IsNullOrEmpty(condition))
            {
                strQry += " where " + condition;
            }
            conn = new CmCn();
            try
            {
                return conn.ExcuteDataTable(strQry);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable Load_W_M_IssueDocDetail(string need_infor, string condition)
        {
            string strQry = "select ";
            if (!string.IsNullOrEmpty(need_infor))
            {
                strQry += need_infor;
            }
            else
            {
                strQry += " * \n";
            }
            strQry += " FROM [HVN_SYS].[dbo].[W_M_IssueDocDetail]  \n";
            if (!string.IsNullOrEmpty(condition))
            {
                strQry += " where " + condition;
            }
            conn = new CmCn();
            try
            {
                return conn.ExcuteDataTable(strQry);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable Load_W_M_ReceiveDocDetail(string need_infor, string condition)
        {
            string strQry = "select ";
            if (!string.IsNullOrEmpty(need_infor))
            {
                strQry += need_infor;
            }
            else
            {
                strQry += " * \n";
            }
            strQry += " FROM [HVN_SYS].[dbo].[W_M_ReceiveDocDetail]  \n";
            if (!string.IsNullOrEmpty(condition))
            {
                strQry += " where " + condition;
            }
            conn = new CmCn();
            try
            {
                return conn.ExcuteDataTable(strQry);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable Load_KPI_QC_SupplierClaim(string need_infor, string condition)
        {
            string strQry = "select ";
            if (!string.IsNullOrEmpty(need_infor))
            {
                strQry += need_infor;
            }
            else
            {
                strQry += " * \n";
            }
            strQry += " FROM [HVN_SYS].[dbo].[KPI_QC_SupplierClaim]  \n";
            if (!string.IsNullOrEmpty(condition))
            {
                strQry += " where " + condition;
            }
            conn = new CmCn();
            try
            {
                return conn.ExcuteDataTable(strQry);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable Load_W_M_ReceiveLabel(string need_infor, string condition)
        {
            string strQry = "select ";
            if (!string.IsNullOrEmpty(need_infor))
            {
                strQry += need_infor;
            }
            else
            {
                strQry += " * \n";
            }
            strQry += " FROM [HVN_SYS].[dbo].[W_M_ReceiveLabel]  \n";
            if (!string.IsNullOrEmpty(condition))
            {
                strQry += " where " + condition;
            }
            conn = new CmCn();
            try
            {
                return conn.ExcuteDataTable(strQry);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable Load_W_M_RubberLabel(string need_infor, string condition)
        {
            string strQry = "select ";
            if (!string.IsNullOrEmpty(need_infor))
            {
                strQry += need_infor;
            }
            else
            {
                strQry += " * \n";
            }
            strQry += " FROM [HVN_SYS].[dbo].[W_M_RubberLabel]  \n";
            if (!string.IsNullOrEmpty(condition))
            {
                strQry += " where " + condition;
            }
            conn = new CmCn();
            try
            {
                return conn.ExcuteDataTable(strQry);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable Load_P_MasterListProduct_BOM(string need_infor, string condition)
        {
            string strQry = "select ";
            if (!string.IsNullOrEmpty(need_infor))
            {
                strQry += need_infor;
            }
            else
            {
                strQry += " * \n";
            }
            strQry += " FROM [HVN_SYS].[dbo].[P_MasterListProduct_BOM]  \n";
            if (!string.IsNullOrEmpty(condition))
            {
                strQry += " where " + condition;
            }
            conn = new CmCn();
            try
            {
                return conn.ExcuteDataTable(strQry);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable Load_TEMP_W_M_Receiving(string need_infor, string condition)
        {
            string strQry = "select ";
            if (!string.IsNullOrEmpty(need_infor))
            {
                strQry += need_infor;
            }
            else
            {
                strQry += " * \n";
            }
            strQry += " FROM [HVN_SYS].[dbo].[TEMP_W_M_Receiving]  \n";
            if (!string.IsNullOrEmpty(condition))
            {
                strQry += " where " + condition;
            }
            conn = new CmCn();
            try
            {
                return conn.ExcuteDataTable(strQry);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable Load_TEMP_W_R_Receiving(string need_infor, string condition)
        {
            string strQry = "select ";
            if (!string.IsNullOrEmpty(need_infor))
            {
                strQry += need_infor;
            }
            else
            {
                strQry += " * \n";
            }
            strQry += " FROM [HVN_SYS].[dbo].[TEMP_W_R_Receiving]  \n";
            if (!string.IsNullOrEmpty(condition))
            {
                strQry += " where " + condition;
            }
            conn = new CmCn();
            try
            {
                return conn.ExcuteDataTable(strQry);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable Load_TEMP_W_M_IssueToQC(string need_infor, string condition)
        {
            string strQry = "select ";
            if (!string.IsNullOrEmpty(need_infor))
            {
                strQry += need_infor;
            }
            else
            {
                strQry += " * \n";
            }
            strQry += " FROM [HVN_SYS].[dbo].[TEMP_W_M_IssueToQC]  \n";
            if (!string.IsNullOrEmpty(condition))
            {
                strQry += " where " + condition;
            }
            conn = new CmCn();
            try
            {
                return conn.ExcuteDataTable(strQry);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable Load_W_M_IssueDoc(string need_infor, string condition)
        {
            string strQry = "select ";
            if (!string.IsNullOrEmpty(need_infor))
            {
                strQry += need_infor;
            }
            else
            {
                strQry += " * \n";
            }
            strQry += " FROM [HVN_SYS].[dbo].[W_M_IssueDoc]  \n";
            if (!string.IsNullOrEmpty(condition))
            {
                strQry += " where " + condition;
            }
            conn = new CmCn();
            try
            {
                return conn.ExcuteDataTable(strQry);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable Load_W_M_ReceiveDoc(string need_infor, string condition)
        {
            string strQry = "select ";
            if (!string.IsNullOrEmpty(need_infor))
            {
                strQry += need_infor;
            }
            else
            {
                strQry += " * \n";
            }
            strQry += " FROM [HVN_SYS].[dbo].[W_M_ReceiveDoc]  \n";
            if (!string.IsNullOrEmpty(condition))
            {
                strQry += " where " + condition;
            }
            conn = new CmCn();
            try
            {
                return conn.ExcuteDataTable(strQry);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable Load_W_M_CheckingPlan(string need_infor, string condition)
        {
            string strQry = "select ";
            if (!string.IsNullOrEmpty(need_infor))
            {
                strQry += need_infor;
            }
            else
            {
                strQry += " * \n";
            }
            strQry += " FROM [HVN_SYS].[dbo].[W_M_CheckingPlan]  \n";
            if (!string.IsNullOrEmpty(condition))
            {
                strQry += " where " + condition;
            }
            conn = new CmCn();
            try
            {
                return conn.ExcuteDataTable(strQry);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable Load_W_M_CheckingPlanDetail(string need_infor, string condition)
        {
            string strQry = "select ";
            if (!string.IsNullOrEmpty(need_infor))
            {
                strQry += need_infor;
            }
            else
            {
                strQry += " * \n";
            }
            strQry += " FROM [HVN_SYS].[dbo].[W_M_CheckingPlanDetail]  \n";
            if (!string.IsNullOrEmpty(condition))
            {
                strQry += " where " + condition;
            }
            conn = new CmCn();
            try
            {
                return conn.ExcuteDataTable(strQry);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable Load_W_M_IssueLabel(string need_infor, string condition)
        {
            string strQry = "select ";
            if (!string.IsNullOrEmpty(need_infor))
            {
                strQry += need_infor;
            }
            else
            {
                strQry += " * \n";
            }
            strQry += " FROM [HVN_SYS].[dbo].[W_M_IssueLabel]  \n";
            if (!string.IsNullOrEmpty(condition))
            {
                strQry += " where " + condition;
            }
            conn = new CmCn();
            try
            {
                return conn.ExcuteDataTable(strQry);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable Load_PR_Detail(string need_infor, string condition)
        {
            string strQry = "select ";
            if (!string.IsNullOrEmpty(need_infor))
            {
                strQry += need_infor;
            }
            else
            {
                strQry += " * \n";
            }
            strQry += " FROM [HVN_SYS].[dbo].[PUR_PRDetail]  \n";
            if (!string.IsNullOrEmpty(condition))
            {
                strQry += " where " + condition;
            }
            conn = new CmCn();
            try
            {
                return conn.ExcuteDataTable(strQry);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable Load_PUR_PR(string need_infor, string condition)
        {
            string strQry = "select ";
            if (!string.IsNullOrEmpty(need_infor))
            {
                strQry += need_infor;
            }
            else
            {
                strQry += " * \n";
            }
            strQry += " FROM [HVN_SYS].[dbo].[PUR_PR]  \n";
            if (!string.IsNullOrEmpty(condition))
            {
                strQry += " where " + condition;
            }
            conn = new CmCn();
            try
            {
                return conn.ExcuteDataTable(strQry);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable Load_PUR_MasterListItem_Change(string need_infor, string condition)
        {
            string strQry = "select ";
            if (!string.IsNullOrEmpty(need_infor))
            {
                strQry += need_infor;
            }
            else
            {
                strQry += " * \n";
            }
            strQry += " FROM [HVN_SYS].[dbo].[PUR_MasterListItem_Change]  \n";
            if (!string.IsNullOrEmpty(condition))
            {
                strQry += " where " + condition;
            }
            conn = new CmCn();
            try
            {
                return conn.ExcuteDataTable(strQry);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable Load_PUR_PO(string need_infor, string condition)
        {
            string strQry = "select ";
            if (!string.IsNullOrEmpty(need_infor))
            {
                strQry += need_infor;
            }
            else
            {
                strQry += " * \n";
            }
            strQry += " FROM [HVN_SYS].[dbo].[PUR_PO]  \n";
            if (!string.IsNullOrEmpty(condition))
            {
                strQry += " where " + condition;
            }
            conn = new CmCn();
            try
            {
                return conn.ExcuteDataTable(strQry);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable Load_PUR_VS(string need_infor, string condition)
        {
            string strQry = "select ";
            if (!string.IsNullOrEmpty(need_infor))
            {
                strQry += need_infor;
            }
            else
            {
                strQry += " * \n";
            }
            strQry += " FROM [HVN_SYS].[dbo].[PUR_VS]  \n";
            if (!string.IsNullOrEmpty(condition))
            {
                strQry += " where " + condition;
            }
            conn = new CmCn();
            try
            {
                return conn.ExcuteDataTable(strQry);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable Load_MasterListProduct(string need_infor, string condition)
        {
            string strQry = "select ";
            if (!string.IsNullOrEmpty(need_infor))
            {
                strQry += need_infor;
            }
            else
            {
                strQry += " * \n";
            }
            strQry += " FROM [HVN_SYS].[dbo].[P_MasterListProduct]  \n";
            if (!string.IsNullOrEmpty(condition))
            {
                strQry += " where " + condition + " \n";
            }
            conn = new CmCn();
            try
            {
                return conn.ExcuteDataTable(strQry);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable Load_MasterListLine(string need_infor, string line_id)
        {
            string strQry = "select ";
            if (!string.IsNullOrEmpty(need_infor))
            {
                strQry += need_infor;
            }
            else
            {
                strQry += " [line_name] \n";
                strQry += ",[des_en] \n";
                strQry += ",[des_vn] \n";
            }
            strQry += " FROM [HVN_SYS].[dbo].[P_MasterListLine]  \n";
            if (!string.IsNullOrEmpty(line_id))
            {
                strQry += " where line_id='" + line_id + "' \n";
            }
            conn = new CmCn();
            try
            {
                return conn.ExcuteDataTable(strQry);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable Load_Monitor_Issue(string need_infor, string condition)
        {
            string strQry = "select ";
            if (!string.IsNullOrEmpty(need_infor))
            {
                strQry += need_infor;
            }
            else
            {
                strQry += " * \n";
            }
            strQry += " FROM [HVN_SYS].[dbo].[M_MonitorIssue]  \n";
            if (!string.IsNullOrEmpty(condition))
            {
                strQry += " where " + condition;
            }
            conn = new CmCn();
            try
            {
                return conn.ExcuteDataTable(strQry);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable Load_W_MasterList_Material(string need_infor, string condition)
        {
            string strQry = "select ";
            if (!string.IsNullOrEmpty(need_infor))
            {
                strQry += need_infor;
            }
            else
            {
                strQry += " * \n";
            }
            strQry += " FROM [HVN_SYS].[dbo].[W_MasterList_Material]  \n";
            if (!string.IsNullOrEmpty(condition))
            {
                strQry += " where " + condition;
            }
            conn = new CmCn();
            try
            {
                return conn.ExcuteDataTable(strQry);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable Load_W_MasterList_Rubber(string need_infor, string condition)
        {
            string strQry = "select ";
            if (!string.IsNullOrEmpty(need_infor))
            {
                strQry += need_infor;
            }
            else
            {
                strQry += " * \n";
            }
            strQry += " FROM [HVN_SYS].[dbo].[W_MasterList_Rubber]  \n";
            if (!string.IsNullOrEmpty(condition))
            {
                strQry += " where " + condition;
            }
            conn = new CmCn();
            try
            {
                return conn.ExcuteDataTable(strQry);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void Create_Grid(DevExpress.XtraGrid.Views.Grid.GridView Grid_name, string dgv_name)
        {
            DataTable dt;
            conn = new CmCn();
            string strQry = "Select * from ADM_DataGridInfor where dgv_name='" + dgv_name + "'";
            dt = conn.ExcuteDataTable(strQry);
            foreach (DataRow row in dt.Rows)
            {
                string Caption = row["column_field"].ToString().ToUpper();
                string FieldName = row["column_field"].ToString();
                string Name = row["dgv_column"].ToString();
                GridColumn myCol = new GridColumn() { Caption = Caption, Visible = true, FieldName = FieldName, Name = Name };
                Grid_name.Columns.Add(myCol);
            }
        }
        public void Load_Data_Grid(DevExpress.XtraGrid.GridControl dgv_name, string table_source, string condition)
        {
            DataTable dt;
            conn = new CmCn();
            string strQry = "Select * from ADM_DataGridInfor where dgv_name='" + dgv_name.Name + "'";
            dt = conn.ExcuteDataTable(strQry);
            string strQry2 = "select ";
            int i = 1;
            foreach (DataRow row in dt.Rows)
            {
                if (i == 1)
                {
                    strQry2 += row["column_field"].ToString();
                }
                else
                {
                    strQry2 += "," + row["column_field"].ToString();
                }
                i++;
            }
            strQry2 += "\n from " + table_source;
            if (!string.IsNullOrEmpty(condition))
            {
                strQry2 += "\n where  " + condition;
            }
            dgv_name.DataSource = conn.ExcuteDataTable(strQry2);
        }
        public int Generate_M_Label_code()
        {
            string Qry = "SELECT MAX(label_code) FROM P_Label ";
            conn = new CmCn();
            DataTable dt = conn.ExcuteDataTable(Qry);
            if (dt.Rows[0][0].ToString() != "")
            {
                string max_value = dt.Rows[0][0].ToString().Substring(2, 5);
                return int.Parse(max_value) + 1;
            }
            else
            {
                return 10001;
            }
        }
        public string Load_Line_name(string Line_ID)
        {
            string strQry = "select line_name from P_MasterListLine where line_id='" + Line_ID + "' ";
            conn = new CmCn();
            try
            {
                return conn.ExcuteString(strQry);
            }
            catch (Exception)
            {
                return "ERR";
            }
        }
        public DataTable Load_time_plan()
        {
            string strQry = " select a.shift_name,a.plus_hour_start,a.plus_hour_end \n";
            strQry += " from P_MasterListShift a, (select top 1 shift from PL_PlanFG where start_time < GETDATE() and end_time > GETDATE()) b \n";
            strQry += " where a.shift_name = b.shift \n";
            conn = new CmCn();
            try
            {
                return conn.ExcuteDataTable(strQry);
            }
            catch (Exception)
            {
                return null;
            }
        }
        public string Load_status_of_Line(string Line_name)
        {
            string strQry = "select issue_name from M_MonitorIssue \n";
            strQry += "where location = '" + Line_name + "' and status = 'open' \n";
            try
            {
                return string.IsNullOrEmpty(conn.ExcuteString(strQry)) ? "" : conn.ExcuteString(strQry);
            }
            catch (Exception)
            {
                return "NG";
            }
        }
        public string Load_downtime_of_Line(string Line_name)
        {
            string strQry = "select convert(time,getdate()-start_time) as downtime from M_MonitorIssue  \n";
            strQry += "where location = '" + Line_name + "' and status = 'open' \n";
            try
            {
                return string.IsNullOrEmpty(conn.ExcuteString(strQry)) ? "33:33" : conn.ExcuteString(strQry).Substring(0, 5);
            }
            catch (Exception)
            {
                return "33:33";
            }
        }
        public string Load_Qty(DateTime Begin_time, DateTime Finish_time, string product_code)
        {
            if (!string.IsNullOrEmpty(product_code))
            {
                string strQry = "select sum(a.product_quantity) from P_Label a \n  ";
                strQry += "where a.scanned_date >'" + Begin_time.ToString("yyyy-MM-dd HH:mm:ss");
                strQry += "'and a.scanned_date <'" + Finish_time.ToString("yyyy-MM-dd HH:mm:ss");
                strQry += "' and a.product_customer_code='" + product_code + "' ";
                conn = new CmCn();
                try
                {
                    return string.IsNullOrEmpty(conn.ExcuteString(strQry)) ? "0" : conn.ExcuteString(strQry);
                }
                catch (Exception)
                {
                    return "0";
                }
            }
            else
            {
                return "0";
            }
        }
        public void Print_Pallet_Label(string Pallet_no, string Operator)
        {
            //GET INFORMATION
            string strQry = "select label_code,product_customer_code,product_quantity,lot_no from P_Label where pallet_no=N'" + Pallet_no + "' and place not in ('Shipped')";
            conn = new CmCn();
            DataTable dt = conn.ExcuteDataTable(strQry);
            List<P_Label_Entity> List_Box = new List<P_Label_Entity>();
            int stt = 1;
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    P_Label_Entity item = new P_Label_Entity();
                    item.Stt = stt.ToString();
                    item.Label_code = row["label_code"].ToString();
                    item.Product_customer_code = row["product_customer_code"].ToString();
                    item.Product_quantity = int.Parse(row["product_quantity"].ToString());
                    item.Lot_no = row["lot_no"].ToString();
                    List_Box.Add(item);
                    stt++;
                }
            }
            else
            {
                return;
            }
            //----------------- PRINT LABEL
            Excel.Application app;
            Excel.Workbook workbook;
            Excel.Sheets mWorkSheets;
            Excel.Worksheet worksheet;
            string file_name = "PACKING_PALLET";
            string pathExcel = @"C:\HVN_SYS\01.Format_Excel\PACKING_PALLET.xlsm";
            if (System.IO.File.Exists(pathExcel))
            {
                app = new Excel.Application();
                app.Visible = false;
                app.DisplayAlerts = false;
                workbook = app.Workbooks.Open(pathExcel, 0, false, 5, "", "", false, Excel.XlPlatform.xlWindows, "", true, false, 0, true, false, false);
                mWorkSheets = workbook.Worksheets;
                worksheet = (Excel.Worksheet)mWorkSheets.get_Item("Sheet1");
                worksheet = workbook.ActiveSheet;
                worksheet.Cells[6, 3] = Operator;
                worksheet.Cells[7, 3] = Pallet_no;
                //--Hien thi thong tin chung Packing
                var packing_infor = List_Box.GroupBy(x => x.Product_customer_code).Select(x => x);
                int i = 11;
                foreach (var item in packing_infor)
                {
                    worksheet.Cells[i, 1] = item.Key.ToString();
                    int qty = 0;
                    foreach (P_Label_Entity row in item)
                    {
                        qty += row.Product_quantity;
                    }
                    worksheet.Cells[i, 4] = qty.ToString();
                    i++;
                }
                //--Hien thi thong tin chi tiet cac thung
                int j = 19;
                foreach (P_Label_Entity item in List_Box)
                {
                    worksheet.Cells[j, 1] = item.Stt.ToString();
                    worksheet.Cells[j, 2] = item.Label_code;
                    worksheet.Cells[j, 3] = item.Product_customer_code;
                    worksheet.Cells[j, 4] = item.Product_quantity.ToString();
                    worksheet.Cells[j, 5] = item.Lot_no;
                    j++;
                }
                worksheet.Unprotect();
                if (File.Exists("" + file_name.Replace("/", "-") + ".xlsm"))
                {
                    File.Delete("" + file_name.Replace("/", "-") + ".xlsm");
                }
                app.DisplayAlerts = false;
                workbook.SaveAs("" + file_name.Replace("/", "-") + ".xlsm", Excel.XlFileFormat.xlAddIn,
                          Type.Missing, Type.Missing, Type.Missing, Type.Missing, Excel.XlSaveAsAccessMode.xlNoChange,
                          Type.Missing, Type.Missing, Type.Missing,
                          Type.Missing, Type.Missing);
                var printerSettings = new PrinterSettings();
                workbook.PrintOutEx(Type.Missing, Type.Missing, Type.Missing, Type.Missing, printerSettings.PrinterName, Type.Missing, Type.Missing, Type.Missing);
                workbook.Close();
                app.Quit();
            }
        }
        public void Print_Location_Label(string location, string Operator)
        {
            //GET INFORMATION
            string strQry = "select label_code,product_customer_code,product_quantity,lot_no from P_Label \n where wh_location=N'" + location + "' and place =N'FG Zone'\n order by wh_locate_date";
            conn = new CmCn();
            DataTable dt = conn.ExcuteDataTable(strQry);
            List<P_Label_Entity> List_Box = new List<P_Label_Entity>();
            int stt = 1;
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    P_Label_Entity item = new P_Label_Entity();
                    item.Stt = stt.ToString();
                    item.Label_code = row["label_code"].ToString();
                    item.Product_customer_code = row["product_customer_code"].ToString();
                    item.Product_quantity = int.Parse(row["product_quantity"].ToString());
                    List_Box.Add(item);
                    stt++;
                }
            }
            else
            {
                return;
            }
            //----------------- PRINT LABEL
            Excel.Application app;
            Excel.Workbook workbook;
            Excel.Sheets mWorkSheets;
            Excel.Worksheet worksheet;
            string pathExcel = @"C:\HVN_SYS\01.Format_Excel\Location_label.xlsm";
            if (System.IO.File.Exists(pathExcel))
            {
                app = new Excel.Application();
                app.Visible = false;
                app.DisplayAlerts = false;
                workbook = app.Workbooks.Open(pathExcel, 0, false, 5, "", "", false, Excel.XlPlatform.xlWindows, "", true, false, 0, true, false, false);
                mWorkSheets = workbook.Worksheets;
                worksheet = (Excel.Worksheet)mWorkSheets.get_Item("Sheet1");
                worksheet = workbook.ActiveSheet;
                worksheet.Cells[6, 2] = Operator;
                worksheet.Cells[7, 2] = location;
                worksheet.Cells[8, 2] = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                //--Hien thi thong tin chung location
                var packing_infor = List_Box.GroupBy(x => x.Product_customer_code).Select(x => x);
                int i = 11;
                foreach (var item in packing_infor)
                {
                    worksheet.Cells[i, 1] = item.Key.ToString();
                    int qty = 0;
                    int qty_box = 0;
                    foreach (P_Label_Entity row in item)
                    {
                        qty += row.Product_quantity;
                        qty_box++;
                    }
                    worksheet.Cells[i, 3] = qty.ToString();
                    worksheet.Cells[i, 4] = qty_box.ToString();
                    i++;
                }
                //--Hien thi thong tin chi tiet cac thung
                int j = 4;
                foreach (P_Label_Entity item in List_Box)
                {
                    if (j < 43)
                    {
                        worksheet.Cells[j, 6] = item.Stt.ToString();
                        worksheet.Cells[j, 7] = item.Label_code;
                        worksheet.Cells[j, 8] = item.Product_customer_code;
                        worksheet.Cells[j, 9] = item.Product_quantity.ToString();
                    }
                    else
                    {
                        worksheet.Cells[j - 24, 10] = item.Stt.ToString();
                        worksheet.Cells[j - 24, 11] = item.Label_code;
                        worksheet.Cells[j - 24, 12] = item.Product_customer_code;
                        worksheet.Cells[j - 24, 13] = item.Product_quantity.ToString();
                    }
                    j++;
                }
                worksheet.Unprotect();
                app.DisplayAlerts = false;
                var printerSettings = new PrinterSettings();
                workbook.PrintOutEx(Type.Missing, Type.Missing, Type.Missing, Type.Missing, printerSettings.PrinterName, Type.Missing, Type.Missing, Type.Missing);
                workbook.Close();
                app.Quit();
            }
        }
        public void Print_HR_Visitor_Registration(DataTable dt)
        {
            Excel.Application app;
            Excel.Workbook workbook;
            Excel.Sheets mWorkSheets;
            Excel.Worksheet worksheet;
            string pathExcel = @"C:\HVN_SYS\01.Format_Excel\HRVisitorRegistrationForm.xlsx";
            if (System.IO.File.Exists(pathExcel))
            {
                app = new Excel.Application();
                app.Visible = false;
                app.DisplayAlerts = false;
                workbook = app.Workbooks.Open(pathExcel, 0, false, 5, "", "", false, Excel.XlPlatform.xlWindows, "", true, false, 0, true, false, false);
                mWorkSheets = workbook.Worksheets;
                worksheet = (Excel.Worksheet)mWorkSheets.get_Item("Sheet1");
                worksheet = workbook.ActiveSheet;
                int i = 11;
                foreach (DataRow item in dt.Rows)
                {
                    worksheet.Cells[i, 1] = item["reg_date"].ToString();
                    worksheet.Cells[i, 2] = item["In_Time"].ToString();
                    worksheet.Cells[i, 3] = item["Out_time"].ToString();
                    worksheet.Cells[i, 4] = item["visitor"].ToString();
                    worksheet.Cells[i, 5] = item["number_visitor"].ToString();
                    worksheet.Cells[i, 6] = item["visitor_company"].ToString();
                    worksheet.Cells[i, 7] = item["id_no"].ToString();
                    worksheet.Cells[i, 8] = item["carry_on_item"].ToString();
                    worksheet.Cells[i, 17] = item["training"].ToString();
                    worksheet.Cells[i, 18] = item["registor"].ToString();
                    worksheet.Cells[i, 19] = item["visitor"].ToString();
                    worksheet.Rows[i].AutoFit();
                    i++;
                }
                worksheet.Unprotect();
                app.DisplayAlerts = false;
                var printerSettings = new PrinterSettings();
                workbook.PrintOutEx(Type.Missing, Type.Missing, Type.Missing, Type.Missing, printerSettings.PrinterName, Type.Missing, Type.Missing, Type.Missing);
                workbook.Close();
                app.Quit();
            }
        }
        public DataTable Load_Current_Plan()
        {
            string strQry = "select a.plan_date,a.shift,a.number_operator,a.line_no,a.customer_product_code,a.start_time,a.end_time,a.product_code,b.line_id,a.target,c.standard_time_FG \n";
            strQry += " from PL_PlanFG a, P_MasterListLine b, P_MasterListProduct c \n";
            strQry += " where start_time<GETDATE() and end_time> GETDATE() and a.line_no = b.line_name and a.product_code=c.product_code \n";
            conn = new CmCn();
            try
            {
                return conn.ExcuteDataTable(strQry);
            }
            catch (Exception)
            {
                return null;
            }
        }
        public DataTable ReadExcelFile(string sheetName, string path)
        {
            using (OleDbConnection conn = new OleDbConnection())
            {
                DataTable dt = new DataTable();
                string Import_FileName = path;
                string fileExtension = Path.GetExtension(Import_FileName);
                if (fileExtension == ".xls")
                    conn.ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Import_FileName + ";" + "Extended Properties='Excel 8.0;HDR=YES;'";
                if (fileExtension == ".xlsx")
                    conn.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Import_FileName + ";" + "Extended Properties='Excel 12.0 Xml;HDR=YES;'";
                using (OleDbCommand comm = new OleDbCommand())
                {
                    comm.CommandText = "Select * from [" + sheetName + "$]";

                    comm.Connection = conn;

                    using (OleDbDataAdapter da = new OleDbDataAdapter())
                    {
                        da.SelectCommand = comm;
                        da.Fill(dt);
                        return dt;
                    }
                }
            }
        }
        public void Export_Excel(DevExpress.XtraGrid.GridControl Grid_name)
        {
            SaveFileDialog SaveDialog = new SaveFileDialog();
            SaveDialog.Filter = "Excel (.xlsx)|*.xlsx";
            if (SaveDialog.ShowDialog() != DialogResult.Cancel)
            {
                string ExportFilePath = SaveDialog.FileName;
                //Using System.IO;
                string FileExtenstion = Path.GetExtension(ExportFilePath);
                switch (FileExtenstion)
                {
                    case ".xlsx":
                        Grid_name.ExportToXlsx(ExportFilePath);
                        break;
                    default:
                        break;
                }
                if (File.Exists(ExportFilePath))
                {
                    try
                    {
                        //Try to open the file and let windows decide how to open it.
                        System.Diagnostics.Process.Start(ExportFilePath);
                    }
                    catch
                    {
                        String msg = "The file could not be opened." + Environment.NewLine + Environment.NewLine + "Path: " + ExportFilePath;
                        MessageBox.Show(msg, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    String msg = "The file could not be saved." + Environment.NewLine + Environment.NewLine + "Path: " + ExportFilePath;
                    MessageBox.Show(msg, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        public void Export_Excel_Pivot(DevExpress.XtraPivotGrid.PivotGridControl Pivotgrid_name)
        {
            SaveFileDialog SaveDialog = new SaveFileDialog();
            SaveDialog.Filter = "Excel (.xlsx)|*.xlsx";
            if (SaveDialog.ShowDialog() != DialogResult.Cancel)
            {
                string ExportFilePath = SaveDialog.FileName;
                //Using System.IO; 
                string FileExtenstion = Path.GetExtension(ExportFilePath);
                switch (FileExtenstion)
                {
                    case ".xlsx":
                        Pivotgrid_name.ExportToXlsx(ExportFilePath);
                        break;
                    default:
                        break;
                }
                if (File.Exists(ExportFilePath))
                {
                    try
                    {
                        //Try to open the file and let windows decide how to open it.
                        System.Diagnostics.Process.Start(ExportFilePath);
                    }
                    catch
                    {
                        String msg = "The file could not be opened." + Environment.NewLine + Environment.NewLine + "Path: " + ExportFilePath;
                        MessageBox.Show(msg, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    String msg = "The file could not be saved." + Environment.NewLine + Environment.NewLine + "Path: " + ExportFilePath;
                    MessageBox.Show(msg, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        public DataTable ReadDataFromExcelFile(string pathFile, string sheetName)
        {
            DataTable dt = new DataTable();
            string connectionString = "";
            string abc = Path.GetExtension(pathFile);
            connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + pathFile.Trim() + ";Excel 12.0;HDR=YES;IMEX=1";
            OleDbConnection oledbConn = new OleDbConnection(connectionString);
            try
            {
                // Mở kết nối
                oledbConn.Open();
                OleDbCommand cmd = new OleDbCommand("SELECT * FROM [" + sheetName + "$]", oledbConn);
                OleDbDataAdapter oleda = new OleDbDataAdapter();
                oleda.SelectCommand = cmd;
                DataSet ds = new DataSet();

                oleda.Fill(ds);

                dt = ds.Tables[0];
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                oledbConn.Close();
            }
            return dt;
        }
        #region HUTCHINSON_KPI_PLAN
        public DataTable KPI_Load_KPI_PLANT(string condition)
        {
            string strQry = "select * from KPI_PlantDashBoard \n";
            if (!string.IsNullOrEmpty(condition))
            {
                strQry += " where " + condition;
            }
            conn = new CmCn();
            try
            {
                return conn.ExcuteDataTable(strQry);
            }
            catch (Exception)
            {
                return null;
            }
        }
        public DataTable KPI_Load_KPI_Incident(string field, string condition)
        {
            string strQry = "select ";
            if (string.IsNullOrEmpty(field))
            {
                strQry += "* from KPI_IncidentMonitoring \n";
            }
            else
            {
                strQry += field + " from KPI_IncidentMonitoring \n";
            }

            if (!string.IsNullOrEmpty(condition))
            {
                strQry += " where " + condition;
            }
            conn = new CmCn();
            try
            {
                return conn.ExcuteDataTable(strQry);
            }
            catch (Exception)
            {
                return null;
            }
        }
        public DataTable KPI_Load_KPI_Action(string field, string condition)
        {
            string strQry = "select ";
            if (string.IsNullOrEmpty(field))
            {
                strQry += "* from KPI_ActionMonitoring \n";
            }
            else
            {
                strQry += field + " from KPI_ActionMonitoring \n";
            }

            if (!string.IsNullOrEmpty(condition))
            {
                strQry += " where " + condition;
            }
            conn = new CmCn();
            try
            {
                return conn.ExcuteDataTable(strQry);
            }
            catch (Exception)
            {
                return null;
            }
        }
        public void KPI_Insert_Incident(KPI_IncidentMonitoring kPI_Incident)
        {
            string strQry = "";
            strQry += "insert into KPI_IncidentMonitoring (inc_name,inc_type,inc_theme,inc_des,inc_level,author,location,created_time,created_for" +
                ",isAction,last_user_commit,is8D,inc_customer,inc_status,sort_time,cost,sort_ext_cost,sort_int_cost,trans_cost,customer_claim_cost,other_cost,extrus_lot,finishing_lot,image_link)  \n";
            strQry += "values (N'" + kPI_Incident.Inc_name + "',N'" + kPI_Incident.Inc_type + "',N'" + kPI_Incident.Inc_theme + "'";
            strQry += ",N'" + kPI_Incident.Inc_des + "',N'" + kPI_Incident.Inc_level + "',N'" + kPI_Incident.Author + "',N'" + kPI_Incident.Location + "'";
            strQry += ",getdate(),N'" + kPI_Incident.Created_for.ToString("yyyy-MM-dd HH:mm:ss") + "',N'" + kPI_Incident.IsAction + "',N'" + kPI_Incident.Last_user_commit + "',N'" + kPI_Incident.Is8D + "',N'" + kPI_Incident.Inc_customer + "',N'" + kPI_Incident.Inc_status + "',N'" + kPI_Incident.Sorting_time + "',N'" + kPI_Incident.Cost + "',N'" + kPI_Incident.Sort_ext_cost + "',N'" + kPI_Incident.Sort_int_cost + "',N'" + kPI_Incident.Trans_cost + "',N'" + kPI_Incident.Customer_claim_cost + "',N'" + kPI_Incident.Other_cost + "',N'" + kPI_Incident.Extrus_lot.ToString("yyyy-MM-dd") + "',N'" + kPI_Incident.Finishing_lot.ToString("yyyy-MM-dd") + "',N'" + kPI_Incident.Image_link + "') \n";
            conn = new CmCn();
            try
            {
                conn.ExcuteQry(strQry);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void KPI_Update_Incident(KPI_IncidentMonitoring kPI_Incident)
        {
            string strQry = "update KPI_IncidentMonitoring set inc_type='" + kPI_Incident.Inc_type + "',inc_name='" + kPI_Incident.Inc_name + "',inc_theme='" + kPI_Incident.Inc_theme + "',inc_des='" + kPI_Incident.Inc_des + "'";
            strQry += ",inc_level='" + kPI_Incident.Inc_level + "',location='" + kPI_Incident.Location + "',update_time=getdate(),created_for=N'" + kPI_Incident.Created_for.ToString("yyyy-MM-dd hh:mm:ss") + "',last_user_commit=N'" + kPI_Incident.Last_user_commit + "',is8D=N'" + kPI_Incident.Is8D + "',inc_status=N'" + kPI_Incident.Inc_status + "',inc_customer=N'" + kPI_Incident.Inc_customer + "',cost=N'" + kPI_Incident.Cost + "',sort_time=N'" + kPI_Incident.Sorting_time + "',sort_ext_cost=N'" + kPI_Incident.Sort_ext_cost + "',sort_int_cost=N'" + kPI_Incident.Sort_int_cost + "',trans_cost=N'" + kPI_Incident.Trans_cost + "',customer_claim_cost=N'" + kPI_Incident.Customer_claim_cost + "',other_cost=N'" + kPI_Incident.Other_cost + "',extrus_lot=N'" + kPI_Incident.Extrus_lot + "',finishing_lot=N'" + kPI_Incident.Finishing_lot + "',image_link=N'" + kPI_Incident.Image_link + "' \n";
            strQry += " where check_id=N'" + kPI_Incident.Check_id + "' \n";
            conn = new CmCn();
            try
            {
                conn.ExcuteQry(strQry);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void KPI_Insert_Action(KPI_ActionMonitoring_Entity kPI_Action)
        {
            string strQry = "insert into KPI_ActionMonitoring (act_name,act_des,inc_name,priority,planned_for,assigned_user,location,status,created_date,last_user_commit,last_time_commit,theme)  \n";
            strQry += "values (N'" + kPI_Action.Act_name + "',N'" + kPI_Action.Act_des + "',N'" + kPI_Action.Inc_name + "'";
            strQry += ",N'" + kPI_Action.Priority + "',N'" + kPI_Action.Planned_for.ToString("yyyy-MM-dd") + "',N'" + kPI_Action.Assigned_user + "',N'" + kPI_Action.Location + "'";
            strQry += ",N'" + kPI_Action.Status + "',getdate(),N'" + kPI_Action.Last_user_commit + "',N'" + kPI_Action.Last_time_commit.ToString("yyyy-MM-dd") + "',N'" + kPI_Action.Theme + "') \n";
            strQry += " update KPI_IncidentMonitoring set isAction='Yes' where inc_name='" + kPI_Action.Inc_name + "' ";
            conn = new CmCn();
            try
            {
                conn.ExcuteQry(strQry);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void KPI_Update_Action(KPI_ActionMonitoring_Entity kPI_Action)
        {
            string strQry = "Update KPI_ActionMonitoring set act_des=N'" + kPI_Action.Act_des + "',inc_name=N'" + kPI_Action.Inc_name + "'";
            strQry += ",priority=N'" + kPI_Action.Priority + "',planned_for=N'" + kPI_Action.Planned_for.ToString("yyyy-MM-dd") + "',assigned_user=N'" + kPI_Action.Assigned_user + "'";
            strQry += ",[location]=N'" + kPI_Action.Location + "',last_user_commit=N'" + kPI_Action.Last_user_commit + "',last_time_commit=getdate() \n";
            strQry += "where check_id=N'" + kPI_Action.Check_id + "'";
            conn = new CmCn();
            try
            {
                conn.ExcuteQry(strQry);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void Insert_W_M_IssueLabel(W_M_IssueLabel_Entity item)
        {
            string strQry = "insert into W_M_IssueLabel (whmi_code,m_name,product_customer_code,quantity,lot_no,issue_date,p_shift,p_line,weight,supply_date,m_doc_id) \n";
            strQry += "select N'" + item.Whmi_code + "',N'" + item.M_name + "',N'" + item.Product_customer_code + "',N'" + item.Quantity + "',N'" + item.Lot_no.ToString("yyyy-MM-dd") + "',getdate(),N'" + item.P_shift + "',N'" + item.P_line + "',N'" + item.Weight + "',N'" + item.Supply_date.ToString("yyyy-MM-dd") + "',N'" + item.M_doc_id + "'\n";
            conn = new CmCn();
            try
            {
                conn.ExcuteQry(strQry);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void Insert_W_M_IssueLabel_2(W_M_IssueLabel_Entity item, string kind_update)
        {
            string strQry = "insert into W_M_IssueLabel (whmi_code,m_name,product_customer_code,quantity,lot_no,issue_date,p_shift,p_line,weight,supply_date,whmr_code,m_doc_id) \n";
            strQry += "select N'" + item.Whmi_code + "',N'" + item.M_name + "',N'" + item.Product_customer_code + "',N'" + item.Quantity + "',N'" + item.Lot_no.ToString("yyyy-MM-dd") + "',getdate(),N'" + item.P_shift + "',N'" + item.P_line + "',N'" + item.Weight + "',N'" + item.Supply_date.ToString("yyyy-MM-dd") + "',N'" + item.Whmr_code + "',N'" + item.M_doc_id + "'\n";
            if (kind_update == "Update special")
            {
                strQry += "update W_M_ReceiveLabel set quantity=N'0' where whmr_code=N'" + item.Whmr_code + "'\n";
            }
            else
            {
                strQry += "update W_M_ReceiveLabel set quantity=quantity-" + item.Quantity + " where whmr_code=N'" + item.Whmr_code + "'";
            }
            strQry += "\nInsert into W_M_HistoryOfTransaction (whmr_code,m_name,quantity,lot_no,[transaction],input_time,PIC,whmi_code,m_note) \n ";
            strQry += " select N'" + item.Whmr_code + "',N'" + item.M_name + "',N'" + item.Quantity + "',N'" + item.Lot_no.ToString("yyyy-MM-dd") + "' \n ";
            strQry += " ,N'" + item.Transation + "',getdate(),N'" + item.Pic + "',N'" + item.Whmi_code + "',N'" + item.Note + "' \n ";
            conn = new CmCn();
            try
            {
                conn.ExcuteQry(strQry);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void Return_W_M_IssueLabel_2(W_M_IssueLabel_Entity item, float new_quantity)
        {
            string strQry = "Insert into W_M_HistoryOfTransaction (whmr_code,m_name,quantity,lot_no,[transaction],input_time,PIC,whmi_code,m_note) \n ";
            strQry += " select N'" + item.Whmr_code + "',N'" + item.M_name + "',N'" + item.Quantity + "',N'" + item.Lot_no.ToString("yyyy-MM-dd") + "' \n ";
            strQry += " ,N'" + item.Transation + "',getdate(),N'" + item.Pic + "',N'" + item.Whmi_code + "',N'" + item.Note + "' \n ";
            strQry += "Update W_M_ReceiveLabel set quantity=N'" + new_quantity.ToString() + "',pic_return_pd=N'" + item.Pic + "',time_return_pd=getdate() where whmr_code=N'" + item.Whmr_code + "'";
            conn = new CmCn();
            try
            {
                conn.ExcuteQry(strQry);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void Insert_W_M_ReceiveLabel(W_M_ReceiveLabel_Entity item)
        {
            string strQry = "insert into W_M_ReceiveLabel ([whmr_code],[m_name],[quantity],[created_time],[created_user],[rm_doc_id]) \n";
            strQry += "select N'" + item.Whmr_code + "',N'" + item.M_name + "',N'" + item.Quantity + "',getdate(),N'" + item.Created_user + "',N'" + item.Rm_doc_id + "'\n";
            strQry += "insert into W_M_HistoryOfTransaction([whmr_code],[m_name],[quantity],[transaction],[input_time],[PIC]) \n";
            strQry += "select N'" + item.Whmr_code + "',N'" + item.M_name + "',N'" + item.Quantity + "',N'Create label',getdate(),N'" + item.Created_user + "'\n";
            conn = new CmCn();
            try
            {
                conn.ExcuteQry(strQry);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void Insert_W_M_ReceiveLabel_Stock(W_M_ReceiveLabel_Entity item)
        {
            string strQry = "insert into W_M_ReceiveLabel ([whmr_code],[m_name],[quantity],[created_time],[created_user],[rm_doc_id],lot_no,time_qc_check,wh_okng,qc_okng) \n";
            strQry += "select N'" + item.Whmr_code + "',N'" + item.M_name + "',N'" + item.Quantity + "',getdate(),N'" + item.Created_user + "',N'" + item.Rm_doc_id + "',N'" + item.Lot_no.ToString("yyyy-MM-dd") + "',getdate(),N'OK',N'OK'\n";
            strQry += "insert into W_M_HistoryOfTransaction([whmr_code],[m_name],[quantity],[transaction],[input_time],[PIC],lot_no) \n";
            strQry += "select N'" + item.Whmr_code + "',N'" + item.M_name + "',N'" + item.Quantity + "',N'Create Stock label',getdate(),N'" + item.Created_user + "',N'" + item.Lot_no.ToString("yyyy-MM-dd") + "'\n";
            conn = new CmCn();
            try
            {
                conn.ExcuteQry(strQry);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void Print_W_M_IssueLabel(W_M_IssueLabel_Entity item)
        {
            Excel.Application app;
            Excel.Workbook workbook;
            Excel.Sheets mWorkSheets;
            Excel.Worksheet worksheet;
            string pathExcel = @"C:\HVN_SYS\01.Format_Excel\WHMI_Label.xlsm";
            if (System.IO.File.Exists(pathExcel))
            {
                app = new Excel.Application();
                app.Visible = false;
                app.DisplayAlerts = false;
                workbook = app.Workbooks.Open(pathExcel, 0, false, 5, "", "", false, Excel.XlPlatform.xlWindows, "", true, false, 0, true, false, false);
                mWorkSheets = workbook.Worksheets;
                worksheet = (Excel.Worksheet)mWorkSheets.get_Item("Sheet1");
                worksheet = workbook.ActiveSheet;
                worksheet.Cells[1, 2] = item.Product_customer_code;
                worksheet.Cells[2, 2] = item.M_name;
                worksheet.Cells[3, 2] = item.Quantity;
                worksheet.Cells[4, 2] = item.Lot_no.ToString("dd/MM/yyyy");
                worksheet.Cells[5, 2] = item.Supply_date.ToString("dd/MM/yyyy");
                worksheet.Cells[6, 2] = item.P_shift;
                worksheet.Cells[7, 2] = item.P_line;
                worksheet.Cells[8, 2] = item.Weight + " G";
                worksheet.Cells[1, 3] = item.Whmi_code;
                worksheet.Unprotect();
                app.DisplayAlerts = false;
                var printerSettings = new PrinterSettings();
                workbook.PrintOutEx(Type.Missing, Type.Missing, Type.Missing, Type.Missing, printerSettings.PrinterName, Type.Missing, Type.Missing, Type.Missing);
                workbook.Close();
                app.Quit();
            }
        }
        public void Print_W_M_SupplyDocument(List<W_M_IssueDocDetail_Entity> List_Data, bool isPrintResult, DateTime Supply_date, string shift, string file_path)
        {
            Excel.Application app;
            Excel.Workbook workbook;
            Excel.Sheets mWorkSheets;
            Excel.Worksheet worksheet;
            string pathExcel = @"C:\HVN_SYS\01.Format_Excel\WHMI_SupplyDocument.xlsx";
            if (System.IO.File.Exists(pathExcel))
            {
                app = new Excel.Application();
                app.Visible = false;
                app.DisplayAlerts = false;
                workbook = app.Workbooks.Open(pathExcel, 0, false, 5, "", "", false, Excel.XlPlatform.xlWindows, "", true, false, 0, true, false, false);
                mWorkSheets = workbook.Worksheets;
                worksheet = (Excel.Worksheet)mWorkSheets.get_Item("Sheet1");
                worksheet = workbook.ActiveSheet;
                worksheet.Cells[4, 4] = shift;
                worksheet.Cells[4, 7] = Supply_date.ToString("dd/MM/yyyy");
                int i = 6;
                int stt = 1;
                if (List_Data.Count > 10)
                {
                    for (int j = 0; j < List_Data.Count - 10; j++)
                    {
                        worksheet.Range[worksheet.Cells[10, 1], worksheet.Cells[10, 10]].Insert(Type.Missing);
                    }
                }
                foreach (W_M_IssueDocDetail_Entity item in List_Data)
                {
                    worksheet.Cells[i, 1] = stt;
                    worksheet.Cells[i, 2] = item.P_line;
                    worksheet.Cells[i, 3] = item.Product_customer_code;
                    worksheet.Cells[i, 4] = item.M_name;
                    worksheet.Cells[i, 5] = item.M_demand;
                    if (isPrintResult)
                    {
                        worksheet.Cells[i, 6] = item.Actual_qty;
                    }
                    i++;
                    stt++;
                }
                worksheet.Unprotect();
                app.DisplayAlerts = false;
                string filename = @file_path;
                workbook.SaveAs(filename);
                //workbook.SaveAs(filename , Excel.XlFileFormat.xlAddIn,
                //          Type.Missing, Type.Missing, Type.Missing, Type.Missing, Excel.XlSaveAsAccessMode.xlNoChange,
                //          Type.Missing, Type.Missing, Type.Missing,
                //          Type.Missing, Type.Missing);
                //var printerSettings = new PrinterSettings();
                //workbook.PrintOutEx(Type.Missing, Type.Missing, Type.Missing, Type.Missing, printerSettings.PrinterName, Type.Missing, Type.Missing, Type.Missing);
                //workbook.Close();
                app.Quit();
            }
        }
        public void Print_W_M_ReceiveLabel(W_M_ReceiveLabel_Entity item, string kind_label)
        {
            Excel.Application app;
            Excel.Workbook workbook;
            Excel.Sheets mWorkSheets;
            Excel.Worksheet worksheet;
            string pathExcel = @"C:\HVN_SYS\01.Format_Excel\WHMR_Label.xlsm";
            if (System.IO.File.Exists(pathExcel))
            {
                app = new Excel.Application();
                app.Visible = false;
                app.DisplayAlerts = false;
                workbook = app.Workbooks.Open(pathExcel, 0, false, 5, "", "", false, Excel.XlPlatform.xlWindows, "", true, false, 0, true, false, false);
                mWorkSheets = workbook.Worksheets;
                worksheet = (Excel.Worksheet)mWorkSheets.get_Item("Sheet1");
                worksheet = workbook.ActiveSheet;
                worksheet.Cells[1, 2] = item.M_name;
                worksheet.Cells[1, 4] = item.Whmr_code;
                if (kind_label == "QCOK")
                {
                    worksheet.Cells[2, 2] = item.Quantity;
                    worksheet.Cells[3, 2] = item.Lot_no.ToString("dd/MM/yyyy");
                    worksheet.Cells[4, 2] = item.Time_qc_check.ToString("dd/MM/yyyy");
                    worksheet.Cells[6, 1] = "OK";
                }
                else if (kind_label == "QCNG")
                {
                    worksheet.Cells[2, 2] = item.Quantity_ng;
                    worksheet.Cells[3, 2] = item.Lot_no.ToString("dd/MM/yyyy");
                    worksheet.Cells[4, 2] = item.Time_qc_check.ToString("dd/MM/yyyy");
                    worksheet.Cells[6, 1] = "NG";
                }
                else
                {
                    worksheet.Cells[2, 2] = item.Quantity;
                }
                DateTimeFormatInfo dfi = DateTimeFormatInfo.CurrentInfo;
                Calendar myCal = dfi.Calendar;
                string WW = myCal.GetWeekOfYear(item.Created_date, dfi.CalendarWeekRule, dfi.FirstDayOfWeek).ToString();
                string YY = item.Created_date.ToString("yy");
                worksheet.Cells[6, 3] = WW + "/" + YY;
                worksheet.Unprotect();
                app.DisplayAlerts = false;
                var printerSettings = new PrinterSettings();
                workbook.PrintOutEx(Type.Missing, Type.Missing, Type.Missing, Type.Missing, printerSettings.PrinterName, Type.Missing, Type.Missing, Type.Missing);
                workbook.Close();
                app.Quit();
            }
        }
        public void Print_W_M_RubberLabel(W_M_RubberLabel_Entity item, string kind_label)
        {
            Excel.Application app;
            Excel.Workbook workbook;
            Excel.Sheets mWorkSheets;
            Excel.Worksheet worksheet;
            string pathExcel = @"C:\HVN_SYS\01.Format_Excel\WHRR_Label.xlsm";
            if (System.IO.File.Exists(pathExcel))
            {
                app = new Excel.Application();
                app.Visible = false;
                app.DisplayAlerts = false;
                workbook = app.Workbooks.Open(pathExcel, 0, false, 5, "", "", false, Excel.XlPlatform.xlWindows, "", true, false, 0, true, false, false);
                mWorkSheets = workbook.Worksheets;
                worksheet = (Excel.Worksheet)mWorkSheets.get_Item("Sheet1");
                worksheet = workbook.ActiveSheet;
                worksheet.Cells[2, 1] = item.R_name;
                //worksheet.Cells[6, 5] = item.Whrr_code;
                worksheet.Cells[5, 6] = item.Whrr_code;
                //worksheet.Cells[7, 5] = item.Whrr_code;
                if (kind_label == "QCOK")
                {
                    worksheet.Cells[4, 1] = item.Weight + " Kg";
                    worksheet.Cells[6, 1] = item.Lot_no.ToString("dd/MM/yyyy");
                    worksheet.Cells[9, 1] = item.Expired_date.ToString("dd/MM/yyyy");
                    worksheet.Cells[12, 1] = DateTime.Today.ToString("dd/MM/yyyy");
                    worksheet.Cells[7, 6] = "OK";
                }
                else if (kind_label == "QCNG")
                {
                    worksheet.Cells[4, 1] = item.Weight + " Kg";
                    worksheet.Cells[6, 1] = item.Lot_no.ToString("dd/MM/yyyy");
                    worksheet.Cells[9, 1] = item.Expired_date.ToString("dd/MM/yyyy");
                    worksheet.Cells[12, 1] = DateTime.Today.ToString("dd/MM/yyyy");
                    worksheet.Cells[7, 6] = "NG";
                }
                else
                {
                    worksheet.Cells[4, 1] = item.Weight;
                    worksheet.Cells[6, 1] = item.Lot_no.ToString("dd/MM/yyyy");
                    worksheet.Cells[9, 1] = item.Expired_date.ToString("dd/MM/yyyy");
                }
                DateTimeFormatInfo dfi = DateTimeFormatInfo.CurrentInfo;
                Calendar myCal = dfi.Calendar;
                string WW = myCal.GetWeekOfYear(item.Created_date, dfi.CalendarWeekRule, dfi.FirstDayOfWeek).ToString();
                string YY = item.Created_date.ToString("yy");
                worksheet.Cells[10, 4] = WW + "/" + YY;
                worksheet.Unprotect();
                app.DisplayAlerts = false;
                var printerSettings = new PrinterSettings();
                workbook.PrintOutEx(Type.Missing, Type.Missing, Type.Missing, Type.Missing, printerSettings.PrinterName, Type.Missing, Type.Missing, Type.Missing);
                workbook.Close();
                app.Quit();
            }
        }
        public void Print_W_M_Receive_Document(string rm_doc_id, string truck_no, string cond1, string cond2, string cond3, bool isCompleted, string File_path)
        {
            DataTable dt = Load_W_M_ReceiveDoc("rm_doc_id,truck_no,supplier,receive_date", "rm_doc_id=N'" + rm_doc_id + "'");
            DataTable dt_detail = new DataTable();
            DataTable dt_list = new DataTable();
            if (isCompleted)
            {
                string strQry = "select a.m_name,a.quantity,a.number_carton,b.act_qty,b.act_boxqty \n ";
                strQry += " from  \n ";
                strQry += " (select m_name,quantity,number_carton  \n ";
                strQry += " from W_M_ReceiveDocDetail where rm_doc_id=N'" + rm_doc_id + "') as a \n ";
                strQry += " left join \n ";
                strQry += " (select m_name,sum(quantity) as qty_notcheck,sum(quantity) as act_qty,count(m_name) as act_boxqty  \n ";
                strQry += " from TEMP_W_M_Receiving where rm_doc_id=N'" + rm_doc_id + "' group by m_name) as b \n ";
                strQry += " on a.m_name=b.m_name \n ";
                conn = new CmCn();
                dt_detail = conn.ExcuteDataTable(strQry);
                string strQry2 = "select * from TEMP_W_M_Receiving where rm_doc_id=N'" + rm_doc_id + "'";
                dt_list = conn.ExcuteDataTable(strQry2);
            }
            else
            {
                dt_detail = Load_W_M_ReceiveDocDetail("m_name,quantity,number_carton", "rm_doc_id=N'" + rm_doc_id + "'");
            }
            Excel.Application app;
            Excel.Workbook workbook;
            Excel.Sheets mWorkSheets;
            Excel.Worksheet worksheet;
            string pathExcel = @"C:\HVN_SYS\01.Format_Excel\WHMR_CheckSheet.xlsx";
            if (System.IO.File.Exists(pathExcel))
            {
                app = new Excel.Application();
                app.Visible = false;
                app.DisplayAlerts = false;
                workbook = app.Workbooks.Open(pathExcel, 0, false, 5, "", "", false, Excel.XlPlatform.xlWindows, "", true, false, 0, true, false, false);
                mWorkSheets = workbook.Worksheets;
                worksheet = (Excel.Worksheet)mWorkSheets.get_Item("Check sheet");
                worksheet = workbook.ActiveSheet;
                worksheet.Cells[4, 3] = dt.Rows[0]["supplier"].ToString();
                worksheet.Cells[5, 3] = DateTime.Today.ToString("dd/MM/yyyy");
                worksheet.Cells[6, 3] = dt.Rows[0]["rm_doc_id"].ToString();
                worksheet.Cells[7, 3] = dt.Rows[0]["truck_no"].ToString();
                if (cond1 == "OK")
                {
                    worksheet.Cells[11, 4] = "X";
                }
                else
                {
                    worksheet.Cells[11, 5] = "X";
                }
                if (cond2 == "OK")
                {
                    worksheet.Cells[12, 4] = "X";
                }
                else
                {
                    worksheet.Cells[12, 5] = "X";
                }
                if (cond3 == "OK")
                {
                    worksheet.Cells[13, 4] = "X";
                }
                else
                {
                    worksheet.Cells[13, 5] = "X";
                }
                int begin_row = 20;
                foreach (DataRow row in dt_detail.Rows)
                {
                    worksheet.Cells[begin_row, 2] = row["m_name"].ToString();
                    worksheet.Cells[begin_row, 3] = row["quantity"].ToString();
                    worksheet.Cells[begin_row, 5] = row["number_carton"].ToString();
                    if (isCompleted)
                    {
                        worksheet.Cells[begin_row, 6] = row["act_qty"].ToString();
                        worksheet.Cells[begin_row, 7] = row["act_boxqty"].ToString();
                    }
                    begin_row++;
                }
                /*
                if (dt_list.Rows.Count > 0)
                {
                    if (dt_list.Rows.Count>5)
                    {
                        for (int i = 0; i < dt_list.Rows.Count-5; i++)
                        {
                            worksheet.Range[worksheet.Cells[46, 1], worksheet.Cells[46, 10]].Insert(Type.Missing);
                        }
                    }
                    int begin_row2 = 43;
                    int dem = 1;
                    foreach (DataRow item in dt_list.Rows)
                    {
                        worksheet.Cells[begin_row2, 1] = dem;
                        worksheet.Cells[begin_row2, 2] = item["whmr_code"].ToString();
                        worksheet.Cells[begin_row2, 3] = item["m_name"].ToString();
                        worksheet.Cells[begin_row2, 4] = item["quantity"].ToString();
                        if (item["wh_okng"].ToString() == "False")
                        {
                            worksheet.Cells[begin_row2, 6] = "X";
                        }
                        else
                        {
                            worksheet.Cells[begin_row2, 5] = "X";
                        }
                        begin_row2++;
                        dem++;
                    }
                }
                */
                worksheet.Unprotect();
                app.DisplayAlerts = false;
                string filename = @File_path;
                workbook.SaveAs(filename);
                //if (File.Exists(@"\\172.16.180.20\30.Scan\wh\Receiving_Material_Document\"+ rm_doc_id + ".pdf"))
                //{
                //    File.Delete(@"\\172.16.180.20\30.Scan\wh\Receiving_Material_Document\"+ rm_doc_id + ".pdf");
                //}
                //workbook.ExportAsFixedFormat(Microsoft.Office.Interop.Excel.XlFixedFormatType.xlTypePDF, @"\\172.16.180.20\30.Scan\wh\Receiving_Material_Document\" + rm_doc_id + ".pdf");
                workbook.Close();
                app.Quit();
            }
        }
        public void Print_PUR_PR_Detail(PUR_PR_Entity Pr, string File_path, string kind)
        {

            Excel.Application app;
            Excel.Workbook workbook;
            Excel.Sheets mWorkSheets;
            Excel.Worksheet worksheet;
            string pathExcel = @"C:\HVN_SYS\01.Format_Excel\PUR_PR_Detail.xlsx";
            if (System.IO.File.Exists(pathExcel))
            {
                string strQry = "Select * from PUR_PRDetail where pr_no=N'" + Pr.Pr_no + "' order by item_name";
                conn = new CmCn();
                DataTable dt = conn.ExcuteDataTable(strQry);
                List<PUR_PRDetail_Entity> List_detail = new List<PUR_PRDetail_Entity>();
                List<PUR_PR_CompareCDE_Entity> List_CompareCDE = new List<PUR_PR_CompareCDE_Entity>();
                if (dt.Rows.Count > 0)
                {
                    int Stt = 1;
                    foreach (DataRow row in dt.Rows)
                    {
                        PUR_PRDetail_Entity item = new PUR_PRDetail_Entity();
                        item.Stt = Stt;
                        item.Pr_no = Pr.Pr_no;
                        item.Item_name = row["item_name"].ToString();
                        item.Hut_code = row["hut_code"].ToString();
                        item.Supplier_name = row["supplier_name"].ToString();
                        item.Quantity = decimal.Parse(row["quantity"].ToString());
                        item.Unit = row["unit"].ToString();
                        item.Unit_price = decimal.Parse(row["unit_price"].ToString());
                        item.Vat = decimal.Parse(row["vat"].ToString());
                        item.Vat_amount = item.Quantity * item.Vat * item.Unit_price;
                        item.Amount = decimal.Parse(row["amount"].ToString());
                        List_detail.Add(item);
                        Stt++;
                    }
                }
                List<PUR_PR_CostSaving_Entity> List_saving_item = new List<PUR_PR_CostSaving_Entity>();
                string strQry2 = "Select * from PUR_PR_CostSaving where pr_no=N'" + Pr.Pr_no + "' order by item_name";
                DataTable dt2 = conn.ExcuteDataTable(strQry2);
                if (dt2.Rows.Count > 0)
                {
                    int Stt = 1;
                    foreach (DataRow row in dt2.Rows)
                    {
                        PUR_PR_CostSaving_Entity item = new PUR_PR_CostSaving_Entity();
                        item.Stt = Stt;
                        item.Pr_no = Pr.Pr_no;
                        item.Item_name = row["item_name"].ToString();
                        item.Before_price = decimal.Parse(row["before_price"].ToString());
                        item.After_price = decimal.Parse(row["after_price"].ToString());
                        item.Volume = decimal.Parse(row["volume"].ToString());
                        item.Is_purchased = row["is_purchased"].ToString();
                        List_saving_item.Add(item);
                        Stt++;
                    }
                }
                string strQry3 = "Select * from PUR_PR_CompareCDE where pr_no=N'" + Pr.Pr_no + "' order by item_name";
                DataTable dt3 = conn.ExcuteDataTable(strQry3);
                if (dt3.Rows.Count > 0)
                {
                    int Stt = 1;
                    foreach (DataRow row in dt3.Rows)
                    {
                        PUR_PR_CompareCDE_Entity item = new PUR_PR_CompareCDE_Entity();
                        item.Stt = Stt;
                        item.Pr_no = Pr.Pr_no;
                        item.Item_name = row["item_name"].ToString();
                        item.Cde_budget = decimal.Parse(row["cde_budget"].ToString());
                        item.Actual_cost = decimal.Parse(row["actual_cost"].ToString());
                        item.Remain_budget = decimal.Parse(row["remain_budget"].ToString());
                        //item.Ultili_budget = item.Actual_cost / item.Cde_budget;
                        List_CompareCDE.Add(item);
                        Stt++;
                    }
                }
                app = new Excel.Application();
                app.Visible = false;
                app.DisplayAlerts = false;
                workbook = app.Workbooks.Open(pathExcel, 0, false, 5, "", "", false, Excel.XlPlatform.xlWindows, "", true, false, 0, true, false, false);
                mWorkSheets = workbook.Worksheets;
                worksheet = (Excel.Worksheet)mWorkSheets.get_Item("Sheet1");
                worksheet = workbook.ActiveSheet;
                worksheet.Cells[3, 1] = "Requester/ Người yêu cầu:" + Pr.Requester;
                worksheet.Cells[4, 1] = "Department/bộ phận:" + Pr.Dept;
                if (string.IsNullOrEmpty(Pr.Old_pr_no))
                {
                    worksheet.Cells[2, 7] = "PR no./ Số PR:" + Pr.Pr_no;
                }
                else
                {
                    worksheet.Cells[2, 7] = "PR no./ Số PR:" + Pr.Pr_no +" ( for replacing "+Pr.Old_pr_no +")";
                }
                worksheet.Cells[3, 7] = "PR date/Ngày yêu cầu:" + Pr.Pr_date.ToString("dd/MM/yyyy");
                worksheet.Cells[4, 7] = "Estimate received date/Ngày nhận hàng dự kiến:" + Pr.Estimate_received_date.ToString("dd/MM/yyyy");
                worksheet.Cells[8, 1] = "Type of request / phân loại:" + Pr.Pr_type;
                worksheet.Cells[9, 1] = "Advance payment/ TT trước:" + Pr.Advance_payment;
                worksheet.Cells[8, 7] = "IN CAPEX NUMBER (FINANCE): " + Pr.Capex_no;
                worksheet.Cells[44, 7] = Pr.Pr_currency;
                int begin_detail_row = 14;
                int begin_CDE_row = 47;
                int begin_saving_row = 80;
                int begin_sign_row = 111;
                var hiddenRange = worksheet.Range[worksheet.Cells[begin_detail_row + List_detail.Count, 1], worksheet.Cells[43, 10]];
                hiddenRange.EntireRow.Hidden = true;
                var hiddenRange2 = worksheet.Range[worksheet.Cells[begin_CDE_row + List_CompareCDE.Count, 1], worksheet.Cells[77, 10]];
                hiddenRange2.EntireRow.Hidden = true;
                var hiddenRange3 = worksheet.Range[worksheet.Cells[begin_saving_row + List_saving_item.Count, 1], worksheet.Cells[109, 10]];
                hiddenRange3.EntireRow.Hidden = true;
                foreach (PUR_PRDetail_Entity row in List_detail)
                {
                    worksheet.Cells[begin_detail_row, 1] = row.Stt;
                    worksheet.Cells[begin_detail_row, 2] = row.Item_name;
                    worksheet.Cells[begin_detail_row, 3] = row.Hut_code;
                    worksheet.Cells[begin_detail_row, 4] = row.Supplier_name;
                    worksheet.Cells[begin_detail_row, 5] = row.Quantity.ToString();
                    worksheet.Cells[begin_detail_row, 6] = row.Unit;
                    worksheet.Cells[begin_detail_row, 7] = row.Unit_price.ToString();
                    worksheet.Cells[begin_detail_row, 8] = row.Vat.ToString();
                    worksheet.Cells[begin_detail_row, 9] = row.Vat_amount.ToString();
                    worksheet.Cells[begin_detail_row, 10] = row.Amount.ToString();
                    worksheet.Rows[begin_detail_row].AutoFit();
                    begin_detail_row++;
                }
                foreach (PUR_PR_CompareCDE_Entity row in List_CompareCDE)
                {
                    worksheet.Cells[begin_CDE_row, 1] = row.Stt;
                    worksheet.Cells[begin_CDE_row, 2] = row.Item_name;
                    worksheet.Cells[begin_CDE_row, 4] = row.Cde_budget.ToString();
                    worksheet.Cells[begin_CDE_row, 5] = row.Actual_cost.ToString();
                    worksheet.Cells[begin_CDE_row, 7] = row.Remain_budget.ToString();
                    worksheet.Rows[begin_CDE_row].AutoFit();
                    begin_CDE_row++;
                }
                foreach (PUR_PR_CostSaving_Entity row in List_saving_item)
                {
                    worksheet.Cells[begin_saving_row, 1] = row.Stt;
                    worksheet.Cells[begin_saving_row, 2] = row.Item_name;
                    worksheet.Cells[begin_saving_row, 4] = row.Before_price.ToString();
                    worksheet.Cells[begin_saving_row, 5] = row.After_price.ToString();
                    worksheet.Cells[begin_saving_row, 7] = row.Volume.ToString();
                    worksheet.Cells[begin_saving_row, 9] = row.Is_purchased;
                    worksheet.Rows[begin_saving_row].AutoFit();
                    begin_saving_row++;
                }
                worksheet.Cells[14,begin_detail_row].Rows.AutoFit();

                if (kind == "begin")
                {
                    Excel.Range oRange = (Excel.Range)worksheet.Cells[begin_sign_row, 1];
                    float Left = (float)((double)oRange.Left);
                    float Top = (float)((double)oRange.Top);
                    if (!string.IsNullOrEmpty(Pr.Requester_sign))
                    {
                        worksheet.Shapes.AddPicture(Pr.Requester_sign, Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoCTrue, Left + 100, Top + 2, 80, 60);
                    }
                    worksheet.Unprotect();
                    app.DisplayAlerts = false;
                    string filename = @File_path;
                    if (File.Exists(filename))
                    {
                        File.Delete(filename);
                    }
                    workbook.ExportAsFixedFormat(Microsoft.Office.Interop.Excel.XlFixedFormatType.xlTypePDF, filename);
                    workbook.Close();
                    app.Quit();
                }
                else
                {
                    Excel.Range oRange = (Excel.Range)worksheet.Cells[begin_sign_row, 1];
                    float Left = (float)((double)oRange.Left);
                    float Top = (float)((double)oRange.Top);
                    switch (Pr.Pr_status)
                    {
                        case "Pending requester":
                            break;
                        case "Pending checker":
                            if (!string.IsNullOrEmpty(Pr.Requester_sign))
                            {
                                worksheet.Shapes.AddPicture(Pr.Requester_sign, Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoCTrue, Left + 100, Top + 2, 80, 60);
                            }
                            break;
                        case "Pending approver":
                            if (!string.IsNullOrEmpty(Pr.Requester_sign))
                            {
                                worksheet.Shapes.AddPicture(Pr.Requester_sign, Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoCTrue, Left + 100, Top + 2, 80, 60);
                            }
                            if (!string.IsNullOrEmpty(Pr.Check_sign))
                            {
                                worksheet.Shapes.AddPicture(Pr.Check_sign, Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoCTrue, Left + 253, Top + 2, 80, 60);
                            }
                            break;
                        case "Pending issue PO":
                            if (!string.IsNullOrEmpty(Pr.Requester_sign))
                            {
                                worksheet.Shapes.AddPicture(Pr.Requester_sign, Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoCTrue, Left + 100, Top + 2, 80, 60);
                            }
                            if (!string.IsNullOrEmpty(Pr.Check_sign))
                            {
                                worksheet.Shapes.AddPicture(Pr.Check_sign, Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoCTrue, Left + 253, Top + 2, 80, 60);
                            }
                            if (!string.IsNullOrEmpty(Pr.Approve_sign))
                            {
                                worksheet.Shapes.AddPicture(Pr.Approve_sign, Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoCTrue, Left + 398, Top + 2, 80, 60);
                            }
                            break;
                        default:
                            if (!string.IsNullOrEmpty(Pr.Requester_sign))
                            {
                                worksheet.Shapes.AddPicture(Pr.Requester_sign, Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoCTrue, Left + 100, Top + 2, 80, 60);
                            }
                            if (!string.IsNullOrEmpty(Pr.Check_sign))
                            {
                                worksheet.Shapes.AddPicture(Pr.Check_sign, Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoCTrue, Left + 250, Top + 2, 80, 60);
                            }
                            if (!string.IsNullOrEmpty(Pr.Approve_sign))
                            {
                                worksheet.Shapes.AddPicture(Pr.Approve_sign, Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoCTrue, Left + 390, Top + 2, 80, 60);
                            }
                            break;
                    }
                    worksheet.Unprotect();
                    app.DisplayAlerts = false;
                    string filename = @File_path;
                    if (kind=="print")
                    {
                        var printerSettings = new PrinterSettings();
                        workbook.PrintOutEx(Type.Missing, Type.Missing, Type.Missing, Type.Missing, printerSettings.PrinterName, Type.Missing, Type.Missing, Type.Missing);
                    }
                    else
                    {
                        if (File.Exists(filename))
                        {
                            File.Delete(filename);
                        }
                        workbook.ExportAsFixedFormat(Microsoft.Office.Interop.Excel.XlFixedFormatType.xlTypePDF, File_path);
                    }
                    workbook.Close();
                    app.Quit();
                }
            }
        }
        public void Print_PUR_PO_Detail(PUR_PO_Entity Po, string File_path, string kind)
        {

            Excel.Application app;
            Excel.Workbook workbook;
            Excel.Sheets mWorkSheets;
            Excel.Worksheet worksheet;
            string pathExcel = @"C:\HVN_SYS\01.Format_Excel\PUR_PO_Detail.xlsx";
            if (System.IO.File.Exists(pathExcel))
            {
                string strQry = "Select * from PUR_PODetail where Po_no=N'" + Po.Po_no + "'";
                conn = new CmCn();
                DataTable dt = conn.ExcuteDataTable(strQry);
                List<PUR_PODetail_Entity> List_detail = new List<PUR_PODetail_Entity>();
                if (dt.Rows.Count > 0)
                {
                    int stt = 1;
                    foreach (DataRow row in dt.Rows)
                    {
                        PUR_PODetail_Entity item = new PUR_PODetail_Entity();
                        item.Stt = stt;
                        item.Po_no = row["Po_no"].ToString();
                        item.Item_name = row["Item_name"].ToString();
                        item.Erp_code = row["Erp_code"].ToString();
                        item.Hut_code = row["Hut_code"].ToString();
                        item.Quantity = decimal.Parse(row["Quantity"].ToString());
                        item.Unit = row["Unit"].ToString();
                        item.Unit_price = decimal.Parse(row["Unit_price"].ToString());
                        item.Unit_currency = row["Unit_currency"].ToString();
                        item.Vat = decimal.Parse(row["Vat"].ToString());
                        item.Amount = decimal.Parse(row["Amount"].ToString());
                        item.Vat_amount = item.Quantity * item.Unit_price * item.Vat;
                        item.Moq = decimal.Parse(row["Moq"].ToString());
                        item.Standard_packing = decimal.Parse(row["Standard_packing"].ToString());
                        List_detail.Add(item);
                        stt++;
                    }
                }
                //Load supplier
                string strQry2 = "select * from PUR_MasterListSupplier where supplier_name=N'" + Po.Supplier_name + "'";
                conn = new CmCn();
                DataTable dt2 = conn.ExcuteDataTable(strQry2);
                PUR_MasterListSupplier_Entity supplier = new PUR_MasterListSupplier_Entity();
                if (dt2.Rows.Count > 0)
                {
                    supplier.Supplier_name = dt2.Rows[0]["supplier_name"].ToString();
                    supplier.Sup_address = dt2.Rows[0]["sup_address"].ToString();
                    supplier.Sup_tel = dt2.Rows[0]["sup_tel"].ToString();
                    supplier.Tax_code = dt2.Rows[0]["tax_code"].ToString();
                    supplier.Contact_pic = dt2.Rows[0]["contact_pic"].ToString();
                    supplier.Email_address= dt2.Rows[0]["Email_address"].ToString();
                }
                app = new Excel.Application();
                app.Visible = false;
                app.DisplayAlerts = false;
                workbook = app.Workbooks.Open(pathExcel, 0, false, 5, "", "", false, Excel.XlPlatform.xlWindows, "", true, false, 0, true, false, false);
                mWorkSheets = workbook.Worksheets;
                worksheet = (Excel.Worksheet)mWorkSheets.get_Item("Sheet1");
                worksheet = workbook.ActiveSheet;
                worksheet.Cells[7, 3] = "van.nguyen";
                worksheet.Cells[8, 3] = "van.nguyen@hutchinson.com";
                worksheet.Cells[3, 7] = "Supplier:" + Po.Supplier_name;
                worksheet.Cells[4, 8] = supplier.Sup_address;
                worksheet.Cells[5, 8] = supplier.Sup_tel;
                worksheet.Cells[6, 8] = supplier.Tax_code;
                worksheet.Cells[7, 8] = supplier.Contact_pic;
                worksheet.Cells[8, 8] = supplier.Email_address;
               
                if (Po.Old_po_no!="")
                {
                    worksheet.Cells[10, 3] = Po.Po_no+"( for replacing " + Po.Old_po_no+")";
                }
                else
                {
                    worksheet.Cells[10, 3] = Po.Po_no;
                }
                worksheet.Cells[11, 3] = Po.Po_date;
                worksheet.Cells[12, 3] = Po.Payment_term;
                worksheet.Cells[10, 8] = Po.Delivery_mode;
                worksheet.Cells[11, 8] = Po.Incoterm;
                worksheet.Cells[12, 8] = Po.Pickup_date;
                worksheet.Cells[10, 11] = Po.Po_currency;
                int begin_detail_row = 15;
                int begin_sign_row = 54;
                var hiddenRange = worksheet.Range[worksheet.Cells[begin_detail_row + List_detail.Count, 1], worksheet.Cells[47, 11]];
                hiddenRange.EntireRow.Hidden = true;
                foreach (PUR_PODetail_Entity row in List_detail)
                {
                    worksheet.Cells[begin_detail_row, 1] = row.Stt;
                    worksheet.Cells[begin_detail_row, 2] = row.Item_name;
                    worksheet.Cells[begin_detail_row, 4] = row.Erp_code;
                    worksheet.Cells[begin_detail_row, 5] = row.Hut_code;
                    worksheet.Cells[begin_detail_row, 6] = row.Quantity.ToString();
                    worksheet.Cells[begin_detail_row, 7] = row.Unit;
                    worksheet.Cells[begin_detail_row, 8] = row.Unit_price.ToString();
                    worksheet.Cells[begin_detail_row, 9] = row.Vat.ToString();
                    worksheet.Cells[begin_detail_row,10] = row.Vat_amount;
                    worksheet.Cells[begin_detail_row, 11] = row.Amount;
                    begin_detail_row++;
                }
                if (kind == "begin")
                {
                    Excel.Range oRange = (Excel.Range)worksheet.Cells[begin_sign_row, 1];
                    float Left = (float)((double)oRange.Left);
                    float Top = (float)((double)oRange.Top);
                    if (!string.IsNullOrEmpty(Po.Po_pic))
                    {
                        worksheet.Shapes.AddPicture(Po.Po_pic_sign, Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoCTrue, Left + 50, Top + 5, 120, 80);
                    }
                    worksheet.Unprotect();
                    app.DisplayAlerts = false;
                    string filename = @File_path;
                    if (File.Exists(filename))
                    {
                        File.Delete(filename);
                    }
                    workbook.ExportAsFixedFormat(Microsoft.Office.Interop.Excel.XlFixedFormatType.xlTypePDF, filename);
                    workbook.Close();
                    app.Quit();
                }
                else
                {
                    Excel.Range oRange = (Excel.Range)worksheet.Cells[begin_sign_row, 1];
                    float Left = (float)((double)oRange.Left);
                    float Top = (float)((double)oRange.Top);
                    switch (Po.Po_status)
                    {
                        case "Pending checker":
                            if (!string.IsNullOrEmpty(Po.Po_pic))
                            {
                                worksheet.Shapes.AddPicture(Po.Po_pic_sign, Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoCTrue, Left + 50, Top +5, 120,80);
                            }
                            break;
                        case "Pending approver":
                            if (!string.IsNullOrEmpty(Po.Po_pic_sign))
                            {
                                worksheet.Shapes.AddPicture(Po.Po_pic_sign, Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoCTrue, Left + 50, Top +5, 120,80);
                            }
                            if (!string.IsNullOrEmpty(Po.Po_checker_sign))
                            {
                                worksheet.Shapes.AddPicture(Po.Po_checker_sign, Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoCTrue, Left +280, Top +5, 120,80);
                            }
                            break;
                        case "PO approved":
                            if (!string.IsNullOrEmpty(Po.Po_pic_sign))
                            {
                                worksheet.Shapes.AddPicture(Po.Po_pic_sign, Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoCTrue, Left + 50, Top +5, 120,80);
                            }
                            if (!string.IsNullOrEmpty(Po.Po_checker_sign))
                            {
                                worksheet.Shapes.AddPicture(Po.Po_checker_sign, Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoCTrue, Left +280, Top +5, 120,80);
                            }
                            if (!string.IsNullOrEmpty(Po.Po_approver_sign))
                            {
                                worksheet.Shapes.AddPicture(Po.Po_approver_sign, Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoCTrue, Left + 530, Top +5, 150,100);
                            }
                            break;
                        default:
                            if (!string.IsNullOrEmpty(Po.Po_pic_sign))
                            {
                                worksheet.Shapes.AddPicture(Po.Po_pic_sign, Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoCTrue, Left + 50, Top +5, 120,80);
                            }
                            if (!string.IsNullOrEmpty(Po.Po_checker_sign))
                            {
                                worksheet.Shapes.AddPicture(Po.Po_checker_sign, Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoCTrue, Left +280, Top +5, 120,80);
                            }
                            if (!string.IsNullOrEmpty(Po.Po_approver_sign))
                            {
                                worksheet.Shapes.AddPicture(Po.Po_approver_sign, Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoCTrue, Left + 530, Top + 5, 150,100);
                            }
                            break;
                    }
                    worksheet.Unprotect();
                    app.DisplayAlerts = false;
                    if (kind=="export")
                    {
                        string filename = File_path;
                        if (File.Exists(filename))
                        {
                            File.Delete(filename);
                        }
                        workbook.ExportAsFixedFormat(Microsoft.Office.Interop.Excel.XlFixedFormatType.xlTypePDF, filename);
                    }
                    else
                    {
                        var printerSettings = new PrinterSettings();
                        workbook.PrintOutEx(Type.Missing, Type.Missing, Type.Missing, Type.Missing, printerSettings.PrinterName, Type.Missing, Type.Missing, Type.Missing);
                    }
                    workbook.Close();
                    app.Quit();
                }
            }
        }
        public void Print_PUR_VS_Detail(PUR_VS_Entity Current_VS, string @File_path, string kind)
        {
            Excel.Application app;
            Excel.Workbook workbook;
            Excel.Sheets mWorkSheets;
            Excel.Worksheet worksheet;
            string pathExcel = @"C:\HVN_SYS\01.Format_Excel\PUR_VS_Detail.xlsx";
            if (System.IO.File.Exists(pathExcel))
            {
                app = new Excel.Application();
                app.Visible = false;
                app.DisplayAlerts = false;
                workbook = app.Workbooks.Open(pathExcel, 0, false, 5, "", "", false, Excel.XlPlatform.xlWindows, "", true, false, 0, true, false, false);
                mWorkSheets = workbook.Worksheets;
                worksheet = (Excel.Worksheet)mWorkSheets.get_Item("Sheet1");
                worksheet = workbook.ActiveSheet;
                worksheet.Cells[3, 2] = Current_VS.Vs_date.ToString("dd/MM/yyyy");
                worksheet.Cells[4, 2] = Current_VS.Vs_requester;
                worksheet.Cells[16, 1] = "1.3 Part no.:" + Current_VS.Vs_des;
                worksheet.Cells[19, 3] = Current_VS.Supplier_1;
                worksheet.Cells[19, 5] = Current_VS.Supplier_2;
                worksheet.Cells[19, 7] = Current_VS.Supplier_3;
                switch (Current_VS.Selected_supplier)
                {
                    case "SUPPLIER 1":
                        worksheet.Cells[79, 1] = "FINAL CHOICE / ARGUMENTS :"+Current_VS.Supplier_1;
                        break;
                    case "SUPPLIER 2":
                        worksheet.Cells[79, 1] = "FINAL CHOICE / ARGUMENTS :" + Current_VS.Supplier_2;
                        break;
                    case "SUPPLIER 3":
                        worksheet.Cells[79, 1] = "FINAL CHOICE / ARGUMENTS :" + Current_VS.Supplier_3;
                        break;
                    default:
                        worksheet.Cells[79, 1] = "FINAL CHOICE / ARGUMENTS :" + Current_VS.Supplier_1;
                        break;
                }
                worksheet.Cells[78, 1] = "Comments:"+Current_VS.Vs_comment;
                int begin_row_1 = 21;
                int begin_row_2 = 21;
                int begin_row_3 = 21;
                conn = new CmCn();
                string strQry1 = "select a.*  \n ";
                strQry1 += " from PUR_VS_SupplierInfo a \n ";
                strQry1 += " where a.vs_id=N'" + Current_VS.Vs_id + "'  \n ";
                strQry1 += " and a.supplier_name=N'" + Current_VS.Supplier_1 + "' \n ";
                strQry1 += " order by a.vs_stt \n ";
                DataTable dt1 = conn.ExcuteDataTable(strQry1);
                foreach (DataRow row in dt1.Rows)
                {
                    worksheet.Cells[begin_row_1, 3] = "'"+row["vs_des"].ToString();
                    worksheet.Cells[begin_row_1, 4] = row["vs_score"].ToString();
                    begin_row_1++;
                }
                if (Current_VS.Supplier_2!="")
                {
                    string strQry2 = "select a.*  \n ";
                    strQry2 += " from PUR_VS_SupplierInfo a \n ";
                    strQry2 += " where a.vs_id=N'" + Current_VS.Vs_id + "'  \n ";
                    strQry2 += " and a.supplier_name=N'" + Current_VS.Supplier_2 + "' \n ";
                    strQry2 += " order by a.vs_stt \n ";
                    DataTable dt2 = conn.ExcuteDataTable(strQry2);
                    foreach (DataRow row in dt2.Rows)
                    {
                        worksheet.Cells[begin_row_2, 5] = "'" + row["vs_des"].ToString();
                        worksheet.Cells[begin_row_2, 6] = row["vs_score"].ToString();
                        begin_row_2++;
                    }
                }
                if (Current_VS.Supplier_3 != "")
                {
                    string strQry3 = "select a.*  \n ";
                    strQry3 += " from PUR_VS_SupplierInfo a \n ";
                    strQry3 += " where a.vs_id=N'" + Current_VS.Vs_id + "'  \n ";
                    strQry3 += " and a.supplier_name=N'" + Current_VS.Supplier_3 + "' \n ";
                    strQry3 += " order by a.vs_stt \n ";
                    DataTable dt3 = conn.ExcuteDataTable(strQry3);
                    foreach (DataRow row in dt3.Rows)
                    {
                        worksheet.Cells[begin_row_3, 7] = "'" + row["vs_des"].ToString();
                        worksheet.Cells[begin_row_3, 8] = row["vs_score"].ToString();
                        begin_row_3++;
                    }
                }
                int begin_sign_row = 85;
                Excel.Range oRange = (Excel.Range)worksheet.Cells[begin_sign_row, 2];
                float Left = (float)((double)oRange.Left);
                float Top = (float)((double)oRange.Top);

                if (!string.IsNullOrEmpty(Current_VS.Dept_sign))
                {
                    worksheet.Cells[begin_sign_row-2, 2] = Current_VS.Dept_mgr;
                    worksheet.Cells[begin_sign_row-1, 2] = Current_VS.Dept_mgr_date.ToString("dd/MM/yyyy");
                    worksheet.Shapes.AddPicture(Current_VS.Dept_sign, Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoCTrue, Left + 80, Top + 5, 120, 100);
                }
                if (!string.IsNullOrEmpty(Current_VS.Fin_mgr_sign))
                {
                    worksheet.Cells[begin_sign_row - 2, 3] = Current_VS.Fin_mgr;
                    worksheet.Cells[begin_sign_row - 1, 3] = Current_VS.Fin_mgr_date.ToString("dd/MM/yyyy");
                    worksheet.Shapes.AddPicture(Current_VS.Fin_mgr_sign, Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoCTrue, Left + 380, Top + 5, 120, 100);
                }
                if (!string.IsNullOrEmpty(Current_VS.Pur_mgr_sign))
                {
                    worksheet.Cells[begin_sign_row - 2, 5] = Current_VS.Pur_mgr;
                    worksheet.Cells[begin_sign_row - 1, 5] = Current_VS.Pur_mgr_date.ToString("dd/MM/yyyy");
                    worksheet.Shapes.AddPicture(Current_VS.Pur_mgr_sign, Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoCTrue, Left + 680, Top + 5, 120, 100);
                }
                if (!string.IsNullOrEmpty(Current_VS.Plant_mgr_sign))
                {
                    worksheet.Cells[begin_sign_row - 2, 7] = Current_VS.Plant_mgr;
                    worksheet.Cells[begin_sign_row - 1, 7] = Current_VS.Plant_mgr_date.ToString("dd/MM/yyyy");
                    worksheet.Shapes.AddPicture(Current_VS.Plant_mgr_sign, Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoCTrue, Left + 975, Top + 5, 130, 100);
                }
                worksheet.Unprotect();
                app.DisplayAlerts = false;
                if (kind == "export")
                {
                    string filename = @File_path;
                    if (File.Exists(filename))
                    {
                        File.Delete(filename);
                    }
                    workbook.ExportAsFixedFormat(Microsoft.Office.Interop.Excel.XlFixedFormatType.xlTypePDF, filename);
                }
                else
                {
                    var printerSettings = new PrinterSettings();
                    workbook.PrintOutEx(Type.Missing, Type.Missing, Type.Missing, Type.Missing, printerSettings.PrinterName, Type.Missing, Type.Missing, Type.Missing);
                }
                workbook.Close();
                app.Quit();
            }
        }
        public void Print_HR_CarRegistration(HR_CarRegistration_Entity Request)
        {
            Excel.Application app;
            Excel.Workbook workbook;
            Excel.Sheets mWorkSheets;
            Excel.Worksheet worksheet;
            string pathExcel = @"C:\HVN_SYS\01.Format_Excel\HR_GO.xlsx";
            if (System.IO.File.Exists(pathExcel))
            {
                app = new Excel.Application();
                app.Visible = false;
                app.DisplayAlerts = false;
                workbook = app.Workbooks.Open(pathExcel, 0, false, 5, "", "", false, Excel.XlPlatform.xlWindows, "", true, false, 0, true, false, false);
                mWorkSheets = workbook.Worksheets;
                worksheet = (Excel.Worksheet)mWorkSheets.get_Item("Sheet1");
                worksheet = workbook.ActiveSheet;
                worksheet.Cells[9, 4] = Request.Requester;
                worksheet.Cells[11, 9] = Request.Dept;
                worksheet.Cells[14, 1] = "Thời gian xin ra ngoài/Time go out:"+Request.From_date.ToString("dd/MM/yyyy HH:mm") +"  ->  "+ Request.To_date.ToString("dd/MM/yyyy HH:mm");
                worksheet.Cells[15, 1] = "Địa điểm đến/Destination:"+Request.From_loc+" -> "+Request.To_loc;
                worksheet.Cells[16, 1] = "Mục đích/ Purpose:" + Request.Purpose;
                worksheet.Cells[17, 1] = "Loại xe sử dụng/ Type of car:" + Request.Car_type;
                int begin_sign_row = 24;
                Excel.Range oRange = (Excel.Range)worksheet.Cells[begin_sign_row, 1];
                float Left = (float)((double)oRange.Left);
                float Top = (float)((double)oRange.Top);
                if (!string.IsNullOrEmpty(Request.Requester_sign))
                {
                    worksheet.Shapes.AddPicture(Request.Requester_sign, Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoCTrue, Left + 38, Top + 2, 80, 60);
                }
                if (!string.IsNullOrEmpty(Request.Dept_mgr_sign))
                {
                    worksheet.Shapes.AddPicture(Request.Dept_mgr_sign, Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoCTrue, Left + 280, Top + 2, 80, 60);
                }
                if (!string.IsNullOrEmpty(Request.Plant_mgr_sign))
                {
                    worksheet.Shapes.AddPicture(Request.Plant_mgr_sign, Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoCTrue, Left + 520, Top + 2, 100, 75);
                }
                if (!string.IsNullOrEmpty(Request.Hr_pic_sign))
                {
                    worksheet.Shapes.AddPicture(Request.Hr_pic_sign, Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoCTrue, Left + 330, Top + 200, 80, 60);
                }
                worksheet.Unprotect();
                app.DisplayAlerts = false;
                var printerSettings = new PrinterSettings();
                workbook.PrintOutEx(Type.Missing, Type.Missing, Type.Missing, Type.Missing, printerSettings.PrinterName, Type.Missing, Type.Missing, Type.Missing);
                workbook.Close();
                app.Quit();
            }
        }
        public void Update_time_to_Warehouse(P_Label_Entity p_Label)
        {
            string strQry = "update P_Label set op_input_wh='" + p_Label.Op_input_wh + "',date_input_wh=getdate(),place=N'" + p_Label.Place + "',isLock=N'Unblock' \n";
            strQry += " where label_code=N'" + p_Label.Label_code + "' \n";
            strQry += "insert into W_HistoryOfTransaction(label_code,product_code,product_customer_code,product_quantity,lot_no,[transaction],[location],pallet_no,input_time,PIC) \n";
            strQry += "select N'" + p_Label.Label_code + "',N'" + p_Label.Product_code + "',N'" + p_Label.Product_customer_code + "',N'" + p_Label.Product_quantity + "',N'" + p_Label.Lot_no + "'";
            strQry += ",N'" + p_Label.Note + "',N'" + p_Label.Wh_location + "',N'" + p_Label.Pallet_no + "',getdate(),N'" + p_Label.Op_input_wh + "' \n";
            conn = new CmCn();
            try
            {
                conn.ExcuteQry(strQry);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void Update_time_leave_Warehouse(P_Label_Entity p_Label)
        {
            string strQry = "update P_Label set date_input_wh=null,op_input_wh=null,wh_locate_date=null,wh_location=null,wh_op_locate=null,date_input_packing_zone=null,op_input_packing_zone=null,date_packed=null \n";
            strQry += ", op_packed = null, pallet_no = null, date_locate_packed = null, location_packed = null, op_locate_packed = null, place = null,patrol_result=null,patrol_op=null,patrol_date=null \n";
            strQry += " where label_code=N'" + p_Label.Label_code + "' \n";
            strQry += "insert into W_HistoryOfTransaction(label_code,product_code,product_customer_code,product_quantity,lot_no,[transaction],[location],pallet_no,input_time,PIC) \n";
            strQry += "select N'" + p_Label.Label_code + "',N'" + p_Label.Product_code + "',N'" + p_Label.Product_customer_code + "',N'" + p_Label.Product_quantity + "',N'" + p_Label.Lot_no + "'";
            strQry += ",N'" + p_Label.Note + "',N'" + p_Label.Wh_location + "',N'" + p_Label.Pallet_no + "',getdate(),N'" + p_Label.Op_input_wh + "' \n";
            conn = new CmCn();
            try
            {
                conn.ExcuteQry(strQry);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void Update_time_QC_Receive(P_Label_Entity p_Label)
        {
            string strQry = "update P_Label set place = N'QC Area' \n";
            strQry += " where label_code=N'" + p_Label.Label_code + "' \n";
            strQry += "insert into W_HistoryOfTransaction(label_code,product_code,product_customer_code,product_quantity,lot_no,[transaction],[location],pallet_no,input_time,PIC) \n";
            strQry += "select N'" + p_Label.Label_code + "',N'" + p_Label.Product_code + "',N'" + p_Label.Product_customer_code + "',N'" + p_Label.Product_quantity + "',N'" + p_Label.Lot_no + "'";
            strQry += ",N'" + p_Label.Note + "',N'" + p_Label.Wh_location + "',N'" + p_Label.Pallet_no + "',getdate(),N'" + p_Label.Op_input_wh + "' \n";
            conn = new CmCn();
            try
            {
                conn.ExcuteQry(strQry);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void Update_time_QC_Move(P_Label_Entity p_Label)
        {
            string strQry = "update P_Label set place = N'' \n";
            strQry += " where label_code=N'" + p_Label.Label_code + "' \n";
            strQry += "insert into W_HistoryOfTransaction(label_code,product_code,product_customer_code,product_quantity,lot_no,[transaction],[location],pallet_no,input_time,PIC) \n";
            strQry += "select N'" + p_Label.Label_code + "',N'" + p_Label.Product_code + "',N'" + p_Label.Product_customer_code + "',N'" + p_Label.Product_quantity + "',N'" + p_Label.Lot_no + "'";
            strQry += ",N'" + p_Label.Note + "',N'" + p_Label.Wh_location + "',N'" + p_Label.Pallet_no + "',getdate(),N'" + p_Label.Op_input_wh + "' \n";
            conn = new CmCn();
            try
            {
                conn.ExcuteQry(strQry);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void Update_time_Inspection(P_Label_Entity p_Label)
        {
            string strQry = "update P_Label set place = N'QC Area',patrol_date=getdate(),patrol_op=N'" + p_Label.Op_input_wh + "',patrol_result=N'" + p_Label.Patrol_result + "' \n";
            strQry += " where label_code=N'" + p_Label.Label_code + "' \n";
            strQry += "insert into W_HistoryOfTransaction(label_code,product_code,product_customer_code,product_quantity,lot_no,[transaction],[location],pallet_no,input_time,PIC) \n";
            strQry += "select N'" + p_Label.Label_code + "',N'" + p_Label.Product_code + "',N'" + p_Label.Product_customer_code + "',N'" + p_Label.Product_quantity + "',N'" + p_Label.Lot_no + "'";
            strQry += ",N'" + p_Label.Note + "',N'" + p_Label.Wh_location + "',N'" + p_Label.Pallet_no + "',getdate(),N'" + p_Label.Op_input_wh + "' \n";
            conn = new CmCn();
            try
            {
                conn.ExcuteQry(strQry);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void Update_time_to_WH_Location(ObservableCollection<P_Label_Entity> List_label, bool isPallet)
        {
            string strQry = "";
            string qry2 = "";
            if (isPallet)
            {
                foreach (P_Label_Entity item in List_label)
                {
                    strQry += "update P_Label set date_locate_packed=getdate(),location_packed=N'" + item.Wh_location + "',wh_location=N'" + item.Wh_location + "',op_locate_packed=N'" + item.Wh_op_locate + "',place=N'" + item.Place + "' \n";
                    strQry += "where label_code=N'" + item.Label_code + "' \n";
                    if (string.IsNullOrEmpty(qry2))
                    {
                        qry2 += "select N'" + item.Label_code + "',N'" + item.Product_code + "',N'" + item.Product_customer_code + "',N'" + item.Product_quantity + "',N'" + item.Lot_no + "'";
                        qry2 += ",N'" + item.Note + "',N'" + item.Wh_location + "',N'" + item.Pallet_no + "',getdate(),N'" + item.Wh_op_locate + "' \n";
                    }
                    else
                    {
                        qry2 += " union all select N'" + item.Label_code + "',N'" + item.Product_code + "',N'" + item.Product_customer_code + "',N'" + item.Product_quantity + "',N'" + item.Lot_no + "'";
                        qry2 += ",N'" + item.Note + "',N'" + item.Wh_location + "',N'" + item.Pallet_no + "',getdate(),N'" + item.Wh_op_locate + "' \n";
                    }
                }
            }
            else
            {
                foreach (P_Label_Entity item in List_label)
                {
                    strQry += "update P_Label set wh_locate_date=getdate(),wh_location=N'" + item.Wh_location + "',wh_op_locate=N'" + item.Wh_op_locate + "',place=N'" + item.Place + "' \n";
                    strQry += "where label_code=N'" + item.Label_code + "' \n";
                    if (string.IsNullOrEmpty(qry2))
                    {
                        qry2 += "select N'" + item.Label_code + "',N'" + item.Product_code + "',N'" + item.Product_customer_code + "',N'" + item.Product_quantity + "',N'" + item.Lot_no + "'";
                        qry2 += ",N'" + item.Note + "',N'" + item.Wh_location + "',N'" + item.Pallet_no + "',getdate(),N'" + item.Wh_op_locate + "' \n";
                    }
                    else
                    {
                        qry2 += " union all select N'" + item.Label_code + "',N'" + item.Product_code + "',N'" + item.Product_customer_code + "',N'" + item.Product_quantity + "',N'" + item.Lot_no + "'";
                        qry2 += ",N'" + item.Note + "',N'" + item.Wh_location + "',N'" + item.Pallet_no + "',getdate(),N'" + item.Wh_op_locate + "' \n";
                    }
                }
            }
            strQry += "insert into W_HistoryOfTransaction(label_code,product_code,product_customer_code,product_quantity,lot_no,[transaction],[location],pallet_no,input_time,PIC) \n";
            strQry += qry2;
            conn = new CmCn();
            try
            {
                conn.ExcuteQry(strQry);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void Update_time_move_from_Location(ObservableCollection<P_Label_Entity> List_label)
        {
            string strQry = "";
            string qry2 = "";
            foreach (P_Label_Entity item in List_label)
            {
                strQry += "update P_Label set place=N'" + item.Place + "' \n";
                strQry += "where label_code=N'" + item.Label_code + "' \n";
                if (string.IsNullOrEmpty(qry2))
                {
                    qry2 += "select N'" + item.Label_code + "',N'" + item.Product_code + "',N'" + item.Product_customer_code + "',N'" + item.Product_quantity + "',N'" + item.Lot_no + "'";
                    qry2 += ",N'" + item.Note + "',N'" + item.Wh_location + "',N'" + item.Pallet_no + "',getdate(),N'" + item.Wh_op_locate + "' \n";
                }
                else
                {
                    qry2 += " union all select N'" + item.Label_code + "',N'" + item.Product_code + "',N'" + item.Product_customer_code + "',N'" + item.Product_quantity + "',N'" + item.Lot_no + "'";
                    qry2 += ",N'" + item.Note + "',N'" + item.Wh_location + "',N'" + item.Pallet_no + "',getdate(),N'" + item.Wh_op_locate + "' \n";
                }
            }
            strQry += "insert into W_HistoryOfTransaction(label_code,product_code,product_customer_code,product_quantity,lot_no,[transaction],[location],pallet_no,input_time,PIC) \n";
            strQry += qry2;
            conn = new CmCn();
            try
            {
                conn.ExcuteQry(strQry);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void Update_Material_Location(ObservableCollection<W_M_ReceiveLabel_Entity> List_label)
        {
            string strQry = "";
            string qry2 = "";
            foreach (W_M_ReceiveLabel_Entity item in List_label)
            {
                strQry += "update W_M_ReceiveLabel set locate_time=getdate(),wh_location=N'" + item.Wh_location + "',op_locate=N'" + item.Op_locate + "' \n";
                strQry += "where label_code=N'" + item.Whmr_code + "' \n";
                if (string.IsNullOrEmpty(qry2))
                {
                    qry2 += "select N'" + item.Whmr_code + "',N'" + item.M_name + "',N'" + item.Quantity + "',N'" + item.Transaction + "'";
                    qry2 += ",N'" + item.Wh_location + "',getdate(),N'" + item.Op_locate + "',N'" + item.Place + "' \n";
                }
                else
                {
                    qry2 += " union all select N'" + item.Whmr_code + "',N'" + item.M_name + "',N'" + item.Quantity + "',N'" + item.Transaction + "'";
                    qry2 += ",N'" + item.Wh_location + "',getdate(),N'" + item.Op_locate + "',N'" + item.Place + "' \n";
                }
            }
            strQry += "insert into W_M_HistoryOfTransaction([whmr_code],[m_name],[quantity],[transaction],[location],[input_time],[PIC],[place]) \n";
            strQry += qry2;
            conn = new CmCn();
            try
            {
                conn.ExcuteQry(strQry);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void Update_time_to_Packing_zone(List<P_Label_Entity> List_label)
        {
            string strQry = "";
            string qry2 = "";
            foreach (P_Label_Entity item in List_label)
            {
                strQry += "update P_Label set place=N'" + item.Place + "',\n";
                strQry += "date_input_packing_zone=N'" + item.Date_input_packing_zone.ToString("yyyy-MM-dd hh:mm:ss") + "',op_input_packing_zone=N'" + item.Op_input_packing_zone + "',pallet_no=N'" + item.Pallet_no + "',location_packed=null,op_locate_packed=null,date_locate_packed=null,isLock=N'Unblock'\n";
                strQry += "where label_code=N'" + item.Label_code + "' \n";
                if (string.IsNullOrEmpty(qry2))
                {
                    qry2 += "select N'" + item.Label_code + "',N'" + item.Product_code + "',N'" + item.Product_customer_code + "',N'" + item.Product_quantity + "',N'" + item.Lot_no + "'";
                    qry2 += ",N'" + item.Note + "',N'',N'" + item.Pallet_no + "',getdate(),N'" + item.Op_input_packing_zone + "' \n";
                }
                else
                {
                    qry2 += " union all select N'" + item.Label_code + "',N'" + item.Product_code + "',N'" + item.Product_customer_code + "',N'" + item.Product_quantity + "',N'" + item.Lot_no + "'";
                    qry2 += ",N'" + item.Note + "',N'',N'" + item.Pallet_no + "',getdate(),N'" + item.Op_input_packing_zone + "' \n";
                }
            }
            strQry += "insert into W_HistoryOfTransaction(label_code,product_code,product_customer_code,product_quantity,lot_no,[transaction],[location],pallet_no,input_time,PIC) \n";
            strQry += qry2;
            conn = new CmCn();
            try
            {
                conn.ExcuteQry(strQry);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void Insert_TEMP_W_ShippingInfor(P_Label_Entity item)
        {
            string strQry = "Insert into TEMP_W_ShippingInfor ([label_code],[product_code],[product_customer_code],[product_quantity], \n ";
            strQry += " [lot_no],[transaction],[pallet_no],[input_time],[PIC],[invoice_no],[truck_no],[isActive],[is_check],[error]) values \n ";
            strQry += " (N'" + item.Label_code + "',N'" + item.Product_code + "',N'" + item.Product_customer_code + "',N'" + item.Product_quantity + "' \n ";
            strQry += " ,N'" + item.Lot_no + "',N'" + item.Note + "',N'" + item.Pallet_no + "',getdate() \n ";
            strQry += " ,N'" + item.Ship_op + "',N'" + item.Invoice_no + "',N'" + item.Truck_no + "',N'0',N'" + item.Check + "',N'" + item.Error + "') \n ";
            try
            {
                conn = new CmCn();
                conn.ExcuteQry(strQry);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public void Insert_TEMP_W_M_Receiving(W_M_ReceiveLabel_Entity item)
        {
            string strQry = "Insert into TEMP_W_M_Receiving ([whmr_code],[m_name],[quantity],[rm_doc_id] \n ";
            strQry += " ,[wh_op],[wh_receive_time],[truck_no],[place],[transaction],[is_check],[wh_okng],is_active) values \n ";
            strQry += " (N'" + item.Whmr_code + "',N'" + item.M_name + "',N'" + item.Quantity + "' \n ";
            strQry += " ,N'" + item.Rm_doc_id + "',N'" + item.Wh_op + "',getdate() \n ";
            strQry += " ,N'" + item.Truck_no + "',N'" + item.Place + "',N'" + item.Transaction + "',N'" + item.Is_check + "',N'OK',N'0') \n ";
            try
            {
                conn = new CmCn();
                conn.ExcuteQry(strQry);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public void Insert_TEMP_W_R_Receiving(W_M_RubberLabel_Entity item)
        {
            string strQry = "Insert into TEMP_W_R_Receiving ([whrr_code],[r_name],[weight],[rm_doc_id] \n ";
            strQry += " ,[wh_op],[wh_receive_time],[truck_no],[place],[transaction],[is_check],[wh_okng],is_active) values \n ";
            strQry += " (N'" + item.Whrr_code + "',N'" + item.R_name + "',N'" + item.Weight + "' \n ";
            strQry += " ,N'" + item.Rm_doc_id + "',N'" + item.Wh_op + "',getdate() \n ";
            strQry += " ,N'" + item.Truck_no + "',N'" + item.Place + "',N'" + item.Transaction + "',N'" + item.Is_check + "',N'OK',N'0') \n ";
            try
            {
                conn = new CmCn();
                conn.ExcuteQry(strQry);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public void Confirm_Shipping(string truck_no, DataTable dt)
        {
            string strQry = "";
            foreach (DataRow item in dt.Rows)
            {
                DateTime Input_Time = DateTime.Parse(item["input_time"].ToString());
                strQry += "update P_label set ship_date=N'" + Input_Time.ToString("yyyy-MM-dd hh:mm:ss") + "',ship_op=N'" + item["PIC"].ToString() + "',place=N'Shipped',";
                strQry += "invoice_no=N'" + item["invoice_no"].ToString() + "'\n";
                strQry += "where label_code = N'" + item["label_code"].ToString() + "'\n";
            }
            strQry += " insert into W_HistoryOfTransaction ([label_code],[product_code],[product_customer_code],[product_quantity],[lot_no], \n";
            strQry += " [transaction],[pallet_no],[input_time],[PIC],[invoice_no],[truck_no]) \n";
            strQry += " SELECT [label_code],[product_code],[product_customer_code],[product_quantity],[lot_no], \n ";
            strQry += " [transaction],[pallet_no],[input_time],[PIC],[invoice_no],[truck_no] \n ";
            strQry += " from TEMP_W_ShippingInfor ";
            strQry += " where truck_no=N'" + truck_no + "' and isActive=N'0' \n ";
            strQry += "update TEMP_W_ShippingInfor set isActive=N'1' where truck_no=N'" + truck_no + "'\n";
            conn = new CmCn();
            conn.ExcuteQry(strQry);
        }
        public void Confirm_Receiving(string truck_no, DataTable dt)
        {
            string strQry = "";
            foreach (DataRow item in dt.Rows)
            {
                DateTime Input_Time = DateTime.Parse(item["wh_receive_time"].ToString());
                strQry += "update W_M_ReceiveLabel set wh_receive_time=N'" + Input_Time.ToString("yyyy-MM-dd hh:mm:ss") + "',wh_op=N'" + item["wh_op"].ToString() + "',place=N'WH Material',";
                strQry += "rm_doc_id=N'" + item["rm_doc_id"].ToString() + "'\n";
                strQry += "where whmr_code = N'" + item["whmr_code"].ToString() + "'\n";
            }
            strQry += " insert into [W_M_HistoryOfTransaction] ([whmr_code],[m_name],[quantity],[transaction],[input_time],[PIC],[invoice_no],[truck_no]) \n";
            strQry += " SELECT [whmr_code],[m_name],[quantity],[transaction],[wh_receive_time],[wh_op],[rm_doc_id],[truck_no] \n ";
            strQry += " from TEMP_W_M_Receiving ";
            strQry += " where truck_no=N'" + truck_no + "'\n ";
            conn = new CmCn();
            conn.ExcuteQry(strQry);
        }
        public void Update_time_Unpacking(List<P_Label_Entity> List_label)
        {
            string strQry = "";
            string qry2 = "";
            foreach (P_Label_Entity item in List_label)
            {
                strQry += "update P_Label set op_input_wh=N'" + item.Op_input_wh + "',wh_locate_date=null,wh_location=null,wh_op_locate=null,date_input_packing_zone=null,op_input_packing_zone=null,date_packed=null,op_packed=null,pallet_no=null,date_locate_packed=null,location_packed=null,op_locate_packed=null \n";
                strQry += "where label_code=N'" + item.Label_code + "' \n";
                if (string.IsNullOrEmpty(qry2))
                {
                    qry2 += "select N'" + item.Label_code + "',N'" + item.Product_code + "',N'" + item.Product_customer_code + "',N'" + item.Product_quantity + "',N'" + item.Lot_no + "'";
                    qry2 += ",N'" + item.Note + "',N'',N'',getdate(),N'" + item.Wh_op_locate + "' \n";
                }
                else
                {
                    qry2 += " union all select N'" + item.Label_code + "',N'" + item.Product_code + "',N'" + item.Product_customer_code + "',N'" + item.Product_quantity + "',N'" + item.Lot_no + "'";
                    qry2 += ",N'" + item.Note + "',N'',N'',getdate(),N'" + item.Wh_op_locate + "' \n";
                }
            }
            strQry += "insert into W_HistoryOfTransaction(label_code,product_code,product_customer_code,product_quantity,lot_no,[transaction],[location],pallet_no,input_time,PIC) \n";
            strQry += qry2;
            conn = new CmCn();
            try
            {
                conn.ExcuteQry(strQry);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void Update_detail_invoice(List<LOG_InvoiceDetail_Entity> List_data, string invoice_no, string truck_no, DateTime ship_date)
        {
            string strQry = "";
            string qry2 = "";
            foreach (LOG_InvoiceDetail_Entity item in List_data)
            {
                if (item.Quantity > 0)
                {
                    if (string.IsNullOrEmpty(qry2))
                    {
                        qry2 += "select N'" + invoice_no + "',N'" + item.Product_customer_code + "',N'" + item.Product_name + "',N'" + item.Quantity + "',N'" + item.Unit + "',N'" + item.Hs_code + "' \n";
                    }
                    else
                    {
                        qry2 += "union all select N'" + invoice_no + "',N'" + item.Product_customer_code + "',N'" + item.Product_name + "',N'" + item.Quantity + "',N'" + item.Unit + "',N'" + item.Hs_code + "' \n";
                    }
                }
            }
            if (qry2 != "")
            {
                strQry = "delete from LOG_Invoice where invoice_no=N'" + invoice_no + "' \n";
                strQry += "insert into LOG_Invoice ([invoice_no],[truck_no],[last_user_commit],[last_time_commit],ship_date) \n";
                strQry += "values (N'" + invoice_no + "',N'" + truck_no + "',N'" + General_Infor.username + "',getdate(),N'" + ship_date.ToString("yyyy-MM-dd") + "') \n";
                strQry += "delete from LOG_InvoiceDetail where invoice_no=N'" + invoice_no + "' \n";
                strQry += "insert into LOG_InvoiceDetail(invoice_no,product_customer_code,product_name,quantity,unit,hs_code) \n";
                strQry += qry2;
            }
            conn = new CmCn();
            try
            {
                conn.ExcuteQry(strQry);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void Update_P_MasterListProduct_BOM(List<P_MasterListProduct_BOM_Entity> List_data, string Product_cust_code)
        {
            string strQry = "";
            string qry2 = "";
            foreach (P_MasterListProduct_BOM_Entity item in List_data)
            {
                if (item.M_quantity > 0)
                {
                    if (string.IsNullOrEmpty(qry2))
                    {
                        qry2 += "select N'" + Product_cust_code + "',N'" + item.M_name + "',N'" + item.M_quantity + "' \n";
                    }
                    else
                    {
                        qry2 += "union all select N'" + Product_cust_code + "',N'" + item.M_name + "',N'" + item.M_quantity + "' \n";
                    }
                }
            }
            if (qry2 != "")
            {
                strQry += "delete from P_MasterListProduct_BOM where product_customer_code=N'" + Product_cust_code + "' \n";
                strQry += "insert into P_MasterListProduct_BOM(product_customer_code,m_name,m_quantity) \n";
                strQry += qry2;
            }
            conn = new CmCn();
            try
            {
                conn.ExcuteQry(strQry);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void Update_W_M_IssueDoc(List<W_M_IssueDocDetail_Entity> List_data, string doc_id, DateTime supply_date)
        {
            string strQry = "";
            string qry2 = "";
            foreach (W_M_IssueDocDetail_Entity item in List_data)
            {
                if (!string.IsNullOrEmpty(item.M_name))
                {
                    if (string.IsNullOrEmpty(qry2))
                    {
                        qry2 += "select N'" + doc_id + "',N'" + item.M_name + "',N'" + item.Product_customer_code + "',N'" + item.M_demand + "',N'" + item.P_line + "',N'" + item.P_shift + "' \n";
                    }
                    else
                    {
                        qry2 += "union all select N'" + doc_id + "',N'" + item.M_name + "',N'" + item.Product_customer_code + "',N'" + item.M_demand + "',N'" + item.P_line + "',N'" + item.P_shift + "' \n";
                    }
                }
            }
            if (qry2 != "")
            {
                strQry = "delete from W_M_IssueDoc where m_doc_id=N'" + doc_id + "' \n";
                strQry += "insert into W_M_IssueDoc ([m_doc_id],[last_user_commit],[last_time_commit],[m_doc_status],m_doc_supply_date) \n";
                strQry += "values (N'" + doc_id + "',N'" + General_Infor.username + "',getdate(),N'Processing',N'" + supply_date.ToString("yyyy-MM-dd") + "') \n";
                strQry += "delete from W_M_IssueDocDetail where m_doc_id=N'" + doc_id + "' \n";
                strQry += "insert into W_M_IssueDocDetail(m_doc_id,m_name,product_customer_code,m_demand,p_line,p_shift) \n";
                strQry += qry2;
            }
            conn = new CmCn();
            try
            {
                conn.ExcuteQry(strQry);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void Update_W_M_ReceiveDoc(List<W_M_ReceiveDocDetail_Entity> List_data, W_M_ReceiveDoc_Entity Doc)
        {
            string strQry = "";
            string qry2 = "";
            foreach (W_M_ReceiveDocDetail_Entity item in List_data)
            {
                if (item.Quantity > 0)
                {
                    if (string.IsNullOrEmpty(qry2))
                    {
                        qry2 += "select N'" + Doc.Rm_doc_id + "',N'" + item.M_name + "',N'" + item.Quantity + "',N'" + item.Number_carton + "',N'" + item.Lot_no + "' \n";
                    }
                    else
                    {
                        qry2 += "union all select N'" + Doc.Rm_doc_id + "',N'" + item.M_name + "',N'" + item.Quantity + "',N'" + item.Number_carton + "',N'" + item.Lot_no + "' \n";
                    }
                }
            }
            if (qry2 != "")
            {
                strQry = "delete from W_M_ReceiveDoc where rm_doc_id=N'" + Doc.Rm_doc_id + "' \n";
                strQry += "insert into W_M_ReceiveDoc ([rm_doc_id],[supplier],[receive_date],[last_user_commit],[last_time_commit],[rm_doc_link],[truck_no],[rm_doc_name],[rm_doc_link2],[rm_doc_link3],[rm_doc_name2],[rm_doc_name3],rm_kind) \n";
                strQry += "values (N'" + Doc.Rm_doc_id + "',N'" + Doc.Supplier + "',N'" + Doc.Receive_date.ToString("yyyy-MM-dd") + "',N'" + General_Infor.username + "',getdate(),N'" + Doc.Rm_doc_link + "',N'" + Doc.Truck_no + "',N'" + Doc.Rm_doc_name + "',N'" + Doc.Rm_doc_link2 + "',N'" + Doc.Rm_doc_link3 + "',N'" + Doc.Rm_doc_name2 + "',N'" + Doc.Rm_doc_name3 + "',N'" + Doc.Rm_kind + "') \n";
                strQry += "delete from W_M_ReceiveDocDetail where rm_doc_id=N'" + Doc.Rm_doc_id + "' \n";
                strQry += "insert into W_M_ReceiveDocDetail([rm_doc_id],[m_name],[quantity],[number_carton],[m_lot_no]) \n";
                strQry += qry2;
            }
            conn = new CmCn();
            try
            {
                conn.ExcuteQry(strQry);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void Confirm_W_M_ReceiveDoc(List<W_M_ReceiveDocDetail_Entity> List_data)
        {
            string strQry = "";
            foreach (W_M_ReceiveDocDetail_Entity item in List_data)
            {
                strQry += "update W_M_ReceiveDocDetail set  qty_receive=N'" + item.Qty_receive + "' where rm_doc_id=N'" + item.Rm_doc_id + "' and m_name=N'" + item.M_name + "' \n";
            }
            conn = new CmCn();
            try
            {
                conn.ExcuteQry(strQry);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void Insert_TEMP_W_M_IssueToQC(W_M_ReceiveLabel_Entity item)
        {
            string strQry = "DELETE FROM TEMP_W_M_IssueToQC WHERE [whmr_code] =N'" + item.Whmr_code + "' \n";
            strQry += "insert into TEMP_W_M_IssueToQC([whmr_code],[m_name],[quantity],[wh_op],[wh_issue_time],[transaction],[place],[is_check],[rm_plan_id],[qc_shift]) \n";
            strQry += "values (N'" + item.Whmr_code + "',N'" + item.M_name + "',N'" + item.Quantity + "',N'" + item.Pic_issue_qc + "',getdate()" +
                ",N'" + item.Transaction + "',N'" + item.Place + "',N'" + item.Is_check + "',N'" + item.Rm_plan_id + "',N'" + item.Qc_shift + "')";
            conn = new CmCn();
            try
            {
                conn.ExcuteQry(strQry);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void Update_W_M_CheckingPlan(List<W_M_CheckingPlanDetail_Entity> List_data, string plan_id, DateTime Checking_date)
        {
            string strQry = "";
            string qry2 = "";
            foreach (W_M_CheckingPlanDetail_Entity item in List_data)
            {
                if (!string.IsNullOrEmpty(item.M_name))
                {
                    if (string.IsNullOrEmpty(qry2))
                    {
                        qry2 += "select N'" + plan_id + "',N'" + item.M_name + "',N'" + item.Quantity + "',N'" + item.P_shift + "',N'" + item.Plan_type + "' \n";
                    }
                    else
                    {
                        qry2 += "union all select N'" + plan_id + "',N'" + item.M_name + "',N'" + item.Quantity + "',N'" + item.P_shift + "',N'" + item.Plan_type + "' \n";
                    }
                }
            }
            if (qry2 != "")
            {
                strQry = "delete from W_M_CheckingPlan where rm_plan_id=N'" + plan_id + "' \n";
                strQry += "insert into W_M_CheckingPlan ([rm_plan_id],[check_date],[last_time_commit],[last_user_commit]) \n";
                strQry += "values (N'" + plan_id + "',N'" + Checking_date.ToString("yyyy-MM-dd") + "',getdate(),N'" + General_Infor.username + "') \n";
                strQry += "delete from W_M_CheckingPlanDetail where rm_plan_id=N'" + plan_id + "' \n";
                strQry += "insert into W_M_CheckingPlanDetail([rm_plan_id],[m_name],[quantity],[p_shift],plan_type) \n";
                strQry += qry2;
            }
            conn = new CmCn();
            try
            {
                conn.ExcuteQry(strQry);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string Current_month()
        {
            string strQry = "select values_number from ADM_DurationControl where parent_name=N'working_month' and start_date<=N'" + DateTime.Today.ToString("yyyy-MM-dd") + "' and end_date>=N'" + DateTime.Today.ToString("yyyy-MM-dd") + "'";
            conn = new CmCn();
            try
            {
                return conn.ExcuteString(strQry);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string Current_month_name()
        {
            string strQry = "select values_string from ADM_DurationControl where parent_name=N'working_month' and start_date<=N'" + DateTime.Today.ToString("yyyy-MM-dd") + "' and end_date>=N'" + DateTime.Today.ToString("yyyy-MM-dd") + "'";
            conn = new CmCn();
            try
            {
                return conn.ExcuteString(strQry);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void Delete_invoice(string invoice_no)
        {
            string strQry = "delete from LOG_Invoice where invoice_no=N'" + invoice_no + "' \n";
            strQry += "delete from LOG_InvoiceDetail where invoice_no=N'" + invoice_no + "' ";
            conn = new CmCn();
            try
            {
                conn.ExcuteQry(strQry);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void Delete_W_M_IssueDocDetail(string m_doc_id)
        {
            string strQry = "delete from W_M_IssueDoc where m_doc_id=N'" + m_doc_id + "' \n";
            strQry += "delete from W_M_IssueDocDetail where m_doc_id=N'" + m_doc_id + "' \n";
            conn = new CmCn();
            try
            {
                conn.ExcuteQry(strQry);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void Delete_W_M_ReceiveDocDetail(string m_doc_id)
        {
            string strQry = "delete from W_M_ReceiveDoc where rm_doc_id=N'" + m_doc_id + "' \n";
            strQry += "delete from W_M_ReceiveDocDetail where rm_doc_id=N'" + m_doc_id + "' \n";
            conn = new CmCn();
            try
            {
                conn.ExcuteQry(strQry);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void Delete_PR(string pr_no)
        {
            string strQry = "update PUR_PR set is_active='No' where pr_no=N'" + pr_no + "' ";
            conn = new CmCn();
            try
            {
                conn.ExcuteQry(strQry);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void SendEmail(string Subject, string To, string Cc, string Body)
        {
            Outlook.Application app = new Outlook.Application();
            Outlook.MailItem mailItem = app.CreateItem(Outlook.OlItemType.olMailItem);
            mailItem.Subject = Subject;
            mailItem.To = To;
            if (!string.IsNullOrEmpty(Cc))
            {
                mailItem.CC = Cc;
            }
            mailItem.Body = Body;
            //mailItem.Attachments.Add(logPath);//logPath is a string holding path to the log.txt file
            //mailItem.Importance = Outlook.OlImportance.olImportanceHigh;
            try
            {
                if (!string.IsNullOrEmpty(To))
                {
                    mailItem.Send();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
