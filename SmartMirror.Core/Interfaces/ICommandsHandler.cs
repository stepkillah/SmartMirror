using System.Threading;
using System.Threading.Tasks;
using SmartMirror.Core.Enums;
using SmartMirror.Core.Models;

namespace SmartMirror.Core.Interfaces
{
    public interface ICommandsHandler
    {
        ValueTask HandleCommand(VoiceCommands command, object data, CancellationToken cancellationToken = default);
        ValueTask<CommandRecognitionResult> RecognizeCommand(string rawText);
    }
}
