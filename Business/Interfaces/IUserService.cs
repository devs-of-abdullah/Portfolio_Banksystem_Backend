public interface IUserService
{
    Task<OperationResult<User>> AddUserAsync(string fullname, string email, string password);
    Task<OperationResult<string>> DeleteUserAsync(int userId);
    Task<OperationResult<string>> UpdateUserEmailAsync(int userId, string newEmail);
    Task<OperationResult<string>> UpdateUserFullNameAsync(int userId, string newFullName);
    Task<OperationResult<string>> UpdateUserPasswordAsync(int userId, string newPassword);
    Task<OperationResult<string>> LoginUserAsync(string email, string password);

    Task<OperationResult<List<Account>>> GetUserAccountsAsync(int userId);
}
