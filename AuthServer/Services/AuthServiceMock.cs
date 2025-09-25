using AuthServer.Interfaces;

namespace AuthServer.Services;

/// <summary>
/// Заглушка для проверки пользователей
/// </summary>
public class AuthServiceMock : IAuthService
{
    /// <summary>
    /// Пары Логин/Пароль
    /// </summary>
    private readonly Dictionary<string, string> _loginPairs = new()
    {
        { "vasya", "123" },
        { "petya", "456" },
        { "olga", "qwerty" },
        { "maria", "P@ssword" },
    };
    public bool CheckUser(string login, string password)
    {
        return _loginPairs.TryGetValue(login, out var value) && value == password;
    }

    public bool UserExist(IEnumerable<string> logins, out string? notExistLogin)
    {
        foreach (var login in logins)
        {
            if (!_loginPairs.ContainsKey(login))
            {
                notExistLogin = login;
                return false;
            }
        }

        notExistLogin = null;
        return true;
    }
}