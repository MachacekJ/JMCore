using ACore.Models.BaseRR;
using JMCoreTest.Blazor.Shared.Controllers.HttpTestController.Models;
using JMCoreTest.Blazor.Shared.Models;

namespace JMCoreTest.Blazor.Shared.Controllers.HttpTestController;

public class HttpPostWeatherTestResponse: ApiResponseBase
{

    public DataEnvelope<WeatherForecast> Res { get; set; }
}