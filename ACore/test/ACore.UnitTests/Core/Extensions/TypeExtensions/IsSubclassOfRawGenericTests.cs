using ACore.Extensions;
using ACore.UnitTests.Core.Extensions.ObjectExtensions.FakeData;
using FluentAssertions;

namespace ACore.UnitTests.Core.Extensions.ObjectExtensions;

public class IsSubclassOfRawGenericTests
{
  [Theory]
  [InlineData(typeof(GenericClass<>), true)]
  [InlineData(typeof(object), false)]
  [InlineData(typeof(IsSubclassOfRawGenericTests), false)]
  public void BaseTest(Type typeToCheck, bool result)
  {
    // Arrange
    var classAsSut = new InheritedGenericClass("fake");

    // Act
    var res = classAsSut.GetType().IsSubclassOfRawGeneric(typeToCheck);

    // Assert
    res.Should().Be(result);
  }
}