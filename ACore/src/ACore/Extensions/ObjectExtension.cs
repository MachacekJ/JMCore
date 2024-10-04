using System.Reflection;
using System.Text.Json;

namespace ACore.Extensions;

public static class ObjectExtensionMethods
{
  
  public static object? PropertyValue(this object self, string propertyName)
    => GetProperty(self, propertyName)?.GetValue(self);

  public static string HashObject(this object? serializableObject, string salt = "")
    => serializableObject == null ? string.Empty : JsonSerializer.Serialize(serializableObject).HashString(salt);
  
  private static PropertyInfo? GetProperty(this object self, string propertyName)
    => self.GetType().GetProperty(propertyName);
}