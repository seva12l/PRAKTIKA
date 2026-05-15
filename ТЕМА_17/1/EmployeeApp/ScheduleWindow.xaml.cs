using System.Windows;

namespace EmployeeApp
{
    public partial class ScheduleWindow : Window
    {
        public string NotificationText { get; private set; } = string.Empty;

        public ScheduleWindow()
        {
            InitializeComponent();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            NotificationText = TextField.Text.Trim();
            DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
