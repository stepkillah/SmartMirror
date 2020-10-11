***REMOVED***
***REMOVED***
***REMOVED***
***REMOVED***

namespace SmartMirror.Core
***REMOVED***
    public class AudioService
    ***REMOVED***
        public async Task StartRecord()
        ***REMOVED***
            //using var startRes = $"arecord -D plughw:1,0 test.wav".StartBash();
            //await Task.Delay(TimeSpan.FromSeconds(10));
            //startRes.Kill();
            //await RecognizeSpeechAsync();
      ***REMOVED***


        private async Task RecognizeSpeechAsync()
        ***REMOVED***
            var config = SpeechConfig.FromSubscription("***REMOVED***", "***REMOVED***");

            using var audioConfig = AudioConfig.FromDefaultMicrophoneInput();

            using var recognizer = new SpeechRecognizer(config, audioConfig);
            var result = await recognizer.RecognizeOnceAsync();

***REMOVED***
            ***REMOVED***
                case ResultReason.RecognizedSpeech:
                    Console.WriteLine($"We recognized: ***REMOVED***result.Text***REMOVED***");
***REMOVED***
***REMOVED***
                    Console.WriteLine($"NOMATCH: Speech could not be recognized.");
***REMOVED***
***REMOVED***
                    ***REMOVED***
            ***REMOVED***
                        Console.WriteLine($"CANCELED: Reason=***REMOVED***cancellation.Reason***REMOVED***");

                        if (cancellation.Reason == CancellationReason.Error)
                        ***REMOVED***
                            Console.WriteLine($"CANCELED: ErrorCode=***REMOVED***cancellation.ErrorCode***REMOVED***");
                            Console.WriteLine($"CANCELED: ErrorDetails=***REMOVED***cancellation.ErrorDetails***REMOVED***");
                            Console.WriteLine($"CANCELED: Did you update the subscription info?");
                      ***REMOVED***

***REMOVED***
                  ***REMOVED***
          ***REMOVED***
      ***REMOVED***

  ***REMOVED***
***REMOVED***
