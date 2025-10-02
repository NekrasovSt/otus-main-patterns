using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AuthServer.Dto;
using AuthServer.Interfaces;
using AuthServer.Services;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IAuthService, AuthServiceMock>();
builder.Services.AddScoped<IGameServerClient, GameServerClientMock>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/createGame", async (CreateGameDto dto, IAuthService authService, IGameServerClient gameServerClient) =>
    {
        if (!authService.CheckUser(dto.Login, dto.Password))
        {
            return Results.Unauthorized();
        }

        if (dto.Players.Count == 0)
        {
            return Results.BadRequest("Should be at least one player");
        }

        if (!authService.UserExist(dto.Players, out var login))
        {
            return Results.BadRequest($"Unknown user '{login}'");
        }

        var players = dto.Players.Union([dto.Login]).ToList();
        var gameId = await gameServerClient.CreateGameAsync(players);

        var claims = new List<Claim> { new Claim(ClaimTypes.Name, dto.Login), new Claim("GameId", gameId.ToString()) };
        // создаем JWT-токен
        var key =
            "mysupersecret_secretsecretsecretkey!123mysupersecret_secretsecretsecretkey!123mysupersecret_secretsecretsecretkey!123mysupersecret_secretsecretsecretkey!123";

        var jwt = new JwtSecurityToken(
            issuer: "OTUS",
            audience: "OTUS audience",
            claims: claims,
            expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(2)),
            signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                SecurityAlgorithms.HmacSha256));
        var token = new JwtSecurityTokenHandler().WriteToken(jwt);
        return Results.Ok(token);
    })
    .WithName("CreateGame")
    .WithOpenApi();
app.MapPost("/joinGame",
    async (JoinGameDto dto, IAuthService authService, IGameServerClient gameServerClient) =>
    {
        if (!authService.CheckUser(dto.Login, dto.Password))
        {
            return Results.Unauthorized();
        }

        if (!await gameServerClient.JoinGameAsync(dto.Login, dto.GameId))
        {
            return Results.NotFound("Game not found or player isn't joined");
        }

        // создаем JWT-токен
        var key =
            "mysupersecret_secretsecretsecretkey!123mysupersecret_secretsecretsecretkey!123mysupersecret_secretsecretsecretkey!123mysupersecret_secretsecretsecretkey!123";
        var claims = new List<Claim>
            { new Claim(ClaimTypes.Name, dto.Login), new Claim("GameId", dto.GameId.ToString()) };
        var jwt = new JwtSecurityToken(
            issuer: "OTUS",
            audience: "OTUS audience",
            claims: claims,
            expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(2)),
            signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                SecurityAlgorithms.HmacSha256));
        var token = new JwtSecurityTokenHandler().WriteToken(jwt);
        return Results.Ok(token);
    }).WithOpenApi();
app.MapPost("checkToken", (CheckTokenDto dto) =>
{
    var key =
        "mysupersecret_secretsecretsecretkey!123mysupersecret_secretsecretsecretkey!123mysupersecret_secretsecretsecretkey!123mysupersecret_secretsecretsecretkey!123";
    var tokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        ValidAudience = "OTUS audience",
        ValidIssuer = "OTUS",
        ValidateLifetime = false,

        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
    };
    var handler = new JwtSecurityTokenHandler();
    try
    {
        var claimsPrincipal = handler.ValidateToken(dto.Token, tokenValidationParameters, out _);
    }
    catch (SecurityTokenSignatureKeyNotFoundException e)
    {
        return Results.Unauthorized();
    }

    return Results.Ok();
});
app.Run();

public partial class Program
{
}