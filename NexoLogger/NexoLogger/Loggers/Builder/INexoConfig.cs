using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NexoLogger.Loggers.Builder
{
    public interface INexoConfig
    {
        bool ListenToHostEvents { get; set; }

    }
}
