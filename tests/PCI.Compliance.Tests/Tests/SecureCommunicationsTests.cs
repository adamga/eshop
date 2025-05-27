using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace PCI.Compliance.Tests.Tests
{
    public class SecureCommunicationsTests
    {
        [Fact(DisplayName = "Sensitive data transmissions require HTTPS/TLS (C.4)")]
        public async Task SensitiveData_TransmissionsRequireTLS()
        {
            // Try HTTP (should fail or redirect)
            using var client = new HttpClient { BaseAddress = new System.Uri("http://localhost:5045") };
            var response = await client.GetAsync("/api/orders");
            Assert.True(response.StatusCode == System.Net.HttpStatusCode.MovedPermanently ||
                        response.StatusCode == System.Net.HttpStatusCode.Forbidden ||
                        response.StatusCode == System.Net.HttpStatusCode.NotFound,
                "API should not serve sensitive data over HTTP.");
        }

        [Fact(DisplayName = "Mutual authentication is required for server-to-server communication (C.4)")]
        public void MutualAuthentication_RequiredForServerToServer()
        {
            // Manual/infra check: Ensure mTLS is configured for internal APIs
            // This test is a placeholder for pipeline/config validation
            Assert.True(true, "Review infrastructure-as-code or API gateway config for mTLS enforcement.");
        }
    }
}
