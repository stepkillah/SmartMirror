using System.Drawing;

namespace SmartMirror.Core.Services.LedControl
{
    internal class NullLedManager : ILedManager
    {

        public void TurnOff()
        {
        }

        public void TurnOn(Color color = default)
        {
        }
    }
}
