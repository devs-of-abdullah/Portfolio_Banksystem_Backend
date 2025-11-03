public class LoginRequest
{
    public string? Email { get; set; }
    public string? Password { get; set; }
}

public class User
{
    public int Id { get; set; }
    public string? Fullname { get; set; }
    public string? Email { get; set; }
    public string? PasswordHash { get; set; }  
}
