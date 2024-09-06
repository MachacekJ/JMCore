namespace ACore.Configuration;

public class ACoreServiceOptionBuilder
{
  private string _name = string.Empty;
  private ACoreServiceOptionBuilder()
  {
  }

  public static ACoreServiceOptionBuilder Empty() => new();

  public ACoreServiceOptionBuilder Name(string name)
  {
    _name = name;
    return this;
  }

  public ACoreServiceOptions Build()
  {
    return new ACoreServiceOptions()
    {
      Name = _name
    };
  }

}