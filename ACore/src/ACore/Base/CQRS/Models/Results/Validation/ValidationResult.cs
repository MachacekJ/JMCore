namespace ACore.Base.CQRS.Models.Results.Validation;

public class ValidationResult : Result
{
  public ValidationError[] ValidationErrors { get; }

  private ValidationResult(ValidationTypeEnum validationType, ValidationError[] validationErrors)
    : base(false, validationType.ToError())
  {
    ValidationErrors = validationErrors;
  }

  public static ValidationResult WithErrors(ValidationTypeEnum validationType, ValidationError[] errors) => new(validationType, errors);
}

public class ValidationResult<TValue> : Result<TValue>
{
  private ValidationResult(ValidationTypeEnum validationType, ValidationError[] validationErrors)
    : base(default, false, validationType.ToError()) =>
    ValidationErrors = validationErrors;

  public ValidationError[] ValidationErrors { get; }
  
  public static ValidationResult<TValue> WithErrors(ValidationTypeEnum validationType, ValidationError[] errors) => new(validationType, errors);
}