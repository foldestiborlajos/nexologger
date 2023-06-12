using NexoLogger.Loggers.Builder;

using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace NexoLogger.Loggers.Builder.Hosted
{
    public static class BuilderExtensions
    {


        public static ILoggingBuilder UseNexoLogger(this ILoggingBuilder builder, Action<LoggerBuilder> nexoBuilder)
        {
            nexoBuilder.Invoke(new LoggerBuilder(builder));
            return builder;
        }
    }
}
