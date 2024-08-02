using ACore.Blazor.Services.App;
using ACore.Blazor.Services.App.Models;
using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;

namespace ACore.Blazor.Components;

public partial class TelerikLayout : LayoutComponentBase
{
    [Inject]
    private IAppStartConfiguration AppSettings { get; set; } = null!;

    [Inject]
    private IAppState AppState { get; set; } = null!;

    [Inject]
    private IJSRuntime JsRuntime { get; set; } = null!;

    [Inject]
    private ILogger<TelerikLayout> Logger { get; set; } = null!;

    [Inject]
    public IMediator Mediator { get; set; } = null!;

    private string PageTitle => AppSettings.AppName;

    protected override void OnAfterRender(bool firstRender)
    {
        AppState.SetPageState(PageStateEnum.Rendered);
    }
}

