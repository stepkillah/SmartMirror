using System.Device.Gpio;
using Microsoft.Extensions.Logging;
using SmartMirror.Core.Interfaces;

namespace SmartMirror.Core.Services
{
    public class DisplayManager : IDisplayManager
    {
        private readonly GpioController _gpioController;
        private readonly ILogger _logger;
        private bool _isRunning;

        public DisplayManager(GpioController gpioController, ILogger<DisplayManager> logger)
        {
            _gpioController = gpioController;
            _logger = logger;
        }

        public void TurnOff()
        {
            //TODO implement
            _logger.LogInformation("Disabling display");
            _isRunning = false;
        }

        public void TurnOn()
        {
            //TODO implement
            _logger.LogInformation("Enabling display");
            _isRunning = true;
        }

        public bool IsRunning => _isRunning;
    }
}
