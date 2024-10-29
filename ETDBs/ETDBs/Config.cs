﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace ETDBs
{
    internal class Config
    {
        public string ProgramMode = "";
        public string DBConnectionPath = "Server=localhost\\SQLEXPRESS01;Trusted_Connection=True;";

        public void SaveToFile(string filePath)
        {
            try
            {
                // Сериализация объекта в JSON-строку
                string jsonData = JsonConvert.SerializeObject(this, Formatting.Indented);

                // Запись JSON-строки в файл
                File.WriteAllText(filePath, jsonData);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении файла конфигурации: {ex.Message}");
            }
        }

        // Метод для чтения конфигурации из JSON-файла
        public static Config LoadFromFile(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                    throw new FileNotFoundException("Файл конфигурации не найден.");

                // Чтение JSON-строки из файла
                string jsonData = File.ReadAllText(filePath);

                // Десериализация JSON-строки в объект Config
                return JsonConvert.DeserializeObject<Config>(jsonData);
            }
            catch (Exception ex)
            {
                //MessageBox.Show($"Ошибка при загрузке файла конфигурации: {ex.Message}");
                return null;
            }
        }
    }
}
