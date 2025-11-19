using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
[Authorize]
public class AccountController : ControllerBase
{
    private readonly IAccountService _accountService;

    public AccountController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    [HttpPost("register_account")]
    public async Task<IActionResult> RegisterAccount([FromBody] RegisterAccountRequest request)
    {
        if (string.IsNullOrEmpty(request.Name))
            return BadRequest("Field 'Name' cannot be empty.");

        var result = await _accountService.AddAccountAsync(request.UserId, request.Name);

        if (!result.Success) return BadRequest($"Something went wrong: {result.Message}");
        return Ok(result.Message);
    }

  
    [HttpGet("{userId}/user_accounts")]
    public async Task<IActionResult> GetUserAccounts(int userId)
    {
        var result = await _accountService.GetAllUserAccountsAsync(userId);
        if (!result.Success) return BadRequest(result.Message);
        return Ok(result.Data);
    }

    [HttpGet("{userId}/remove_account/{accountNumber}")]
    public async Task<IActionResult> RemoveAccount(int userId, string accountNumber)
    {
        var result = await _accountService.RemoveAccountAsync(userId,accountNumber);
        if (!result.Success) return BadRequest(result.Message);
        return Ok(result.Data);
    }
}

