namespace ACore.Base.CQRS.Models;

public class ExceptionResult : Result
{
  public Exception Exception { get; }
  private ExceptionResult(Exception exception) : base(false, ErrorTypes.ErrorInternalServer)
  {
    Exception = exception;
  }
  public static ExceptionResult WithException(Exception exception) => new(exception);
}

public class ExceptionResult<TValue> : Result<TValue>
{
  public Exception Exception { get; }
  private ExceptionResult(Exception exception)
    : base(default, false, ErrorTypes.ErrorInternalServer) =>
    Exception = exception;
  
  public static ExceptionResult<TValue> WithException(Exception exception) => new(exception);
}