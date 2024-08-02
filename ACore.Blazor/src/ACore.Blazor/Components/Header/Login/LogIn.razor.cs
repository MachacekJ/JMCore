using ACore.Blazor.Services.App.Models;
using Microsoft.AspNetCore.Components;

namespace ACore.Blazor.Components.Header.Login
{
    public partial class LogIn : JMComponentBase, IDisposable
    {
        private string _mobileIconCss = string.Empty;

        [Inject] private NavigationManager Navigation { get; set; } = null!;

        protected override void OnInitialized()
        {
            AppState.OnResponsiveChange += AppStateOnOnResponsiveChange;
        }

        private Task AppStateOnOnResponsiveChange(ResponsiveTypeEnum type)
        {
            _mobileIconCss = type == ResponsiveTypeEnum.Desktop
                ? string.Empty
                : "jm-mobile-icon";
            StateHasChanged();
            return Task.CompletedTask;
        }
        
        private void LoginClick()
        {
            Navigation.NavigateTo("api/Account/login", true);
        }
        
        public void Dispose()
        {
            AppState.OnResponsiveChange -= AppStateOnOnResponsiveChange;
        }
    }
}