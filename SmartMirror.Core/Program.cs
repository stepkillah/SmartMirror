***REMOVED***
using System.Threading;
***REMOVED***
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
***REMOVED***
using SmartMirror.Core.Common;
using SmartMirror.Core.Extensions;
***REMOVED***
using SmartMirror.Core.Services.LedControl;

namespace SmartMirror.Core
***REMOVED***
    class Program
    ***REMOVED***
        private static IServiceProvider Container ***REMOVED*** get; set; ***REMOVED***
        public static ILogger ProgramLogger;
        private static bool _isCleaning;
        private static readonly CancellationTokenSource AppCancellationTokenSource = new CancellationTokenSource();

        static async Task Main(string[] args)
        ***REMOVED***
            using IHost host = CreateHostBuilder(args).Build();
            Container = host.Services;
            if (Container == null)
                throw new ArgumentNullException(nameof(Container));
            ProgramLogger = host.Services.GetRequiredService<ILoggerFactory>()
                .CreateLogger<Program>();

            ProgramLogger.LogDebug("Container initialized");
            ConfigureConsole();
            ProgramLogger.LogInformation("SmartMirror");
            ProgramLogger.LogInformation($"OS Information: ***REMOVED***System.Runtime.InteropServices.RuntimeInformation.OSDescription***REMOVED***");
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

                var audioService = Container.GetService<ISpeechRecognitionService>();
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
            Container.GetService<ISpeechRecognitionService>()?.StartProcessing();
            Container.GetService<ILedManager>()?.StartProcessing();
            Container.GetService<IMagicMirrorRunner>()?.StartProcessing();
            _ = Task.Run(() => Container.GetService<IKeyboardListener>()?.StartListenKeyCommands(AppCancellationTokenSource.Token));
      ***REMOVED***

        private static void ConfigureConsole()
        ***REMOVED***
            Console.CancelKeyPress += ConsoleOnCancelKeyPress;
            DirectoryInitializer.EnsureCorrectWorkingDirectory(ProgramLogger);
      ***REMOVED***

        #region DI

        static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((_, services) =>
                    services.AddLogging(builder => builder.AddConsole())
                        .AddSmartMirrorServices());

***REMOVED***
  ***REMOVED***
***REMOVED***
