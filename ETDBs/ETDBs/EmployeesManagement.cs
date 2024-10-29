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
        
        public EmployeesManagement(DBManager manager)
        {
            dbManager = manager;
            InitializeComponent();

            UpdateEmployeesTable();
        }

        private void UpdateEmployeesTable()
        {
            try
            {
                // Получаем DataTable с данными работников
                DataTable _employeesTable = dbManager.GetAllEmployees();

                // Присваиваем таблицу DataGridView
                employeesTable.DataSource = _employeesTable;
                employeesTable.ReadOnly = true;
                employeesTable.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                employeesTable.Columns["EmployeeID"].Visible = false;
                employeesTable.Columns["FullName"].DisplayIndex = 0;
                employeesTable.Columns["FullName"].HeaderText = "ФИО";
                employeesTable.Columns["JobTitle"].HeaderText = "Должность";

                employeesTable.AllowUserToAddRows = false;

                var editButtonColumn = new DataGridViewButtonColumn
                {
                    HeaderText = "Редактирование",
                    Text = "Редактировать",
                    UseColumnTextForButtonValue = true,  // Устанавливаем текст для всех кнопок
                    Name = "EditButton"                  // Уникальное имя для обработки нажатия
                };
                employeesTable.Columns.Add(editButtonColumn);

                // Подписываемся на событие для обработки нажатия на кнопку
                employeesTable.CellClick += DataGridViewEmployees_CellClick;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке данных: " + ex.Message);
            }
        }

        private void DataGridViewEmployees_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Проверяем, что клик был по кнопке "Редактировать"
            if (e.ColumnIndex == employeesTable.Columns["EditButton"].Index && e.RowIndex >= 0)
            {
                // Получаем ID сотрудника из текущей строки
                int employeeId = Convert.ToInt32(employeesTable.Rows[e.RowIndex].Cells["EmployeeID"].Value);

                // Вызываем метод редактирования, передавая ID сотрудника
                MessageBox.Show(employeeId.ToString());
            }
        }
    }
}
