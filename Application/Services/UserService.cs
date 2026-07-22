using Application.DTOs.User;
using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasherService _passwordHasher;

    public UserService(IUserRepository userRepository, IPasswordHasherService passwordHasher)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }
    
    public async Task RegisterAsync(RegisterUserDTO dto)
    {
        var userExists = await _userRepository.GetByEmailAsync(dto.Email);

        if (userExists != null)
            throw new Exception("Usuário já existe.");

        var passwordHash = _passwordHasher.Hash(dto.Password);

        var user = new User(
            dto.Username,
            dto.Email,
            passwordHash
        );

        await _userRepository.AddAsync(user);
    }

    public async Task<User?> AuthenticateAsync(LoginUserDTO dto)
    {
        var user = await _userRepository.GetByEmailAsync(dto.Email);

        if (user == null)
            return null;

        var passwordValid = _passwordHasher.Verify(
            dto.Password,
            user.PasswordHash
        );

        if (!passwordValid)
            return null;

        return user;
    }
}