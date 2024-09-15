namespace ACore.Extensions;

public static class StringExtensions
{
  public static Type GetType(this string dataTypeAsString)
    => Type.GetType(dataTypeAsString) ?? throw new Exception($"Unknown datatype {dataTypeAsString}.");
}