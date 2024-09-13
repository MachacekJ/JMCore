namespace ACore.Base.CQRS.Models.Validation;

public enum ErrorValidationTypeEnum
{
  ValidationInput,
  ValidationBusiness
}

public static class ErrorValidationTypeEnumExtensions
{
  public static Error ToError(this ErrorValidationTypeEnum errorValidationType)
  {
    return errorValidationType switch
    {
      ErrorValidationTypeEnum.ValidationBusiness => ErrorTypes.ErrorValidationBusiness,
      ErrorValidationTypeEnum.ValidationInput => ErrorTypes.ErrorValidationInput,
      //  ErrorValidationTypeEnum.ModuleActivation => ErrorValidationTypeCodes.ErrorModuleActivation,
      _ => throw new ArgumentOutOfRangeException(nameof(errorValidationType), errorValidationType, null)
    };
  }
}