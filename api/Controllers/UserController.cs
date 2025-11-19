using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]

public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("register_user")]
    public async Task<IActionResult> RegisterUser(string Fullname, string Email, string Password)
    {
        if (string.IsNullOrWhiteSpace(Fullname) || string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
            return BadRequest("All fields are required.");

        var result = await _userService.AddUserAsync(Fullname, Email, Password);
        if (!result.Success) return BadRequest(result.Message);
        return Ok(result.Message);
    }

    [HttpDelete("{userId}/delete_user")]
    public async Task<IActionResult> RemoveUser(int userId)
    {
        if (userId <= 0) return BadRequest("Valid userId is required.");

        var result = await _userService.DeleteUserAsync(userId);
        if (!result.Success) return BadRequest(result.Message);
        return Ok(result.Message);
    }

  

    [HttpPatch("{userId}/email")]
    public async Task<IActionResult> UpdateUserEmail(int userId, [FromBody] UpdateEmailRequest request)
    {
        if (string.IsNullOrEmpty(request.NewEmail))
            return BadRequest("New email cannot be empty.");

        var result = await _userService.UpdateUserEmailAsync(userId, request.NewEmail);
        if (!result.Success) return BadRequest(result.Message); 
        return Ok(result.Data);
    }

  

    [HttpPatch("{userId}/password")]
    public async Task<IActionResult> UpdateUserPassword(int userId, [FromBody] UpdatePasswordRequest request)
    {
        if (string.IsNullOrEmpty(request.NewPassword)) return BadRequest("New password cannot be empty.");

        var result = await _userService.UpdateUserPasswordAsync(userId, request.NewPassword);
        if (!result.Success) return BadRequest(result.Message);
        return Ok(result.Data);
    }

    [HttpPatch("{userId}/fullname")]
    public async Task<IActionResult> UpdateUserFullname(int userId, [FromBody] UpdateFullnameRequest request)
    {
        if (string.IsNullOrEmpty(request.NewFullname))
            return BadRequest("New Fullname cannot be empty.");

        var result = await _userService.UpdateUserFullNameAsync(userId, request.NewFullname);
        if (!result.Success) return BadRequest(result.Message);
        return Ok(result.Data);
    }
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        if (request.Email == null || request.Password == null) return BadRequest("Login fields cannot be empty.");

        var result = await _userService.LoginUserAsync(request.Email, request.Password);
        if (!result.Success) return BadRequest(result.Message);
        return Ok(result.Data);
    }


    [HttpGet("{userId}/accounts")]
    public async Task<IActionResult> GetUserAccounts(int userId)
    {
        var result = await _userService.GetUserAccountsAsync(userId);
        if (!result.Success) return BadRequest(result.Message);
        return Ok(result.Data);
    }

    [HttpGet("{userId}/balance")]
    public async Task<IActionResult> GetUserBalance(int userId)
    {
        var result = await _userService.GetUserBalanceAsync(userId);
        if (!result.Success) return BadRequest(result.Message);
        return Ok(result.Data);
    }
    [HttpGet("{email}/id")]
    public async Task<IActionResult> GetUserIdByEmail(string email)
    {
        var result = await _userService.GetUserIdByEmail(email);
        if (!result.Success) return BadRequest(result.Message);
        return Ok(result.Data);
    }
}

