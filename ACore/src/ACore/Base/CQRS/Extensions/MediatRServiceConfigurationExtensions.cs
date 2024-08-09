using ACore.Base.CQRS.Notifications;
using Microsoft.Extensions.DependencyInjection;

namespace ACore.Base.CQRS.Extensions;

public static class MediatRServiceConfigurationExtensions
{
  public static void AllNotificationWithoutException(this MediatRServiceConfiguration config)
  {
    // Setting the publisher directly will make the instance a Singleton.
    config.NotificationPublisher = new ACoreNotificationPublisher();
    config.NotificationPublisherType = typeof(ACoreNotificationPublisher);
  }
}