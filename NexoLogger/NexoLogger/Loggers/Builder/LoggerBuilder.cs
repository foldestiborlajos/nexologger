using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;
using NexoLogger.Loggers;
using NexoLogger.Loggers.ConsoleLogger;
using NexoLogger.Loggers.FileLogger;
using NexoLogger.Loggers.StreamLogger;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;

namespace NexoLogger.Loggers.Builder
{
    public class LoggerBuilder
    {
        private readonly ILoggingBuilder _builder;

        private static INexoConfig _config;

        public static INexoConfig GetConfig => _config;

        private static readonly ConcurrentDictionary<string, ILoggerProvider> _providers = new ConcurrentDictionary<string, ILoggerProvider>();

        internal static bool TryGetProvider<K>( out ILoggerProvider provider)
        {
            return _providers.TryGetValue(typeof(K).Name, out provider);
        }

        internal static IEnumerable<ILoggerProvider> GetProviders => _providers.Values;


        public LoggerBuilder(ILoggingBuilder builder)
        {
            _builder = builder;
            _config = new NexoConfig();
        }

        private void _addLogging<T>(ILoggerConfiguration<T> conf) where T : AbstractLogger<T>
        {
            _builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider, NexoLoggerProvider<T>>((prov) =>
            {
                var provider = new NexoLoggerProvider<T>(conf);
                _providers.TryAdd(typeof(T).Name, provider);
                return provider;
            }));
        }

        public LoggerBuilder Add<T>(ILoggerConfiguration<T> conf) where T : AbstractLogger<T>
        {
            _addLogging(conf);
            return this;
        }

        public LoggerBuilder Configure(INexoConfig config)
        {
            _config = config;
            return this;
        }

    }
}
