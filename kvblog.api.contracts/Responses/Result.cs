namespace Kvblog.Api.Contracts.Responses;

public class Result<T>
{
    public bool IsSuccess { get; }
    public T? Value { get; }
    public string? Error { get; }
    public ErrorType ErrorType { get; }

    private Result(bool isSuccess, T? value, string? error, ErrorType errorType)
    {
        IsSuccess = isSuccess;
        Value = value;
        Error = error;
        ErrorType = errorType;
    }

    public static Result<T> Success(T value) =>
        new(true, value, null, ErrorType.None);

    public static Result<T> Failure(string error, ErrorType errorType = ErrorType.General) =>
        new(false, default, error, errorType);

    public static Result<T> NotFound(string error) =>
        new(false, default, error, ErrorType.NotFound);

    public static Result<T> ValidationError(string error) =>
        new(false, default, error, ErrorType.Validation);
}

public class Result
{
    public bool IsSuccess { get; }
    public string? Error { get; }
    public ErrorType ErrorType { get; }

    private Result(bool isSuccess, string? error, ErrorType errorType)
    {
        IsSuccess = isSuccess;
        Error = error;
        ErrorType = errorType;
    }

    public static Result Success() =>
        new(true, null, ErrorType.None);

    public static Result Failure(string error, ErrorType errorType = ErrorType.General) =>
        new(false, error, errorType);

    public static Result NotFound(string error) =>
        new(false, error, ErrorType.NotFound);

    public static Result ValidationError(string error) =>
        new(false, error, ErrorType.Validation);
}

public enum ErrorType
{
    None,
    NotFound,
    Validation,
    General
}
