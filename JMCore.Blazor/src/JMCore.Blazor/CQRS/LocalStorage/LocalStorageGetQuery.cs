using JMCore.Blazor.CQRS.LocalStorage.Models;
using MediatR;

namespace JMCore.Blazor.CQRS.LocalStorage;

public class LocalStorageGetQuery(LocalStorageCategoryEnum category, string key) : IRequest<LocalStorageData>
{
    public string Key => key;
    public LocalStorageCategoryEnum Category => category;
}