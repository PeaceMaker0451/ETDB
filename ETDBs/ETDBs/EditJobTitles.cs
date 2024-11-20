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
            UpdateJobTitlesTable();
            addNewJobTitleButton.Click += AddNewJobTitleButton_Click;

            Program.SetFormSize(this);
        }

        private void AddNewJobTitleButton_Click(object sender, EventArgs e)
        {
            string name = InputDialog.ShowDialog("Новая должность");
            if (name != null)
            {
                dbManager.AddJobTitle(name);
                MessageBox.Show($"Должность '{name}' добавлена!");
                UpdateJobTitlesTable();
            }
        }

        private void UpdateJobTitlesTable()
        {
            jobTitlesTable.CellClick -= JobTitleCellButtonClick;
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
                HeaderText = "Редактирование должности",
                Text = "Редактировать",
                UseColumnTextForButtonValue = true,
                Name = "EditButton"
            };
            jobTitlesTable.Columns.Add(editButtonColumn);
            jobTitlesTable.Columns["EditButton"].DisplayIndex = 1;

            var deleteButtonColumn = new DataGridViewButtonColumn
            {
                HeaderText = "Удаление должности",
                Text = "Удалить",
                UseColumnTextForButtonValue = true,
                Name = "DeleteButton"
            };
            jobTitlesTable.Columns.Add(deleteButtonColumn);
            jobTitlesTable.Columns["DeleteButton"].DisplayIndex = jobTitlesTable.Columns.Count - 1;

            jobTitlesTable.CellClick += JobTitleCellButtonClick;
        }

        private void JobTitleCellButtonClick(object sender, DataGridViewCellEventArgs e)
        {

            int jobTitleId = Convert.ToInt32(jobTitlesTable.Rows[e.RowIndex].Cells["JobTitleID"].Value);

            if (e.ColumnIndex == jobTitlesTable.Columns["EditButton"].Index && e.RowIndex >= 0)
            {
                string name = InputDialog.ShowDialog("Переименование должности");
                if (name != null)
                {
                    dbManager.UpdateJobTitle(jobTitleId, name);
                    MessageBox.Show($"Должность '{name}' переименована!");
                    UpdateJobTitlesTable();
                }
            }
            else if (e.ColumnIndex == jobTitlesTable.Columns["DeleteButton"].Index && e.RowIndex >= 0)
            {
                string name = jobTitlesTable.Rows[e.RowIndex].Cells["Title"].Value.ToString();

                DialogResult result = MessageBox.Show(
                   $"Вы собираетесь удалить должность '{name}', всех сотрудников этой должности (!!!) и все, что с ними связано.\nЭто действие невозможно обратить\n\nВы уверены?",
                   "Удаление сотрудника",
                   MessageBoxButtons.YesNo,
                   MessageBoxIcon.Warning
               );

                // Проверка результата
                if (result == DialogResult.Yes)
                    dbManager.DeleteJobTitle(jobTitleId);

                UpdateJobTitlesTable();
            }
        }
    }
}
