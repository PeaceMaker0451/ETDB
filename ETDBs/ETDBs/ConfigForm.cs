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
    internal partial class ConfigForm : Form
    {
        private Config config;
        public ConfigForm(Config _config)
        {
            this.StartPosition = FormStartPosition.WindowsDefaultLocation;
            InitializeComponent();

            config = _config;

            okButton.Click += (s, e) => { this.DialogResult = DialogResult.OK; this.Close(); };
            
            Ready();
            
            Program.SetFormSize(this);
        }

        private void Ready()
        {
            modeComboBox.SelectedIndex = config.ProgramMode;
            modeComboBox.SelectedIndexChanged += (s, e) => config.ProgramMode = modeComboBox.SelectedIndex;

            changeConnectionStringButton.Click += (s, e) => 
            { 
                var change = MessageBox.Show("Замена строки подключения перезапишет предыдущую строку. Вы уверены, что хотите сделать это?", "Замена строки подключения", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if(change == DialogResult.Yes)
                {
                    var dialog = new InputDialog("Введите новую строку подключения");
                    var result = dialog.ShowDialog();

                    if (result == DialogResult.OK)
                        config.DBConnectionPath = dialog.InputValue;
                }
            };
            //connectionTextBox.Text = config.DBConnectionPath;
            //connectionTextBox.TextChanged += (s, e) => config.DBConnectionPath = connectionTextBox.Text;

            notifyComboBox.SelectedIndex = config.notificationLevel;
            notifyComboBox.SelectedIndexChanged += (s, e) => config.notificationLevel = notifyComboBox.SelectedIndex;

            alwaysConfigComboBox.Checked = config.alwaysConfig;
            alwaysConfigComboBox.CheckedChanged += (s, e) => config.alwaysConfig = alwaysConfigComboBox.Checked;

            startWithSystemCheckBox.CheckedChanged += (s, e) => { config.startWithSystem = startWithSystemCheckBox.Checked; AutoStartManager.SetAutoStart(config.startWithSystem); };
            startWithSystemCheckBox.Checked = config.startWithSystem;

            fontSize.Value = config.textSize;
            fontSize.ValueChanged += (s, e) => { config.textSize = (int)fontSize.Value; Program.SetFormSize(this); };

            minutsToNotifyNum.Value = config.notificationInterval;
            minutsToNotifyNum.ValueChanged += (s, e) => config.notificationInterval = (int)minutsToNotifyNum.Value;

            daysToNotifyNum.Value = config.maxDaysToNotifyAboutEvent;
            daysToNotifyNum.ValueChanged += (s, e) => config.maxDaysToNotifyAboutEvent = (int)daysToNotifyNum.Value;

            notifyWhenNotHidedCheckBox.Checked = config.notifyWhenProgramIsNotHided;
            notifyWhenNotHidedCheckBox.CheckedChanged += (s, e) => config.notifyWhenProgramIsNotHided = notifyWhenNotHidedCheckBox.Checked;

            startHidedCheckBox.Checked = config.startHided;
            startHidedCheckBox.CheckedChanged += (s, e) => config.startHided = startHidedCheckBox.Checked;

            RedDays.Value = config.redDates;
            RedDays.ValueChanged += (s, e) => { config.redDates = (int)RedDays.Value; };

            yellowDays.ValueChanged += (s, e) => { RedDays.Maximum = yellowDays.Value; config.yellowDates = (int)yellowDays.Value; };
            yellowDays.Value = config.yellowDates;
        }
    }
}
