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
            StartListenGpio(_gpioOptions.LedGPIO, GpioButton.LED);
            StartListenGpio(_gpioOptions.DisplayGPIO, GpioButton.Display);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            StopListenGpio(_gpioOptions.LedGPIO, GpioButton.LED);
            StopListenGpio(_gpioOptions.DisplayGPIO, GpioButton.Display);
            return Task.CompletedTask;
        }

        private void StartListenGpio(int pinNumber, GpioButton button)
        {
            try
            {
                _logger.LogInformation($"Starting listening for {button} button on pin {pinNumber}");
                if (!_gpioController.IsPinOpen(pinNumber))
                {
                    _gpioController.OpenPin(pinNumber);
                    _gpioController.SetPinMode(pinNumber,
                        _gpioController.IsPinModeSupported(pinNumber, PinMode.InputPullUp)
                            ? PinMode.InputPullUp
                            : PinMode.Input);
                }

                if (!_gpioController.IsPinOpen(pinNumber)) return;

                _gpioController.RegisterCallbackForPinValueChangedEvent(pinNumber, PinEventTypes.Rising,
                    OnButtonReleased);
                _logger.LogInformation($"Started listening for {button} button on pin {pinNumber}");
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Starting listening for {button} button failed on pin {pinNumber}");
            }
        }

        private void StopListenGpio(int pinNumber, GpioButton button)
        {
            try
            {
                _logger.LogInformation($"Stopping listening for {button} button on pin {pinNumber}");

                if (!_gpioController.IsPinOpen(pinNumber)) return;

                _gpioController.UnregisterCallbackForPinValueChangedEvent(pinNumber, OnButtonReleased);
                _gpioController.ClosePin(pinNumber);
                _logger.LogInformation($"Stopped listening for {button} button on pin {pinNumber}");
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Stopping listening for {button} button failed on pin {pinNumber}");
            }
        }


        private async void OnButtonReleased(object sender, PinValueChangedEventArgs pinvaluechangedeventargs)
        {
            _logger.LogInformation($"Button released on pin: {pinvaluechangedeventargs.PinNumber} with type: {pinvaluechangedeventargs.ChangeType}");
            if (pinvaluechangedeventargs.PinNumber == _gpioOptions.LedGPIO)
            {
                await CommandExecuted(GpioButton.LED);
            }
            else if (pinvaluechangedeventargs.PinNumber == _gpioOptions.DisplayGPIO)
            {
                await CommandExecuted(GpioButton.Display);
            }
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
                        _logger.LogInformation($"{GpioButton.LED} GPIO command recognized");
                        await _commandsHandler.HandleCommand(
                            _ledManager.IsRunning ? SmartMirrorCommand.LedOff : SmartMirrorCommand.LedOn, null);
                        break;
                    case GpioButton.Display:
                        _logger.LogInformation($"{GpioButton.Display} GPIO command recognized");
                        break;
                    default:
                        return;
                }
            }
            catch (TaskCanceledException)
            {
                _logger.LogInformation($"GPIO callback dedupe for {button}");
            }
        }


        public void Dispose()
        {
            _cts?.Dispose();
        }
    }
}
