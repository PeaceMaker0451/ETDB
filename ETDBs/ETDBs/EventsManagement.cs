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
    public partial class EventsManagement : Form
    {
        private DBManager dbManager;

        private int selectedIndex;
        private int selectedID;

        private bool updating;
        public EventsManagement(DBManager manager, Config config)
        {
            dbManager = manager;
            InitializeComponent();
            employeesList.SelectedIndexChanged += EmployeesList_SelectedIndexChanged;
            titleEventsTable.DataBindingComplete += TitleEventsTable_DataBindingComplete;
            employeeEventsTable.DataBindingComplete += EmployeeEventsTable_DataBindingComplete;
            RefreshTable();
        }

        private void EmployeesList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (updating)
                return;

            var listBox = sender as ListBox;

            // Проверяем, что выбранный элемент не равен null
            if (listBox.SelectedValue != null)
            {
                if (!(listBox.SelectedValue is int))
                    return;

                int selectedEmployeeID = (int)listBox.SelectedValue;
                // Здесь можно использовать ID сотрудника
                selectedID = selectedEmployeeID;
                selectedIndex = listBox.SelectedIndex;
                RefreshTable();
            }
        }

        public void RefreshTable()
        {
            updating = true;
            refreshBar.Maximum = 100;

            refreshBar.Value = 20;
            UpdateEmployeesList();
            TrySetPreviousIndex();
            UpdateTitleEventsTable();
            UpdateEmployeeEventsTable();
            updating = false;
            refreshBar.Value = 100;
            Task.Delay(1000);
            refreshBar.Value = 0;
        }

        private void UpdateEmployeesList()
        {
            employeesList.DataSource = null;
            employeesList.Items.Clear();
            var employees = Employee.GetEmployees(dbManager);

            // Привязываем список сотрудников к ListBox
            employeesList.DataSource = employees;
            employeesList.DisplayMember = nameof(Employee.DisplayText); // Отображаем формат, определённый в ToString
            employeesList.ValueMember = nameof(Employee.EmployeeID); // Сохраняем ID сотрудника как значение
        }

        private void TrySetPreviousIndex()
        {
            try
            {
                employeesList.SelectedIndex = selectedIndex;
            }
            catch(Exception e)
            {
                MessageBox.Show($"Невозможно сохранить выделение сотрудника: {e.Message}");
                employeesList.SelectedIndex = 0;
            }
        }

        private void UpdateTitleEventsTable()
        {
            titleEventsTable.CellClick -= TitleEventsTable_CellClick;
            titleEventsTable.Columns.Clear();

            var periodColumn = new DataGridViewTextBoxColumn
            {
                Name = "PeriodicityText",
                HeaderText = "Периодичность",
                ReadOnly = true,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                DisplayIndex = 3
            };
            titleEventsTable.Columns.Add(periodColumn);

            var nextEventColumn = new DataGridViewTextBoxColumn
            {
                Name = "NextEventDate",
                HeaderText = "Дата следующего события",
                ReadOnly = true,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                DisplayIndex = 5
            };
            titleEventsTable.Columns.Add(nextEventColumn);

            titleEventsTable.DataSource = dbManager.GetAllEmployeeJobTitleEventsTable(selectedID);
            titleEventsTable.ReadOnly = true;
            titleEventsTable.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            titleEventsTable.Columns["EventID"].Visible = false;
            titleEventsTable.Columns["IsMonths"].Visible = false;
            titleEventsTable.Columns["EventName"].DisplayIndex = 0;
            titleEventsTable.Columns["EventName"].HeaderText = "Событие";
            titleEventsTable.Columns["StartDate"].HeaderText = "Дата начала отсчета";
            titleEventsTable.Columns["StartDate"].DisplayIndex = 2;
            titleEventsTable.Columns["ToNext"].Visible = false;
            titleEventsTable.AllowUserToAddRows = false;

            var editButtonColumn = new DataGridViewButtonColumn
            {
                HeaderText = "Редактирование",
                Text = "Редактировать",
                UseColumnTextForButtonValue = true,
                Name = "EditButton"
            };
            titleEventsTable.Columns.Add(editButtonColumn);
            titleEventsTable.Columns["EditButton"].DisplayIndex = 1;

            titleEventsTable.CellClick += TitleEventsTable_CellClick;
        }

        private void TitleEventsTable_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewRow row in titleEventsTable.Rows)
            {
                if (row.Cells["IsMonths"].Value is bool isMonths &&
                    row.Cells["ToNext"].Value is int toNextValue &&
                    row.Cells["StartDate"].Value is DateTime startDate &&
                    titleEventsTable.Columns.Contains("PeriodicityText") &&
                    titleEventsTable.Columns.Contains("NextEventDate"))
                {
                    // Генерируем текст для столбца PeriodicityText
                    string periodText = isMonths ? $"Месяцы - {toNextValue}" : $"Дни - {toNextValue}";
                    row.Cells["PeriodicityText"].Value = periodText;  // Присваиваем текст в новый столбец

                    // Вычисляем дату следующего события
                    DateTime nextEventDate = TablesTools.CalculateNextEventDate(startDate, isMonths, toNextValue);
                    row.Cells["NextEventDate"].Value = nextEventDate.ToString("dd.MM.yyyy");  // Присваиваем дату в новый столбец
                }
            }
        }

        private void TitleEventsTable_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void UpdateEmployeeEventsTable()
        {
            employeeEventsTable.CellClick -= EmployeeEventsTable_CellClick;
            employeeEventsTable.Columns.Clear();

            // Добавляем новый столбец для отображения текстового значения периодичности
            var periodColumn = new DataGridViewTextBoxColumn
            {
                Name = "PeriodicityText",
                HeaderText = "Периодичность",
                ReadOnly = true,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                DisplayIndex = 3
            };
            employeeEventsTable.Columns.Add(periodColumn);

            var nextEventColumn = new DataGridViewTextBoxColumn
            {
                Name = "NextEventDate",
                HeaderText = "Дата следующего события",
                ReadOnly = true,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                DisplayIndex = 5
            };
            employeeEventsTable.Columns.Add(nextEventColumn);

            // Присваиваем таблицу DataGridView
            employeeEventsTable.DataSource = dbManager.GetAllEmployeeEventsTable(selectedID);
            employeeEventsTable.ReadOnly = true;
            employeeEventsTable.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            employeeEventsTable.Columns["EventID"].Visible = false;
            employeeEventsTable.Columns["IsMonths"].Visible = false;
            employeeEventsTable.Columns["ToNext"].Visible = false;
            employeeEventsTable.Columns["EventName"].DisplayIndex = 0;
            employeeEventsTable.Columns["EventName"].HeaderText = "Событие";
            employeeEventsTable.Columns["StartDate"].HeaderText = "Дата начала отсчета";
            employeeEventsTable.Columns["StartDate"].DisplayIndex = 2;
            employeeEventsTable.AllowUserToAddRows = false;

            // Добавление кнопки "Редактирование"
            var editButtonColumn = new DataGridViewButtonColumn
            {
                HeaderText = "Редактирование",
                Text = "Редактировать",
                UseColumnTextForButtonValue = true, // Устанавливаем текст для всех кнопок
                Name = "EditButton"                  // Уникальное имя для обработки нажатия
            };
            employeeEventsTable.Columns.Add(editButtonColumn);
            employeeEventsTable.Columns["EditButton"].DisplayIndex = 1;

            // Подписываемся на событие для обработки нажатия на кнопку
            employeeEventsTable.CellClick += EmployeeEventsTable_CellClick;
        }

        private void EmployeeEventsTable_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewRow row in employeeEventsTable.Rows)
            {
                if (row.Cells["IsMonths"].Value is bool isMonths &&
                    row.Cells["ToNext"].Value is int toNextValue &&
                    row.Cells["StartDate"].Value is DateTime startDate &&
                    employeeEventsTable.Columns.Contains("PeriodicityText") &&
                    employeeEventsTable.Columns.Contains("NextEventDate"))
                {
                    // Генерируем текст для столбца PeriodicityText
                    string periodText = isMonths ? $"Месяцы - {toNextValue}" : $"Дни - {toNextValue}";
                    row.Cells["PeriodicityText"].Value = periodText;  // Присваиваем текст в новый столбец

                    // Вычисляем дату следующего события
                    DateTime nextEventDate = TablesTools.CalculateNextEventDate(startDate, isMonths, toNextValue);
                    row.Cells["NextEventDate"].Value = nextEventDate.ToString("dd.MM.yyyy");  // Присваиваем дату в новый столбец
                }
            }
        }

        private void EmployeeEventsTable_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
