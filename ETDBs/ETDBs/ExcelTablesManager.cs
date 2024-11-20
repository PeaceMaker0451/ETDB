using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ETDBs
{
    public class ExcelTablesManager
    {
        public static void ExportToExcel(DataGridView dataGridView, string sheetName, string filePath, bool includeHiddenColumns = false)
        {
            try
            {
                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add(sheetName);

                    // Получение списка столбцов с учетом DisplayIndex, видимости и типа
                    var columns = dataGridView.Columns
                        .Cast<DataGridViewColumn>()
                        .Where(col => (includeHiddenColumns || col.Visible) && !(col is DataGridViewButtonColumn)) // Исключаем скрытые и кнопки
                        .OrderBy(col => col.DisplayIndex) // Учет DisplayIndex
                        .ToList();

                    // Заголовки
                    for (int col = 0; col < columns.Count; col++)
                    {
                        worksheet.Cell(1, col + 1).Value = columns[col].HeaderText;
                    }

                    // Данные
                    for (int row = 0; row < dataGridView.Rows.Count; row++)
                    {
                        for (int col = 0; col < columns.Count; col++)
                        {
                            var column = columns[col];
                            worksheet.Cell(row + 2, col + 1).Value = dataGridView.Rows[row].Cells[column.Index].Value?.ToString() ?? string.Empty;
                        }
                    }

                    // Сохраняем файл
                    workbook.SaveAs(filePath);
                    MessageBox.Show("Экспорт успешно завершен!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка экспорта: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
