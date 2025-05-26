using System.IO;
using System.Linq;
using System.Text.Json;
using Xunit;

namespace Compliance.UnitTests
{
    public class SensitiveDataTests
    {
        [Fact]
        public void PaymentData_ShouldNotBeStoredInPlaintext()
        {
            // Example: Check that payment data is not stored in plaintext in Basket.API
            var files = Directory.GetFiles("../../../../src/Basket.API/", "*.json", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                var content = File.ReadAllText(file);
                Assert.DoesNotContain("cardNumber", content, ignoreCase: true);
                Assert.DoesNotContain("cvv", content, ignoreCase: true);
            }
        }

        [Fact]
        public void AppSettings_ShouldNotContainSecrets()
        {
            // Example: Check that appsettings files do not contain secrets in plaintext
            var files = Directory.GetFiles("../../../../src/", "appsettings*.json", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                var content = File.ReadAllText(file);
                Assert.DoesNotContain("password", content, ignoreCase: true);
                Assert.DoesNotContain("secret", content, ignoreCase: true);
                Assert.DoesNotContain("key=", content, ignoreCase: true);
            }
        }

        [Fact]
        public void UserCredentials_ShouldBeHashedOrEncrypted()
        {
            // Example: Check that user credentials are not stored in plaintext in Identity.API
            var files = Directory.GetFiles("../../../../src/Identity.API/", "*.json", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                var content = File.ReadAllText(file);
                Assert.DoesNotContain("password", content, ignoreCase: true);
            }
        }
    }
}
