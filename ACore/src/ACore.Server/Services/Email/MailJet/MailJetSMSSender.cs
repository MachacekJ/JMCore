using ACore.Server.Services.SMS;
using Mailjet.Client;
using Mailjet.Client.Resources.SMS;
using Microsoft.Extensions.Options;

namespace ACore.Server.Services.Email.MailJet;

// ReSharper disable once InconsistentNaming
public class MailJetSMSSender : ISMSSenderJM
{
    private readonly MailJetOptions _mailJetOptions;

    public MailJetSMSSender(IOptions<MailJetOptions> mailjetSettings)
    {
        _mailJetOptions = mailjetSettings.Value;
    }

    public async Task SendSMSAsync(string from, string to, string message)
    {
        MailjetClient client = new MailjetClient(_mailJetOptions.SMS);

        MailjetRequest request = new MailjetRequest
            {
                Resource = Send.Resource,

            }
            .Property(Send.From, from)
            .Property(Send.To, to)
            .Property(Send.Text, message);

        MailjetResponse response = await client.PostAsync(request);
        if (response.IsSuccessStatusCode)
        {
            Console.WriteLine($@"Total: {response.GetTotal()}, Count: {response.GetCount()}");
            Console.WriteLine(response.GetData());
        }
        else
        {
            Console.WriteLine($@"StatusCode: {response.StatusCode}");
            Console.WriteLine($@"ErrorInfo: {response.GetErrorInfo()}");
            Console.WriteLine(response.GetData());
            Console.WriteLine($@"ErrorMessage: {response.GetErrorMessage()}");
        }
    }


}