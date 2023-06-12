using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using NexoLogger.Loggers.ConsoleLogger;
using NexoLogger.Loggers.FileLogger;
using NexoLogger.Loggers.StreamLogger;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace NexoLogger.Loggers
{
    public class NexoLoggerProvider<T> : INexoLoggerProvider, ILoggerProvider where T : AbstractLogger<T>
    {

        private readonly ConcurrentDictionary<string, INexoLogger> _loggers = new ConcurrentDictionary<string, INexoLogger>();

        private ILoggerConfiguration<T> _defConfiguration;

        public bool TryGetLogger<K>(out INexoLogger logger) {  
            return _loggers.TryGetValue(typeof(K).FullName, out logger);   
        }
            
        public NexoLoggerProvider (ILoggerConfiguration<T> configuration) {
            _defConfiguration = configuration;
        }

        public ILogger CreateLogger(string categoryName) => _loggers.GetOrAdd(categoryName, (name)=> NexoLoggerFactory<T>.Create(name, _defConfiguration));

        public void Dispose()
        {
            
            _loggers.Clear();
        }
    }
}
