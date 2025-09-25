namespace AuthServer.Interfaces;

public interface IGameServerClient
{
    Task<Guid> CreateGameAsync(IEnumerable<string> logins);
    Task<bool> JoinGameAsync(string login, Guid gameId);
}