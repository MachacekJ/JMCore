using Microsoft.AspNetCore.Components.Web;
using Telerik.Blazor.Components;
using Telerik.SvgIcons;

namespace JMCore.Blazor.Components.Header.Login;

public partial class UserInfo : JMComponentBase
{
    private TelerikContextMenu<MenuItem>? _contextMenuRef;
    private TelerikButton? _nameItemRef;
    private bool _isShown = false;
    private string _fullName;
    private string _displayName;
    
    private class MenuItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ISvgIcon Icon { get; set; }
    }

    private static IEnumerable<MenuItem> MenuItems { get; set; } = new List<MenuItem>
    {
        new()
        {
            Id = 0,
            Name = "Account",
            Icon = SvgIcon.User
        },
        new()
        {
            Id = 1,
            Name = "Settings",
            Icon = SvgIcon.Gear
        }
    };




    private async Task ToggleContextMenu(MouseEventArgs e)
    {
        if (_isShown)
            await _contextMenuRef!.HideAsync();
        else
            await _contextMenuRef!.ShowAsync(e.ClientX, e.ClientY + 15);
        _isShown = !_isShown;
    }

    private async Task MenuClick(MenuItem item)
    {
        _isShown = true;
        await ToggleContextMenu(new MouseEventArgs());
    }

}