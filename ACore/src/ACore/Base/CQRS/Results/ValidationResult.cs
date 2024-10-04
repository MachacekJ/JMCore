using ACore.Base.CQRS.Pipelines.Models;
using ACore.Base.CQRS.Results.Models;

namespace ACore.Base.CQRS.Results;

public class ValidationResult : Result
{
  public static readonly ResultErrorItem ResultErrorItemValidationInput = new(
    "ValidationInput",
    "A validation problem occurred.");

  public static readonly ResultErrorItem ResultErrorItemValidationBusiness = new(
    "ValidationBusiness",
    "A business validation problem occurred.");
  public FluentValidationErrorItem[] ValidationErrors { get; }

  private ValidationResult(ValidationTypeEnum validationType, FluentValidationErrorItem[] validationErrors)
    : base(false, validationType.ToError())
  {
    ValidationErrors = validationErrors;
  }

  public static ValidationResult WithErrors(ValidationTypeEnum validationType, FluentValidationErrorItem[] errors) => new(validationType, errors);
}

public class ValidationResult<TValue> : Result<TValue>
{
  private ValidationResult(ValidationTypeEnum validationType, FluentValidationErrorItem[] validationErrors)
    : base(default, false, validationType.ToError()) =>
    ValidationErrors = validationErrors;

  public FluentValidationErrorItem[] ValidationErrors { get; }
  
  public static ValidationResult<TValue> WithErrors(ValidationTypeEnum validationType, FluentValidationErrorItem[] errors) => new(validationType, errors);
}