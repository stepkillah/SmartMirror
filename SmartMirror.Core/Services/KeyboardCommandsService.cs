***REMOVED***
using System.Diagnostics;
using System.Drawing;
using System.Threading;
***REMOVED***
using SmartMirror.Core.Helpers;
***REMOVED***
using SmartMirror.Core.LedControl;
using SmartMirror.Core.VoiceRecognition.Microsoft;

namespace SmartMirror.Core.Services
***REMOVED***
    public class KeyboardCommandsService : IKeyboardCommandsService
    ***REMOVED***
***REMOVED***
        private readonly ILedManager _ledManager;

        public KeyboardCommandsService(ILedManager ledManager, ILogger<KeyboardCommandsService> logger)
        ***REMOVED***
***REMOVED***
            _ledManager = ledManager;
      ***REMOVED***

        public void StartListenKeyCommands(CancellationToken cancellationToken)
        ***REMOVED***
            _logger.LogInformation("Starting waiting for keyboard commands");
            while (!cancellationToken.IsCancellationRequested)
            ***REMOVED***
                if (Debugger.IsAttached)
                ***REMOVED***
                    Console.Read();
              ***REMOVED***
                else
                ***REMOVED***
                    Console.WriteLine("Waiting for key command");
                    var line = Console.ReadLine();
                    if (line == null)
    ***REMOVED***
                    var result = MirrorCommandsHelper.RecognizeCommand(line, _logger);
        ***REMOVED***
    ***REMOVED***
                    switch (result.Command)
                    ***REMOVED***
                        case VoiceCommands.LedOn:
                            _ledManager.TurnOn();
    ***REMOVED***
                        case VoiceCommands.LedOff:
                            _ledManager.TurnOff();
    ***REMOVED***
                        case VoiceCommands.LedColorSet:
                            if (result.CommandData is Color color)
                                _ledManager.TurnOn(color);
    ***REMOVED***
                        default:
                            _logger.LogInformation("Unknown command");
        ***REMOVED***
                  ***REMOVED***
              ***REMOVED***
          ***REMOVED***
      ***REMOVED***
  ***REMOVED***
***REMOVED***
