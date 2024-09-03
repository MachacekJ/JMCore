using FluentValidation;
using FluentValidation.Validators;

namespace ACore.Server.Modules.SettingModule.CQRS.SettingSave;

public class SettingSaveValidator: AbstractValidator<SettingSaveCommand>
{
  public SettingSaveValidator()
  {
    //var text = languageManager.GetString("LessThanValidator");
    RuleFor(r => r.Key).NotEmpty().NotNull().MaximumLength(256);
    RuleFor(r => r.Value).MaximumLength(65536);
    
  }
}