namespace ETDBs
{
    partial class EventsManagement
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EventsManagement));
            this.employeesList = new System.Windows.Forms.ListBox();
            this.employeeEventsTable = new System.Windows.Forms.DataGridView();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.titleEventsTable = new System.Windows.Forms.DataGridView();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.refreshButton = new System.Windows.Forms.ToolStripButton();
            this.refreshBar = new System.Windows.Forms.ToolStripProgressBar();
            this.addEmployeeEventButton = new System.Windows.Forms.ToolStripButton();
            this.editTitleEventsButton = new System.Windows.Forms.ToolStripButton();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.таблицыToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.обновитьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.правкаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.configMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.добавитьЛичноеСобытиеToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.редактироватьДолжностныеСобытияToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.оПрограммеToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.оПрограммеToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.лицензияToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.readMeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.viewEmployeeMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.aboutTool = new System.Windows.Forms.ToolStripMenuItem();
            this.viewAllTool = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.employeeEventsTable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.titleEventsTable)).BeginInit();
            this.groupBox4.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.viewEmployeeMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // employeesList
            // 
            this.employeesList.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.employeesList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.employeesList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.employeesList.FormattingEnabled = true;
            this.employeesList.Items.AddRange(new object[] {
            "Сотрудник 1",
            "Сотрудник 2"});
            this.employeesList.Location = new System.Drawing.Point(3, 16);
            this.employeesList.Name = "employeesList";
            this.employeesList.Size = new System.Drawing.Size(123, 357);
            this.employeesList.TabIndex = 0;
            // 
            // employeeEventsTable
            // 
            this.employeeEventsTable.BackgroundColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.employeeEventsTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.employeeEventsTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.employeeEventsTable.Location = new System.Drawing.Point(3, 16);
            this.employeeEventsTable.Name = "employeeEventsTable";
            this.employeeEventsTable.Size = new System.Drawing.Size(634, 153);
            this.employeeEventsTable.TabIndex = 1;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(0, 27);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.groupBox1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.groupBox2);
            this.splitContainer1.Size = new System.Drawing.Size(779, 376);
            this.splitContainer1.SplitterDistance = 129;
            this.splitContainer1.TabIndex = 2;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.employeesList);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(129, 376);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Сотрудники";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.splitContainer2);
            this.groupBox2.Controls.Add(this.toolStrip1);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(646, 376);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "События";
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(3, 45);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.groupBox3);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.groupBox4);
            this.splitContainer2.Size = new System.Drawing.Size(640, 328);
            this.splitContainer2.SplitterDistance = 152;
            this.splitContainer2.TabIndex = 1;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.titleEventsTable);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Location = new System.Drawing.Point(0, 0);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(640, 152);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "События по должности";
            // 
            // titleEventsTable
            // 
            this.titleEventsTable.BackgroundColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.titleEventsTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.titleEventsTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.titleEventsTable.Location = new System.Drawing.Point(3, 16);
            this.titleEventsTable.Name = "titleEventsTable";
            this.titleEventsTable.Size = new System.Drawing.Size(634, 133);
            this.titleEventsTable.TabIndex = 2;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.employeeEventsTable);
            this.groupBox4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox4.Location = new System.Drawing.Point(0, 0);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(640, 172);
            this.groupBox4.TabIndex = 2;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "События сотрудника";
            // 
            // toolStrip1
            // 
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(22, 22);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.refreshButton,
            this.refreshBar,
            this.addEmployeeEventButton,
            this.editTitleEventsButton});
            this.toolStrip1.Location = new System.Drawing.Point(3, 16);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(640, 29);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // refreshButton
            // 
            this.refreshButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.refreshButton.Image = global::ETDBs.Properties.Resources.sync_16dp_273849_FILL0_wght500_GRAD200_opsz20;
            this.refreshButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.refreshButton.Name = "refreshButton";
            this.refreshButton.Size = new System.Drawing.Size(26, 26);
            this.refreshButton.Text = "Обновить";
            // 
            // refreshBar
            // 
            this.refreshBar.Name = "refreshBar";
            this.refreshBar.Size = new System.Drawing.Size(100, 26);
            // 
            // addEmployeeEventButton
            // 
            this.addEmployeeEventButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.addEmployeeEventButton.Image = global::ETDBs.Properties.Resources.event_24dp_273849_FILL0_wght400_GRAD200_opsz24;
            this.addEmployeeEventButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.addEmployeeEventButton.Name = "addEmployeeEventButton";
            this.addEmployeeEventButton.Size = new System.Drawing.Size(26, 26);
            this.addEmployeeEventButton.Text = "Добавить событие сотрудника";
            // 
            // editTitleEventsButton
            // 
            this.editTitleEventsButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.editTitleEventsButton.Image = global::ETDBs.Properties.Resources.receipt_long_24dp_273849_FILL0_wght400_GRAD200_opsz24;
            this.editTitleEventsButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.editTitleEventsButton.Name = "editTitleEventsButton";
            this.editTitleEventsButton.Size = new System.Drawing.Size(26, 26);
            this.editTitleEventsButton.Text = "toolStripButton3";
            this.editTitleEventsButton.ToolTipText = "Редактирование должостных событий";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.таблицыToolStripMenuItem,
            this.правкаToolStripMenuItem,
            this.оПрограммеToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(779, 24);
            this.menuStrip1.TabIndex = 3;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // таблицыToolStripMenuItem
            // 
            this.таблицыToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.обновитьToolStripMenuItem});
            this.таблицыToolStripMenuItem.Name = "таблицыToolStripMenuItem";
            this.таблицыToolStripMenuItem.Size = new System.Drawing.Size(68, 20);
            this.таблицыToolStripMenuItem.Text = "Таблицы";
            // 
            // обновитьToolStripMenuItem
            // 
            this.обновитьToolStripMenuItem.Name = "обновитьToolStripMenuItem";
            this.обновитьToolStripMenuItem.Size = new System.Drawing.Size(128, 22);
            this.обновитьToolStripMenuItem.Text = "Обновить";
            // 
            // правкаToolStripMenuItem
            // 
            this.правкаToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.configMenuItem,
            this.добавитьЛичноеСобытиеToolStripMenuItem,
            this.редактироватьДолжностныеСобытияToolStripMenuItem});
            this.правкаToolStripMenuItem.Name = "правкаToolStripMenuItem";
            this.правкаToolStripMenuItem.Size = new System.Drawing.Size(59, 20);
            this.правкаToolStripMenuItem.Text = "Правка";
            // 
            // configMenuItem
            // 
            this.configMenuItem.Name = "configMenuItem";
            this.configMenuItem.Size = new System.Drawing.Size(283, 22);
            this.configMenuItem.Text = "Вернуться к конфигурации";
            // 
            // добавитьЛичноеСобытиеToolStripMenuItem
            // 
            this.добавитьЛичноеСобытиеToolStripMenuItem.Name = "добавитьЛичноеСобытиеToolStripMenuItem";
            this.добавитьЛичноеСобытиеToolStripMenuItem.Size = new System.Drawing.Size(283, 22);
            this.добавитьЛичноеСобытиеToolStripMenuItem.Text = "Добавить личное событие";
            // 
            // редактироватьДолжностныеСобытияToolStripMenuItem
            // 
            this.редактироватьДолжностныеСобытияToolStripMenuItem.Name = "редактироватьДолжностныеСобытияToolStripMenuItem";
            this.редактироватьДолжностныеСобытияToolStripMenuItem.Size = new System.Drawing.Size(283, 22);
            this.редактироватьДолжностныеСобытияToolStripMenuItem.Text = "Редактировать должностные события";
            // 
            // оПрограммеToolStripMenuItem
            // 
            this.оПрограммеToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.оПрограммеToolStripMenuItem1,
            this.лицензияToolStripMenuItem,
            this.readMeToolStripMenuItem});
            this.оПрограммеToolStripMenuItem.Name = "оПрограммеToolStripMenuItem";
            this.оПрограммеToolStripMenuItem.Size = new System.Drawing.Size(94, 20);
            this.оПрограммеToolStripMenuItem.Text = "О программе";
            // 
            // оПрограммеToolStripMenuItem1
            // 
            this.оПрограммеToolStripMenuItem1.Name = "оПрограммеToolStripMenuItem1";
            this.оПрограммеToolStripMenuItem1.Size = new System.Drawing.Size(127, 22);
            this.оПрограммеToolStripMenuItem1.Text = "Справка";
            // 
            // лицензияToolStripMenuItem
            // 
            this.лицензияToolStripMenuItem.Name = "лицензияToolStripMenuItem";
            this.лицензияToolStripMenuItem.Size = new System.Drawing.Size(127, 22);
            this.лицензияToolStripMenuItem.Text = "Лицензия";
            // 
            // readMeToolStripMenuItem
            // 
            this.readMeToolStripMenuItem.Name = "readMeToolStripMenuItem";
            this.readMeToolStripMenuItem.Size = new System.Drawing.Size(127, 22);
            this.readMeToolStripMenuItem.Text = "ReadMe";
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.textBox1.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.Location = new System.Drawing.Point(3, 406);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(317, 13);
            this.textBox1.TabIndex = 4;
            this.textBox1.Text = "Базы данных событий v. 0.4.12";
            // 
            // viewEmployeeMenuStrip
            // 
            this.viewEmployeeMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutTool,
            this.viewAllTool});
            this.viewEmployeeMenuStrip.Name = "viewEmployeeMenuStrip";
            this.viewEmployeeMenuStrip.Size = new System.Drawing.Size(232, 48);
            // 
            // aboutTool
            // 
            this.aboutTool.Name = "aboutTool";
            this.aboutTool.Size = new System.Drawing.Size(231, 22);
            this.aboutTool.Text = "Подробнее";
            // 
            // viewAllTool
            // 
            this.viewAllTool.Name = "viewAllTool";
            this.viewAllTool.Size = new System.Drawing.Size(231, 22);
            this.viewAllTool.Text = "Просмотр всех сотрудников";
            // 
            // EventsManagement
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.ClientSize = new System.Drawing.Size(779, 427);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "EventsManagement";
            this.Text = "ETDB - Планировщик событий";
            ((System.ComponentModel.ISupportInitialize)(this.employeeEventsTable)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.titleEventsTable)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.viewEmployeeMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox employeesList;
        private System.Windows.Forms.DataGridView employeeEventsTable;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.DataGridView titleEventsTable;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.ToolStripButton refreshButton;
        private System.Windows.Forms.ToolStripProgressBar refreshBar;
        private System.Windows.Forms.ToolStripButton addEmployeeEventButton;
        private System.Windows.Forms.ToolStripButton editTitleEventsButton;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem таблицыToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem правкаToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem оПрограммеToolStripMenuItem;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.ToolStripMenuItem configMenuItem;
        private System.Windows.Forms.ContextMenuStrip viewEmployeeMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem aboutTool;
        private System.Windows.Forms.ToolStripMenuItem viewAllTool;
        private System.Windows.Forms.ToolStripMenuItem оПрограммеToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem лицензияToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem readMeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem добавитьЛичноеСобытиеToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem редактироватьДолжностныеСобытияToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem обновитьToolStripMenuItem;
    }
}