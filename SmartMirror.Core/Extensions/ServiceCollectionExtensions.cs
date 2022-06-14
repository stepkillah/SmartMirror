***REMOVED***
using System.Runtime.InteropServices;
using Microsoft.Extensions.DependencyInjection;
***REMOVED***
***REMOVED***
using SmartMirror.Core.Services;
using SmartMirror.Core.Services.ExternalProcesses;
using SmartMirror.Core.Services.LedControl;
using SmartMirror.Core.Services.SpeechRecognition;
using SmartMirror.Core.Services.SpeechRecognition.Microsoft;

namespace SmartMirror.Core.Extensions
***REMOVED***
    public static class ServiceCollectionExtensions
    ***REMOVED***
        public static IServiceCollection AddSmartMirrorServices(this IServiceCollection services)
        ***REMOVED***
            services.AddSingleton(InitAudioPlayer);
            services.AddSingleton(InitSpeechRecognitionService);
            services.AddSingleton(InitLedManager);
            services.AddSingleton<IMagicMirrorRunner, MagicMirrorRunner>();
            services.AddSingleton<IKeyboardListener, KeyboardListener>();
            services.AddScoped<ICommandsHandler, CommandsHandler>();
            return services;
      ***REMOVED***


        private static ISpeechRecognitionService InitSpeechRecognitionService(IServiceProvider arg)
        ***REMOVED***
***REMOVED***
            ***REMOVED***
                var audioService = new SpeechRecognitionService(
                    arg.GetService<ILogger<SpeechRecognitionService>>(), 
                    arg.GetService<ICommandsHandler>(), 
                    arg.GetService<IAudioPlayer>());
                return audioService;
          ***REMOVED***
            catch (Exception e)
            ***REMOVED***
                Program.ProgramLogger.LogError(e, nameof(InitSpeechRecognitionService));
                return new NullSpeechRecognitionService();
          ***REMOVED***
      ***REMOVED***


        //private static IAudioService InitDeepSpeechAudioService(IServiceProvider arg)
        //***REMOVED***
        //    try
        //    ***REMOVED***
        //        var audioService = new DeepSpeechAudioManager(arg.GetService<ILogger<DeepSpeechAudioManager>>());
        //        return audioService;
        //  ***REMOVED***
        //    catch (Exception e)
        //    ***REMOVED***
        //        ProgramLogger.LogError(e, nameof(InitAudioService));
        //        return new NullAudioService();
        //  ***REMOVED***
        //***REMOVED***

        private static ILedManager InitLedManager(IServiceProvider arg)
        ***REMOVED***
***REMOVED***
            ***REMOVED***
                return new LedManager(arg.GetService<ILogger<LedManager>>());
          ***REMOVED***
            catch (Exception e)
            ***REMOVED***
                Program.ProgramLogger.LogError(e, nameof(InitLedManager));
                return new NullLedManager();
          ***REMOVED***
      ***REMOVED***

        private static IAudioPlayer InitAudioPlayer(IServiceProvider arg) =>
            RuntimeInformation.IsOSPlatform(OSPlatform.Linux)
                ? new APlayRunner(arg.GetService<ILogger<APlayRunner>>())
                : new NAudioPlayerService(arg.GetService<ILogger<NAudioPlayerService>>());
  ***REMOVED***
***REMOVED***
