using SmartMirror.Core.Enums;

namespace SmartMirror.Core.Models
{
    public class CommandRecognitionResult
    {
        public bool Success { get; private init; }
        public SmartMirrorCommand Command { get; private init; }
        public object CommandData { get; private init; }

        public static CommandRecognitionResult Failed() => new() { Success = false };
        public static CommandRecognitionResult SuccessResult(SmartMirrorCommand command) => new() { Success = true, Command = command };
        public static CommandRecognitionResult SuccessResult(SmartMirrorCommand command, object commandData) => new() { Success = true, Command = command, CommandData = commandData };
    }
}
