using ACore.Base.CQRS.Results.Models;

namespace ACore.Base.CQRS.Results;

public class Result
{
  public Guid Id { get; private set; }
  public bool IsSuccess { get; }
  public bool IsFailure => !IsSuccess;
  public ResultErrorItem ResultErrorItem { get; }

  protected Result(bool isSuccess, ResultErrorItem resultErrorItem)
  {
    Id = Guid.NewGuid();
    switch (isSuccess)
    {
      case true when resultErrorItem != ResultErrorItem.None:
        throw new InvalidOperationException();
      case false when resultErrorItem == ResultErrorItem.None:
        throw new InvalidOperationException();
      default:
        IsSuccess = isSuccess;
        ResultErrorItem = resultErrorItem;
        break;
    }
  }

  public static Result Success() => new(true, ResultErrorItem.None);
  public static Result<TValue> Success<TValue>(TValue value) => new(value, true, ResultErrorItem.None);
  
  public static Result Failure(ResultErrorItem resultErrorItem) => new(false, resultErrorItem);
  public static Result<TValue> Failure<TValue>(ResultErrorItem resultErrorItem) => new(default, false, resultErrorItem);
}

public class Result<TValue> : Result
{
  private readonly TValue? _value;

  protected internal Result(TValue? value, bool isSuccess, ResultErrorItem resultErrorItem)
    : base(isSuccess, resultErrorItem) =>
    _value = value;

  public TValue? ResultValue => IsSuccess
    ? _value
    : default;
  
  public new static Result<TValue> Failure(ResultErrorItem resultErrorItem) => new(default, false, resultErrorItem);
}