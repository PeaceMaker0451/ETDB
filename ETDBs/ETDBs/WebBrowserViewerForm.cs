using System;
using System.IO;
using System.Windows.Forms;
using Spire.Doc;

namespace ETDBs
{
    public partial class WebBrowserViewerForm : Form
    {
        private string documentPath;
        private WebBrowser webBrowser1;

        public WebBrowserViewerForm(string pathToDocument)
        {
            InitializeComponent();

            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.SuspendLayout();
            // 
            // webBrowser1
            // 
            this.webBrowser1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowser1.Location = new System.Drawing.Point(0, 0);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.Size = new System.Drawing.Size(800, 450);
            this.webBrowser1.TabIndex = 0;
            // 
            // DocumentViewerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.webBrowser1);
            this.Name = "DocumentViewerForm";
            this.Text = "Document Viewer";
            this.ResumeLayout(false);

            // Сохраняем путь к документу
            documentPath = pathToDocument;

            // Проверяем и отображаем документ
            DisplayDocument();
        }

        private void DisplayDocument()
        {
            try
            {
                // Проверка наличия файла
                if (!File.Exists(documentPath))
                {
                    MessageBox.Show("Файл не найден: " + documentPath, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Конвертация в PDF
                string tempPdfPath = Path.Combine(Path.GetTempPath(), "temp.pdf");

                Document document = new Document();
                document.LoadFromFile(documentPath);
                document.SaveToFile(tempPdfPath, FileFormat.PDF);

                // Отображение в WebBrowser
                webBrowser1.Navigate(tempPdfPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при отображении документа: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
