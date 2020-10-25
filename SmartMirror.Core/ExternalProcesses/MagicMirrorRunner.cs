***REMOVED***
using System.Diagnostics;
***REMOVED***

namespace SmartMirror.Core.ExternalProcesses
***REMOVED***
    public class MagicMirrorRunner : IMagicMirrorRunner
    ***REMOVED***
        private const string DefaultUserName = "magicmirror";

***REMOVED***
***REMOVED***

        public MagicMirrorRunner(ILogger<MagicMirrorRunner> logger)
        ***REMOVED***
***REMOVED***
      ***REMOVED***

        private Process _magicMirrorRunProcess;
***REMOVED***
        ***REMOVED***
            if (_magicMirrorRunProcess != null)
***REMOVED***

            _magicMirrorRunProcess = new Process()
            ***REMOVED***
                StartInfo = new ProcessStartInfo("sudo", $"-H -u ***REMOVED***DefaultUserName***REMOVED*** -g nogroup DISPLAY=:0.0 npm start")
                ***REMOVED***
                    RedirectStandardOutput = true,
                    RedirectStandardInput = false,
                    UseShellExecute = false,
                    CreateNoWindow = false,
                    WorkingDirectory = "/home/***REMOVED***/magicmirror"
              ***REMOVED***
          ***REMOVED***;
            _magicMirrorRunProcess.Start();
      ***REMOVED***

        public void StopProcessing()
        ***REMOVED***
            if (_magicMirrorRunProcess == null)
***REMOVED***
            
            _magicMirrorRunProcess.Kill();
            _magicMirrorRunProcess.Dispose();
            _magicMirrorRunProcess = null;
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
