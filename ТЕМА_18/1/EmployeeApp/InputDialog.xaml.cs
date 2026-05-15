using System.Windows;

namespace EmployeeApp
{
    public partial class InputDialog : Window
    {
        public string Result { get; private set; } = string.Empty;

        public InputDialog(string title, string prompt)
        {
            InitializeComponent();
            Title = title;
            PromptText.Text = prompt;
            Loaded += (_, _) => InputBox.Focus();
        }

        private void OkClick(object sender, RoutedEventArgs e)
        {
            Result = InputBox.Text;
            DialogResult = true;
            Close();
        }

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
