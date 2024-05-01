using JMCore.Blazor;
using Telerik.Blazor.Components;

namespace JMCoreTest.Blazor.Client.Pages;

public partial class Index : JMPageBase
{
    private TelerikTextBox txt;
    private string aa;
    private void Click()
    {
        aa = txt.Value;
    }
}