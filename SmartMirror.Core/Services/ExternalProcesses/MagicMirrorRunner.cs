using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SmartMirror.Core.Models;

namespace SmartMirror.Core.Services.ExternalProcesses
{
    public class MagicMirrorRunner : IHostedService, IDisposable
    {
        private bool _isDisposed;
        private readonly ILogger _logger;
        private readonly MagicMirrorOptions _mirrorOptions;

        public MagicMirrorRunner(ILogger<MagicMirrorRunner> logger,
            IOptions<MagicMirrorOptions> magicMirrorOptions)
        {
            _logger = logger;
            _mirrorOptions = magicMirrorOptions.Value;
        }

        private Process _magicMirrorRunProcess;

        public Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                if (_magicMirrorRunProcess != null)
                    return Task.CompletedTask;

                _logger.LogInformation($"{nameof(MagicMirrorRunner)} Starting");
                _magicMirrorRunProcess = new Process()
                {
                    StartInfo = new ProcessStartInfo("npm", $"run start")
                    {
                        RedirectStandardOutput = false,
                        RedirectStandardInput = false,
                        UseShellExecute = false,
                        CreateNoWindow = false,
                        UserName = _mirrorOptions.RunAsUser,
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
                _logger.LogError(e, nameof(StartAsync));
            }
            return Task.CompletedTask;
        }


        public Task StopAsync(CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation($"{nameof(MagicMirrorRunner)}: Stopping");

                if (_magicMirrorRunProcess == null)
                    return Task.CompletedTask;
                _magicMirrorRunProcess.Kill();
                _logger.LogInformation($"{nameof(MagicMirrorRunner)}: Stopped");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(MagicMirrorRunner)}: Stopping error");
            }
            return Task.CompletedTask;
        }

        #region disposing
        protected virtual void Dispose(bool disposing)
        {
            if (_isDisposed) return;

            if (disposing)
            {
                _magicMirrorRunProcess?.Dispose();
                _magicMirrorRunProcess = null;
            }

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
