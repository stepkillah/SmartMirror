using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SmartMirror.Core.Interfaces;

namespace SmartMirror.Core.Services
{
    public class KeyboardListener : BackgroundService
    {
        private readonly ILogger _logger;
        private readonly IServiceProvider _serviceProvider;

        public KeyboardListener(ILogger<KeyboardListener> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (Debugger.IsAttached && RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    Console.Read();
                }
                else
                {
                    await using var scope = _serviceProvider.CreateAsyncScope();
                    var commandsHandler = scope.ServiceProvider.GetRequiredService<ICommandsHandler>();
                    try
                    {
                        await Task.Run(() => WaitForCommand(commandsHandler, stoppingToken), stoppingToken);
                    }
                    catch (OperationCanceledException e)
                    {
                        _logger.LogWarning(e, "Canceled keyboard listener");
                    }
                }
            }
        }


        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting waiting for keyboard commands");
            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Stopping waiting for keyboard commands");
            return base.StopAsync(cancellationToken);
        }

        private async Task WaitForCommand(ICommandsHandler commandsHandler, CancellationToken stoppingToken)
        {
            Console.WriteLine("Waiting for key command");
            string line = Console.ReadLine();
            if (line == null)
                return;
            var result = await commandsHandler.RecognizeCommand(line);
            if (!result.Success)
            {
                if (!string.IsNullOrEmpty(line))
                    Console.WriteLine("Command not recognized");
                return;
            }

            await commandsHandler.HandleCommand(result.Command, result.CommandData, stoppingToken);
        }
    }
}
