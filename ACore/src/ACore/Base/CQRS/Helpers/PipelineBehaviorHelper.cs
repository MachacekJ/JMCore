using ACore.Base.CQRS.Configuration;
using ACore.Base.CQRS.Models.Results;
using ACore.Base.CQRS.Models.Results.Validation;

namespace ACore.Base.CQRS.Helpers;

public class PipelineBehaviorHelper<TResponse>
  where TResponse : Result
{
  public bool CheckIfModuleIsActive(IModuleOptions moduleOptions, string whereIsModuleRegistered, out TResponse? result)
  {
    result = null;

    if (moduleOptions.IsActive)
      return true;

    result = CreateErrorExceptionResult<TResponse>(new Exception($"Module '{moduleOptions.ModuleName}' is not active. Add this module to {whereIsModuleRegistered}."));
    return false;
  }

  public static TResult CreateErrorValidationResult<TResult>(ValidationTypeEnum validationTypeEnum, ValidationError[] errors)
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

  public static TResult CreateErrorExceptionResult<TResult>(Exception exception)
    where TResult : Result
  {
    if (typeof(TResult) == typeof(Result))
      return ExceptionResult.WithException(exception) as TResult
             ?? throw new Exception($"Cannot convert {typeof(TResult).Name} to {nameof(Result)}");


    if (typeof(TResult).GetGenericTypeDefinition() != typeof(Result<>))
      throw new Exception($"Cannot convert {typeof(TResult).Name} to {nameof(Result)}");

    var exceptionResult = typeof(ExceptionResult<>)
      .GetGenericTypeDefinition()
      .MakeGenericType(typeof(TResult).GenericTypeArguments[0])
      .GetMethod(nameof(ExceptionResult<TResult>.WithException))?
      .Invoke(null, [exception]);

    if (exceptionResult == null)
      throw new Exception($"Cannot create generic error result {typeof(TResult).Name} generic {typeof(TResult).GenericTypeArguments[0].Name}");

    return (TResult)exceptionResult;
  }
}