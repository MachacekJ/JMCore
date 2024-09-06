using ACore.CQRS;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace ACore.Configuration;

public static class ACoreServiceExtension
{
  // public static void AddACore(this IServiceCollection services, Action<ACoreServiceOptionBuilder>? options = null)
  // {
  //   var opt = ACoreServiceOptionBuilder.Empty();
  //   options?.Invoke(opt);
  //   AddACore(services, opt.Build());
  // }

  public static void AddACore(this IServiceCollection services, ACoreServiceOptions opt)
  {
    var myOptionsInstance = Options.Create(opt);
    services.AddSingleton(myOptionsInstance);
    
    services.AddCQRS();
  }
}