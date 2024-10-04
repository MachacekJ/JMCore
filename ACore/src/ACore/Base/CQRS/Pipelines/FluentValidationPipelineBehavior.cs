using ACore.Base.CQRS.Pipelines.Models;
using ACore.Base.CQRS.Results;
using ACore.Base.CQRS.Results.Models;
using FluentValidation;
using MediatR;

namespace ACore.Base.CQRS.Pipelines;

public class FluentValidationPipelineBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators) : IPipelineBehavior<TRequest, TResponse>
  where TRequest : IRequest<TResponse>
  where TResponse : Result
{
  public async Task<TResponse> Handle(
    TRequest request,
    RequestHandlerDelegate<TResponse> next,
    CancellationToken cancellationToken)
  {
    if (!validators.Any())
      return await next();

    var errors = validators
      .Select(validator => validator.Validate(request))
      .SelectMany(validationResult => validationResult.Errors)
      .Where(validationFailure => validationFailure is not null)
      .Select(FluentValidationErrorItem.Create)
      .ToArray();

    if (errors.Length != 0)
      return CreateErrorValidationResult<TResponse>(ValidationTypeEnum.ValidationInput, errors);

    return await next();
  }

  private static TResult CreateErrorValidationResult<TResult>(ValidationTypeEnum validationTypeEnum, FluentValidationErrorItem[] errors)
    where TResult : Result
  {
    if (typeof(TResult) == typeof(Result))
      return ValidationResult.WithErrors(validationTypeEnum, errors) as TResult
             ?? throw new Exception($"Cannot convert {typeof(TResult).Name} to {nameof(Result)}");


    if (typeof(TResult).GetGenericTypeDefinition() != typeof(Result<>))
      throw new Exception($"Cannot convert {typeof(TResult).Name} to {nameof(Result)}");

    var validationResult = typeof(ValidationResult<>)
      .GetGenericTypeDefinition()
      .MakeGenericType(typeof(TResult).GenericTypeArguments[0])
      .GetMethod(nameof(ValidationResult<TResult>.WithErrors))?
      .Invoke(null, [validationTypeEnum, errors]);

    if (validationResult == null)
      throw new Exception($"Cannot create generic error result {typeof(TResult).Name} generic {typeof(TResult).GenericTypeArguments[0].Name}");

    return (TResult)validationResult;
  }
}