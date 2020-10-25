//using System;
//using System.Diagnostics;
//***REMOVED***
//using System.Linq;
//using DeepSpeechClient.Interfaces;
//using DeepSpeechClient.Models;
//using NAudio.Wave;

//namespace SmartMirror.Core.VoiceRecognition.DeepSpeech
//***REMOVED***
//    public class DeepSpeechAudioManager
//    ***REMOVED***
//        public DeepSpeechAudioManager()
//        ***REMOVED***

//            string scorer = null;
//            string audio = "VoiceRecognition/DeepSpeech/arctic_a0024.wav";
//            bool extended = false;
//            string model = "VoiceRecognition/DeepSpeech/deepspeech-0.8.2-models.tflite";
//            model = Path.Combine(Directory.GetCurrentDirectory(), model);
//            Stopwatch stopwatch = new Stopwatch();
//***REMOVED***
//            ***REMOVED***
//                Console.WriteLine("Loading model...");
//                stopwatch.Start();
//                // sphinx-doc: csharp_ref_model_start
//                using IDeepSpeech sttClient = new DeepSpeechClient.DeepSpeech(model ?? "deepspeech-0.8.2-models.tflite");
//                // sphinx-doc: csharp_ref_model_stop
//                stopwatch.Stop();

//                Console.WriteLine($"Model loaded - ***REMOVED***stopwatch.Elapsed.Milliseconds***REMOVED*** ms");
//                stopwatch.Reset();
//                if (scorer != null)
//                ***REMOVED***
//                    Console.WriteLine("Loading scorer...");
//                    sttClient.EnableExternalScorer(scorer ?? "kenlm.scorer");
//              ***REMOVED***
//                audio = Path.Combine(Directory.GetCurrentDirectory(), audio);
//                string audioFile = audio;
//                var waveBuffer = new WaveBuffer(File.ReadAllBytes(audioFile));
//                using (var waveInfo = new WaveFileReader(audioFile))
//                ***REMOVED***
//                    Console.WriteLine("Running inference....");

//                    stopwatch.Start();

//                    string speechResult;
//                    // sphinx-doc: csharp_ref_inference_start
//                    if (extended)
//                    ***REMOVED***
//                        Metadata metaResult = sttClient.SpeechToTextWithMetadata(waveBuffer.ShortBuffer,
//                            Convert.ToUInt32(waveBuffer.MaxSize / 2), 1);
//                        speechResult = MetadataToString(metaResult.Transcripts[0]);
//                  ***REMOVED***
//                    else
//                    ***REMOVED***
//                        speechResult = sttClient.SpeechToText(waveBuffer.ShortBuffer,
//                            Convert.ToUInt32(waveBuffer.MaxSize / 2));
//                  ***REMOVED***
//                    // sphinx-doc: csharp_ref_inference_stop

//                    stopwatch.Stop();

//                    Console.WriteLine($"Audio duration: ***REMOVED***waveInfo.TotalTime.ToString()***REMOVED***");
//                    Console.WriteLine($"Inference took: ***REMOVED***stopwatch.Elapsed.ToString()***REMOVED***");
//                    Console.WriteLine((extended ? $"Extended result: " : "Recognized text: ") + speechResult);
//              ***REMOVED***
//                waveBuffer.Clear();
//          ***REMOVED***
//***REMOVED***
//            ***REMOVED***
//                Console.WriteLine(ex.Message);
//          ***REMOVED***
//      ***REMOVED***

//        static string MetadataToString(CandidateTranscript transcript)
//        ***REMOVED***
//            var nl = Environment.NewLine;
//            string retval =
//                Environment.NewLine + $"Recognized text: ***REMOVED***string.Join("", transcript?.Tokens?.Select(x => x.Text))***REMOVED*** ***REMOVED***nl***REMOVED***"
//                                    + $"Confidence: ***REMOVED***transcript?.Confidence***REMOVED*** ***REMOVED***nl***REMOVED***"
//                                    + $"Item count: ***REMOVED***transcript?.Tokens?.Length***REMOVED*** ***REMOVED***nl***REMOVED***"
//                                    + string.Join(nl, transcript?.Tokens?.Select(x => $"Timestep : ***REMOVED***x.Timestep***REMOVED*** TimeOffset: ***REMOVED***x.StartTime***REMOVED*** Char: ***REMOVED***x.Text***REMOVED***"));
//            return retval;
//      ***REMOVED***
//  ***REMOVED***
//***REMOVED***
