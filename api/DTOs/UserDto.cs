
public class RegisterUserDto
{
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
public class RemoveUserDto
{
    public string Email {  set; get; } = string.Empty;
    public string Password { get; set; } = string.Empty ;
}

