public class Account
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string? AccountNumber { get; set; }
    public decimal Balance { get; set; }
}
public class RegisterAccountRequest
{
    public int UserId { get; set; }
    public string? Name { get; set; }
}

public class UpdateAccountNameRequest
{
    public int AccountId { get; set; }
    public string? NewName { get; set; }
}
