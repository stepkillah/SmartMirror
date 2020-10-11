***REMOVED***
***REMOVED***

namespace SmartMirror.Core
***REMOVED***
    public class AudioService
    ***REMOVED***
        public async Task StartRecord()
        ***REMOVED***
            using var startRes = $"arecord -D plughw:1,0 test.wav".StartBash();
            await Task.Delay(TimeSpan.FromSeconds(10));
            startRes.Kill();
      ***REMOVED***

  ***REMOVED***
***REMOVED***
