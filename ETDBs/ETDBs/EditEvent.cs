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
    public enum EditEventMode
    {
        All,
        OnlyBase,
        OnlyDate
    }
    
    public partial class EditEvent : Form
    {
        public string Name { get; private set; }
        public DateTime StartDate { get; private set; }
        public int ToNext { get; private set; }
        public bool IsMonths { get; private set; }

        public bool OneTime { get; private set; }

        public int Expired { get; private set; }
        private int baseExpired;

        private EditEventMode Mode;

        public EditEvent(EditEventMode mode = EditEventMode.All)
        {
            InitializeComponent();

            Name = "Новое событие";
            StartDate = DateTime.Today;
            Mode = mode;

            this.AcceptButton = okButton;
            this.CancelButton = cancelButton;

            Ready();
        }

        public EditEvent(string name, DateTime startDate, int toNext, bool isMonths, bool oneTime, int expired, EditEventMode mode = EditEventMode.All)
        {
            InitializeComponent();

            Name = name;
            StartDate = startDate;
            ToNext = toNext;
            IsMonths = isMonths;
            OneTime = oneTime;
            Expired = expired;
            Mode = mode;

            Ready();
        }

        private void Ready()
        {
            Program.SetFormSize(this);
            nameBox.Text = Name;
            nameBox.TextChanged += (s, e) => Name = nameBox.Text;

            if (OneTime)
            {
                toNextNumeric.Minimum = 0;
            }
            else
            {
                toNextNumeric.Minimum = 1;
            }

            startDate.Value = StartDate;
            startDate.ValueChanged += (s, e) => { StartDate = startDate.Value; UpdateNextDate(); baseExpired = 0; };

            try
            {
                toNextNumeric.Value = ToNext;
            }
            catch
            {
                ToNext = 1;
                toNextNumeric.Value = 1;
            }
            toNextNumeric.ValueChanged += (s, e) =>
            {
                ToNext = (int)toNextNumeric.Value;
                UpdateNextDate();
            };



            SetIsMonthCombo(IsMonths);
            isMonthsCombo.SelectedIndexChanged += (s, e) => 
            { 
                if (isMonthsCombo.SelectedIndex == 1) 
                    IsMonths = true; 
                else IsMonths = false; 

                UpdateNextDate(); 
            };

            oneTimeCheckBox.CheckedChanged += (s, e) => 
            { 
                OneTime = oneTimeCheckBox.Checked; 
                UpdateNextDate(); 

                if(oneTimeCheckBox.Checked)
                {
                    toNextNumeric.Enabled = false;
                    isMonthsCombo.Enabled = false;
                    toNextNumeric.Minimum = 0;
                    toNextNumeric.Value = 0;

                }
                else
                {
                    toNextNumeric.Enabled = true;
                    isMonthsCombo.Enabled = true;
                    toNextNumeric.Minimum = 1;
                }
            };

            oneTimeCheckBox.Checked = OneTime;

            baseExpired = 0;
            if (TablesTools.CalculateNextEventDate(StartDate, IsMonths, ToNext, Expired) <= DateTime.Today)
                expiredCheckBox.Checked = false;
            else
                expiredCheckBox.Checked = true;

            if(OneTime == true && Expired == 1)
            {
                expiredCheckBox.Checked = true;
                baseExpired = 0;
            }
                

            expiredCheckBox.CheckedChanged += (s, e) => 
            {  
                UpdateNextDate(); 
            };

            UpdateNextDate();

            switch (Mode)
            {
                case EditEventMode.OnlyBase:
                    startDate.Enabled = false;
                    expiredCheckBox.Enabled = false;
                    nextEventDate.Enabled = false;
                    break;
                case EditEventMode.OnlyDate:
                    nameBox.Enabled = false;
                    toNextNumeric.Enabled = false;
                    isMonthsCombo.Enabled = false;
                    oneTimeCheckBox.Enabled = false;
                    break;
            }

            cancelButton.Click += CancelButton_Click;
            okButton.Click += OkButton_Click;
        }

        private void UpdateNextDate()
        {
            if (Mode == EditEventMode.OnlyBase)
                return;
            
            if(StartDate > DateTime.Today)
            {
                expiredCheckBox.Checked = false;
                expiredCheckBox.Enabled = false;
                Expired = 0;
            }
            else
            {
                expiredCheckBox.Enabled = true;
            }
            
            if (expiredCheckBox.Checked == true)
            {
                if (OneTime == true || ToNext == 0)
                {
                    Expired = 1;
                }
                else
                {
                    Expired = 0;
                    while (TablesTools.CalculateNextEventDate(StartDate, IsMonths, ToNext, Expired) <= DateTime.Today)
                    {
                        Expired++;
                    }
                }
            }
            else
                Expired = baseExpired;

            nextEventDate.Value = TablesTools.CalculateNextEventDate(StartDate, IsMonths, ToNext, Expired);
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            this.DialogResult= DialogResult.OK;
            this.Close();
        }

        private void SetIsMonthCombo(bool isMonths)
        {
            if (isMonths)
            {
                isMonthsCombo.SelectedIndex = 1;
            }
            else
            {
                isMonthsCombo.SelectedIndex = 0;
            }
        }
    }
}
