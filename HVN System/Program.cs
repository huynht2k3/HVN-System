using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DevExpress.UserSkins;
using DevExpress.Skins;
using HVN_System.View.Production;
using HVN_System.View.Planning;
using HVN_System.View.PlantKPI;
using HVN_System.View.Warehouse;
using HVN_System.View.Admin;
using HVN_System.View.HR;
using HVN_System.View.QC;

namespace HVN_System
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new View.frmLogin());
            //Application.Run(new frmQCCheckingMaterialDetail());
        }
    }
}
