using Application.DTOs.User;
using Domain.Entities;

namespace Application.Interfaces.Services;

public interface IUserService
{
    Task RegisterAsync(RegisterUserDTO dto);

    Task<User?> AuthenticateAsync(LoginUserDTO dto);
    
    Task<UserDTO?> GetCurrentUserAsync();
    
    Task Update(UpdateUserDTO dto);

    Task Delete();

    Task ChangePasswordAsync(ChangePasswordDTO dto);
}