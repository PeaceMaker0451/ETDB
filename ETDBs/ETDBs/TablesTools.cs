using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETDBs
{
    public class TablesTools
    {
        public static DataTable SearchInDataTable(DataTable originalTable, string searchText, string columnName = "")
        {
            if (string.IsNullOrEmpty(searchText))
                return originalTable; // Возвращаем оригинальную таблицу, если строка поиска пуста

            // Создаем новый DataTable, чтобы хранить результаты поиска
            DataTable filteredTable = originalTable.Clone(); // Клонируем структуру оригинальной таблицы

            // Проверяем, указан ли столбец для поиска
            if (!string.IsNullOrEmpty(columnName) && !originalTable.Columns.Contains(columnName))
            {
                throw new ArgumentException($"Столбец с именем '{columnName}' не существует в таблице.");
            }

            // Проходим по всем строкам оригинальной таблицы
            foreach (DataRow row in originalTable.Rows)
            {
                if (string.IsNullOrEmpty(columnName))
                {
                    // Если имя колонки не указано, проверяем каждую ячейку в строке
                    foreach (var item in row.ItemArray)
                    {
                        if (item.ToString().IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            // Если нашли совпадение, добавляем строку в новую таблицу
                            filteredTable.ImportRow(row);
                            break; // Прерываем цикл, так как достаточно одного совпадения
                        }
                    }
                }
                else
                {
                    // Если имя колонки указано, проверяем только указанную колонку
                    if (row[columnName].ToString().IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        filteredTable.ImportRow(row);
                    }
                }
            }

            return filteredTable; // Возвращаем таблицу с отфильтрованными строками
        }
    }
}
