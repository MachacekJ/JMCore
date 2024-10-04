using ACore.Base.CQRS.Results.Models;

namespace ACore.Base.CQRS.Results;

public class ExceptionResult : Result
{
  public static readonly ResultErrorItem ResultErrorItemInternalServer = new(
    "InternalServer",
    "Internal server error.");
  
  public Exception Exception { get; }
  private ExceptionResult(Exception exception) : base(false, ResultErrorItemInternalServer)
  {
    Exception = exception;
  }
  public static ExceptionResult WithException(Exception exception) => new(exception);
}

public class ExceptionResult<TValue> : Result<TValue>
{
  public Exception Exception { get; }
  private ExceptionResult(Exception exception)
    : base(default, false, ExceptionResult.ResultErrorItemInternalServer) =>
    Exception = exception;
  
  public static ExceptionResult<TValue> WithException(Exception exception) => new(exception);
}