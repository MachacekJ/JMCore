namespace ACore.Server.Storages.Attributes;

public static class CheckSumAttributesExtensions
{
  public static bool IsHashCheck(this Type entityEntry)
  {
    var enableCheckSumAttribute = Attribute.GetCustomAttribute(entityEntry, typeof(CheckSumAttribute));
    
    if (enableCheckSumAttribute is CheckSumAttribute checkSumAttribute)
      return true;

    return false;
  }
}