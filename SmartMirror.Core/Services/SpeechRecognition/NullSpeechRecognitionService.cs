using System.Threading.Tasks;
using SmartMirror.Core.Interfaces;

namespace SmartMirror.Core.Services.SpeechRecognition
{
    internal class NullSpeechRecognitionService : ISpeechRecognitionService
    {
        public ValueTask StartProcessing() => ValueTask.CompletedTask;

        public ValueTask StopProcessing() => ValueTask.CompletedTask;

    }
}
