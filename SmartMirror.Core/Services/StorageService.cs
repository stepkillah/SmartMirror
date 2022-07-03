using System.Drawing;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SmartMirror.Core.Interfaces;
using SmartMirror.Core.JsonConverters;
using SmartMirror.Core.Models;

namespace SmartMirror.Core.Services
{
    public class StorageService : IStorageService
    {
        private UserData _userData;
        private readonly ILogger _logger;
        private bool _isDataSaved;

        private readonly JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions()
        {
            Converters =
            {
                new HtmlColorConverter()
            }
        };

        public StorageService(ILogger<StorageService> logger)
        {
            _logger = logger;
        }


        public async ValueTask<Color> GetLedColor()
        {
            if (_userData == null)
                await Load();

            return _userData?.LedColor ?? default;
        }

        public async ValueTask SetLedColor(Color color)
        {
            if (_userData == null)
                await Load();

            _userData ??= new UserData();
            if (_userData.LedColor == color)
                return;
            _userData.LedColor = color;
            _isDataSaved = false;
            _ = Task.Run(Save);
        }

        private async Task Load()
        {
            try
            {
                _logger.LogInformation("Loading user data");
                await using var file = File.OpenRead("userdata.json");
                _userData = await JsonSerializer.DeserializeAsync<UserData>(file, _jsonSerializerOptions);
                _logger.LogInformation("User data loaded successfully");
                _isDataSaved = true;
            }
            catch (System.Exception e)
            {
                _logger.LogError(e, "User data load failed");
            }
        }

        private async Task Save()
        {
            if (_isDataSaved)
            {
                _logger.LogInformation("No user data to save");
                return;
            }

            try
            {
                _logger.LogInformation("Saving user data");
                if (_userData == null)
                {
                    _logger.LogInformation("No user data to save");
                    return;
                }

                await using var file = File.Open("userdata.json", FileMode.Truncate);
                await JsonSerializer.SerializeAsync(file, _userData, _jsonSerializerOptions);
                _logger.LogInformation("User data saved successfully");
                _isDataSaved = true;
            }
            catch (System.Exception e)
            {
                _logger.LogError(e, "User data save failed");
            }
        }
    }
}
