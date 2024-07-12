namespace JMCore.Server.ResX;

public class ResXLocalizationOptions
{
    public List<int> SupportedCultures { get; set; } = [1033];
    public List<ResXSource> OtherResourceManager { get; set; } = [];
}