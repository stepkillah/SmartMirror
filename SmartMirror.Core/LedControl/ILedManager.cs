***REMOVED***
using System.Drawing;

namespace SmartMirror.Core.LedControl
***REMOVED***
    public interface ILedManager : IDisposable
    ***REMOVED***
        void StartProcessing();
        void TurnOff();
        void TurnOn(Color color = default);
  ***REMOVED***
***REMOVED***
