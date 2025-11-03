using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

public class TransactionService : ITransactionService
{
    private readonly AppContext _context;

    public TransactionService(AppContext context)
    {
        _context = context;
    }

    public async Task<OperationResult<bool>> TransferToAccountAsync(string senderAccountNumber, string receiverAccountNumber, decimal amount)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var senderAccount = await _context.Accounts.FirstOrDefaultAsync(a => a.AccountNumber == senderAccountNumber);
            var receiverAccount = await _context.Accounts.FirstOrDefaultAsync(a => a.AccountNumber == receiverAccountNumber);

            if (senderAccount == null || receiverAccount == null)
                return OperationResult<bool>.Fail("Reciever account was not found.");

            if (amount <= 0)
                return OperationResult<bool>.Fail("Amount must be greater than 0.");

            if (amount > senderAccount.Balance)
                return OperationResult<bool>.Fail("Insufficient balance.");

            senderAccount.Balance -= amount;
            receiverAccount.Balance += amount;

            var senderTransaction = new Transaction { Type = "Send", AccountId = senderAccount.Id, Amount = amount };
            var receiverTransaction = new Transaction { Type = "Receive", AccountId = receiverAccount.Id, Amount = amount };

            await _context.Transactions.AddRangeAsync(senderTransaction, receiverTransaction);

            await _context.SaveChangesAsync();

            await transaction.CommitAsync();

            return OperationResult<bool>.Ok(true, "Transfer completed successfully");
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            return OperationResult<bool>.Fail("Transaction failed: " + ex.Message);
        }
    }

    public async Task<OperationResult<bool>> DepositToAccountAsync(string accountNumber, decimal amount)
    {
        try
        {
            var account = await _context.Accounts.FirstOrDefaultAsync(a => a.AccountNumber == accountNumber);

            if (account == null)
                return OperationResult<bool>.Fail("Incorrect account number.");

            if (amount <= 0)
                return OperationResult<bool>.Fail("Amount must be greater than 0.");

            if (amount > 3000)
                return OperationResult<bool>.Fail("Amount must be smaller than or equal to 3000.");

            account.Balance += amount;

            var transaction = new Transaction()
            {
                Amount = amount,
                AccountId = account.Id,
                Type = "Deposit",
            };

            await _context.Transactions.AddAsync(transaction);
            await _context.SaveChangesAsync();

            return OperationResult<bool>.Ok(true, $"{amount} deposited to account {accountNumber}");
        }
        catch (Exception ex)
        {
            return OperationResult<bool>.Fail(ex.Message);
        }
    }

    public async Task<OperationResult<bool>> WithdrawFromAccountAsync(string accountNumber, decimal amount)
    {
        try
        {
            var account = await _context.Accounts.FirstOrDefaultAsync(a => a.AccountNumber == accountNumber);

            if (account == null)
                return OperationResult<bool>.Fail("Incorrect account number.");

            if (amount <= 0)
                return OperationResult<bool>.Fail("Amount must be greater than 0.");

            if (amount > account.Balance)
                return OperationResult<bool>.Fail("Insufficient balance.");

            account.Balance -= amount;

            var transaction = new Transaction()
            {
                Amount = amount,
                AccountId = account.Id,
                Type = "Withdraw",
            };

            await _context.Transactions.AddAsync(transaction);
            await _context.SaveChangesAsync();

            return OperationResult<bool>.Ok(true, $"{amount} withdrawn from account {accountNumber}");
        }
        catch (Exception ex)
        {
            return OperationResult<bool>.Fail(ex.Message);
        }
    }
}
