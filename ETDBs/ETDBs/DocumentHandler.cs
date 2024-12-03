using System;
using System.IO;
using System.Windows.Forms; // Для использования DataGridView
using NPOI.XSSF.UserModel; // Для XLSX (новый Excel)
using NPOI.HSSF.UserModel; // Для XLS (старый Excel)
using NPOI.SS.UserModel; // Общий интерфейс для работы с таблицами
using NPOI.XWPF.UserModel; // Для работы с DOCX
using System.Linq;
using System.Diagnostics;

public class DocumentHandler
{
    /// <summary>
    /// Тип документа.
    /// </summary>
    public enum DocumentType
    {
        None,
        Word,
        Excel
    }

    private object _document; // Может содержать XWPFDocument, IWorkbook (HSSFWorkbook или XSSFWorkbook)
    private DocumentType _documentType = DocumentType.None; // Текущий тип документа

    /// <summary>
    /// Чтение документа из указанного пути.
    /// </summary>
    public void LoadDocument(string path)
    {
        if (!File.Exists(path))
            throw new FileNotFoundException("Файл не найден.");

        var extension = Path.GetExtension(path).ToLower();

        using (var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
        {
            if (extension == ".docx")
            {
                _document = new XWPFDocument(fileStream);
                _documentType = DocumentType.Word;
            }
            else if (extension == ".xlsx")
            {
                _document = new XSSFWorkbook(fileStream);
                _documentType = DocumentType.Excel;
            }
            else if (extension == ".xls")
            {
                _document = new HSSFWorkbook(fileStream);
                _documentType = DocumentType.Excel;
            }
            else
            {
                throw new NotSupportedException("Формат файла не поддерживается.");
            }
        }
    }

    public void ReplaceTag(string tag, string value)
    {
        if (_document == null)
            throw new InvalidOperationException("Документ не загружен.");

        string fullTag = $"%{tag}%";

        if (_documentType == DocumentType.Word)
        {
            var wordDoc = (XWPFDocument)_document;

            Debug.WriteLine("Обрабатываем все абзацы");
            // Обрабатываем все абзацы
            foreach (var paragraph in wordDoc.Paragraphs)
            {
                ReplaceTagInParagraph(paragraph, fullTag, value);
            }

            Debug.WriteLine("Обрабатываем таблицы");
            // Обрабатываем таблицы
            foreach (var table in wordDoc.Tables)
            {
                foreach (var row in table.Rows)
                {
                    foreach (var cell in row.GetTableCells())
                    {
                        // Обрабатываем текст в ячейке
                        foreach (var paragraph in cell.Paragraphs)
                        {
                            ReplaceTagInParagraph(paragraph, fullTag, value);
                        }

                        // Если в ячейке есть другие таблицы (вложенные таблицы), их тоже нужно обработать
                        foreach (var nestedTable in cell.Tables)
                        {
                            foreach (var nestedRow in nestedTable.Rows)
                            {
                                foreach (var nestedCell in nestedRow.GetTableCells())
                                {
                                    foreach (var paragraph in nestedCell.Paragraphs)
                                    {
                                        ReplaceTagInParagraph(paragraph, fullTag, value);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            Debug.WriteLine("Обрабатываем шапки");

            // Обрабатываем шапки
            foreach (var header in wordDoc.HeaderList)
            {
                foreach (var paragraph in header.Paragraphs)
                {
                    ReplaceTagInParagraph(paragraph, fullTag, value);
                }
            }

            Debug.WriteLine("Обрабатываем нижние колонтитулы");

            // Обрабатываем нижние колонтитулы
            foreach (var footer in wordDoc.FooterList)
            {
                foreach (var paragraph in footer.Paragraphs)
                {
                    ReplaceTagInParagraph(paragraph, fullTag, value);
                }
            }
        }
        else if (_documentType == DocumentType.Excel)
        {
            var excelDoc = (IWorkbook)_document;

            for (int i = 0; i < excelDoc.NumberOfSheets; i++)
            {
                var sheet = excelDoc.GetSheetAt(i);
                foreach (IRow row in sheet)
                {
                    foreach (NPOI.SS.UserModel.ICell cell in row.Cells)
                    {
                        if (cell.CellType == CellType.String && cell.StringCellValue.Contains(fullTag))
                        {
                            cell.SetCellValue(cell.StringCellValue.Replace(fullTag, value));
                            Debug.WriteLine($"Подстрока '{cell.StringCellValue}' изменена: тег {fullTag} заменен на значение {value}");
                        }
                    }
                }
            }
        }
        else
        {
            throw new NotSupportedException("Операция не поддерживается для этого типа документа.");
        }
    }

    /// <summary>
    /// Замена тега в абзаце Word-документа.
    /// </summary>
    private void ReplaceTagInParagraph(XWPFParagraph paragraph, string fullTag, string value)
    {
        // Объединяем текст всех Run
        string paragraphText = string.Join(string.Empty, paragraph.Runs.Select(run => run.Text));

        Debug.WriteLine($"Попытка замены тега в подстроке '{paragraphText}': тег {fullTag} будет заменен на значение {value}");

        if (paragraphText.Contains(fullTag))
        {
            // Выполняем замену
            paragraphText = paragraphText.Replace(fullTag, value);
            Debug.WriteLine($"Подстрока '{paragraphText}' изменена: тег {fullTag} заменен на значение {value}");

            // Удаляем старые Run
            while (paragraph.Runs.Count > 0)
            {
                paragraph.RemoveRun(0);
            }

            // Добавляем новый текст в виде одного Run
            var newRun = paragraph.CreateRun();
            newRun.SetText(paragraphText);
        }
    }

    /// <summary>
    /// Сохранение документа по указанному пути.
    /// </summary>
    public void SaveDocument(string path)
    {
        if (_document == null)
            throw new InvalidOperationException("Документ не загружен.");

        using (var fileStream = new FileStream(path, FileMode.Create, FileAccess.Write))
        {
            if (_documentType == DocumentType.Word)
            {
                ((XWPFDocument)_document).Write(fileStream);
            }
            else if (_documentType == DocumentType.Excel)
            {
                ((IWorkbook)_document).Write(fileStream);
            }
            else
            {
                throw new NotSupportedException("Операция не поддерживается для этого типа документа.");
            }
        }
    }

    /// <summary>
    /// Создание Excel-документа из данных DataGridView.
    /// Игнорирует кнопки, чекбоксы и булевые значения.
    /// </summary>
    public static void CreateExcelFromDataGridView(DataGridView dataGridView, string path)
    {
        if (dataGridView == null)
            throw new ArgumentNullException(nameof(dataGridView));

        // Создаем новый Excel-документ
        IWorkbook workbook = new XSSFWorkbook();
        ISheet sheet = workbook.CreateSheet("Sheet1");

        // Добавляем заголовки из DataGridView
        IRow headerRow = sheet.CreateRow(0);
        for (int i = 0; i < dataGridView.Columns.Count; i++)
        {
            // Добавляем только текстовые столбцы
            if (dataGridView.Columns[i] is DataGridViewTextBoxColumn)
            {
                headerRow.CreateCell(i).SetCellValue(dataGridView.Columns[i].HeaderText);
            }
        }

        // Заполняем данными из DataGridView
        for (int i = 0; i < dataGridView.Rows.Count; i++)
        {
            IRow row = sheet.CreateRow(i + 1);
            for (int j = 0; j < dataGridView.Columns.Count; j++)
            {
                var column = dataGridView.Columns[j];

                // Игнорируем кнопки, чекбоксы и булевые значения
                if (column is DataGridViewButtonColumn ||
                    column is DataGridViewCheckBoxColumn ||
                    column.ValueType == typeof(bool))
                {
                    continue;
                }

                // Получаем значение ячейки
                var cellValue = dataGridView.Rows[i].Cells[j].Value?.ToString() ?? string.Empty;

                // Записываем значение в Excel
                row.CreateCell(j).SetCellValue(cellValue);
            }
        }

        // Сохраняем документ
        try
        {
            using (var fileStream = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                workbook.Write(fileStream);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ошибка сохранения файла: {ex.Message}");
        }
        
        
    }

    /// <summary>
    /// Получить тип загруженного документа.
    /// </summary>
    /// <returns>Тип документа (None, Word, Excel).</returns>
    public DocumentType GetDocumentType()
    {
        return _documentType;
    }
}