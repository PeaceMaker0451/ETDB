using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace ETDBs
{
    public partial class EditEmployee : Form
    {
        private DBManager dbManager;


        public int eID { get; private set; }
        public string eName { get; private set; }
        public int eTitle { get; private set; }
        public DateTime eMedTime { get; private set; }
        public DateTime eHireTime { get; private set; }
        public int eStatus { get; private set; }
        public DataTable eAttributes { get; private set; }

        public EditEmployee(DBManager manager)
        {
            dbManager = manager;
            InitializeComponent();


            UpdateTitlesTable();
            Ready();
            SetDefaultValues();
        }

        public EditEmployee(DBManager manager, int id, string name, int title, DateTime medTime, DateTime hireTime, int status)
        {
            dbManager = manager;
            InitializeComponent();

            UpdateTitlesTable(id);
            Ready();

            eID = id; eName = name; eTitle = title; eMedTime = medTime; eHireTime = hireTime; eStatus = status;
            SetValues();
            SetDefaultValues();
        }

        private void SetDefaultValues()
        {
            eName = nameTextBox.Text;
            eTitle = jobTitleComboBox.SelectedIndex + 1;
            eHireTime = hireDate.Value;
            eMedTime = medDate.Value;
            eStatus = statusComboBox.SelectedIndex + 1;
            eAttributes = (DataTable)attributesTable.DataSource;
        }

        private void Ready()
        {
            this.StartPosition = FormStartPosition.CenterScreen;
            LoadJobTitles();
            LoadStatuses();
            cancelButton.Click += CancelButton_Click;
            confirmButton.Click += ConfirmButton_Click;

            nameTextBox.TextChanged += (s, e) => 
            { 
                eName = nameTextBox.Text; 
                if (nameTextBox.Text == null || nameTextBox.Text == "") 
                    nameCheckIcon.Visible = true; 
                else 
                    nameCheckIcon.Visible = false; 
            };

            jobTitleComboBox.SelectedValueChanged += (s, e) => eTitle = jobTitleComboBox.SelectedIndex + 1;
            hireDate.ValueChanged += (s, e) => eHireTime = hireDate.Value;
            medDate.ValueChanged += (s, e) => eMedTime = medDate.Value;
            statusComboBox.SelectedValueChanged += (s, e) => eStatus = statusComboBox.SelectedIndex + 1;
            attributesTable.CellValueChanged += (s, e) => eAttributes = (DataTable)attributesTable.DataSource;
        }

        private void SetValues()
        {
            nameTextBox.Text = eName;
            jobTitleComboBox.SelectedIndex = eTitle - 1;
            hireDate.Value = eHireTime;
            medDate.Value = eMedTime;
            statusComboBox.SelectedIndex = eStatus - 1;
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void ConfirmButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void UpdateTitlesTable(int id = 0)
        {
            attributesTable.Columns.Clear();
            try
            {
                DataTable titles = null;
                
                if (id == 0)
                    titles = dbManager.GetEmptyAttributesTemplate();
                else
                    titles = dbManager.GetEmployeeAttributesById(id);

                attributesTable.DataSource = titles;
                attributesTable.ReadOnly = false;
                attributesTable.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                attributesTable.Columns["EmployeeID"].Visible = false;
                attributesTable.AllowUserToAddRows = false;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке данных: " + ex.Message);
            }
        }

        private void LoadJobTitles()
        {
            var jobTitles = dbManager.GetAllJobTitles();
            jobTitleComboBox.DisplayMember = "Title"; // Имя столбца для отображения
            jobTitleComboBox.ValueMember = "JobTitleID"; // Имя столбца для значения
            jobTitleComboBox.DataSource = jobTitles;
        }

        // Метод для загрузки статусов в ComboBox
        private void LoadStatuses()
        {
            var statuses = dbManager.GetAllStatuses();
            statusComboBox.DisplayMember = "StatusName"; // Имя столбца для отображения
            statusComboBox.ValueMember = "StatusID"; // Имя столбца для значения
            statusComboBox.DataSource = statuses;
        }
    }
}
