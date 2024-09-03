namespace ACore.Models;

public static class ErrorValidationTypeCodes
{
  public const string ValidationInput = "ValidationInput";
  public const string ValidationBusiness = "ValidationBusiness";
}

public enum ErrorValidationTypeEnum
{
  ValidationInput,
  ValidationBusiness
}

public sealed class ValidationResult : Result
{
  private static readonly Error ErrorValidationInput = new(
    ErrorValidationTypeCodes.ValidationInput,
    "A validation problem occurred.");

  private static readonly Error ErrorValidationBusiness = new(
    ErrorValidationTypeCodes.ValidationBusiness,
    "A business validation problem occurred.");
  
  private ValidationResult(Error[] errors, ErrorValidationTypeEnum errorValidationType)
    : base(false, errorValidationType switch {
      ErrorValidationTypeEnum.ValidationBusiness => ErrorValidationBusiness,
      ErrorValidationTypeEnum.ValidationInput => ErrorValidationInput,
      _ => throw new ArgumentOutOfRangeException(nameof(errorValidationType), errorValidationType, null)
    } ) =>
    Errors = errors;

  public Error[] Errors { get; }

  public static ValidationResult WithErrors(Error[] errors, ErrorValidationTypeEnum errorValidationType) => new(errors, errorValidationType);
}

