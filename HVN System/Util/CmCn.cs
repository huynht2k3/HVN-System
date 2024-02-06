using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace HVN_System
{
    class CmCn
    {
        string SrvName = "172.16.180.24";
        string TargetDB = "HVN_SYS";
        //string DBKind = "SQL";
        string strCon = "";
        SqlConnection sqlcon = null;
        SqlCommand sqlcmd = null;
        DataTable dt = null;
        SqlDataAdapter sqladapt = null;

        public CmCn()
        {
            this.SrvName = "172.16.180.24";
            this.TargetDB = "HVN_SYS";
        }
        public CmCn(string SrvName, string TargetDB)
        {
            this.SrvName = SrvName;
            this.TargetDB = TargetDB;
        }

        private void OpenDB()
        {
            this.SrvName = string.IsNullOrEmpty(this.SrvName) ? "172.16.180.24" : this.SrvName;
            this.TargetDB = string.IsNullOrEmpty(this.TargetDB) ? "HVN_SYS" : this.TargetDB;

            strCon = @"Data Source=" + this.SrvName + ";Initial Catalog=" + this.TargetDB + ";Persist Security Info=True;User ID=ksyserp; Password=ksyserp";
            sqlcon = new SqlConnection(strCon);
            try
            {
                sqlcon.Open();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //insert,update,delete
        public void ExcuteQry(string strQry)
        {
            if (!string.IsNullOrEmpty(strQry))
            {
                OpenDB();
                sqlcmd = new SqlCommand(strQry, sqlcon);
                sqlcmd.CommandTimeout = 100000;
                try
                {
                    sqlcmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    sqlcon.Close();
                    sqlcon = null;
                }
            }
        }
        //get data return datatable
        public DataTable ExcuteDataTable(string strQry)
        {
            dt = new DataTable();
            sqladapt = new SqlDataAdapter();
            if (!string.IsNullOrEmpty(strQry))
            {
                OpenDB();
                try
                {
                    sqlcmd = new SqlCommand(strQry, sqlcon);
                    sqlcmd.CommandTimeout = 100000;
                    sqladapt.SelectCommand = sqlcmd;
                    sqladapt.Fill(dt);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    sqlcon.Close();
                    sqlcon = null;
                }
            }
            return dt;
        }
        //get data return string
        public string ExcuteString(string strQry)
        {
            string values = "";
            dt = new DataTable();
            sqladapt = new SqlDataAdapter();
            if (!string.IsNullOrEmpty(strQry))
            {
                OpenDB();
                try
                {
                    sqlcmd = new SqlCommand(strQry, sqlcon);
                    sqlcmd.CommandTimeout = 100000;
                    sqladapt.SelectCommand = sqlcmd;
                    sqladapt.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        values = dt.Rows[0][0].ToString();
                    }
                }
                catch (Exception)
                {
                }
                finally
                {
                    sqlcon.Close();
                    sqlcon = null;
                }
            }
            return values;
        }
        //get data return true/false
        public bool ExcuteReturnBool(string strQry)
        {
            dt = new DataTable();
            sqladapt = new SqlDataAdapter();
            if (!string.IsNullOrEmpty(strQry))
            {
                OpenDB();
                try
                {
                    sqlcmd = new SqlCommand(strQry, sqlcon);
                    sqlcmd.CommandTimeout = 100000;
                    sqladapt.SelectCommand = sqlcmd;
                    sqladapt.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        return true;
                    }
                }
                catch (Exception)
                {
                }
                finally
                {
                    sqlcon.Close();
                    sqlcon = null;
                }
            }
            return false;
        }
    }
}
