namespace EmployeeApp.Models
{
    public class UserModel
    {
        public string Login { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
    }
}
