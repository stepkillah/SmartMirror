using System.Threading.Tasks;
using SmartMirror.Core.Interfaces;

namespace SmartMirror.Core.Services.SpeechRecognition
{
    internal class NullSpeechRecognitionService : ISpeechRecognitionService
    {
        public void StartProcessing()
        {

        }

        public Task StopProcessing()
        {
            return Task.CompletedTask;
        }

        public void Dispose()
        {
        }
    }
}
