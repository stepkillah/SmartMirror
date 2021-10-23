***REMOVED***
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
***REMOVED***
***REMOVED***
using SmartMirror.Core.VoiceRecognition.Microsoft;

namespace SmartMirror.Core.Helpers
***REMOVED***
    public static class MirrorCommandsHelper
    ***REMOVED***

        public static readonly Dictionary<string, VoiceCommands> CommandsMapTable = new Dictionary<string, VoiceCommands>()
        ***REMOVED***
            ***REMOVED***VoiceCommands.LedOn.GetAttributeOfType<DescriptionAttribute>().Description, VoiceCommands.LedOn***REMOVED***,
            ***REMOVED***VoiceCommands.LedOff.GetAttributeOfType<DescriptionAttribute>().Description, VoiceCommands.LedOff***REMOVED***,
            ***REMOVED***VoiceCommands.LedColorSet.GetAttributeOfType<DescriptionAttribute>().Description, VoiceCommands.LedColorSet***REMOVED***
      ***REMOVED***;

        public static CommandRecognitionResult RecognizeCommand(string rawText, ILogger logger)
        ***REMOVED***
            if (rawText == null)
                return CommandRecognitionResult.Failed();

            var lowerCommand = rawText.ToLowerInvariant().TrimEnd('.').Trim(' ');
            object commandData = null;
            if (lowerCommand.Contains(' ') && lowerCommand.StartsWith("color"))
            ***REMOVED***
                var parsed = lowerCommand.Split(' ');
                lowerCommand = parsed[0];
                if (parsed.Length > 1)
                    commandData = GetColorFromCommand(parsed[1], logger);
          ***REMOVED***

            return !CommandsMapTable.ContainsKey(lowerCommand)
                ? CommandRecognitionResult.Failed()
                : CommandRecognitionResult.SuccessResult(CommandsMapTable[lowerCommand], commandData);
      ***REMOVED***

        private static Color? GetColorFromCommand(string command, ILogger logger)
        ***REMOVED***
***REMOVED***
            ***REMOVED***
                return ColorTranslator.FromHtml(command);
          ***REMOVED***
            catch (Exception e)
            ***REMOVED***
                logger.LogError(e, $"***REMOVED***nameof(AudioService)***REMOVED***: Parsing color failed");
                return null;
          ***REMOVED***
      ***REMOVED***
  ***REMOVED***
***REMOVED***
