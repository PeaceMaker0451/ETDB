using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Xml.Linq;
using System.IO;

namespace ETDBs
{
    public partial class EventsPlanning : Form
    {
        private DBManager dbManager;
        private Config config;

        private DataTable employeesData;
        private DataTable filteredEmployeesData;
        private string searchText = "";
        private string statusFilterText = "";
        private string titleFilterText = "";

        private bool showExpired = true;

        private System.Threading.Timer timer;

        private List<DateTime> deadLines = new List<DateTime>();

        private bool formIsHided = false;


        public EventsPlanning(DBManager manager, Config _config)
        {
            dbManager = manager;
            config = _config;
            InitializeComponent();
            FillFilters();

            this.SizeChanged += EventsPlanning_SizeChanged;
            this.FormClosing += EventsPlanning_FormClosing;
            this.Load += (s, e) => { eventsTable.ClearSelection(); employeesTable.ClearSelection(); };

            configMenuItem.Click += (s, e) => { this.DialogResult = DialogResult.Retry; this.Close(); };
            
            refreshButton.Click += (s, e) => RefreshTable();
            refreshMenuItem.Click += (s, e) => RefreshTable();
            exportEventsTableButton.Click += ExportEventsTableButton_Click;
            exportEventsTableMenuItem.Click += ExportEventsTableButton_Click;
            exportEmployeesTableButton.Click += ExportEmployeesTableButton_Click;
            exportEmployeesTableMenuItem.Click += ExportEmployeesTableButton_Click;

            aboutMenuItem.Click += (s, e) => { new About().ShowDialog(); };
            licenseMenuItem.Click += (s, e) =>
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

            toolStripButton1.Click += (s, e) => 
            {
                new TemplateDocumentExport(TagExtractor.ExtractTagsFromDataGridView(eventsTable, "SelectCheckBox")).ShowDialog();
            }; 

            searchTextBox.TextChanged += (s, e) => { searchText = searchTextBox.Text; RefreshTableAsync(); };
            RefreshTable();

            if (config.notifyWhenProgramIsNotHided)
                timer = new System.Threading.Timer(TimerCallback, null, 0, 60000 * config.notificationInterval);

            if (config.startHided)
                this.Load += (s,e) => this.WindowState = FormWindowState.Minimized;

            this.FormClosed += (s, e) => this.notifyIcon1.Visible = false;

            Program.SetFormSize(this);
        }

        private void ExportEmployeesTableButton_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "Excel Workbook (*.xlsx)|*.xlsx";
                saveFileDialog.Title = "Сохранить как";
                saveFileDialog.DefaultExt = "xlsx";
                saveFileDialog.AddExtension = true;
                saveFileDialog.FileName = "NewTable";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // Получаем путь выбранного файла
                    string filePath = saveFileDialog.FileName;

                    // Создаем файл или выполняем с ним какие-то действия
                    DocumentHandler.CreateExcelFromDataGridView(employeesTable, filePath);
                }
            }
        }

        private void ExportEventsTableButton_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "Excel Workbook (*.xlsx)|*.xlsx";
                saveFileDialog.Title = "Сохранить как";
                saveFileDialog.DefaultExt = "xlsx";
                saveFileDialog.AddExtension = true;
                saveFileDialog.FileName = "NewTable";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // Получаем путь выбранного файла
                    string filePath = saveFileDialog.FileName;

                    // Создаем файл или выполняем с ним какие-то действия
                    DocumentHandler.CreateExcelFromDataGridView(eventsTable, filePath);
                }
            }
        }

        private async void TimerCallback(object state)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke((MethodInvoker)(async () => RefreshTable()));
            }
            else
            {
                RefreshTable();
            }

            NotifyAboutDeadLines();

        }

        private void EventsPlanning_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.None)
            {
                e.Cancel = true;
                return;
            }

            DialogResult result = MessageBox.Show(
                    $"Вы собираетесь закрыть программу. В этом случае, вы не будете получать уведомления о событиях.\nВы уверены?",
                    "Выход из программы отслеживания событий",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

            if (result == DialogResult.No)
            {
                e.Cancel = true;
            }
            else
            {
                timer?.Dispose();
            }
        }

        private void EventsPlanning_SizeChanged(object sender, EventArgs e)
        {
            if (formIsHided)
                return;
            
            if (this.WindowState == FormWindowState.Minimized )
            {
                //this.TopMost = true;
                //formIsHided = true;
                notifyIcon1.Visible = true;
                //this.ShowInTaskbar = false;

                notifyIcon1.BalloonTipTitle = "ETDB - Программа свернута.";
                notifyIcon1.BalloonTipText = "Программа скрыта и продолжит работать в фоне. \nЧтобы снова открыть окно, нажмите дважды на ярлык программы в трее.";
                notifyIcon1.BalloonTipIcon = ToolTipIcon.Info;
                notifyIcon1.ShowBalloonTip(10000);
                
                if(!config.notifyWhenProgramIsNotHided)
                    timer = new System.Threading.Timer(TimerCallback, null, 0, 60000 * config.notificationInterval);
            }
            if(this.WindowState == FormWindowState.Normal)
            {
                notifyIcon1.Visible = false;
                //this.ShowInTaskbar = true;

                if (!config.notifyWhenProgramIsNotHided)
                    timer?.Dispose();
            }
        }

        private void NotifyAboutDeadLines()
        {
            TablesTools.NotifyEvents(eventsTable, config.maxDaysToNotifyAboutEvent, config.simplifyNotifications, notifyIcon1);
        }

        public void RefreshTable()
        {
            try
            {
                UpdateEmployeesTable();
                UpdateEventsTable();
            }
            catch(Exception ex)
            {
                if(config.notificationLevel > 1)
                {
                    MessageBox.Show($"Ошибка обновления таблиц - {ex}");
                }
            } 
        }

        public async Task RefreshTableAsync()
        {
            try
            {
                refreshBar.Maximum = 100;

                refreshBar.Value = 20;
            }
            catch (Exception ex)
            {
                if (config.notificationLevel > 2)
                {
                    MessageBox.Show($"Ошибка обращения к интерфейсу - {ex}");
                }
            }
            try
            {
                await UpdateEmployeesTable();
                await UpdateEventsTable();
            }
            catch (Exception ex)
            {
                if (config.notificationLevel > 1)
                {
                    MessageBox.Show($"Ошибка обновления таблиц - {ex}");
                }
            }
            try
            {
                refreshBar.Value = 100;
                await Task.Delay(1000);
                refreshBar.Value = 0;
            }
            catch (Exception ex)
            {
                if (config.notificationLevel > 1)
                {
                    MessageBox.Show($"Ошибка обращения к интерфейсу - {ex}");
                }
            }
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

            try
            {
                filteredEmployeesData = TablesTools.SearchInDataTable(TablesTools.FilterDataTable(TablesTools.FilterDataTable(employeesData, titleFilterText, "JobTitle"), statusFilterText, "Status"), searchText);
            }
            catch { }
            
            employeesTable.DataSource = filteredEmployeesData;
            employeesTable.ReadOnly = true;
            employeesTable.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            
            if(config.notificationLevel < 2)
                employeesTable.Columns["EmployeeID"].Visible = false;

            employeesTable.Columns["FullName"].DisplayIndex = 0;
            employeesTable.Columns["FullName"].HeaderText = "ФИО";
            employeesTable.Columns["JobTitle"].HeaderText = "Должность";
            employeesTable.Columns["Status"].HeaderText = "Статус";


            employeesTable.AllowUserToAddRows = false;

            var viewButtonColumn = new DataGridViewButtonColumn
            {
                HeaderText = "Дополнительно",
                Text = "Подробнее",
                UseColumnTextForButtonValue = true,
                Name = "ViewButton"
            };
            employeesTable.Columns.Add(viewButtonColumn);
            employeesTable.Columns["ViewButton"].DisplayIndex = 1;

            employeesTable.CellClick += DataGridViewEmployees_CellClick;
        }

        private void DataGridViewEmployees_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == employeesTable.Columns["ViewButton"].Index && e.RowIndex >= 0)
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

                    var form = new EditEmployee(dbManager, employeeId, row["FullName"].ToString(), jobTitleId, statusId, false);

                    form.ShowDialog();
                }
                else
                {
                    MessageBox.Show("Сотрудник не найден.");
                }


            }
        }

        private Task UpdateEventsTable()
        {
            eventsTable.CellClick -= EventsTable_CellClick;

            eventsTable.Columns.Clear();
            eventsTable.AllowUserToAddRows = false;

            eventsTable.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            try
            {
                TablesTools.CopyDataTableToDataGridView(dbManager.GetAllEmployeesEventsWithAttributes(filteredEmployeesData), eventsTable);
            }
            catch { }
            

            var deadLineColumn = new DataGridViewTextBoxColumn
            {
                Name = "DeadLine",
                HeaderText = "DeadLine",
                ReadOnly = true,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                DisplayIndex = 1
            };
            eventsTable.Columns.Add(deadLineColumn);

            var urgencyColumn = new DataGridViewTextBoxColumn
            {
                Name = "UrgencyLevel",
                HeaderText = "Осталось дней",
                ReadOnly = true,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                DisplayIndex = 0
            };
            eventsTable.Columns.Add(urgencyColumn);

            var nextDate = new DataGridViewTextBoxColumn
            {
                Name = "NextEventDate",
                HeaderText = "Дата",
                ValueType = typeof(DateTime),
                ReadOnly = true,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                DisplayIndex = 5
            };
            eventsTable.Columns.Add(nextDate);

            var PeriodicityText = new DataGridViewTextBoxColumn
            {
                Name = "PeriodicityText",
                HeaderText = "Периодичность",
                ReadOnly = true,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                DisplayIndex = 5
            };
            eventsTable.Columns.Add(PeriodicityText);

            var markAsViewedColumn = new DataGridViewButtonColumn
            {
                HeaderText = "Отметить событие просмотренным",
                Text = "Просмотрено",
                UseColumnTextForButtonValue = true,
                Name = "ViewedButton"
            };
            eventsTable.Columns.Add(markAsViewedColumn);
            eventsTable.Columns["ViewedButton"].DisplayIndex = 0;

            var exportColumn = new DataGridViewButtonColumn
            {
                HeaderText = "Экспортировать событие по шаблону",
                Text = "Экспортировать",
                UseColumnTextForButtonValue = true,
                Name = "ExportButton"
            };
            eventsTable.Columns.Add(exportColumn);
            eventsTable.Columns["ExportButton"].DisplayIndex = 2;

            var selectCheckBoxColumn = new DataGridViewCheckBoxColumn
            {
                HeaderText = "Выделение строк",
                Name = "SelectCheckBox"
            };
            eventsTable.Columns.Add(selectCheckBoxColumn);
            eventsTable.Columns["SelectCheckBox"].DisplayIndex = 1;

            foreach (DataGridViewColumn column in eventsTable.Columns)
            {
                column.ReadOnly = true;
            }

            eventsTable.Columns["SelectCheckBox"].ReadOnly = false;

            try
            {
                eventsTable.Columns["EmployeeName"].HeaderText = "ФИО";
                eventsTable.Columns["JobTitle"].HeaderText = "Должность";
                eventsTable.Columns["EventName"].HeaderText = "Событие";
                eventsTable.Columns["Expired"].HeaderText = "Было просмотренно раз:";

                if (config.notificationLevel < 2)
                {
                    eventsTable.Columns["EmployeeID"].Visible = false;
                    eventsTable.Columns["EventID"].Visible = false;
                    eventsTable.Columns["EventDate"].Visible = false;
                    eventsTable.Columns["OneTime"].Visible = false;
                    eventsTable.Columns["ToNext"].Visible = false;
                    eventsTable.Columns["IsMonths"].Visible = false;
                    eventsTable.Columns["DeadLine"].Visible = false;
                    eventsTable.Columns["IsTitleEvent"].Visible = false;
                }
            }
            catch(Exception ex)
            {
                if (config.notificationLevel > 1)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
            
            

            foreach (DataGridViewRow row in eventsTable.Rows)
            {
                if (eventsTable.Rows.Count > 0)
                {
                    if (row.Cells["IsMonths"].Value is bool isMonths &&
                    row.Cells["ToNext"].Value is int toNextValue &&
                    eventsTable.Columns.Contains("PeriodicityText"))
                    {
                        string periodText = "";

                        if ((bool)row.Cells["OneTime"].Value == true)
                            periodText = $"Одноразовое";
                        else
                            periodText = isMonths ? $"Месяцы - {toNextValue}" : $"Дни - {toNextValue}";

                        row.Cells["PeriodicityText"].Value = periodText;

                        if (row.Cells["EventDate"].Value is DateTime startDate && eventsTable.Columns.Contains("NextEventDate"))
                        {
                            if ((bool)row.Cells["OneTime"].Value == true && (int)row.Cells["Expired"].Value == 1)
                            {
                                row.Cells["NextEventDate"].Value = "Истекло";
                                
                                if(!showExpired)
                                    row.Visible = false;
                            }
                            else
                            {
                                DateTime nextEventDate = TablesTools.CalculateNextEventDate(startDate, isMonths, toNextValue, (int)row.Cells["Expired"].Value);
                                row.Cells["NextEventDate"].Value = nextEventDate.Date;
                            }

                            try
                            {
                                if (eventsTable.Columns.Contains("DeadLine") && eventsTable.Columns.Contains("UrgencyLevel"))
                                {
                                    if(row.Cells["NextEventDate"].Value is DateTime nextEventDate)
                                    {
                                        var deadLineValue = (nextEventDate - DateTime.Today).Days;

                                        row.Cells["DeadLine"].Value = deadLineValue;

                                        row.Cells["UrgencyLevel"].Value = $"{deadLineValue} дней";

                                        if (deadLineValue >= config.yellowDates)
                                        {
                                            row.Cells["UrgencyLevel"].Style.BackColor = Color.DarkGreen;
                                            row.Cells["UrgencyLevel"].Style.ForeColor = Color.White;
                                        }
                                        else if (deadLineValue >= config.redDates)
                                        {
                                            row.Cells["UrgencyLevel"].Style.BackColor = Color.DarkGoldenrod;
                                            row.Cells["UrgencyLevel"].Style.ForeColor = Color.White;
                                        }
                                        else if (deadLineValue > 0)
                                        {
                                            row.Cells["UrgencyLevel"].Style.BackColor = Color.DarkRed;
                                            row.Cells["UrgencyLevel"].Style.ForeColor = Color.White;
                                        }
                                        else
                                        {
                                            row.Cells["UrgencyLevel"].Style.BackColor = Color.Black;
                                            row.Cells["UrgencyLevel"].Style.ForeColor = Color.White;
                                        }
                                    }
                                    else
                                    {
                                        row.Cells["DeadLine"].Value = 10000000;
                                        row.Cells["UrgencyLevel"].Value = $"Истекло";
                                        row.Cells["UrgencyLevel"].Style.BackColor = Color.Black;
                                        row.Cells["UrgencyLevel"].Style.ForeColor = Color.White;
                                    }
                                    
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show($"Ошибка рассчета остатка дней - {ex.Message}");
                            }
                            
                        }
                    }
                }
            }

            foreach (DataGridViewColumn column in eventsTable.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            eventsTable.Sort(eventsTable.Columns["DeadLine"], ListSortDirection.Ascending);
            eventsTable.CellClick += EventsTable_CellClick;
            return Task.CompletedTask;
        }

        private void EventsTable_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Проверяем, что клик был по кнопке "Редактировать"
            if (e.ColumnIndex == eventsTable.Columns["ViewedButton"].Index && e.RowIndex >= 0)
            {
                var name = eventsTable.Rows[e.RowIndex].Cells["EventName"].Value;
                var employee = eventsTable.Rows[e.RowIndex].Cells["EmployeeName"].Value;

                DialogResult result = MessageBox.Show(
                    $"Вы подтвердите просмотр события '{name}' для сотрудника {employee}.\n\nЭто сменит дату события следующей, или сделает событие истекшим.\n\nВы уверены?",
                    "Просмотр события",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                // Проверка результата
                if (result == DialogResult.Yes)
                {
                    bool isTitle;
                    int employeeID;
                    int eventID;
                    DateTime startDate;
                    DateTime nextEventDate;
                    int expired;

                    try
                    {
                        isTitle = (bool)eventsTable.Rows[e.RowIndex].Cells["IsTitleEvent"].Value;
                        employeeID = (int)eventsTable.Rows[e.RowIndex].Cells["EmployeeID"].Value;
                        eventID = (int)eventsTable.Rows[e.RowIndex].Cells["EventID"].Value;
                        startDate = (DateTime)eventsTable.Rows[e.RowIndex].Cells["EventDate"].Value;
                        nextEventDate = (DateTime)eventsTable.Rows[e.RowIndex].Cells["NextEventDate"].Value;
                        expired = (int)eventsTable.Rows[e.RowIndex].Cells["Expired"].Value; //NextEventDate
                    }
                    catch
                    {
                        MessageBox.Show($"Это событие не может быть просмотренно");
                        return;
                    }

                    if (isTitle)
                    {
                        try
                        {
                            dbManager.AddOrUpdateJobTitleEventDate(employeeID, eventID, startDate, expired + 1);
                        }
                        catch
                        {
                            MessageBox.Show($"Событие {name} ({nextEventDate}) не может быть просмотренно");
                            return;
                        }
                        
                        
                        if (config.notificationLevel >= 1)
                            MessageBox.Show($"Событие {name} от {nextEventDate} успешно зачтено");

                        RefreshTableAsync();
                    }
                    else
                    {
                        var oneTime = (bool)eventsTable.Rows[e.RowIndex].Cells["OneTime"].Value;
                        var isMonths = (bool)eventsTable.Rows[e.RowIndex].Cells["IsMonths"].Value;
                        var timeToNext = (int)eventsTable.Rows[e.RowIndex].Cells["ToNext"].Value;

                        
                        try
                        {
                            dbManager.UpdateEmployeeEvent(employeeID, eventID, name.ToString(), startDate, timeToNext, isMonths, oneTime, expired + 1);
                        }
                        catch
                        {
                            MessageBox.Show($"Событие {name} ({nextEventDate}) не может быть просмотренно");
                            return;
                        }

                        if (config.notificationLevel >= 1)
                            MessageBox.Show($"Событие {name} от {nextEventDate} успешно зачтено");

                        RefreshTableAsync();
                    }
                }
            }
            else if (e.ColumnIndex == eventsTable.Columns["ExportButton"].Index && e.RowIndex >= 0)
            {
                var clickedRow = eventsTable.Rows[e.RowIndex];

                // Инициализируем словарь
                Dictionary<string, string> rowData = new Dictionary<string, string>();

                // Проходим по всем колонкам строки
                foreach (DataGridViewColumn column in eventsTable.Columns)
                {
                    // Пропускаем колонки с кнопками и чекбоксами
                    if (column is DataGridViewButtonColumn || column is DataGridViewCheckBoxColumn)
                        continue;

                    // Получаем видимое имя колонки
                    string columnName = column.HeaderText;

                    // Получаем значение ячейки
                    var cellValue = clickedRow.Cells[column.Index].Value;

                    // Если значение является DateTime, форматируем его, иначе преобразуем в строку
                    string formattedValue = cellValue is DateTime dateValue
                        ? dateValue.ToString("dd.MM.yyyy")
                        : cellValue?.ToString() ?? string.Empty;

                    // Добавляем в словарь
                    rowData[columnName] = formattedValue;
                }

                new TemplateDocumentExport(rowData).ShowDialog();
            }
        }

        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
        }
    }
}
