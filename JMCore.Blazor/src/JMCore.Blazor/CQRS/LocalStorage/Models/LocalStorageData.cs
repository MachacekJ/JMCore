using System.Text.Json;

namespace JMCore.Blazor.CQRS.LocalStorage.Models;

public class LocalStorageData
{
    public LocalStorageData()
    {
        IsValue = false;
    }

    public LocalStorageData(string value)
    {
        Value = value; // Newtonsoft.Json.JsonConvert.SerializeObject(value);
        IsValue = true;
    }

    private string Value { get; } = null!;
    public bool IsValue { get; }

    public T? GetValue<T>()
    {
        return JsonSerializer.Deserialize<T>(Value);
    }
}