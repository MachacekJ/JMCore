using ACore.Base;
using ACore.Models;
using FluentValidation;
using MediatR;

namespace ACore.CQRS;

public class ValidationPipelineBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators) : IPipelineBehavior<TRequest, TResponse>
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
      .Select(Error.Create)
      .ToArray();

    if (errors.Length != 0)
    {
      return BehaviorHelper<TResponse>.CreateErrorValidationResult<TResponse>(ErrorValidationTypeEnum.ValidationInput, errors);
    }
    
    return await next();
  }
}