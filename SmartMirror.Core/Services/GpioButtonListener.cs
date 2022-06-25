using System;
using System.Device.Gpio;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SmartMirror.Core.Enums;
using SmartMirror.Core.Interfaces;
using SmartMirror.Core.Models;
using SmartMirror.Core.Services.LedControl;

namespace SmartMirror.Core.Services
{
    public class GpioButtonListener : IHostedService, IDisposable
    {
        private readonly GpioController _gpioController;
        private readonly ILogger _logger;
        private readonly GpioOptions _gpioOptions;
        private readonly ICommandsHandler _commandsHandler;
        private readonly ILedManager _ledManager;

        public GpioButtonListener(
            GpioController gpioController,
            ILogger<GpioButtonListener> logger,
            IOptions<GpioOptions> gpioOptions,
            ICommandsHandler commandsHandler,
            ILedManager ledManager)
        {
            _gpioController = gpioController;
            _logger = logger;
            _commandsHandler = commandsHandler;
            _ledManager = ledManager;
            _gpioOptions = gpioOptions.Value;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Starting listening for buttons");
                if (!_gpioController.IsPinOpen(_gpioOptions.LedControlGPIO))
                {
                    _gpioController.OpenPin(_gpioOptions.LedControlGPIO);
                    _gpioController.SetPinMode(_gpioOptions.LedControlGPIO,
                        _gpioController.IsPinModeSupported(_gpioOptions.LedControlGPIO, PinMode.InputPullUp)
                            ? PinMode.InputPullUp
                            : PinMode.Input);
                }


                _gpioController.RegisterCallbackForPinValueChangedEvent(_gpioOptions.LedControlGPIO, PinEventTypes.Rising, OnButtonReleased);
                _gpioController.RegisterCallbackForPinValueChangedEvent(_gpioOptions.LedControlGPIO, PinEventTypes.Falling, OnButtonPressed);

                _logger.LogInformation("Started listening for buttons");
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Starting listening for buttons failed");
            }
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Stopping listening for buttons");
                if (!_gpioController.IsPinOpen(_gpioOptions.LedControlGPIO)) return Task.CompletedTask;

                _gpioController.UnregisterCallbackForPinValueChangedEvent(_gpioOptions.LedControlGPIO, OnButtonReleased);
                _gpioController.UnregisterCallbackForPinValueChangedEvent(_gpioOptions.LedControlGPIO, OnButtonPressed);
                _gpioController.ClosePin(_gpioOptions.LedControlGPIO);
                _logger.LogInformation("Stopped listening for buttons");
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Stopping listening for buttons failed");
            }
            return Task.CompletedTask;
        }

        private void OnButtonPressed(object sender, PinValueChangedEventArgs pinvaluechangedeventargs)
        {
            _logger.LogInformation($"Button pressed: PIN: {pinvaluechangedeventargs.PinNumber}. Type: {pinvaluechangedeventargs.ChangeType}");
        }

        private async void OnButtonReleased(object sender, PinValueChangedEventArgs pinvaluechangedeventargs)
        {
            _logger.LogInformation($"Button released: PIN: {pinvaluechangedeventargs.PinNumber}. Type: {pinvaluechangedeventargs.ChangeType}");
            if (pinvaluechangedeventargs.PinNumber == _gpioOptions.LedControlGPIO)
                await CommandExecuted(GpioButton.LED);
        }

        private CancellationTokenSource _cts;
        private async Task CommandExecuted(GpioButton button)
        {
            try
            {

                if (_cts != null)
                {
                    _cts.Cancel();
                    _cts.Dispose();
                    _cts = null;
                }
                _cts = new CancellationTokenSource();

                await Task.Delay(50, _cts.Token);
                switch (button)
                {
                    case GpioButton.LED:
                        _logger.LogInformation("LED button command recognized");
                        await _commandsHandler.HandleCommand(
                            _ledManager.IsRunning ? SmartMirrorCommand.LedOff : SmartMirrorCommand.LedOn, null);
                        break;
                    case GpioButton.Display:
                        break;
                    default:
                        return;
                }
            }
            catch (TaskCanceledException)
            {

                _logger.LogInformation("GPIO callback dedupe");
            }
        }


        public void Dispose()
        {
            _cts?.Dispose();
        }
    }
}
