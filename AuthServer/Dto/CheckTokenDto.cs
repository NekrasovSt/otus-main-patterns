using System.ComponentModel.DataAnnotations;

namespace AuthServer.Dto;

public class CheckTokenDto
{
    /// <summary>
    /// Токен
    /// </summary>
    [Required]
    public required string Token { get; set; }
}