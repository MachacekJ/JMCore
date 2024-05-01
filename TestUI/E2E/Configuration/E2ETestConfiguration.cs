// ReSharper disable ClassNeverInstantiated.Global
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
namespace JMCoreTest.Blazor.EE.Configuration;

public class E2ETestConfiguration
{
    public string Url { get; set; }
    public string ChromeDriverPath { get; set; }
    public string? User { get; set; }
}