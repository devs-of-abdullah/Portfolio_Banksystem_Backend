using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[ApiController]
[Route("[controller]")] 
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

    [HttpPatch("update_account_name")]
    public async Task<IActionResult> UpdateAccountName([FromBody] UpdateAccountNameRequest request)
    {
        if (string.IsNullOrEmpty(request.NewName))
            return BadRequest("Field 'NewName' cannot be empty.");

        var result = await _accountService.UpdateAccountNameAsync(request.AccountId, request.NewName);
        if (!result.Success) return BadRequest(result.Message);
        return Ok(result.Message);
    }

    [HttpGet("account_info")]
    public async Task<IActionResult> GetAccountInfo([FromQuery] int accountId)
    {
        var result = await _accountService.GetAccountByIdAsync(accountId);
        if (!result.Success) return BadRequest(result.Message);
        return Ok(result.Data);
    }

    [HttpPatch("activate_account")]
    public async Task<IActionResult> ActivateAccount([FromQuery] int accountId)
    {
        var result = await _accountService.ActivateAccountAsync(accountId);
        if (!result.Success) return BadRequest(result.Message);
        return Ok(result.Message);
    }

    [HttpPatch("deactivate_account")]
    public async Task<IActionResult> DeactivateAccount([FromQuery] int accountId)
    {
        var result = await _accountService.DeActivateAccountAsync(accountId);
        if (!result.Success) return BadRequest(result.Message);
        return Ok(result.Message);
    }

    [HttpPatch("activate_all_accounts")]
    public async Task<IActionResult> ActivateAllAccounts([FromQuery] int userId)
    {
        var result = await _accountService.ActivateAllAccountsAsync(userId);
        if (!result.Success) return BadRequest(result.Message);
        return Ok(result.Message);
    }

    [HttpPatch("deactivate_all_accounts")]
    public async Task<IActionResult> DeactivateAllAccounts([FromQuery] int userId)
    {
        var result = await _accountService.DeActivateAllAccountsAsync(userId);
        if (!result.Success) return BadRequest(result.Message);
        return Ok(result.Message);
    }

    [HttpGet("account_balance")]
    public async Task<IActionResult> GetAccountBalance([FromQuery] string accountNumber)
    {
        var result = await _accountService.GetAccountBalanceAsync(accountNumber);
        if (!result.Success) return BadRequest(result.Message);
        return Ok(result.Data);
    }
    
}

