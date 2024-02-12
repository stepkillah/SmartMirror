using System;
using System.Device.Spi;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Iot.Device.Graphics;
using Iot.Device.Ws28xx;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SmartMirror.Core.Interfaces;
using SmartMirror.Core.Models;

namespace SmartMirror.Core.Services.LedControl
{
    public class LedManager : ILedManager, IDisposable, IAsyncDisposable
    {
        private bool _isDisposed;

        private readonly ILogger _logger;
        private readonly LedOptions _ledOptions;

        private Ws28xx _led;
        private SpiDevice _spiDevice;
        private bool _isRunning = true;
        private Color _currentColor;
        private readonly IStorageService _storageService;

        public LedManager(ILogger<LedManager> logger, IOptions<LedOptions> ledOptions, IHostApplicationLifetime hostApplicationLifetime, IStorageService storageService)
        {
            _logger = logger;
            _storageService = storageService;
            _ledOptions = ledOptions.Value;

            hostApplicationLifetime.ApplicationStarted.Register(OnStarted);
            hostApplicationLifetime.ApplicationStopping.Register(OnStopping);

            InitSpi();
        }

        public bool IsRunning => _isRunning;

        public async Task Toggle()
        {
            if (_led == null)
                return;
            if (_isRunning)
            {
                await TurnOff();
            }
            else
            {
                await TurnOn();
            }
        }

        public Task TurnOff()
        {
            if (_led == null || !_isRunning)
                return Task.CompletedTask;
            ColorWipe(_led, Color.Black, _ledOptions.Count);
            _isRunning = false;
            _logger.LogInformation("LED Turned OFF");
            return Task.CompletedTask;
        }

        public async Task TurnOn() => await TurnOn(default);

        public async Task TurnOn(Color color)
        {
            if (_led == null)
                return;
            if (color != default)
            {
                _currentColor = color;
                await _storageService.SetLedColor(_currentColor);
            }

            ColorWipe(_led, _currentColor == default ? Color.WhiteSmoke : _currentColor, _ledOptions.Count);
            _isRunning = true;
            _logger.LogInformation("LED Turned ON");
        }


        #region application lifecycle

        private async void OnStarted()
        {
            _logger.LogInformation("On started callback");
            var color = await _storageService.GetLedColor();
            await TurnOn(color);
        }

        private void OnStopping()
        {
            _logger.LogInformation("On stopping callback");
            TurnOff();
        }

        #endregion


        #region private methods

        private void InitSpi()
        {
            var settings = new SpiConnectionSettings(_ledOptions.BusId, _ledOptions.ChipSelectLine)
            {
                ClockFrequency = 2_400_000,
                Mode = SpiMode.Mode0,
                DataBitLength = 8
            };
            _spiDevice = SpiDevice.Create(settings);

            _led = new Ws2812b(_spiDevice, _ledOptions.Count);
        }


        private void ColorWipe(Ws28xx neo, Color color, int count)
        {
            var img = neo.Image;
            for (var i = 0; i < count; i++)
            {
                if (_ledOptions.Missing.Contains(i))
                {
                    img.SetPixel(i, 0, Color.Black);
                    neo.Update();
                    continue;
                }
                img.SetPixel(i, 0, color);
                neo.Update();
            }
        }

        private void TheatreChase(Ws28xx neo, Color color, int count, int iterations = 10)
        {
            var img = neo.Image;
            for (var i = 0; i < iterations; i++)
            {
                for (var j = 0; j < 3; j++)
                {
                    for (var k = 0; k < count; k += 3)
                    {
                        img.SetPixel(j + k, 0, color);
                    }

                    neo.Update();
                    System.Threading.Thread.Sleep(100);
                    for (var k = 0; k < count; k += 3)
                    {
                        img.SetPixel(j + k, 0, Color.Black);
                    }
                }
            }
        }

        private Color Wheel(int position)
        {
            if (position < 85)
                return Color.FromArgb(position * 3, 255 - position * 3, 0);


            if (position < 170)
            {
                position -= 85;
                return Color.FromArgb(255 - position * 3, 0, position * 3);
            }

            position -= 170;
            return Color.FromArgb(0, position * 3, 255 - position * 3);
        }

        private void Rainbow(Ws28xx neo, int count, int iterations = 1)
        {
            var img = neo.Image;
            for (var i = 0; i < 255 * iterations; i++)
            {
                for (var j = 0; j < count; j++)
                {
                    img.SetPixel(j, 0, Wheel((i + j) & 255));
                }

                neo.Update();
            }
        }

        private void RainbowCycle(Ws28xx neo, int count, int iterations = 1)
        {
            var img = neo.Image;
            for (var i = 0; i < 255 * iterations; i++)
            {
                for (var j = 0; j < count; j++)
                {
                    img.SetPixel(j, 0, Wheel((j * 255 / count + i) & 255));
                }

                neo.Update();
            }
        }

        private void TheaterChaseRainbow(Ws28xx neo, int count)
        {
            var img = neo.Image;
            for (var i = 0; i < 255; i++)
            {
                for (var j = 0; j < 3; j++)
                {
                    for (var k = 0; k < count; k += 3)
                    {
                        img.SetPixel(k + j, 0, Wheel((k + i) % 255));
                    }

                    neo.Update();
                    System.Threading.Thread.Sleep(100);

                    for (var k = 0; k < count; k += 3)
                    {
                        img.SetPixel(k + j, 0, Color.Black);
                    }
                }
            }
        }

        #endregion

        #region disposing

        protected virtual void Dispose(bool disposing)
        {
            if (_isDisposed) return;

            if (disposing)
            {
                if (_isRunning)
                    TurnOff();

                _led = null;
                _spiDevice.Dispose();
                _spiDevice = null;
            }
            _isDisposed = true;

            _logger.LogInformation($"{nameof(LedManager)} disposed.");
        }

        protected virtual ValueTask DisposeAsync(bool disposing)
        {
            if (_isDisposed) return ValueTask.CompletedTask;

            if (disposing)
            {
                if (_isRunning)
                    TurnOff();

                _led = null;
                _spiDevice.Dispose();
                _spiDevice = null;
            }
            _isDisposed = true;

            _logger.LogInformation($"{nameof(LedManager)} disposed.");
            return ValueTask.CompletedTask;
        }


        public async ValueTask DisposeAsync()
        {
            // Do not change this code. Put cleanup code in 'DisposeAsync(bool disposing)' method
            await DisposeAsync(disposing: true);
            GC.SuppressFinalize(this);
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion

    }
}
