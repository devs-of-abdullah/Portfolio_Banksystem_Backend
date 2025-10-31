using Microsoft.AspNetCore.Mvc;
public class AccountController:ControllerBase{

   private readonly IAccountService _accountService;
   public AccountController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    [HttpPost("register_account")]
    public async Task<IActionResult> RegisterAccount(int userId, string name)
    {
        if (string.IsNullOrEmpty(name)) return BadRequest("filled can not be empty"); 

        var result = await _accountService.AddAccountAsync(userId, name);

        if (!result.Success) return BadRequest($"something went wrong: {result.Message}");
        return Ok(result.Message);
    }


    [HttpPost("update_account_name")]
    public async Task<IActionResult> UpdateAccountName(int accountId, string newName)
    {
        if (string.IsNullOrEmpty(newName)) return BadRequest("filled can not be empty");
        var result = await _accountService.UpdateAccountNameAsync(accountId, newName);
        if (!result.Success) return BadRequest(result.Message);
        return Ok(result.Message);

    }

    [HttpGet("account_info")]
    public async Task<IActionResult> GetAccountInfo(int accountId)
    {
            var result = await _accountService.GetAccountByIdAsync(accountId);
        if (!result.Success) return BadRequest(result.Message);
        return Ok(result.Data);
    }
    [HttpPatch("activate_account")]
    public async Task<IActionResult> ActivateAccount(int accountId)
    {
        var result = await _accountService.ActivateAccountAsync(accountId);
        if (!result.Success) return BadRequest(result.Message);
        return Ok(result.Message);
    }
    [HttpPatch("deactivate_account")]
    public async Task<IActionResult> DeActivateAccount(int accountId)
    {
        var result = await _accountService.DeActivateAccountAsync(accountId);
        if (!result.Success) return BadRequest(result.Message);
        return Ok(result.Message);
    }
    [HttpPatch("deactivate_all_accounts")]
    public async Task<IActionResult> DeActivateAllAccounts(int userId)
    {
        var result = await _accountService.DeActivateAllAccountsAsync(userId);
        if (!result.Success) return BadRequest(result.Message);
        return Ok(result.Message);
    }
    [HttpPatch("activate_all_accounts")]
    public async Task<IActionResult> ActivateAllAccounts(int userId)
    {
        var result = await _accountService.ActivateAllAccountsAsync(userId);
        if (!result.Success) return BadRequest(result.Message);
        return Ok(result.Message);

    }
    [HttpGet("account_balance")]
    public async Task<IActionResult> GetAccountBalance(string accountNumber)
    {
        var result = await _accountService.GetAccountBalanceAsync(accountNumber);
        if(!result.Success) return BadRequest(result.Message);
        return Ok(result.Data);
    }

}
