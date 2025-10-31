

public class User
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;

    public List<Account> Accounts { get; set; } = new List<Account>();
}

public class Account
{
    public int Id { get; set; }
    public string AccountNumber { get; set; } = string.Empty;

    public string AccountName {  get; set; } = string.Empty;
    public decimal Balance { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsActive { get; set; } = true;

    public int UserId { get; set; }
    public User User { get; set; } = null!;
    public List<Transaction> Transactions { get; set; } = new List<Transaction>();
}

public class Transaction
{
    public int Id { get; set; }
    public string Type { get; set; } = string.Empty; 
    public decimal Amount { get; set; }
    public DateTime Date { get; set; } = DateTime.UtcNow;

    public int AccountId { get; set; }
    public Account Account { get; set; } = null!;
}

