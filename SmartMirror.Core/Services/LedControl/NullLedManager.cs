using System.Drawing;
using System.Threading.Tasks;

namespace SmartMirror.Core.Services.LedControl
{
    internal class NullLedManager : ILedManager
    {

        public Task TurnOff()
        {
            return Task.CompletedTask;
        }

        public Task TurnOn(Color color = default)
        {
            return Task.CompletedTask;
        }

        public Task Toggle()
        {
            return Task.CompletedTask;
        }

        public bool IsRunning { get; }
    }
}
