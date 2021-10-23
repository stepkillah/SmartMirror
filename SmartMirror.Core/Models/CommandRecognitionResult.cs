using SmartMirror.Core.VoiceRecognition.Microsoft;

namespace SmartMirror.Core.Models
***REMOVED***
    public class CommandRecognitionResult
    ***REMOVED***
        public bool Success ***REMOVED*** get; private init; ***REMOVED***
        public VoiceCommands Command ***REMOVED*** get; private init; ***REMOVED***
        public object CommandData ***REMOVED*** get; private init; ***REMOVED***

        public static CommandRecognitionResult Failed() => new CommandRecognitionResult() ***REMOVED*** Success = false ***REMOVED***;
        public static CommandRecognitionResult SuccessResult(VoiceCommands command) => new CommandRecognitionResult() ***REMOVED*** Success = true, Command = command ***REMOVED***;
        public static CommandRecognitionResult SuccessResult(VoiceCommands command, object commandData) => new CommandRecognitionResult() ***REMOVED*** Success = true, Command = command, CommandData = commandData ***REMOVED***;
  ***REMOVED***
***REMOVED***
