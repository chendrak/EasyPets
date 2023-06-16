using System;

namespace EasyPets.EasyPets;

public abstract class Result
{
    public bool Success { get; protected set; }
    public bool IsFailed => !Success;
}

public abstract class Result<T> : Result
{
    private T _data;

    protected Result(T data)
    {
        Data = data;
    }

    public T Data
    {
        get => Success
            ? _data
            : throw new Exception($"You can't access .{nameof(Data)} when .{nameof(Success)} is false");
        set => _data = value;
    }

    public virtual string ErrorMessage => throw new Exception("No ErrorMessage available");
}

public class SuccessResult<T> : Result<T>
{
    public SuccessResult(T data) : base(data)
    {
        Success = true;
    }
}

public class ErrorResult<T> : Result<T>
{
    public ErrorResult(string message) : base(default)
    {
        ErrorMessage = message;
        Success = false;
    }

    public override string ErrorMessage { get; }
}

public static class Results
{
    public static ErrorResult<T> Fail<T>(string message)
    {
        return new ErrorResult<T>(message);
    }

    public static SuccessResult<T> Ok<T>(T value)
    {
        return new SuccessResult<T>(value);
    }
}