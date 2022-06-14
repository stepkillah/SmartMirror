using System.Threading;
***REMOVED***
using SmartMirror.Core.Enums;
***REMOVED***

namespace SmartMirror.Core.Interfaces
***REMOVED***
    public interface ICommandsHandler
    ***REMOVED***
        ValueTask HandleCommand(VoiceCommands command, object data, CancellationToken cancellationToken = default);
        ValueTask<CommandRecognitionResult> RecognizeCommand(string rawText);
  ***REMOVED***
***REMOVED***
