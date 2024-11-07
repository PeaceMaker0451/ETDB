namespace ETDBs
{
    partial class ConfigForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.modeComboBox = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.connectionTextBox = new System.Windows.Forms.TextBox();
            this.notifyComboBox = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.configComboBox = new System.Windows.Forms.CheckBox();
            this.okButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(104, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Режим программы";
            // 
            // modeComboBox
            // 
            this.modeComboBox.FormattingEnabled = true;
            this.modeComboBox.Items.AddRange(new object[] {
            "Редактирование сотрудников",
            "Редактирование событий",
            "Отслеживание событий"});
            this.modeComboBox.Location = new System.Drawing.Point(15, 25);
            this.modeComboBox.Name = "modeComboBox";
            this.modeComboBox.Size = new System.Drawing.Size(273, 21);
            this.modeComboBox.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 60);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(113, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Строка подключения";
            // 
            // connectionTextBox
            // 
            this.connectionTextBox.Location = new System.Drawing.Point(15, 76);
            this.connectionTextBox.Name = "connectionTextBox";
            this.connectionTextBox.Size = new System.Drawing.Size(343, 20);
            this.connectionTextBox.TabIndex = 3;
            // 
            // notifyComboBox
            // 
            this.notifyComboBox.FormattingEnabled = true;
            this.notifyComboBox.Items.AddRange(new object[] {
            "Стандартный",
            "Осторожный"});
            this.notifyComboBox.Location = new System.Drawing.Point(15, 124);
            this.notifyComboBox.Name = "notifyComboBox";
            this.notifyComboBox.Size = new System.Drawing.Size(156, 21);
            this.notifyComboBox.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 108);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(117, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Уровень оповещения";
            // 
            // configComboBox
            // 
            this.configComboBox.AutoSize = true;
            this.configComboBox.Location = new System.Drawing.Point(15, 160);
            this.configComboBox.Name = "configComboBox";
            this.configComboBox.Size = new System.Drawing.Size(288, 17);
            this.configComboBox.TabIndex = 6;
            this.configComboBox.Text = "Вызывать окно конфигурации при каждом запуске";
            this.configComboBox.UseVisualStyleBackColor = true;
            // 
            // okButton
            // 
            this.okButton.Location = new System.Drawing.Point(15, 188);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(147, 23);
            this.okButton.TabIndex = 7;
            this.okButton.Text = "Готово";
            this.okButton.UseVisualStyleBackColor = true;
            // 
            // ConfigForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.ClientSize = new System.Drawing.Size(398, 223);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.configComboBox);
            this.Controls.Add(this.notifyComboBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.connectionTextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.modeComboBox);
            this.Controls.Add(this.label1);
            this.Name = "ConfigForm";
            this.Text = "Конфигуация";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox modeComboBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox connectionTextBox;
        private System.Windows.Forms.ComboBox notifyComboBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox configComboBox;
        private System.Windows.Forms.Button okButton;
    }
}