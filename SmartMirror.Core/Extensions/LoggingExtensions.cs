using Microsoft.Extensions.Logging;

namespace SmartMirror.Core.Extensions
{
    public static class LoggingExtensions
    {

        public static ILoggingBuilder AddSmartMirrorLogging(this ILoggingBuilder services)
        {
            services.AddSimpleConsole(options => options.TimestampFormat = "[HH:mm:ss] ");
            services.AddFile("logs/smartMirror-{Date}.txt", retainedFileCountLimit: 3, minimumLevel: LogLevel.Warning);
            return services;
        }

    }
}
