using NexoLogger.Loggers;
using System;

public class NexoLoggerFactory<T> where T : class, INexoLogger
{

    public static T Create<T>(string name, ILoggerConfiguration<T> conf) where T : AbstractLogger<T> 
    {
		return (Activator.CreateInstance(typeof(T), name, conf)) as T;
    }
}
