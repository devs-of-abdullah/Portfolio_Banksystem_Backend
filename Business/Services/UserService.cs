using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;


public class UserService : IUserService
{
    private readonly AppContext _context;
    private readonly IConfiguration _configuration;

    public UserService(AppContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public async Task<OperationResult<User>> AddUserAsync(string fullName, string email, string password)
    {
        try
        {
            email = email.Trim().ToLower();

            if (await _context.users.AnyAsync(u => u.Email == email))
                return OperationResult<User>.Fail($"{email} already exists");

            var user = new User
            {
                FullName = fullName,
                Email = email,
                PasswordHash = PasswordHelper.HashPassword(password)
            };

            _context.users.Add(user);
            await _context.SaveChangesAsync();

            return OperationResult<User>.Ok(user, "User created successfully");
        }
        catch (Exception ex)
        {
            return OperationResult<User>.Fail($"Error creating user: {ex.Message}");
        }
    }

  
    public async Task<OperationResult<string>> DeleteUserAsync(int userId)
    {
        try
        {
            var user = await _context.users.FindAsync(userId);
            if (user == null)
                return OperationResult<string>.Fail("User not found");

            _context.users.Remove(user);
            await _context.SaveChangesAsync();

            return OperationResult<string>.Ok("", "User deleted successfully");
        }
        catch (Exception ex)
        {
            return OperationResult<string>.Fail($"Error deleting user: {ex.Message}");
        }
    }


    public async Task<OperationResult<User>> UpdateUserEmailAsync(int userId, string newEmail)
    {
        try
        {
            newEmail = newEmail.Trim().ToLower();
            var user = await _context.users.FindAsync(userId);

            if (user == null)
                return OperationResult<User>.Fail("User not found");

            bool emailExists = await _context.users.AnyAsync(u => u.Email == newEmail && u.Id != user.Id);
            if (emailExists)
                return OperationResult<User>.Fail($"Email '{newEmail}' already exists");

            user.Email = newEmail;
            await _context.SaveChangesAsync();

            return OperationResult<User>.Ok(user, "Email updated successfully");
        }
        catch (Exception ex)
        {
            return OperationResult<User>.Fail($"Error updating email: {ex.Message}");
        }
    }
    public async Task<OperationResult<int>> GetUserIdByEmail(string email)
    {
        try
        {
          string formatedEmail = email.Trim().ToLower();
           var user =await _context.users.FirstOrDefaultAsync(u => u.Email == formatedEmail);
            if(user == null)
                return OperationResult<int>.Fail("User not found");
            return OperationResult<int>.Ok(user.Id, "id found");

        }
        catch (Exception ex) {
            return OperationResult<int>.Fail($"Error while getting user id by email ({ex.Message}) ");
        }

    }


    public async Task<OperationResult<User>> UpdateUserPasswordAsync(int userId, string newPassword)
    {
        try
        {
            var user = await _context.users.FindAsync(userId);
            if (user == null)
                return OperationResult<User>.Fail("User not found");

            user.PasswordHash = PasswordHelper.HashPassword(newPassword);
            await _context.SaveChangesAsync();

            return OperationResult<User>.Ok(user, "Password updated successfully");
        }
        catch (Exception ex)
        {
            return OperationResult<User>.Fail($"Error updating password: {ex.Message}");
        }
    }

    public async Task<OperationResult<User>> UpdateUserFullNameAsync(int userId, string newFullName)
    {
        try
        {
            var user = await _context.users.FindAsync(userId);
            if (user == null)
                return OperationResult<User>.Fail("User not found");

            user.FullName = newFullName.Trim(); 
            await _context.SaveChangesAsync();

            return OperationResult<User>.Ok(user, "Fullname updated successfully");
        }
        catch (Exception ex)
        {
            return OperationResult<User>.Fail($"Error updating full name: {ex.Message}");
        }
    }
    public async Task<OperationResult<object>> LoginUserAsync(string email, string password)
    {
        try
        {
            email = email.Trim().ToLower();

            var user = await _context.users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
                return OperationResult<object>.Fail("Email not found.");

            bool isPasswordCorrect = PasswordHelper.VerifyPassword(password, user.PasswordHash);
            if (!isPasswordCorrect)
                return OperationResult<object>.Fail("Incorrect password.");

            string token = new JwtService(_configuration).GenerateToken(user);

            var userDto = new
            {   token,
                user.Id,
                user.FullName,
                user.Email,
            };

           

            return OperationResult<object>.Ok(userDto, "Login successful.");
        }
        catch (Exception ex)
        {
            return OperationResult<object>.Fail($"Error during login: {ex.Message}");
        }
    }

    public async Task<OperationResult<List<Account>>> GetUserAccountsAsync(int userId)
    {
        try
        {
            var accounts = await _context.accounts
                .Where(a => a.UserId == userId)
                .ToListAsync();

            if (!accounts.Any())
                return OperationResult<List<Account>>.Fail("No accounts found for this user.");

            return OperationResult<List<Account>>.Ok(accounts, "User accounts retrieved successfully.");
        }
        catch (Exception ex)
        {
            return OperationResult<List<Account>>.Fail(ex.Message);
        }
    }
    public async Task<OperationResult<decimal>> GetUserBalanceAsync(int userId)
    {
        var user = await _context.users
            .Include(u => u.Accounts)
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
            return OperationResult<decimal>.Fail("User not found.");

        if (user.Accounts == null || user.Accounts.Count == 0)
            return OperationResult<decimal>.Fail("User has no accounts.");

        var totalBalance = user.Accounts.Sum(a => a.Balance);

        return OperationResult<decimal>.Ok(totalBalance, "User total balance calculated successfully.");
    }

}
