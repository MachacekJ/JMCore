using ACore.Extensions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ACore.Base.CQRS;

public abstract class LoggerNotificationHandler<TNotification>(ILogger logger) : INotificationHandler<TNotification>
  where TNotification : INotification
{
  protected abstract Task Handle2(TNotification notification, CancellationToken cancellationToken);

  public async Task Handle(TNotification notification, CancellationToken cancellationToken)
  {
    try
    {
      await Handle2(notification, cancellationToken);
    }
    catch (Exception e)
    {
      logger.LogError(e, e.Message);
      throw;
    }
  }
}

/// <summary>
/// Performs all <see cref="INotificationHandler{TNotification}"/> and some exception in class doesn't influence on success.
/// Exception is only logged not propagated. 
/// </summary>
public class TaskWhenAllPublisher : INotificationPublisher
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

public static class TaskWhenAllPublisherExtensions
{
  public static void ACoreMediatorConfiguration(this MediatRServiceConfiguration config)
  {
    // Setting the publisher directly will make the instance a Singleton.
    config.NotificationPublisher = new TaskWhenAllPublisher();

    // Seting the publisher type will:
    // 1. Override the value set on NotificationPublisher
    // 2. Use the service lifetime from the ServiceLifetime property below
    config.NotificationPublisherType = typeof(TaskWhenAllPublisher);

    config.Lifetime = ServiceLifetime.Transient;
  }
}