using System.Reflection;
using ACore.Blazor.Services.Page.Interfaces;
using ACore.Tests;
using ACore.Tests.BaseInfrastructure;
using JMCoreTest.Blazor.EE.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace JMCoreTest.Blazor.EE;

//https://www.softwaretestinghelp.com/file-upload-in-selenium/
//https://www.selenium.dev/documentation/webdriver/getting_started/install_library/
public class E2EBaseTests : BaseTests
{
    private E2ETestConfiguration E2ETestConfiguration { get; set; } = null!;

    protected async Task RunE2EPageAsync(MemberInfo? method, IPageData page, Func<IWebDriver, Task> testCode)
    {
        await RunE2EAsync(method, page.PageId, testCode);
    }
    
    private async Task RunE2EAsync(MemberInfo? method, string relativeUrl, Func<IWebDriver, Task> testCode)
    {
        var chromeOptions = new ChromeOptions();
        chromeOptions.AddArguments("headless");
        //  chromeOptions.DebuggerAddress = "https://localhost:7072/";
        //  chromeOptions.BinaryLocation = Environment.CurrentDirectory;
        ChromeDriver? chromeDriver = null;
        try
        {
            await RunTestAsync(method, async () =>
            {
                var url = E2ETestConfiguration.Url + relativeUrl;
                chromeDriver = new ChromeDriver(Path.Combine(RootDir, E2ETestConfiguration.ChromeDriverPath)); //,chromeOptions);
                if (chromeDriver == null)
                    throw new Exception("WebBrowser cannot be loaded.");

                chromeDriver.Navigate().GoToUrl(url);
                chromeDriver.Manage().Window.Maximize();
                chromeDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(20);
                await testCode(chromeDriver);
            });
        }
        finally
        {
            if (chromeDriver != null)
            {
                chromeDriver.Close();
                chromeDriver.Dispose();
            }
        }
    }

    protected override async Task GetServicesAsync(IServiceProvider sp)
    {
        await base.GetServicesAsync(sp);
        var config = sp.GetService<IConfiguration>();
        var e2ETestConfiguration = new E2ETestConfiguration();
        config?.GetSection("E2E").Bind(e2ETestConfiguration);
        E2ETestConfiguration = e2ETestConfiguration ?? throw new Exception("E2E section missing in appsettings.Test.json");
    }
}