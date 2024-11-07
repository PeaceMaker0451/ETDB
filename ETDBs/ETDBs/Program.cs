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

            var configForm = new ConfigForm(config);

            if (config.alwaysConfig)
            {
                configForm.ShowDialog();
            }

            bool notLoaded = true;
            
            while(notLoaded)
            {
                try
                {
                    dbManager = new DBManager(config.DBConnectionPath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Невозможно подключиться к базе данных - {ex.Message}");
                    configForm.ShowDialog();
                    continue;
                }
                

                splash.Hide();

                switch (config.ProgramMode)
                {
                    case 0:
                        notLoaded = false;
                        Application.Run(new EmployeesManagement(dbManager));
                        break;
                    case 1:
                        notLoaded = false;
                        Application.Run(new EventsManagement(dbManager,config));
                        break;
                    default:
                        MessageBox.Show($"Неправильный режим программы");
                        configForm.ShowDialog();
                        continue;
                }
            }
        }
    }
}
