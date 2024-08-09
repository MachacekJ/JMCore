using FluentValidation.Results;

namespace ACore.Base.CQRS.Pipelines.Models;

public class FluentValidationErrorItem(ValidationFailure validationFailure)
{
  public ValidationFailure ValidationFailure => validationFailure;
  
  public static FluentValidationErrorItem Create(ValidationFailure vf)
  {
    return new FluentValidationErrorItem(vf);
  }
}