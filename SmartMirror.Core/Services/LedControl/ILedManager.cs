using System.Drawing;

namespace SmartMirror.Core.Services.LedControl
{
    public interface ILedManager
    {
        void TurnOff();
        void TurnOn(Color color = default);
    }
}
