using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NAudio.Wave;
using SmartMirror.Core.Interfaces;

namespace SmartMirror.Core.Services.SpeechRecognition.DeepSpeech
{
    public class SpeechRecognitionService : ISpeechRecognitionService
    {
        private readonly ILogger _logger;
        private bool _disposedValue;

        public SpeechRecognitionService(ILogger<SpeechRecognitionService> logger)
        {
            _logger = logger;

        }
        
        public ValueTask StartProcessing()
        {
            //string scorer = null;
            //string audio = "VoiceRecognition/DeepSpeech/arctic_a0024.wav";
            //bool extended = false;
            //string model = "VoiceRecognition/DeepSpeech/deepspeech-0.8.2-models.pbmm";
            //model = Path.Combine(Directory.GetCurrentDirectory(), model);
            //Stopwatch stopwatch = new Stopwatch();
            //try
            //{
            //    Console.WriteLine("Loading model...");
            //    stopwatch.Start();
            //    // sphinx-doc: csharp_ref_model_start
            //    using IDeepSpeech sttClient = new DeepSpeechClient.DeepSpeech(model ?? "deepspeech-0.8.2-models.pbmm");
            //    // sphinx-doc: csharp_ref_model_stop
            //    stopwatch.Stop();

            //    Console.WriteLine($"Model loaded - {stopwatch.Elapsed.Milliseconds} ms");
            //    stopwatch.Reset();
            //    if (scorer != null)
            //    {
            //        Console.WriteLine("Loading scorer...");
            //        sttClient.EnableExternalScorer(scorer ?? "kenlm.scorer");
            //    }
            //    audio = Path.Combine(Directory.GetCurrentDirectory(), audio);
            //    string audioFile = audio;
            //    var waveBuffer = new WaveBuffer(File.ReadAllBytes(audioFile));
            //    using (var waveInfo = new WaveFileReader(audioFile))
            //    {
            //        Console.WriteLine("Running inference....");

            //        stopwatch.Start();

            //        string speechResult;
            //        // sphinx-doc: csharp_ref_inference_start
            //        if (extended)
            //        {
            //            Metadata metaResult = sttClient.SpeechToTextWithMetadata(waveBuffer.ShortBuffer,
            //                Convert.ToUInt32(waveBuffer.MaxSize / 2), 1);
            //            speechResult = MetadataToString(metaResult.Transcripts[0]);
            //        }
            //        else
            //        {
            //            speechResult = sttClient.SpeechToText(waveBuffer.ShortBuffer,
            //                Convert.ToUInt32(waveBuffer.MaxSize / 2));
            //        }
            //        // sphinx-doc: csharp_ref_inference_stop

            //        stopwatch.Stop();

            //        Console.WriteLine($"Audio duration: {waveInfo.TotalTime.ToString()}");
            //        Console.WriteLine($"Inference took: {stopwatch.Elapsed.ToString()}");
            //        Console.WriteLine((extended ? $"Extended result: " : "Recognized text: ") + speechResult);
            //    }
            //    waveBuffer.Clear();
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //}
            //return ValueTask.CompletedTask;

            throw new NotImplementedException();
        }

        public ValueTask StopProcessing()
        {
            throw new NotImplementedException();
        }


        #region private methods

        //static string MetadataToString(CandidateTranscript transcript)
        //{
        //    var nl = Environment.NewLine;
        //    string retval =
        //        Environment.NewLine + $"Recognized text: {string.Join("", transcript?.Tokens?.Select(x => x.Text))} {nl}"
        //                            + $"Confidence: {transcript?.Confidence} {nl}"
        //                            + $"Item count: {transcript?.Tokens?.Length} {nl}"
        //                            + string.Join(nl, transcript?.Tokens?.Select(x => $"Timestep : {x.Timestep} TimeOffset: {x.StartTime} Char: {x.Text}"));
        //    return retval;
        //}

        #endregion

        #region disposing
        protected virtual void Dispose(bool disposing)
        {
            if (_disposedValue) return;

            if (disposing)
            {
                // TODO: dispose managed state (managed objects)
            }

            _disposedValue = true;
        }


        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion

    }
}
