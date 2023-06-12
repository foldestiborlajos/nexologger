using NexoLogger.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NexoLogger.Loggers.FileLogger
{
    public class FileLoggerConfig : ILoggerConfiguration<FileLogger> 
    {
      
        public string LogRootPath { get; set; } = Path.GetTempPath();

        public string DirectoryName { get; set; } = "nexologs";

        public string FileName { get; set; } = "logs";

        public string Extension { get; set; } = ".txt"; 

        public float FileRotationTreshold { get; set; } = 1024 * 5;

        public LogLevels MinLogLevel { get; set; } = LogLevels.Debug;

        public Func<ILoggerConfiguration<FileLogger>, bool> Filter { get; set; } = (ILoggerConfiguration<FileLogger> config) => { return true; };

    }
}
