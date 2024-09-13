using FluentValidation;
using FluentValidation.Results;

namespace ACore.Base.CQRS.Models;

public class Error(string code, string message, Severity severity = Severity.Info, string? paramName = null, Dictionary<string, object>? formattedMessagePlaceholderValues = null)
{
  public static readonly Error None = new(string.Empty, string.Empty);
  
  public string? ParamName { get; } = paramName;

  public Severity Severity { get; } = severity;

  public string Code { get; } = code;

  public string Message { get; } = message;

  public Dictionary<string,object>? FormattedMessagePlaceholderValues { get; } = formattedMessagePlaceholderValues;

  public static Error Create(ValidationFailure vf)
  {
    return new Error(vf.ErrorCode, vf.ErrorMessage, vf.Severity, vf.PropertyName, vf.FormattedMessagePlaceholderValues);
  }
  
  public override string ToString() => Code;
}