using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ETDBs
{
    public class TagExtractor
    {
        public static Dictionary<string, List<string>> ExtractTagsFromDataGridView(DataGridView dataGridView, string filterColumn = null)
        {
            // Словарь для хранения тегов
            var result = new Dictionary<string, List<string>>();

            // Списки для индексов, порядков и дат
            var indices = new List<string>();
            var orders = new List<string>();
            var todayShortDate = new List<string>();
            var todayFormattedDate = new List<string>();

            // Получение текущей даты
            DateTime today = DateTime.Today;
            string shortDate = today.ToString("dd.MM.yyyy"); // Формат 01.01.2020
            string formattedDate = $"{today:dd} {today:MMMM yyyy} г."; // Формат "05" ноября 2024 г.

            // Текущий индекс для порядков
            int filteredIndex = 0;

            // Проходим по строкам DataGridView
            foreach (DataGridViewRow row in dataGridView.Rows)
            {
                // Проверяем, что строка не является новой (пустой)
                if (row.IsNewRow) continue;

                // Проверяем фильтр, если указан
                if (filterColumn != null)
                {
                    var selectCheckBoxCell = row.Cells[filterColumn];
                    if (selectCheckBoxCell == null || !(selectCheckBoxCell.Value is bool isChecked) || !isChecked)
                    {
                        continue;
                    }
                }

                // Добавляем индекс строки (в исходной таблице)
                indices.Add(row.Index.ToString());

                // Добавляем порядок строки (в отфильтрованном списке)
                orders.Add((filteredIndex + 1).ToString());
                filteredIndex++;

                // Добавляем даты для текущей строки
                todayShortDate.Add(shortDate);
                todayFormattedDate.Add(formattedDate);

                // Обрабатываем колонки
                foreach (DataGridViewColumn column in dataGridView.Columns)
                {
                    // Пропускаем колонки с кнопками или чекбоксами
                    if (column is DataGridViewButtonColumn || column is DataGridViewCheckBoxColumn)
                    {
                        continue;
                    }

                    var fieldValue = row.Cells[column.Index].Value;
                    // Получаем значение ячейки
                    var cellValue = fieldValue is DateTime dateValue
                        ? dateValue.ToString("dd.MM.yyyy")
                        : fieldValue?.ToString() ?? string.Empty;

                    // Добавляем значение в соответствующий список
                    if (!result.ContainsKey(column.HeaderText))
                    {
                        result[column.HeaderText] = new List<string>();
                    }

                    result[column.HeaderText].Add(cellValue);
                }
            }

            // Добавляем списки "Индекс", "Порядок" и даты в словарь
            if (indices.Count > 0)
            {
                result["Индекс"] = indices;
            }
            if (orders.Count > 0)
            {
                result["Порядок"] = orders;
            }
            if (todayShortDate.Count > 0)
            {
                result["Сегодня (Коротко)"] = todayShortDate;
            }
            if (todayFormattedDate.Count > 0)
            {
                result["Сегодня"] = todayFormattedDate;
            }

            return result;
        }
    }
}
