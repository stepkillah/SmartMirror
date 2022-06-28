using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
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
        private readonly IDisplayManager _displayManager;
        private readonly ILogger _logger;

        private readonly Dictionary<string, SmartMirrorCommand> _commandsMapTable = new Dictionary<string, SmartMirrorCommand>()
        {
            {SmartMirrorCommand.LedOn.GetAttributeOfType<DescriptionAttribute>().Description, SmartMirrorCommand.LedOn},
            {SmartMirrorCommand.LedOff.GetAttributeOfType<DescriptionAttribute>().Description, SmartMirrorCommand.LedOff},
            {SmartMirrorCommand.LedColorSet.GetAttributeOfType<DescriptionAttribute>().Description, SmartMirrorCommand.LedColorSet},
            {SmartMirrorCommand.PlayTestSound.GetAttributeOfType<DescriptionAttribute>().Description, SmartMirrorCommand.PlayTestSound},
            {SmartMirrorCommand.DisplayToggle.GetAttributeOfType<DescriptionAttribute>().Description, SmartMirrorCommand.DisplayToggle},
        };
        #endregion

        #region ctor
        public CommandsHandler(ILedManager ledManager, IAudioPlayer audioPlayer, ILogger<CommandsHandler> logger, IDisplayManager displayManager)
        {
            _ledManager = ledManager;
            _audioPlayer = audioPlayer;
            _logger = logger;
            _displayManager = displayManager;
        }
        #endregion


        public async ValueTask HandleCommand(SmartMirrorCommand command, object data, CancellationToken cancellationToken = default)
        {
            switch (command)
            {
                case SmartMirrorCommand.LedOn:
                    _ledManager.TurnOn();
                    break;
                case SmartMirrorCommand.LedOff:
                    _ledManager.TurnOff();
                    break;
                case SmartMirrorCommand.LedColorSet:
                    if (data is Color color)
                        _ledManager.TurnOn(color);
                    break;
                case SmartMirrorCommand.PlayTestSound:
                    await _audioPlayer.Play(Constants.SuccessSoundPath, cancellationToken);
                    break;
                case SmartMirrorCommand.DisplayToggle:
                    await _displayManager.Toggle();
                    break;
                default:
                    _logger.LogWarning("Unknown command");
                    break;
            }
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
                {
                    commandData = GetColorFromCommand(
                        string.Join("", parsed.Skip(1)));
                }
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
