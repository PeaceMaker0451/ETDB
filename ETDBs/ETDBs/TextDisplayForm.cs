using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ETDBs
{
    public partial class TextDisplayForm : Form
    {
        private TextBox textBox;
        private Button backButton;

        // Конструктор, принимающий текст и заголовок
        public TextDisplayForm(string title, string content)
        {
            InitializeComponent();

            // Устанавливаем заголовок формы
            this.Text = title;

            // Настройка TextBox для отображения текста
            textBox = new TextBox
            {
                Text = content, // Вставляем текст, переданный в конструктор
                Dock = DockStyle.Fill, // Заполняет все пространство формы
                Multiline = true, // Разрешаем многострочное отображение текста
                ReadOnly = true, // Текст только для чтения
                ScrollBars = ScrollBars.Both, // Добавляем полосы прокрутки, если текст выходит за пределы
                WordWrap = true // Автоматический перенос слов
            };

            // Настройка кнопки "Назад"
            backButton = new Button
            {
                Text = "Назад",
                Dock = DockStyle.Bottom,
                Height = 40,
            };
            backButton.Click += BackButton_Click;

            // Добавление элементов управления на форму
            Controls.Add(textBox);
            Controls.Add(backButton);

            // Настройки формы
            Size = new System.Drawing.Size(800, 600); // Установите желаемый размер формы
            StartPosition = FormStartPosition.CenterScreen;

            Program.SetFormSize(this);
        }

        // Обработчик нажатия кнопки "Назад"
        private void BackButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
