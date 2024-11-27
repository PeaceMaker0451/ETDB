namespace ETDBs
{
    partial class EmployeesManagement
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EmployeesManagement));
            this.employeesTable = new System.Windows.Forms.DataGridView();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.tableTool = new System.Windows.Forms.ToolStripMenuItem();
            this.экспортироватьВExcelТаблицуToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.обновитьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editTool = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteAttribute = new System.Windows.Forms.ToolStripMenuItem();
            this.добавитьДолжностьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.добавитьДопПолеToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.configMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutTool = new System.Windows.Forms.ToolStripMenuItem();
            this.справкаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.лицензияToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.createNewEmployeeButton = new System.Windows.Forms.ToolStripButton();
            this.editJobTitlesButton = new System.Windows.Forms.ToolStripButton();
            this.addStatusButton = new System.Windows.Forms.ToolStripButton();
            this.addAttributeButton = new System.Windows.Forms.ToolStripButton();
            this.refreshButton = new System.Windows.Forms.ToolStripButton();
            this.refreshProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.searchTextBox = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.titleFilter = new System.Windows.Forms.ToolStripComboBox();
            this.statusFilter = new System.Windows.Forms.ToolStripComboBox();
            this.exportButton = new System.Windows.Forms.ToolStripButton();
            this.textBox1 = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.employeesTable)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // employeesTable
            // 
            this.employeesTable.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.employeesTable.BackgroundColor = System.Drawing.SystemColors.GradientActiveCaption;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.employeesTable.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.employeesTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.employeesTable.DefaultCellStyle = dataGridViewCellStyle2;
            this.employeesTable.Location = new System.Drawing.Point(0, 51);
            this.employeesTable.Name = "employeesTable";
            this.employeesTable.Size = new System.Drawing.Size(829, 443);
            this.employeesTable.TabIndex = 0;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tableTool,
            this.editTool,
            this.aboutTool});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.menuStrip1.Size = new System.Drawing.Size(829, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // tableTool
            // 
            this.tableTool.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.экспортироватьВExcelТаблицуToolStripMenuItem,
            this.обновитьToolStripMenuItem});
            this.tableTool.Name = "tableTool";
            this.tableTool.Size = new System.Drawing.Size(68, 20);
            this.tableTool.Text = "Таблицы";
            // 
            // экспортироватьВExcelТаблицуToolStripMenuItem
            // 
            this.экспортироватьВExcelТаблицуToolStripMenuItem.Name = "экспортироватьВExcelТаблицуToolStripMenuItem";
            this.экспортироватьВExcelТаблицуToolStripMenuItem.Size = new System.Drawing.Size(250, 22);
            this.экспортироватьВExcelТаблицуToolStripMenuItem.Text = "Экспортировать в Excel таблицу";
            // 
            // обновитьToolStripMenuItem
            // 
            this.обновитьToolStripMenuItem.Name = "обновитьToolStripMenuItem";
            this.обновитьToolStripMenuItem.Size = new System.Drawing.Size(250, 22);
            this.обновитьToolStripMenuItem.Text = "Обновить";
            // 
            // editTool
            // 
            this.editTool.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deleteAttribute,
            this.добавитьДолжностьToolStripMenuItem,
            this.добавитьДопПолеToolStripMenuItem,
            this.configMenuItem});
            this.editTool.Name = "editTool";
            this.editTool.Size = new System.Drawing.Size(59, 20);
            this.editTool.Text = "Правка";
            // 
            // deleteAttribute
            // 
            this.deleteAttribute.Name = "deleteAttribute";
            this.deleteAttribute.Size = new System.Drawing.Size(223, 22);
            this.deleteAttribute.Text = "Удалить доп. поле";
            this.deleteAttribute.Click += new System.EventHandler(this.deleteAttribute_Click);
            // 
            // добавитьДолжностьToolStripMenuItem
            // 
            this.добавитьДолжностьToolStripMenuItem.Name = "добавитьДолжностьToolStripMenuItem";
            this.добавитьДолжностьToolStripMenuItem.Size = new System.Drawing.Size(223, 22);
            this.добавитьДолжностьToolStripMenuItem.Text = "Добавить должность";
            // 
            // добавитьДопПолеToolStripMenuItem
            // 
            this.добавитьДопПолеToolStripMenuItem.Name = "добавитьДопПолеToolStripMenuItem";
            this.добавитьДопПолеToolStripMenuItem.Size = new System.Drawing.Size(223, 22);
            this.добавитьДопПолеToolStripMenuItem.Text = "Добавить доп. поле";
            // 
            // configMenuItem
            // 
            this.configMenuItem.Name = "configMenuItem";
            this.configMenuItem.Size = new System.Drawing.Size(223, 22);
            this.configMenuItem.Text = "Вернуться к конфигурации";
            // 
            // aboutTool
            // 
            this.aboutTool.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.справкаToolStripMenuItem,
            this.лицензияToolStripMenuItem});
            this.aboutTool.Name = "aboutTool";
            this.aboutTool.Size = new System.Drawing.Size(94, 20);
            this.aboutTool.Text = "О программе";
            // 
            // справкаToolStripMenuItem
            // 
            this.справкаToolStripMenuItem.Name = "справкаToolStripMenuItem";
            this.справкаToolStripMenuItem.Size = new System.Drawing.Size(127, 22);
            this.справкаToolStripMenuItem.Text = "Справка";
            // 
            // лицензияToolStripMenuItem
            // 
            this.лицензияToolStripMenuItem.Name = "лицензияToolStripMenuItem";
            this.лицензияToolStripMenuItem.Size = new System.Drawing.Size(127, 22);
            this.лицензияToolStripMenuItem.Text = "Лицензия";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.toolStrip1);
            this.groupBox1.Controls.Add(this.employeesTable);
            this.groupBox1.Location = new System.Drawing.Point(0, 27);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(829, 494);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Таблица сотрудников";
            // 
            // toolStrip1
            // 
            this.toolStrip1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(22, 22);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.createNewEmployeeButton,
            this.editJobTitlesButton,
            this.addStatusButton,
            this.addAttributeButton,
            this.refreshButton,
            this.refreshProgressBar,
            this.toolStripSeparator1,
            this.searchTextBox,
            this.toolStripSeparator2,
            this.titleFilter,
            this.statusFilter,
            this.exportButton});
            this.toolStrip1.Location = new System.Drawing.Point(3, 16);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.toolStrip1.Size = new System.Drawing.Size(823, 32);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // createNewEmployeeButton
            // 
            this.createNewEmployeeButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.createNewEmployeeButton.Image = global::ETDBs.Properties.Resources.badge_16dp_273849_FILL0_wght500_GRAD200_opsz20;
            this.createNewEmployeeButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.createNewEmployeeButton.Name = "createNewEmployeeButton";
            this.createNewEmployeeButton.Size = new System.Drawing.Size(26, 29);
            this.createNewEmployeeButton.Text = "Добавить сотрудника";
            this.createNewEmployeeButton.ToolTipText = "Добавить сотрудника";
            // 
            // editJobTitlesButton
            // 
            this.editJobTitlesButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.editJobTitlesButton.Image = global::ETDBs.Properties.Resources.work_16dp_273849_FILL0_wght500_GRAD200_opsz20;
            this.editJobTitlesButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.editJobTitlesButton.Name = "editJobTitlesButton";
            this.editJobTitlesButton.Size = new System.Drawing.Size(26, 29);
            this.editJobTitlesButton.Text = "Редактировать список должностей";
            // 
            // addStatusButton
            // 
            this.addStatusButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.addStatusButton.Image = global::ETDBs.Properties.Resources.domain_16dp_273849_FILL0_wght500_GRAD200_opsz20;
            this.addStatusButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.addStatusButton.Name = "addStatusButton";
            this.addStatusButton.Size = new System.Drawing.Size(26, 29);
            this.addStatusButton.Text = "Добавить статус";
            this.addStatusButton.Visible = false;
            // 
            // addAttributeButton
            // 
            this.addAttributeButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.addAttributeButton.Image = global::ETDBs.Properties.Resources.view_week_16dp_273849_FILL0_wght500_GRAD200_opsz20;
            this.addAttributeButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.addAttributeButton.Name = "addAttributeButton";
            this.addAttributeButton.Size = new System.Drawing.Size(26, 29);
            this.addAttributeButton.Text = "Добавить столбец дополнительных данных";
            // 
            // refreshButton
            // 
            this.refreshButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.refreshButton.Image = global::ETDBs.Properties.Resources.sync_16dp_273849_FILL0_wght500_GRAD200_opsz20;
            this.refreshButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.refreshButton.Name = "refreshButton";
            this.refreshButton.Size = new System.Drawing.Size(26, 29);
            this.refreshButton.Text = "Обновить";
            // 
            // refreshProgressBar
            // 
            this.refreshProgressBar.Name = "refreshProgressBar";
            this.refreshProgressBar.Size = new System.Drawing.Size(100, 29);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 32);
            // 
            // searchTextBox
            // 
            this.searchTextBox.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.searchTextBox.Name = "searchTextBox";
            this.searchTextBox.Size = new System.Drawing.Size(150, 32);
            this.searchTextBox.Text = "Поиск";
            this.searchTextBox.ToolTipText = "Поле поиска";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 32);
            // 
            // titleFilter
            // 
            this.titleFilter.Name = "titleFilter";
            this.titleFilter.Size = new System.Drawing.Size(121, 32);
            this.titleFilter.Text = "Должность - фильтр";
            // 
            // statusFilter
            // 
            this.statusFilter.Name = "statusFilter";
            this.statusFilter.Size = new System.Drawing.Size(121, 32);
            this.statusFilter.Text = "Статус - фильтр";
            // 
            // exportButton
            // 
            this.exportButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.exportButton.Image = global::ETDBs.Properties.Resources.file_export_24dp_273849_FILL0_wght400_GRAD0_opsz24;
            this.exportButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.exportButton.Name = "exportButton";
            this.exportButton.Size = new System.Drawing.Size(26, 29);
            this.exportButton.Text = "Экспорт в Excel";
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.textBox1.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.Location = new System.Drawing.Point(3, 527);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(317, 13);
            this.textBox1.TabIndex = 5;
            this.textBox1.Text = "Базы данных событий v. 1.0.1";
            // 
            // EmployeesManagement
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.ClientSize = new System.Drawing.Size(829, 545);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.menuStrip1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "EmployeesManagement";
            this.Text = "ETDB - Редактирование списка сотрудников";
            ((System.ComponentModel.ISupportInitialize)(this.employeesTable)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView employeesTable;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem tableTool;
        private System.Windows.Forms.ToolStripMenuItem aboutTool;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton createNewEmployeeButton;
        private System.Windows.Forms.ToolStripButton refreshButton;
        private System.Windows.Forms.ToolStripButton editJobTitlesButton;
        private System.Windows.Forms.ToolStripProgressBar refreshProgressBar;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripButton addAttributeButton;
        private System.Windows.Forms.ToolStripButton addStatusButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripTextBox searchTextBox;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem editTool;
        private System.Windows.Forms.ToolStripMenuItem deleteAttribute;
        private System.Windows.Forms.ToolStripMenuItem экспортироватьВExcelТаблицуToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem обновитьToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem добавитьДолжностьToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem добавитьДопПолеToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem справкаToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem лицензияToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem configMenuItem;
        private System.Windows.Forms.ToolStripComboBox titleFilter;
        private System.Windows.Forms.ToolStripComboBox statusFilter;
        private System.Windows.Forms.ToolStripButton exportButton;
        private System.Windows.Forms.TextBox textBox1;
    }
}

