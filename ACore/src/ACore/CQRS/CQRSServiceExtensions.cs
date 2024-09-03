using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace ACore.CQRS;

public static class CQRSExtensions
{
  public static void AddCQRS(this IServiceCollection services)
  {
    services.AddMediatR((c) =>
    {
      c.RegisterServicesFromAssemblyContaining(typeof(CQRSExtensions));
    });
    services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingPipelineBehavior<,>));
    services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>));
    services.AddValidatorsFromAssembly(ACore.AssemblyReference.Assembly,
      includeInternalTypes: true);

  }
}