using System.ComponentModel;

namespace SmartMirror.Core.Enums
{
    public enum SmartMirrorCommand : byte
    {
        Unknown = 0,
        [Description("light on")]
        LedOn = 1,
        [Description("light off")]
        LedOff = 2,
        [Description("color")]
        LedColorSet = 3,
        [Description("sound test")]
        PlayTestSound = 4,
        [Description("display on")]
        DisplayOn = 5,
        [Description("display off")]
        DisplayOff= 6
    }
}
