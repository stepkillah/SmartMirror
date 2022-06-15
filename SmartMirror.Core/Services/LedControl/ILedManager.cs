using System;
using System.Drawing;

namespace SmartMirror.Core.Services.LedControl
{
    public interface ILedManager : IDisposable
    {
        void StartProcessing();
        void TurnOff();
        void TurnOn(Color color = default);
    }
}
