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

        private bool edit;


        public int eID { get; private set; }
        public string eName { get; private set; }
        public int eTitle { get; private set; }
        public int eStatus { get; private set; }
        public DataTable eAttributes { get; private set; }

        private List<int> titlesIds;

        public EditEmployee(DBManager manager, bool _edit = true)
        {
            dbManager = manager;
            edit = _edit;
            InitializeComponent();


            UpdateAttributesTable();
            Ready();
            SetDefaultValues();
        }

        public EditEmployee(DBManager manager, int id, string name, int title, int status, bool _edit = true)
        {
            dbManager = manager;
            edit = _edit;
            InitializeComponent();

            UpdateAttributesTable(id);
            Ready();

            eID = id; eName = name; eTitle = title; eStatus = status;
            SetValues();
            SetDefaultValues();
        }

        private void SetDefaultValues()
        {
            eName = nameTextBox.Text;
            eTitle = (int)jobTitleComboBox.SelectedValue;
            eStatus = (int)statusComboBox.SelectedValue;
            eAttributes = (DataTable)attributesTable.DataSource;
        }

        private void Ready()
        {
            Program.SetFormSize(this);
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

            jobTitleComboBox.SelectedValueChanged += (s, e) => eTitle = (int)jobTitleComboBox.SelectedValue;
            statusComboBox.SelectedValueChanged += (s, e) => eStatus = (int)statusComboBox.SelectedValue;
            attributesTable.CellValueChanged += (s, e) => eAttributes = (DataTable)attributesTable.DataSource;
        }

        private void SetValues()
        {
            nameTextBox.Text = eName;
            jobTitleComboBox.SelectedValue = eTitle;
            statusComboBox.SelectedValue = eStatus;

            if(!edit)
            {
                nameTextBox.Enabled = false;
                statusComboBox.Enabled = false;
                jobTitleComboBox.Enabled = false;
            }
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

        private void UpdateAttributesTable(int id = 0)
        {
            attributesTable.Columns.Clear();
            attributesTable.AllowUserToAddRows = false;
            attributesTable.ReadOnly = false;
            try
            {
                DataTable titles = null;
                
                if (id <= 0)
                    titles = dbManager.GetEmptyAttributesTemplate();
                else
                    titles = dbManager.GetEmployeeAttributesById(id);

                attributesTable.DataSource = titles;
                
                
                attributesTable.Columns["EmployeeID"].Visible = false;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке данных: " + ex.Message);
            }
        }

        private void LoadJobTitles()
        {
            var jobTitles = dbManager.GetAllJobTitles();

            jobTitleComboBox.DisplayMember = "Title";
            jobTitleComboBox.ValueMember = "JobTitleID";
            jobTitleComboBox.DataSource = jobTitles;
        }

        private void LoadStatuses()
        {
            var statuses = dbManager.GetAllStatuses();
            statusComboBox.DisplayMember = "StatusName";
            statusComboBox.ValueMember = "StatusID";
            statusComboBox.DataSource = statuses;
        }
    }
}
