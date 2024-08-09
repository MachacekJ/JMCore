namespace ACore.Base.CQRS.Results.Models;

public class ResultErrorItem(string code, string message)
{
  public static readonly ResultErrorItem None = new(string.Empty, string.Empty);

  public string Code { get; } = code;

  public string Message { get; } = message;

  public override string ToString() => $"Code:{Code};Message:{Message}";
}