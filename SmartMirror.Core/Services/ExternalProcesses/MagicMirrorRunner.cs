using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SmartMirror.Core.Interfaces;
using SmartMirror.Core.Models;

namespace SmartMirror.Core.Services.ExternalProcesses
{
    public class MagicMirrorRunner : IMagicMirrorRunner, IDisposable
    {
        private bool _isDisposed;
        private readonly ILogger _logger;
        private readonly MagicMirrorOptions _mirrorOptions;

        public MagicMirrorRunner(ILogger<MagicMirrorRunner> logger, IOptions<MagicMirrorOptions> magicMirrorOptions)
        {
            _logger = logger;
            _mirrorOptions = magicMirrorOptions.Value;
        }

        private Process _magicMirrorRunProcess;
        public ValueTask StartProcessing()
        {
            try
            {
                if (_magicMirrorRunProcess != null)
                    return ValueTask.CompletedTask;

                _logger.LogInformation($"{nameof(MagicMirrorRunner)} Starting");
                _magicMirrorRunProcess = new Process()
                {
                    StartInfo = new ProcessStartInfo("sudo", $"-H -u {_mirrorOptions.DefaultUserName} DISPLAY=:0.0 npm start")
                    {
                        RedirectStandardOutput = false,
                        RedirectStandardInput = false,
                        UseShellExecute = false,
                        CreateNoWindow = false,
                        WorkingDirectory = _mirrorOptions.WorkingDirectory
                    }
                };
                _logger.LogInformation(_magicMirrorRunProcess.Start()
                    ? $"{nameof(MagicMirrorRunner)} Started"
                    : $"{nameof(MagicMirrorRunner)} Start failed");
            }
            catch (Exception e)
            {
                _magicMirrorRunProcess = null;
                _logger.LogError(e, nameof(StartProcessing));
            }
            return ValueTask.CompletedTask;
        }

        public ValueTask StopProcessing()
        {
            try
            {
                _logger.LogInformation($"{nameof(MagicMirrorRunner)}: Stopping");

                if (_magicMirrorRunProcess == null)
                    return ValueTask.CompletedTask;
                _magicMirrorRunProcess.Close();
                _magicMirrorRunProcess.Dispose();
                _magicMirrorRunProcess = null;
                _logger.LogInformation($"{nameof(MagicMirrorRunner)}: Stopped");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(MagicMirrorRunner)}: Stopping error");
            }
            return ValueTask.CompletedTask;
        }



        #region disposing
        protected virtual void Dispose(bool disposing)
        {
            if (_isDisposed) return;

            if (disposing)
                StopProcessing();

            _isDisposed = true;

            _logger.LogInformation($"{nameof(MagicMirrorRunner)} disposed.");
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
