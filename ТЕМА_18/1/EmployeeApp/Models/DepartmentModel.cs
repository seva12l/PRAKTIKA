namespace EmployeeApp.Models
{
    public class DepartmentModel
    {
        public string Name { get; set; } = string.Empty;

        public override string ToString() => Name;
    }
}
