using ACore.Base;

namespace ACore.Models;

public static class ErrorTypes
{
//  public const string ValidationInput = "ValidationInput";
  // public const string ValidationBusiness = "ValidationBusiness";
   public const string ModuleActivation = "ModuleActivation";

  public static readonly Error ErrorInternalServer = new(
    "InternalServer",
    "Internal server error.");
  
  public static readonly Error ErrorValidationInput = new(
    "ValidationInput",
    "A validation problem occurred.");

  public static readonly Error ErrorValidationBusiness = new(
    "ValidationBusiness",
    "A business validation problem occurred.");
}

public enum ErrorValidationTypeEnum
{
  ValidationInput,
  ValidationBusiness
}

public static class ErrorValidationTypeEnumExtensions
{
  public static Error ToError(this ErrorValidationTypeEnum errorValidationType)
  {
    return errorValidationType switch
    {
      ErrorValidationTypeEnum.ValidationBusiness => ErrorTypes.ErrorValidationBusiness,
      ErrorValidationTypeEnum.ValidationInput => ErrorTypes.ErrorValidationInput,
      //  ErrorValidationTypeEnum.ModuleActivation => ErrorValidationTypeCodes.ErrorModuleActivation,
      _ => throw new ArgumentOutOfRangeException(nameof(errorValidationType), errorValidationType, null)
    };
  }
}

public class ExceptionResult : Result
{
  public Exception Exception { get; }
  private ExceptionResult(Exception exception) : base(false, ErrorTypes.ErrorInternalServer)
  {
    Exception = exception;
  }
  public static ExceptionResult WithException(Exception exception) => new(exception);
}

public class ExceptionResult<TValue> : Result<TValue>
{
  public Exception Exception { get; }
  private ExceptionResult(Exception exception)
    : base(default, false, ErrorTypes.ErrorInternalServer) =>
    Exception = exception;
  
  /// <summary>
  /// This function is used in <see cref="BehaviorHelper{TResponse}"/>, don't remove it.
  /// </summary>
  public static ExceptionResult<TValue> WithException(Exception exception) => new(exception);
}

public class ValidationResult : Result
{
  public Error[] ValidationErrors { get; }

  private ValidationResult(ErrorValidationTypeEnum errorValidationType, Error[] validationErrors)
    : base(false, errorValidationType.ToError())
  {
    ValidationErrors = validationErrors;
  }

  public static ValidationResult WithErrors(ErrorValidationTypeEnum errorValidationType, Error[] errors) => new(errorValidationType, errors);
}

public class ValidationResult<TValue> : Result<TValue>
{
  private ValidationResult(ErrorValidationTypeEnum errorValidationType, Error[] validationErrors)
    : base(default, false, errorValidationType.ToError()) =>
    ValidationErrors = validationErrors;

  public Error[] ValidationErrors { get; }

  /// <summary>
  /// This function is used in <see cref="BehaviorHelper{TResponse}"/>, don't remove it.
  /// </summary>
  public static ValidationResult<TValue> WithErrors(ErrorValidationTypeEnum errorValidationType, Error[] errors) => new(errorValidationType, errors);
}