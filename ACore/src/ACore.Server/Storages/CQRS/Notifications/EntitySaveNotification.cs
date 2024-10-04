using ACore.Server.Storages.Definitions.Models.PK;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ACore.Server.Storages.CQRS.Notifications;

public class EntitySaveNotification<TEntity, TPK>(TEntity? oldEntity, TEntity? newEntity) : INotification
  where TEntity : PKEntity<TPK>
{
  public TEntity? OldEntity => oldEntity;
  public TEntity? NewEntity => newEntity;
}
