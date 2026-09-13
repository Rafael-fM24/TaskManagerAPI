using Application.DTOs.User;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Services;
using AutoMapper;
using Domain.Entities;
using Moq;

namespace Application.UnitTest.Services;

public class UserServiceTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock = new ();
    private readonly Mock<IPasswordHasherService> _passwordHasherServiceMock = new ();
    private readonly Mock<IMapper> _mapperMock = new ();
    private readonly Mock<ICurrentUserService> _currentUserServiceMock = new ();
    private readonly List<User> _users;

    public UserServiceTests()
    {
        _users = 
        [
            new User("User1","user1@gmail.com", "593922"),
            new User("User2","user2@gmail.com", "123456")
        ];
    }

    private UserService CreateService()
    {
        return new UserService(
            _userRepositoryMock.Object,
            _passwordHasherServiceMock.Object,
            _mapperMock.Object,
            _currentUserServiceMock.Object);
    }
    
    [Fact]
    public async Task RegisterAsync_ShouldCreateUser()
    {
        // Arrange
        var dto = new RegisterUserDTO
        {
            Username = "User",
            Email = "user@gmail.com",
            Password = "3125534"
        };
        
        _userRepositoryMock
            .Setup(x => x.GetByEmailAsync(dto.Email))
            .ReturnsAsync((User?)null);
        
        _passwordHasherServiceMock
            .Setup(x => x.Hash(dto.Password))
            .Returns(dto.Password);

        var service = CreateService();
        
        // Act
        await service.RegisterAsync(dto);
        
        // Assert
        _userRepositoryMock.Verify(
            x => x.Add(It.Is<User>(user => 
                user.Username == dto.Username &&
                user.Email == dto.Email && 
                user.PasswordHash == dto.Password
            )), 
            Times.Once);
    }

    [Fact]
    public async Task RegisterAsync_ShouldThrow_WhenEmailAlreadyExists()
    {
        // Arrange
        var dto = new RegisterUserDTO
        {
            Username = "User",
            Email = "user1@gmail.com",
            Password = "3125534"
        };
        
        _userRepositoryMock
            .Setup(x => x.GetByEmailAsync(dto.Email))
            .ReturnsAsync(_users[0]);
        
        _passwordHasherServiceMock
            .Setup(x => x.Hash(dto.Password))
            .Returns(dto.Password);
        
        var service = CreateService();
        
        // Act & Arrange
        await Assert.ThrowsAsync<Exception>( () => service.RegisterAsync(dto));

        _userRepositoryMock.Verify(x => x.SaveAsync(), Times.Never);
    }

    [Fact]
    public async Task LoginAsync_ShouldReturnUser_WhenCredentialsAreValid()
    {
        // Arrange
        var dto = new LoginUserDTO()
        {
            Email = _users[0].Email,
            Password = _users[0].PasswordHash
        };
        
        _userRepositoryMock
            .Setup(x => x.GetByEmailAsync(_users[0].Email))
            .ReturnsAsync(_users[0]);

        _passwordHasherServiceMock
            .Setup(x => x.Verify(dto.Password, _users[0].PasswordHash))
            .Returns(true);
        
        var service = CreateService();
        
        // Act
        var result = await service.AuthenticateAsync(dto);

        // Assert
        Assert.Equal(_users[0].Email, result.Email);
        Assert.Equal(_users[0].PasswordHash, result.PasswordHash);
    }

    [Theory]
    [InlineData("user2@gmail.com", "3333333")]
    [InlineData("userFail@gmail.com", "123456")]
    public async Task LoginAsync_ShouldThrow_WhenCredentialsAreInvalid(string email, string password)
    {
        // Arrange
        _userRepositoryMock
            .Setup(x => x.GetByEmailAsync(email))
            .ReturnsAsync((User?)null);
        
        var service = CreateService();

        // Act & Assert
        var result = await service.AuthenticateAsync(
            new LoginUserDTO
            {
                Email = email,
                Password = password
            });

        Assert.Null(result);
    }

    [Fact]
    public async Task GetCurrentUserAsync_ShouldReturnCurrentUser()
    {
        //Arrange
        _currentUserServiceMock
            .Setup(x => x.UserId)
            .Returns(_users[1].Id);
        
        _userRepositoryMock
            .Setup(x => x.GetByIdAsync(_users[1].Id))
            .ReturnsAsync(_users[1]);
        
        _mapperMock
            .Setup(x => x.Map<UserDTO>(_users[1]))
            .Returns(new UserDTO()
            {
                Id = _users[1].Id,
                Username = _users[1].Username,
                Email = _users[1].Email
            });
        
        var service = CreateService();
        
        // Act
        var result = await service.GetCurrentUserAsync();
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal(_users[1].Id, result.Id);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateUser()
    {
        // Arrange
        var dto = new UpdateUserDTO()
        {
            Email = "NewUserEmail@gmail.com",
            Username = "NewUsername"
        };
        
        _currentUserServiceMock
            .Setup(x => x.UserId)
            .Returns(_users[0].Id);

        _userRepositoryMock
            .Setup(x => x.GetByIdAsync(_users[0].Id))
            .ReturnsAsync(_users[0]);

        var service = CreateService();

        // Act
        await service.UpdateAsync(dto);

        // Assert
        Assert.Equal(dto.Email, _users[0].Email);
        Assert.Equal(dto.Username, _users[0].Username);
        
        _userRepositoryMock.Verify(x => x.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task ChangePasswordAsync_ShouldChangePassword()
    {
        // Arrange
        var oldPasswordHash = _users[0].PasswordHash;
        
        var dto = new ChangePasswordDTO
        {
            CurrentPassword = "593922",
            NewPassword = "31288873"
        };

        _currentUserServiceMock
            .Setup(x => x.UserId)
            .Returns(_users[0].Id);

        _userRepositoryMock
            .Setup(x => x.GetByIdAsync(_users[0].Id))
            .ReturnsAsync(_users[0]);

        _passwordHasherServiceMock
            .Setup(x => x.Verify(
                dto.CurrentPassword,
                oldPasswordHash))
            .Returns(true);

        _passwordHasherServiceMock
            .Setup(x => x.Hash(dto.NewPassword))
            .Returns("new-password-hash");

        var service = CreateService();

        // Act
        await service.ChangePasswordAsync(dto);

        // Assert
        _passwordHasherServiceMock.Verify(
            x => x.Verify(
                dto.CurrentPassword,
                oldPasswordHash),
            Times.Once);

        _passwordHasherServiceMock.Verify(
            x => x.Hash(dto.NewPassword),
            Times.Once);

        _userRepositoryMock.Verify(
            x => x.SaveAsync(),
            Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ShouldDeleteUser()
    {
        // Arrange
        _currentUserServiceMock
            .Setup(x => x.UserId)
            .Returns(_users[0].Id);

        _userRepositoryMock
            .Setup(x => x.GetByIdAsync(_users[0].Id))
            .ReturnsAsync(_users[0]);

        var service = CreateService();

        // Act
        await service.DeleteAsync();

        // Assert
        _userRepositoryMock.Verify(
            x => x.RemoveUser(_users[0]), 
            Times.Once);
        
        _userRepositoryMock.Verify(
            x => x.SaveAsync(), 
            Times.Once);
    }
}