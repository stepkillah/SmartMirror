***REMOVED***
using System.Collections.Generic;
using System.ComponentModel;
***REMOVED***
***REMOVED***
***REMOVED***
***REMOVED***
***REMOVED***
using SmartMirror.Core.Helpers;

namespace SmartMirror.Core.VoiceRecognition.Microsoft
***REMOVED***
    public class AudioService : IAudioService, IDisposable
    ***REMOVED***
        private ILogger _logger;

***REMOVED***
***REMOVED***

        private const string ActivationRecognitionTable = "Assets/mirror_activation.table";

***REMOVED***
***REMOVED***
        private readonly KeywordRecognitionModel _keywordModel;
***REMOVED***

        private readonly Dictionary<string, VoiceCommands> _commandsMap = new Dictionary<string, VoiceCommands>()
        ***REMOVED***
            ***REMOVED***VoiceCommands.LedOn.GetAttributeOfType<DescriptionAttribute>().Description, VoiceCommands.LedOn***REMOVED***,
            ***REMOVED***VoiceCommands.LedOff.GetAttributeOfType<DescriptionAttribute>().Description, VoiceCommands.LedOff***REMOVED***
      ***REMOVED***;

        public AudioService(ILogger<AudioService> logger)
        ***REMOVED***
***REMOVED***
***REMOVED***
***REMOVED***

            var keywordRecognitionTablePath = Path.Combine(Directory.GetCurrentDirectory(), ActivationRecognitionTable);
***REMOVED***


            var config = SpeechConfig.FromSubscription("***REMOVED***", "***REMOVED***");
***REMOVED***
      ***REMOVED***


        public event EventHandler<CommandRecognizedEventArgs> CommandRecognized;
***REMOVED***
        ***REMOVED***
***REMOVED***
***REMOVED***
      ***REMOVED***

***REMOVED***
        ***REMOVED***
            if (_isRunning)
            ***REMOVED***
***REMOVED***
                    await _keywordRecognizer.StopRecognitionAsync();
          ***REMOVED***

            _isRunning = false;
      ***REMOVED***


***REMOVED***
***REMOVED***
        ***REMOVED***
***REMOVED***
            ***REMOVED***
***REMOVED***
***REMOVED***
***REMOVED***

***REMOVED***
                ***REMOVED***
***REMOVED***
                        Console.WriteLine($"We recognized: ***REMOVED***result.Text***REMOVED***");
                        RecognizeCommand(result.Text);
***REMOVED***
***REMOVED***
                        Console.WriteLine($"NOMATCH: Speech could not be recognized.");
***REMOVED***
***REMOVED***
                        ***REMOVED***
    ***REMOVED***
    ***REMOVED***
                      ***REMOVED***
              ***REMOVED***
          ***REMOVED***
***REMOVED***
            ***REMOVED***
***REMOVED***
          ***REMOVED***
      ***REMOVED***



***REMOVED***
        ***REMOVED***
***REMOVED***
***REMOVED***
***REMOVED***
***REMOVED***
            ***REMOVED***
***REMOVED***
                    Console.WriteLine($"We recognized keyword: ***REMOVED***result.Text***REMOVED***");
                    await RecognizeCommandAsync();
***REMOVED***
***REMOVED***
                    Console.WriteLine($"NOMATCH: Speech could not be recognized.");
    ***REMOVED***
***REMOVED***
***REMOVED***
                    ***REMOVED***
***REMOVED***
***REMOVED***
                  ***REMOVED***
          ***REMOVED***
      ***REMOVED***

        private void RecognizeCommand(string rawText)
        ***REMOVED***
            var lowerCommand = rawText.ToLowerInvariant().TrimEnd('.');
            if (!_commandsMap.ContainsKey(lowerCommand))
***REMOVED***
            var command = _commandsMap[lowerCommand];
            CommandRecognized?.Invoke(this, new CommandRecognizedEventArgs(command));
      ***REMOVED***


***REMOVED***
        ***REMOVED***
***REMOVED***
            Console.WriteLine($"CANCELED: Reason=***REMOVED***cancellation.Reason***REMOVED***");
***REMOVED***
            Console.WriteLine($"CANCELED: ErrorCode=***REMOVED***cancellation.ErrorCode***REMOVED***");
            Console.WriteLine($"CANCELED: ErrorDetails=***REMOVED***cancellation.ErrorDetails***REMOVED***");
            Console.WriteLine($"CANCELED: Did you update the subscription info?");

      ***REMOVED***
***REMOVED***

***REMOVED***
        protected virtual void Dispose(bool disposing)
        ***REMOVED***
***REMOVED***
***REMOVED***
            ***REMOVED***
***REMOVED***
***REMOVED***
                _keywordRecognizer?.Dispose();
                _keywordRecognizer = null;
***REMOVED***
***REMOVED***
          ***REMOVED***

***REMOVED***
      ***REMOVED***


***REMOVED***
        ***REMOVED***
***REMOVED***
***REMOVED***
      ***REMOVED***
***REMOVED***


  ***REMOVED***
***REMOVED***
