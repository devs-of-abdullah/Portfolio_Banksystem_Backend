using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

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

            if (await _context.Users.AnyAsync(u => u.Email == email))
                return OperationResult<User>.Fail($"{email} already exists");

            var user = new User
            {
                FullName = fullName,
                Email = email,
                PasswordHash = PasswordHelper.HashPassword(password)
            };

            _context.Users.Add(user);
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
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return OperationResult<string>.Fail("User not found");

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return OperationResult<string>.Ok("", "User deleted successfully");
        }
        catch (Exception ex)
        {
            return OperationResult<string>.Fail($"Error deleting user: {ex.Message}");
        }
    }


    public async Task<OperationResult<string>> UpdateUserEmailAsync(int userId, string newEmail)
    {
        try
        {
            newEmail = newEmail.Trim().ToLower();
            var user = await _context.Users.FindAsync(userId);

            if (user == null)
                return OperationResult<string>.Fail("User not found");

            bool emailExists = await _context.Users.AnyAsync(u => u.Email == newEmail && u.Id != user.Id);
            if (emailExists)
                return OperationResult<string>.Fail($"Email '{newEmail}' already exists");

            user.Email = newEmail;
            await _context.SaveChangesAsync();

            return OperationResult<string>.Ok("", "Email updated successfully");
        }
        catch (Exception ex)
        {
            return OperationResult<string>.Fail($"Error updating email: {ex.Message}");
        }
    }

 
    public async Task<OperationResult<string>> UpdateUserFullNameAsync(int userId, string newFullName)
    {
        try
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return OperationResult<string>.Fail("User not found");

            user.FullName = newFullName;
            await _context.SaveChangesAsync();

            return OperationResult<string>.Ok("", "Full name updated successfully");
        }
        catch (Exception ex)
        {
            return OperationResult<string>.Fail($"Error updating full name: {ex.Message}");
        }
    }

    public async Task<OperationResult<string>> UpdateUserPasswordAsync(int userId, string newPassword)
    {
        try
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return OperationResult<string>.Fail("User not found");

            user.PasswordHash = PasswordHelper.HashPassword(newPassword);
            await _context.SaveChangesAsync();

            return OperationResult<string>.Ok("", "Password updated successfully");
        }
        catch (Exception ex)
        {
            return OperationResult<string>.Fail($"Error updating password: {ex.Message}");
        }
    }

 
    public async Task<OperationResult<string>> LoginUserAsync(string email, string password)
    {
        try
        {
            email = email.Trim().ToLower();

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
                return OperationResult<string>.Fail("Email not found");

            bool isPasswordCorrect = PasswordHelper.VerifyPassword(password, user.PasswordHash);
            if (!isPasswordCorrect)
                return OperationResult<string>.Fail("Incorrect password");

            string token = new JwtService(_configuration).GenerateToken(user);

            return OperationResult<string>.Ok(token, "Login successful");
        }
        catch (Exception ex)
        {
            return OperationResult<string>.Fail($"Error during login: {ex.Message}");
        }
    }

    
}
