using Microsoft.Extensions.Configuration;

namespace ACore.Server.Services.Email.MailJet;

public class MailJetOptions
{
    private const string AppSettingsNodeName = "MailJetSettings";
    public string PublicKey { get; private init; } = null!;
    public string PrivateKey { get; private init; } = null!;
    public string Email { get; private init; } = null!;
    public string SMS { get; private init; } = null!;

    public static MailJetOptions CreateFromConfig(IConfiguration configuration)
    {
        return new MailJetOptions()
        {
            PublicKey = configuration[$"{AppSettingsNodeName}:PublicKey"] ?? throw new InvalidOperationException(),
            PrivateKey = configuration[$"{AppSettingsNodeName}:PrivateKey"] ?? throw new InvalidOperationException(),
            Email = configuration[$"{AppSettingsNodeName}:Email"] ?? throw new InvalidOperationException(),
            SMS = configuration[$"{AppSettingsNodeName}:SMS"] ?? throw new InvalidOperationException()
        };
    }
}