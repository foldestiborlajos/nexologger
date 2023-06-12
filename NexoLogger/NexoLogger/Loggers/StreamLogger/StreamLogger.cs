using Microsoft.Extensions.Logging;
using NexoLogger.Loggers.Builder;
using NexoLogger.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NexoLogger.Loggers.StreamLogger
{
    public class StreamLogger : AbstractLogger<StreamLogger>
    {
        public StreamLogger() { }

        public StreamLogger(string name, ILoggerConfiguration<StreamLogger> _conf) : base(name, _conf) { }   

        public override StreamLoggerConfig GetConfig => (StreamLoggerConfig)base.GetConfig;

        public override bool IsEnabled(LogLevel logLevel) {
            return logLevel <= (LogLevel)GetConfig.MinLogLevel;
        }

        private void _log(ILogEntry entry) {
            using (var writer = new StreamWriter(GetConfig.Target, GetConfig.Encoding, 1024, true))
            {
                writer.WriteLine(entry);
            }
        }

        public override void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            if (state is ILogEntry entry && GetConfig.Target is not null && GetConfig.Target.CanWrite)
            {

                try
                {
                    _log(entry);

                }
                catch (IOException ex)
                {

                    throw ex;

                }
            }
            else if (LoggerBuilder.GetConfig.ListenToHostEvents) {

                try
                {
                    _log(GetAsLogEntry((LogLevels)logLevel, state));
                }
                catch (IOException ex)
                {
                    Debug.WriteLine(ex);
                }

            }
        }

        public async override Task<bool> WriteLogAsync(ILogEntry entry) {
            if (GetConfig.Target is not null && GetConfig.Target.CanWrite)
            {
                    try
                    {
                        using (var writer = new StreamWriter(GetConfig.Target, GetConfig.Encoding, 1024, true))
                        {
                            await writer.WriteAsync(entry.Formatter(entry, null));
                            return true;
                        }

                    }
                    catch (IOException ex)
                    {

                        throw ex;

                    }
                
            }
            return false;
        }

    }
}
