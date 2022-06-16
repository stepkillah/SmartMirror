using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SmartMirror.Core.Enums;
using SmartMirror.Core.Helpers;
using SmartMirror.Core.Interfaces;
using SmartMirror.Core.Models;
using SmartMirror.Core.Services.LedControl;

namespace SmartMirror.Core.Services
{
    public class CommandsHandler : ICommandsHandler
    {
        #region private fields

        private readonly ILedManager _ledManager;
        private readonly IAudioPlayer _audioPlayer;
        private readonly ILogger _logger;

        private readonly Dictionary<string, VoiceCommands> _commandsMapTable = new Dictionary<string, VoiceCommands>()
        {
            {VoiceCommands.LedOn.GetAttributeOfType<DescriptionAttribute>().Description, VoiceCommands.LedOn},
            {VoiceCommands.LedOff.GetAttributeOfType<DescriptionAttribute>().Description, VoiceCommands.LedOff},
            {VoiceCommands.LedColorSet.GetAttributeOfType<DescriptionAttribute>().Description, VoiceCommands.LedColorSet},
            {VoiceCommands.PlayTestSound.GetAttributeOfType<DescriptionAttribute>().Description, VoiceCommands.PlayTestSound}
        };
        #endregion

        #region ctor
        public CommandsHandler(ILedManager ledManager, IAudioPlayer audioPlayer, ILogger<CommandsHandler> logger)
        {
            _ledManager = ledManager;
            _audioPlayer = audioPlayer;
            _logger = logger;
        }
        #endregion


        public ValueTask HandleCommand(VoiceCommands command, object data, CancellationToken cancellationToken = default)
        {
            switch (command)
            {
                case VoiceCommands.LedOn:
                    _ledManager.TurnOn();
                    break;
                case VoiceCommands.LedOff:
                    _ledManager.TurnOff();
                    break;
                case VoiceCommands.LedColorSet:
                    if (data is Color color)
                        _ledManager.TurnOn(color);
                    break;
                case VoiceCommands.PlayTestSound:
                    _audioPlayer.Play(Constants.SuccessSoundPath, cancellationToken);
                    break;
                default:
                    _logger.LogWarning("Unknown command");
                    break;
            }
            return ValueTask.CompletedTask;
        }

        public ValueTask<CommandRecognitionResult> RecognizeCommand(string rawText)
        {
            if (rawText == null)
                return ValueTask.FromResult(CommandRecognitionResult.Failed());

            var lowerCommand = rawText.ToLowerInvariant().TrimEnd('.').Trim(' ');
            object commandData = null;
            if (lowerCommand.Contains(' ') && lowerCommand.StartsWith("color"))
            {
                var parsed = lowerCommand.Split(' ');
                lowerCommand = parsed[0];
                if (parsed.Length > 1)
                    commandData = GetColorFromCommand(parsed[1]);
            }

            return !_commandsMapTable.ContainsKey(lowerCommand)
                ? ValueTask.FromResult(CommandRecognitionResult.Failed())
                : ValueTask.FromResult(CommandRecognitionResult.SuccessResult(_commandsMapTable[lowerCommand], commandData));
        }

        private Color? GetColorFromCommand(string command)
        {
            try
            {
                return ColorTranslator.FromHtml(command);
            }
            catch (ArgumentException e)
            {
                _logger.LogWarning(e, $"{nameof(CommandsHandler)}: Parsing color failed");
                return null;
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"{nameof(CommandsHandler)}: Parsing color failed");
                return null;
            }
        }
    }
}
