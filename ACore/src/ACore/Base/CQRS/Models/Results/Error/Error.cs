namespace ACore.Base.CQRS.Models.Results.Error;

public partial class Error
{
  private Error(string code, string message)
  {
    Code = code;
    Message = message;
  }

  public string Code { get; }

  public string Message { get; }

  public override string ToString() => $"Code:{Code};Message:{Message}";
}