using JMCore.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using JMCore.Models.BaseRR;

// ReSharper disable UnusedTypeParameter

namespace JMCore.Server.Controllers;

public abstract class BaseController<T> : Controller where T : BaseController<T>
{
  private readonly ILogger<T> _logger;

  protected BaseController(ILogger<T> logger)
  {
    _logger = logger;
  }

  public string ControllerName(string controller)
    => controller.Replace("Controller", string.Empty);

  protected async Task RunInCatch<TApiResponse>(ApiResponseBase res, Func<Task> testCode)
    where TApiResponse : ApiResponseBase
  {
    try
    {
      await testCode();
    }
    catch (Exception ex)
    {
      res.ServerErrorId = Guid.NewGuid();
      _logger.LogError("ErrorId:{DtoErrorId}->ControllerBase:{MessageRecur}", res.ServerErrorId,
        ex.MessageRecur());
    }
  }
}