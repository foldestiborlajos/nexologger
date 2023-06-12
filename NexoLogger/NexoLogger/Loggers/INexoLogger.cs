using Microsoft.Extensions.Logging;
using NexoLogger.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NexoLogger.Loggers
{
    public interface INexoLogger : ILogger
    {    

        Task<bool> WriteLogAsync(ILogEntry entry);
    }
}
