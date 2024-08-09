using FluentValidation;

namespace ACore.Server.Modules.SettingsDbModule.CQRS.SettingsDbSave;

public class SettingsDbSaveValidator: AbstractValidator<SettingsDbSaveCommand>
{
  public SettingsDbSaveValidator()
  {
    RuleFor(r => r.Key).NotEmpty().NotNull().MaximumLength(256);
    RuleFor(r => r.Value).MaximumLength(65536);
  }
}