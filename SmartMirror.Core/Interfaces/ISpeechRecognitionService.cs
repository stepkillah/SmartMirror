using System;
using System.Threading.Tasks;

namespace SmartMirror.Core.Interfaces
{
    public interface ISpeechRecognitionService : IDisposable
    {
        void StartProcessing();
        Task StopProcessing();
    }
}
