using Microsoft.EntityFrameworkCore;

public class UserService
{
    private readonly AppContext _context;
    public UserService(AppContext context) { _context = context; }
    public async Task<OperationResult> AddUserAsync(string Fullname, string Email, string Password)
    {
        try
        {
           Email = Email.Trim().ToLower();
            if (await _context.Users.AnyAsync(u => u.Email == Email))
                return OperationResult.Fail($"{Email} already exists");

            var user = new User
            {
                FullName = Fullname,
                Email = Email,
                PasswordHash = PasswordHelper.HashPassword(Password)
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return OperationResult.Ok("User Created Successfully");
        }
        catch (Exception ex)
        {
            return OperationResult.Fail($"Something went wrong while creating the user: {ex.Message}");
        }
    }
    public async Task<OperationResult> DeleteUserAsync(string Email, string Password)
    {

        try
        {
            Email = Email.Trim().ToLower();

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == Email);

            if (user == null) return OperationResult.Fail("Email does not exists");

            if (!PasswordHelper.VerifyPassword(Password, user.PasswordHash)) return OperationResult.Fail("Password is incorrect");

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return OperationResult.Ok($"User with email {Email} deleted succefully");

        }
        catch (Exception ex)
        {
            return OperationResult.Fail($"Something went wrong: {ex.Message}");
        }
    }

    public async Task<OperationResult> UpdateUserEmailAsync(int userId, string newEmail)
    {
        try
        {
            newEmail = newEmail.Trim().ToLower();

            var user = await _context.Users.FindAsync(userId);

            if (user == null) return OperationResult.Fail("User not found");

            bool emailExists = await _context.Users.AnyAsync(u => u.Email == newEmail && u.Id != user.Id);

            if (emailExists)  return OperationResult.Fail($"Email '{newEmail}' is already exists");

            user.Email = newEmail;

            await _context.SaveChangesAsync();

            return OperationResult.Ok("Email updated successfully");
        }
        catch (Exception ex)
        {
            return OperationResult.Fail($"Something went wrong: {ex.Message}");
        }
    }
    public async Task<OperationResult> UpdateUserFullNameAsync(int userId, string newFullName)
    {
        try
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return OperationResult.Fail("User not found");

            user.FullName = newFullName;
            await _context.SaveChangesAsync();

            return OperationResult.Ok("Full name updated successfully");
        }
        catch (Exception ex)
        {
            return OperationResult.Fail($"Something went wrong: {ex.Message}");
        }
    }
    public async Task<OperationResult> UpdateUserPasswordAsync(int userId, string newPassword)
    {
        try
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return OperationResult.Fail("User not found");

            user.PasswordHash = PasswordHelper.HashPassword(newPassword);
            await _context.SaveChangesAsync();

            return OperationResult.Ok("Password updated successfully");
        }
        catch (Exception ex)
        {
            return OperationResult.Fail($"Something went wrong: {ex.Message}");
        }
    }

    public async Task<OperationResult> LoginUserAsync(string Email, string Password)
    {
        try
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == Email);

            if (user == null)
                return OperationResult.Fail("Email not found");

            bool isPasswordCorrect = PasswordHelper.VerifyPassword(Password, user.PasswordHash);

            if (!isPasswordCorrect)
                return OperationResult.Fail("Incorrect password");

            return OperationResult.Ok("Login successful");
        }
        catch (Exception ex)
        {
            return OperationResult.Fail($"Something went wrong: {ex.Message}");
        }
    }

}


