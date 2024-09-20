using FluentValidation;
using FluentValidation.Results;

namespace ACore.Base.CQRS.Models.Results.Validation;

public class ValidationError
{
  public string? ParamName { get; }

  public Severity Severity { get; }

  public string Code { get; }

  public string Message { get; }

  public Dictionary<string, object>? FormattedMessagePlaceholderValues { get; }
  
  private ValidationError(string code, string message, Severity severity = Severity.Info, string? paramName = null, Dictionary<string, object>? formattedMessagePlaceholderValues = null)
  {
    ParamName = paramName;
    Severity = severity;
    Code = code;
    Message = message;
    FormattedMessagePlaceholderValues = formattedMessagePlaceholderValues;
  }
  
  public static ValidationError Create(ValidationFailure vf)
  {
    return new ValidationError(vf.ErrorCode, vf.ErrorMessage, vf.Severity, vf.PropertyName, vf.FormattedMessagePlaceholderValues);
  }
}