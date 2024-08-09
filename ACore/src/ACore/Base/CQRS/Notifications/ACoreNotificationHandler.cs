using MediatR;
using Microsoft.Extensions.Logging;

namespace ACore.Base.CQRS.Notifications;

public abstract class ACoreNotificationHandler<TNotification>(ILogger logger) : ACoreNotificationHandler, INotificationHandler<TNotification>
  where TNotification : INotification
{
  protected abstract Task HandleMethod(TNotification notification, CancellationToken cancellationToken);

  public async Task Handle(TNotification notification, CancellationToken cancellationToken)
  {
    try
    {
      await HandleMethod(notification, cancellationToken);
    }
    catch (Exception e)
    {
      logger.LogError(e, e.Message);
      throw;
    }
  }
}

public abstract class ACoreNotificationHandler
{
  public abstract bool ThrowException { get; }
}