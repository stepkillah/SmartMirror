using System.IO;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace SmartMirror.Core.Extensions
{
    public static class ConfigurationExtensions
    {
        public static IHostBuilder ConfigureSmartMirrorRootPath(this IHostBuilder hostBuilder)
        {
            using var factory = LoggerFactory.Create(loggerBuilder => loggerBuilder.AddSmartMirrorLogging());
            var log = factory.CreateLogger<IConfigurationBuilder>();
            var currentDir = Directory.GetCurrentDirectory();
            log.LogInformation($"Current working directory: {currentDir}");

            var assemblyLocation = Assembly.GetEntryAssembly()?.Location;
            if (string.IsNullOrEmpty(assemblyLocation))
            {
                log.LogInformation($"Empty assembly location");
                return hostBuilder;
            }
            var finalDir = Path.GetDirectoryName(assemblyLocation);

            if (!string.IsNullOrEmpty(finalDir))
            {
                hostBuilder.ConfigureHostConfiguration(builder => builder.SetBasePath(finalDir));
                log.LogInformation($"Set configuration base path: {finalDir}");
                Directory.SetCurrentDirectory(finalDir);
                log.LogInformation($"Set current working directory base path: {finalDir}");
                hostBuilder.UseContentRoot(finalDir);
                log.LogInformation($"Set content root directory base path: {finalDir}");
            }
            else
            {
                log.LogInformation($"Empty final directory location");
            }

            return hostBuilder;

        }
    }
}
