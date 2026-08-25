using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.User;

public class UpdateUserDTO
{
    
    public string Username { get; init; } = string.Empty;
   
    [EmailAddress]
    public string Email { get; init; } = string.Empty; 
}