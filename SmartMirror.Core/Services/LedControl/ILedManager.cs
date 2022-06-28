using System.Drawing;
using System.Threading.Tasks;

namespace SmartMirror.Core.Services.LedControl
{
    public interface ILedManager
    {
        Task TurnOff();
        Task TurnOn(Color color = default);
        Task Toggle();
        bool IsRunning { get; }
    }
}
