using Microsoft.EntityFrameworkCore;

public class AccountService :IAccountService{
   private readonly AppContext _context;
   public AccountService(AppContext context){
        _context = context;
   }

   public async Task<OperationResult<Account>> AddAccountAsync(int userId, string Name)
    {
        if (string.IsNullOrEmpty(Name)) return OperationResult<Account>.Fail("Name field is required");

          var user = await _context.Users.FindAsync(userId);

        if (user == null) return OperationResult<Account>.Fail($"{userId} Not exists!!");

          var existing = await _context.Accounts
            .FirstOrDefaultAsync(a => a.UserId == userId && a.AccountName == Name && a.IsActive);

        if (existing != null)
            return OperationResult<Account>.Fail("This user already has an account with the same name.");

        Account account = new Account()
        {
            UserId = userId,
            Balance = 0m,
            AccountName = Name,
            AccountNumber = $"I{userId}N{Guid.NewGuid().ToString("N")[..10].ToUpper()}",
            CreatedAt = DateTime.UtcNow,
        };
     

        _context.Accounts.Add(account);
        await _context.SaveChangesAsync();

        return OperationResult<Account>.Ok(account, "Account Added succefully");
    }
   public async Task<OperationResult<string>> UpdateAccountNameAsync(int accountId, string newName)
    {
        if (string.IsNullOrEmpty(newName)) return OperationResult<string>.Fail("Name field is required");
        
        var account = await _context.Accounts.FindAsync(accountId);

        if (account == null) return OperationResult<string>.Fail("Account id does not exist");

        var duplicate = await _context.Accounts.AnyAsync(a=> a.UserId == account.UserId && a.AccountName == newName && a.Id != accountId && a.IsActive );
        if (duplicate)
            return OperationResult<string>.Fail("This account name already exists for this user.");

        account.AccountName = newName;
        await _context.SaveChangesAsync();
        return OperationResult<string>.Ok(newName, "Account Name changed Succesfully");
    }
  
    public async Task<OperationResult<Account>> GetAccountByIdAsync(int accountId)
    {
        var account = await _context.Accounts.FindAsync(accountId);
        if(account == null ) return OperationResult<Account>.Fail($"Account {accountId} does not exist");

        return OperationResult<Account>.Ok(account, "Account details");

    }
    public async Task<OperationResult<string>> ActivateAccountAsync(int accountId)
    {
        var account = await _context.Accounts.FindAsync(accountId);
        if (account == null) return OperationResult<string>.Fail($"Account {accountId} does not exist");

        if (account.IsActive)
            return OperationResult<string>.Fail("Account is already active.");

        account.IsActive = true;
        await _context.SaveChangesAsync(); 

        return OperationResult<string>.Ok("", "Account activated successfully.");
    }

    public async Task<OperationResult<string>> DeActivateAccountAsync(int accountId)
    {
        var account = await _context.Accounts.FindAsync(accountId);
        if (account == null) return OperationResult<string>.Fail($"Account {accountId} does not exist");

        if (!account.IsActive)
            return OperationResult<string>.Fail("Account is already deactivated.");

        account.IsActive = false;
        await _context.SaveChangesAsync(); 

        return OperationResult<string>.Ok("", "Account deactivated successfully.");
    }


    public async Task<OperationResult<string>> ActivateAllAccountsAsync(int userId)
    {
     
        var user = await _context.Users.FindAsync(userId);
        if (user == null)
            return OperationResult<string>.Fail("User not found");

        var accounts = await _context.Accounts
            .Where(a => a.UserId == userId && !a.IsActive)
            .ToListAsync();

        if (!accounts.Any())
            return OperationResult<string>.Fail("No inactive accounts found for this user.");

        foreach (var account in accounts)
        {
            account.IsActive = true;
        }

        await _context.SaveChangesAsync();

        return OperationResult<string>.Ok($"{accounts.Count} accounts activated successfully.",
            "All accounts activated successfully");
    }

    public async Task<OperationResult<string>> DeActivateAllAccountsAsync(int userId)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user == null)
            return OperationResult<string>.Fail("User not found");

        var accounts = await _context.Accounts
            .Where(a => a.UserId == userId && a.IsActive)
            .ToListAsync();

        if (!accounts.Any())
            return OperationResult<string>.Fail("No active accounts found for this user.");

        foreach (var account in accounts)
        {
            account.IsActive = false;
        }

        await _context.SaveChangesAsync();

        return OperationResult<string>.Ok($"{accounts.Count} accounts deactivated successfully.",
            "All accounts deactivated successfully");
    }
   public async Task<OperationResult<List<Account>>> GetAllUserAccountsAsync(int userId)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user == null)
            return OperationResult<List<Account>>.Fail("User not found");

        var accounts = await _context.Accounts
            .Where(a => a.UserId == userId && a.IsActive)
            .ToListAsync();
        return OperationResult<List<Account>>.Ok(accounts, "Accounts");
    }

}

