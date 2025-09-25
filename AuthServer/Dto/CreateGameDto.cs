using System.ComponentModel.DataAnnotations;

namespace AuthServer.Dto;

public class CreateGameDto
{
    /// <summary>
    /// Логин
    /// </summary>
    [Required]
    public required string Login { get; set; }
    /// <summary>
    /// Пароль
    /// </summary>
    [Required]
    public required string Password { get; set; }
    
    /// <summary>
    /// Игроки
    /// </summary>
    [Required]
    public required List<string> Players { get; set; } = new();
}