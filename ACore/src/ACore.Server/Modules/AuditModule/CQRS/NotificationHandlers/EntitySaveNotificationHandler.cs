using ACore.Base.CQRS;
using ACore.Base.CQRS.Notifications;
using ACore.Server.Storages.CQRS.Notifications;
using ACore.Server.Storages.Definitions.Models.PK;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ACore.Server.Modules.AuditModule.CQRS.NotificationHandlers;

public class EntitySaveNotificationHandler<TEntity, TPK>(ILogger<EntitySaveNotificationHandler<TEntity, TPK>> logger) : LoggerNotificationHandler<EntitySaveNotification<TEntity, TPK>>(logger)
  where TEntity : PKEntity<TPK>
{
  protected override async Task HandleMethod(EntitySaveNotification<TEntity, TPK> notification, CancellationToken cancellationToken)
  {
    await Task.Delay(1000, cancellationToken);
    throw new NotImplementedException();
    var old = notification.OldEntity;
    var newE = notification.NewEntity;

    //return Task.CompletedTask;
  }
}

public class EntitySaveNotificationHandler2<TEntity, TPK>(ILogger<EntitySaveNotificationHandler<TEntity, TPK>> logger) : LoggerNotificationHandler<EntitySaveNotification<TEntity, TPK>>(logger)
  where TEntity : PKEntity<TPK>
{
  protected override async Task HandleMethod(EntitySaveNotification<TEntity, TPK> notification, CancellationToken cancellationToken)
  {
    await Task.Delay(500, cancellationToken);
    throw new AggregateException();
    var old = notification.OldEntity;
    var newE = notification.NewEntity;

    // return Task.CompletedTask;
  }
}



