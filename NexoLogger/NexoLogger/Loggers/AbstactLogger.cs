using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using NexoLogger.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace NexoLogger.Loggers
{
    public abstract class AbstractLogger<T> : INexoLogger where T : AbstractLogger<T>
    {

        private readonly string _name;

        private ILoggerConfiguration<T> _config;

        public virtual ILoggerConfiguration<T> GetConfig => _config;

        public void SetConfig(ILoggerConfiguration<T> config) {
            _config = config;
        }

        public AbstractLogger() {
            throw new ArgumentNullException();
        }

        public AbstractLogger(string name, ILoggerConfiguration<T> config)
        {
            _config = config ?? throw new ArgumentNullException("Missing configuration data!");
            _name = name;
        }

        public string Name => _name;

        public IDisposable? BeginScope<TState>(TState state) where TState : notnull => default!;

        public ILogEntry GetAsLogEntry<T>(LogLevels level, T state) {
            return new DefaultLogEntry(level, state.ToString()); 
        }

        public virtual bool IsEnabled(LogLevel logLevel) => (LogLevels)logLevel <= _config.MinLogLevel && _config.Filter(_config);

        public abstract void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter);

        public abstract Task<bool> WriteLogAsync(ILogEntry entry);

    }
}
