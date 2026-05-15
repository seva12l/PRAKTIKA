using System.ComponentModel;
using System.Text.Json.Serialization;

namespace EmployeeApp.Models
{
    public class EmployeeModel : INotifyPropertyChanged
    {
        private string fullName = string.Empty;
        private string position = string.Empty;
        private string department = string.Empty;
        private decimal salary;
        private bool isAvailable = true;

        public string FullName
        {
            get => fullName;
            set { fullName = value; OnPropertyChanged(nameof(FullName)); OnPropertyChanged(nameof(Initials)); }
        }

        public string Position
        {
            get => position;
            set { position = value; OnPropertyChanged(nameof(Position)); }
        }

        public string Department
        {
            get => department;
            set { department = value; OnPropertyChanged(nameof(Department)); }
        }

        public decimal Salary
        {
            get => salary;
            set { salary = value; OnPropertyChanged(nameof(Salary)); }
        }

        public bool IsAvailable
        {
            get => isAvailable;
            set
            {
                isAvailable = value;
                OnPropertyChanged(nameof(IsAvailable));
                OnPropertyChanged(nameof(StatusText));
            }
        }

        [JsonIgnore]
        public string StatusText => IsAvailable ? "Доступен" : "Отсутствует";

        [JsonIgnore]
        public string Initials
        {
            get
            {
                if (string.IsNullOrWhiteSpace(fullName)) return "?";
                var parts = fullName.Trim().Split(' ', System.StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length == 0) return "?";
                if (parts.Length == 1) return parts[0].Substring(0, 1).ToUpper();
                return (parts[0].Substring(0, 1) + parts[1].Substring(0, 1)).ToUpper();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
