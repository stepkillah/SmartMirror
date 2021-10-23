***REMOVED***
using System.Diagnostics;
using System.Drawing;
using System.Threading;
***REMOVED***
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
***REMOVED***
using SmartMirror.Core.Common;
using SmartMirror.Core.ExternalProcesses;
***REMOVED***
using SmartMirror.Core.LedControl;
using SmartMirror.Core.Services;
//using SmartMirror.Core.VoiceRecognition.DeepSpeech;
using SmartMirror.Core.VoiceRecognition.Microsoft;

namespace SmartMirror.Core
***REMOVED***
    class Program
    ***REMOVED***
        public static IServiceProvider Container ***REMOVED*** get; private set; ***REMOVED***
        public static ILogger ProgramLogger;
        private static bool _isCleaning;
        private static readonly CancellationTokenSource AppCancellationTokenSource = new CancellationTokenSource();

        static async Task Main(string[] args)
        ***REMOVED***
            using IHost host = CreateHostBuilder(args).Build();
            Container = host.Services;
            if (Container == null)
                throw new ArgumentNullException(nameof(Container));
            ProgramLogger = Container.GetService<ILoggerFactory>()
                .CreateLogger<Program>();

            ProgramLogger.LogDebug("Container initialized");
            ConfigureConsole();
            ProgramLogger.LogInformation("SmartMirror");
            var osInfo = System.Runtime.InteropServices.RuntimeInformation.OSDescription;
            ProgramLogger.LogInformation($"OS Information: ***REMOVED***osInfo***REMOVED***");
            StartProgram();
            ProgramLogger.LogInformation("Program services started");
            await host.StartAsync(AppCancellationTokenSource.Token);
            ProgramLogger.LogInformation("App successfully started");
            await host.WaitForShutdownAsync(AppCancellationTokenSource.Token);
      ***REMOVED***

        private static void ConsoleOnCancelKeyPress(object sender, ConsoleCancelEventArgs e)
        ***REMOVED***
            e.Cancel = true;
            CleanupAndClose().ConfigureAwait(false);
      ***REMOVED***

        private static async Task CleanupAndClose()
        ***REMOVED***
            if (_isCleaning)
            ***REMOVED***
                ProgramLogger.LogInformation("Cleaning already started.");
***REMOVED***
          ***REMOVED***

***REMOVED***
            ***REMOVED***
                _isCleaning = true;
                ProgramLogger.LogInformation("Cleaning started");

                var audioService = Container.GetService<IAudioService>();
                if (audioService != null)
                ***REMOVED***
                    await audioService.StopProcessing();
                    audioService.Dispose();
              ***REMOVED***

                var ledManager = Container.GetService<ILedManager>();
                ledManager?.Dispose();

                var magicMirrorRunner = Container.GetService<IMagicMirrorRunner>();
                magicMirrorRunner?.Dispose();

                ProgramLogger.LogInformation("Cleaning finished\nClosing app...");
                AppCancellationTokenSource.Cancel();
          ***REMOVED***
***REMOVED***
            ***REMOVED***
                _isCleaning = false;
          ***REMOVED***
      ***REMOVED***


        private static void StartProgram()
        ***REMOVED***
            Container.GetService<IAudioService>()?.StartProcessing();
            Container.GetService<ILedManager>()?.StartProcessing();
            Container.GetService<IMagicMirrorRunner>()?.StartProcessing();
            _ = Task.Run(() => Container.GetService<IKeyboardCommandsService>()?.StartListenKeyCommands(AppCancellationTokenSource.Token));
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
                    ledManager?.TurnOn();
***REMOVED***
                case VoiceCommands.LedOff:
                    ledManager?.TurnOff();
***REMOVED***
                case VoiceCommands.LedColorSet:
                    if (e.Data is Color color)
                        ledManager?.TurnOn(color);
***REMOVED***
                default:
***REMOVED***
          ***REMOVED***
      ***REMOVED***


        private static void AudioServiceOnKeywordCommandRecognized(object sender, EventArgs e) => Container.GetService<IAPlayRunner>()?.Play(Constants.SuccessSoundPath);

        private static void AudioServiceOnCommandRecognitionError(object sender, EventArgs e) => Container.GetService<IAPlayRunner>()?.Play(Constants.ErrorSoundPath);
        #region DI

        static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((_, services) =>
                    services.AddLogging(builder => builder.AddConsole())
                        .AddSingleton<IAPlayRunner, APlayRunner>()
                        .AddSingleton(InitAudioService)
                        .AddSingleton(InitLedManager)
                        .AddSingleton<IMagicMirrorRunner, MagicMirrorRunner>()
                        .AddSingleton<IKeyboardCommandsService, KeyboardCommandsService>());

        private static IAudioService InitAudioService(IServiceProvider arg)
        ***REMOVED***
***REMOVED***
            ***REMOVED***
                var audioService = new AudioService(arg.GetService<ILogger<AudioService>>());
                audioService.CommandRecognized += AudioServiceOnCommandRecognized;
                audioService.KeywordCommandRecognized += AudioServiceOnKeywordCommandRecognized;
                audioService.CommandRecognitionError += AudioServiceOnCommandRecognitionError;
                return audioService;
          ***REMOVED***
            catch (Exception e)
            ***REMOVED***
                ProgramLogger.LogError(e, nameof(InitAudioService));
                return new NullAudioService();
          ***REMOVED***
      ***REMOVED***


        //private static IAudioService InitDeepSpeechAudioService(IServiceProvider arg)
        //***REMOVED***
        //    try
        //    ***REMOVED***
        //        var audioService = new DeepSpeechAudioManager(arg.GetService<ILogger<DeepSpeechAudioManager>>());
        //        return audioService;
        //  ***REMOVED***
        //    catch (Exception e)
        //    ***REMOVED***
        //        ProgramLogger.LogError(e, nameof(InitAudioService));
        //        return new NullAudioService();
        //  ***REMOVED***
        //***REMOVED***

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
