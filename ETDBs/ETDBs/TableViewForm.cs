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
    public partial class TableViewForm : Form
    {
        public DataGridView dataGridView;
        private Button backButton;

        // Конструктор, принимающий DataTable для отображения
        public TableViewForm(DataTable table)
        {
            InitializeComponent();

            // Настройка DataGridView
            dataGridView = new DataGridView
            {
                Dock = DockStyle.Fill,
                DataSource = table,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
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
            Controls.Add(dataGridView);
            Controls.Add(backButton);

            // Настройки формы
            Text = "Просмотр таблицы";
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
