***REMOVED***
***REMOVED***
***REMOVED***
using Microsoft.Extensions.Logging.Abstractions;
using SmartMirror.Core.VoiceRecognition;

namespace SmartMirror.Core
***REMOVED***
    class Program
    ***REMOVED***
        private static LedManager _ledManager;
        private static AudioService _audioService;
        private static ILoggerFactory _loggerFactory;

        static async Task Main(string[] args)
        ***REMOVED***
            Console.Clear();
            Console.WriteLine("SmartMirror");
            var osInfo = System.Runtime.InteropServices.RuntimeInformation.OSDescription;
            Console.WriteLine($"OS Information: ***REMOVED***osInfo***REMOVED***");
            Console.CancelKeyPress+=ConsoleOnCancelKeyPress;
            Init();
            await StartProgram();
            Console.Read();
      ***REMOVED***

        private static async void ConsoleOnCancelKeyPress(object sender, ConsoleCancelEventArgs e)
        ***REMOVED***
            if (_audioService != null)
                await _audioService.StopProcessing();
      ***REMOVED***


        private static async Task StartProgram()
        ***REMOVED***
            _audioService.StartProcessing();
            await _ledManager.StartLedProcessing();
      ***REMOVED***

        private static void Init()
        ***REMOVED***
            _loggerFactory = NullLoggerFactory.Instance;
            _ledManager = new LedManager(_loggerFactory.CreateLogger<LedManager>());
            _audioService = new AudioService(_loggerFactory.CreateLogger<AudioService>());
            _audioService.CommandRecognized += AudioServiceOnCommandRecognized;
      ***REMOVED***

        private static void AudioServiceOnCommandRecognized(object? sender, CommandRecognizedEventArgs e)
        ***REMOVED***
            switch (e.Command)
            ***REMOVED***
                case VoiceCommands.LedOn:
                    _ledManager.TurnOn();
***REMOVED***
                case VoiceCommands.LedOff:
                    _ledManager.TurnOff();
***REMOVED***
                default: 
***REMOVED***
          ***REMOVED***
      ***REMOVED***
  ***REMOVED***
***REMOVED***
