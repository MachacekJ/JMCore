// ReSharper disable UnusedAutoPropertyAccessor.Global
namespace JMCore.Server.Services.SMS;

public class MemorySMSSender : ISMSSenderJM
{
    public class SMSData
    {
        public string Message { get; set; } = null!;
        public string From { get; set; } = null!;
        public string To { get; set; } = null!;
    }
    
    public List<SMSData> AllSMS { get; set; } = new();

    public async Task SendSMSAsync(string from, string to, string message)
    {
        AllSMS.Add(new SMSData()
        {
            From = from,
            To = to,
            Message = message
        });
        await Task.Delay(0);
    }
}