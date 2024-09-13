using System.Reflection;
using ACore.Base.CQRS.Models;
using ACore.Base.CQRS.Models.Validation;
using ACore.Extensions;
using ACore.Models;
using ACore.Server.Modules.SettingModule.CQRS.SettingGet;
using ACore.Server.Modules.SettingModule.CQRS.SettingSave;
using FluentAssertions;
using FluentValidation;
using Xunit;

namespace ACore.Tests.Server.Modules.SettingModule;

public class CQRS : SettingStorageModule
{
  [Fact]
  public async Task SettingsCommandAndQueryTest_OK()
  {
    const string key = "key";
    const string value = "value";

    var method = MethodBase.GetCurrentMethod();
    await RunTestAsync(method, async () =>
    {
      var resultCommand = await Mediator.Send(new SettingSaveCommand(key, value));
      resultCommand.IsSuccess.Should().BeTrue();
      
      var result = await Mediator.Send(new SettingGetQuery(key));
      result.ResultValue.Should().Be(value);
    });
  }

  [Fact]
  public async Task SettingsCommandAndQueryTest_EmptyKey()
  {
    const string key = "";
    const string value = "value";

    var method = MethodBase.GetCurrentMethod();
    await RunTestAsync(method, async () =>
    {
      var result = await Mediator.Send(new SettingSaveCommand(key, value));
      result.IsFailure.Should().Be(true);
      result.IsSuccess.Should().Be(false);
      result.Error.Should().NotBeNull();
      result.Error.Code.Should().NotBeNull().And.Be(ErrorTypes.ErrorValidationInput.Code);
      result.Error.Message.Should().NotBeNull();
      var validationResult = result as ValidationResult;
      validationResult.Should().NotBeNull();
      validationResult?.ValidationErrors.Should().NotBeNull().And.HaveCountGreaterThan(0);
    });
  }
  
  [Fact]
  public async Task SettingsCommandAndQueryTest_MaxLengthKey()
  {
    var key = new string('*', 257);
    const string value = "value";

    var method = MethodBase.GetCurrentMethod();
    await RunTestAsync(method, async () =>
    {
      var result = await Mediator.Send(new SettingSaveCommand(key, value));
      result.IsFailure.Should().Be(true);
      result.IsSuccess.Should().Be(false);
      result.Error.Should().NotBeNull();
      result.Error.Code.Should().NotBeNull().And.Be(ErrorTypes.ErrorValidationInput.Code);
      result.Error.Message.Should().NotBeNull();
      var validationResult = result as ValidationResult;
      validationResult.Should().NotBeNull();
      validationResult?.ValidationErrors.Should().NotBeNull().And.HaveCountGreaterThan(0);
    });
  }
  
  [Fact]
  public async Task SettingsCommandAndQueryTest_MaxLengthValue()
  {
    const string key = "test";
    var value = new string('*', 65537);;

    var method = MethodBase.GetCurrentMethod();
    await RunTestAsync(method, async () =>
    {
      var result = await Mediator.Send(new SettingSaveCommand(key, value));
      result.IsFailure.Should().Be(true);
      result.IsSuccess.Should().Be(false);
      result.Error.Should().NotBeNull();
      result.Error.Code.Should().NotBeNull().And.Be(ErrorTypes.ErrorValidationInput.Code);
      result.Error.Message.Should().NotBeNull();
      var validationResult = result as ValidationResult;
      validationResult.Should().NotBeNull();
      validationResult?.ValidationErrors.Should().NotBeNull().And.HaveCountGreaterThan(0);
      validationResult?.ValidationErrors[0].FormattedMessagePlaceholderValues.Should().HaveCountGreaterThan(4);
      validationResult?.ValidationErrors[0].Code.Should().NotBeEmpty();
      validationResult?.ValidationErrors[0].Message.Should().NotBeEmpty();
      validationResult?.ValidationErrors[0].ParamName.Should().NotBeEmpty();
      validationResult?.ValidationErrors[0].Severity.Should().Be(Severity.Error);
    });
  }
}