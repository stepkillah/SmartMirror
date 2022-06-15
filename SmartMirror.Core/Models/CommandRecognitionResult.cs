using SmartMirror.Core.Enums;

namespace SmartMirror.Core.Models
{
    public class CommandRecognitionResult
    {
        public bool Success { get; private init; }
        public VoiceCommands Command { get; private init; }
        public object CommandData { get; private init; }

        public static CommandRecognitionResult Failed() => new CommandRecognitionResult() { Success = false };
        public static CommandRecognitionResult SuccessResult(VoiceCommands command) => new CommandRecognitionResult() { Success = true, Command = command };
        public static CommandRecognitionResult SuccessResult(VoiceCommands command, object commandData) => new CommandRecognitionResult() { Success = true, Command = command, CommandData = commandData };
    }
}
