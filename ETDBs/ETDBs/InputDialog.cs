using System;
using System.Windows.Forms;

public class InputDialog : Form
{
    private TextBox inputTextBox;
    public string InputValue { get; private set; }

    public InputDialog(string title)
    {
        InitializeComponent();
        // Настройка заголовка
        this.Text = title;
        this.Width = 300;
        this.Height = 150;
        this.StartPosition = FormStartPosition.CenterScreen;

        // Поле ввода
        inputTextBox = new TextBox() { Left = 20, Top = 20, Width = 240 };
        this.Controls.Add(inputTextBox);

        // Кнопка "ОК"
        Button okButton = new Button() { Text = "OK", Left = 50, Width = 80, Top = 60, DialogResult = DialogResult.OK };
        okButton.Click += (sender, e) => { InputValue = inputTextBox.Text; this.Close(); };
        this.Controls.Add(okButton);

        // Кнопка "Отмена"
        Button cancelButton = new Button() { Text = "Cancel", Left = 150, Width = 80, Top = 60, DialogResult = DialogResult.Cancel };
        cancelButton.Click += (sender, e) => { this.Close(); };
        this.Controls.Add(cancelButton);

        this.AcceptButton = okButton;
        this.CancelButton = cancelButton;
    }

    public static string ShowDialog(string title)
    {
        using (var dialog = new InputDialog(title))
        {
            return dialog.ShowDialog() == DialogResult.OK ? dialog.InputValue : null;
        }
    }

    private void InitializeComponent()
    {
            this.SuspendLayout();
            // 
            // InputDialog
            // 
            this.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "InputDialog";
            this.ResumeLayout(false);

    }
}