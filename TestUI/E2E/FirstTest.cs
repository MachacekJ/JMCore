// using System.Reflection;
// using FluentAssertions;
// using OpenQA.Selenium;
//
// namespace JMCoreTest.Blazor.E2E;
//
// public class FirstTest : E2EBaseT
// {
//     [Theory]
//     [MemberData(nameof(Data))]
//     public async Task Test1(string value1, string value2, string expected)
//     {
//         await RunE2ETestAsync(MethodBase.GetCurrentMethod(), async (driver) =>
//         {
//             var el = driver.FindElement(By.ClassName("txt1"));
//             el.FindElement(By.TagName("input")).SendKeys(value1 + value2);
//             await Task.Delay(500);
//             driver.FindElement(By.ClassName("btn1")).Click();
//             var aa = driver.FindElement(By.XPath("//*[@testid='aa']")).FindElement(By.TagName("div")).Text;
//
//             aa.Should().Match(expected);
//         });
//     }
//
//     public static IEnumerable<object[]> Data =>
//         new List<object[]>
//         {
//             new object[] { "ahoj", "2", "ahoj2" },
//         };
// }