namespace MiniECommerce.Application.Common;

public class ServiceResult<T>
{
    public bool Success { get; set; }
    public string? Message { get; set; }
    public T? Data { get; set; }

    public static ServiceResult<T> SuccessResult(T data, string? message = null)
    {
        return new ServiceResult<T> { Success = true, Data = data, Message = message };
    }

    public static ServiceResult<T> Failure(string message)
    {
        return new ServiceResult<T> { Success = false, Message = message };
    }
}

public class ServiceResult
{
    public bool Success { get; set; }
    public string? Message { get; set; }

    public static ServiceResult SuccessResult(string? message = null)
    {
        return new ServiceResult { Success = true, Message = message };
    }

    public static ServiceResult Failure(string message)
    {
        return new ServiceResult { Success = false, Message = message };
    }
}
