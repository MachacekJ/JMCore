using Blazored.LocalStorage;
using MediatR;

namespace ACore.Blazor.CQRS.LocalStorage;

// ReSharper disable once UnusedType.Global
public class LocalStorageSaveHandler(ILocalStorageService localStorage) : IRequestHandler<LocalStorageSaveCommand>
{
    public async Task Handle(LocalStorageSaveCommand request, CancellationToken cancellationToken)
    {
        await localStorage.SetItemAsync(LocalStorageHelper.GetKey(request.Category, request.Key), request.Value, cancellationToken);
    }
}