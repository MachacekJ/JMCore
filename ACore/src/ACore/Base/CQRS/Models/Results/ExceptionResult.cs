namespace ACore.Base.CQRS.Models.Results;

public class ExceptionResult : Result
{
  public Exception Exception { get; }
  private ExceptionResult(Exception exception) : base(false, Results.Error.Error.ErrorInternalServer)
  {
    Exception = exception;
  }
  public static ExceptionResult WithException(Exception exception) => new(exception);
}

public class ExceptionResult<TValue> : Result<TValue>
{
  public Exception Exception { get; }
  private ExceptionResult(Exception exception)
    : base(default, false, Results.Error.Error.ErrorInternalServer) =>
    Exception = exception;
  
  public static ExceptionResult<TValue> WithException(Exception exception) => new(exception);
}