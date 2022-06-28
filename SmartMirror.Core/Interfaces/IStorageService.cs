using System.Drawing;
using System.Threading.Tasks;

namespace SmartMirror.Core.Interfaces
{
    public interface IStorageService
    {
        ValueTask<Color> GetLedColor();
        ValueTask SetLedColor(Color color);
    }
}
