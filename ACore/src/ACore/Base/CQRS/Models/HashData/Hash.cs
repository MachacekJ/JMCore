namespace ACore.Base.CQRS.Models.HashData;

public class HashData
{
  public string? HashToCheck { get; private set; }

  public void SetHashToCheck(string? hashToCheck)
  {
    HashToCheck = hashToCheck;
  }
}