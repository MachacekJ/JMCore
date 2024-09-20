namespace ACore.Base.CQRS.Models.Results.Error;

public partial class Error
{
  public static readonly Error None = new(string.Empty, string.Empty);
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