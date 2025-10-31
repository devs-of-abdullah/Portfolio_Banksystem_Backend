
    public interface IAccountService
    {
        Task<OperationResult<Account>> AddAccountAsync(int userId, string Name);
        Task<OperationResult<string>> UpdateAccountNameAsync(int accountId,string newName);
        Task<OperationResult<Account>> GetAccountByIdAsync(int accountId);
        Task<OperationResult<string>> ActivateAccountAsync(int accountId); 
        Task<OperationResult<string>> DeActivateAccountAsync(int accountId);
        Task<OperationResult<string>> ActivateAllAccountsAsync(int accountId);
        Task<OperationResult<string>> DeActivateAllAccountsAsync(int accountId);
        Task<OperationResult<List<Account>>> GetAllUserAccountsAsync(int userId);
        Task<OperationResult<decimal>> GetAccountBalanceAsync(string accountNumber);

    }

