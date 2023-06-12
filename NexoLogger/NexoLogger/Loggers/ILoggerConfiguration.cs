using NexoLogger.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NexoLogger.Loggers
{
    public interface ILoggerConfiguration<T> where T : AbstractLogger<T>
    {
        LogLevels MinLogLevel { get; }

        Func<ILoggerConfiguration<T>, bool> Filter { get; set; } 
    }
}
