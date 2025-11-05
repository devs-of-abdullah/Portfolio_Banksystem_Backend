
public interface IUserService
{
    Task<OperationResult<string>> LoginUserAsync(string email, string password);
    Task<OperationResult<User>> AddUserAsync(string fullname, string email, string password);
    Task<OperationResult<string>> DeleteUserAsync(int userId);
    Task<OperationResult<string>> UpdateUserEmailAsync(int userId, string newEmail);
    Task<OperationResult<string>> UpdateUserFullNameAsync(int userId, string newFullname);
    Task<OperationResult<string>> UpdateUserPasswordAsync(int userId, string newPassword);
    Task<OperationResult<List<Account>>> GetUserAccountsAsync(int userId);
    Task<OperationResult<decimal>> GetUserBalanceAsync(int userId);
    Task<OperationResult<int>> GetUserIdByEmail(string email);
}
