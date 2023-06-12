using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NexoLogger.Models
{
    public class DefaultLogEntry:ILogEntry
    {
       private readonly DateTime _logged;
       private readonly string _message;
       private readonly LogLevels _level;

       public DefaultLogEntry( LogLevels level, string message) {
            _logged = DateTime.Now;
            _level = level;
            _message = message;
        
       }

       public Func<ILogEntry, Exception?, string> Formatter => (entry, exp) =>
       {
            return $"# {(entry as DefaultLogEntry)?.Logged.ToString()} # [ {Enum.GetName(entry.Level)} ] # {entry.Message}";
       };

       public DateTime Logged => _logged;
   
       public string Message => _message;
      
       public LogLevels Level => _level;


    }
}
