***REMOVED***
using System.Device.S***REMOVED***;
using System.Drawing;
using System.Linq;
using Iot.Device.Graphics;
using Iot.Device.Ws28xx;
***REMOVED***
***REMOVED***
***REMOVED***

namespace SmartMirror.Core.Services.LedControl
***REMOVED***
    public class LedManager : ILedManager
    ***REMOVED***
***REMOVED***

***REMOVED***
        private readonly LedOptions _ledOptions;

        private Ws28xx _led;
        private S***REMOVED***Device _s***REMOVED***Device;
        private bool _isRunning = true;

        public LedManager(ILogger<LedManager> logger, IOptions<LedOptions> ledOptions)
        ***REMOVED***
***REMOVED***
            _ledOptions = ledOptions.Value;
            var settings = new S***REMOVED***ConnectionSettings(_ledOptions.BusId, _ledOptions.ChipSelectLine)
            ***REMOVED***
                ClockFrequency = 2_400_000,
                Mode = S***REMOVED***Mode.Mode0,
                DataBitLength = 8
          ***REMOVED***;
            _s***REMOVED***Device = S***REMOVED***Device.Create(settings);

            _led = new Ws2812b(_s***REMOVED***Device, _ledOptions.Count);
      ***REMOVED***

***REMOVED***
        ***REMOVED***
            TurnOn();
      ***REMOVED***

        public void TurnOff()
        ***REMOVED***
            if (_led == null || !_isRunning)
***REMOVED***
            ColorWipe(_led, Color.Black, _ledOptions.Count);
            _isRunning = false;
            _logger.LogWarning("LED Turned OFF");
      ***REMOVED***

        public void TurnOn(Color color = default)
        ***REMOVED***
            if (_led == null)
***REMOVED***
            ColorWipe(_led, color == default ? Color.White : color, _ledOptions.Count);
***REMOVED***
            _logger.LogWarning("LED Turned ON");
      ***REMOVED***

        private void ColorWipe(Ws28xx neo, Color color, int count)
        ***REMOVED***
            BitmapImage img = neo.Image;
            for (var i = 0; i < count; i++)
            ***REMOVED***
                if (_ledOptions.Missing.Contains(i))
                ***REMOVED***
                    img.SetPixel(i, 0, Color.Black);
                    neo.Update();
                    continue;
              ***REMOVED***
                img.SetPixel(i, 0, color);
                neo.Update();
          ***REMOVED***
      ***REMOVED***

        private void TheatreChase(Ws28xx neo, Color color, int count, int iterations = 10)
        ***REMOVED***
            BitmapImage img = neo.Image;
            for (var i = 0; i < iterations; i++)
            ***REMOVED***
                for (var j = 0; j < 3; j++)
                ***REMOVED***
                    for (var k = 0; k < count; k += 3)
                    ***REMOVED***
                        img.SetPixel(j + k, 0, color);
                  ***REMOVED***

                    neo.Update();
                    System.Threading.Thread.Sleep(100);
                    for (var k = 0; k < count; k += 3)
                    ***REMOVED***
                        img.SetPixel(j + k, 0, Color.Black);
                  ***REMOVED***
              ***REMOVED***
          ***REMOVED***
      ***REMOVED***

        private Color Wheel(int position)
        ***REMOVED***
            if (position < 85)
                return Color.FromArgb(position * 3, 255 - position * 3, 0);


            if (position < 170)
            ***REMOVED***
                position -= 85;
                return Color.FromArgb(255 - position * 3, 0, position * 3);
          ***REMOVED***

            position -= 170;
            return Color.FromArgb(0, position * 3, 255 - position * 3);
      ***REMOVED***

        private void Rainbow(Ws28xx neo, int count, int iterations = 1)
        ***REMOVED***
            BitmapImage img = neo.Image;
            for (var i = 0; i < 255 * iterations; i++)
            ***REMOVED***
                for (var j = 0; j < count; j++)
                ***REMOVED***
                    img.SetPixel(j, 0, Wheel((i + j) & 255));
              ***REMOVED***

                neo.Update();
          ***REMOVED***
      ***REMOVED***

        private void RainbowCycle(Ws28xx neo, int count, int iterations = 1)
        ***REMOVED***
            BitmapImage img = neo.Image;
            for (var i = 0; i < 255 * iterations; i++)
            ***REMOVED***
                for (var j = 0; j < count; j++)
                ***REMOVED***
                    img.SetPixel(j, 0, Wheel(((int)(j * 255 / count) + i) & 255));
              ***REMOVED***

                neo.Update();
          ***REMOVED***
      ***REMOVED***

        private void TheaterChaseRainbow(Ws28xx neo, int count)
        ***REMOVED***
            BitmapImage img = neo.Image;
            for (var i = 0; i < 255; i++)
            ***REMOVED***
                for (var j = 0; j < 3; j++)
                ***REMOVED***
                    for (var k = 0; k < count; k += 3)
                    ***REMOVED***
                        img.SetPixel(k + j, 0, Wheel((k + i) % 255));
                  ***REMOVED***

                    neo.Update();
                    System.Threading.Thread.Sleep(100);

                    for (var k = 0; k < count; k += 3)
                    ***REMOVED***
                        img.SetPixel(k + j, 0, Color.Black);
                  ***REMOVED***
              ***REMOVED***
          ***REMOVED***
      ***REMOVED***

        private void SetLastPixel(Ws28xx device)
        ***REMOVED***
            BitmapImage image = device.Image;
            image.Clear(Color.Blue);
            image.SetPixel(15, 0, Color.Red);
            device.Update();

      ***REMOVED***

        #region disposing
        protected virtual void Dispose(bool disposing)
        ***REMOVED***
***REMOVED***

***REMOVED***
            ***REMOVED***
***REMOVED***
                    TurnOff();

                _led = null;
                _s***REMOVED***Device.Dispose();
                _s***REMOVED***Device = null;
          ***REMOVED***
***REMOVED***

            _logger.LogInformation($"***REMOVED***nameof(LedManager)***REMOVED*** disposed.");
      ***REMOVED***

***REMOVED***
        ***REMOVED***
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
***REMOVED***
***REMOVED***
      ***REMOVED***
***REMOVED***
  ***REMOVED***
***REMOVED***