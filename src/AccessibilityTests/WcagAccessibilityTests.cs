using System;
using Xunit;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace AccessibilityTests
{
    public class WcagAccessibilityTests : IDisposable
    {
        private readonly IWebDriver _driver;
        private readonly string _baseUrl = "http://localhost:5000"; // Change to your app's URL

        public WcagAccessibilityTests()
        {
            var options = new ChromeOptions();
            options.AddArgument("--headless");
            _driver = new ChromeDriver(options);
        }

        [Fact]
        public void Images_Should_Have_Alt_Text()
        {
            _driver.Navigate().GoToUrl(_baseUrl);
            var images = _driver.FindElements(By.TagName("img"));
            foreach (var img in images)
            {
                var alt = img.GetAttribute("alt");
                Assert.False(string.IsNullOrEmpty(alt), $"Image missing alt text: {img.GetAttribute("src")}");
            }
        }

        public void Dispose()
        {
            _driver?.Quit();
        }
    }
}
