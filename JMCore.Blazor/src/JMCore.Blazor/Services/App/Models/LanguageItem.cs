using Telerik.SvgIcons;

namespace JMCore.Blazor.Services.App.Models;
public class LanguageItem(int lcid, string name, string text, ISvgIcon icon)
{
    public string Id => name;
    public int LCID => lcid;
    public ISvgIcon Icon => icon;
    public string Text => text;
    

}
