using ACore.Base.CQRS.Notifications;
using Microsoft.Extensions.Logging;

namespace ACore.UnitTests.Core.Base.CQRS.Notifications.ACoreNotificationPublisher.FakeClasses;

public class ThrowNotificationHandler(ILogger<ThrowNotificationHandler> logger, bool throwException) : ACoreNotificationHandler<ThrowNotification>(logger)
{
  public override bool ThrowException => throwException;

  protected override Task HandleMethod(ThrowNotification notification, CancellationToken cancellationToken)
  {
    throw new NotImplementedException();
  }
}