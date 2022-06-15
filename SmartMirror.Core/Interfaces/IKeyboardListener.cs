using System.Threading;

namespace SmartMirror.Core.Interfaces
{
    public interface IKeyboardListener
    {
        void StartListenKeyCommands(CancellationToken cancellationToken);
    }
}
