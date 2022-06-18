using System.Threading.Tasks;

namespace SmartMirror.Core.Interfaces
{
    public interface ISpeechRecognitionService
    {
        ValueTask StartProcessing();
        ValueTask StopProcessing();
    }
}
