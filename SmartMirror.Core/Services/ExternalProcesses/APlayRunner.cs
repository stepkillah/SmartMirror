***REMOVED***
using System.Diagnostics;
using System.Threading;
***REMOVED***
***REMOVED***
***REMOVED***

namespace SmartMirror.Core.Services.ExternalProcesses
***REMOVED***
    public class APlayRunner : IAudioPlayer
    ***REMOVED***
***REMOVED***

        public APlayRunner(ILogger<APlayRunner> logger)
        ***REMOVED***
***REMOVED***
      ***REMOVED***


        public async Task Play(string path, CancellationToken cancellationToken = default)
        ***REMOVED***
***REMOVED***
            ***REMOVED***
                using var aplayProcess = new Process()
                ***REMOVED***
                    StartInfo = new ProcessStartInfo("aplay", path)
                    ***REMOVED***
                        RedirectStandardOutput = true,
                        RedirectStandardInput = false,
                        UseShellExecute = false,
                        CreateNoWindow = false,
                  ***REMOVED***
              ***REMOVED***;
                aplayProcess.Start();
                await aplayProcess.StandardOutput.ReadToEndAsync();
                aplayProcess.Kill();
          ***REMOVED***
            catch (Exception e)
            ***REMOVED***
                _logger.LogError(e, nameof(Play));
          ***REMOVED***

      ***REMOVED***
  ***REMOVED***
***REMOVED***
