using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace ACore.Server.Configuration;

public static class ACoreServerExtensions
{
  public static void AddCQRS(this IServiceCollection services)
  {
    services.AddMediatR((c) =>
    {
      c.RegisterServicesFromAssemblyContaining(typeof(ACoreServerExtensions));
    });

    services.AddValidatorsFromAssembly(typeof(ACoreServerExtensions).Assembly,
      includeInternalTypes: true);

  }
}