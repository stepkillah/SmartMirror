***REMOVED***
using System.Diagnostics;
***REMOVED***
using System.Linq;
***REMOVED***
using DeepSpeechClient.Interfaces;
using DeepSpeechClient.Models;
***REMOVED***
using NAudio.Wave;
using SmartMirror.Core.VoiceRecognition.Microsoft;

namespace SmartMirror.Core.VoiceRecognition.DeepSpeech
***REMOVED***
    public class DeepSpeechAudioManager : IAudioService
    ***REMOVED***
***REMOVED***
        private bool _disposedValue;

        public DeepSpeechAudioManager(ILogger<DeepSpeechAudioManager> logger)
        ***REMOVED***
***REMOVED***

            string scorer = null;
            string audio = "VoiceRecognition/DeepSpeech/arctic_a0024.wav";
            bool extended = false;
            string model = "VoiceRecognition/DeepSpeech/deepspeech-0.8.2-models.pbmm";
            model = Path.Combine(Directory.GetCurrentDirectory(), model);
            Stopwatch stopwatch = new Stopwatch();
***REMOVED***
            ***REMOVED***
                Console.WriteLine("Loading model...");
                stopwatch.Start();
                // sphinx-doc: csharp_ref_model_start
                using IDeepSpeech sttClient = new DeepSpeechClient.DeepSpeech(model ?? "deepspeech-0.8.2-models.pbmm");
                // sphinx-doc: csharp_ref_model_stop
                stopwatch.Stop();

                Console.WriteLine($"Model loaded - ***REMOVED***stopwatch.Elapsed.Milliseconds***REMOVED*** ms");
                stopwatch.Reset();
                if (scorer != null)
                ***REMOVED***
                    Console.WriteLine("Loading scorer...");
                    sttClient.EnableExternalScorer(scorer ?? "kenlm.scorer");
              ***REMOVED***
                audio = Path.Combine(Directory.GetCurrentDirectory(), audio);
                string audioFile = audio;
                var waveBuffer = new WaveBuffer(File.ReadAllBytes(audioFile));
                using (var waveInfo = new WaveFileReader(audioFile))
                ***REMOVED***
                    Console.WriteLine("Running inference....");

                    stopwatch.Start();

                    string speechResult;
                    // sphinx-doc: csharp_ref_inference_start
                    if (extended)
                    ***REMOVED***
                        Metadata metaResult = sttClient.SpeechToTextWithMetadata(waveBuffer.ShortBuffer,
                            Convert.ToUInt32(waveBuffer.MaxSize / 2), 1);
                        speechResult = MetadataToString(metaResult.Transcripts[0]);
                  ***REMOVED***
                    else
                    ***REMOVED***
                        speechResult = sttClient.SpeechToText(waveBuffer.ShortBuffer,
                            Convert.ToUInt32(waveBuffer.MaxSize / 2));
                  ***REMOVED***
                    // sphinx-doc: csharp_ref_inference_stop

                    stopwatch.Stop();

                    Console.WriteLine($"Audio duration: ***REMOVED***waveInfo.TotalTime.ToString()***REMOVED***");
                    Console.WriteLine($"Inference took: ***REMOVED***stopwatch.Elapsed.ToString()***REMOVED***");
                    Console.WriteLine((extended ? $"Extended result: " : "Recognized text: ") + speechResult);
              ***REMOVED***
                waveBuffer.Clear();
          ***REMOVED***
***REMOVED***
            ***REMOVED***
                Console.WriteLine(ex.Message);
          ***REMOVED***
      ***REMOVED***

        static string MetadataToString(CandidateTranscript transcript)
        ***REMOVED***
            var nl = Environment.NewLine;
            string retval =
                Environment.NewLine + $"Recognized text: ***REMOVED***string.Join("", transcript?.Tokens?.Select(x => x.Text))***REMOVED*** ***REMOVED***nl***REMOVED***"
                                    + $"Confidence: ***REMOVED***transcript?.Confidence***REMOVED*** ***REMOVED***nl***REMOVED***"
                                    + $"Item count: ***REMOVED***transcript?.Tokens?.Length***REMOVED*** ***REMOVED***nl***REMOVED***"
                                    + string.Join(nl, transcript?.Tokens?.Select(x => $"Timestep : ***REMOVED***x.Timestep***REMOVED*** TimeOffset: ***REMOVED***x.StartTime***REMOVED*** Char: ***REMOVED***x.Text***REMOVED***"));
            return retval;
      ***REMOVED***


***REMOVED***
        ***REMOVED***
      ***REMOVED***

        public Task StopProcessing()
        ***REMOVED***
            return Task.CompletedTask;
      ***REMOVED***

***REMOVED***

        private void StartKeywordRecognition()
        ***REMOVED***

      ***REMOVED***

***REMOVED***

        #region disposing
        protected virtual void Dispose(bool disposing)
        ***REMOVED***
            if (_disposedValue) return;

***REMOVED***
            ***REMOVED***
                // TODO: dispose managed state (managed objects)
          ***REMOVED***

            _disposedValue = true;
      ***REMOVED***


***REMOVED***
        ***REMOVED***
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
***REMOVED***
***REMOVED***
      ***REMOVED*** 
***REMOVED***
  ***REMOVED***
***REMOVED***
