using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace JMCore.CQRS;

public static class CQRSExtensions
{
  public static void AddCQRS(this IServiceCollection services)
  {
    services.AddMediatR((c) =>
    {
      c.RegisterServicesFromAssemblyContaining(typeof(CQRSExtensions));
    });
    services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
  }
}