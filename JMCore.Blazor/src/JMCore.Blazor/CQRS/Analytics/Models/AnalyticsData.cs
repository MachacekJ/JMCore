namespace JMCore.Blazor.CQRS.Analytics.Models;

public class AnalyticsData
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public AnalyticsTypeEnum AnalyticsTypeEnum { get; set; }
    public string Name { get; set; } = null!;
    public string Value { get; set; } = null!;
    
    public DateTime Created { get; set; } = DateTime.Now.ToUniversalTime();
}