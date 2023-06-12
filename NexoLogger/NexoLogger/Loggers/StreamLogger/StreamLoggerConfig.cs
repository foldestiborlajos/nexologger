using Microsoft.Extensions.Logging;
using NexoLogger.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace NexoLogger.Loggers.StreamLogger
{
    public class StreamLoggerConfig : ILoggerConfiguration<StreamLogger>
    {

        public StreamLoggerConfig() {
        }

        public Stream Target { get; set; }

        public Encoding Encoding { get; set; } = Encoding.Default; 

        public LogLevels MinLogLevel { get; set; } = LogLevels.Debug;

        public Func<ILoggerConfiguration<StreamLogger>, bool> Filter { get; set; } = (ILoggerConfiguration<StreamLogger> config) => { return true; };
    }
}
