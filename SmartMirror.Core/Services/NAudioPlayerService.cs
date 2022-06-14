***REMOVED***
using System.Threading;
***REMOVED***
***REMOVED***
using NAudio.Wave;
***REMOVED***

namespace SmartMirror.Core.Services
***REMOVED***
    public class NAudioPlayerService : IAudioPlayer
    ***REMOVED***
***REMOVED***

        public NAudioPlayerService(ILogger<NAudioPlayerService> logger)
        ***REMOVED***
***REMOVED***
      ***REMOVED***

        public async Task Play(string path, CancellationToken cancellationToken = default)
        ***REMOVED***
***REMOVED***
            ***REMOVED***
                await using var audioFile = new AudioFileReader(path);
                using var outputDevice = new WaveOutEvent();
                outputDevice.Init(audioFile);
                outputDevice.Play();
                while (outputDevice.PlaybackState == PlaybackState.Playing)
                ***REMOVED***
                    await Task.Delay(1000, cancellationToken);
              ***REMOVED***
          ***REMOVED***
            catch (Exception e)
            ***REMOVED***
                _logger.LogError(e, nameof(Play));
          ***REMOVED***
      ***REMOVED***
  ***REMOVED***
***REMOVED***
