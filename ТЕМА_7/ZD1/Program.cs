using System;

/*
 * Задание 1.
 * Создаем пользовательское исключение AdminDeletionException.
 * Класс UserManager имеет метод DeleteUser, который проверяет роль пользователя.
 * Если роль "Admin" - выбрасываем наше исключение.
 * В Main обрабатываем исключение через try-catch.
*/

class AdminDeletionException : Exception
{
    public AdminDeletionException() : base("Нельзя удалить администратора!") { }

    public AdminDeletionException(string message) : base(message) { }

    public AdminDeletionException(string message, Exception innerException) : base(message, innerException) { }
}

class UserManager
{
    public void DeleteUser(string role)
    {
        if (role == "Admin")
            throw new AdminDeletionException("Попытка удалить пользователя с ролью Admin запрещена!");

        Console.WriteLine("Пользователь с ролью " + role + " успешно удален");
    }
}

class Program
{
    static void Main()
    {
        UserManager manager = new UserManager();

        try
        {
            manager.DeleteUser("User");
            manager.DeleteUser("Admin");
        }
        catch (AdminDeletionException ex)
        {
            Console.WriteLine("Ошибка: " + ex.Message);
        }

        Console.ReadKey();
    }
}