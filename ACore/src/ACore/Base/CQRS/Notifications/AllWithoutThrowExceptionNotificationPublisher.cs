using ACore.Extensions;
using MediatR;

namespace ACore.Base.CQRS.Notifications;

/// <summary>
/// Performs all <see cref="INotificationHandler{TNotification}"/> and some exception in class doesn't influence on success.
/// Exception is only logged not propagated. 
/// </summary>
public class AllWithoutThrowExceptionNotificationPublisher : INotificationPublisher
{
  public async Task Publish(
    IEnumerable<NotificationHandlerExecutor> handlerExecutors,
    INotification notification,
    CancellationToken cancellationToken)
  {
    var tasks = new List<Task>();
    foreach (var handler in handlerExecutors)
    {
      if (CheckHandler(handler.HandlerInstance))
        tasks.Add(handler.HandlerCallback(
          notification,
          cancellationToken));
      else
        throw new Exception($"Notification Handler {handler.HandlerInstance.GetType().FullName} must be inherited from {typeof(LoggerNotificationHandler<>).FullName}");
    }

    try
    {
      var allTask = Task.WhenAll(tasks);
      await allTask.ConfigureAwait(false);
    }
    catch
    {
      // ignored -> We don't want to stop running e.g. Save Entity, when INotificationHandler throw any exception.
      // This exception must be logged. Each handler must be inherited from LoggerNotificationHandler.
      //  if (allTask?.Exception != null) ExceptionDispatchInfo.Capture(allTask.Exception).Throw();
    }
  }

  private bool CheckHandler(object handler)
    => handler.GetType().IsSubclassOfRawGeneric(typeof(LoggerNotificationHandler<>));
}