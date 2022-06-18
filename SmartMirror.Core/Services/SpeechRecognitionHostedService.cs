using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using SmartMirror.Core.Interfaces;

namespace SmartMirror.Core.Services
{
    public class SpeechRecognitionHostedService : IHostedService
    {
        private readonly ISpeechRecognitionService _speechRecognitionService;

        public SpeechRecognitionHostedService(ISpeechRecognitionService speechRecognitionService)
        {
            _speechRecognitionService = speechRecognitionService;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _speechRecognitionService.StartProcessing();
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _speechRecognitionService.StopProcessing();
        }
    }
}
