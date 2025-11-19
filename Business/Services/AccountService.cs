using Microsoft.EntityFrameworkCore;

public class AccountService :IAccountService{
   private readonly AppContext _context;
   public AccountService(AppContext context){
        _context = context;
   }

    public async Task<OperationResult<Account>> AddAccountAsync(int userId, string name)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(name))
                return OperationResult<Account>.Fail("Account name is required.");

            var user = await _context.users.FindAsync(userId);
            if (user == null)
                return OperationResult<Account>.Fail($"User with ID {userId} does not exist.");

            bool exists = await _context.accounts
                .AnyAsync(a => a.UserId == userId && a.AccountName.ToLower() == name.ToLower() && a.IsActive);

            if (exists)
                return OperationResult<Account>.Fail("This user already has an active account with the same name.");

            var account = new Account
            {
                UserId = userId,
                Balance = 0m,
                AccountName = name.Trim(),
                AccountNumber = $"I{userId}N{Guid.NewGuid().ToString("N")[..10].ToUpper()}",
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };

            _context.accounts.Add(account);
            await _context.SaveChangesAsync();

            return OperationResult<Account>.Ok(account, "Account added successfully.");
        } 
        catch(Exception ex) {return OperationResult<Account>.Fail(ex.Message);}

    }

    public async Task<OperationResult<bool>> RemoveAccountAsync(int userId, string accountNumber)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(accountNumber))
                return OperationResult<bool>.Fail("Account number is required.");

            var account = await _context.accounts
                .FirstOrDefaultAsync(a => a.AccountNumber == accountNumber);

            if (account == null)
                return OperationResult<bool>.Fail("Account number does not exist.");

            if (account.UserId == userId)
            {
                _context.accounts.Remove(account);
                await _context.SaveChangesAsync();
                return OperationResult<bool>.Ok(true, "Account deleted successfully.");
            }
            else
            {
                return OperationResult<bool>.Fail("You don’t have access to delete this account.");
            }
        }
        catch (Exception ex)
        {
            return OperationResult<bool>.Fail($"Error: {ex.Message}");
        }
    }

    public async Task<OperationResult<string>> UpdateAccountNameAsync(int accountId, string newName)
    {
        try
        {
            if (string.IsNullOrEmpty(newName)) return OperationResult<string>.Fail("Name field is required");

            var account = await _context.accounts.FindAsync(accountId);

            if (account == null) return OperationResult<string>.Fail("Account id does not exist");

            var duplicate = await _context.accounts.AnyAsync(a => a.UserId == account.UserId && a.AccountName == newName && a.Id != accountId && a.IsActive);
            if (duplicate)
                return OperationResult<string>.Fail("This account name already exists for this user.");

            account.AccountName = newName;
            await _context.SaveChangesAsync();
            return OperationResult<string>.Ok(newName, "Account Name changed Succesfully");
        }
                catch(Exception ex) {return OperationResult<string>.Fail(ex.Message);}

    }
  
    public async Task<OperationResult<Account>> GetAccountByIdAsync(int accountId)
    {
        try
        {
             var account = await _context.accounts.FindAsync(accountId);
                    if(account == null ) return OperationResult<Account>.Fail($"Account {accountId} does not exist");

                    return OperationResult<Account>.Ok(account, "Account details");
        }
        catch (Exception ex) { return OperationResult<Account>.Fail(ex.Message); }



    }
    public async Task<OperationResult<string>> ActivateAccountAsync(int accountId)
    {
        try
        {
           var account = await _context.accounts.FindAsync(accountId);
                if (account == null) return OperationResult<string>.Fail($"Account {accountId} does not exist");

                if (account.IsActive)
                    return OperationResult<string>.Fail("Account is already active.");

                account.IsActive = true;
                await _context.SaveChangesAsync(); 

                return OperationResult<string>.Ok("", "Account activated successfully.");
        }
        catch (Exception ex) { return OperationResult<string>.Fail(ex.Message); }


    }

    public async Task<OperationResult<string>> DeActivateAccountAsync(int accountId)
    {
        try
        {
         var account = await _context.accounts.FindAsync(accountId);
                if (account == null) return OperationResult<string>.Fail($"Account {accountId} does not exist");

                if (!account.IsActive)
                    return OperationResult<string>.Fail("Account is already deactivated.");

                account.IsActive = false;
                await _context.SaveChangesAsync(); 

                return OperationResult<string>.Ok("", "Account deactivated successfully.");
        }        catch(Exception ex) {return OperationResult<string>.Fail(ex.Message);}

       
    }


    public async Task<OperationResult<string>> ActivateAllAccountsAsync(int userId)
    {
        try {      
        var user = await _context.users.FindAsync(userId);
        if (user == null)
            return OperationResult<string>.Fail("User not found");

        var accounts = await _context.accounts
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
        catch (Exception ex) { return OperationResult<string>.Fail(ex.Message); }

    }

    public async Task<OperationResult<string>> DeActivateAllAccountsAsync(int userId)
    {
        try
        {
            var user = await _context.users.FindAsync(userId);
            if (user == null)
                return OperationResult<string>.Fail("User not found");

            var accounts = await _context.accounts
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
        catch(Exception ex) {return OperationResult<string>.Fail(ex.Message);}

    }
   public async Task<OperationResult<List<Account>>> GetAllUserAccountsAsync(int userId)
    {
        try
        {
            var user = await _context.users.FindAsync(userId);
            if (user == null)
                return OperationResult<List<Account>>.Fail("User not found");

            var accounts = await _context.accounts
                .Where(a => a.UserId == userId && a.IsActive)
                .ToListAsync();
            return OperationResult<List<Account>>.Ok(accounts, "Accounts");
        }   
        catch(Exception ex) {return OperationResult<List<Account>>.Fail(ex.Message);}


    }

    public async Task<OperationResult<decimal>> GetAccountBalanceAsync(string accountNumber)
    {
        try
        {
            var account = await _context.accounts.FirstOrDefaultAsync(a => a.AccountNumber == accountNumber);

            if (account == null) return OperationResult<decimal>.Fail("Account not found");

            var balance = account.Balance;

            return OperationResult<decimal>.Ok(balance, "Account balance returned");
        }catch(Exception ex) {return OperationResult<decimal>.Fail(ex.Message);}
    }
   


}

