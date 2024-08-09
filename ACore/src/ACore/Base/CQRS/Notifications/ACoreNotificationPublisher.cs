using System.Runtime.ExceptionServices;
using ACore.Extensions;
using MediatR;

namespace ACore.Base.CQRS.Notifications;

/// <summary>
/// Performs all <see cref="INotificationHandler{TNotification}"/> and some exception in class doesn't influence on success.
/// Exception is only logged not propagated. 
/// </summary>
public class ACoreNotificationPublisher : INotificationPublisher
{
  public async Task Publish(
    IEnumerable<NotificationHandlerExecutor> handlerExecutors,
    INotification notification,
    CancellationToken cancellationToken)
  {
    var withExceptions = new List<Task>();
    var withoutExceptions = new List<Task>();

    foreach (var handler in handlerExecutors)
    {
      if (!ValidateHandler(handler.HandlerInstance))
        throw new Exception($"Notification Handler {handler.HandlerInstance.GetType().FullName} must be inherited from {typeof(ACoreNotificationHandler<>).FullName}");

      var aCoreNotificationHandler = (ACoreNotificationHandler)handler.HandlerInstance;
      if (aCoreNotificationHandler.ThrowException)
        withExceptions.Add(handler.HandlerCallback(notification, cancellationToken));
      else
        withoutExceptions.Add(handler.HandlerCallback(notification, cancellationToken));
    }

    Task? allTaskWithException = null;
    try
    {
      allTaskWithException = Task.WhenAll(withExceptions);
      await allTaskWithException.ConfigureAwait(false);
    }
    catch
    {
      if (allTaskWithException?.Exception != null) ExceptionDispatchInfo.Capture(allTaskWithException.Exception).Throw();
      throw;
    }
    
    try
    {
      var allTaskWithoutException = Task.WhenAll(withoutExceptions);
      await allTaskWithoutException.ConfigureAwait(false);
    }
    catch
    {
      // ignored -> We don't want to stop running e.g. Save Entity, when INotificationHandler throw any exception.
      // This exception must be logged. Each handler must be inherited from LoggerNotificationHandler.
      //  if (allTask?.Exception != null) ExceptionDispatchInfo.Capture(allTask.Exception).Throw();
    }
  }

  private bool ValidateHandler(object handler)
    => handler.GetType().IsSubclassOfRawGeneric(typeof(ACoreNotificationHandler<>));
}