public class FundTransferRequest
{
    public string? SenderAccount { get; set; }
    public string? ReceiverAccount { get; set; }
    public decimal Amount { get; set; }
}
public class AccountTransactionRequest
{
    public string? AccountNumber { get; set; }
    public decimal Amount { get; set; }
}
