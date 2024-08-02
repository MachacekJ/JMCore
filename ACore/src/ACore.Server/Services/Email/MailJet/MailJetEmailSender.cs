using Mailjet.Client;
using Mailjet.Client.TransactionalEmails;
using Microsoft.Extensions.Options;

namespace ACore.Server.Services.Email.MailJet;

public class MailJetEmailSender : IEmailSenderJM
{
    private readonly MailJetOptions _mailJetOptions;

    public MailJetEmailSender(IOptions<MailJetOptions> mailjetSettings)
    {
        _mailJetOptions = mailjetSettings.Value;
    }

    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        MailjetClient client = new MailjetClient(_mailJetOptions.PublicKey,
            _mailJetOptions.PrivateKey)
        {
            //Version = ApiVersion.V3_1,

        };
        //MailjetRequest request = new MailjetRequest
        //{
        //    Resource = Send.Resource,
        //};

        // construct your email with builder
        var emaill = new TransactionalEmailBuilder()
            .WithFrom(new SendContact(_mailJetOptions.Email))
            .WithSubject(subject)
            .WithHtmlPart(htmlMessage)
            .WithTo(new SendContact(email))
            .Build();

        // invoke API to send email  var response = 
        await client.SendTransactionalEmailAsync(emaill);
    }
}