namespace JMCore.Server.ResX;

public class ResXLocalizationOptions
{
    public List<int> SupportedCultures { get; set; } = new() { 1033 };
    public List<ResXManagerInfo> OtherResourceManager { get; set; } = new();
}