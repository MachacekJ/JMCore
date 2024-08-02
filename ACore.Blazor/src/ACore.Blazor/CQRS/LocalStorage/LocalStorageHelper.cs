using ACore.Blazor.CQRS.LocalStorage.Models;

namespace ACore.Blazor.CQRS.LocalStorage
{
    public static class LocalStorageHelper
    {
        public static string GetKey(LocalStorageCategoryEnum category, string key) => $"{category}-{key}";
    }
}