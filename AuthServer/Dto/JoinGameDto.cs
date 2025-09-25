using System.ComponentModel.DataAnnotations;

namespace AuthServer.Dto;

public class JoinGameDto
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
    /// Ид игры
    /// </summary>
    [Required]
    public required Guid GameId { get; set; }
}