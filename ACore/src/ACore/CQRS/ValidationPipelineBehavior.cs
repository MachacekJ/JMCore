using ACore.Models;
using FluentValidation;
using MediatR;

namespace ACore.CQRS;

public class ValidationPipelineBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators) : IPipelineBehavior<TRequest, TResponse>
  where TRequest : MediatR.IRequest<TResponse>
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
      .Select(Error.Create)
      .ToArray();

    if (errors.Length != 0)
      return CreateValidationResult<TResponse>(errors);
    
    return await next();
  }

  private static TResult CreateValidationResult<TResult>(Error[] errors)
    where TResult : Result
  {
    if (typeof(TResult) != typeof(Result))
      throw new Exception($"Cannot convert {typeof(TResult).Name} to {nameof(Result)}");
    
    return ValidationResult.WithErrors(errors, ErrorValidationTypeEnum.ValidationInput) as TResult 
           ?? throw new Exception($"Cannot convert {typeof(TResult).Name} to {nameof(Result)}");
    
    // var validationResult = typeof(ValidationResult<>)
    //   .GetGenericTypeDefinition()
    //   .MakeGenericType(typeof(TResult).GenericTypeArguments[0])
    //   .GetMethod(nameof(ValidationResult.WithErrors))!
    //   .Invoke(null, new object?[] { errors })!;
    //
    // return (TResult)validationResult;
  }
}