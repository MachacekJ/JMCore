using System.Reflection;
using System.Runtime.InteropServices.Marshalling;

namespace ACore.Extensions;

public static class ObjectExtensionMethods
{
  public static void CopyPropertiesFrom(this object self, object parent, Action<(string propName, object? oldValue, object? newValue)>? update = null)
  {
    var fromProperties = parent.GetType().GetProperties();
    var toProperties = self.GetType().GetProperties();

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
          update?.Invoke(new(toProperty.Name, oldValue, newValue));
        }

        if (oldValue == null && newValue != null)
        {
          toProperty.SetValue(self, newValue);
          update?.Invoke(new(toProperty.Name, oldValue, newValue));
        }
        
        if (newValue != null && !newValue.Equals(oldValue))
        {
          toProperty.SetValue(self, newValue);
          update?.Invoke(new(toProperty.Name, oldValue, newValue));
        }

        break;
      }
    }
  }

  public static IEnumerable<(string propName, object? value)> AllProperties(this object self)
    => self.GetType().GetProperties().Select(e=>new ValueTuple<string, object?>(e.Name, e.GetValue(self)));
  

  public static T? GetPropValue<T>(this object self, string name)
  {
    var fromProperties = self.GetType().GetProperties().Single(p => p.Name == name).GetValue(self, null);

    if (fromProperties == null)
      return default;

    return (T)Convert.ChangeType(fromProperties, typeof(T));
  }

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
}