namespace ACore.Extensions;

public static class TypeExtensions
{
  public static string ACoreTypeName(this Type type)
  => type.FullName ?? type.Name;

  public static bool IsSubclassOfRawGeneric(this Type toCheck, Type  generic)
  {
    while (toCheck != null && toCheck != typeof(object))
    {
      var cur = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
      if (generic == cur)
      {
        return true;
      }

      if (toCheck.BaseType != null)
        toCheck = toCheck.BaseType;
      else
        break;
    }

    return false;
  }
}