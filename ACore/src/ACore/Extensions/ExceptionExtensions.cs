namespace ACore.Extensions;

public static class ExceptionExtensions
{
  public static string MessageRecursive(this Exception ex, bool withStackTrace = false)
  {
    var allMessages = string.Empty;
    RecursiveInnerExceptionMessage(ex, ref allMessages);
    if (withStackTrace)
      allMessages += ex.StackTrace;
    return allMessages;
  }

  private static void RecursiveInnerExceptionMessage(Exception ex, ref string mess)
  {
    if (!string.IsNullOrEmpty(ex.Message))
      mess += "->" + ex.Message;


    if (ex is AggregateException aggregateException)
    {
      foreach (var exceptionItem in aggregateException.InnerExceptions)
      {
        RecursiveInnerExceptionMessage(exceptionItem, ref mess);
      }
    }
    else if (ex.InnerException != null)
    {
      RecursiveInnerExceptionMessage(ex.InnerException, ref mess);
    }
  }
}