namespace ACore.Base.CQRS.Models;

public class Result
{
  public Guid Id { get; private set; }
  public bool IsSuccess { get; }
  public bool IsFailure => !IsSuccess;
  public Error Error { get; }

  protected Result(bool isSuccess, Error error)
  {
    Id = Guid.NewGuid();
    switch (isSuccess)
    {
      case true when error != Error.None:
        throw new InvalidOperationException();
      case false when error == Error.None:
        throw new InvalidOperationException();
      default:
        IsSuccess = isSuccess;
        Error = error;
        break;
    }
  }

  public static Result Success() => new(true, Error.None);
  public static Result<TValue> Success<TValue>(TValue value) => new(value, true, Error.None);
  
  public static Result Failure(Error error) => new(false, error);
  public static Result<TValue> Failure<TValue>(Error error) => new(default, false, error);
}

public class Result<TValue> : Result
{
  private readonly TValue? _value;

  protected internal Result(TValue? value, bool isSuccess, Error error)
    : base(isSuccess, error) =>
    _value = value;

  public TValue? ResultValue => IsSuccess
    ? _value
    : default;
  
  public new static Result<TValue> Failure(Error error) => new(default, false, error);
}