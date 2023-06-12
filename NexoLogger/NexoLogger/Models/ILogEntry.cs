using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NexoLogger.Models
{
    public interface ILogEntry
    {

        Func<ILogEntry, Exception?, string> Formatter { get;}
        
        string Message { get; }

        LogLevels Level { get; }
    }
}
