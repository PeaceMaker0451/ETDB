using NPOI.POIFS.Crypt.Dsig;
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
    public enum ExportType
    {
        FullDocument,
        Table
    }
    
    
    public partial class TemplateDocumentExport : Form
    {

        private int selectedIndex = -1;
        private string selectedPath = "";

        private ExportType exportType;

        private Dictionary<string, string> tags;
        private Dictionary<string, List<string>> multiTags;

        public TemplateDocumentExport(Dictionary<string,string> _tags)
        {
            exportType = ExportType.FullDocument;
            tags = _tags;
            InitializeComponent();
            LoadDocuments();
            ShowTags();

            documentsListBox.SelectedIndexChanged += documentsListBox_SelectedIndexChanged;

            CancelButton = cancelButton;
            AcceptButton = acceptButton;

            cancelButton.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };
            acceptButton.Click += (s, e) => { ExportDocument();};
            viewButton.Click += (s, e) => OpenFile(selectedPath);
        }

        public TemplateDocumentExport(Dictionary<string, List<string>> _tags)
        {
            exportType = ExportType.Table;
            multiTags = _tags;
            InitializeComponent();
            LoadDocuments();
            ShowMultiTags();

            documentsListBox.SelectedIndexChanged += documentsListBox_SelectedIndexChanged;

            CancelButton = cancelButton;
            AcceptButton = acceptButton;

            cancelButton.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };
            acceptButton.Click += (s, e) => { ExportDocument();};
            viewButton.Click += (s, e) => OpenFile(selectedPath);
        }

        private void LoadDocuments()
        {
            string folderPath = "";

            if(exportType == ExportType.FullDocument)
                folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ETDB", "Documents");
            else if (exportType == ExportType.Table)
                folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ETDB", "Tables");

            // Проверяем, существует ли такая директория
            if (Directory.Exists(folderPath))
            {
                // Очищаем ListBox перед загрузкой новых данных
                documentsListBox.Items.Clear();

                // Получаем все файлы с расширениями .docx, .xls, .xlsx

                List<string> files;

                if (exportType == ExportType.FullDocument)
                {
                    files = Directory.GetFiles(folderPath)
                    .Where(file => file.EndsWith(".docx", StringComparison.OrdinalIgnoreCase) || file.EndsWith(".xls", StringComparison.OrdinalIgnoreCase) || file.EndsWith(".xlsx", StringComparison.OrdinalIgnoreCase))
                    .ToList();
                }
                else
                {
                    files = Directory.GetFiles(folderPath)
                    .Where(file => file.EndsWith(".xls", StringComparison.OrdinalIgnoreCase) || file.EndsWith(".xlsx", StringComparison.OrdinalIgnoreCase))
                    .ToList();
                }
                

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

        private void ShowMultiTags()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"Данная таблица позволит вам использовать экспортировать следующие теги:");

            foreach (var tag in multiTags)
            {
                sb.AppendLine($"%{tag.Key}% - {tag.Value.Count}");
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

                string folderPath = "";
                
                if (exportType == ExportType.FullDocument)
                    folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ETDB", "Documents");
                else if (exportType == ExportType.Table)
                    folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ETDB", "Tables");

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
            if (exportType == ExportType.FullDocument)
            {
                var dh = new DocumentHandler();

                dh.LoadDocument(selectedPath);

                foreach (var tag in tags)
                {
                    dh.ReplaceTag(tag.Key, tag.Value);
                }

                string tempPath = "";

                if (dh.GetDocumentType() == DocumentHandler.DocumentType.Word)
                    tempPath = Path.Combine(Path.GetTempPath(), "temp.docx");
                else if (dh.GetDocumentType() == DocumentHandler.DocumentType.Excel)
                    tempPath = Path.Combine(Path.GetTempPath(), "temp.xls");

                dh.SaveDocument(tempPath);
                DisplayDocument(tempPath);
            }
            else
            {
                var generator = new DocumentGenerator(selectedPath, Path.Combine(Path.GetTempPath(), "temp.xls"));
                generator.GenerateDocumentFromTemplate(multiTags);
                DisplayDocument(Path.Combine(Path.GetTempPath(), "temp.xls"));
            }
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

                System.Diagnostics.Process.Start(documentPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при открытии документа: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
