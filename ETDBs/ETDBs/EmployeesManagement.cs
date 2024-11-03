using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ETDBs
{
    public partial class EmployeesManagement : Form
    {
        private DBManager dbManager;

        private string searchText;
        private DataTable employeesData;
        
        public EmployeesManagement(DBManager manager)
        {
            dbManager = manager;
            InitializeComponent();

            editJobTitlesButton.Click += EditJobTitlesButton_Click;
            addStatusButton.Click += AddStatusButton_Click;
            addAttributeButton.Click += AddAttributeButton_Click;
            createNewEmployeeButton.Click += CreateNewEmployeeButton_Click;
            refreshButton.Click += RefreshButton_Click;

            searchTextBox.TextChanged += (s, e) => { searchText = searchTextBox.Text;  SetEmployeesTable(); };

            UpdateEmployeesTable();
        }

        private void EditJobTitlesButton_Click(object sender, EventArgs e)
        {
            var form = new EditJobTitles(dbManager);

            form.ShowDialog();
            RefreshTable();
        }

        private void AddStatusButton_Click(object sender, EventArgs e)
        {
            string name = InputDialog.ShowDialog("Новый статус");
            if (name != null)
            {
                dbManager.AddOrUpdateEmployeeAttribute(1, name, "");
                MessageBox.Show($"Статус '{name}' добавлен!");
                RefreshTable();
            }
        }

        private void AddAttributeButton_Click(object sender, EventArgs e)
        {
            string name = InputDialog.ShowDialog("Имя нового параметра");
            if(name != null)
            {
                dbManager.AddOrUpdateEmployeeAttribute(1, name, "");
                MessageBox.Show($"Аттрибут '{name}' добавлен!");
                RefreshTable(); 
            }
        }

        private void CreateNewEmployeeButton_Click(object sender, EventArgs e)
        {
            var form = new EditEmployee(dbManager);

            if (form.ShowDialog() == DialogResult.OK)
            {
                int employeeId = dbManager.AddEmployee(form.eTitle, form.eName, form.eHireTime, form.eMedTime, form.eStatus);
                dbManager.UpdateEmployeeAttributesFromTable(employeeId, form.eAttributes);
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

            // Присваиваем таблицу DataGridView
            employeesTable.DataSource = TablesTools.SearchInDataTable(employeesData, searchText);
            employeesTable.ReadOnly = true;
            employeesTable.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            employeesTable.Columns["EmployeeID"].Visible = false;
            employeesTable.Columns["FullName"].DisplayIndex = 0;
            employeesTable.Columns["FullName"].HeaderText = "ФИО";
            employeesTable.Columns["JobTitle"].HeaderText = "Должность";
            employeesTable.Columns["Status"].HeaderText = "Статус";
            employeesTable.Columns["HireDate"].HeaderText = "Дата найма";
            employeesTable.Columns["MedDate"].HeaderText = "Дата прохождения медосмотра";


            employeesTable.AllowUserToAddRows = false;

            var editButtonColumn = new DataGridViewButtonColumn
            {
                HeaderText = "Редактирование",
                Text = "Редактировать",
                UseColumnTextForButtonValue = true,  // Устанавливаем текст для всех кнопок
                Name = "EditButton"                  // Уникальное имя для обработки нажатия
            };
            employeesTable.Columns.Add(editButtonColumn);
            employeesTable.Columns["EditButton"].DisplayIndex = 1;

            // Подписываемся на событие для обработки нажатия на кнопку
            employeesTable.CellClick += DataGridViewEmployees_CellClick;
        }

        private Task SetSearchFilter()
        {
            searchFilterComboBox.Items.Clear();

            return Task.CompletedTask;
        }

        private void DataGridViewEmployees_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Проверяем, что клик был по кнопке "Редактировать"
            if (e.ColumnIndex == employeesTable.Columns["EditButton"].Index && e.RowIndex >= 0)
            {
                // Получаем ID сотрудника из текущей строки
                int employeeId = Convert.ToInt32(employeesTable.Rows[e.RowIndex].Cells["EmployeeID"].Value);

                DataTable employeeData = dbManager.GetEmployeeByIdRaw(employeeId);

                if (employeeData.Rows.Count > 0)
                {
                    // Получаем первую строку данных
                    DataRow row = employeeData.Rows[0];

                    var medDate = row["MedDate"];
                    var hireDate = row["HireDate"];

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

                    if (medDate == DBNull.Value || !DateTime.TryParse(medDate.ToString(), out DateTime medDateValue))
                    {
                        throw new FormatException("MedDate is not a valid DateTime.");
                    }

                    if (hireDate == DBNull.Value || !DateTime.TryParse(hireDate.ToString(), out DateTime hireDateValue))
                    {
                        throw new FormatException("HireDate is not a valid DateTime.");
                    }

                    var form = new EditEmployee(dbManager, employeeId, row["FullName"].ToString(), jobTitleId, medDateValue, hireDateValue, statusId);

                    //var form = new EditEmployee(dbManager, employeeId, row["FullName"].ToString(), Convert.ToInt32(row["JobTitle"]), Convert.ToDateTime(row["MedDate"]), Convert.ToDateTime(row["HireDate"]), Convert.ToInt32(row["Status"]));

                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        dbManager.UpdateEmployee(employeeId, form.eTitle, form.eName, form.eHireTime, form.eMedTime, form.eStatus);
                        try
                        {
                            dbManager.UpdateEmployeeAttributesFromTable(employeeId, form.eAttributes);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"{ex.Message}");
                        }
                        
                        MessageBox.Show($"Сотрудник '{form.eName}' обновлен!");
                        RefreshTable();
                    }
                }
                else
                {
                    MessageBox.Show("Сотрудник не найден.");
                }

                
            }
        }
    }
}
