using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

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

        public static void ConvertExcelToPdf(string excelFilePath, string pdfFilePath)
        {
            try
            {
                // Шаг 1: Конвертируем Excel в HTML
                string htmlFilePath = Path.Combine(Path.GetTempPath(), "temp.html");

                // Чтение Excel файла с использованием ClosedXML
                using (var workbook = new XLWorkbook(excelFilePath))
                {
                    StringBuilder htmlContent = new StringBuilder();
                    htmlContent.AppendLine("<html><body><table border='1'>");

                    // Проходим по всем листам Excel и преобразуем их в HTML
                    foreach (var worksheet in workbook.Worksheets)
                    {
                        htmlContent.AppendLine("<tr><th colspan='" + worksheet.ColumnCount() + "'>" + worksheet.Name + "</th></tr>");

                        foreach (var row in worksheet.Rows())
                        {
                            htmlContent.AppendLine("<tr>");
                            foreach (var cell in row.Cells())
                            {
                                htmlContent.AppendLine("<td>" + cell.Value.ToString() + "</td>");
                            }
                            htmlContent.AppendLine("</tr>");
                        }
                    }

                    htmlContent.AppendLine("</table></body></html>");

                    // Сохраняем HTML содержимое в файл
                    File.WriteAllText(htmlFilePath, htmlContent.ToString());
                }

                // Шаг 2: Конвертируем HTML в PDF с использованием WkHtmlToPdf
                ConvertHtmlToPdf(htmlFilePath, pdfFilePath);

                // Удаляем временный HTML файл после конвертации
                if (File.Exists(htmlFilePath))
                {
                    File.Delete(htmlFilePath);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при конвертации Excel в PDF: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Метод для конвертации HTML в PDF с использованием WkHtmlToPdf
        private static void ConvertHtmlToPdf(string htmlFilePath, string pdfFilePath)
        {
            try
            {
                // Путь к исполнимому файлу WkHtmlToPdf (убедитесь, что он установлен на вашем устройстве)
                string wkHtmlToPdfPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "wkhtmltopdf.exe");

                // Запуск процесса конвертации HTML в PDF
                System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo()
                {
                    FileName = wkHtmlToPdfPath,
                    Arguments = $"\"{htmlFilePath}\" \"{pdfFilePath}\"",
                    CreateNoWindow = true,
                    UseShellExecute = false
                };

                using (var process = System.Diagnostics.Process.Start(startInfo))
                {
                    process.WaitForExit(); // Ожидаем завершения процесса
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при конвертации HTML в PDF: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
