using Domain.Entities;

namespace Domain.UnitTest.Entities;

public class UserTests
{
    private readonly User _user = new(
        "Username", 
        "UserEmail@gmail.com", 
        "Password593");
    
    [Fact]
    public void Constructor_ShouldInitializeProperties()
    {
        // Arrange
        var userName = "User";
        var userEmail = "User@gmail.com";
        var passwordHash = "112233445566";

        // Act
        var user = new User(
            userName,
            userEmail,
            passwordHash);
        
        // Assert
        Assert.Equal(userName, user.Username);
        Assert.Equal(userEmail, user.Email);
        Assert.Equal(passwordHash, user.PasswordHash);
    }

    [Fact]
    public void Constructor_ShouldGenerateId()
    {
        // Act
        var user = new User(
            "User",
            "User@gmail.com",
            "Password593");

        // Assert
        Assert.NotEqual(Guid.Empty, user.Id);
    }

    [Fact]
    public void Update_ShouldUpdateUsernameAndEmail()
    {
        // Arrange
        var userName = "NewUser";
        var userEmail = "NewUserEmail@gmail.com";
        
        // Act
        _user.Update(userName, userEmail);
        
        // Assert
        Assert.Equal(userName, _user.Username);
        Assert.Equal(userEmail, _user.Email);
    }

    [Fact]
    public void Update_ShouldUpdateEmail()
    {
        // Arrange
        var originalUserName = _user.Username;
        var userEmail = "NewUserEmail@gmail.com";
        
        // Act
        _user.Update(_user.Username, userEmail);
        
        // Assert
        Assert.Equal(originalUserName, _user.Username);
        Assert.Equal(userEmail, _user.Email);
    }

    [Fact]
    public void Update_ShouldUpdateUserName()
    {
        // Arrange
        var userName = "NewUser";
        var originalEmail = _user.Email;
        
        // Act
        _user.Update(userName, _user.Username);
        
        // Assert
        Assert.Equal(userName, _user.Username);
        Assert.Equal(originalEmail, _user.Email);
    }

    [Theory]
    [InlineData("NewUser", null)]
    [InlineData("NewUser", "")]
    [InlineData("NewUser", " ")]
    [InlineData("NewUser", "\t")]
    [InlineData(null, "NewUserEmail@gmail.com")]
    [InlineData("", "NewUserEmail@gmail.com")]
    [InlineData(" ", "NewUserEmail@gmail.com")]
    [InlineData("\t", "NewUserEmail@gmail.com")]
    public void Update_ShouldThrowArgumentException_WhenNullWhiteSpace(string username, string email)
    {
        // Act
        var action = () => _user.Update(username, email);
        
        // Assert
        Assert.Throws<ArgumentException>(action);
    }

    [Fact]
    public void ChangePassword_ShouldChangePassword()
    {
        // Arrange
        var passwordHash = "593978";
         
        // Act
        _user.ChangePassword(passwordHash);
        
        // Assert
        Assert.Equal(passwordHash, _user.PasswordHash);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("\t")]
    public void ChangePassword_ShouldThrowArgumentException_WhenPasswordHashIsNullOrWhiteSpace(string password)
    {
        // Act
        var action = () => _user.ChangePassword(password);
        
        // Assert
        Assert.Throws<ArgumentException>(action);
    }
}