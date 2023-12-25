using JMCore.Blazor.CQRS.LocalStorage.Models;

namespace JMCore.Blazor.CQRS.LocalStorage
{
    public static class LocalStorageHelper
    {
        public static string GetKey(LocalStorageCategoryEnum category, string key) => $"{category}-{key}";
    }
}