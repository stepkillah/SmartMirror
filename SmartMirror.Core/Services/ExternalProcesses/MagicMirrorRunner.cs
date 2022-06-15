***REMOVED***
using System.Diagnostics;
***REMOVED***
***REMOVED***
***REMOVED***
***REMOVED***
***REMOVED***

namespace SmartMirror.Core.Services.ExternalProcesses
***REMOVED***
    public class MagicMirrorRunner : IMagicMirrorRunner, IDisposable
    ***REMOVED***
***REMOVED***
***REMOVED***
        private readonly MagicMirrorOptions _mirrorOptions;

        public MagicMirrorRunner(ILogger<MagicMirrorRunner> logger, IOptions<MagicMirrorOptions> magicMirrorOptions)
        ***REMOVED***
***REMOVED***
            _mirrorOptions = magicMirrorOptions.Value;
      ***REMOVED***

        private Process _magicMirrorRunProcess;
        public ValueTask StartProcessing()
        ***REMOVED***
***REMOVED***
            ***REMOVED***
                if (_magicMirrorRunProcess != null)
                    return ValueTask.CompletedTask;

                _logger.LogInformation($"***REMOVED***nameof(MagicMirrorRunner)***REMOVED*** Starting");
                _magicMirrorRunProcess = new Process()
                ***REMOVED***
                    StartInfo = new ProcessStartInfo("sudo", $"-H -u ***REMOVED***_mirrorOptions.DefaultUserName***REMOVED*** DISPLAY=:0.0 npm start")
                    ***REMOVED***
                        RedirectStandardOutput = false,
                        RedirectStandardInput = false,
                        UseShellExecute = false,
                        CreateNoWindow = false,
                        WorkingDirectory = _mirrorOptions.WorkingDirectory
                  ***REMOVED***
              ***REMOVED***;
                _logger.LogInformation(_magicMirrorRunProcess.Start()
                    ? $"***REMOVED***nameof(MagicMirrorRunner)***REMOVED*** Started"
                    : $"***REMOVED***nameof(MagicMirrorRunner)***REMOVED*** Start failed");
          ***REMOVED***
            catch (Exception e)
            ***REMOVED***
                _magicMirrorRunProcess = null;
                _logger.LogError(e, nameof(StartProcessing));
          ***REMOVED***
            return ValueTask.CompletedTask;
      ***REMOVED***

        public ValueTask StopProcessing()
        ***REMOVED***
***REMOVED***
            ***REMOVED***
                _logger.LogInformation($"***REMOVED***nameof(MagicMirrorRunner)***REMOVED***: Stop***REMOVED***ng");

                if (_magicMirrorRunProcess == null)
                    return ValueTask.CompletedTask;
                _magicMirrorRunProcess.Close();
                _magicMirrorRunProcess.Dispose();
                _magicMirrorRunProcess = null;
                _logger.LogInformation($"***REMOVED***nameof(MagicMirrorRunner)***REMOVED***: Stopped");
          ***REMOVED***
***REMOVED***
            ***REMOVED***
                _logger.LogError(ex, $"***REMOVED***nameof(MagicMirrorRunner)***REMOVED***: Stop***REMOVED***ng error");
          ***REMOVED***
            return ValueTask.CompletedTask;
      ***REMOVED***



        #region disposing
        protected virtual void Dispose(bool disposing)
        ***REMOVED***
***REMOVED***

***REMOVED***
                StopProcessing();

***REMOVED***

            _logger.LogInformation($"***REMOVED***nameof(MagicMirrorRunner)***REMOVED*** disposed.");
      ***REMOVED***

***REMOVED***
        ***REMOVED***
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
***REMOVED***
***REMOVED***
      ***REMOVED***
***REMOVED***
  ***REMOVED***
***REMOVED***
