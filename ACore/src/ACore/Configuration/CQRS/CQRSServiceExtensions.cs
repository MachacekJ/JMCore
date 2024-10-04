using ACore.Base.CQRS;
using ACore.Base.CQRS.Pipelines;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace ACore.Configuration.CQRS;

public static class CQRSExtensions
{
  public static void AddCQRS(this IServiceCollection services)
  {
    services.AddMediatR((c) =>
    {
      c.RegisterServicesFromAssemblyContaining(typeof(CQRSExtensions));
      c.ACoreMediatorConfiguration();
    });
    services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingPipelineBehavior<,>));
    services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>));
    services.AddValidatorsFromAssembly(typeof(CQRSExtensions).Assembly, includeInternalTypes: true);

  }
}