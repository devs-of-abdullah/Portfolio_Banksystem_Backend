using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
[Authorize]
public class TransactionController : ControllerBase
{
    private readonly ITransactionService _transactionService;

    public TransactionController(ITransactionService transactionService)
    {
        _transactionService = transactionService;
    }

   
    [HttpPost("transfer")]
    public async Task<IActionResult> TransferFunds([FromBody] FundTransferRequest request)
    {
        if (request.Amount <= 0 || string.IsNullOrEmpty(request.SenderAccount) || string.IsNullOrEmpty(request.ReceiverAccount))
            return BadRequest("All fields are required and amount must be greater than 0.");

        var result = await _transactionService.TransferToAccountAsync(
            request.SenderAccount,
            request.ReceiverAccount,
            request.Amount
        );

        if (!result.Success) return BadRequest(result.Message);
        return Ok(result.Data);
    }

  
    [HttpPost("deposit")]
    public async Task<IActionResult> DepositFunds([FromBody] AccountTransactionRequest request)
    {
        if (request.Amount <= 0 || string.IsNullOrEmpty(request.AccountNumber))
            return BadRequest("Account number and amount are required, and amount must be greater than 0.");

        var result = await _transactionService.DepositToAccountAsync(request.AccountNumber, request.Amount);
        if (!result.Success) return BadRequest(result.Message);
        return Ok(result.Message);
    }

    [HttpPost("withdraw")]
    public async Task<IActionResult> WithdrawFunds([FromBody] AccountTransactionRequest request)
    {
        if (request.Amount <= 0 || string.IsNullOrEmpty(request.AccountNumber))
            return BadRequest("Account number and amount are required, and amount must be greater than 0.");

        var result = await _transactionService.WithdrawFromAccountAsync(request.AccountNumber, request.Amount);
        if (!result.Success) return BadRequest(result.Message);
        return Ok(result.Message);
    }
}
