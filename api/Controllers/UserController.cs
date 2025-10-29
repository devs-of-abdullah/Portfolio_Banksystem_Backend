
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    public UserController(IUserService userService)
    {
        _userService = userService;
    }
    [HttpPost("register_user")]
    public async Task<IActionResult> RegisterUser([FromBody] RegisterUserDto request)
    {
        if (string.IsNullOrWhiteSpace(request.FullName) || string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
        {
            return BadRequest("All fields are required.");
        }
        var result = await _userService.AddUserAsync(request.FullName, request.Email, request.Password);

        if (!result.Success)  return BadRequest(result.Message);
        return Ok(result.Message);
    }
    [HttpDelete("delete_user")]
    public async Task<IActionResult> RemoveUser(int userId)
    {
        if (  string.IsNullOrWhiteSpace(userId.ToString())) return BadRequest("All fields are required.");
        
            var result = await _userService.DeleteUserAsync( userId);
        if (!result.Success) return BadRequest(result.Message);
        
        return Ok(result.Message);
    }
    [HttpPatch("update_user_email")]
    public async Task<IActionResult> UpdateUserEmail(int UserId, string NewEmail)
    {
        if (string.IsNullOrEmpty(NewEmail)) return BadRequest("field can not be empty");  

        var result = await _userService.UpdateUserEmailAsync(UserId, NewEmail);
        if (!result.Success) return BadRequest(result.Message);
        return Ok(result.Message);
    }
    [HttpPatch("update_user_fullname")]
    public async Task<IActionResult> UpdateUserFullname(int UserId, string NewFullname)
    {
        if (string.IsNullOrEmpty(NewFullname)) return BadRequest("field can not be empty");

        var result = await _userService.UpdateUserFullNameAsync(UserId, NewFullname);
        if (!result.Success) return BadRequest(result.Message);
        return Ok(result.Message);
    }
    [HttpPatch("update_user_password")]
    public async Task<IActionResult> UpdateUserPassword(int UserId, string NewPassword)
    {
        if (string.IsNullOrEmpty(NewPassword)) return BadRequest("field can not be empty");

        var result = await _userService.UpdateUserFullNameAsync(UserId, NewPassword);
        if (!result.Success) return BadRequest(result.Message);
        return Ok(result.Message);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(string Email, string Password)
    {
        if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password))
            return BadRequest("Email and Password are required");

        var result = await _userService.LoginUserAsync(Email, Password);

        if (!result.Success)
            return BadRequest(result.Message);

        return Ok(new
        {
            Token = result.Data,
            Message = result.Message
        });
    }

    [HttpGet("{userId}/accounts")]
    public async Task<IActionResult> GetUserAccounts(int userId)
    {
        var result = await _userService.GetUserAccountsAsync(userId);

        if (!result.Success)
            return BadRequest(result.Message);
        return Ok(result.Data);
    }






}

