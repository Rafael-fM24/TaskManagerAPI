using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.User;

public class RegisterUserDTO
{
    [Required]
    public string Username { get; init; } = string.Empty;
    
    [Required]
    [EmailAddress]
    public string Email { get; init; } = string.Empty;
    
    [Required]
    public string Password { get; init; } = string.Empty;
}