using System;

/*
 * Задание 3.
 * Реализуем паттерн Издатель-Подписчик с пользовательским делегатом.
 * UserLoginManager - издатель, генерирует событие UserLoggedIn.
 * SecuritySystem и NotificationService - подписчики на это событие.
 * Когда пользователь входит - оба подписчика получают уведомление.
*/

delegate void LoginHandler(string username);

class UserLoginManager
{
    public event LoginHandler UserLoggedIn;

    public void Login(string username)
    {
        Console.WriteLine("Пользователь " + username + " входит в систему...");
        if (UserLoggedIn != null)
            UserLoggedIn(username);
    }
}

class SecuritySystem
{
    public void CheckAccess(string username)
    {
        Console.WriteLine("SecuritySystem: проверка доступа для " + username + " - доступ разрешен");
    }
}

class NotificationService
{
    public void SendNotification(string username)
    {
        Console.WriteLine("NotificationService: уведомление отправлено для " + username);
    }
}

class Program
{
    static void Main()
    {
        UserLoginManager manager = new UserLoginManager();
        SecuritySystem security = new SecuritySystem();
        NotificationService notification = new NotificationService();

        manager.UserLoggedIn += security.CheckAccess;
        manager.UserLoggedIn += notification.SendNotification;

        manager.Login("Иванов");
        Console.WriteLine();
        manager.Login("Петров");

        Console.ReadKey();
    }
}