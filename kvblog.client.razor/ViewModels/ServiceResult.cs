namespace Kvblog.Client.Razor.ViewModels;

public class ServiceResult<T>
{
    public bool IsSuccess { get; }
    public T? Value { get; }
    public string? ErrorMessage { get; }

    private ServiceResult(bool isSuccess, T? value, string? errorMessage)
    {
        IsSuccess = isSuccess;
        Value = value;
        ErrorMessage = errorMessage;
    }

    public static ServiceResult<T> Success(T value) =>
        new(true, value, null);

    public static ServiceResult<T> Failure(string errorMessage) =>
        new(false, default, errorMessage);
}

public class ServiceResult
{
    public bool IsSuccess { get; }
    public string? ErrorMessage { get; }

    private ServiceResult(bool isSuccess, string? errorMessage)
    {
        IsSuccess = isSuccess;
        ErrorMessage = errorMessage;
    }

    public static ServiceResult Success() =>
        new(true, null);

    public static ServiceResult Failure(string errorMessage) =>
        new(false, errorMessage);
}
