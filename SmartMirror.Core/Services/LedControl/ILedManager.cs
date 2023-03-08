using System.Drawing;
using System.Threading.Tasks;

namespace SmartMirror.Core.Services.LedControl
{
    public interface ILedManager
    {
        Task TurnOff();
        Task TurnOn();
        Task TurnOn(Color color);
        Task Toggle();
        bool IsRunning { get; }
    }
}
