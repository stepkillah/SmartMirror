namespace SmartMirror.Core.Interfaces
{
    public interface IDisplayManager
    {
        void TurnOff();
        void TurnOn();
        bool IsRunning { get; }
    }
}
