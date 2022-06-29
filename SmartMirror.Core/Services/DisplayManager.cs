using System.Device.Gpio;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SmartMirror.Core.Interfaces;
using SmartMirror.Core.Models;

namespace SmartMirror.Core.Services
{
    public class DisplayManager : IDisplayManager
    {
        private readonly GpioController _gpioController;
        private readonly ILogger _logger;
        private readonly GpioOptions _gpioOptions;

        public DisplayManager(GpioController gpioController, ILogger<DisplayManager> logger, IOptions<GpioOptions> gpioOptions)
        {
            _gpioController = gpioController;
            _logger = logger;
            _gpioOptions = gpioOptions.Value;
        }

        public async Task Toggle()
        {
            try
            {
                _logger.LogInformation("Toggling display");
                if (!_gpioController.IsPinOpen(_gpioOptions.Display.ControlGpio))
                    _gpioController.OpenPin(_gpioOptions.Display.ControlGpio, PinMode.Output);
                if (!_gpioController.IsPinOpen(_gpioOptions.Display.ControlGpio))
                {
                    _logger.LogInformation("Pin open failed");
                    return;
                }

                _gpioController.Write(_gpioOptions.Display.ControlGpio, PinValue.Low);
                await Task.Delay(300);
                _gpioController.Write(_gpioOptions.Display.ControlGpio, PinValue.High);
                _gpioController.ClosePin(_gpioOptions.Display.ControlGpio);
            }
            catch (System.Exception e)
            {
                _logger.LogError(e, "Display toggle failed");
            }
        }
    }
}
