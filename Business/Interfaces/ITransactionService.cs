    public interface ITransactionService
    {

     Task<OperationResult<bool>> TransferToAccountAsync(string senderAccountNumber,string recieverAccountNumber,int amount);
      
     Task<OperationResult<bool>> DepositToAccountAsync(string accountNumber,int amount);

     Task<OperationResult<bool>> WithdrawFromAccountAsync(string accountNumber,int amount);
            
    }

 