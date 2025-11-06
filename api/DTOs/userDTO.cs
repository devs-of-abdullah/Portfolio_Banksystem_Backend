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
public class UpdateFullnameRequest
{
    public string NewFullname { get; set; } = string.Empty;
}
public class UpdatePasswordRequest
{
    public string NewPassword { get; set; } = string.Empty;
}
public class UpdateEmailRequest
{
    public string NewEmail { get; set; } = string.Empty;
}