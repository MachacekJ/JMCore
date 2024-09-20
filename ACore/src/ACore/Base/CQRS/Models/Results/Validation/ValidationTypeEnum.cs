namespace ACore.Base.CQRS.Models.Results.Validation;

public enum ValidationTypeEnum
{
  ValidationInput,
  ValidationBusiness
}

public static class ErrorValidationTypeEnumExtensions
{
  public static Error.Error ToError(this ValidationTypeEnum validationType)
  {
    return validationType switch
    {
      ValidationTypeEnum.ValidationBusiness => Error.Error.ErrorValidationBusiness,
      ValidationTypeEnum.ValidationInput => Error.Error.ErrorValidationInput,
      _ => throw new ArgumentOutOfRangeException(nameof(validationType), validationType, null)
    };
  }
}