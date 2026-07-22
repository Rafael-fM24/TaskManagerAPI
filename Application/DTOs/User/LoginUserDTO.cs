using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.User;

public class LoginUserDTO
{
    [Required]
    [EmailAddress]
    public string Email { get; init; } = string.Empty;
    
    [Required]
    public string Password { get; init; } = string.Empty;
}