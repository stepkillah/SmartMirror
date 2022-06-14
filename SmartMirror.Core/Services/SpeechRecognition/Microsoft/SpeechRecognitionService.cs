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

        private const string ActivationRecognitionTable = "Assets/mirror_activation.table";

***REMOVED***
***REMOVED***
***REMOVED***
***REMOVED***


***REMOVED***
***REMOVED***
***REMOVED***
            IAudioPlayer audioPlayer)
        ***REMOVED***
***REMOVED***
***REMOVED***
***REMOVED***
***REMOVED***
***REMOVED***

            var keywordRecognitionTablePath = Path.Combine(Directory.GetCurrentDirectory(), ActivationRecognitionTable);
***REMOVED***


            var config = SpeechConfig.FromSubscription("***REMOVED***", "***REMOVED***");
***REMOVED***
      ***REMOVED***
        
***REMOVED***
        ***REMOVED***
***REMOVED***
***REMOVED***
            _logger.LogInformation($"***REMOVED***nameof(SpeechRecognitionService)***REMOVED*** started.");
      ***REMOVED***

***REMOVED***
        ***REMOVED***
***REMOVED***
            ***REMOVED***
                _logger.LogInformation($"***REMOVED***nameof(SpeechRecognitionService)***REMOVED***: Stop***REMOVED***ng");
***REMOVED***
                ***REMOVED***
***REMOVED***
***REMOVED***
              ***REMOVED***

***REMOVED***
                _logger.LogInformation($"***REMOVED***nameof(SpeechRecognitionService)***REMOVED***: Stopped");
          ***REMOVED***
***REMOVED***
            ***REMOVED***
                _logger.LogError(ex, $"***REMOVED***nameof(SpeechRecognitionService)***REMOVED***: Stop***REMOVED***ng error");
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
                        _logger.LogInformation($"We recognized: ***REMOVED***result.Text***REMOVED***");
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
            ***REMOVED***
***REMOVED***
                    _logger.LogInformation($"We recognized keyword: ***REMOVED***result.Text***REMOVED***");
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

***REMOVED***
      ***REMOVED***


***REMOVED***
        ***REMOVED***
***REMOVED***
            _logger.LogInformation($"CANCELED: Reason=***REMOVED***cancellation.Reason***REMOVED***");
***REMOVED***
            _logger.LogInformation($"CANCELED: ErrorCode=***REMOVED***cancellation.ErrorCode***REMOVED***");
            _logger.LogInformation($"CANCELED: ErrorDetails=***REMOVED***cancellation.ErrorDetails***REMOVED***");
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

            _logger.LogInformation($"***REMOVED***nameof(SpeechRecognitionService)***REMOVED*** disposed.");
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

            _logger.LogInformation($"***REMOVED***nameof(SpeechRecognitionService)***REMOVED*** disposed.");
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
                    _logger.LogError(e, $"Keyword recognizer disposing failed 3 of 3");
***REMOVED***
              ***REMOVED***
***REMOVED***
                _logger.LogWarning(e, $"Keyword recognizer disposing failed. Trying ***REMOVED***_triesCount***REMOVED*** of ***REMOVED***_maxTriesCount***REMOVED*** tries");
***REMOVED***
          ***REMOVED***
      ***REMOVED***
***REMOVED***


  ***REMOVED***
***REMOVED***
