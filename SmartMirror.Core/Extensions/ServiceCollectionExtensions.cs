using System;
using System.Device.Gpio;
using System.Runtime.InteropServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SmartMirror.Core.Interfaces;
using SmartMirror.Core.Models;
using SmartMirror.Core.Services;
using SmartMirror.Core.Services.ExternalProcesses;
using SmartMirror.Core.Services.LedControl;
using SmartMirror.Core.Services.SpeechRecognition;
using SmartMirror.Core.Services.SpeechRecognition.Microsoft;

namespace SmartMirror.Core.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSmartMirrorServices(this IServiceCollection services)
        {
            services.AddSingleton(InitAudioPlayer);
            services.AddSingleton(InitSpeechRecognitionService);
            services.AddSingleton(InitLedManager);
            services.AddSingleton<IDisplayManager, DisplayManager>();
            services.AddSingleton(InitGpioController);
            services.AddHostedService<GpioButtonListener>();
            services.AddHostedService<MagicMirrorRunner>();
            services.AddHostedService<SpeechRecognitionHostedService>();
            services.AddHostedService<KeyboardListener>();
            services.AddScoped<ICommandsHandler, CommandsHandler>();
            return services;
        }

        public static IServiceCollection ConfigureSmartMirrorOptions(this IServiceCollection services, HostBuilderContext context)
        {
            services.Configure<MagicMirrorOptions>(context.Configuration.GetSection("MagicMirrorRunner"));
            services.Configure<LedOptions>(context.Configuration.GetSection("LED"));
            services.Configure<SpeechRecognitionOptions>(context.Configuration.GetSection("SpeechRecognition"));
            services.Configure<GpioOptions>(context.Configuration.GetSection("ButtonsControl"));

            return services;
        }

        private static ISpeechRecognitionService InitSpeechRecognitionService(IServiceProvider arg)
        {
            try
            {
                var audioService = new SpeechRecognitionService(
                    arg.GetService<ILogger<SpeechRecognitionService>>(),
                    arg.GetService<ICommandsHandler>(),
                    arg.GetService<IAudioPlayer>(),
                    arg.GetService<IOptions<SpeechRecognitionOptions>>());
                return audioService;
            }
            catch (Exception e)
            {
                Program.ProgramLogger.LogError(e, nameof(InitSpeechRecognitionService));
                return new NullSpeechRecognitionService();
            }
        }


        //private static IAudioService InitDeepSpeechAudioService(IServiceProvider arg)
        //{
        //    try
        //    {
        //        var audioService = new DeepSpeechAudioManager(arg.GetService<ILogger<DeepSpeechAudioManager>>());
        //        return audioService;
        //    }
        //    catch (Exception e)
        //    {
        //        ProgramLogger.LogError(e, nameof(InitAudioService));
        //        return new NullAudioService();
        //    }
        //}

        private static ILedManager InitLedManager(IServiceProvider arg)
        {
            try
            {
                return new LedManager(
                    arg.GetService<ILogger<LedManager>>(),
                    arg.GetService<IOptions<LedOptions>>(),
                    arg.GetService<IHostApplicationLifetime>());
            }
            catch (Exception e)
            {
                Program.ProgramLogger.LogError(e, nameof(InitLedManager));
                return new NullLedManager();
            }
        }

        private static IAudioPlayer InitAudioPlayer(IServiceProvider arg) =>
            RuntimeInformation.IsOSPlatform(OSPlatform.Linux)
                ? new APlayRunner(arg.GetService<ILogger<APlayRunner>>())
                : new NAudioPlayerService(arg.GetService<ILogger<NAudioPlayerService>>());

        private static GpioController InitGpioController(IServiceProvider arg)
        {
            var logger = arg.GetRequiredService<ILogger<GpioController>>();
            try
            {
                return new GpioController();
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error initializing GpioController");
                return null;
            }
        }
    }
}
