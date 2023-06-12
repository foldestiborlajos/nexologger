using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NexoLogger.Loggers
{
    internal interface INexoLoggerProvider
    {
        public bool TryGetLogger<K>(out INexoLogger logger);
    }
}
