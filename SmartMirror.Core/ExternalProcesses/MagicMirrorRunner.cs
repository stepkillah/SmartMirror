***REMOVED***
using System.Diagnostics;
***REMOVED***

namespace SmartMirror.Core.ExternalProcesses
***REMOVED***
    public class MagicMirrorRunner : IMagicMirrorRunner
    ***REMOVED***
        private const string DefaultUserName = "***REMOVED***";

***REMOVED***
***REMOVED***

        public MagicMirrorRunner(ILogger<MagicMirrorRunner> logger)
        ***REMOVED***
***REMOVED***
      ***REMOVED***

        private Process _magicMirrorRunProcess;
***REMOVED***
        ***REMOVED***
***REMOVED***
            ***REMOVED***
                if (_magicMirrorRunProcess != null)
***REMOVED***

                _magicMirrorRunProcess = new Process()
                ***REMOVED***
                    StartInfo = new ProcessStartInfo("sudo", $"-H -u ***REMOVED***DefaultUserName***REMOVED*** DISPLAY=:0.0 npm start")
                    ***REMOVED***
                        RedirectStandardOutput = true,
                        RedirectStandardInput = false,
                        UseShellExecute = false,
                        CreateNoWindow = false,
                        WorkingDirectory = "/home/***REMOVED***/Projects/MagicMirror"
                  ***REMOVED***
              ***REMOVED***;
                _magicMirrorRunProcess.Start();
          ***REMOVED***
            catch (Exception e)
            ***REMOVED***
                _magicMirrorRunProcess = null;
                _logger.LogError(e, nameof(StartProcessing));
          ***REMOVED***
      ***REMOVED***

        public void StopProcessing()
        ***REMOVED***
***REMOVED***
            ***REMOVED***
                _logger.LogInformation($"***REMOVED***nameof(MagicMirrorRunner)***REMOVED***: Stop***REMOVED***ng");

                if (_magicMirrorRunProcess == null)
***REMOVED***

                _magicMirrorRunProcess.Kill();
                _magicMirrorRunProcess.Dispose();
                _magicMirrorRunProcess = null;
          ***REMOVED***
***REMOVED***
            ***REMOVED***
                _logger.LogError(ex,$"***REMOVED***nameof(MagicMirrorRunner)***REMOVED***: Stop***REMOVED***ng error");
          ***REMOVED***
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
