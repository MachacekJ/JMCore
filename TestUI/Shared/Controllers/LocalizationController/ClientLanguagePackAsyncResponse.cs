using JMCore.Localizer;
using JMCore.Models.BaseRR;

namespace JMCoreTest.Blazor.Shared.Controllers.LocalizationController;

public class ClientLanguagePackAsyncResponse: ApiResponseBase
{
    public IEnumerable<LocalizationRecord> Data { get; set; } = null!;
}