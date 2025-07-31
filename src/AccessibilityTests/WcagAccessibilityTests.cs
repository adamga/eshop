/**
 * WcagAccessibilityTests.cs
 * 
 * Test Purpose:
 * This test class validates Web Content Accessibility Guidelines (WCAG) compliance for the eShop
 * web application. It performs automated accessibility testing to ensure the application meets
 * accessibility standards and is usable by people with disabilities.
 * 
 * How the Test Works:
 * 1. Sets up a headless Chrome browser using Selenium WebDriver for automated testing
 * 2. Navigates to the application's base URL (configurable via _baseUrl field)
 * 3. Contains accessibility validation tests such as:
 *    - Images_Should_Have_Alt_Text(): Finds all <img> elements and validates that each has
 *      proper alt text attributes for screen reader compatibility
 * 4. Uses xUnit framework for test assertions and structure
 * 5. Implements IDisposable to properly clean up WebDriver resources after tests complete
 * 
 * This test class is essential for ensuring the eShop application is accessible to users with
 * disabilities and complies with WCAG guidelines, which may be required for legal compliance
 * and inclusive user experience.
 */
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
