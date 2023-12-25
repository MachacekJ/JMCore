using JMCore.Localizer;
using JMCore.Server.ResX;
using System.Resources;
using ResX_MainLayout = JMCoreTest.Blazor.Server.ResX.Resources.ResX_MainLayout;


namespace JMCoreTest.Blazor.Server.ResX;

//https://github.com/telerik/blazor-ui-messages/tree/main/Messages
public static class ResXInfrastructure
{
    public static List<ResXManagerInfo> RegisterResX()
    {
        return new List<ResXManagerInfo>
        {
            new(
                nameof(Client.ResX.ResX_MainLayout),
                new ResourceManager(typeof(ResX_MainLayout)),
                LocalizationScopeEnum.Client),
            new(
                nameof(JMCore.Blazor.ResX.ResX_Telerik),
                new ResourceManager(typeof(Resources.TelerikMessages)),
                LocalizationScopeEnum.Client)
        };
    }
}