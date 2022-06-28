using System;
using System.Device.Gpio;
using Polly;

namespace SmartMirror.Core.Extensions
{
    public static class GpioExtensions
    {
        public static void OpenPinWithRetry(this GpioController gpioController, int pinNumber)
        {
            Policy
                .Handle<UnauthorizedAccessException>()
                .WaitAndRetry(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)))
                .Execute(() => gpioController.OpenPin(pinNumber));
        }
    }
}
