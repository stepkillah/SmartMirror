using System.Threading.Tasks;

namespace SmartMirror.Core.Interfaces
{
    public interface IProcessService
    {
        ValueTask StartProcessing();
        ValueTask StopProcessing();
    }
}
