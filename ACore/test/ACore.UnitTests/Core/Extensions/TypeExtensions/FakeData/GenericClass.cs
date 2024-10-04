namespace ACore.UnitTests.Core.Extensions.ObjectExtensions.FakeData;

public class GenericClass<T>(T value)
{
  public T Test => value;
}