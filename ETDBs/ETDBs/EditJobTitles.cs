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
    public partial class EditJobTitles : Form
    {
        private DBManager dbManager;
        private string searchText;
        public EditJobTitles(DBManager manager)
        {
            dbManager = manager;
            this.StartPosition = FormStartPosition.CenterScreen;
            InitializeComponent();
            UpdateJobTitlesYable();
            addNewJobTitleButton.Click += AddNewJobTitleButton_Click;
        }

        private void AddNewJobTitleButton_Click(object sender, EventArgs e)
        {
            string name = InputDialog.ShowDialog("Новая должность");
            if (name != null)
            {
                dbManager.AddJobTitle(name);
                MessageBox.Show($"Должность '{name}' добавлена!");
                UpdateJobTitlesYable();
            }
        }

        private void UpdateJobTitlesYable()
        {
            jobTitlesTable.CellClick -= EditJobTitleCellButton;
            jobTitlesTable.Columns.Clear();

            // Присваиваем таблицу DataGridView
            jobTitlesTable.DataSource = TablesTools.SearchInDataTable(dbManager.GetAllJobTitles(), searchText);
            jobTitlesTable.ReadOnly = true;
            jobTitlesTable.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            jobTitlesTable.Columns["JobTitleID"].Visible = false;
            jobTitlesTable.Columns["Title"].HeaderText = "Должность";
            jobTitlesTable.Columns["Title"].DisplayIndex = 0;


            jobTitlesTable.AllowUserToAddRows = false;

            var editButtonColumn = new DataGridViewButtonColumn
            {
                HeaderText = "Редактирование",
                Text = "Редактировать",
                UseColumnTextForButtonValue = true,  // Устанавливаем текст для всех кнопок
                Name = "EditButton"                  // Уникальное имя для обработки нажатия
            };
            jobTitlesTable.Columns.Add(editButtonColumn);
            jobTitlesTable.Columns["EditButton"].DisplayIndex = 1;

            // Подписываемся на событие для обработки нажатия на кнопку
            jobTitlesTable.CellClick += EditJobTitleCellButton;
        }

        private void EditJobTitleCellButton(object sender, DataGridViewCellEventArgs e)
        {
            string name = InputDialog.ShowDialog("Переименование");
            if (name != null)
            {
                int jobTitleId = Convert.ToInt32(jobTitlesTable.Rows[e.RowIndex].Cells["JobTitleID"].Value);

                dbManager.UpdateJobTitle(jobTitleId, name);
                MessageBox.Show($"Должность '{name}' переименована!");
                UpdateJobTitlesYable();
            }
        }
    }
}
