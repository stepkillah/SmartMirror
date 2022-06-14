***REMOVED***
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
***REMOVED***
***REMOVED***

namespace SmartMirror.Core.Services
***REMOVED***
    public class KeyboardListener : IKeyboardListener
    ***REMOVED***
***REMOVED***
***REMOVED***

        public KeyboardListener(ILogger<KeyboardListener> logger, ICommandsHandler commandsHandler)
        ***REMOVED***
***REMOVED***
***REMOVED***
      ***REMOVED***

        public async void StartListenKeyCommands(CancellationToken cancellationToken)
        ***REMOVED***
            _logger.LogInformation("Starting waiting for keyboard commands");
            while (!cancellationToken.IsCancellationRequested)
            ***REMOVED***
                if (Debugger.IsAttached && RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                ***REMOVED***
                    Console.Read();
              ***REMOVED***
                else
                ***REMOVED***
                    Console.WriteLine("Waiting for key command");
                    var line = Console.ReadLine();
                    if (line == null)
    ***REMOVED***
                    var result = await _commandsHandler.RecognizeCommand(line);
        ***REMOVED***
    ***REMOVED***
                    await _commandsHandler.HandleCommand(result.Command, result.CommandData, cancellationToken);
              ***REMOVED***
          ***REMOVED***
      ***REMOVED***
  ***REMOVED***
***REMOVED***
