using Microsoft.AspNetCore.Mvc;
public class TransactionController: ControllerBase{

    private readonly ITransactionService _transactionService;

    public TransactionController(ITransactionService transactionService)
    {
        _transactionService = transactionService;
    }

    [HttpPost("transfer")]
    public async Task<IActionResult> TransferToAccount(string senderAccountNumber,string recieverAccountNumber, int amount)
    {
        var result = await _transactionService.TransferToAccountAsync(senderAccountNumber, recieverAccountNumber, amount);   
        if(!result.Success) return BadRequest(result.Message);
        return Ok(result.Message);
    }
    [HttpPost("deposit")]
    public async Task<IActionResult> DepositToAccount(string accountNumber, int amount)
    {
        var result = await _transactionService.DepositToAccountAsync(accountNumber,amount);
        if (!result.Success) return BadRequest(result.Message);
        return Ok(result.Message);
    }
    [HttpPost("withdraw")]
    public async Task<IActionResult> WithdrawFromAccount(string accountNumber, int amount)
    {
        var result = await _transactionService.WithdrawFromAccountAsync(accountNumber, amount);
        if (!result.Success) return BadRequest(result.Message);
        return Ok(result.Message);
    }



}

