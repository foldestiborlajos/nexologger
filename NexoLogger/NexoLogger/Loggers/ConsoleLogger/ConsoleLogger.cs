using Microsoft.Extensions.Logging;
using NexoLogger.Loggers.Builder;
using NexoLogger.Loggers.FileLogger;
using NexoLogger.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NexoLogger.Loggers.ConsoleLogger
{
    public class ConsoleLogger : AbstractLogger<ConsoleLogger>
    {
        public ConsoleLogger(string name, ILoggerConfiguration<ConsoleLogger> conf) : base(name, conf) { }


        private ConsoleColor _getConsoleColor(LogLevels level)
        {
            switch (level)
            {
                case LogLevels.Debug:
                    {
                        return ConsoleColor.Gray;
                    }
                case LogLevels.Info:
                    {
                        return ConsoleColor.Green;
                    }
                case LogLevels.Error:
                    {
                        return ConsoleColor.Red;
                    }
                default:
                    {
                        throw new NotImplementedException();
                    }
            }
        }

        private bool _valid(ILogEntry entry)
        {
            return entry.Message.Length <= 1000;
        }

        private void _log(ILogEntry e)
        {
            var originalColor = System.Console.ForegroundColor;
            System.Console.ForegroundColor = _getConsoleColor(e.Level);
            System.Console.WriteLine(e.Formatter(e, null));
            System.Console.ForegroundColor = originalColor;
        }

        private async Task<bool> _logAsync(ILogEntry e)
        {
            var originalColor = System.Console.ForegroundColor;
            System.Console.ForegroundColor = _getConsoleColor(e.Level);
            await System.Console.Out.WriteAsync(e.Formatter(e, null));
            System.Console.ForegroundColor = originalColor;

            return true;
        }


        public override void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            if (IsEnabled(logLevel)) {

                if (state is ILogEntry e)
                {
                    if (_valid(e))
                    {
                        _log(e);

                    }
                    else
                    {
                        throw new ArgumentException("A console message shouldn't be longer than 1000 characters");
                    }
                }
                else if (LoggerBuilder.GetConfig.ListenToHostEvents)
                {

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
        }

        public async override Task<bool> WriteLogAsync(ILogEntry entry)
        {
            if (IsEnabled((LogLevel)entry.Level))
            {
                return await _logAsync(entry);
            }
            else {
                return false;
            }
        }
    }
}
