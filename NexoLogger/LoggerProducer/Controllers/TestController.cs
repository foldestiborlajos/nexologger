using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NexoLogger.Loggers;
using NexoLogger.Loggers.Builder.Extensions;
using NexoLogger.Loggers.StreamLogger;
using NexoLogger.Models;
using System.Diagnostics;

namespace LoggerProducer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
         private ILogger _logger;
         private ILogger _logger2;
         private Stream _test;

        public TestController(ILogger<TestController> logger) {
            _test = new MemoryStream();
            _logger= logger.Configure<TestController, StreamLogger>((config) => {
                (config as StreamLoggerConfig).Target = _test;
            });
        }

        [HttpPost]
        [Produces("application/json")]
        public JsonResult CreateRequest([FromBody] string message)
        {
            using (var stream = new MemoryStream())
            {
                _logger.Configure<TestController, StreamLogger>((config) =>
                {
                    (config as StreamLoggerConfig).Target = stream;
                });
                _logger.WriteToLog(LogLevels.Error, "hello");
                _logger.WriteToLogInfo(message + " # " + "es");

                var streamResult = "";
                using var read = new StreamReader(stream);
                read.BaseStream.Position = 0;
                streamResult = read.ReadToEnd();
            }

            _logger.WriteLogDebugAsync<TestController>(message);
            
            return new JsonResult(message);
        }
    }


}
