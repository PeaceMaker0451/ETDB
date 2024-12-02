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

            // Проходим по всем колонкам в DataGridView
            foreach (DataGridViewColumn column in dataGridView.Columns)
            {
                // Пропускаем колонки с кнопками или чекбоксами
                if (column is DataGridViewButtonColumn || column is DataGridViewCheckBoxColumn)
                {
                    continue;
                }

                // Создаем список значений для текущей колонки
                var values = new List<string>();

                // Проходим по строкам DataGridView
                foreach (DataGridViewRow row in dataGridView.Rows)
                {
                    // Проверяем, что строка не является новой (пустой)
                    if (row.IsNewRow) continue;

                    // Если в колонке "SelectCheckBox" установлено значение true, добавляем значение из текущей ячейки
                    if (filterColumn != null)
                    {
                        var selectCheckBoxCell = row.Cells[filterColumn];
                        if (selectCheckBoxCell != null && selectCheckBoxCell.Value is bool isChecked && isChecked)
                        {
                            var cellValue = row.Cells[column.Index]?.Value?.ToString();
                            values.Add(cellValue);
                        }
                    }
                    else
                    {
                        var cellValue = row.Cells[column.Index]?.Value?.ToString();
                        values.Add(cellValue);
                    }
                    
                }

                // Если в колонке есть значения, добавляем их в словарь
                if (values.Count > 0)
                {
                    result[column.HeaderText] = values;
                }
            }

            return result;
        }
    }
}
