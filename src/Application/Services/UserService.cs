using Application.DTOs.User;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using AutoMapper;
using Domain.Entities;
using Domain.Exceptions;

namespace Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasherService _passwordHasher;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;

    public UserService(IUserRepository userRepository, IPasswordHasherService passwordHasher, IMapper mapper, ICurrentUserService currentUserService)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }
    
    public async Task RegisterAsync(RegisterUserDTO dto)
    {
        var userExists = await _userRepository.GetByEmailAsync(dto.Email);

        if (userExists != null)
            throw new Exception("User already exists");

        var passwordHash = _passwordHasher.Hash(dto.Password);

        var user = new User(
            dto.Username,
            dto.Email,
            passwordHash
        );
        
        _userRepository.Add(user);
        
        await _userRepository.SaveAsync();
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

    public async Task<UserDTO?> GetCurrentUserAsync()
    {
        var userId = _currentUserService.UserId;

        var user = await _userRepository.GetByIdAsync(userId);

        if(user == null)
            throw new NotFoundException("User not found");

        return _mapper.Map<UserDTO>(user);
    }

    public async Task UpdateAsync(UpdateUserDTO dto)
    {
        var userid = _currentUserService.UserId;
        var user = await _userRepository.GetByIdAsync(userid);

        if(user == null)
            throw new NotFoundException("User not found");
        
        user.Update(dto.Username, dto.Email);
        
        await _userRepository.SaveAsync();
    }

    public async Task DeleteAsync()
    {
        var userId = _currentUserService.UserId;

        var user = await _userRepository.GetByIdAsync(userId);
        
        if(user == null)
            throw new NotFoundException("User not found");
        
        _userRepository.RemoveUser(user);
        await _userRepository.SaveAsync();
    }

    public async Task ChangePasswordAsync(ChangePasswordDTO dto)
    {
        var userId = _currentUserService.UserId;

        var user = await _userRepository.GetByIdAsync(userId);

        if (user == null)
            throw new NotFoundException("User not found");

        if (!_passwordHasher.Verify(dto.CurrentPassword, user.PasswordHash))
            throw new Exception("Incorrect current password");

        var newHash = _passwordHasher.Hash(dto.NewPassword);

        user.ChangePassword(newHash);

        await _userRepository.SaveAsync();
    }
}