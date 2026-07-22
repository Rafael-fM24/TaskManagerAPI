namespace Domain.Entities;

public class User
{
    public Guid Id { get; private set; }
    
    public string Username { get; private set; } = string.Empty;
    
    public string Email { get; private set; } = string.Empty;
    
    public string Password { get; private set; } = string.Empty;
    
    public ICollection<TaskItem> Tasks { get; private set; } = new List<TaskItem>();
    
    private User() { }

    public User(string username, string email, string password)
    {
        Id = Guid.NewGuid();
        Username = username;
        Email = email;
        Password = password;
    }
}