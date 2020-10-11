***REMOVED***
using System.Reflection;
***REMOVED***

namespace SmartMirror.Core.Common
***REMOVED***
    public static class DirectoryInitializer
    ***REMOVED***
        public static void EnsureCorrectWorkingDirectory(ILogger log)
        ***REMOVED***
            var currentDir = Directory.GetCurrentDirectory();
            log.LogInformation($"Current working directory: ***REMOVED***currentDir***REMOVED***");

            var assemblyLocation = Assembly.GetEntryAssembly()?.Location;
            if (string.IsNullOrEmpty(assemblyLocation))
            ***REMOVED***
                log.LogInformation($"Empty assembly location");
***REMOVED***
          ***REMOVED***

            var finalLocation = Path.GetDirectoryName(assemblyLocation);
            if (!string.IsNullOrEmpty(finalLocation))
            ***REMOVED***
                Directory.SetCurrentDirectory(finalLocation);
                log.LogInformation($"Current working set to: ***REMOVED***finalLocation***REMOVED***");
          ***REMOVED***
            else
            ***REMOVED***
                log.LogInformation($"Empty final directory location");
          ***REMOVED***
      ***REMOVED***
  ***REMOVED***
***REMOVED***
