using System.Threading;
using System.Threading.Tasks;

namespace SmartMirror.Core.Interfaces
{
    public interface IAudioPlayer
    {
        Task Play(string path, CancellationToken cancellationToken = default);
    }
}
