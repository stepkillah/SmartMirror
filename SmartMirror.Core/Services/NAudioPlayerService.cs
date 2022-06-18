using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NAudio.Wave;
using SmartMirror.Core.Interfaces;

namespace SmartMirror.Core.Services
{
    public class NAudioPlayerService : IAudioPlayer
    {
        private readonly ILogger _logger;

        public NAudioPlayerService(ILogger<NAudioPlayerService> logger)
        {
            _logger = logger;
        }

        public async Task Play(string path, CancellationToken cancellationToken = default)
        {
            try
            {
                await using var audioFile = new AudioFileReader(path);
                using var outputDevice = new WaveOutEvent();
                outputDevice.Init(audioFile);
                outputDevice.Play();
                while (outputDevice.PlaybackState == PlaybackState.Playing)
                {
                    await Task.Delay(1000, cancellationToken);
                }
            }
            catch (TaskCanceledException e)
            {
                _logger.LogWarning(e, "NAudio playback canceled");
            }
            catch (Exception e)
            {
                _logger.LogError(e, nameof(Play));
            }
        }
    }
}
