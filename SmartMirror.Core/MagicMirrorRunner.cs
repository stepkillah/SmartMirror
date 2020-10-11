using System.Diagnostics;

namespace SmartMirror.Core
***REMOVED***
    public class MagicMirrorRunner
    ***REMOVED***
        private Process _magicMirrorRunProcess;
***REMOVED***
        ***REMOVED***
            if (_magicMirrorRunProcess != null)
***REMOVED***
            _magicMirrorRunProcess = new Process()
            ***REMOVED***
                StartInfo = new ProcessStartInfo("npm")
                ***REMOVED***
                    Arguments = "start",
                    RedirectStandardOutput = true,
                    RedirectStandardInput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
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


  ***REMOVED***
***REMOVED***
