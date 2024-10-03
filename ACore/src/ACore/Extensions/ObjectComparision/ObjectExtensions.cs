using System.Reflection;

namespace ACore.Extensions.ObjectComparision;

public static class ObjectExtensions
{
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
                       || (rightValue != null && leftValue == null)
                       || (rightValue != null && leftValue != null && !rightValue.Equals(leftValue));

        results.Add(new ComparisonResult(leftProperty.Name, leftProperty.PropertyType, isChange, leftValue, rightValue));
        break;
      }
    }

    return results.ToArray();
  }
  
  private static PropertyInfo[] GetProperties(object obj)
    => obj.GetType().GetProperties();
}

