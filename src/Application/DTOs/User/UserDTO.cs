namespace Application.DTOs.User;

public class UserDTO
{
    public Guid Id { get; init; }
    
    public string Username { get; init; } = string.Empty;
    
    public string Email { get; init; } = string.Empty;
    
    public string PasswordHash { get; init; } = string.Empty;
}