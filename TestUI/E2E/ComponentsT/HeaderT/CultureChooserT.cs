using System.Reflection;
using FluentAssertions;
using JMCore.Blazor.Components.Header;
using JMCore.Blazor.Components.SideBar.RightSideBar;
using JMCoreTest.Blazor.Client;
using JMCoreTest.Blazor.E2E;
using OpenQA.Selenium;

namespace JMCoreTest.Blazor.EE.ComponentsT.HeaderT;

public class CultureChooserT : E2EBaseT
{
    [Fact]
    public async Task ChangeLanguage()
    {
        await RunE2EPageAsync(MethodBase.GetCurrentMethod(), AppPageList.Home, async (driver) =>
        {
            // Check if english set.
            var el = driver.FindElement(By.Id(nameof(CultureChooser)));
            var langEn = el.FindElement(By.ClassName("k-svg-i-flag-icons-us"));
            langEn.Should().NotBeNull();

            // Change language.
            el.Click();
            await Task.Delay(500);
            var cs = driver.FindElement(By.Id(nameof(CultureItems)));
            var cs2 = cs.FindElement(By.ClassName("k-svg-i-flag-icons-cz"));
            cs2.Click();
            // wait on reload
            await Task.Delay(5000);
            
            // Check if language cz
            el = driver.FindElement(By.Id(nameof(CultureChooser)));
            var langCs = el.FindElement(By.ClassName("k-svg-i-flag-icons-cz"));
            langCs.Should().NotBeNull();
        });
    }
}