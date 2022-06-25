using System;
using SmartMirror.Core.Enums;

namespace SmartMirror.Core.Services.SpeechRecognition.Microsoft
{
    public class CommandRecognizedEventArgs : EventArgs
    {
        public CommandRecognizedEventArgs(SmartMirrorCommand command)
        {
            Command = command;
        }

        public CommandRecognizedEventArgs(SmartMirrorCommand command, object data) : this(command)
        {
            Data = data;
        }

        public object Data { get; }
        public SmartMirrorCommand Command { get; }
    }
}
