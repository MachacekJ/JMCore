using Microsoft.Extensions.Logging;
using Moq;

namespace ACore.UnitTests.FakeMoq;

public class LoggerHelper<T>
  where T : class
{
 
  public List<LogLevel> LogLevels { get; } = new();
  public List<string> LogMessages { get; } = new();
  public List<Exception> LogExceptions { get; } = new();
  public ILogger<T> LoggerMocked
  {
    get
    {
      var loggerMock = new Mock<ILogger<T>>();
      loggerMock.Setup(e => e.IsEnabled(It.IsAny<LogLevel>())).Returns(true);
      loggerMock.Setup(m => m.Log(
        It.IsAny<LogLevel>(),
        It.IsAny<EventId>(),
        It.IsAny<It.IsAnyType>(),
        It.IsAny<Exception>(),
        It.IsAny<Func<It.IsAnyType, Exception?, string>>()
      )).Callback(new InvocationAction(invocation =>
      {
        var logLevel = (LogLevel)invocation.Arguments[0];
        //var eventId = (EventId)invocation.Arguments[1];
        var state = invocation.Arguments[2];
        var exception = (Exception)invocation.Arguments[3];
        var formatter = invocation.Arguments[4];

        var invokeMethod = formatter.GetType().GetMethod("Invoke");
        var logMessage = invokeMethod?.Invoke(formatter, [state, exception]) ?? throw new NullReferenceException();

        LogLevels.Add(logLevel);
        LogMessages.Add((string)logMessage);
        LogExceptions.Add(exception);
      }));
      return loggerMock.Object;
    }
  }
}