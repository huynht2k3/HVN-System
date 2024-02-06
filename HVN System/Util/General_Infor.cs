using HVN_System.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HVN_System.Util
{
    public class General_Infor
    {
        public static ADM_Account myaccount;
        public static string version= "1.0.0.229";
        public static string KPI_month;
        public static string KPI_month_name;
        public static string KPI_year;
        public static string username;
        public static DateTime default_time= new DateTime(1990,9,2);
        public static List<ADM_Permission_Entity> List_permission;
    }
}
 