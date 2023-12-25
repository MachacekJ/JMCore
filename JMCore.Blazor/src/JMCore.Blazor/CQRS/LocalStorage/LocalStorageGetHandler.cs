using Blazored.LocalStorage;
using JMCore.Blazor.CQRS.LocalStorage.Models;
using MediatR;

namespace JMCore.Blazor.CQRS.LocalStorage;

public class LocalStorageGetHandler(ILocalStorageService localStorage)
    : IRequestHandler<LocalStorageGetQuery, LocalStorageData>
{
    public async Task<LocalStorageData> Handle(LocalStorageGetQuery request, CancellationToken cancellationToken)
    {
        var key = LocalStorageHelper.GetKey(request.Category, request.Key);
        if (!await localStorage.ContainKeyAsync(key, cancellationToken))
            return new LocalStorageData();

        var value = await localStorage.GetItemAsync<string>(key, cancellationToken);
        return value != null ? new LocalStorageData(value) : new LocalStorageData();
    }
}