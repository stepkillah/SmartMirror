using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SmartMirror.Core.Common;
using SmartMirror.Core.Extensions;
using SmartMirror.Core.Interfaces;
using SmartMirror.Core.Services.LedControl;

namespace SmartMirror.Core
{
    class Program
    {
        private static IServiceProvider Container { get; set; }
        public static ILogger ProgramLogger;
        private static bool _isCleaning;
        private static readonly CancellationTokenSource AppCancellationTokenSource = new CancellationTokenSource();

        static async Task Main(string[] args)
        {
            using IHost host = CreateHostBuilder(args).Build();
            Container = host.Services;
            if (Container == null)
                throw new ArgumentNullException(nameof(Container));
            ProgramLogger = host.Services.GetRequiredService<ILoggerFactory>()
                .CreateLogger<Program>();

            ProgramLogger.LogDebug("Container initialized");
            ConfigureConsole();
            ProgramLogger.LogInformation("SmartMirror");
            ProgramLogger.LogInformation($"OS Information: {System.Runtime.InteropServices.RuntimeInformation.OSDescription}");
            StartProgram();
            ProgramLogger.LogInformation("Program services started");
            await host.StartAsync(AppCancellationTokenSource.Token);
            ProgramLogger.LogInformation("App successfully started");
            await host.WaitForShutdownAsync(AppCancellationTokenSource.Token);
        }

        private static void ConsoleOnCancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            e.Cancel = true;
            CleanupAndClose().ConfigureAwait(false);
        }

        private static async Task CleanupAndClose()
        {
            if (_isCleaning)
            {
                ProgramLogger.LogInformation("Cleaning already started.");
                return;
            }

            try
            {
                _isCleaning = true;
                ProgramLogger.LogInformation("Cleaning started");

                var speechService = Container.GetService<ISpeechRecognitionService>();
                if (speechService != null)
                {
                    await speechService.StopProcessing();
                    speechService.Dispose();
                }

                var ledManager = Container.GetService<ILedManager>();
                ledManager?.Dispose();

                var magicMirrorRunner = Container.GetService<IMagicMirrorRunner>();
                magicMirrorRunner?.Dispose();

                ProgramLogger.LogInformation("Cleaning finished\nClosing app...");
                AppCancellationTokenSource.Cancel();
            }
            finally
            {
                _isCleaning = false;
            }
        }


        private static void StartProgram()
        {
            Container.GetService<ISpeechRecognitionService>()?.StartProcessing();
            Container.GetService<ILedManager>()?.StartProcessing();
            Container.GetService<IMagicMirrorRunner>()?.StartProcessing();
            _ = Task.Run(() => Container.GetService<IKeyboardListener>()?.StartListenKeyCommands(AppCancellationTokenSource.Token));
        }

        private static void ConfigureConsole()
        {
            Console.CancelKeyPress += ConsoleOnCancelKeyPress;
            DirectoryInitializer.EnsureCorrectWorkingDirectory(ProgramLogger);
        }

        #region DI

        static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((context, services) =>
                    services.AddLogging(builder => builder.AddConsole())
                        .ConfigureSmartMirrorOptions(context)
                        .AddSmartMirrorServices());

        #endregion
    }
}
