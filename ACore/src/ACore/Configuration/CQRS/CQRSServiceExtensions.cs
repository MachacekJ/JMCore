using ACore.Base.CQRS;
using ACore.Base.CQRS.Extensions;
using ACore.Base.CQRS.Notifications;
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
      c.AllNotificationWithoutException();
    });
    services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingPipelineBehavior<,>));
    services.AddTransient(typeof(IPipelineBehavior<,>), typeof(FluentValidationPipelineBehavior<,>));
    services.AddValidatorsFromAssembly(typeof(CQRSExtensions).Assembly, includeInternalTypes: true);

  }
}