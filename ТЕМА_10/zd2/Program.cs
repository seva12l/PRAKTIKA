using System;

AuthenticationService service = new AuthenticationService();

service.SetStrategy(new BasicAuth());
service.Authenticate("admin", "12345");

service.SetStrategy(new OAuthAuth());
service.Authenticate("user@mail.com", "oauth_token_xyz");

service.SetStrategy(new JWTAuth());
service.Authenticate("user", "eyJhbGciOiJIUzI1NiJ9.token.signature");


interface IAuthStrategy
{
    bool Authenticate(string login, string secret);
}

class BasicAuth : IAuthStrategy
{
    public bool Authenticate(string login, string password)
    {
        Console.WriteLine("Basic: проверка логина " + login + " и пароля");
        return login == "admin" && password == "12345";
    }
}

class OAuthAuth : IAuthStrategy
{
    public bool Authenticate(string login, string token)
    {
        Console.WriteLine("OAuth: проверка токена для " + login);
        return token.StartsWith("oauth_");
    }
}

class JWTAuth : IAuthStrategy
{
    public bool Authenticate(string login, string jwt)
    {
        Console.WriteLine("JWT: проверка токена для " + login);
        return jwt.Split('.').Length == 3;
    }
}

class AuthenticationService
{
    private IAuthStrategy strategy;

    public void SetStrategy(IAuthStrategy strategy)
    {
        this.strategy = strategy;
    }

    public void Authenticate(string login, string secret)
    {
        if (strategy == null)
        {
            Console.WriteLine("Стратегия не выбрана");
            return;
        }
        bool result = strategy.Authenticate(login, secret);
        Console.WriteLine(result ? "Успешно" : "Отказано");
        Console.WriteLine();
    }
}
