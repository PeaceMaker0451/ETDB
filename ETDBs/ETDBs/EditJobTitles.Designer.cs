namespace ETDBs
{
    partial class EditJobTitles
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
            this.jobTitlesTable = new System.Windows.Forms.DataGridView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.addNewJobTitleButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.jobTitlesTable)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // jobTitlesTable
            // 
            this.jobTitlesTable.BackgroundColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.jobTitlesTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.jobTitlesTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.jobTitlesTable.Location = new System.Drawing.Point(3, 16);
            this.jobTitlesTable.Name = "jobTitlesTable";
            this.jobTitlesTable.Size = new System.Drawing.Size(569, 495);
            this.jobTitlesTable.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.jobTitlesTable);
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(575, 514);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Список должностей";
            // 
            // addNewJobTitleButton
            // 
            this.addNewJobTitleButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.addNewJobTitleButton.Location = new System.Drawing.Point(3, 520);
            this.addNewJobTitleButton.Name = "addNewJobTitleButton";
            this.addNewJobTitleButton.Size = new System.Drawing.Size(569, 23);
            this.addNewJobTitleButton.TabIndex = 2;
            this.addNewJobTitleButton.Text = "Добавить";
            this.addNewJobTitleButton.UseVisualStyleBackColor = true;
            // 
            // EditJobTitles
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.ClientSize = new System.Drawing.Size(577, 551);
            this.Controls.Add(this.addNewJobTitleButton);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.Name = "EditJobTitles";
            this.Text = "Должности";
            ((System.ComponentModel.ISupportInitialize)(this.jobTitlesTable)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView jobTitlesTable;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button addNewJobTitleButton;
    }
}