public class OperationResult<T>
{
    public bool Success { get; set; }
    public T? Data { get; set; }
    public string? Message { get; set; }

    public static OperationResult<T> Ok(T data,string message) => new() { Success = true,Data = data, Message = message };
    public static OperationResult<T> Fail(string message) => new() { Success = false, Message = message };
}