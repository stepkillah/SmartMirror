using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SmartMirror.Core.Interfaces;

namespace SmartMirror.Core.Services.ExternalProcesses
{
    public class APlayRunner : IAudioPlayer
    {
        private readonly ILogger _logger;

        public APlayRunner(ILogger<APlayRunner> logger)
        {
            _logger = logger;
        }


        public async Task Play(string path, CancellationToken cancellationToken = default)
        {
            try
            {
                using var aplayProcess = new Process()
                {
                    StartInfo = new ProcessStartInfo("aplay", path)
                    {
                        RedirectStandardOutput = true,
                        RedirectStandardInput = false,
                        UseShellExecute = false,
                        CreateNoWindow = false,
                    }
                };
                aplayProcess.Start();
                await aplayProcess.StandardOutput.ReadToEndAsync();
                aplayProcess.Kill();
            }
            catch (Exception e)
            {
                _logger.LogError(e, nameof(Play));
            }

        }
    }
}
