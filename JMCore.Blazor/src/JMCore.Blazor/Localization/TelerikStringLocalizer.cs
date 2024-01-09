using JMCore.Blazor.ResX;
using Microsoft.Extensions.Localization;
using Telerik.Blazor.Services;

namespace JMCore.Blazor.Localization
{
    public class JMTelerikStringLocalizer : ITelerikStringLocalizer
    {
        private readonly IStringLocalizer<ResX_Telerik> _localizer;

        public JMTelerikStringLocalizer(IStringLocalizer<ResX_Telerik> localizer)
        {
            _localizer = localizer;
            //  localizer.ContextId = nameof(ResX_Telerik);
        }

        public string this[string name] => _localizer.GetString(name);
    }
}