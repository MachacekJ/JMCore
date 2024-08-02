namespace ACore.Server.Services.SMS;

// ReSharper disable once InconsistentNaming
public interface ISMSSenderJM
{
    // ReSharper disable once InconsistentNaming
    public Task SendSMSAsync(string from, string to, string message);
}