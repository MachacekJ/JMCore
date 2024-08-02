using ACore.Localizer;
using ACore.Models.BaseRR;

namespace JMCoreTest.Blazor.Shared.Controllers.LocalizationController;

public class ClientLanguagePackAsyncResponse: ApiResponseBase
{
    public IEnumerable<LocalizationRecord> Data { get; set; } = null!;
}