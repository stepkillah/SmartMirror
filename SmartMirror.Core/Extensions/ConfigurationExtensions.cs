using System.IO;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace SmartMirror.Core.Extensions
{
    public static class ConfigurationExtensions
    {
        public static void SetSmartMirrorBasePath(this IConfigurationBuilder builder)
        {
            using var factory = LoggerFactory.Create(loggerBuilder => loggerBuilder.AddSmartMirrorLogging());
            var log = factory.CreateLogger<IConfigurationBuilder>();

            var currentDir = Directory.GetCurrentDirectory();
            log.LogInformation($"Current working directory: {currentDir}");

            var assemblyLocation = Assembly.GetEntryAssembly()?.Location;
            if (string.IsNullOrEmpty(assemblyLocation))
            {
                log.LogInformation($"Empty assembly location");
                return;
            }

            var finalLocation = Path.GetDirectoryName(assemblyLocation);
            if (!string.IsNullOrEmpty(finalLocation))
            {
                if (finalLocation == currentDir)
                {
                    log.LogInformation($"Current working dir is correct");
                    return;
                }
                builder.SetBasePath(finalLocation);
                log.LogInformation($"Current working set to: {finalLocation}");
            }
            else
            {
                log.LogInformation($"Empty final directory location");
            }
        }
    }
}
