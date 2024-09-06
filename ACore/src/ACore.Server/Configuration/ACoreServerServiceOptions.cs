using ACore.Configuration;

namespace ACore.Server.Configuration;

public class ACoreServerServiceOptions
{
  public ACoreServiceOptions ACoreServiceOptions { get; set; } = new ACoreServiceOptions();

  public string ServerName { get; set; } = "ServefName";
}