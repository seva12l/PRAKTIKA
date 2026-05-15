using System.ComponentModel;

public class Employee : INotifyPropertyChanged
{
    private string fullName = string.Empty;
    private string position = string.Empty;
    private decimal salary;

    public string FullName
    {
        get => fullName;
        set { fullName = value; OnPropertyChanged(nameof(FullName)); }
    }

    public string Position
    {
        get => position;
        set { position = value; OnPropertyChanged(nameof(Position)); }
    }

    public decimal Salary
    {
        get => salary;
        set { salary = value; OnPropertyChanged(nameof(Salary)); }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
