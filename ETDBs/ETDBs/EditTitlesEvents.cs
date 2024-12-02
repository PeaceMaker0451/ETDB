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
    public partial class EditTitlesEvents : Form
    {
        private DBManager dbManager;
        private Config config;

        private int selectedIndex;
        private int selectedID;
        private bool updating;

        public EditTitlesEvents(DBManager manager, Config config)
        {
            dbManager = manager;
            this.config = config;
            InitializeComponent();

            addEventButton.Click += AddEventButton_Click;
            jobTitlesList.SelectedIndexChanged += JobTitlesList_SelectedIndexChanged;
            eventsTable.DataBindingComplete += EventsTable_DataBindingComplete;

            exportButton.Click += (s, e) =>
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

                        DocumentHandler.CreateExcelFromDataGridView(eventsTable, filePath);
                    }
                }
            };

            RefreshTable();

            try
            {
                if(jobTitlesList.Items.Count > 0)
                {
                    int selectedEmployeeID = (int)jobTitlesList.SelectedValue;
                    selectedID = selectedEmployeeID;
                    RefreshTable();
                }
                
            }
            catch
            {

            }
            

            Program.SetFormSize(this);
        }

        private void AddEventButton_Click(object sender, EventArgs e)
        {
            if (jobTitlesList.Items.Count <= 0 || jobTitlesList.SelectedIndex < 0)
            {
                MessageBox.Show($"Невозможно добавить событие, выберите должность");
                return;
            }

            var form = new EditEvent(EditEventMode.OnlyBase);

            if (form.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    dbManager.AddJobTitleEvent(selectedID, form.Name, form.ToNext, form.IsMonths, form.OneTime);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"{ex.Message}");
                }

                if (config.notificationLevel == 1)
                    MessageBox.Show($"Событие '{form.Name}' добавлено!");

                RefreshTable();
            }
        }

        private void JobTitlesList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (updating)
                return;

            var listBox = sender as ListBox;

            if (listBox.SelectedValue != null)
            {
                if (!(listBox.SelectedValue is int))
                    return;

                int selectedTitleID = (int)listBox.SelectedValue;
                // Здесь можно использовать ID сотрудника
                selectedID = selectedTitleID;
                selectedIndex = listBox.SelectedIndex;
                RefreshTable();
            }
        }

        public async void RefreshTable()
        {
            updating = true;
            refreshBar.Maximum = 100;

            refreshBar.Value = 20;
            UpdateTitlesList();
            if(jobTitlesList.Items.Count <= 0)
            {
                refreshBar.Value = 0;
                return;
            }
            TrySetPreviousIndex();
            UpdateEventsTable();
            updating = false;
            refreshBar.Value = 100;
            await Task.Delay(1000);
            
        }

        private void UpdateTitlesList()
        {
            jobTitlesList.DataSource = null;
            jobTitlesList.Items.Clear();
            var titles = JobTitle.GetJobTitles(dbManager).OrderBy(e => e.JobTitleName).ToList();

            jobTitlesList.DataSource = titles;
            jobTitlesList.DisplayMember = nameof(JobTitle.JobTitleName);
            jobTitlesList.ValueMember = nameof(JobTitle.JobTitleID);
        }

        private void TrySetPreviousIndex()
        {
            try
            {
                jobTitlesList.SelectedIndex = selectedIndex;
            }
            catch (Exception e)
            {
                MessageBox.Show($"Невозможно сохранить выделение должности: {e.Message}");
                jobTitlesList.SelectedIndex = 0;
            }
        }

        private void UpdateEventsTable()
        {
            eventsTable.CellClick -= EventsTable_CellClick;
            eventsTable.Columns.Clear();

            var periodColumn = new DataGridViewTextBoxColumn
            {
                Name = "PeriodicityText",
                HeaderText = "Периодичность",
                ReadOnly = true,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                DisplayIndex = 3
            };
            eventsTable.Columns.Add(periodColumn);

            eventsTable.DataSource = dbManager.GetAllJobTitleEventsTable(selectedID);
            eventsTable.ReadOnly = true;
            eventsTable.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            if (config.notificationLevel == 0)
                eventsTable.Columns["EventID"].Visible = false;

            eventsTable.Columns["IsMonths"].Visible = false;
            eventsTable.Columns["EventName"].DisplayIndex = 0;
            eventsTable.Columns["EventName"].HeaderText = "Событие";
            eventsTable.Columns["ToNext"].Visible = false;
            eventsTable.Columns["OneTime"].Visible = false;
            eventsTable.AllowUserToAddRows = false;

            var editButtonColumn = new DataGridViewButtonColumn
            {
                HeaderText = "Редактирование должностного события",
                Text = "Редактировать",
                UseColumnTextForButtonValue = true,
                Name = "EditButton"
            };
            eventsTable.Columns.Add(editButtonColumn);
            eventsTable.Columns["EditButton"].DisplayIndex = 1;

            var deleteButtonColumn = new DataGridViewButtonColumn
            {
                HeaderText = "Удаление должностного события",
                Text = "Удалить",
                UseColumnTextForButtonValue = true,
                Name = "DeleteButton"
            };
            eventsTable.Columns.Add(deleteButtonColumn);
            eventsTable.Columns["DeleteButton"].DisplayIndex = eventsTable.Columns.Count - 1;

            eventsTable.CellClick += EventsTable_CellClick;
        }

        private void EventsTable_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewRow row in eventsTable.Rows)
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

                }
            }
        }

        private void EventsTable_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int eventId = 0;

            if (e.ColumnIndex == eventsTable.Columns["EditButton"].Index && e.RowIndex >= 0)
            {
                

                try
                {
                    eventId = Convert.ToInt32(eventsTable.Rows[e.RowIndex].Cells["EventID"].Value);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"{ex.Message}");
                }

                DataTable eventData = dbManager.GetJobTitleEventTable(selectedID, eventId);

                if (eventData.Rows.Count > 0)
                {
                    DataRow row = eventData.Rows[0];

                    var name = row["EventName"];
                    var toNext = row["ToNext"];
                    var isMonths = row["IsMonths"];
                    var oneTime = row["OneTime"];

                    var form = new EditEvent(name.ToString(), DateTime.Today, (int)toNext, (bool)isMonths, (bool)oneTime, 0, EditEventMode.OnlyBase);

                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            dbManager.UpdateJobTitleEvent(selectedID, eventId, form.Name, form.ToNext, form.IsMonths, form.OneTime);
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
                    MessageBox.Show("Невозможно изменить должность.");
                }
            }
            else if (e.ColumnIndex == eventsTable.Columns["DeleteButton"].Index && e.RowIndex >= 0)
            {
                try
                {
                    eventId = Convert.ToInt32(eventsTable.Rows[e.RowIndex].Cells["EventID"].Value);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"{ex.Message}");
                }

                DataTable eventData = dbManager.GetJobTitleEventTable(selectedID, eventId);

                if (eventData.Rows.Count > 0)
                {
                    DataRow row = eventData.Rows[0];

                    var name = row["EventName"];
                    var toNext = row["ToNext"];
                    var isMonths = row["IsMonths"];
                    var oneTime = row["OneTime"];

                    DialogResult result = MessageBox.Show(
                   $"Вы собираетесь удалить личное событие '{name}'.\nЭто действие невозможно обратить\n\nВы уверены?",
                   "Удаление даты события",
                   MessageBoxButtons.YesNo,
                   MessageBoxIcon.Warning
               );

                    // Проверка результата
                    try
                    {
                        if (result == DialogResult.Yes)
                            dbManager.DeleteJobTitleEvent(selectedID, eventId);
                        RefreshTable();
                    }
                    catch(Exception ex)
                    {
                        new TextDisplayForm("Ошибка", ex.ToString()).Show();
                    }
                    
                }
                else
                {
                    MessageBox.Show("Невозможно удалить должность.");
                }
            }
        }
    }
}
