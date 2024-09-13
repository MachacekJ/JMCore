namespace ACore.Base.CQRS.Models.Validation;

public class ValidationResult : Result
{
  public Error[] ValidationErrors { get; }

  private ValidationResult(ErrorValidationTypeEnum errorValidationType, Error[] validationErrors)
    : base(false, errorValidationType.ToError())
  {
    ValidationErrors = validationErrors;
  }

  public static ValidationResult WithErrors(ErrorValidationTypeEnum errorValidationType, Error[] errors) => new(errorValidationType, errors);
}

public class ValidationResult<TValue> : Result<TValue>
{
  private ValidationResult(ErrorValidationTypeEnum errorValidationType, Error[] validationErrors)
    : base(default, false, errorValidationType.ToError()) =>
    ValidationErrors = validationErrors;

  public Error[] ValidationErrors { get; }
  
  public static ValidationResult<TValue> WithErrors(ErrorValidationTypeEnum errorValidationType, Error[] errors) => new(errorValidationType, errors);
}