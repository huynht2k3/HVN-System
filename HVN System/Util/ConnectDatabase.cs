using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace HVN_System
{
    public class ConnectDatabase
    {
        public static string ConnectionString = "server=172.16.180.24;database=HVN_SYS;uid=ksyserp;pwd=ksyserp";
        public static SqlConnection Connect
        {
            get
            {
                SqlConnection con = new SqlConnection(ConnectionString);
                try
                {
                    con.Open();
                    return con;
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }
    }
}
