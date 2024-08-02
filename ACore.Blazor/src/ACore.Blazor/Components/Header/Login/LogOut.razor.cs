using ACore.Blazor.Services.App.Models;

namespace ACore.Blazor.Components.Header.Login
{
    public partial class LogOut : JMComponentBase, IDisposable
    {
        private string _mobileIconCss = string.Empty;

        protected override void OnInitialized()
        {
            AppState.OnResponsiveChange += AppStateOnOnResponsiveChange;
        }

        private Task AppStateOnOnResponsiveChange(ResponsiveTypeEnum type)
        {
            _mobileIconCss = type== ResponsiveTypeEnum.Desktop
                ? string.Empty
                : "jm-mobile-icon";
            StateHasChanged();
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            AppState.OnResponsiveChange -= AppStateOnOnResponsiveChange;
        }
    }
}