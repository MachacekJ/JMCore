using ACore.Base.CQRS.Models.Results;
using MediatR;
using Microsoft.Extensions.Options;

namespace ACore.Server.Configuration.CQRS.OptionsGet;

public class OptionsGetHandler(IOptions<ACoreServerOptions> options): IRequestHandler<OptionsGetQuery, Result<ACoreServerOptions>>
{
  public Task<Result<ACoreServerOptions>> Handle(OptionsGetQuery request, CancellationToken cancellationToken)
  {
    return Task.FromResult(Result.Success(options.Value));
  }
}