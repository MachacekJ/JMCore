namespace ACore.UnitTests.Core.Extensions.TypeExtensions.FakeData;

public class GenericClass<T>(T value)
{
  public T Test => value;
}