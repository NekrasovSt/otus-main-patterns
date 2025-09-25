using AuthServer.Interfaces;

namespace AuthServer.Services;

/// <summary>
/// Заглушка для взаимодействия с игровым сервером
/// </summary>
public class GameServerClientMock: IGameServerClient
{
    private static Dictionary<Guid, IEnumerable<string>> _games = new ();
    public Task<Guid> CreateGameAsync(IEnumerable<string> logins)
    {
        var gameId = Guid.NewGuid();
        _games.Add(gameId, logins);
        return Task.FromResult(gameId);
    }

    public Task<bool> JoinGameAsync(string login, Guid gameId)
    {
        if (_games.TryGetValue(gameId, out var players))
        {
            return Task.FromResult(players.Contains(login));
        }

        return Task.FromResult(false);
    }
}