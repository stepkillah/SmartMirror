using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using Microsoft.Extensions.Logging;
using SmartMirror.Core.Interfaces;

namespace SmartMirror.Core.Services
{
    public class KeyboardListener : IKeyboardListener
    {
        private readonly ILogger _logger;
        private readonly ICommandsHandler _commandsHandler;

        public KeyboardListener(ILogger<KeyboardListener> logger, ICommandsHandler commandsHandler)
        {
            _logger = logger;
            _commandsHandler = commandsHandler;
        }

        public async void StartListenKeyCommands(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting waiting for keyboard commands");
            while (!cancellationToken.IsCancellationRequested)
            {
                if (Debugger.IsAttached && RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    Console.Read();
                }
                else
                {
                    Console.WriteLine("Waiting for key command");
                    var line = Console.ReadLine();
                    if (line == null)
                        return;
                    var result = await _commandsHandler.RecognizeCommand(line);
                    if (!result.Success)
                        return;
                    await _commandsHandler.HandleCommand(result.Command, result.CommandData, cancellationToken);
                }
            }
        }
    }
}
