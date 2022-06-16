using Microsoft.Extensions.Logging;

namespace SmartMirror.Core.Extensions
{
    public static class LoggingExtensions
    {

        public static ILoggingBuilder AddSmartMirrorLogging(this ILoggingBuilder services)
        {
            services.AddConsole();
            return services;
        }

    }
}
