using System.Reflection;
using System.Text.Json;

namespace ACore.Extensions;

public static class ObjectExtensions
{
  public static object? PropertyValue(this object self, string propertyName)
    => GetProperty(self, propertyName)?.GetValue(self);

  public static string HashObject(this object? serializableObject, string salt = "")
    => serializableObject == null ? string.Empty : JsonSerializer.Serialize(serializableObject).HashString(salt);

  private static PropertyInfo? GetProperty(this object self, string propertyName)
    => self.GetType().GetProperty(propertyName);

  public static ComparisonResult[] Compare<T>(this T leftObj, T? rightObj, string? parentName = null)
    where T : class
  {
    var results = new List<ComparisonResult>();
    var rightProperties = rightObj == null ? null : GetProperties(rightObj);
    var leftProperties = GetProperties(leftObj);


    foreach (var leftProperty in leftProperties)
    {
      var leftValue = leftProperty.GetValue(leftObj);

      if (rightProperties == null)
      {
        results.Add(new ComparisonResult(leftProperty.Name, leftProperty.PropertyType, true, leftValue, null));
        continue;
      }

      foreach (var newProperty in rightProperties)
      {
        if (leftProperty.Name != newProperty.Name)
          continue;

        if (leftProperty.PropertyType != newProperty.PropertyType)
          continue;

        var rightValue = newProperty.GetValue(rightObj);

        var isChange = (rightValue == null && leftValue != null)
                       || (rightValue != null && leftValue == null);

        if (!isChange && rightValue != null && leftValue != null)
          isChange = CompareValue(leftValue, rightValue);

        results.Add(new ComparisonResult(leftProperty.Name, leftProperty.PropertyType, isChange, leftValue, rightValue));
        break;
      }
    }

    return results.ToArray();
  }

  private static bool CompareValue(object leftValue, object rightValue)
  {
    bool isChange;
    if (rightValue is byte[] enumRight && leftValue is byte[] enumLeft)
      isChange = !enumRight.SequenceEqual(enumLeft);
    else
      isChange = !rightValue.Equals(leftValue);
    return isChange;
  }

  private static PropertyInfo[] GetProperties(object obj)
    => obj.GetType().GetProperties();
}