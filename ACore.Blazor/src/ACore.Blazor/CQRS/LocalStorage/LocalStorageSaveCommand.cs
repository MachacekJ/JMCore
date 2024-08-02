using MediatR;
using System.Text.Json;
using ACore.Blazor.CQRS.LocalStorage.Models;

namespace ACore.Blazor.CQRS.LocalStorage;

// ReSharper disable once ClassNeverInstantiated.Global
public class LocalStorageSaveCommand(LocalStorageCategoryEnum category, string key, object value, Type type) : IRequest
{
    public LocalStorageCategoryEnum Category => category;
    public string Key => key;
    public string Value => JsonSerializer.Serialize(value, type);
}