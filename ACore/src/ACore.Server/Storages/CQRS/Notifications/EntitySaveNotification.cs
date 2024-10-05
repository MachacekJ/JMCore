using ACore.Server.Storages.Models.SaveInfo;
using MediatR;

namespace ACore.Server.Storages.CQRS.Notifications;

public class EntitySaveNotification(SaveInfoItem saveInfoItem) : INotification
{
  public SaveInfoItem SaveInfo => saveInfoItem;
}
