using ClosedXML.Excel;
using Spire.Doc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ETDBs
{
    
    public partial class TemplateDocumentExport : Form
    {

        private int selectedIndex = -1;
        public string selectedPath = "";

        private Dictionary<string, string> tags;

        public TemplateDocumentExport(Dictionary<string,string> _tags)
        {
            tags = _tags;
            InitializeComponent();
            LoadDocuments();
            ShowTags();

            documentsListBox.SelectedIndexChanged += documentsListBox_SelectedIndexChanged;

            CancelButton = cancelButton;
            AcceptButton = acceptButton;

            cancelButton.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };
            acceptButton.Click += (s, e) => { ExportDocument(); DialogResult = DialogResult.OK; Close(); };
            viewButton.Click += (s, e) => OpenFile(selectedPath);
        }

        private void LoadDocuments()
        {
            string folderPath = "";

            folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ETDB", "Documents");

            // Проверяем, существует ли такая директория
            if (Directory.Exists(folderPath))
            {
                // Очищаем ListBox перед загрузкой новых данных
                documentsListBox.Items.Clear();

                // Получаем все файлы с расширениями .docx, .xls, .xlsx
                var files = Directory.GetFiles(folderPath)
                    .Where(file => file.EndsWith(".docx", StringComparison.OrdinalIgnoreCase) || file.EndsWith(".xls", StringComparison.OrdinalIgnoreCase) || file.EndsWith(".xlsx", StringComparison.OrdinalIgnoreCase))
                    .ToList();

                // Добавляем файлы в ListBox, отображая их имена
                foreach (var file in files)
                {
                    string fileName = Path.GetFileName(file);  // Получаем имя файла
                    documentsListBox.Items.Add(fileName);
                }
            }
            else
            {
                MessageBox.Show("Directory not found!");
            }
        }

        private void ShowTags()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"Данная таблица позволит вам использовать экспортировать следующие теги:");

            foreach (var tag in tags)
            {
                sb.AppendLine($"%{tag.Key}% - {tag.Value}");
            }

            tagsTextBox.Text = sb.ToString();
        }

        private void documentsListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (documentsListBox.SelectedIndex == selectedIndex) return;
            selectedIndex = documentsListBox.SelectedIndex;
            
            if (documentsListBox.SelectedIndex >= 0)
            {
                string selectedFileName = documentsListBox.SelectedItem.ToString();

                string folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ETDB", "Documents");
                string selectedFilePath = Path.Combine(folderPath, selectedFileName);

                selectedPath = selectedFilePath;
            }
        }

        private void OpenFile(string filePath)
        {
            try
            {
                DisplayDocument(filePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка открытия файла: {ex.Message}");
            }
        }

        private void ExportDocument()
        {
            var doc = DocumentsManager.OpenDocument(selectedPath);

            foreach (var tag in tags)
            {
                doc = DocumentsManager.ReplaceTagWithValue(doc, tag.Key, tag.Value);
            }

            string tempPdfPath = Path.Combine(Path.GetTempPath(), "temp.docx");

            doc.SaveToFile(tempPdfPath);
            DisplayDocument(tempPdfPath);
        }

        private void DisplayDocument(string documentPath)
        {
            try
            {
                // Проверка, существует ли файл
                if (!File.Exists(documentPath))
                {
                    MessageBox.Show("Файл не найден: " + documentPath, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Открытие документа в стандартной программе с помощью Process.Start
                System.Diagnostics.Process.Start(documentPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при открытии документа: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
