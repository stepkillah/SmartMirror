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
            if (_magicMirrorRunProcess != null)
***REMOVED***

            string defaultUser;
            using (var proc = new Process() ***REMOVED*** StartInfo = new ProcessStartInfo("whoami") ***REMOVED*** RedirectStandardOutput = true ***REMOVED*** ***REMOVED***)
            ***REMOVED***
                proc.Start();
                defaultUser = proc.StandardOutput.ReadToEnd().TrimEnd('\n');
          ***REMOVED***

            if (string.IsNullOrEmpty(defaultUser))
                defaultUser = DefaultUserName;

            _magicMirrorRunProcess = new Process()
            ***REMOVED***
                StartInfo = new ProcessStartInfo("sudo", $"-u ***REMOVED***defaultUser***REMOVED*** npm start")
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
