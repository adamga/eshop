using System.IO;
using System.Linq;
using Xunit;

namespace Compliance.UnitTests
{
    public class ConfigurationFilesTests
    {
        [Fact]
        public void AppSettings_ShouldNotContainPlaintextConnectionStrings()
        {
            // Check that connection strings in appsettings files are not in plaintext
            var files = Directory.GetFiles("../../../../src/", "appsettings*.json", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                var content = File.ReadAllText(file);
                Assert.DoesNotContain("Data Source=", content, ignoreCase: true);
                Assert.DoesNotContain("Password=", content, ignoreCase: true);
            }
        }
    }

    public class LoggingTests
    {
        [Fact]
        public void Logs_ShouldNotContainSensitiveData()
        {
            // Example: Check that log files do not contain card numbers or sensitive info
            var logDirs = new[] { "../../../../src/Basket.API/logs/", "../../../../src/Ordering.API/logs/" };
            foreach (var dir in logDirs)
            {
                if (!Directory.Exists(dir)) continue;
                var files = Directory.GetFiles(dir, "*.log", SearchOption.AllDirectories);
                foreach (var file in files)
                {
                    var content = File.ReadAllText(file);
                    Assert.DoesNotContain("cardNumber", content, ignoreCase: true);
                    Assert.DoesNotContain("cvv", content, ignoreCase: true);
                    Assert.DoesNotContain("password", content, ignoreCase: true);
                }
            }
        }
    }
}
