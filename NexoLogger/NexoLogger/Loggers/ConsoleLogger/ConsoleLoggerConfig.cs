using NexoLogger.Loggers.FileLogger;
using NexoLogger.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NexoLogger.Loggers.ConsoleLogger
{
    public class ConsoleLoggerConfig : ILoggerConfiguration<ConsoleLogger>
    {

        public LogLevels MinLogLevel { get; set; } = LogLevels.None;

        public Func<ILoggerConfiguration<ConsoleLogger>, bool> Filter { get; set; } = (ILoggerConfiguration<ConsoleLogger> config) => { return true; };
    }
}
