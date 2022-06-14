***REMOVED***
using SmartMirror.Core.Enums;

***REMOVED***
***REMOVED***
    public class CommandRecognizedEventArgs : EventArgs
    ***REMOVED***
        public CommandRecognizedEventArgs(VoiceCommands command)
        ***REMOVED***
            Command = command;
      ***REMOVED***

        public CommandRecognizedEventArgs(VoiceCommands command, object data) : this(command)
        ***REMOVED***
            Data = data;
      ***REMOVED***

        public object Data ***REMOVED*** get; ***REMOVED***
        public VoiceCommands Command ***REMOVED*** get; ***REMOVED***
  ***REMOVED***
***REMOVED***
