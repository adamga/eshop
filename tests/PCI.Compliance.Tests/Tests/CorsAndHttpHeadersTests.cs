using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace PCI.Compliance.Tests.Tests
{
    public class CorsAndHttpHeadersTests
    {
        [Fact(DisplayName = "CORS policy is secure (C.3.6)")]
        public async Task CorsPolicy_IsSecure()
        {
            using var client = new HttpClient { BaseAddress = new System.Uri("https://localhost:7298") };
            var request = new HttpRequestMessage(HttpMethod.Options, "/api/catalog");
            request.Headers.Add("Origin", "http://evil.com");
            request.Headers.Add("Access-Control-Request-Method", "GET");
            var response = await client.SendAsync(request);
            Assert.DoesNotContain("http://evil.com", response.Headers.ToString());
        }

        [Fact(DisplayName = "HTTP security headers are present (C.3.1)")]
        public async Task HttpSecurityHeaders_ArePresent()
        {
            using var client = new HttpClient { BaseAddress = new System.Uri("https://localhost:7298") };
            var response = await client.GetAsync("/");
            Assert.True(response.Headers.Contains("Strict-Transport-Security"), "HSTS header missing");
            Assert.True(response.Headers.Contains("Content-Security-Policy") || response.Headers.Contains("X-Content-Security-Policy"), "CSP header missing");
        }
    }
}
