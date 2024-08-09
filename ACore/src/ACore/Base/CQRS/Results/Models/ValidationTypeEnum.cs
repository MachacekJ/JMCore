namespace ACore.Base.CQRS.Results.Models;

public enum ValidationTypeEnum
{
  ValidationInput,
  ValidationBusiness
}

public static class ValidationTypeEnumExtensions
{
  public static ResultErrorItem ToError(this ValidationTypeEnum validationType)
  {
    return validationType switch
    {
      ValidationTypeEnum.ValidationBusiness => ValidationResult.ResultErrorItemValidationBusiness,
      ValidationTypeEnum.ValidationInput => ValidationResult.ResultErrorItemValidationInput,
      _ => throw new ArgumentOutOfRangeException(nameof(validationType), validationType, null)
    };
  }
}