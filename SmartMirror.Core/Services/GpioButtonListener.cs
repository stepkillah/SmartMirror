using System;
using System.Collections.Generic;
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
using Button = Iot.Device.Button.GpioButton;

namespace SmartMirror.Core.Services
{
    public class GpioButtonListener : IHostedService, IDisposable
    {
        private readonly GpioController _gpioController;
        private readonly ILogger _logger;
        private readonly GpioOptions _gpioOptions;
        private readonly ICommandsHandler _commandsHandler;
        private readonly ILedManager _ledManager;

        private readonly Dictionary<Button, GpioButton> _buttons = new Dictionary<Button, GpioButton>();
        private readonly Dictionary<int, Button> _pinMapping = new Dictionary<int, Button>();

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
            StartListenGpio(_gpioOptions.Display.TriggerGpio, GpioButton.Display);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            StopListenGpio(_gpioOptions.LedGPIO, GpioButton.LED);
            StopListenGpio(_gpioOptions.Display.TriggerGpio, GpioButton.Display);
            return Task.CompletedTask;
        }

        private void StartListenGpio(int pinNumber, GpioButton button)
        {
            try
            {
                _logger.LogInformation($"Starting listening for {button} button on pin {pinNumber}");


                Button gpioButton = new Button(pinNumber, _gpioController,
                    false, PinMode.InputPullUp, TimeSpan.FromMilliseconds(50))
                {
                    IsDoublePressEnabled = false,
                    IsHoldingEnabled = false
                };

                gpioButton.Press += GpioButtonOnPress;
                _buttons.Add(gpioButton, button);
                _pinMapping.Add(pinNumber, gpioButton);

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
                if (_pinMapping.ContainsKey(pinNumber))
                {
                    var gpioButton = _pinMapping[pinNumber];
                    _pinMapping.Remove(pinNumber);
                    if (_buttons.ContainsKey(gpioButton))
                        _buttons.Remove(gpioButton);
                    gpioButton.Press -= GpioButtonOnPress;
                    gpioButton.Dispose();
                    _logger.LogInformation($"Stopped listening for {button} button on pin {pinNumber}");
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Stopping listening for {button} button failed on pin {pinNumber}");
            }
        }


        private async void GpioButtonOnPress(object sender, EventArgs e)
        {
            _logger.LogInformation("Button press detected");
            if (sender is Button button && _buttons.ContainsKey(button))
                await CommandExecuted(_buttons[button]);
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
                        await _commandsHandler.HandleCommand(SmartMirrorCommand.DisplayToggle, null);
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
            foreach (var gpioButton in _buttons)
                gpioButton.Key.Dispose();
            _buttons.Clear();
            _pinMapping.Clear();
        }
    }
}
