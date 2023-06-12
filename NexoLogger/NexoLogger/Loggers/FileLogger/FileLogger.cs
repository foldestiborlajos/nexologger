using Microsoft.Extensions.Logging;
using NexoLogger.Loggers.Builder;
using NexoLogger.Models;
using System.Diagnostics;
using System.IO.Abstractions;

namespace NexoLogger.Loggers.FileLogger
{
    public class FileLogger : AbstractLogger<FileLogger>
    {

        public FileLogger(string name, ILoggerConfiguration<FileLogger> conf) : base(name, conf) { }

        public override FileLoggerConfig GetConfig => (FileLoggerConfig)base.GetConfig;


        private string _getPath()
        {
            return GetConfig.LogRootPath + GetConfig.DirectoryName + Path.DirectorySeparatorChar;
        }

        private string _getFullFileName(string number)
        {
            return _getPath() + _getFileName(number);
        }


        private string _getFileName(string number)
        {
            var serial = String.IsNullOrEmpty(number) ? "" : "#" + number;
            return $"#{GetConfig.FileName}{serial}#{GetConfig.Extension}";
        }

        private int _getLatest()
        {
            return _getOrCreateLogDirectory().EnumerateFiles(_getFileName("*"))
                     .OrderByDescending((info) => info.Name)
                     .Count();        
        }

        private void _rotate()
        {
            new FileInfo(_getFullFileName("")).MoveTo(_getFullFileName((_getLatest()).ToString()));
        }

        private DirectoryInfo _getOrCreateLogDirectory()
        {
            var dir = new DirectoryInfo(_getPath());
            if (!dir.Exists)
            {
                dir.Create();
            };

            return dir;
        }


        private FileInfo _getCurrent()
        {
            var _dir = _getOrCreateLogDirectory();
            var current = new FileInfo(_getFullFileName(""));
            if (current.Exists && current.Length > GetConfig.FileRotationTreshold)
            {
                _rotate();
                current.Refresh();
            }

            return current;
        }


        private void _log(FileInfo current, ILogEntry ent) {
            using StreamWriter w = current.AppendText();
            w.WriteLine(ent.Formatter(ent, null));
        }


        private async Task<bool> _logAsync(FileInfo current, ILogEntry ent) {
            using StreamWriter w = current.AppendText();
            await w.WriteLineAsync(ent.Formatter(ent, null));
            return true;
        }

        public override void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {

            if (state is ILogEntry ent)
            {
                try
                {
                    _log(_getCurrent(), ent);
                }
                catch (IOException ex)
                {
                    Debug.WriteLine(ex);
                }

            } else if (LoggerBuilder.GetConfig.ListenToHostEvents) {

                try
                {
                    _log(_getCurrent(), GetAsLogEntry((LogLevels)logLevel, state));
                }
                catch (IOException ex)
                {
                    Debug.WriteLine(ex);
                }
            }
        }

        public async override Task<bool> WriteLogAsync(ILogEntry entry)
        {
            
                try
                {
                    return await _logAsync(_getCurrent(), entry);
                }
                catch (IOException ex)
                {
                    Debug.WriteLine(ex);
                throw ex;
                }
        }

    }
}
