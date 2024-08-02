using ACore.Blazor.Services.App;
using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;

namespace ACore.Blazor;

public abstract class JMComponentBase : ComponentBase
{
    [Inject]
    public IMediator Mediator { get; set; } = null!;

    [Inject]
    public IAppState AppState { get; set; } = null!;

    [Inject]
    public ILogger<JMComponentBase> Log { get; set; } = null!;
}