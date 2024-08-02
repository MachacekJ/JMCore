using ACore.Localizer;
using ACore.Server.ResX;
using System.Resources;
using ResX_MainLayout = JMCoreTest.Blazor.Server.ResX.Resources.ResX_MainLayout;


namespace JMCoreTest.Blazor.Server.ResX;

//https://github.com/telerik/blazor-ui-messages/tree/main/Messages
public static class ResXInfrastructure
{
    public static List<ResXSource> RegisterResX()
    {
        return new List<ResXSource>
        {
            new(
                nameof(Client.ResX.ResX_MainLayout),
                new ResourceManager(typeof(ResX_MainLayout)),
                LocalizationScopeEnum.Client),
            new(
                nameof(ACore.Blazor.ResX.ResX_Telerik),
                new ResourceManager(typeof(Resources.TelerikMessages)),
                LocalizationScopeEnum.Client)
        };
    }
}