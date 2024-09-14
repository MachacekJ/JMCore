namespace ACore.Base.CQRS.Models.Validation;

public class ValidationResult : Result
{
  public Error[] ValidationErrors { get; }

  private ValidationResult(ValidationTypeEnum validationType, Error[] validationErrors)
    : base(false, validationType.ToError())
  {
    ValidationErrors = validationErrors;
  }

  public static ValidationResult WithErrors(ValidationTypeEnum validationType, Error[] errors) => new(validationType, errors);
}

public class ValidationResult<TValue> : Result<TValue>
{
  private ValidationResult(ValidationTypeEnum validationType, Error[] validationErrors)
    : base(default, false, validationType.ToError()) =>
    ValidationErrors = validationErrors;

  public Error[] ValidationErrors { get; }
  
  public static ValidationResult<TValue> WithErrors(ValidationTypeEnum validationType, Error[] errors) => new(validationType, errors);
}