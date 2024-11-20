namespace ETDBs
{
    partial class EditEvent
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
            this.nameBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.isMonthsCombo = new System.Windows.Forms.ComboBox();
            this.toNextNumeric = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.startDate = new System.Windows.Forms.DateTimePicker();
            this.cancelButton = new System.Windows.Forms.Button();
            this.okButton = new System.Windows.Forms.Button();
            this.oneTimeCheckBox = new System.Windows.Forms.CheckBox();
            this.expiredCheckBox = new System.Windows.Forms.CheckBox();
            this.nextEventDate = new System.Windows.Forms.DateTimePicker();
            this.label4 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.toNextNumeric)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(103, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Название события";
            // 
            // nameBox
            // 
            this.nameBox.Location = new System.Drawing.Point(15, 25);
            this.nameBox.Name = "nameBox";
            this.nameBox.Size = new System.Drawing.Size(200, 20);
            this.nameBox.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 105);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(85, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Переодичность";
            // 
            // isMonthsCombo
            // 
            this.isMonthsCombo.FormattingEnabled = true;
            this.isMonthsCombo.Items.AddRange(new object[] {
            "Дней",
            "Месяцев"});
            this.isMonthsCombo.Location = new System.Drawing.Point(82, 121);
            this.isMonthsCombo.Name = "isMonthsCombo";
            this.isMonthsCombo.Size = new System.Drawing.Size(133, 21);
            this.isMonthsCombo.TabIndex = 3;
            // 
            // toNextNumeric
            // 
            this.toNextNumeric.Location = new System.Drawing.Point(15, 121);
            this.toNextNumeric.Name = "toNextNumeric";
            this.toNextNumeric.Size = new System.Drawing.Size(61, 20);
            this.toNextNumeric.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 57);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(158, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Дата первого возникновения";
            // 
            // startDate
            // 
            this.startDate.Location = new System.Drawing.Point(15, 73);
            this.startDate.Name = "startDate";
            this.startDate.Size = new System.Drawing.Size(200, 20);
            this.startDate.TabIndex = 6;
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(119, 237);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(96, 23);
            this.cancelButton.TabIndex = 7;
            this.cancelButton.Text = "Отмена";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // okButton
            // 
            this.okButton.Location = new System.Drawing.Point(15, 237);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(96, 23);
            this.okButton.TabIndex = 8;
            this.okButton.Text = "Готово";
            this.okButton.UseVisualStyleBackColor = true;
            // 
            // oneTimeCheckBox
            // 
            this.oneTimeCheckBox.AutoSize = true;
            this.oneTimeCheckBox.Location = new System.Drawing.Point(15, 152);
            this.oneTimeCheckBox.Name = "oneTimeCheckBox";
            this.oneTimeCheckBox.Size = new System.Drawing.Size(94, 17);
            this.oneTimeCheckBox.TabIndex = 9;
            this.oneTimeCheckBox.Text = "Одноразовое";
            this.oneTimeCheckBox.UseVisualStyleBackColor = true;
            // 
            // expiredCheckBox
            // 
            this.expiredCheckBox.AutoSize = true;
            this.expiredCheckBox.Location = new System.Drawing.Point(15, 175);
            this.expiredCheckBox.Name = "expiredCheckBox";
            this.expiredCheckBox.Size = new System.Drawing.Size(149, 17);
            this.expiredCheckBox.TabIndex = 10;
            this.expiredCheckBox.Text = "Просмотренно/Истекло";
            this.expiredCheckBox.UseVisualStyleBackColor = true;
            // 
            // nextEventDate
            // 
            this.nextEventDate.Location = new System.Drawing.Point(15, 211);
            this.nextEventDate.Name = "nextEventDate";
            this.nextEventDate.Size = new System.Drawing.Size(200, 20);
            this.nextEventDate.TabIndex = 12;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 195);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(199, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "(Следующее возникновение события)";
            // 
            // EditEvent
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.ClientSize = new System.Drawing.Size(231, 269);
            this.Controls.Add(this.nextEventDate);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.expiredCheckBox);
            this.Controls.Add(this.oneTimeCheckBox);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.startDate);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.toNextNumeric);
            this.Controls.Add(this.isMonthsCombo);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.nameBox);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "EditEvent";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Событие";
            ((System.ComponentModel.ISupportInitialize)(this.toNextNumeric)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox nameBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox isMonthsCombo;
        private System.Windows.Forms.NumericUpDown toNextNumeric;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker startDate;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.CheckBox oneTimeCheckBox;
        private System.Windows.Forms.CheckBox expiredCheckBox;
        private System.Windows.Forms.DateTimePicker nextEventDate;
        private System.Windows.Forms.Label label4;
    }
}