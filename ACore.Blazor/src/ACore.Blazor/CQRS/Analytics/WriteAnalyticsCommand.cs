using ACore.Blazor.CQRS.Analytics.Models;
using MediatR;

namespace ACore.Blazor.CQRS.Analytics;

public class WriteAnalyticsCommand(AnalyticsData analyticsData) : IRequest
{
    public AnalyticsData AnalyticsData { get; } = analyticsData;
}