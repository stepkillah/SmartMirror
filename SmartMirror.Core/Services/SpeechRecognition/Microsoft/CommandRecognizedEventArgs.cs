using System;
using SmartMirror.Core.Enums;

namespace SmartMirror.Core.Services.SpeechRecognition.Microsoft
{
    public class CommandRecognizedEventArgs : EventArgs
    {
        public CommandRecognizedEventArgs(VoiceCommands command)
        {
            Command = command;
        }

        public CommandRecognizedEventArgs(VoiceCommands command, object data) : this(command)
        {
            Data = data;
        }

        public object Data { get; }
        public VoiceCommands Command { get; }
    }
}
