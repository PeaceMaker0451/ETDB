using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;

public class DocumentGenerator
{
    private readonly string _templatePath;
    private readonly string _outputPath;

    public DocumentGenerator(string templatePath, string outputPath)
    {
        _templatePath = templatePath;
        _outputPath = outputPath;
    }

    public void GenerateDocumentFromTemplate(Dictionary<string, List<string>> data)
    {
        try
        {
            // Чтение шаблона Excel
            using (var fileStream = new FileStream(_templatePath, FileMode.Open, FileAccess.Read))
            {
                IWorkbook workbook = new XSSFWorkbook(fileStream);
                ISheet sheet = workbook.GetSheetAt(0);

                // Чтение тегов из первой строки (поле)
                IRow headerRow = sheet.GetRow(0);
                var names = new List<string>();
                var fields = new List<string>();

                // Получаем вторую строку (индекс 1)
                IRow secondRow = sheet.GetRow(1);

                // Проверка на пустые строки
                bool hasValues = false;
                if (secondRow != null)
                {
                    // Перебираем все ячейки во второй строке
                    for (int i = 0; i < secondRow.LastCellNum; i++)
                    {
                        ICell cell = secondRow.GetCell(i);

                        if (cell != null && !string.IsNullOrWhiteSpace(cell.ToString()))
                        {
                            // Если хотя бы одна ячейка не пустая, ставим флаг
                            hasValues = true;
                            break;
                        }
                    }
                }

                if (hasValues)
                {
                    for (int i = 0; i < headerRow.LastCellNum; i++)
                    {
                        ICell cell = secondRow.GetCell(i);
                        ICell nameCell = headerRow.GetCell(i);
                        // Проверка на null для ячеек
                        if (cell != null)
                        {
                            fields.Add(cell.ToString());
                        }
                        if (nameCell != null)
                        {
                            names.Add(nameCell.ToString());
                        }
                    }
                }
                else if(headerRow != null)
                {
                    for (int i = 0; i < headerRow.LastCellNum; i++)
                    {
                        ICell cell = headerRow.GetCell(i);

                        // Проверка на null для ячеек
                        if (cell != null)
                        {
                            fields.Add(cell.ToString());
                        }
                    }
                }
                else
                {
                    throw new InvalidOperationException("Первая строка таблицы шаблона была пуста");
                }

                // Выводим информацию о тегах для отладки
                Debug.WriteLine("Поля из шаблона:");
                foreach (var value in fields)
                {
                    Debug.WriteLine(value);
                }

                // Создаем новый документ Excel для вывода
                IWorkbook outputWorkbook = new XSSFWorkbook();
                ISheet outputSheet = outputWorkbook.CreateSheet("Новая таблица");

                // Количество строк (по наибольшему списку значений)
                int rowCount = data.Values.Max(v => v.Count);

                // Выводим количество строк и проверяем данные для отладки
                Debug.WriteLine($"Количество строк, которые будут добавлены: {rowCount}");

                bool hasNamesRow = false;

                if(names.Count > 0)
                {
                    hasNamesRow = true;

                    IRow namestRow = outputSheet.CreateRow(0);

                    for (int i = 0; i < names.Count; i++)
                    {
                        namestRow.CreateCell(i).SetCellValue(names[i]);
                    }
                }

                // Заполнение таблицы
                for (int rowIndex = 0; rowIndex < rowCount; rowIndex++)
                {
                    IRow outputRow;

                    if (hasNamesRow)
                        outputRow = outputSheet.CreateRow(rowIndex + 1);
                    else
                        outputRow = outputSheet.CreateRow(rowIndex);

                    for (int i = 0; i < fields.Count; i++)
                    {
                        outputRow.CreateCell(i).SetCellValue(fields[i]);
                    }

                    foreach(var cell in outputRow.Cells)
                    {
                        

                        foreach(var tagAndValuePair in data)
                        {
                            cell.SetCellValue(cell.ToString().Replace($"%{tagAndValuePair.Key}%", tagAndValuePair.Value[rowIndex]));
                            
                            Debug.WriteLine($"Заполнена ячейка строки {rowIndex} [{cell.ToString()}]");
                        }

                    }
                }

                // Сохранение результата
                using (var outputStream = new FileStream(_outputPath, FileMode.Create, FileAccess.Write))
                {
                    outputWorkbook.Write(outputStream);
                    Debug.WriteLine($"Документ сохранен по пути: {_outputPath}");
                }
            }
        }
        catch (FileNotFoundException ex)
        {
            // Ошибка, если файл шаблона не найден
            MessageBox.Show($"Ошибка: файл шаблона не найден. {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        catch (UnauthorizedAccessException ex)
        {
            // Ошибка, если нет доступа к файлу
            MessageBox.Show($"Ошибка: нет доступа к файлу. {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        catch (IOException ex)
        {
            // Ошибка, если произошла проблема с файлом
            MessageBox.Show($"Ошибка ввода-вывода: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        catch (Exception ex)
        {
            // Обрабатываем другие типы ошибок
            MessageBox.Show($"Произошла непредвиденная ошибка: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}