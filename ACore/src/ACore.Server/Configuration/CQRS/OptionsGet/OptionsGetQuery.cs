using ACore.Base.CQRS.Models.Results;
using MediatR;

namespace ACore.Server.Configuration.CQRS.OptionsGet;

public class OptionsGetQuery : IRequest<Result<ACoreServerOptions>>;