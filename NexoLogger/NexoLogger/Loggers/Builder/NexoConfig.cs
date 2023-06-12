using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NexoLogger.Loggers.Builder
{
    public class NexoConfig : INexoConfig
    {
        public bool ListenToHostEvents { get; set; }
    }
}
