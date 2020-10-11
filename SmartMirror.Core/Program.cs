***REMOVED***
***REMOVED***

namespace SmartMirror.Core
***REMOVED***
    class Program
    ***REMOVED***
        private static LedManager _ledManager;
        private static AudioService _audioService;

        static async Task Main(string[] args)
        ***REMOVED***
            Console.Clear();
            Console.WriteLine("SmartMirror");
            var osInfo = System.Runtime.InteropServices.RuntimeInformation.OSDescription;
            Console.WriteLine($"OS Information: ***REMOVED***osInfo***REMOVED***");
            Init();
            await StartProgram();
            Console.Read();
      ***REMOVED***
        
        private static async Task StartProgram()
        ***REMOVED***
            await _audioService.StartRecord();
            await _ledManager.StartLedProcessing();
      ***REMOVED***

        private static void Init()
        ***REMOVED***
            _ledManager = new LedManager();
            _audioService = new AudioService();
      ***REMOVED***
  ***REMOVED***
***REMOVED***
