using FluentValidation;
using FluentValidation.Results;

namespace ACore.Models;

public class Error(string code, string message, Severity severity = Severity.Info, string? paramName = null, Dictionary<string, object>? formattedMessagePlaceholderValues = null)
  : IEquatable<Error>
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

  //public static implicit operator string(Error error) => error.Code;

  public static bool operator ==(Error? a, Error? b)
  {
    if (a is null && b is null)
    {
      return true;
    }

    if (a is null || b is null)
    {
      return false;
    }

    return a.Equals(b);
  }

  public static bool operator !=(Error? a, Error? b) => !(a == b);

  public bool Equals(Error? other)
  {
    if (other is null)
    {
      return false;
    }

    return Code == other.Code && Message == other.Message;
  }

  public override bool Equals(object? obj) => obj is Error error && Equals(error);

  public override int GetHashCode() => HashCode.Combine(Code, Message);

  public override string ToString() => Code;
}