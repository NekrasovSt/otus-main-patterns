namespace AuthServer.Interfaces;

public interface IAuthService
{
    bool CheckUser(string login, string password);
    bool UserExist(IEnumerable<string> logins, out string? notExistLogin);
}