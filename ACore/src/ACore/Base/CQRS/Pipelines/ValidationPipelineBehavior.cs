using ACore.Base.CQRS.Helpers;
using ACore.Base.CQRS.Models;
using ACore.Base.CQRS.Models.Validation;
using FluentValidation;
using MediatR;

namespace ACore.Base.CQRS.Pipelines;

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
      return PipelineBehaviorHelper<TResponse>.CreateErrorValidationResult<TResponse>(ErrorValidationTypeEnum.ValidationInput, errors);
    }
    
    return await next();
  }
}