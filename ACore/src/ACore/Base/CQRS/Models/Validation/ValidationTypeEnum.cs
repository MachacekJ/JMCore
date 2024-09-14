namespace ACore.Base.CQRS.Models.Validation;

public enum ValidationTypeEnum
{
  ValidationInput,
  ValidationBusiness
}

public static class ErrorValidationTypeEnumExtensions
{
  public static Error ToError(this ValidationTypeEnum validationType)
  {
    return validationType switch
    {
      ValidationTypeEnum.ValidationBusiness => ErrorTypes.ErrorValidationBusiness,
      ValidationTypeEnum.ValidationInput => ErrorTypes.ErrorValidationInput,
      _ => throw new ArgumentOutOfRangeException(nameof(validationType), validationType, null)
    };
  }
}