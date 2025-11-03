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

    [HttpDelete("delete_user")]
    public async Task<IActionResult> RemoveUser(int userId)
    {
        if (userId <= 0) return BadRequest("Valid userId is required.");

        var result = await _userService.DeleteUserAsync(userId);
        if (!result.Success) return BadRequest(result.Message);
        return Ok(result.Message);
    }

    [HttpPatch("update_user_email")]
    public async Task<IActionResult> UpdateUserEmail(int UserId, string NewEmail)
    {
        if (string.IsNullOrEmpty(NewEmail)) return BadRequest("NewEmail cannot be empty.");

        var result = await _userService.UpdateUserEmailAsync(UserId, NewEmail);
        if (!result.Success) return BadRequest(result.Message);
        return Ok(result.Message);
    }

    [HttpPatch("update_user_fullname")]
    public async Task<IActionResult> UpdateUserFullname(int UserId, string NewFullname)
    {
        if (string.IsNullOrEmpty(NewFullname)) return BadRequest("NewFullname cannot be empty.");

        var result = await _userService.UpdateUserFullNameAsync(UserId, NewFullname);
        if (!result.Success) return BadRequest(result.Message);
        return Ok(result.Message);
    }

    [HttpPatch("update_user_password")]
    public async Task<IActionResult> UpdateUserPassword(int UserId, string NewPassword)
    {
        if (string.IsNullOrEmpty(NewPassword)) return BadRequest("NewPassword cannot be empty.");

        var result = await _userService.UpdateUserPasswordAsync(UserId, NewPassword);
        if (!result.Success) return BadRequest(result.Message);
        return Ok(result.Message);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
            return BadRequest("Email and Password are required.");

        var result = await _userService.LoginUserAsync(request.Email, request.Password);
        if (!result.Success) return BadRequest(result.Message);

        return Ok(new
        {
            token = result.Data,
            message = result.Message
        });
    }

    [Authorize]
    [HttpGet("{userId}/accounts")]
    public async Task<IActionResult> GetUserAccounts(int userId)
    {
        var result = await _userService.GetUserAccountsAsync(userId);
        if (!result.Success) return BadRequest(result.Message);
        return Ok(result.Data);
    }

    [Authorize]
    [HttpGet("{userId}/balance")]
    public async Task<IActionResult> GetUserBalance(int userId)
    {
        var result = await _userService.GetUserBalanceAsync(userId);
        if (!result.Success) return BadRequest(result.Message);
        return Ok(result.Data);
    }
}

