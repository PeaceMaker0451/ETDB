using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ETDBs
{
    public partial class EmployeesManagement : Form
    {
        private DBManager dbManager;
        private Config config;

        private string searchText;
        private string statusFilterText;
        private string titleFilterText;
        private DataTable employeesData;
        
        public EmployeesManagement(DBManager manager, Config _config)
        {
            config = _config;
            dbManager = manager;
            InitializeComponent();

            editJobTitlesButton.Click += EditJobTitlesButton_Click;
            addStatusButton.Click += AddStatusButton_Click;
            addAttributeButton.Click += AddAttributeButton_Click;
            createNewEmployeeButton.Click += CreateNewEmployeeButton_Click;
            refreshButton.Click += RefreshButton_Click;

            configMenuItem.Click += (s, e) => { this.DialogResult = DialogResult.Retry; this.Close(); };
            справкаToolStripMenuItem.Click += (s, e) => { new TextDisplayForm("Справка", "Справка").ShowDialog(); };
            
            лицензияToolStripMenuItem.Click += (s, e) => 
            {
                try
                {
                    string licenseFilePath = Path.Combine(Application.StartupPath, "LICENSE.txt");

                    // Проверяем существует ли файл
                    if (File.Exists(licenseFilePath))
                    {
                        string licenseContent = File.ReadAllText(licenseFilePath);

                        //MessageBox.Show(licenseContent, "License Content", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        new TextDisplayForm("Лицензия", licenseContent).ShowDialog();
                    }
                    else
                    {
                        MessageBox.Show($"Файл {licenseFilePath} Не был найден.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка чтения файла: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            readMeToolStripMenuItem.Click += (s, e) =>
            {
                try
                {
                    string licenseFilePath = Path.Combine(Application.StartupPath, "ReadMe.txt");

                    // Проверяем существует ли файл
                    if (File.Exists(licenseFilePath))
                    {
                        string licenseContent = File.ReadAllText(licenseFilePath);

                        //MessageBox.Show(licenseContent, "License Content", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        new TextDisplayForm("ReadMe", licenseContent).ShowDialog();
                    }
                    else
                    {
                        MessageBox.Show($"Файл {licenseFilePath} Не был найден.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка чтения файла: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            добавитьДолжностьToolStripMenuItem.Click += EditJobTitlesButton_Click;
            добавитьДопПолеToolStripMenuItem.Click += AddAttributeButton_Click;
            добавитьСтатусToolStripMenuItem.Click += AddStatusButton_Click;
            обновитьToolStripMenuItem.Click += (s,e) => RefreshTable();

            searchTextBox.TextChanged += (s, e) => { searchText = searchTextBox.Text;  SetEmployeesTable(); };

            UpdateEmployeesTable();

            FillFilters();
            Program.SetFormSize(this);
        }

        private void EditJobTitlesButton_Click(object sender, EventArgs e)
        {
            var form = new EditJobTitles(dbManager);

            try
            {
                form.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Невозможно редактировать должности.");
            }
            
            RefreshTable();
        }

        private void AddStatusButton_Click(object sender, EventArgs e)
        {
            string name = InputDialog.ShowDialog("Новый статус");
            if (name != null)
            {

                dbManager.AddStatus(name);
                
                if(config.notificationLevel >= 1)
                    MessageBox.Show($"Статус '{name}' добавлен!");

                RefreshTable();
            }
        }

        private void AddAttributeButton_Click(object sender, EventArgs e)
        {
            string name = InputDialog.ShowDialog("Имя нового параметра");
            if(name != null)
            {
                try
                {
                    dbManager.AddOrUpdateEmployeeAttribute(1, name, "");
                }
                catch (InvalidOperationException ex)
                {
                    MessageBox.Show($"Необходимо добавить хотя бы одного сотрудника для добавления доп поля.");
                }

                if (config.notificationLevel >= 1)
                    MessageBox.Show($"Аттрибут '{name}' добавлен!");
                
                RefreshTable(); 
            }
        }

        private void CreateNewEmployeeButton_Click(object sender, EventArgs e)
        {
            var form = new EditEmployee(dbManager);

            var jobTitles = dbManager.GetAllJobTitles();

            if (jobTitles.Rows.Count == 0)
            {
                MessageBox.Show("Невозможно создать сотрудника. Сначала добавьте хотя бы одну должность");
                return;
            }

            if (form.ShowDialog() == DialogResult.OK)
            {
                int employeeId = dbManager.AddEmployee(form.eTitle, form.eName, form.eStatus);
                dbManager.UpdateEmployeeAttributesFromTable(employeeId, form.eAttributes);
                
                if (config.notificationLevel >= 1)
                    MessageBox.Show($"Сотрудник '{form.eName}' добавлен!");

                RefreshTable();
            }
        }

        private void RefreshButton_Click(object sender, EventArgs e)
        {
            RefreshTable();
        }

        public async void RefreshTable()
        {
            refreshProgressBar.Maximum = 100;

            refreshProgressBar.Value = 20;
            await UpdateEmployeesTable();

            refreshProgressBar.Value = 100;
            await Task.Delay(1000);
            refreshProgressBar.Value = 0;
        }

        private Task UpdateEmployeesTable()
        {
            try
            {
                employeesData = dbManager.GetAllEmployeesWithAttributes();
                SetEmployeesTable();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке данных: " + ex.Message);
            }

            return Task.CompletedTask;
        }

        private void SetEmployeesTable()
        {
            employeesTable.CellClick -= DataGridViewEmployees_CellClick;
            employeesTable.Columns.Clear();

            employeesTable.DataSource = TablesTools.SearchInDataTable(TablesTools.FilterDataTable(TablesTools.FilterDataTable(employeesData, titleFilterText, "JobTitle"), statusFilterText,"Status"), searchText);
            employeesTable.ReadOnly = true;
            employeesTable.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            employeesTable.Columns["EmployeeID"].Visible = false;
            employeesTable.Columns["FullName"].DisplayIndex = 1;
            employeesTable.Columns["FullName"].HeaderText = "ФИО";
            employeesTable.Columns["JobTitle"].DisplayIndex = 2;
            employeesTable.Columns["JobTitle"].HeaderText = "Должность";
            employeesTable.Columns["Status"].DisplayIndex = 3;
            employeesTable.Columns["Status"].HeaderText = "Статус";


            employeesTable.AllowUserToAddRows = false;

            var editButtonColumn = new DataGridViewButtonColumn
            {
                HeaderText = "Редактирование сотрудника",
                Text = "Редактировать",
                UseColumnTextForButtonValue = true,
                Name = "EditButton"
            };
            employeesTable.Columns.Add(editButtonColumn);
            employeesTable.Columns["EditButton"].DisplayIndex = 2;

            // Подписываемся на событие для обработки нажатия на кнопку
            employeesTable.CellClick += DataGridViewEmployees_CellClick;

            var deleteButtonColumn = new DataGridViewButtonColumn
            {
                HeaderText = "Удаление сотрудника",
                Text = "Удалить",
                UseColumnTextForButtonValue = true,
                Name = "DeleteButton"
            };
            employeesTable.Columns.Add(deleteButtonColumn);
            employeesTable.Columns["DeleteButton"].DisplayIndex = employeesTable.Columns.Count - 1;

        }

        private void FillFilters()
        {
            var statusTable = dbManager.GetAllStatuses();
            statusFilter.Items.Add("Статус - Нет");

            foreach (DataRow row in statusTable.Rows)
            {
                string statusName = row["StatusName"].ToString();
                statusFilter.Items.Add(statusName);
            }


            var jobTitleTable = dbManager.GetAllJobTitles();
            titleFilter.Items.Add("Должность - Нет");

            foreach (DataRow row in jobTitleTable.Rows)
            {
                string jobTitle = row["Title"].ToString();
                titleFilter.Items.Add(jobTitle);
            }

            statusFilter.SelectedIndex = 0;
            titleFilter.SelectedIndex = 0;

            statusFilter.SelectedIndexChanged += (s, e) => { if (statusFilter.SelectedIndex == 0) statusFilterText = ""; else statusFilterText = statusFilter.Text; RefreshTable(); };
            titleFilter.SelectedIndexChanged += (s, e) => { if (titleFilter.SelectedIndex == 0) titleFilterText = ""; else titleFilterText = titleFilter.Text; RefreshTable(); };
        }

        private void DataGridViewEmployees_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == employeesTable.Columns["EditButton"].Index && e.RowIndex >= 0)
            {
                int employeeId = Convert.ToInt32(employeesTable.Rows[e.RowIndex].Cells["EmployeeID"].Value);

                DataTable employeeData = dbManager.GetEmployeeByIdRaw(employeeId);

                if (employeeData.Rows.Count > 0)
                {
                    DataRow row = employeeData.Rows[0];

                    var jobTitleValue = row["JobTitleID"];
                    var statusValue = row["StatusID"];


                    int jobTitleId = 0;
                    if (jobTitleValue != DBNull.Value)
                    {
                        if (!int.TryParse(jobTitleValue.ToString(), out jobTitleId))
                        {
                            throw new FormatException("JobTitle is not a valid integer.");
                        }
                    }
                    else
                    {
                        throw new InvalidOperationException("JobTitle is NULL.");
                    }

                    int statusId = 0;
                    if (statusValue != DBNull.Value)
                    {
                        if (!int.TryParse(statusValue.ToString(), out statusId))
                        {
                            throw new FormatException("Status is not a valid integer.");
                        }
                    }
                    else
                    {
                        throw new InvalidOperationException("Status is NULL.");
                    }

                    var form = new EditEmployee(dbManager, employeeId, row["FullName"].ToString(), jobTitleId, statusId);

                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            dbManager.UpdateEmployee(employeeId, form.eTitle, form.eName, form.eStatus);
                            dbManager.UpdateEmployeeAttributesFromTable(employeeId, form.eAttributes);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"{ex.Message}");
                        }

                        if (config.notificationLevel >= 1)
                            MessageBox.Show($"Сотрудник '{form.eName}' обновлен!");
                        
                        RefreshTable();
                    }
                }
                else
                {
                    MessageBox.Show("Сотрудник не найден.");
                }

                
            }
            else if(e.ColumnIndex == employeesTable.Columns["DeleteButton"].Index && e.RowIndex >= 0)
            {
                int employeeId = Convert.ToInt32(employeesTable.Rows[e.RowIndex].Cells["EmployeeID"].Value);
                string employeeName = employeesTable.Rows[e.RowIndex].Cells["FullName"].Value.ToString();

                DialogResult result = MessageBox.Show(
                   $"Вы собираетесь удалить сотрудника '{employeeName}' и все, что с ним связано (личные события, даты должностных событий).\nЭто действие невозможно обратить\n\nВы уверены?",
                   "Удаление сотрудника",
                   MessageBoxButtons.YesNo,
                   MessageBoxIcon.Warning
               );

                // Проверка результата
                if (result == DialogResult.Yes)
                    dbManager.DeleteEmployee(employeeId);

                RefreshTable();
            }
        }
    }
}
