using System.IO;
using System.Reflection;
using Microsoft.Extensions.Logging;

namespace SmartMirror.Core.Common
{
    public static class DirectoryInitializer
    {
        public static void EnsureCorrectWorkingDirectory(ILogger log)
        {
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
                Directory.SetCurrentDirectory(finalLocation);
                log.LogInformation($"Current working set to: {finalLocation}");
            }
            else
            {
                log.LogInformation($"Empty final directory location");
            }
        }
    }
}
