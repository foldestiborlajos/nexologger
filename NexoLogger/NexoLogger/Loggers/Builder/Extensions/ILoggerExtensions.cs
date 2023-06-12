using Microsoft.Extensions.Logging;
using NexoLogger.Loggers;
using NexoLogger.Loggers.Builder;
using NexoLogger.Models;


namespace NexoLogger.Loggers.Builder.Extensions
{
    public static class ILoggerExtensions
    {
        public static void WriteToLog(this ILogger logger, ILogEntry entry)
        {

            logger.Log((LogLevel)entry.Level, 0, entry, null, entry.Formatter);

        }

        public static void WriteToLog(this ILogger logger, LogLevels level, string message)
        {
            var entry = new DefaultLogEntry(level, message);
            logger.WriteToLog(entry);
        }

        public static void WriteToLogDebug(this ILogger logger, string Message)
        {
            logger.WriteToLog(LogLevels.Debug, Message);
        }

        public static void WriteToLogInfo(this ILogger logger, string Message)
        {
            logger.WriteToLog(LogLevels.Info, Message);
        }

        public static void WriteToLogError(this ILogger logger, string Message)
        {
            logger.WriteToLog(LogLevels.Error, Message);
        }

        public static async Task<bool> WriteLogAsync<K>(this ILogger logger, ILogEntry entry) 
        {
            LoggerBuilder.GetProviders
                .ToList()
                .ForEach(async (provider) =>
                {             
                    INexoLogger _logger = null;
                    if ((provider as INexoLoggerProvider).TryGetLogger<K>(out _logger) && _logger is not null && _logger.IsEnabled((LogLevel)entry.Level) ){
                        await _logger.WriteLogAsync(entry);
                    }
                });
            return true;  
        }

        public static async Task<bool> WriteLogAsync<K>(this ILogger logger, LogLevels level, string message) 
        {
            var entry = new DefaultLogEntry(level, message);
            return await logger.WriteLogAsync<K>(entry);
        }

        public static async Task<bool> WriteLogDebugAsync<K>(this ILogger logger, string Message) 
        {
            return await logger.WriteLogAsync<K>(LogLevels.Debug, Message);
        }

        public static async Task<bool> WriteLogInfoAsync<K>(this ILogger logger, string Message)
        {
            return await logger.WriteLogAsync<K>(LogLevels.Info, Message);
        }

        public static async Task<bool> WriteLogErrorAsync<K, T>(this ILogger logger, string Message) 
        {
        
            return await logger.WriteLogAsync<K>(LogLevels.Error, Message);
        }

        private static AbstractLogger<T> TryGetLogger<K, T>() where T:AbstractLogger<T> 
        {
            ILoggerProvider prov = null;
            INexoLogger _logger = null;
            if (LoggerBuilder.TryGetProvider<T>(out prov) && prov is NexoLoggerProvider<T> nexoprov && nexoprov.TryGetLogger<K>(out _logger) && _logger is T nexolog)
            {
                return nexolog;
            }
            return _logger as AbstractLogger<T>;
        }

        public static ILogger Configure<K, T>(this ILogger logger, Action<ILoggerConfiguration<T>> configAction) where T : AbstractLogger<T>
        {
            AbstractLogger<T> _logger = TryGetLogger<K, T>();
            if (_logger is not null ) {
                configAction(_logger.GetConfig);              
            }


            return logger;
        }
    }
}
