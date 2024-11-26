using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace ETDBs
{
    internal static class Program
    {
        public static Config config;
        public static string configPath;
        public static DBManager dbManager;

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            LoadAndRunApplication();
        }

        static void LoadAndRunApplication()
        {
            string localAppDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

            // Создаем папку для вашей программы, если она еще не создана
            string programFolder = Path.Combine(localAppDataPath, "ETDB");
            if (!Directory.Exists(programFolder))
            {
                Directory.CreateDirectory(programFolder);
            }

            configPath = Path.Combine(programFolder, "config.json");
            config = LoadOrInitializeConfig();

            var configForm = new ConfigForm(config);
            var splashScreen = new SplashScreen();

            bool appReady = false;

            bool starting = true;

            while (!appReady)
            {
                splashScreen.Hide();

                if (config.alwaysConfig || starting == false)
                {
                    if (configForm.ShowDialog() == DialogResult.Cancel)
                    {
                        // Если окно ConfigForm закрыто с Cancel, завершаем приложение
                        Application.Exit();
                        return;
                    }

                    config.SaveToFile(configPath);
                }

                splashScreen.Show();
                
                starting = false;

                var dbManager = InitializeDatabaseManager(config, configForm);

                if (dbManager == null)
                    continue;

                splashScreen.Hide();

                appReady = StartApplication(config, dbManager);
            }
        }

        static Config LoadOrInitializeConfig()
        {
            var config = Config.LoadFromFile(configPath);

            config = config ?? new Config();
            return config;
        }

        static DBManager InitializeDatabaseManager(Config config, ConfigForm configForm)
        {
            try
            {
                return new DBManager(config.DBConnectionPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Невозможно подключиться к базе данных - {ex.Message}");
                return null; // Возвращаем null, если подключение не удалось
            }
        }

        static bool StartApplication(Config config, DBManager dbManager)
        {
            switch (config.ProgramMode)
            {
                case 0:
                    return RunMainForm(new EmployeesManagement(dbManager, config), config);

                case 1:
                    return RunMainForm(new EventsManagement(dbManager, config), config);
                case 2:
                    return RunMainForm(new EventsPlanning(dbManager, config), config);

                default:
                    MessageBox.Show("Неправильный режим программы");
                    return false;
            }
        }

        static bool RunMainForm(Form mainForm, Config config)
        {
            //mainForm.FormClosed += (sender, args) =>
            //{
            //    if (mainForm.DialogResult == DialogResult.Retry)
            //    {
            //        LoadAndRunApplication(); // Возврат к конфигурации
            //    }
            //};

            if(mainForm.ShowDialog() == DialogResult.Retry)
            {
                return false;
            }
            return true;
        }

        public static void SetFormSize(Form form)
        {
            form.Font = new Font(form.Font.FontFamily, config.textSize);
        }
    }
}
