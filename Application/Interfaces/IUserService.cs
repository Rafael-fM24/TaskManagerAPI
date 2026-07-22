using Application.DTOs.User;
using Domain.Entities;

namespace Application.Interfaces;

public interface IUserService
{
    Task RegisterAsync(RegisterUserDTO dto);

    Task<User?> AuthenticateAsync(LoginUserDTO dto);
}