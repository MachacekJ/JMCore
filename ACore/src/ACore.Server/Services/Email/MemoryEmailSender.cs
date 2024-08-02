using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Logging;
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace ACore.Server.Services.Email;

public class MemoryEmailSender : IEmailSenderJM
{
    public class EmailTest
    {
        public string Email { get; set; } = null!;
        public string Subject { get; set; } = null!;
        public string HtmlMessage { get; set; } = null!;
    }

    private readonly ILogger<IEmailSender> _logger;

    public List<EmailTest> AllEmails { get; } = new();

    public MemoryEmailSender(ILogger<IEmailSender> logger)
    {
        _logger = logger;
    }

    public Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        var mailFake = new EmailTest()
        {
            Email = email,
            Subject = subject,
            HtmlMessage = htmlMessage
        };
        AllEmails.Add(mailFake);
        _logger.LogInformation($"MailMock: {Newtonsoft.Json.JsonConvert.SerializeObject(mailFake)}");
        return Task.CompletedTask;
    }
}