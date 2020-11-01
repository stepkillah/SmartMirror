***REMOVED***
using System.Diagnostics;
using System.Drawing;
***REMOVED***
using Microsoft.Extensions.DependencyInjection;
***REMOVED***
using SmartMirror.Core.Common;
using SmartMirror.Core.ExternalProcesses;
using SmartMirror.Core.LedControl;
using SmartMirror.Core.VoiceRecognition.DeepSpeech;
using SmartMirror.Core.VoiceRecognition.Microsoft;

namespace SmartMirror.Core
***REMOVED***
    class Program
    ***REMOVED***
        public static IServiceProvider Container ***REMOVED*** get; private set; ***REMOVED***
        public static ILogger ProgramLogger;

        private static bool _isRunning = true;
        private static bool _isCleaning;

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
            ***REMOVED***
                if (Debugger.IsAttached)
                ***REMOVED***
                    Console.Read();
              ***REMOVED***
                else
                ***REMOVED***
                    Console.WriteLine("Waiting for command");
                    var line = Console.ReadLine();
                    if (!string.IsNullOrEmpty(line) && line.StartsWith("led color"))
                    ***REMOVED***
            ***REMOVED***
                        ***REMOVED***
                            var color = line.Substring(10, line.Length - 10);
                            Color ledColor = ColorTranslator.FromHtml(color);
                            var ledManager = Container.GetService<ILedManager>();
                            ledManager.TurnOn(ledColor);
                      ***REMOVED***
                        catch (Exception e)
                        ***REMOVED***
                            ProgramLogger.LogError(e, "Waiting for command");
                      ***REMOVED***
                  ***REMOVED***
              ***REMOVED***
          ***REMOVED***
      ***REMOVED***

        private static void ConsoleOnCancelKeyPress(object sender, ConsoleCancelEventArgs e)
        ***REMOVED***
            e.Cancel = true;
            CleanupAndClose().ConfigureAwait(false);
      ***REMOVED***

        private static async Task CleanupAndClose()
        ***REMOVED***
            if (_isCleaning)
                ProgramLogger.LogInformation("Cleaning already started.");
***REMOVED***
            ***REMOVED***
                _isCleaning = true;
                ProgramLogger.LogInformation("Cleaning started");

                var audioService = Container.GetService<IAudioService>();
                await audioService.StopProcessing();
                audioService.Dispose();

                var ledManager = Container.GetService<ILedManager>();
                ledManager.Dispose();

                var magicMirrorRunner = Container.GetService<IMagicMirrorRunner>();
                magicMirrorRunner.Dispose();

                ProgramLogger.LogInformation("Cleaning finished\nClosing app...");
***REMOVED***
                Environment.Exit(0);
          ***REMOVED***
***REMOVED***
            ***REMOVED***
                _isCleaning = false;
          ***REMOVED***
      ***REMOVED***


        private static void StartProgram()
        ***REMOVED***
            Container.GetService<IAudioService>().StartProcessing();
            Container.GetService<ILedManager>().StartProcessing();
            Container.GetService<IMagicMirrorRunner>().StartProcessing();
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
                case VoiceCommands.LedColorSet:
                    if (e.Data is Color color)
                        ledManager.TurnOn(color);
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
                //.AddSingleton(InitDeepSpeechAudioService)
                .AddSingleton(InitLedManager)
                .AddSingleton<IMagicMirrorRunner, MagicMirrorRunner>()
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

        private static IAudioService InitDeepSpeechAudioService(IServiceProvider arg)
        ***REMOVED***
***REMOVED***
            ***REMOVED***
                var audioService = new DeepSpeechAudioManager(arg.GetService<ILogger<DeepSpeechAudioManager>>());
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
