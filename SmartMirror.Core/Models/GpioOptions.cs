
namespace SmartMirror.Core.Models
{
    public class GpioOptions
    {
        public int LedGPIO { get; set; }

        public DisplayOptions Display { get; set; }

    }

    public class DisplayOptions
    {
        public int TriggerGpio { get; set; }
        public int ControlGpio { get; set; }
    }

}
