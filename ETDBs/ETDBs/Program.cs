using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ETDBs
{
    internal static class Program
    {
        public static Config config;
        public static DBManager dbManager;

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var splash = new SplashScreen();

            splash.Show();

            config = Config.LoadFromFile(Path.Combine(Application.StartupPath, "config.json"));

            if (config == null)
            {
                config = new Config();
                config.DBConnectionPath = "Server=localhost\\SQLEXPRESS;Database=EmployeesEventsDB;Trusted_Connection=True;";
            }

            dbManager = new DBManager(config.DBConnectionPath);

            splash.Hide();

            Application.Run(new EmployeesManagement(dbManager));
        }
    }
}
