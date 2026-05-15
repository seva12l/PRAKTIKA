namespace EmployeeApp.Models
{
    /// <summary>
    /// Учётная запись пользователя для локальной аутентификации.
    /// Сохраняется в users.json. Пароль хранится в виде SHA-256-хэша.
    /// </summary>
    public class UserModel
    {
        public string Login { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
    }
}
