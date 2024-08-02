using ACore.Models.BaseRR;
using Telerik.DataSource;

namespace JMCoreTest.Blazor.Shared.Controllers.HttpTestController;

public class HttpPostTestRequest : ApiRequestBase
{
    public static readonly int TooManyItems = -1000;

    public DataSourceRequest Req { get; set; }
}