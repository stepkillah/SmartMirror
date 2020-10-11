***REMOVED***
***REMOVED***
***REMOVED***
using Microsoft.Extensions.Logging.Abstractions;
using SmartMirror.Core.Common;
using SmartMirror.Core.VoiceRecognition;

namespace SmartMirror.Core
***REMOVED***
    class Program
    ***REMOVED***
        private static LedManager _ledManager;
        private static AudioService _audioService;
        private static MagicMirrorRunner _magicMirrorRunner;
        private static ILoggerFactory _loggerFactory;
        private static ILogger _mainLogger;

        private static bool _isRunning = true;

        static async Task Main(string[] args)
        ***REMOVED***
            Console.Clear();
            Console.WriteLine("SmartMirror");
            var osInfo = System.Runtime.InteropServices.RuntimeInformation.OSDescription;
            Console.WriteLine($"OS Information: ***REMOVED***osInfo***REMOVED***");
            Console.CancelKeyPress += ConsoleOnCancelKeyPress;
            Init();
            await StartProgram();

            while (_isRunning)
            ***REMOVED***
                await Task.Delay(50);
          ***REMOVED***
      ***REMOVED***

        private static void ConsoleOnCancelKeyPress(object sender, ConsoleCancelEventArgs e)
        ***REMOVED***
            e.Cancel = true;
            CleanupAndClose();
      ***REMOVED***

        private static async Task CleanupAndClose()
        ***REMOVED***
            Console.WriteLine("Cleaning started");
            if (_audioService != null)
                await _audioService.StopProcessing();
            //_magicMirrorRunner?.StopProcessing();
            Console.WriteLine("Cleaning finished\nClosing app...");
            _isRunning = false;
            Environment.Exit(0);
      ***REMOVED***


        private static async Task StartProgram()
        ***REMOVED***
            //_magicMirrorRunner?.StartProcessing();
            _audioService?.StartProcessing();
            await _ledManager.StartProcessing();
      ***REMOVED***

        private static void Init()
        ***REMOVED***
            _loggerFactory = NullLoggerFactory.Instance;
            _mainLogger = _loggerFactory.CreateLogger<Program>();
            DirectoryInitializer.EnsureCorrectWorkingDirectory(_mainLogger);
            _ledManager = new LedManager(_loggerFactory.CreateLogger<LedManager>());
            //_magicMirrorRunner = new MagicMirrorRunner();
            StartAudioService();
      ***REMOVED***

        private static void StartAudioService()
        ***REMOVED***
***REMOVED***
            ***REMOVED***
                _audioService = new AudioService(_loggerFactory.CreateLogger<AudioService>());
                _audioService.CommandRecognized += AudioServiceOnCommandRecognized;
          ***REMOVED***
            catch (Exception e)
            ***REMOVED***
                Console.WriteLine($"Audio service initialization failed: ***REMOVED***e.Message***REMOVED***");
          ***REMOVED***
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
