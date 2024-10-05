using ACore.UnitTests.Core.Base.CQRS.Notifications.ACoreNotificationPublisher.FakeClasses;
using ACore.UnitTests.FakeMoq;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ACore.UnitTests.Core.Base.CQRS.Notifications.ACoreNotificationPublisher;

public class PublishTests
{
  [Fact]
  public async Task ThrowExceptionTest()
  {
    // Arrange
    var loggerHelper = new LoggerHelper<ThrowNotificationHandler>();
    var throwNotification = new ThrowNotification();
    var allHandlers = AllHandlers(loggerHelper.LoggerMocked, true);
    var sut = CreateNotificationPublisherAsSut();

    // Act
    Func<Task> ac = async () =>  await sut.Publish(allHandlers, throwNotification, CancellationToken.None);
    
    // Assert
    await ac.Should().ThrowAsync<NotImplementedException>();
  }
  
  [Fact]
  public async Task NotThrowExceptionTest()
  {
    // Arrange
    var loggerHelper = new LoggerHelper<ThrowNotificationHandler>();
    var throwNotification = new ThrowNotification();
    var allHandlers = AllHandlers(loggerHelper.LoggerMocked, false);
    var sut = CreateNotificationPublisherAsSut();

    // Act
    await sut.Publish(allHandlers, throwNotification, CancellationToken.None);

    // Assert
    loggerHelper.LogLevels.Should().HaveCountGreaterThan(0);
    loggerHelper.LogMessages.Should().HaveCountGreaterThan(0);
    loggerHelper.LogExceptions.Where(e => e.Message == new NotImplementedException().Message).Should().HaveCount(2);
  }

  private static ACore.Base.CQRS.Notifications.ACoreNotificationPublisher CreateNotificationPublisherAsSut()
    => new();

  private static List<NotificationHandlerExecutor> AllHandlers(ILogger<ThrowNotificationHandler> loggerHelper, bool throwExceptions)
  {
    var aa = new ThrowNotificationHandler(loggerHelper, throwExceptions);
    var h = new List<NotificationHandlerExecutor>
    {
      new(aa, (notification, cancellationToken) => aa.Handle(notification as ThrowNotification ?? throw new InvalidOperationException(), cancellationToken)),
      new(aa, (notification, cancellationToken) => aa.Handle(notification as ThrowNotification ?? throw new InvalidOperationException(), cancellationToken))
    };
    return h;
  }
}