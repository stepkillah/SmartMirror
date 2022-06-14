***REMOVED***
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
***REMOVED***
***REMOVED***
using SmartMirror.Core.Enums;
using SmartMirror.Core.Helpers;
***REMOVED***
***REMOVED***
using SmartMirror.Core.Services.LedControl;

namespace SmartMirror.Core.Services
***REMOVED***
    public class CommandsHandler : ICommandsHandler
    ***REMOVED***
        #region private fields

        private readonly ILedManager _ledManager;
***REMOVED***
***REMOVED***

        private readonly Dictionary<string, VoiceCommands> _commandsMapTable = new Dictionary<string, VoiceCommands>()
        ***REMOVED***
            ***REMOVED***VoiceCommands.LedOn.GetAttributeOfType<DescriptionAttribute>().Description, VoiceCommands.LedOn***REMOVED***,
            ***REMOVED***VoiceCommands.LedOff.GetAttributeOfType<DescriptionAttribute>().Description, VoiceCommands.LedOff***REMOVED***,
            ***REMOVED***VoiceCommands.LedColorSet.GetAttributeOfType<DescriptionAttribute>().Description, VoiceCommands.LedColorSet***REMOVED***,
            ***REMOVED***VoiceCommands.PlayTestSound.GetAttributeOfType<DescriptionAttribute>().Description, VoiceCommands.PlayTestSound***REMOVED***
      ***REMOVED***;
***REMOVED***

        #region ctor
        public CommandsHandler(ILedManager ledManager, IAudioPlayer audioPlayer, ILogger<CommandsHandler> logger)
        ***REMOVED***
            _ledManager = ledManager;
***REMOVED***
***REMOVED***
      ***REMOVED***
***REMOVED***


        public ValueTask HandleCommand(VoiceCommands command, object data, CancellationToken cancellationToken = default)
        ***REMOVED***
            switch (command)
            ***REMOVED***
                case VoiceCommands.LedOn:
                    _ledManager.TurnOn();
***REMOVED***
                case VoiceCommands.LedOff:
                    _ledManager.TurnOff();
***REMOVED***
                case VoiceCommands.LedColorSet:
                    if (data is Color color)
                        _ledManager.TurnOn(color);
***REMOVED***
                case VoiceCommands.PlayTestSound:
                    _audioPlayer.Play(Constants.SuccessSoundPath, cancellationToken);
***REMOVED***
                default:
                    _logger.LogInformation("Unknown command");
***REMOVED***
          ***REMOVED***
            return ValueTask.CompletedTask;
      ***REMOVED***

        public ValueTask<CommandRecognitionResult> RecognizeCommand(string rawText)
        ***REMOVED***
            if (rawText == null)
                return ValueTask.FromResult(CommandRecognitionResult.Failed());

            var lowerCommand = rawText.ToLowerInvariant().TrimEnd('.').Trim(' ');
            object commandData = null;
            if (lowerCommand.Contains(' ') && lowerCommand.StartsWith("color"))
            ***REMOVED***
                var parsed = lowerCommand.Split(' ');
                lowerCommand = parsed[0];
                if (parsed.Length > 1)
                    commandData = GetColorFromCommand(parsed[1]);
          ***REMOVED***

            return !_commandsMapTable.ContainsKey(lowerCommand)
                ? ValueTask.FromResult(CommandRecognitionResult.Failed())
                : ValueTask.FromResult(CommandRecognitionResult.SuccessResult(_commandsMapTable[lowerCommand], commandData));
      ***REMOVED***

        private Color? GetColorFromCommand(string command)
        ***REMOVED***
***REMOVED***
            ***REMOVED***
                return ColorTranslator.FromHtml(command);
          ***REMOVED***
            catch (Exception e)
            ***REMOVED***
                _logger.LogError(e, $"***REMOVED***nameof(CommandsHandler)***REMOVED***: Parsing color failed");
                return null;
          ***REMOVED***
      ***REMOVED***
  ***REMOVED***
***REMOVED***
