using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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

        public static DataTable FilterDataTable(DataTable originalTable, string searchText, int columnIndex)
        {
            if (searchText == null || searchText == "")
                return originalTable;
            // Создаем новый DataTable с такой же структурой, как у исходной таблицы
            DataTable filteredTable = originalTable.Clone();

            // Проходим по всем строкам в исходной таблице
            foreach (DataRow row in originalTable.Rows)
            {
                // Если значение в указанном столбце совпадает с поисковым текстом
                if (row[columnIndex].ToString() == searchText)
                {
                    // Добавляем строку в новую таблицу
                    filteredTable.ImportRow(row);
                }
            }

            return filteredTable;
        }

        public static DataTable FilterDataTable(DataTable originalTable, string searchText, string columnName)
        {
            if (searchText == null || searchText == "")
                return originalTable;
            // Создаем новый DataTable с такой же структурой, как у исходной таблицы
            DataTable filteredTable = originalTable.Clone();

            // Проверяем, существует ли столбец с таким именем
            if (originalTable.Columns.Contains(columnName))
            {
                // Получаем индекс столбца по имени
                int columnIndex = originalTable.Columns[columnName].Ordinal;

                // Проходим по всем строкам в исходной таблице
                foreach (DataRow row in originalTable.Rows)
                {
                    // Если значение в указанном столбце совпадает с поисковым текстом
                    if (row[columnIndex].ToString() == searchText)
                    {
                        // Добавляем строку в новую таблицу
                        filteredTable.ImportRow(row);
                    }
                }
            }
            else
            {
                throw new ArgumentException("Столбец с таким именем не существует в таблице.", nameof(columnName));
            }

            return filteredTable;
        }

        public static DateTime CalculateNextEventDate(DateTime startDate, bool isMonths, int toNextValue)
        {
            DateTime nextEventDate = startDate;

            if(toNextValue <= 0)
            {
                return nextEventDate;
            }
            
            while (nextEventDate <= DateTime.Today)
            {
                if (isMonths)
                {
                    nextEventDate = nextEventDate.AddMonths(toNextValue);
                }
                else
                {
                    nextEventDate = nextEventDate.AddDays(toNextValue);
                }
            }

            return nextEventDate;
        }

        public static DateTime CalculateNextEventDate(DateTime startDate, bool isMonths, int toNextValue, int count)
        {
            DateTime nextEventDate = startDate;

            if (toNextValue <= 0)
            {
                return nextEventDate;
            }

            int i = 0;

            while (i < count )
            {
                if (isMonths)
                {
                    nextEventDate = nextEventDate.AddMonths(toNextValue);
                }
                else
                {
                    nextEventDate = nextEventDate.AddDays(toNextValue);
                }

                i++;
            }

            return nextEventDate;
        }

        public static void CopyDataTableToDataGridView(DataTable table, DataGridView dataGridView)
        {
            dataGridView.Columns.Clear();
            dataGridView.Rows.Clear();

            DataTable newTable = new DataTable();

            foreach (DataColumn column in table.Columns)
            {
                dataGridView.Columns.Add(column.ColumnName, column.ColumnName);
                //newTable.Columns.Add(column.ColumnName, column.ColumnName);
            }

            foreach (DataRow row in table.Rows)
            {
                var rowData = row.ItemArray;
                dataGridView.Rows.Add(rowData);
            }
        }

        public static void NotifyEvents(DataGridView dataGridView, int daysThreshold, bool simplify, NotifyIcon notifyIcon = null)
        {
            int i = 0;
            string simplifiedMessage = "";

            foreach (DataGridViewRow row in dataGridView.Rows)
            {
                if (row.IsNewRow) continue;

                if (int.TryParse(row.Cells["DeadLine"].Value?.ToString(), out int daysLeft))
                {
                    if (daysLeft <= daysThreshold) // Если дней осталось меньше или равно порогу
                    {
                        string fullName = row.Cells["EmployeeName"].Value?.ToString() ?? "Не указано";
                        string jobTitle = row.Cells["JobTitle"].Value?.ToString() ?? "Не указано";
                        string urgencyLevel = row.Cells["UrgencyLevel"].Value?.ToString() ?? "Не указано";
                        string nextEventDate = row.Cells["NextEventDate"].Value?.ToString() ?? "Не указано";
                        string periodicityText = row.Cells["PeriodicityText"].Value?.ToString() ?? "Не указано";
                        string eventName = row.Cells["EventName"].Value?.ToString() ?? "Не указано";

                        if (!simplify)
                        {
                           

                            string message = $"Событие: {eventName}\n" +
                                             $"Сотрудник: {fullName}\n" +
                                             $"Должность: {jobTitle}\n" +
                                             $"Дата следующего события: {nextEventDate}\n" +
                                             $"Периодичность: {periodicityText}\n" +
                                             $"Осталось дней: {daysLeft}";

                            MessageBox.Show(message, "Уведомление о событии", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        
                        i++;
                        simplifiedMessage += $"({jobTitle}) {fullName} - {eventName} (осталось {urgencyLevel})\n";
                    }
                }
            }

            if (notifyIcon != null && i > 0)
            {
                notifyIcon.BalloonTipTitle = $"ETDB - Требуют внимания событий: {i}.";
                notifyIcon.BalloonTipText =  $"{simplifiedMessage}";
                notifyIcon.BalloonTipIcon = ToolTipIcon.Info;
                notifyIcon.ShowBalloonTip(10000);
            }

            if (simplify && i > 0)
            {
                MessageBox.Show($"Требуют внимания событий: {i}.\n\n{simplifiedMessage}", "Уведомление о событиях", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
        }
    }
}
