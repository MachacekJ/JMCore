using JMCore.Blazor.CQRS.Analytics.Models;
using JMCore.Blazor.CQRS.LocalStorage;
using JMCore.Blazor.CQRS.LocalStorage.Models;
using MediatR;

namespace JMCore.Blazor.CQRS.Analytics;

public class WriteAnalyticsHandler(IMediator mediator) : IRequestHandler<WriteAnalyticsCommand>
{
    public async Task Handle(WriteAnalyticsCommand request, CancellationToken cancellationToken)
    {
        string analyticsName = Enum.GetName(typeof(AnalyticsTypeEnum), request.AnalyticsData.AnalyticsTypeEnum)!;
        var savedAnalyticsValues = await mediator.Send(new LocalStorageGetQuery(LocalStorageCategoryEnum.Analytics, analyticsName), cancellationToken);
        var analytics = new List<AnalyticsData>();
        if (savedAnalyticsValues.IsValue)
        {
            analytics = savedAnalyticsValues.GetValue<List<AnalyticsData>>() ?? [];
        }

        analytics.Add(request.AnalyticsData);

        if (analytics.Count > 10)
        {
            // TODO send to server
            analytics.Clear();
        }

        await mediator.Send(new LocalStorageSaveCommand(LocalStorageCategoryEnum.Analytics, analyticsName, analytics, analytics.GetType()), cancellationToken);
    }
}