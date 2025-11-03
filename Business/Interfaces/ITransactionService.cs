    public interface ITransactionService
    {

     Task<OperationResult<bool>> TransferToAccountAsync(string senderAccountNumber,string recieverAccountNumber,decimal amount);
      
     Task<OperationResult<bool>> DepositToAccountAsync(string accountNumber,decimal amount);

     Task<OperationResult<bool>> WithdrawFromAccountAsync(string accountNumber,decimal amount);
            
    }

 