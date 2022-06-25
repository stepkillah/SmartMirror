using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SmartMirror.Core.Interfaces;
using SmartMirror.Core.Models;

namespace SmartMirror.Core.Services.SpeechRecognition.Microsoft
{
    public sealed class SpeechRecognitionService : ISpeechRecognitionService, IAsyncDisposable
    {
        private readonly ILogger _logger;
        private readonly ICommandsHandler _commandsHandler;
        private readonly IAudioPlayer _audioPlayer;

        private bool _isDisposed;
        private bool _isRunning;

        private KeywordRecognizer _keywordRecognizer;
        private AudioConfig _audioConfig;
        private KeywordRecognitionModel _keywordModel;
        private SpeechRecognizer _speechRecognizer;


        public SpeechRecognitionService(
            ILogger<SpeechRecognitionService> logger,
            ICommandsHandler commandsHandler,
            IAudioPlayer audioPlayer,
            IOptions<SpeechRecognitionOptions> recognitionOptions)
        {
            _logger = logger;
            _commandsHandler = commandsHandler;
            _audioPlayer = audioPlayer;
            _audioConfig = AudioConfig.FromDefaultMicrophoneInput();
            _keywordRecognizer = new KeywordRecognizer(_audioConfig);

            var keywordRecognitionTablePath = Path.Combine(Directory.GetCurrentDirectory(), recognitionOptions.Value.ActivationRecognitionTablePath);
            _keywordModel = KeywordRecognitionModel.FromFile(keywordRecognitionTablePath);


            var config = SpeechConfig.FromSubscription(recognitionOptions.Value.SubscriptionKey, recognitionOptions.Value.Region);
            _speechRecognizer = new SpeechRecognizer(config, _audioConfig);
        }

        public ValueTask StartProcessing()
        {
            _isRunning = true;
            _ = WaitForVoiceActivation().ConfigureAwait(false);
            _logger.LogInformation($"{nameof(SpeechRecognitionService)} started.");
            return ValueTask.CompletedTask;
        }

        public async ValueTask StopProcessing()
        {
            try
            {
                _logger.LogInformation($"{nameof(SpeechRecognitionService)}: Stopping");
                if (_isRunning && _keywordRecognizer != null)
                    await _keywordRecognizer.StopRecognitionAsync();

                _isRunning = false;
                _logger.LogInformation($"{nameof(SpeechRecognitionService)}: Stopped");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(SpeechRecognitionService)}: Stopping error");
            }
        }


        #region private methods
        private async Task RecognizeCommandAsync()
        {
            try
            {
                if (!_isRunning)
                    return;
                var result = await _speechRecognizer.RecognizeOnceAsync();

                switch (result.Reason)
                {
                    case ResultReason.RecognizedSpeech:
                        _logger.LogInformation($"We recognized: {result.Text}");
                        await RecognizeCommand(result.Text);
                        break;
                    case ResultReason.NoMatch:
                        _logger.LogInformation($"NOMATCH: Speech could not be recognized.");
                        break;
                    case ResultReason.Canceled:
                        HandleCancel(result);
                        break;
                }
            }
            finally
            {
                _ = WaitForVoiceActivation().ConfigureAwait(false);
            }
        }



        private async Task WaitForVoiceActivation()
        {
            if (!_isRunning)
                return;
            var result = await _keywordRecognizer.RecognizeOnceAsync(_keywordModel);
            switch (result.Reason)
            {
                case ResultReason.RecognizedKeyword:
                    _logger.LogInformation($"We recognized keyword: {result.Text}");
                    await Task.WhenAll(_audioPlayer.Play(Constants.SuccessSoundPath), RecognizeCommandAsync());
                    break;
                case ResultReason.NoMatch:
                    _logger.LogInformation($"NOMATCH: Speech could not be recognized.");
                    _ = WaitForVoiceActivation().ConfigureAwait(false);
                    break;
                case ResultReason.Canceled:
                    {
                        HandleCancel(result);
                        break;
                    }
            }
        }

        private async ValueTask RecognizeCommand(string rawText)
        {
            var result = await _commandsHandler.RecognizeCommand(rawText);
            if (!result.Success)
            {
                await _audioPlayer.Play(Constants.ErrorSoundPath);
                return;
            }

            await _commandsHandler.HandleCommand(result.Command, result.CommandData);
        }


        private void HandleCancel(RecognitionResult result)
        {
            var cancellation = CancellationDetails.FromResult(result);
            _logger.LogInformation($"CANCELED: Reason={cancellation.Reason}");
            if (cancellation.Reason != CancellationReason.Error) return;
            _logger.LogInformation($"CANCELED: ErrorCode={cancellation.ErrorCode}");
            _logger.LogInformation($"CANCELED: ErrorDetails={cancellation.ErrorDetails}");
            _logger.LogInformation($"CANCELED: Did you update the subscription info?");

        }

        #endregion

        #region dispose

        private void Dispose(bool disposing)
        {
            if (_isDisposed) return;
            if (disposing)
            {
                _audioConfig?.Dispose();
                _audioConfig = null;
                _keywordModel?.Dispose();
                _keywordModel = null;
                _speechRecognizer?.Dispose();
                _speechRecognizer = null;
                SafeDisposeKeywordRecognizer().GetAwaiter().GetResult();
            }

            _isDisposed = true;

            _logger.LogInformation($"{nameof(SpeechRecognitionService)} disposed.");
        }

        private async ValueTask DisposeAsync(bool disposing)
        {
            if (_isDisposed) return;
            if (disposing)
            {
                _audioConfig?.Dispose();
                _audioConfig = null;
                _keywordModel?.Dispose();
                _keywordModel = null;
                _speechRecognizer?.Dispose();
                _speechRecognizer = null;
                await SafeDisposeKeywordRecognizer();
            }

            _isDisposed = true;

            _logger.LogInformation($"{nameof(SpeechRecognitionService)} disposed.");
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public async ValueTask DisposeAsync()
        {
            await DisposeAsync(disposing: true);
            GC.SuppressFinalize(this);

        }

        private int _maxTriesCount = 3;
        private int _triesCount = 0;
        private async Task SafeDisposeKeywordRecognizer()
        {
            try
            {
                if (_keywordRecognizer != null)
                {
                    await StopProcessing();
                    _logger.LogInformation("Waiting 1 second while keyword recognizer stops");
                    await Task.Delay(1000);
                    _keywordRecognizer?.Dispose();
                    _logger.LogInformation("Keyword recognizer disposed");
                    _keywordRecognizer = null;
                }
            }
            catch (InvalidOperationException e)
            {
                if (_triesCount >= _maxTriesCount)
                {
                    _logger.LogError(e, $"Keyword recognizer disposing failed {_maxTriesCount} of {_maxTriesCount}");
                    return;
                }
                _triesCount++;
                _logger.LogWarning(e, $"Keyword recognizer disposing failed. Trying {_triesCount} of {_maxTriesCount} tries");
                await SafeDisposeKeywordRecognizer();
            }
        }
        #endregion

    }
}
