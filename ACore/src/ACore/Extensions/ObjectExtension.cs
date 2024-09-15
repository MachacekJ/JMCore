using System.Reflection;

namespace ACore.Extensions;

public static class ObjectExtensionMethods
{
  public static void CopyPropertiesFrom(this object self, object parent, Action<(string propName, object? oldValue, object? newValue)>? updatingValue = null)
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

        if (newValue == null && oldValue == null)
        {
          updatingValue?.Invoke(new(toProperty.Name, oldValue, newValue));
          continue;
        }

        var isNew = false;
        if (newValue == null && oldValue != null)
        {
          toProperty.SetValue(self, newValue);
          isNew = true;
        }

        if (oldValue == null && newValue != null)
        {
          toProperty.SetValue(self, newValue);
          isNew = true;
        }
        
        if (newValue != null && !newValue.Equals(oldValue))
        {
          toProperty.SetValue(self, newValue);
          isNew = true;
        }
        
        updatingValue?.Invoke(new(toProperty.Name, oldValue, isNew ? newValue : null));
        
        break;
      }
    }
  }

  public static object? PropertyValue(this object self, string propertyName)
    => self.GetType().GetProperty(propertyName)?.GetValue(self);

  
  public static IEnumerable<(string propName, object? value)> AllPropertiesValues(this object self)
    => GetProperties(self).Select(e=>new ValueTuple<string, object?>(e.Name, e.GetValue(self)));
  
  private static PropertyInfo[] GetProperties(object obj)
  => obj.GetType().GetProperties();
}