using System.Reflection;
using System.Text.Json;

namespace ACore.Extensions;

public static class ObjectExtensionMethods
{
  public static void CopyPropertiesFrom(this object self, object parent, Action<(string propName, string dataType, bool isChange, object? oldValue, object? newValue)>? updatingValue = null)
  {
    var fromProperties = GetProperties(parent);
    var toProperties = GetProperties(self);

    foreach (var fromProperty in fromProperties)
    {
      foreach (var toProperty in toProperties)
      {
        if (fromProperty.Name != toProperty.Name || fromProperty.PropertyType != toProperty.PropertyType)
          continue;

        var newValue = fromProperty.GetValue(parent);
        var oldValue = toProperty.GetValue(self);
        toProperty.SetValue(self, newValue);

        if (updatingValue == null)
          break;
        
        var isChange = (newValue == null && oldValue != null)
                       || (newValue != null && oldValue == null)
                       || (newValue != null && oldValue != null && !newValue.Equals(oldValue));

        updatingValue.Invoke(new ValueTuple<string, string, bool, object?, object?>(toProperty.Name, FullName(fromProperty.PropertyType), isChange, oldValue, newValue));

        break;
      }
    }
  }

  public static object? PropertyValue(this object self, string propertyName)
    => self.GetType().GetProperty(propertyName)?.GetValue(self);

  public static string HashObject(this object? text, string salt = "")
    => text == null ? string.Empty : JsonSerializer.Serialize(text).HashString(salt);

  public static IEnumerable<(string propName, string dataType, object? value)> AllPropertiesValues(this object self)
    => GetProperties(self).Select(e => new ValueTuple<string, string, object?>(e.Name, FullName(e.PropertyType), e.GetValue(self)));

  private static PropertyInfo[] GetProperties(object obj)
    => obj.GetType().GetProperties();

  private static string FullName(Type self)
    => self.FullName ?? throw new Exception($"{nameof(Type.FullName)} is null for datatype name {self.Name}");
}