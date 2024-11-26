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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConfigForm));
            this.label1 = new System.Windows.Forms.Label();
            this.modeComboBox = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.connectionTextBox = new System.Windows.Forms.TextBox();
            this.notifyComboBox = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.alwaysConfigComboBox = new System.Windows.Forms.CheckBox();
            this.okButton = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label10 = new System.Windows.Forms.Label();
            this.fontSize = new System.Windows.Forms.NumericUpDown();
            this.startWithSystemCheckBox = new System.Windows.Forms.CheckBox();
            this.label9 = new System.Windows.Forms.Label();
            this.yellowDays = new System.Windows.Forms.NumericUpDown();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.daysToNotifyNum = new System.Windows.Forms.NumericUpDown();
            this.notifyWhenNotHidedCheckBox = new System.Windows.Forms.CheckBox();
            this.label8 = new System.Windows.Forms.Label();
            this.startHidedCheckBox = new System.Windows.Forms.CheckBox();
            this.RedDays = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.minutsToNotifyNum = new System.Windows.Forms.NumericUpDown();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fontSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.yellowDays)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.daysToNotifyNum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RedDays)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.minutsToNotifyNum)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 25);
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
            this.modeComboBox.Location = new System.Drawing.Point(9, 41);
            this.modeComboBox.Name = "modeComboBox";
            this.modeComboBox.Size = new System.Drawing.Size(192, 21);
            this.modeComboBox.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 65);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(113, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Строка подключения";
            // 
            // connectionTextBox
            // 
            this.connectionTextBox.Location = new System.Drawing.Point(9, 81);
            this.connectionTextBox.Name = "connectionTextBox";
            this.connectionTextBox.Size = new System.Drawing.Size(381, 20);
            this.connectionTextBox.TabIndex = 3;
            // 
            // notifyComboBox
            // 
            this.notifyComboBox.FormattingEnabled = true;
            this.notifyComboBox.Items.AddRange(new object[] {
            "Стандартный",
            "Осторожный",
            "Отладочный"});
            this.notifyComboBox.Location = new System.Drawing.Point(9, 120);
            this.notifyComboBox.Name = "notifyComboBox";
            this.notifyComboBox.Size = new System.Drawing.Size(192, 21);
            this.notifyComboBox.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 104);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(117, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Уровень оповещения";
            // 
            // alwaysConfigComboBox
            // 
            this.alwaysConfigComboBox.AutoSize = true;
            this.alwaysConfigComboBox.Location = new System.Drawing.Point(9, 147);
            this.alwaysConfigComboBox.Name = "alwaysConfigComboBox";
            this.alwaysConfigComboBox.Size = new System.Drawing.Size(288, 17);
            this.alwaysConfigComboBox.TabIndex = 6;
            this.alwaysConfigComboBox.Text = "Вызывать окно конфигурации при каждом запуске";
            this.alwaysConfigComboBox.UseVisualStyleBackColor = true;
            // 
            // okButton
            // 
            this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okButton.Location = new System.Drawing.Point(249, 417);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(147, 23);
            this.okButton.TabIndex = 7;
            this.okButton.Text = "Готово";
            this.okButton.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.fontSize);
            this.groupBox1.Controls.Add(this.startWithSystemCheckBox);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.modeComboBox);
            this.groupBox1.Controls.Add(this.alwaysConfigComboBox);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.notifyComboBox);
            this.groupBox1.Controls.Add(this.connectionTextBox);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(402, 228);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Общее";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(11, 195);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(88, 13);
            this.label10.TabIndex = 18;
            this.label10.Text = "Размер шрифта";
            // 
            // fontSize
            // 
            this.fontSize.Location = new System.Drawing.Point(105, 193);
            this.fontSize.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.fontSize.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.fontSize.Name = "fontSize";
            this.fontSize.Size = new System.Drawing.Size(61, 20);
            this.fontSize.TabIndex = 17;
            this.fontSize.Value = new decimal(new int[] {
            7,
            0,
            0,
            0});
            // 
            // startWithSystemCheckBox
            // 
            this.startWithSystemCheckBox.AutoSize = true;
            this.startWithSystemCheckBox.Location = new System.Drawing.Point(9, 170);
            this.startWithSystemCheckBox.Name = "startWithSystemCheckBox";
            this.startWithSystemCheckBox.Size = new System.Drawing.Size(272, 17);
            this.startWithSystemCheckBox.TabIndex = 7;
            this.startWithSystemCheckBox.Text = "Автоматически запускать при запуске системы";
            this.startWithSystemCheckBox.UseVisualStyleBackColor = true;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(11, 128);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(312, 13);
            this.label9.TabIndex = 14;
            this.label9.Text = "Кол-во оставшихся дней для отображения события желтым";
            // 
            // yellowDays
            // 
            this.yellowDays.Location = new System.Drawing.Point(329, 126);
            this.yellowDays.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.yellowDays.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.yellowDays.Name = "yellowDays";
            this.yellowDays.Size = new System.Drawing.Size(61, 20);
            this.yellowDays.TabIndex = 13;
            this.yellowDays.Value = new decimal(new int[] {
            7,
            0,
            0,
            0});
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.daysToNotifyNum);
            this.groupBox2.Controls.Add(this.notifyWhenNotHidedCheckBox);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.startHidedCheckBox);
            this.groupBox2.Controls.Add(this.RedDays);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.yellowDays);
            this.groupBox2.Controls.Add(this.minutsToNotifyNum);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox2.Location = new System.Drawing.Point(0, 228);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(402, 183);
            this.groupBox2.TabIndex = 9;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Отслеживание событий";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(359, 56);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(31, 13);
            this.label6.TabIndex = 12;
            this.label6.Text = "дней";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 56);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(280, 13);
            this.label7.TabIndex = 11;
            this.label7.Text = "Напоминать о событиях, если до них осталось менее";
            // 
            // daysToNotifyNum
            // 
            this.daysToNotifyNum.Location = new System.Drawing.Point(292, 54);
            this.daysToNotifyNum.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.daysToNotifyNum.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.daysToNotifyNum.Name = "daysToNotifyNum";
            this.daysToNotifyNum.Size = new System.Drawing.Size(61, 20);
            this.daysToNotifyNum.TabIndex = 10;
            this.daysToNotifyNum.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // notifyWhenNotHidedCheckBox
            // 
            this.notifyWhenNotHidedCheckBox.AutoSize = true;
            this.notifyWhenNotHidedCheckBox.Location = new System.Drawing.Point(9, 80);
            this.notifyWhenNotHidedCheckBox.Name = "notifyWhenNotHidedCheckBox";
            this.notifyWhenNotHidedCheckBox.Size = new System.Drawing.Size(275, 17);
            this.notifyWhenNotHidedCheckBox.TabIndex = 9;
            this.notifyWhenNotHidedCheckBox.Text = "Напоминать о событиях, когда окно не свернуто";
            this.notifyWhenNotHidedCheckBox.UseVisualStyleBackColor = true;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(11, 154);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(317, 13);
            this.label8.TabIndex = 16;
            this.label8.Text = "Кол-во оставшихся дней для отображения события красным";
            // 
            // startHidedCheckBox
            // 
            this.startHidedCheckBox.AutoSize = true;
            this.startHidedCheckBox.Location = new System.Drawing.Point(9, 103);
            this.startHidedCheckBox.Name = "startHidedCheckBox";
            this.startHidedCheckBox.Size = new System.Drawing.Size(277, 17);
            this.startHidedCheckBox.TabIndex = 8;
            this.startHidedCheckBox.Text = "Запускать окно отслеживания событий свернуто";
            this.startHidedCheckBox.UseVisualStyleBackColor = true;
            // 
            // RedDays
            // 
            this.RedDays.Location = new System.Drawing.Point(334, 152);
            this.RedDays.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.RedDays.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.RedDays.Name = "RedDays";
            this.RedDays.Size = new System.Drawing.Size(61, 20);
            this.RedDays.TabIndex = 15;
            this.RedDays.Value = new decimal(new int[] {
            7,
            0,
            0,
            0});
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(242, 29);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(37, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "минут";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 29);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(160, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Напоминать о событиях раз в";
            // 
            // minutsToNotifyNum
            // 
            this.minutsToNotifyNum.Location = new System.Drawing.Point(175, 27);
            this.minutsToNotifyNum.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.minutsToNotifyNum.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.minutsToNotifyNum.Name = "minutsToNotifyNum";
            this.minutsToNotifyNum.Size = new System.Drawing.Size(61, 20);
            this.minutsToNotifyNum.TabIndex = 0;
            this.minutsToNotifyNum.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // ConfigForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.ClientSize = new System.Drawing.Size(402, 452);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.okButton);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "ConfigForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Конфигуация";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fontSize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.yellowDays)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.daysToNotifyNum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RedDays)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.minutsToNotifyNum)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox modeComboBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox connectionTextBox;
        private System.Windows.Forms.ComboBox notifyComboBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox alwaysConfigComboBox;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.NumericUpDown minutsToNotifyNum;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox startWithSystemCheckBox;
        private System.Windows.Forms.CheckBox startHidedCheckBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown daysToNotifyNum;
        private System.Windows.Forms.CheckBox notifyWhenNotHidedCheckBox;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.NumericUpDown yellowDays;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.NumericUpDown RedDays;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.NumericUpDown fontSize;
    }
}