using JMCore.Blazor.CQRS.Analytics.Models;
using MediatR;

namespace JMCore.Blazor.CQRS.Analytics;

public class WriteAnalyticsCommand(AnalyticsData analyticsData) : IRequest
{
    public AnalyticsData AnalyticsData { get; } = analyticsData;
}