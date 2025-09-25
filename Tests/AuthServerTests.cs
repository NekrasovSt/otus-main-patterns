using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using AuthServer;
using AuthServer.Dto;
using AuthServer.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace Tests;

public class AuthServerTests
{
    [Fact]
    public async Task CreateGameWrongPassword()
    {
        // Arrange
        await using var application = new WebApplicationFactory<Program>();
        using var client = application.CreateClient();
        var dto = new CreateGameDto()
        {
            Login = "unknown",
            Password = "1234",
            Players = []
        };

        // Act
        var result = await client.PostAsJsonAsync("/createGame", dto);
        
        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, result.StatusCode);
    }

    [Fact]
    public async Task CreateGameNotEnoughPlayers()
    {
        // Arrange
        await using var application = new WebApplicationFactory<Program>();
        using var client = application.CreateClient();
        var dto = new CreateGameDto()
        {
            Login = "vasya",
            Password = "123",
            Players = []
        };

        // Act
        var result = await client.PostAsJsonAsync("/createGame", dto);
        
        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        var content = await result.Content.ReadAsStringAsync();
        Assert.Contains("Should be at least one player", content);
    }
    
    [Fact]
    public async Task CreateGameUnknownPlayer()
    {
        // Arrange
        await using var application = new WebApplicationFactory<Program>();
        using var client = application.CreateClient();
        var dto = new CreateGameDto()
        {
            Login = "vasya",
            Password = "123",
            Players = ["somebody"]
        };

        // Act
        var result = await client.PostAsJsonAsync("/createGame", dto);
        
        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        var content = await result.Content.ReadAsStringAsync();
        Assert.Contains("Unknown user 'somebody'", content);
    }
    
    [Fact]
    public async Task CreateGameOk()
    {
        // Arrange
        await using var application = new WebApplicationFactory<Program>();
        using var client = application.CreateClient();
        var dto = new CreateGameDto()
        {
            Login = "vasya",
            Password = "123",
            Players = ["petya"]
        };

        // Act
        var result = await client.PostAsJsonAsync("/createGame", dto);
        
        // Assert
        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        var content = await result.Content.ReadFromJsonAsync<string>();
        Assert.NotEmpty(content);
        
        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.ReadToken(content) as JwtSecurityToken;
        Assert.Contains(jwt.Claims, i => i.Type == "GameId");
    }

    [Fact]
    public async Task JoinGameWrongLogin()
    {
        // Arrange
        await using var application = new WebApplicationFactory<Program>();
        using var client = application.CreateClient();
        var dto = new JoinGameDto()
        {
            Login = "unknown",
            Password = "123",
            GameId = Guid.NewGuid()
        };
        
        // Act
        var result = await client.PostAsJsonAsync("/joinGame", dto);
        
        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, result.StatusCode);
    }

    [Fact]
    public async Task JoinGameGameNotFound()
    {
        // Arrange
        await using var application = new WebApplicationFactory<Program>();
        using var client = application.CreateClient();
        var dto = new JoinGameDto()
        {
            Login = "petya",
            Password = "456",
            GameId = Guid.NewGuid()
        };
        
        // Act
        var result = await client.PostAsJsonAsync("/joinGame", dto);
        
        // Assert
        Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
    }
    
    [Fact]
    public async Task JoinGameGameOk()
    {
        // Arrange
        var serverMock = new Mock<IGameServerClient>();
        serverMock.Setup(i => i.JoinGameAsync("petya", It.IsAny<Guid>())).ReturnsAsync(true);
        await using var application = new WebApplicationFactory<Program>().WithWebHostBuilder(builder => builder
            .ConfigureServices(services =>
            {
                services.AddScoped<IGameServerClient>((_) => serverMock.Object);
            }));
        using var client = application.CreateClient();
        var dto = new JoinGameDto()
        {
            Login = "petya",
            Password = "456",
            GameId = Guid.NewGuid()
        };
        
        // Act
        var result = await client.PostAsJsonAsync("/joinGame", dto);
        
        // Assert
        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        var content = await result.Content.ReadFromJsonAsync<string>();
        Assert.NotEmpty(content);
        
        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.ReadToken(content) as JwtSecurityToken;
        Assert.Contains(jwt.Claims, i => i.Type == "GameId");
    }

    [Fact]
    public async Task CheckTokenError()
    {
        // Arrange
        await using var application = new WebApplicationFactory<Program>();
        using var client = application.CreateClient();
        var dto = new CheckTokenDto()
        {
            Token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiYWRtaW4iOnRydWUsImlhdCI6MTUxNjIzOTAyMn0.KMUFsIDTnFmyG3nMiGM6H9FNFUROf3wh7SmqJp-QV30"
        };
        // Act
        var result = await client.PostAsJsonAsync("/checkToken", dto);
        
        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, result.StatusCode);
    }
    
    [Fact]
    public async Task CheckTokenOk()
    {
        // Arrange
        await using var application = new WebApplicationFactory<Program>();
        using var client = application.CreateClient();
        var dto = new CheckTokenDto()
        {
            Token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoidmFzeWEiLCJHYW1lSWQiOiIzZWYyOWYyNC1jN2I0LTQ4YmItYTM5MS1mMWQ1MDY0YTFkNjQiLCJleHAiOjE3NTg4MDQ0MTEsImlzcyI6Ik9UVVMiLCJhdWQiOiJPVFVTIGF1ZGllbmNlIn0.FwQt59DZLfULIJ2aLpZEZI9T8jCjrBNV8Y4ATKonN4A"
        };
        // Act
        var result = await client.PostAsJsonAsync("/checkToken", dto);
        
        // Assert
        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
    }
}