using ACore.Configuration;
using ACore.Models;
using FluentValidation;

namespace ACore.Base;

public class BehaviorHelper<TResponse>
  where TResponse : Result
{
  public bool CheckIfModuleIsActive(IModuleOptions moduleOptions, string whereIsModuleRegistered, out TResponse? result)
  {
    result = null;

    if (moduleOptions.IsActive)
      return true;

    result = CreateErrorResult<TResponse>(new Error(ErrorTypes.ModuleActivation, $"Module '{nameof(moduleOptions.ModuleName)}' is not active. Add this module to {whereIsModuleRegistered}.", Severity.Error));
    return false;
  }

  public static TResult CreateErrorValidationResult<TResult>(ErrorValidationTypeEnum validationTypeEnum, Error[] errors)
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
  
  public static TResult CreateErrorResult<TResult>(Error error)
    where TResult : Result
  {
    if (typeof(TResult) == typeof(Result))
      return Result.Failure(error) as TResult
             ?? throw new Exception($"Cannot convert {typeof(TResult).Name} to {nameof(Result)}");
    
    if (typeof(TResult).GetGenericTypeDefinition() != typeof(Result<>)) 
      throw new Exception($"Cannot convert {typeof(TResult).Name} to {nameof(Result)}");
    
    var result = typeof(Result<>)
      .GetGenericTypeDefinition()
      .MakeGenericType(typeof(TResult).GenericTypeArguments[0])
      .GetMethod(nameof(Result<TResult>.Failure))?
      .Invoke(null, [error]);

    if (result == null)
      throw new Exception($"Cannot create generic error result {typeof(TResult).Name} generic {typeof(TResult).GenericTypeArguments[0].Name}");

    return (TResult)result;
    
  }
}