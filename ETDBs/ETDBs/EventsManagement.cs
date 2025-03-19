using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Diagnostics;

namespace ETDBs
{
    public partial class EventsManagement : Form
    {
        private DBManager dbManager;
        private Config config;

        private int selectedIndex;
        private int selectedID;

        private bool updating;
        public EventsManagement(DBManager manager, Config config)
        {
            dbManager = manager;
            this.config = config; 
            InitializeComponent();
            employeesList.SelectedIndexChanged += EmployeesList_SelectedIndexChanged;
            titleEventsTable.DataBindingComplete += TitleEventsTable_DataBindingComplete;
            employeeEventsTable.DataBindingComplete += EmployeeEventsTable_DataBindingComplete;
            addEmployeeEventButton.Click += AddEmployeeEventButton_Click;
            editTitleEventsButton.Click += EditTitleEventsButton_Click;

            aboutTool.Click += ViewEmployeeMenuStrip_Click;
            employeesList.MouseDown += EmployeesList_MouseDown;
            viewAllTool.Click += ViewAllTool_Click;

            configMenuItem.Click += (s, e) => { this.DialogResult = DialogResult.Retry; this.Close(); };
            refreshButton.Click += (s,e) => RefreshTable();

            открытьПапкуДанныхПрограммыToolStripMenuItem.Click += (s, e) =>
            {
                if (System.IO.Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ETDB")))
                {
                    Process.Start("explorer.exe", Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ETDB"));
                }
            };

            оПрограммеToolStripMenuItem1.Click += (s, e) => { new About().ShowDialog(); };
            планировщикСобытийToolStripMenuItem.Click += (s, e) => new EventsPlanning(dbManager, config, true).ShowDialog();
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

            RefreshTable();
            try
            {
                if (employeesList.Items.Count > 0)
                {
                    int selectedEmployeeID = (int)employeesList.SelectedValue;
                    selectedID = selectedEmployeeID;
                    RefreshTable();
                }
                    
            }
            catch
            {

            }
            

            Program.SetFormSize(this);

            typeof(DataGridView).InvokeMember("DoubleBuffered",
            System.Reflection.BindingFlags.NonPublic |
            System.Reflection.BindingFlags.Instance |
            System.Reflection.BindingFlags.SetProperty,
            null, employeeEventsTable, new object[] { true });

            typeof(DataGridView).InvokeMember("DoubleBuffered",
            System.Reflection.BindingFlags.NonPublic |
            System.Reflection.BindingFlags.Instance |
            System.Reflection.BindingFlags.SetProperty,
            null, titleEventsTable, new object[] { true });
        }

        private void ViewAllTool_Click(object sender, EventArgs e)
        {
            using (var tableViewer = new TableViewForm(dbManager.GetAllEmployeesWithAttributes()))
            {
                tableViewer.Load += (s, _e) =>
                {
                    tableViewer.dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
                    tableViewer.dataGridView.Columns["EmployeeID"].Visible = false;
                    tableViewer.dataGridView.Columns["FullName"].DisplayIndex = 0;
                    tableViewer.dataGridView.Columns["FullName"].HeaderText = "ФИО";
                    tableViewer.dataGridView.Columns["JobTitle"].HeaderText = "Должность";
                    tableViewer.dataGridView.Columns["Status"].HeaderText = "Статус";
                };

                tableViewer.ShowDialog();
            }
        }

        private void ViewEmployeeMenuStrip_Click(object sender, EventArgs e)
        {
            if (employeesList.SelectedItem is Employee selectedEmployee)
            {
                var employeeId = selectedEmployee.EmployeeID;
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

                    var form = new EditEmployee(dbManager, employeeId, row["FullName"].ToString(), jobTitleId, statusId, false);

                    form.ShowDialog();
                }
            }
        }

        private void EmployeesList_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                int index = employeesList.IndexFromPoint(e.Location);
                if (index != ListBox.NoMatches)
                {
                    employeesList.SelectedIndex = index;
                    viewEmployeeMenuStrip.Show(employeesList, e.Location);
                }
            }
        }

        private void EditTitleEventsButton_Click(object sender, EventArgs e)
        {
            var form = new EditTitlesEvents(dbManager, config);
            try
            {
                form.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Невозможно редактировать события должностей.");
            }
            RefreshTable();
        }

        private void AddEmployeeEventButton_Click(object sender, EventArgs e)
        {
            var form = new EditEvent();

            if(employeesList.Items.Count <= 0 || employeesList.SelectedIndex < 0)
            {
                MessageBox.Show($"Невозможно добавить событие, выберите сотрудника");
                return;
            }
            
            if (form.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    dbManager.AddEmployeeEvent(selectedID,form.Name,form.StartDate,form.ToNext,form.IsMonths, form.OneTime, form.Expired);
                    if (config.notificationLevel == 1)
                        MessageBox.Show($"Событие '{form.Name}' добавлено!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"{ex.Message}");
                }

                RefreshTable();
            }
        }

        private void EmployeesList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (updating)
                return;

            var listBox = sender as ListBox;

            if (listBox.SelectedValue != null)
            {
                if (!(listBox.SelectedValue is int))
                    return;

                int selectedEmployeeID = (int)listBox.SelectedValue;
                selectedID = selectedEmployeeID;
                selectedIndex = listBox.SelectedIndex;
                RefreshTable();
            }
        }

        public async Task RefreshTable()
        {
            updating = true;
            refreshBar.Maximum = 100;

            refreshBar.Value = 20;
            UpdateEmployeesList();
            if (employeesList.Items.Count <= 0)
            {
                refreshBar.Value = 0;
                return;
            }
            TrySetPreviousIndex();
            UpdateTitleEventsTable();
            UpdateEmployeeEventsTable();
            updating = false;
            refreshBar.Value = 100;
            await Task.Delay(1000);
            refreshBar.Value = 0;
        }

        private void UpdateEmployeesList()
        {
            employeesList.DataSource = null;
            employeesList.Items.Clear();
            var employees = Employee.GetEmployees(dbManager)
                            .OrderBy(e => e.DisplayText) // Сортировка по DisplayText
                            .ToList();

            employeesList.DataSource = employees;
            employeesList.DisplayMember = nameof(Employee.DisplayText);
            employeesList.ValueMember = nameof(Employee.EmployeeID);
        }

        private void TrySetPreviousIndex()
        {
            try
            {
                employeesList.SelectedIndex = selectedIndex;
            }
            catch(Exception e)
            {
                if(config.notificationLevel >= 2)
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
                AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells,
                DisplayIndex = 3
            };
            titleEventsTable.Columns.Add(periodColumn);

            var nextEventColumn = new DataGridViewTextBoxColumn
            {
                Name = "NextEventDate",
                HeaderText = "Дата следующего события",
                ReadOnly = true,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells,
                DisplayIndex = 5
            };
            titleEventsTable.Columns.Add(nextEventColumn);

            titleEventsTable.DataSource = dbManager.GetAllEmployeeJobTitleEventsTable(selectedID);
            titleEventsTable.ReadOnly = true;
            titleEventsTable.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            
            if (config.notificationLevel == 0)
                titleEventsTable.Columns["EventID"].Visible = false;

            titleEventsTable.Columns["IsMonths"].Visible = false;
            titleEventsTable.Columns["EventName"].DisplayIndex = 0;
            titleEventsTable.Columns["EventName"].HeaderText = "Событие";
            titleEventsTable.Columns["StartDate"].HeaderText = "Дата начала отсчета";
            titleEventsTable.Columns["OneTime"].Visible = false;
            titleEventsTable.Columns["Expired"].Visible = false;
            titleEventsTable.Columns["StartDate"].DisplayIndex = 2;
            titleEventsTable.Columns["ToNext"].Visible = false;
            titleEventsTable.AllowUserToAddRows = false;

            var editButtonColumn = new DataGridViewButtonColumn
            {
                HeaderText = "Редактирование даты должностного события",
                Text = "Редактировать",
                UseColumnTextForButtonValue = true,
                Name = "EditButton"
            };
            titleEventsTable.Columns.Add(editButtonColumn);
            titleEventsTable.Columns["EditButton"].DisplayIndex = 1;

            var deleteButtonColumn = new DataGridViewButtonColumn
            {
                HeaderText = "Удаление даты должностного события",
                Text = "Удалить",
                UseColumnTextForButtonValue = true,
                Name = "DeleteButton"
            };
            titleEventsTable.Columns.Add(deleteButtonColumn);
            titleEventsTable.Columns["DeleteButton"].DisplayIndex = titleEventsTable.Columns.Count - 1;

            titleEventsTable.CellClick += TitleEventsTable_CellClick;
        }

        private void TitleEventsTable_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewRow row in titleEventsTable.Rows)
            {
                if (row.Cells["IsMonths"].Value is bool isMonths &&
                    row.Cells["ToNext"].Value is int toNextValue &&
                    titleEventsTable.Columns.Contains("PeriodicityText"))
                {
                    // Генерируем текст для столбца PeriodicityText
                    string periodText = "";

                    if ((bool)row.Cells["OneTime"].Value == true)
                        periodText = $"Одноразовое";
                    else
                        periodText = isMonths ? $"Месяцы - {toNextValue}" : $"Дни - {toNextValue}";
                    
                    row.Cells["PeriodicityText"].Value = periodText;

                    if (row.Cells["StartDate"].Value is DateTime startDate &&
                   titleEventsTable.Columns.Contains("NextEventDate"))
                    {

                        if ((bool)row.Cells["OneTime"].Value == true && (int)row.Cells["Expired"].Value == 1)
                        {
                            row.Cells["NextEventDate"].Value = "Истекло";
                        }
                        else
                        {
                            DateTime nextEventDate = TablesTools.CalculateNextEventDate(startDate, isMonths, toNextValue, (int)row.Cells["Expired"].Value);
                            row.Cells["NextEventDate"].Value = nextEventDate.ToString("dd.MM.yyyy");
                        }
                            

                        
                    }
                }
            }
        }

        private void TitleEventsTable_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int eventId = 0;

            if (e.ColumnIndex == titleEventsTable.Columns["EditButton"].Index && e.RowIndex >= 0)
            {
                

                try
                {
                    eventId = Convert.ToInt32(titleEventsTable.Rows[e.RowIndex].Cells["EventID"].Value);
                }
                catch(Exception ex)
                {
                    MessageBox.Show($"{ex.Message}");
                }
                
                DataTable eventData = dbManager.GetEmployeeJobTitleEventTable(selectedID,eventId);

                if (eventData.Rows.Count > 0)
                {
                    DataRow row = eventData.Rows[0];

                    var name = row["EventName"];
                    var startDate = row["StartDate"];
                    var toNext = row["ToNext"];
                    var isMonths = row["IsMonths"];
                    var oneTime = row["OneTime"];
                    var expired = row["Expired"];

                    if (startDate == DBNull.Value || !DateTime.TryParse(startDate.ToString(), out DateTime startDateValue))
                    {
                        startDateValue = DateTime.Today;
                    }
                    if (oneTime == DBNull.Value)
                    {
                        oneTime = false;
                    }
                    if (expired == DBNull.Value)
                    {
                        expired = 0;
                    }

                    var form = new EditEvent(name.ToString(), startDateValue, (int)toNext, (bool)isMonths, (bool)oneTime, (int)expired, EditEventMode.OnlyDate);

                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            dbManager.AddOrUpdateJobTitleEventDate(selectedID,eventId,form.StartDate, form.Expired);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"{ex.Message}");
                        }

                        if (config.notificationLevel == 1)
                            MessageBox.Show($"Событие '{form.Name}' обновлено!");

                        RefreshTable();
                    }
                }
                else
                {
                    MessageBox.Show("Невозможно изменить событие.");
                }
            }
            else if (e.ColumnIndex == titleEventsTable.Columns["DeleteButton"].Index && e.RowIndex >= 0)
            {
                try
                {
                    eventId = Convert.ToInt32(titleEventsTable.Rows[e.RowIndex].Cells["EventID"].Value);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"{ex.Message}");
                }

                DataTable eventData = dbManager.GetEmployeeJobTitleEventTable(selectedID, eventId);

                if (eventData.Rows.Count > 0)
                {
                    DataRow row = eventData.Rows[0];

                    var name = row["EventName"];
                    var startDate = row["StartDate"];
                    var toNext = row["ToNext"];
                    var isMonths = row["IsMonths"];
                    var oneTime = row["OneTime"];
                    var expired = row["Expired"];

                    if (startDate == DBNull.Value || !DateTime.TryParse(startDate.ToString(), out DateTime startDateValue))
                    {
                        startDateValue = DateTime.Today;
                    }
                    if (oneTime == DBNull.Value)
                    {
                        oneTime = false;
                    }
                    if (expired == DBNull.Value)
                    {
                        expired = 0;
                    }

                    DialogResult result = MessageBox.Show(
                   $"Вы собираетесь удалить дату должостного события '{name}'. Событие перестанет отображаться в планировании и дату можно будет выставить заново.\nЭто действие невозможно обратить\n\nВы уверены?",
                   "Удаление даты события",
                   MessageBoxButtons.YesNo,
                   MessageBoxIcon.Warning
               );

                    // Проверка результата
                    if (result == DialogResult.Yes)
                        dbManager.DeleteTitleEventDate(selectedID, eventId);
                    RefreshTable();
                }
                else
                {
                    MessageBox.Show("Невозможно удалить событие.");
                }
            }
        }

        private void UpdateEmployeeEventsTable()
        {
            employeeEventsTable.CellClick -= EmployeeEventsTable_CellClick;
            employeeEventsTable.Columns.Clear();

            var periodColumn = new DataGridViewTextBoxColumn
            {
                Name = "PeriodicityText",
                HeaderText = "Периодичность",
                ReadOnly = true,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells,
                DisplayIndex = 3
            };
            employeeEventsTable.Columns.Add(periodColumn);

            var nextEventColumn = new DataGridViewTextBoxColumn
            {
                Name = "NextEventDate",
                HeaderText = "Дата следующего события",
                ReadOnly = true,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells,
                DisplayIndex = 5
            };
            employeeEventsTable.Columns.Add(nextEventColumn);

            employeeEventsTable.DataSource = dbManager.GetAllEmployeeEventsTable(selectedID);
            employeeEventsTable.ReadOnly = true;
            employeeEventsTable.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            if (config.notificationLevel == 0)
                employeeEventsTable.Columns["EventID"].Visible = false;
            employeeEventsTable.Columns["IsMonths"].Visible = false;
            employeeEventsTable.Columns["ToNext"].Visible = false;
            employeeEventsTable.Columns["EventName"].DisplayIndex = 0;
            employeeEventsTable.Columns["EventName"].HeaderText = "Событие";
            employeeEventsTable.Columns["StartDate"].HeaderText = "Дата начала отсчета";
            employeeEventsTable.Columns["OneTime"].Visible = false;
            employeeEventsTable.Columns["Expired"].Visible = false;
            employeeEventsTable.Columns["StartDate"].DisplayIndex = 2;
            employeeEventsTable.AllowUserToAddRows = false;

            var editButtonColumn = new DataGridViewButtonColumn
            {
                HeaderText = "Редактирование личного события",
                Text = "Редактировать",
                UseColumnTextForButtonValue = true,
                Name = "EditButton"
            };
            employeeEventsTable.Columns.Add(editButtonColumn);
            employeeEventsTable.Columns["EditButton"].DisplayIndex = 1;

            var deleteButtonColumn = new DataGridViewButtonColumn
            {
                HeaderText = "Удаление личного события",
                Text = "Удалить",
                UseColumnTextForButtonValue = true,
                Name = "DeleteButton"
            };
            employeeEventsTable.Columns.Add(deleteButtonColumn);
            employeeEventsTable.Columns["DeleteButton"].DisplayIndex = employeeEventsTable.Columns.Count - 1;

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
                    string periodText = "";

                    if ((bool)row.Cells["OneTime"].Value == true)
                        periodText = $"Одноразовое";
                    else
                        periodText = isMonths ? $"Месяцы - {toNextValue}" : $"Дни - {toNextValue}";
                    
                    row.Cells["PeriodicityText"].Value = periodText;

                    if (row.Cells["StartDate"].Value is DateTime _startDate &&
                   titleEventsTable.Columns.Contains("NextEventDate"))
                    {

                        if ((bool)row.Cells["OneTime"].Value == true && (int)row.Cells["Expired"].Value == 1)
                        {
                            row.Cells["NextEventDate"].Value = "Истекло";
                        }
                        else
                        {
                            DateTime nextEventDate = TablesTools.CalculateNextEventDate(_startDate, isMonths, toNextValue, (int)row.Cells["Expired"].Value);
                            row.Cells["NextEventDate"].Value = nextEventDate.ToString("dd.MM.yyyy");
                        }
                            

                        
                    }
                }
            }
        }

        private void EmployeeEventsTable_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int eventId = 0;

            if (e.ColumnIndex == employeeEventsTable.Columns["EditButton"].Index && e.RowIndex >= 0)
            {
                

                try
                {
                    eventId = Convert.ToInt32(employeeEventsTable.Rows[e.RowIndex].Cells["EventID"].Value);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"{ex.Message}");
                }

                DataTable eventData = dbManager.GetEmployeeEventTable(selectedID,eventId);

                if (eventData.Rows.Count > 0)
                {
                    DataRow row = eventData.Rows[0];

                    var name = row["EventName"];
                    var startDate = row["StartDate"];
                    var toNext = row["ToNext"];
                    var isMonths = row["IsMonths"];
                    var oneTime = row["OneTime"];
                    var expired = row["Expired"];

                    var form = new EditEvent(name.ToString(), (DateTime)startDate, (int)toNext,(bool)isMonths, (bool)oneTime, (int)expired);

                    if (form.ShowDialog() == DialogResult.OK)
                    { 
                        try
                        {
                            dbManager.UpdateEmployeeEvent(selectedID, eventId, form.Name, form.StartDate, form.ToNext, form.IsMonths, form.OneTime, form.Expired);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"{ex.Message}");
                        }

                        if(config.notificationLevel == 1)
                        MessageBox.Show($"Событие '{form.Name}' обновлено!");

                        RefreshTable();
                    }
                }
                else
                {
                    MessageBox.Show("Невозможно изменить событие.");
                }
            }
            else if (e.ColumnIndex == employeeEventsTable.Columns["DeleteButton"].Index && e.RowIndex >= 0)
            {
                try
                {
                    eventId = Convert.ToInt32(employeeEventsTable.Rows[e.RowIndex].Cells["EventID"].Value);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"{ex.Message}");
                }

                DataTable eventData = dbManager.GetEmployeeEventTable(selectedID, eventId);

                if (eventData.Rows.Count > 0)
                {
                    DataRow row = eventData.Rows[0];

                    var name = row["EventName"];
                    var startDate = row["StartDate"];
                    var toNext = row["ToNext"];
                    var isMonths = row["IsMonths"];
                    var oneTime = row["OneTime"];
                    var expired = row["Expired"];

                    if (startDate == DBNull.Value || !DateTime.TryParse(startDate.ToString(), out DateTime startDateValue))
                    {
                        startDateValue = DateTime.Today;
                    }
                    if (oneTime == DBNull.Value)
                    {
                        oneTime = false;
                    }
                    if (expired == DBNull.Value)
                    {
                        expired = 0;
                    }

                    DialogResult result = MessageBox.Show(
                   $"Вы собираетесь удалить личное событие '{name}'.\nЭто действие невозможно обратить\n\nВы уверены?",
                   "Удаление даты события",
                   MessageBoxButtons.YesNo,
                   MessageBoxIcon.Warning
               );

                    // Проверка результата
                    if (result == DialogResult.Yes)
                        dbManager.DeleteEvent(eventId);
                    RefreshTable();
                }
                else
                {
                    MessageBox.Show("Невозможно удалить событие.");
                }
            }
        }

        private void обновитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RefreshTable();
        }

        private void добавитьЛичноеСобытиеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddEmployeeEventButton_Click(sender,e);
        }

        private void редактироватьДолжностныеСобытияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EditTitleEventsButton_Click(sender, e);
        }
    }
}
