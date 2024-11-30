using Spire.Doc;
using Spire.Doc.Documents;
using System;
using System.Linq;
using System.Windows.Forms;

namespace ETDBs
{
    public class DocumentsManager
    {
        public static void ViewDocButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Word Documents|*.doc;*.docx"
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                // Загружаем документ
                Document document = new Document();
                document.LoadFromFile(openFileDialog.FileName);

                new WebBrowserViewerForm(openFileDialog.FileName).ShowDialog();
            }
        }

        // 1. Открытие документа и получение его внутреннего представления
        public static Document OpenDocument(string filePath)
        {
            Document document = new Document();
            document.LoadFromFile(filePath); // Загружаем документ из файла
            return document; // Возвращаем объект Document
        }

        // 2. Замена тега на значение
        public static Document ReplaceTagWithValue(Document document, string tag, string value)
        {
            // Проходим по всем параграфам в документе
            foreach (Section section in document.Sections)
            {
                foreach (Paragraph paragraph in section.Paragraphs)
                {
                    // Заменяем тег на соответствующее значение
                    paragraph.Text = paragraph.Text.Replace($"%{tag}%", value);
                }
            }
            return document; // Возвращаем обновленный документ
        }

        // 3. Сохранение измененного документа
        public static void SaveDocument(Document document, string savePath)
        {
            document.SaveToFile(savePath, FileFormat.Doc); // Сохраняем документ в формате DOC
        }

        public static void ViewDocument(string documentPath)
        {
            new WebBrowserViewerForm(documentPath).ShowDialog();
        }
    }
}
