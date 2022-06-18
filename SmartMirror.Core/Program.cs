using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SmartMirror.Core.Common;
using SmartMirror.Core.Extensions;

namespace SmartMirror.Core
{
    class Program
    {
        private static IServiceProvider Container { get; set; }
        public static ILogger ProgramLogger;
        private static readonly CancellationTokenSource AppCancellationTokenSource = new CancellationTokenSource();

        static async Task Main(string[] args)
        {

            DirectoryInitializer.EnsureCorrectWorkingDirectory(LoggerFactory.Create(builder => builder.AddSmartMirrorLogging()).CreateLogger<Program>());
            using IHost host = CreateHostBuilder(args).Build();
            Container = host.Services;
            if (Container == null)
                throw new ArgumentNullException(nameof(Container));
            ProgramLogger = host.Services.GetRequiredService<ILoggerFactory>()
                .CreateLogger<Program>();
            ProgramLogger.LogDebug("Container initialized");
            ProgramLogger.LogInformation("SmartMirror");
            ProgramLogger.LogInformation(
                $"OS Information: {System.Runtime.InteropServices.RuntimeInformation.OSDescription}");
            ProgramLogger.LogInformation("Program logger created");
            await host.StartAsync(AppCancellationTokenSource.Token);
            ProgramLogger.LogInformation("App successfully started");
            await host.WaitForShutdownAsync(AppCancellationTokenSource.Token);
        }
        

        #region DI

        static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((context, services) =>
                    services.AddLogging(builder => builder.AddSmartMirrorLogging())
                        .ConfigureSmartMirrorOptions(context)
                        .AddSmartMirrorServices());

        #endregion
    }
}
