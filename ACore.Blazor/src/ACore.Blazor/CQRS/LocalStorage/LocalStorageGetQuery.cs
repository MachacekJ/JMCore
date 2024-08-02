using ACore.Blazor.CQRS.LocalStorage.Models;
using MediatR;

namespace ACore.Blazor.CQRS.LocalStorage;

public class LocalStorageGetQuery(LocalStorageCategoryEnum category, string key) : IRequest<LocalStorageData>
{
    public string Key => key;
    public LocalStorageCategoryEnum Category => category;
}