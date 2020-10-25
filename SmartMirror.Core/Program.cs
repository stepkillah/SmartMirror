***REMOVED***
***REMOVED***
using Microsoft.Extensions.DependencyInjection;
***REMOVED***
using SmartMirror.Core.Common;
using SmartMirror.Core.LedControl;
using SmartMirror.Core.VoiceRecognition.Microsoft;

namespace SmartMirror.Core
***REMOVED***
    class Program
    ***REMOVED***
        public static IServiceProvider Container ***REMOVED*** get; private set; ***REMOVED***
        public static ILogger ProgramLogger;

        private static bool _isRunning = true;

        static void Main(string[] args)
        ***REMOVED***
            InitContainer();
            if (Container == null)
                throw new ArgumentNullException(nameof(Container));

            ConfigureConsole();

            ProgramLogger.LogInformation("SmartMirror");

            var osInfo = System.Runtime.InteropServices.RuntimeInformation.OSDescription;
            ProgramLogger.LogWarning($"OS Information: ***REMOVED***osInfo***REMOVED***");
            StartProgram();
            while (_isRunning)
                Console.ReadKey();
      ***REMOVED***

        private static void ConsoleOnCancelKeyPress(object sender, ConsoleCancelEventArgs e)
        ***REMOVED***
            e.Cancel = true;
            CleanupAndClose().ConfigureAwait(false);
      ***REMOVED***

        private static async Task CleanupAndClose()
        ***REMOVED***
            ProgramLogger.LogInformation("Cleaning started");

            await Container.GetService<IAudioService>().StopProcessing();
            //_magicMirrorRunner?.StopProcessing();

            ProgramLogger.LogInformation("Cleaning finished\nClosing app...");
            _isRunning = false;
            Environment.Exit(0);
      ***REMOVED***


        private static void StartProgram()
        ***REMOVED***
            Container.GetService<IAudioService>().StartProcessing();
            Container.GetService<ILedManager>().StartProcessing();
      ***REMOVED***

        private static void ConfigureConsole()
        ***REMOVED***
            Console.CancelKeyPress += ConsoleOnCancelKeyPress;
            DirectoryInitializer.EnsureCorrectWorkingDirectory(ProgramLogger);
      ***REMOVED***


        private static void AudioServiceOnCommandRecognized(object sender, CommandRecognizedEventArgs e)
        ***REMOVED***
            var ledManager = Container.GetService<ILedManager>();
            switch (e.Command)
            ***REMOVED***
                case VoiceCommands.LedOn:
                    ledManager.TurnOn();
***REMOVED***
                case VoiceCommands.LedOff:
                    ledManager.TurnOff();
***REMOVED***
                default:
***REMOVED***
          ***REMOVED***
      ***REMOVED***

        #region DI


        private static void InitContainer()
        ***REMOVED***

            //setup our DI
            Container = new ServiceCollection()
                .AddLogging(builder => builder.AddConsole())
                .AddSingleton(InitAudioService)
                .AddSingleton(InitLedManager)
                .BuildServiceProvider();


            ProgramLogger = Container.GetService<ILoggerFactory>()
                .CreateLogger<Program>();

            ProgramLogger.LogDebug("Container initialized");
      ***REMOVED***

        private static IAudioService InitAudioService(IServiceProvider arg)
        ***REMOVED***
***REMOVED***
            ***REMOVED***
                var audioService = new AudioService(arg.GetService<ILogger<AudioService>>());
                audioService.CommandRecognized += AudioServiceOnCommandRecognized;
                return audioService;
          ***REMOVED***
            catch (Exception e)
            ***REMOVED***
                ProgramLogger.LogError(e, nameof(InitAudioService));
                return new NullAudioService();
          ***REMOVED***
      ***REMOVED***
        private static ILedManager InitLedManager(IServiceProvider arg)
        ***REMOVED***
***REMOVED***
            ***REMOVED***
                return new LedManager(arg.GetService<ILogger<LedManager>>());
          ***REMOVED***
            catch (Exception e)
            ***REMOVED***
                ProgramLogger.LogError(e, nameof(InitLedManager));
                return new NullLedManager();
          ***REMOVED***
      ***REMOVED***
***REMOVED***
  ***REMOVED***
***REMOVED***
