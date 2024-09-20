using ACore.Server.Modules.ICAMModule.CQRS;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ACore.Server.Modules.ICAMModule.Configuration;

public static class ICAMModuleServiceExtensions
{
  public static void AddICAMModule(this IServiceCollection services, ICAMModuleOptions options)
  {
    services.TryAddTransient(typeof(IPipelineBehavior<,>), typeof(ICAMModulePipelineBehavior<,>));
  }
}