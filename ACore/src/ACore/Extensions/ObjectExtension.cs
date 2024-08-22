using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices.Marshalling;

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
          continue;

        if (newValue == null && oldValue != null)
        {
          toProperty.SetValue(self, newValue);
          updatingValue?.Invoke(new(toProperty.Name, oldValue, newValue));
        }

        if (oldValue == null && newValue != null)
        {
          toProperty.SetValue(self, newValue);
          updatingValue?.Invoke(new(toProperty.Name, oldValue, newValue));
        }
        
        if (newValue != null && !newValue.Equals(oldValue))
        {
          toProperty.SetValue(self, newValue);
          updatingValue?.Invoke(new(toProperty.Name, oldValue, newValue));
        }

        break;
      }
    }
  }

  public static IEnumerable<(string propName, object? value)> AllProperties(this object self)
    => GetProperties(self).Select(e=>new ValueTuple<string, object?>(e.Name, e.GetValue(self)));
  
  public static List<(string propName, object? originalValue, object? newValue)> Differences<T>(this T original, T newValues)
  {
    var diffs = new List<(string propName, object? originalValue, object? newValue)>();
    foreach (var prop in typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public))
    {
      var newVal = prop.GetValue(newValues);
      var origin = prop.GetValue(original);
      var areEqual = AreEqual(newVal, origin);

      if (!areEqual)
      {
        diffs.Add(new ValueTuple<string, object?, object?>(prop.Name, origin, newVal));
      }
    }

    return diffs;
  }

  private static bool AreEqual<T>(T x, T y)
  {
    return EqualityComparer<T>.Default.Equals(x, y);
  }

  private static PropertyInfo[] GetProperties(object obj)
  => obj.GetType().GetProperties();
}