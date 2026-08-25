namespace Domain.Entities;

public class User
{
    public Guid Id { get; private set; }
    
    public string Username { get; private set; } = string.Empty;
    
    public string Email { get; private set; } = string.Empty;
    
    public string PasswordHash { get; private set; } = string.Empty;
    
    public ICollection<TaskItem> Tasks { get; private set; } = new List<TaskItem>();
    
    private User() { }

    public User(string username, string email, string passwordHash)
    {
        Id = Guid.NewGuid();
        Username = username;
        Email = email;
        PasswordHash = passwordHash;
    }

    public void Update(string username, string email)
    {
        if (string.IsNullOrWhiteSpace(username))
            throw new ArgumentException("O nome é obrigatório.");

        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("O e-mail é obrigatório.");

        Username = username;
        Email = email;
    }
    
    public void ChangePassword(string passwordHash)
    {
        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new ArgumentException("Hash da senha inválido.");

        PasswordHash = passwordHash;
    }
}