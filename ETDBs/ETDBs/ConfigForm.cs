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

            okButton.Click += (s, e) => this.Close();
            ValueReady();
        }

        private void ValueReady()
        {
            modeComboBox.SelectedIndex = config.ProgramMode;
            modeComboBox.SelectedIndexChanged += (s, e) => config.ProgramMode = modeComboBox.SelectedIndex;

            connectionTextBox.Text = config.DBConnectionPath;
            connectionTextBox.TextChanged += (s, e) => config.DBConnectionPath = connectionTextBox.Text;

            notifyComboBox.SelectedIndex = config.notificationLevel;
            notifyComboBox.SelectedIndexChanged += (s, e) => config.notificationLevel = notifyComboBox.SelectedIndex;
        }
    }
}
