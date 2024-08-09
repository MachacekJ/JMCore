using ACore.Base.CQRS.Notifications;
using ACore.Server.Modules.AuditModule.CQRS.AuditSave;
using ACore.Server.Storages.CQRS.Notifications;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ACore.Server.Modules.AuditModule.CQRS.NotificationHandlers;

public class EntitySaveNotificationHandler(ILogger<EntitySaveNotificationHandler> logger, IMediator mediator) : ACoreNotificationHandler<EntitySaveNotification>(logger)
{
  public override bool ThrowException => true;
  
  protected override async Task HandleMethod(EntitySaveNotification notification, CancellationToken cancellationToken)
  {
    await mediator.Send(new AuditSaveCommand(notification.SaveInfo), cancellationToken);
  }
}



